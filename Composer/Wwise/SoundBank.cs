using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Composer.IO;

namespace Composer.Wwise
{
    /// <summary>
    /// A Wwise sound bank.
    /// </summary>
    public class SoundBank
    {
        private Dictionary<uint, SoundBankFile> _filesById = new Dictionary<uint, SoundBankFile>();
        private Dictionary<uint, SoundBankEvent> _eventsById = new Dictionary<uint, SoundBankEvent>();
        private WwiseObjectCollection _objects = new WwiseObjectCollection();

        /// <summary>
        /// Loads a SoundBank from a stream.
        /// </summary>
        /// <param name="reader">The EndianReader to read from.</param>
        public SoundBank(EndianReader reader)
            : this(reader, reader.Length - reader.Position)
        {
        }

        /// <summary>
        /// Loads a SoundBank from a stream.
        /// </summary>
        /// <param name="reader">The EndianReader to read from.</param>
        /// <param name="fileSize">The size of the bank file if embedded in a stream.</param>
        public SoundBank(EndianReader reader, long fileSize)
        {
            ReadBlocks(reader, fileSize);
        }

        /// <summary>
        /// Finds a file in the sound bank by ID.
        /// </summary>
        /// <param name="id">The ID of the file to find.</param>
        /// <returns>The SoundBankFile if found, or null otherwise.</returns>
        public SoundBankFile FindFileById(uint id)
        {
            SoundBankFile result;
            if (_filesById.TryGetValue(id, out result))
                return result;
            return null;
        }

        /// <summary>
        /// Finds an event in the sound bank by ID.
        /// </summary>
        /// <param name="id">The ID of the event to find.</param>
        /// <returns>The SoundBankEvent if found, or null otherwise.</returns>
        public SoundBankEvent FindEventById(uint id)
        {
            SoundBankEvent result;
            if (_eventsById.TryGetValue(id, out result))
                return result;
            return null;
        }

        /// <summary>
        /// The sound bank's ID.
        /// </summary>
        public uint ID { get; private set; }

        /// <summary>
        /// The offset of the data area from the start of the sound bank.
        /// Add this to a SoundBankFile's Offset to get its offset within the bank.
        /// </summary>
        public int DataOffset { get; private set; }

        /// <summary>
        /// The files in the sound bank, not stored in any particular order.
        /// </summary>
        public IEnumerable<SoundBankFile> Files
        {
            get { return _filesById.Values; }
        }

        /// <summary>
        /// The events in the sound bank, not stored in any particular order.
        /// </summary>
        public IEnumerable<SoundBankEvent> Events
        {
            get { return _eventsById.Values; }
        }

        /// <summary>
        /// The Wwise objects stored in the sound bank.
        /// </summary>
        public WwiseObjectCollection Objects
        {
            get { return _objects; }
        }

        private void ReadBlocks(EndianReader reader, long fileSize)
        {
            Endian defaultEndian = reader.Endianness;

            long startOffset = reader.Position;
            long endOffset = startOffset + fileSize;
            long offset = startOffset;
            while (offset < endOffset)
            {
                // Read the block header
                // The magic value is *always* big-endian, so switch to it temporarily
                reader.Endianness = Endian.BigEndian;
                int blockMagic = reader.ReadInt32();
                reader.Endianness = defaultEndian;

                int blockSize = reader.ReadInt32();
                offset += 8;

                // Process the block based upon its magic value
                switch (blockMagic)
                {
                    case BlockMagic.BKHD:
                        ReadHeader(reader);
                        break;

                    case BlockMagic.DIDX:
                        ReadFiles(reader, blockSize);
                        break;

                    case BlockMagic.DATA:
                        // Just store the offset of this block's contents to the DataOffset field
                        DataOffset = (int)(offset - startOffset);
                        break;

                    case BlockMagic.HIRC:
                        ReadObjects(reader);
                        break;
                }

                // Skip to the next block
                offset += blockSize;
                reader.SeekTo(offset);
            }
        }

        private void ReadHeader(EndianReader reader)
        {
            reader.Skip(4);
            ID = reader.ReadUInt32();
        }

        private void ReadFiles(EndianReader reader, long blockSize)
        {
            long offset = reader.Position;
            long endOffset = offset + blockSize;
            while (offset < endOffset)
            {
                // TODO: should we just read the file values here instead of in the SoundBankFile constructor?
                SoundBankFile file = new SoundBankFile(this, reader);
                offset += 0xC; // Each file entry is 0xC bytes long

                _filesById[file.ID] = file;
                _objects.Add(file);
            }
        }

        private void ReadObjects(EndianReader reader)
        {
            int numObjects = reader.ReadInt32();
            long offset = reader.Position;

            for (int i = 0; i < numObjects; i++)
            {
                // Read the object's header
                ObjectType type = (ObjectType)reader.ReadSByte();
                int size = reader.ReadInt32();
                offset += 5;

                // Read the object's ID
                uint id = reader.ReadUInt32();

                // Read the rest of the object based upon its type
                IWwiseObject obj = null;
                switch (type)
                {
                    case ObjectType.Voice:
                        obj = new SoundBankVoice(reader, id);
                        break;

                    case ObjectType.Action:
                        obj = new SoundBankAction(reader, id);
                        break;

                    case ObjectType.Event:
                        SoundBankEvent ev = new SoundBankEvent(reader, id);
                        _eventsById[ev.ID] = ev;
                        obj = ev;
                        break;

                    case ObjectType.MusicPlaylistContainer:
                        obj = new SoundBankMusicPlaylist(reader, id);
                        break;
                }

                // Register the object if something was read
                if (obj != null)
                    _objects.Add(obj);

                // Skip to the next object
                offset += size;
                reader.SeekTo(offset);
            }
        }

        /// <summary>
        /// HIRC object types.
        /// </summary>
        private enum ObjectType : sbyte
        {
            Settings = 1,
            Voice,
            Action,
            Event,
            SequenceContainer,
            SwitchContainer,
            ActorMixer,
            AudioBus,
            BlendContainer,
            MusicSegment,
            MusicTrack,
            MusicSwitchContainer,
            MusicPlaylistContainer,
            Attenuation,
            DialogueEvent,
            MotionBus,
            MotionFX,
            Effect,
            AuxiliaryBus
        }

        /// <summary>
        /// Block magic numbers.
        /// </summary>
        private static class BlockMagic
        {
            public const int BKHD = 0x424B4844; // Bank Header
            public const int DIDX = 0x44494458; // Data Index
            public const int DATA = 0x44415441; // Data
            public const int ENVS = 0x454E5653; // Environments
            public const int FXPR = 0x46585052; // Effects production
            public const int HIRC = 0x48495243; // Wwise objects
            public const int STID = 0x53544944; // Bank IDs
            public const int STMG = 0x53544D47; // Project settings
        }
    }
}

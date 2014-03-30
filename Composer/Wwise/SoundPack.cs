using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Composer.IO;
using Composer.Util;

namespace Composer.Wwise
{
    /// <summary>
    /// A Wwise sound pack file.
    /// </summary>
    public class SoundPack
    {
        private int _folderListSize;
        private Dictionary<int, SoundPackFolder> _foldersById = new Dictionary<int, SoundPackFolder>(); // Maps folder IDs to their objects
        private Dictionary<uint, SoundPackFile> _filesById = new Dictionary<uint, SoundPackFile>();
        private WwiseObjectCollection _objects = new WwiseObjectCollection();

        private static readonly int HeaderMagic = CharConstant.FromString("AKPK");
        private const int FolderListStartOffset = 0x1C;

        /// <summary>
        /// Loads a SoundPack from a stream.
        /// </summary>
        /// <param name="reader">The EndianReader to read from.</param>
        public SoundPack(EndianReader reader)
        {
            ReadHeader(reader);
            ReadFolderTable(reader);
            ReadFiles(reader);
        }

        /// <summary>
        /// Finds a folder in the pack by ID.
        /// </summary>
        /// <param name="id">The folder's ID number.</param>
        /// <returns>The SoundPackFolder if found, or null otherwise.</returns>
        public SoundPackFolder FindFolderByID(int id)
        {
            SoundPackFolder result;
            if (_foldersById.TryGetValue(id, out result))
                return result;
            return null;
        }

        /// <summary>
        /// Finds a file in the pack by ID.
        /// </summary>
        /// <param name="id">The ID of the file to find.</param>
        /// <returns>The SoundPackFile if found, or null otherwise.</returns>
        public SoundPackFile FindFileByID(uint id)
        {
            SoundPackFile result;
            if (_filesById.TryGetValue(id, out result))
                return result;
            return null;
        }

        /// <summary>
        /// The folders in the sound pack, not stored in any particular order.
        /// </summary>
        public IEnumerable<SoundPackFolder> Folders
        {
            get { return _foldersById.Values; }
        }

        /// <summary>
        /// The Wwise objects stored in the sound pack.
        /// </summary>
        public WwiseObjectCollection Objects
        {
            get { return _objects; }
        }

        private void ReadHeader(EndianReader reader)
        {
            reader.Endianness = Endian.BigEndian;
            reader.SeekTo(0);

            // Validate the magic number at the beginning
            if (reader.ReadUInt32() != HeaderMagic)
                throw new InvalidOperationException("Invalid sound pack magic");

            reader.Skip(4 + 4); // header size
                                // unknown

            // Read the size of the folder list (needed to find the start of the file table)
            _folderListSize = reader.ReadInt32();

            reader.Skip(4 + 4 + 4); // int32 bank file table size
                                    // int32 sound file table size
                                    // int32 unknown
        }

        private void ReadFolderTable(EndianReader reader)
        {
            reader.Endianness = Endian.BigEndian;

            // Read the number of folders
            int folderCount = reader.ReadInt32();

            // Sort the folders into a list sorted by offset
            SortedList<int, int> folderOffsets = new SortedList<int, int>(); // Maps offset -> ID, sorted by offset
            for (int i = 0; i < folderCount; i++)
            {
                int offset = reader.ReadInt32();
                int id = reader.ReadInt32();
                folderOffsets.Add(offset, id);
            }

            // Read the folder names and create wrappers for them
            foreach (KeyValuePair<int, int> offset in folderOffsets)
            {
                reader.SeekTo(FolderListStartOffset + offset.Key); // The name's offset is relative to the start of the folder list
                string name = reader.ReadAscii();
                _foldersById[offset.Value] = new SoundPackFolder(name);
            }
        }

        private void ReadFiles(EndianReader reader)
        {
            // The file tables come after the folder list and padding
            reader.SeekTo(FolderListStartOffset + _folderListSize);
            ReadFileTable(SoundPackFileType.SoundBank, reader);
            ReadFileTable(SoundPackFileType.SoundFile, reader);
        }

        private void ReadFileTable(SoundPackFileType type, EndianReader reader)
        {
            int fileCount = reader.ReadInt32();

            // Read each file and sort it into its folder
            for (int i = 0; i < fileCount; i++)
            {
                SoundPackFile file = new SoundPackFile(type, reader);

                // Put the file into its parent folder
                SoundPackFolder folder = FindFolderByID(file.FolderIndex);
				if (folder != null)
					folder.AddFile(file);

                // Associate its ID
                _filesById[file.ID] = file;
                _objects.Add(file);
            }
        }
    }
}

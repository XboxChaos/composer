using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Composer.IO;

namespace Composer.Wwise
{
    public class RIFX
    {
        private int _totalSize;

        public RIFX(EndianReader reader)
        {
            ReadHeader(reader);
            ReadBlocks(reader);
        }

        /// <summary>
        /// The compression codec.
        /// </summary>
        public short Codec { get; private set; }

        /// <summary>
        /// The number of audio channels.
        /// </summary>
        public short ChannelCount { get; private set; } 

        /// <summary>
        /// The sample rate.
        /// </summary>
        public int SampleRate { get; private set; }

        /// <summary>
        /// The offset of the audio data from the start of the RIFX file.
        /// </summary>
        public int DataOffset { get; private set; }

        /// <summary>
        /// The size of the audio data.
        /// </summary>
        public int DataSize { get; private set; }

        private void ReadHeader(EndianReader reader)
        {
            reader.Endianness = Endian.BigEndian;

            // Check the 'RIFX' magic
            if (reader.ReadInt32() != 0x52494658) // RIFX
                throw new InvalidOperationException("Invalid RIFX header");

            // The size is in little endian, for whatever reason
            reader.Endianness = Endian.LittleEndian;
            _totalSize = reader.ReadInt32();
            reader.Endianness = Endian.BigEndian;

            // Check the 'WAVE' magic
            if (reader.ReadUInt32() != 0x57415645) // WAVE
                throw new InvalidOperationException("Invalid RIFX WAVE header");
        }

        private void ReadBlocks(EndianReader reader)
        {
            reader.Endianness = Endian.BigEndian;

            int offset = 4 * 3; // Start reading after the header
            long baseOffset = reader.Position - offset; // Start of the RIFX data

            // Read each block in the file
            while (offset < _totalSize)
            {
                // Read the block ID and size
                int blockId = reader.ReadInt32();
                int blockSize = reader.ReadInt32();
                offset += 4 * 2;

                // Handle the block
                switch (blockId)
                {
                    case 0x666D7420: // 'fmt '
                        ReadFormatBlock(reader, blockSize);
                        break;

                    case 0x64617461: // 'data'
                        // Don't read anything, just store the info
                        DataOffset = offset;
                        DataSize = blockSize;
                        break;
                }

                // Skip to the next block
                offset += blockSize;
                reader.SeekTo(offset + baseOffset);
            }
        }

        private void ReadFormatBlock(EndianReader reader, int blockSize)
        {
            if (blockSize < 8)
                throw new InvalidOperationException("Invalid fmt block size");

            Codec = reader.ReadInt16();
            ChannelCount = reader.ReadInt16();
            SampleRate = reader.ReadInt32();
        }
    }
}

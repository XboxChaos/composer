using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Composer.IO;

namespace Composer.Wwise
{
    /// <summary>
    /// Provides constants for common format magic values in RIFF/RIFX files.
    /// </summary>
    public static class RIFFFormat
    {
        public const int WAVE = 0x57415645;
        public const int XWMA = 0x58574D41;
    }

    /// <summary>
    /// Big-endian RIFF container.
    /// </summary>
    public class RIFX
    {
        /// <summary>
        /// Loads RIFX data from a stream.
        /// </summary>
        /// <param name="reader">The EndianReader to read from.</param>
        /// <param name="size">The size of the data to read.</param>
        public RIFX(EndianReader reader, int size)
        {
            ReadHeader(reader);
            ReadBlocks(reader, size);
        }

        /// <summary>
        /// The magic value (e.g. 'WAVE') near the beginning of the file which indicates the file's format.
        /// </summary>
        /// <seealso cref="RIFFFormat"/>
        public int FormatMagic { get; private set; }

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
        /// The average number of bytes to be processed per second.
        /// </summary>
        public int BytesPerSecond { get; private set; }

        /// <summary>
        /// Block alignment.
        /// </summary>
        public short BlockAlign { get; private set; }

        /// <summary>
        /// The number of bits per coded sample.
        /// </summary>
        public short BitsPerSample { get; private set; }

        /// <summary>
        /// Extra codec-specific data in the header.
        /// </summary>
        public byte[] ExtraData { get; private set; }

        /// <summary>
        /// The file's seek table. Can be null.
        /// </summary>
        public int[] SeekOffsets { get; private set; }

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

            // Skip over the size, it's endianness varies and the value itself is just plain wrong sometimes
            reader.Skip(4);

            // Read the format magic value
            FormatMagic = reader.ReadInt32();
        }

        private void ReadBlocks(EndianReader reader, int size)
        {
            reader.Endianness = Endian.BigEndian;

            int offset = 4 * 3; // Start reading after the header
            long baseOffset = reader.Position - offset; // Start of the RIFX data

            // Read each block in the file
            while (offset < size)
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

                    case 0x7365656B: // 'seek'
                        ReadSeekOffsets(reader, blockSize);
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
            BytesPerSecond = reader.ReadInt32();
            BlockAlign = reader.ReadInt16();
            BitsPerSample = reader.ReadInt16();

            short extraDataSize = reader.ReadInt16();
            ExtraData = reader.ReadBlock(extraDataSize);
        }

        private void ReadSeekOffsets(EndianReader reader, int blockSize)
        {
            int numEntries = blockSize / 4; // The block is just an array of uint32s, one for each packet size
            SeekOffsets = new int[numEntries];
            for (int i = 0; i < numEntries; i++)
                SeekOffsets[i] = reader.ReadInt32();
        }
    }
}

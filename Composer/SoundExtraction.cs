using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Composer.IO;
using Composer.Wwise;

namespace Composer
{
    /// <summary>
    /// Provides methods for extracting and converting Wwise sound files.
    /// </summary>
    public static class SoundExtraction
    {
        /// <summary>
        /// Extracts the raw contents of a sound to a file.
        /// </summary>
        /// <param name="reader">The stream to read from.</param>
        /// <param name="offset">The offset of the data to extract.</param>
        /// <param name="size">The size of the data to extract.</param>
        /// <param name="outPath">The path of the file to save to.</param>
        public static void ExtractRaw(IReader reader, int offset, int size, string outPath)
        {
            using (EndianWriter output = new EndianWriter(File.OpenWrite(outPath), Endian.BigEndian))
            {
                // Just copy the data over to the output stream
                reader.SeekTo(offset);
                StreamUtil.Copy(reader, output, size);
            }
        }

        /// <summary>
        /// Extracts an XMA sound and converts it to a WAV.
        /// </summary>
        /// <param name="reader">The stream to read from.</param>
        /// <param name="offset">The offset of the data to extract.</param>
        /// <param name="rifx">The RIFX data for the sound.</param>
        /// <param name="outPath">The path of the file to save to.</param>
        public static void ExtractXMAToWAV(IReader reader, int offset, RIFX rifx, string outPath)
        {
            // Create a temporary file to write an XMA to
            string tempFile = Path.GetTempFileName();

            try
            {
                using (EndianWriter output = new EndianWriter(File.OpenWrite(tempFile), Endian.BigEndian))
                {
                    // Generate an XMA header
                    // ADAPTED FROM wwisexmabank - I DO NOT TAKE ANY CREDIT WHATSOEVER FOR THE FOLLOWING CODE.
                    // See http://hcs64.com/vgm_ripping.html for more information
                    output.WriteInt32(0x52494646); // 'RIFF'
                    output.Endianness = Endian.LittleEndian;
                    output.WriteInt32(rifx.DataSize + 0x34);
                    output.Endianness = Endian.BigEndian;
                    output.WriteInt32(RIFFFormat.WAVE);

                    // Generate the 'fmt ' chunk
                    output.WriteInt32(0x666D7420); // 'fmt '
                    output.Endianness = Endian.LittleEndian;
                    output.WriteInt32(0x20);
                    output.WriteInt16(0x165);
                    output.WriteInt16(16);
                    output.WriteInt16(0);
                    output.WriteInt16(0);
                    output.WriteInt16(1);
                    output.WriteByte(0);
                    output.WriteByte(3);
                    output.WriteInt32(0);
                    output.WriteInt32(rifx.SampleRate);
                    output.WriteInt32(0);
                    output.WriteInt32(0);
                    output.WriteByte(0);
                    output.WriteByte((byte)rifx.ChannelCount);
                    output.WriteInt16(0x0002);

                    // 'data' chunk
                    output.Endianness = Endian.BigEndian;
                    output.WriteInt32(0x64617461); // 'data'
                    output.Endianness = Endian.LittleEndian;
                    output.WriteInt32(rifx.DataSize);

                    // Copy the data chunk contents from the original RIFX
                    reader.SeekTo(offset + rifx.DataOffset);
                    StreamUtil.Copy(reader, output, rifx.DataSize);

                    // END ADAPTED CODE
                }

                // Convert it with towav
                RunProgramSilently("Helpers/towav.exe", "\"" + Path.GetFileName(tempFile) + "\"", Path.GetDirectoryName(tempFile));

                // Move the WAV to the destination path
                File.Move(Path.ChangeExtension(tempFile, "wav"), outPath);
            }
            finally
            {
                // Delete the temporary XMA file
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

        /// <summary>
        /// Extracts an xWMA sound and converts it to WAV.
        /// </summary>
        /// <param name="reader">The stream to read from.</param>
        /// <param name="offset">The offset of the data to extract.</param>
        /// <param name="rifx">The RIFX data for the sound.</param>
        /// <param name="outPath">The path of the file to save to.</param>
        public static void ExtractXWMAToWAV(IReader reader, int offset, RIFX rifx, string outPath)
        {
            // Create a temporary file to write an XWMA to
            string tempFile = Path.GetTempFileName();

            try
            {
                using (EndianWriter output = new EndianWriter(File.OpenWrite(tempFile), Endian.BigEndian))
                {
                    // Generate a little-endian XWMA header
                    // TODO: move this into a class?
                    output.WriteInt32(0x52494646); // 'RIFF'

                    // Recompute the file size because the one Wwise gives us is trash
                    // fileSize = header size (always 0x2C) + dpds data size + data header size (always 0x8) + data size
                    int fileSize = 0x2C + rifx.SeekOffsets.Length * 0x4 + 0x8 + rifx.DataSize;
                    output.Endianness = Endian.LittleEndian;
                    output.WriteInt32(fileSize);

                    output.Endianness = Endian.BigEndian;
                    output.WriteInt32(RIFFFormat.XWMA);

                    // Generate the 'fmt ' chunk
                    output.WriteInt32(0x666D7420); // 'fmt '
                    output.Endianness = Endian.LittleEndian;
                    output.WriteInt32(0x18); // Chunk size
                    output.WriteInt16(rifx.Codec);
                    output.WriteInt16(rifx.ChannelCount);
                    output.WriteInt32(rifx.SampleRate);
                    output.WriteInt32(rifx.BytesPerSecond);
                    output.WriteInt16(rifx.BlockAlign);
                    output.WriteInt16(rifx.BitsPerSample);

                    // Write the extradata
                    // Bytes 4 and 5 have to be flipped because they make up an int16
                    // TODO: add error checking to make sure the extradata is the correct size (0x6)
                    output.WriteInt16(0x6);
                    output.WriteBlock(rifx.ExtraData, 0, 4);
                    output.WriteByte(rifx.ExtraData[5]);
                    output.WriteByte(rifx.ExtraData[4]);

                    // Generate the 'dpds' chunk
                    // It's really just the 'seek' chunk from the original data but renamed
                    output.Endianness = Endian.BigEndian;
                    output.WriteInt32(0x64706473); // 'dpds'

                    output.Endianness = Endian.LittleEndian;
                    output.WriteInt32(rifx.SeekOffsets.Length * 4); // One uint32 per offset
                    foreach (int seek in rifx.SeekOffsets)
                        output.WriteInt32(seek);

                    // 'data' chunk
                    output.Endianness = Endian.BigEndian;
                    output.WriteInt32(0x64617461); // 'data'
                    output.Endianness = Endian.LittleEndian;
                    output.WriteInt32(rifx.DataSize);

                    // Copy the data chunk contents from the original RIFX
                    reader.SeekTo(offset + rifx.DataOffset);
                    StreamUtil.Copy(reader, output, rifx.DataSize);
                }

                // Convert it with xWMAEncode
                RunProgramSilently("Helpers/xWMAEncode.exe", "\"" + tempFile + "\" \"" + outPath + "\"", Directory.GetCurrentDirectory());
            }
            finally
            {
                // Delete the temporary XWMA file
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

        /// <summary>
        /// Extracts a Wwise OGG and converts it to a "regular" OGG file.
        /// </summary>
        /// <param name="reader">The stream to read from.</param>
        /// <param name="offset">The offset of the data to extract.</param>
        /// <param name="size">The size of the data to extract.</param>
        /// <param name="outPath">The path of the file to save to.</param>
        public static void ExtractWwiseToOGG(IReader reader, int offset, int size, string outPath)
        {
            // Just extract the RIFX to a temporary file
            string tempFile = Path.GetTempFileName();

            try
            {
                using (EndianWriter output = new EndianWriter(File.OpenWrite(tempFile), Endian.BigEndian))
                {
                    reader.SeekTo(offset);
                    StreamUtil.Copy(reader, output, size);
                }

                // Run ww2ogg to convert the resulting RIFX to an OGG
                RunProgramSilently("Helpers/ww2ogg.exe",
                    string.Format("\"{0}\" -o \"{1}\" --pcb Helpers/packed_codebooks_aoTuV_603.bin", tempFile, outPath),
                    Directory.GetCurrentDirectory());

                // Run revorb to fix up the OGG
                RunProgramSilently("Helpers/revorb.exe", "\"" + outPath + "\"", Directory.GetCurrentDirectory());
            }
            finally
            {
                // Delete the old RIFX file
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

        /// <summary>
        /// Compresses a WAV file.
        /// </summary>
        /// <param name="wavPath">The path of the WAV file to compress.</param>
        /// <param name="outPath">The path of the file to save to.</param>
        public static void CompressWAV(string wavPath, string outPath)
        {
            RunProgramSilently("Helpers/ffmpeg.exe",
                string.Format("-y -v quiet -i \"{0}\" \"{1}\"", wavPath, outPath),
                Directory.GetCurrentDirectory());
        }

        /// <summary>
        /// Silently executes a program and waits for it to finish.
        /// </summary>
        /// <param name="path">The path to the program to execute.</param>
        /// <param name="arguments">Command-line arguments to pass to the program.</param>
        /// <param name="workingDirectory">The working directory to run in the program in.</param>
        private static void RunProgramSilently(string path, string arguments, string workingDirectory)
        {
            ProcessStartInfo info = new ProcessStartInfo(path, arguments);
            info.CreateNoWindow = true;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.UseShellExecute = false;
            info.WorkingDirectory = workingDirectory;

            Process proc = Process.Start(info);
            proc.WaitForExit();

            string output = proc.StandardError.ReadToEnd();
        }
    }
}

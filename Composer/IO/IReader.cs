using System;

namespace Composer.IO
{
    public interface IReader
    {
        void Close();
        bool EOF { get; }
        long Length { get; }
        long Position { get; }
        string ReadAscii();
        string ReadAscii(int size);
        int ReadBlock(byte[] output, int offset, int size);
        byte[] ReadBlock(int size);
        byte ReadByte();
        float ReadFloat();
        short ReadInt16();
        int ReadInt32();
        long ReadInt64();
        sbyte ReadSByte();
        ushort ReadUInt16();
        uint ReadUInt32();
        ulong ReadUInt64();
        string ReadUTF8();
        string ReadUTF8(int size);
        string ReadUTF16();
        string ReadUTF16(int length);
        bool SeekTo(long offset);
        void Skip(long count);
    }
}      

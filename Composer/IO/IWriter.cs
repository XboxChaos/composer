using System;

namespace Composer.IO
{
    public interface IWriter
    {
        void Close();
        bool EOF { get; }
        long Length { get; }
        long Position { get; }
        bool SeekTo(long offset);
        void Skip(long count);
        void WriteAscii(string str);
        void WriteBlock(byte[] data);
        void WriteBlock(byte[] data, int offset, int size);
        void WriteByte(byte value);
        void WriteFloat(float value);
        void WriteInt16(short value);
        void WriteInt32(int value);
        void WriteInt64(long value);
        void WriteSByte(sbyte value);
        void WriteUInt16(ushort value);
        void WriteUInt32(uint value);
        void WriteUInt64(ulong value);
        void WriteUTF16(string str);
    }
}      

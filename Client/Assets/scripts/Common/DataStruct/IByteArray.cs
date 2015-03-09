using System;
using System.Collections.Generic;
using System.Text;

namespace SDK.Common
{
    /**
     *@brief IByteArray 功能
     */
    public interface IByteArray
    {
        void clear();
        uint compress(uint len_ = 0, CompressionAlgorithm algorithm = CompressionAlgorithm.LZMA);
        uint uncompress(uint len_ = 0, CompressionAlgorithm algorithm = CompressionAlgorithm.LZMA);
		bool readBoolean();
		byte readByte();
        byte readUnsignedByte();
		short readShort();
        ushort readUnsignedShort();
        int readInt();
		uint readUnsignedInt();
        ulong readUnsignedLong();
        string readMultiByte(uint length, Encoding charSet);
        byte[] readBytes(uint len);

        float readFloat();
        double readDouble();

		void writeByte(byte value);
        void writeShort(short value);
        void writeUnsignedShort(ushort value);
		void writeInt(int value);
        void writeUnsignedInt(uint value);
        void writeUnsignedLong(ulong value);
        void writeMultiByte(string value, Encoding charSet, int len);
        void writeBytes(byte[] value, uint start, uint length);

        void setPos(uint pos);
        uint getPos();
        void setEndian(Endian end);
    }
}
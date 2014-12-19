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
        void compress(CompressionAlgorithm algorithm = CompressionAlgorithm.ZLIB);
        void uncompress();
		bool readBoolean();
		byte readByte();
        byte readUnsignedByte();
		short readShort();
        ushort readUnsignedShort();
        int readInt();
		uint readUnsignedInt();
        ulong readUnsignedLong();
        string readMultiByte(uint length, Encoding charSet);

		void writeByte(byte value);
        void writeShort(short value);
        void writeUnsignedShort(ushort value);
		void writeInt(int value);
        void writeUnsignedInt(uint value);
        void writeUnsignedLong(ulong value);
        void writeMultiByte(string value, Encoding charSet, int len);
        void writeBytes(byte[] value, uint start, uint length);

        void setPos(uint pos);
    }
}
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
		int readInt();
		string readMultiByte (uint length, Encoding charSet);
		int readShort();
		uint readUnsignedByte();
		uint readUnsignedInt();
		ushort readUnsignedShort();
        ulong readUnsignedLong();

		void writeByte(byte value);
		void writeInt(int value);
        void writeMultiByte(string value, Encoding charSet, int len);
		void writeShort(short value);
        void writeUnsignedShort(ushort value);
		void writeUnsignedInt(uint value);
        void writeBytes(byte[] value, uint start, uint length);
        void writeUnsignedLong(ulong value);

        void setPos(uint pos);
    }
}
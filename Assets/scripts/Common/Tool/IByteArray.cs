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
        void uncompress ();
		bool readBoolean ();
		int readByte ();
		int readInt ();
		string readMultiByte (uint length, Encoding charSet);
		int readShort ();
		uint readUnsignedByte ();
		uint readUnsignedInt ();
		uint readUnsignedShort ();
		void writeByte (byte value);
		void writeInt (int value);
		void writeMultiByte (string value, Encoding charSet);
		void writeShort (short value);
		void writeUnsignedInt (uint value);
        void writeBytes(byte[] value, uint start, uint length);
    }
}
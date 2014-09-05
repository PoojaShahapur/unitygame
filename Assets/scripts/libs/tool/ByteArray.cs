using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace San.Guo
{
    /**
     *@brief 类似 flash 的 ByteArray 功能
     */
    class ByteArray
    {
        protected byte[] m_buff;            // 当前的缓冲区
		public function get bytesAvailable () : uint;

		
		public static function get defaultObjectEncoding () : uint;
		public static function set defaultObjectEncoding (version:uint) : void;

	
		public function get endian () : String;
		public function set endian (type:String) : void;

		public function get length () : uint;
		public function set length (value:uint) : void;

		public function get objectEncoding () : uint;
		public function set objectEncoding (version:uint) : void;

		public function get position () : uint;
		public function set position (offset:uint) : void;

		public function get shareable () : Boolean;
		public function set shareable (newValue:Boolean) : void;

		public function atomicCompareAndSwapIntAt (byteIndex:int, expectedValue:int, newValue:int) : int;

		public function atomicCompareAndSwapLength (expectedLength:int, newLength:int) : int;

		public function ByteArray ();


		public function clear () : void;


		public function compress (algorithm:String="zlib") : void;

		public function deflate () : void;


		public function inflate () : void;

		public function readBoolean () : Boolean;


		public function readByte () : int;


		public function readBytes (bytes:ByteArray, offset:uint=0, length:uint=0) : void;

		public function readDouble () : Number;

		public function readFloat () : Number;

		public function readInt () : int;

		public function readMultiByte (length:uint, charSet:String) : String;

		public function readObject () : *;


		public function readShort () : int;

		/**
		 * Reads an unsigned byte from the byte stream.
		 * 
		 *   The returned value is in the range 0 to 255.
		 * @return	A 32-bit unsigned integer between 0 and 255.
		 * @langversion	3.0
		 * @playerversion	Flash 9
		 * @playerversion	Lite 4
		 * @throws	EOFError There is not sufficient data available
		 *   to read.
		 */
		public function readUnsignedByte () : uint;

		/**
		 * Reads an unsigned 32-bit integer from the byte stream.
		 * 
		 *   The returned value is in the range 0 to 4294967295.
		 * @return	A 32-bit unsigned integer between 0 and 4294967295.
		 * @langversion	3.0
		 * @playerversion	Flash 9
		 * @playerversion	Lite 4
		 * @throws	EOFError There is not sufficient data available
		 *   to read.
		 */
		public function readUnsignedInt () : uint;

		/**
		 * Reads an unsigned 16-bit integer from the byte stream.
		 * 
		 *   The returned value is in the range 0 to 65535.
		 * @return	A 16-bit unsigned integer between 0 and 65535.
		 * @langversion	3.0
		 * @playerversion	Flash 9
		 * @playerversion	Lite 4
		 * @throws	EOFError There is not sufficient data available
		 *   to read.
		 */
		public function readUnsignedShort () : uint;

		/**
		 * Reads a UTF-8 string from the byte stream.  The string
		 * is assumed to be prefixed with an unsigned short indicating
		 * the length in bytes.
		 * @return	UTF-8 encoded  string.
		 * @langversion	3.0
		 * @playerversion	Flash 9
		 * @playerversion	Lite 4
		 * @throws	EOFError There is not sufficient data available
		 *   to read.
		 */
		public function readUTF () : String;

		/**
		 * Reads a sequence of UTF-8 bytes specified by the length
		 * parameter from the byte stream and returns a string.
		 * @param	length	An unsigned short indicating the length of the UTF-8 bytes.
		 * @return	A string composed of the UTF-8 bytes of the specified length.
		 * @langversion	3.0
		 * @playerversion	Flash 9
		 * @playerversion	Lite 4
		 * @throws	EOFError There is not sufficient data available
		 *   to read.
		 */
		public function readUTFBytes (length:uint) : String;

		/**
		 * Converts the byte array to a string.
		 * If the data in the array begins with a Unicode byte order mark, the application will honor that mark
		 * when converting to a string. If System.useCodePage is set to true, the
		 * application will treat the data in the array as being in the current system code page when converting.
		 * @return	The string representation of the byte array.
		 * @langversion	3.0
		 * @playerversion	Flash 9
		 * @playerversion	Lite 4
		 */
		public function toString () : String;

		/**
		 * Decompresses the byte array. 
		 * After the call, the length property of the ByteArray is set to the new length.
		 * The position property is set to 0.
		 * 
		 *   The byte array must have been compressed using the same algorithm as the uncompress. 
		 * You specify an uncompression algorithm by passing a
		 * value (defined in the CompressionAlgorithm class) as the algorithm
		 * parameter. The supported algorithms include the following: The zlib compressed data format is described at
		 * http://www.ietf.org/rfc/rfc1950.txt.The deflate compression algorithm is described at
		 * http://www.ietf.org/rfc/rfc1951.txt.The lzma compression algorithm is described at
		 * http://www.7-zip.org/7z.html.In order to decode data compressed in a format that uses the deflate compression algorithm,
		 * such as data in gzip or zip format, it will not work to call
		 * uncompress(CompressionAlgorithm.DEFLATE) on
		 * a ByteArray containing the compression formation data. First, you must separate the metadata that is
		 * included as part of the compressed data format from the actual compressed data. For more
		 * information, see the compress() method description.
		 * @param	algorithm	The compression algorithm to use when decompressing. This must be the
		 *   same compression algorithm used to compress the data. Valid values are defined as
		 *   constants in the CompressionAlgorithm class. The default is to use zlib format. 
		 *   Support for the lzma algorithm was added for 
		 *   Flash Player 11.3 and AIR 3.3. You must have those player versions, or later, to use lzma.
		 * @langversion	3.0
		 * @playerversion	Flash 9
		 * @playerversion	Lite 4
		 * @throws	IOError The data is not valid compressed data; it was not compressed with the
		 *   same compression algorithm used to compress.
		 */
		public function uncompress (algorithm:String="zlib") : void;

		/**
		 * Writes a Boolean value. A single byte is written according to the value parameter,
		 * either 1 if true or 0 if false.
		 * @param	value	A Boolean value determining which byte is written. If the parameter is true,
		 *   the method writes a 1; if false, the method writes a 0.
		 * @langversion	3.0
		 * @playerversion	Flash 9
		 * @playerversion	Lite 4
		 */
		public function writeBoolean (value:Boolean) : void;

		/**
		 * Writes a byte to the byte stream.
		 * The low 8 bits of the
		 * parameter are used. The high 24 bits are ignored.
		 * @param	value	A 32-bit integer. The low 8 bits are written to the byte stream.
		 * @langversion	3.0
		 * @playerversion	Flash 9
		 * @playerversion	Lite 4
		 */
		public function writeByte (value:int) : void;

		/**
		 * Writes a sequence of length bytes from the
		 * specified byte array, bytes,
		 * starting offset(zero-based index) bytes
		 * into the byte stream.
		 * 
		 *   If the length parameter is omitted, the default
		 * length of 0 is used; the method writes the entire buffer starting at
		 * offset.
		 * If the offset parameter is also omitted, the entire buffer is
		 * written. If offset or length
		 * is out of range, they are clamped to the beginning and end
		 * of the bytes array.
		 * @param	bytes	The ByteArray object.
		 * @param	offset	A zero-based index indicating the position into the array to begin writing.
		 * @param	length	An unsigned integer indicating how far into the buffer to write.
		 * @langversion	3.0
		 * @playerversion	Flash 9
		 * @playerversion	Lite 4
		 */
		public function writeBytes (bytes:ByteArray, offset:uint=0, length:uint=0) : void;

		/**
		 * Writes an IEEE 754 double-precision (64-bit) floating-point number to the byte stream.
		 * @param	value	A double-precision (64-bit) floating-point number.
		 * @langversion	3.0
		 * @playerversion	Flash 9
		 * @playerversion	Lite 4
		 */
		public function writeDouble (value:Number) : void;

		/**
		 * Writes an IEEE 754 single-precision (32-bit) floating-point number to the byte stream.
		 * @param	value	A single-precision (32-bit) floating-point number.
		 * @langversion	3.0
		 * @playerversion	Flash 9
		 * @playerversion	Lite 4
		 */
		public function writeFloat (value:Number) : void;

		/**
		 * Writes a 32-bit signed integer to the byte stream.
		 * @param	value	An integer to write to the byte stream.
		 * @langversion	3.0
		 * @playerversion	Flash 9
		 * @playerversion	Lite 4
		 */
		public function writeInt (value:int) : void;

		/**
		 * Writes a multibyte string to the byte stream using the specified character set.
		 * @param	value	The string value to be written.
		 * @param	charSet	The string denoting the character set to use. Possible character set strings
		 *   include "shift-jis", "cn-gb", "iso-8859-1", and others.
		 *   For a complete list, see Supported Character Sets.
		 * @langversion	3.0
		 * @playerversion	Flash 9
		 * @playerversion	Lite 4
		 */
		public function writeMultiByte (value:String, charSet:String) : void;

		/**
		 * Writes an object into the byte array in AMF
		 * serialized format.
		 * @param	object	The object to serialize.
		 * @langversion	3.0
		 * @playerversion	Flash 9
		 * @playerversion	Lite 4
		 */
		public function writeObject (object:any) : void;

		/**
		 * Writes a 16-bit integer to the byte stream. The low 16 bits of the parameter are used.
		 * The high 16 bits are ignored.
		 * @param	value	32-bit integer, whose low 16 bits are written to the byte stream.
		 * @langversion	3.0
		 * @playerversion	Flash 9
		 * @playerversion	Lite 4
		 */
		public function writeShort (value:int) : void;

		/**
		 * Writes a 32-bit unsigned integer to the byte stream.
		 * @param	value	An unsigned integer to write to the byte stream.
		 * @langversion	3.0
		 * @playerversion	Flash 9
		 * @playerversion	Lite 4
		 */
		public function writeUnsignedInt (value:uint) : void;

		/**
		 * Writes a UTF-8 string to the byte stream. The length of the UTF-8 string in bytes
		 * is written first, as a 16-bit integer, followed by the bytes representing the
		 * characters of the string.
		 * @param	value	The string value to be written.
		 * @langversion	3.0
		 * @playerversion	Flash 9
		 * @playerversion	Lite 4
		 * @throws	RangeError If the length is larger than
		 *   65535.
		 */
		public function writeUTF (value:String) : void;

		/**
		 * Writes a UTF-8 string to the byte stream. Similar to the writeUTF() method,
		 * but writeUTFBytes() does not prefix the string with a 16-bit length word.
		 * @param	value	The string value to be written.
		 * @langversion	3.0
		 * @playerversion	Flash 9
		 * @playerversion	Lite 4
		 */
		public function writeUTFBytes (value:String) : void;
    }
}

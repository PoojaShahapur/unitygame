using System;
using System.IO;

namespace SDK.Lib
{
	/**
	 * @brief Lzma 算法
	 */
	public class MLzma
	{
		public const uint LZMA_HEADER_LEN = 13;

		public MLzma ()
		{

		}

		public static void CompressFileLZMA (string inFile, string outFile)
		{
			SevenZip.Compression.LZMA.Encoder coder = new SevenZip.Compression.LZMA.Encoder ();
			FileStream input = new FileStream (inFile, FileMode.Open);
			FileStream output = new FileStream (outFile, FileMode.Create);

			// Write the encoder properties
			coder.WriteCoderProperties (output);
			// Write the decompressed file size.
			output.Write (BitConverter.GetBytes (input.Length), 0, 8);

			// Encode the file.
			coder.Code (input, output, input.Length, -1, null);
			output.Flush ();
			output.Close ();
			input.Close ();
		}

		public static void DecompressFileLZMA (string inFile, string outFile)
		{
			SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder ();
			FileStream input = new FileStream (inFile, FileMode.Open);
			FileStream output = new FileStream (outFile, FileMode.Create);

			// Read the decoder properties
			byte[ ] properties = new byte [ 5 ];
			input.Read (properties, 0, 5);

			// Read in the decompress file size.
			byte[ ] fileLengthBytes = new byte [ 8 ];
			input.Read (fileLengthBytes, 0, 8);
			long fileLength = BitConverter.ToInt64 (fileLengthBytes, 0);

			// Decompress the file.
			coder.SetDecoderProperties (properties);
			coder.Code (input, output, input.Length, fileLength, null);
			output.Flush ();
			output.Close ();
			input.Close ();
		}

		public static uint CompressStrLZMA (byte[] inBytes, uint inLen, ref byte[] outBytes, ref uint outLen)
		{
			SevenZip.Compression.LZMA.Encoder coder = new SevenZip.Compression.LZMA.Encoder ();
			MemoryStream inStream = new MemoryStream (inBytes);

			int saveinsize = (int)inLen;
			int saveoutsize = (int)(saveinsize * 1.1 + 1026 * 16 + LZMA_HEADER_LEN);

			outBytes = new byte[saveoutsize];
			MemoryStream outStream = new MemoryStream (outBytes);

			// Write the encoder properties
			coder.WriteCoderProperties (outStream);
			// Write the decompressed file size.
			outStream.Write (BitConverter.GetBytes (inStream.Length), 0, 8);

			// Encode the file.
			coder.Code (inStream, outStream, inStream.Length, saveoutsize, null);
			outStream.Flush ();
			outStream.Close ();
			inStream.Close ();

			return (uint)saveoutsize;
		}

		public static void DecompressStrLZMA (byte[] inBytes, uint inLen, ref byte[] outBytes, ref uint outLen)
		{
			SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder ();
			MemoryStream inStream = new MemoryStream(inBytes);

			uint saveinsize = inLen;
			uint saveoutsize = (uint)(saveinsize * 1.1 + 1026 * 16);
			outBytes = new byte[saveoutsize];
			MemoryStream outStream = new MemoryStream (outBytes);

			// Read the decoder properties
			byte[ ] properties = new byte [ 5 ];
			inStream.Read (properties, 0, 5);

			// Read in the decompress file size.
			byte[ ] fileLengthBytes = new byte [ 8 ];
			inStream.Read (fileLengthBytes, 0, 8);				// 仅仅是读取出来就行了，这个目前用不着
			long fileLength = BitConverter.ToInt64 (fileLengthBytes, 0);

			// Decompress the file.
			coder.SetDecoderProperties (properties);
			coder.Code (inStream, outStream, inStream.Length, fileLength, null);		// 输入长度是 inStream.Length ，不是 inStream.Length - LZMA_HEADER_LEN, outSize 要填未压缩后的字符串的长度，inSize，outSize 这两个参数都要填写，否则会报错
			outStream.Flush ();
			outStream.Close ();
			inStream.Close ();

			outLen = (uint)fileLength;		// 返回解压后的长度，就是压缩的时候保存的数据长度
		}
	}
}
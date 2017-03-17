using System;
using System.IO;
using ComponentAce.Compression.Libs.zlib;

#if ENABLE_SHARP_ZIP_LIB
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;
#endif

namespace SDK.Lib
{
    /// <summary>
    /// Summary description for ICSharp
    /// </summary>
    public class Compress
    {
        // 压缩
        public static void CompressData(byte[] inBytes, uint startPos, uint inLen, ref byte[] outBytes, ref uint outLen, CompressionAlgorithm algorithm = CompressionAlgorithm.ZLIB)
        {
            if(CompressionAlgorithm.ZLIB == algorithm)
            {
                CompressByteZipNet(inBytes, startPos, inLen, ref outBytes, ref outLen);
            }
            else
            {
                CompressStrLZMA(inBytes, startPos, inLen, ref outBytes, ref outLen);
            }
        }

        // 解压缩
        public static void DecompressData(byte[] inBytes, uint startPos, uint inLen, ref byte[] outBytes, ref uint outLen, CompressionAlgorithm algorithm = CompressionAlgorithm.ZLIB)
        {
            if (CompressionAlgorithm.ZLIB == algorithm)
            {
                DecompressByteZipNet(inBytes, startPos, inLen, ref outBytes, ref outLen);
            }
            else
            {
                DecompressStrLZMA(inBytes, startPos, inLen, ref outBytes, ref outLen);
            }
        }

#if DEPRECATE_CODE
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        static public string CompressStrZip(string param)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(param);
            //byte[] data = Convert.FromBase64String(param);
            MemoryStream ms = new MemoryStream();
            Stream stream = new ICSharpCode.SharpZipLib.Zip.ZipInputStream(ms);
            try
            {
                stream.Write(data, 0, data.Length);
            }
            finally
            {
                stream.Close();
                ms.Close();
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        static public string DecompressStrZip(string param)
        {
            string commonString = "";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(param);
            //byte[] buffer=Convert.FromBase64String(param);
            MemoryStream ms = new MemoryStream(buffer);
            Stream sm = new ICSharpCode.SharpZipLib.Zip.ZipInputStream(ms);
            //这里要指明要读入的格式，要不就有乱码
            StreamReader reader = new StreamReader(sm, System.Text.Encoding.UTF8);
            try
            {
                commonString = reader.ReadToEnd();
            }
            finally
            {
                sm.Close();
                ms.Close();
            }
            return commonString;
        }

        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        static public void CompressByteZip(byte[] inBytes, uint startPos, uint inLen, ref byte[] outBytes, ref uint outLen)
        {
            MemoryStream ms = new MemoryStream();
            ZipOutputStream zipStream = new ZipOutputStream(ms);

            //zipStream.SetLevel(9); // 0 - store only to 9 - means best compression

            ZipEntry entry = new ZipEntry("dummyfile.txt"); // 输出的文件名字
            zipStream.PutNextEntry(entry);

            try
            {
                zipStream.Write(inBytes, 0, (int)inLen);

                zipStream.Flush();
                zipStream.Close();      // 一定要先 Close ZipOutputStream ，然后再获取 ToArray ，如果不关闭， ToArray 将不能返回正确的值

                outBytes = ms.ToArray();
                outLen = (uint)outBytes.Length;
            }
            finally
            {
                zipStream.Close();
                ms.Close();
            }
        }
        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        static public void DecompressByteZip(byte[] inBytes, uint startPos, uint inLen, ref byte[] outBytes, ref uint outLen)
        {
            byte[] writeData = new byte[4096];

            MemoryStream outStream = new MemoryStream();
            MemoryStream ms = new MemoryStream();
            ms.Write(inBytes, 0, (int)inLen);
            ms.Position = 0;

            ZipInputStream zipStream = new ZipInputStream(ms);
            zipStream.GetNextEntry();

            try
            {
                int size = 0;

                while ((size = zipStream.Read(writeData, 0, writeData.Length)) > 0)
                {
                    outStream.Write(writeData, 0, size);
                }

                zipStream.Flush();
                zipStream.Close();  // 一定要先 Close ZipOutputStream ，然后再获取 ToArray ，如果不关闭， ToArray 将不能返回正确的值

                outBytes = outStream.ToArray();
                outLen = (uint)outBytes.Length;

                outStream.Close();
            }
            finally
            {
                zipStream.Close();
                ms.Close();
                outStream.Close();
            }
        }

        /**
         * @brief LZMA 压缩处理
         */
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
#endif

        public const uint LZMA_HEADER_LEN = 13;

        public static void CompressStrLZMA(byte[] inBytes, uint startPos, uint inLen, ref byte[] outBytes, ref uint outLen)
        {
            SevenZip.Compression.LZMA.Encoder coder = new SevenZip.Compression.LZMA.Encoder();
            MemoryStream inStream = new MemoryStream();
            inStream.Write(inBytes, (int)startPos, (int)inLen);
            inStream.Position = 0;

            int saveinsize = (int)inLen;
            int saveoutsize = (int)(saveinsize * 1.1 + 1026 * 16 + LZMA_HEADER_LEN);

            outBytes = new byte[saveoutsize];
            MemoryStream outStream = new MemoryStream(outBytes);

            // Write the encoder properties
            coder.WriteCoderProperties(outStream);
            // Write the decompressed file size.
            outStream.Write(BitConverter.GetBytes(inStream.Length), 0, 8);

            // Encode the file.
            coder.Code(inStream, outStream, inStream.Length, -1, null);
            saveoutsize = (int)outStream.Position;
            outStream.Flush();
            outStream.Close();
            inStream.Close();

            outLen = (uint)saveoutsize;
        }

        public static void DecompressStrLZMA(byte[] inBytes, uint startPos, uint inLen, ref byte[] outBytes, ref uint outLen)
        {
            SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();
            MemoryStream inStream = new MemoryStream();
            inStream.Write(inBytes, (int)startPos, (int)inLen);
            inStream.Position = 0;      // 放到 0 位置

            uint saveinsize = inLen;
            uint saveoutsize = (uint)(saveinsize * 1.1 + 1026 * 16);
            outBytes = new byte[saveoutsize];
            MemoryStream outStream = new MemoryStream(outBytes);

            // Read the decoder properties
            byte[] properties = new byte[5];
            inStream.Read(properties, 0, 5);

            // Read in the decompress file size.
            byte[] fileLengthBytes = new byte[8];
            inStream.Read(fileLengthBytes, 0, 8);				// 仅仅是读取出来就行了，这个目前用不着
            long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);

            // Decompress the file.
            coder.SetDecoderProperties(properties);
            coder.Code(inStream, outStream, inStream.Length, fileLength, null);		// 输入长度是 inStream.Length ，不是 inStream.Length - LZMA_HEADER_LEN, outSize 要填未压缩后的字符串的长度，inSize，outSize 这两个参数都要填写，否则会报错
            outStream.Flush();
            outStream.Close();
            inStream.Close();

            outLen = (uint)fileLength;		// 返回解压后的长度，就是压缩的时候保存的数据长度
        }

        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        static public void CompressByteZipNet(byte[] inBytes, uint startPos, uint inLen, ref byte[] outBytes, ref uint outLen)
        {
            MemoryStream ms = new MemoryStream();
            ZOutputStream zipStream = new ZOutputStream(ms, 9);

            try
            {
                zipStream.Write(inBytes, (int)startPos, (int)inLen);

                zipStream.Flush();
                zipStream.Close();      // 一定要先 Close ZipOutputStream ，然后再获取 ToArray ，如果不关闭， ToArray 将不能返回正确的值

                outBytes = ms.ToArray();
                outLen = (uint)outBytes.Length;
                ms.Close();
            }
            catch
            {
                Ctx.mInstance.mLogSys.error("CompressByteZipNet error");
            }
        }
        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        static public void DecompressByteZipNet(byte[] inBytes, uint startPos, uint inLen, ref byte[] outBytes, ref uint outLen)
        {
            MemoryStream outStream = new MemoryStream();
            MemoryStream outms = new MemoryStream();
            outms.Write(inBytes, (int)startPos, (int)inLen);
            outms.Position = 0;
            ZInputStream outzipStream = new ZInputStream(outms);

            byte[] writeData = new byte[1024];

            try
            {
                int size = 0;

                while ((size = outzipStream.read(writeData, 0, writeData.Length)) > 0)
                {
                    if (size > 0)
                    {
                        outStream.Write(writeData, 0, size);
                    }
                    else
                    {
                    }
                }

                outzipStream.Close();  // 一定要先 Close ZipOutputStream ，然后再获取 ToArray ，如果不关闭， ToArray 将不能返回正确的值

                outBytes = outStream.ToArray();
                outLen = (uint)outBytes.Length;

                outStream.Close();
                outms.Close();
            }
            catch
            {
            }
        }
    }
}
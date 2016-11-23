using System.IO;
using System.Text;

namespace SDK.Lib
{
	public class ArchiveHeader
	{
		public const uint ARCHIVETOOL_VERSION = 101100;

		public byte[] mMagic;			// 幻数
		public byte mEndian;			// 大小端
		public uint mHeaderSize;		// 头部大小
		public uint mVersion;			// 版本
		public uint mFileCount;		// 文件总共数量

		public ArchiveHeader()
		{
			mVersion = ARCHIVETOOL_VERSION;

			mMagic = new byte[4];
			mMagic[0] = (byte)'a';
			mMagic[1] = (byte)'s';
			mMagic[2] = (byte)'d';
			mMagic[3] = (byte)'f';

			mEndian = (byte)EEndian.eLITTLE_ENDIAN;		// 0 大端 1 小端
		}

		public void clear()
		{
			mFileCount = 0;
			mHeaderSize = 0;
		}

		public bool readArchiveFileHeader(FileStream fileHandle, ByteBuffer pMByteBuffer)
		{
			pMByteBuffer.clear ();
			fileHandle.Read(pMByteBuffer.dynBuff.buff, 0, 4);
			pMByteBuffer.length = 4;
            string magic = "";
            //pMByteBuffer.readMultiByte(ref magic, 4, Encoding.UTF8);
            pMByteBuffer.readMultiByte(ref magic, 4, GkEncode.eUTF8);
            if (magic != "asdf")		// 检查 magic
			{
				return false;
			}

			pMByteBuffer.clear ();
			fileHandle.Read(pMByteBuffer.dynBuff.buff, 0, (int)calcArchiveHeaderSizeNoFileHeader() - 4);
			pMByteBuffer.length = calcArchiveHeaderSizeNoFileHeader() - 4;
			// 读取 endian 
            pMByteBuffer.readUnsignedInt8(ref mEndian);
			pMByteBuffer.setEndian((EEndian)mEndian);

			// 读取头部大小
            pMByteBuffer.readUnsignedInt32(ref mHeaderSize);

			// 读取版本
            pMByteBuffer.readUnsignedInt32(ref mVersion);
			// 读取文件数量
            pMByteBuffer.readUnsignedInt32(ref mFileCount);

			// 读取整个头
			pMByteBuffer.clear ();
			fileHandle.Read(pMByteBuffer.dynBuff.buff, 0, (int)(mHeaderSize - calcArchiveHeaderSizeNoFileHeader()));
			pMByteBuffer.length = mHeaderSize - calcArchiveHeaderSizeNoFileHeader ();

			return true;
		}

		public uint calcArchiveHeaderSizeNoFileHeader()
		{
			// 写入 magic 
			// 写入 endian 
			// 写入头部总共大小
			// 写入版本
			// 写入文件数量
			return (uint)mMagic.Length + sizeof(byte) + sizeof(uint) + sizeof(uint) + sizeof(uint);
		}
	}
}
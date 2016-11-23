using System.IO;
using System.Text;

namespace SDK.Lib
{
	public class ArchiveHeader
	{
		public const uint ARCHIVETOOL_VERSION = 101100;

		public byte[] mMagic;			// ����
		public byte mEndian;			// ��С��
		public uint mHeaderSize;		// ͷ����С
		public uint mVersion;			// �汾
		public uint mFileCount;		// �ļ��ܹ�����

		public ArchiveHeader()
		{
			mVersion = ARCHIVETOOL_VERSION;

			mMagic = new byte[4];
			mMagic[0] = (byte)'a';
			mMagic[1] = (byte)'s';
			mMagic[2] = (byte)'d';
			mMagic[3] = (byte)'f';

			mEndian = (byte)EEndian.eLITTLE_ENDIAN;		// 0 ��� 1 С��
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
            if (magic != "asdf")		// ��� magic
			{
				return false;
			}

			pMByteBuffer.clear ();
			fileHandle.Read(pMByteBuffer.dynBuff.buff, 0, (int)calcArchiveHeaderSizeNoFileHeader() - 4);
			pMByteBuffer.length = calcArchiveHeaderSizeNoFileHeader() - 4;
			// ��ȡ endian 
            pMByteBuffer.readUnsignedInt8(ref mEndian);
			pMByteBuffer.setEndian((EEndian)mEndian);

			// ��ȡͷ����С
            pMByteBuffer.readUnsignedInt32(ref mHeaderSize);

			// ��ȡ�汾
            pMByteBuffer.readUnsignedInt32(ref mVersion);
			// ��ȡ�ļ�����
            pMByteBuffer.readUnsignedInt32(ref mFileCount);

			// ��ȡ����ͷ
			pMByteBuffer.clear ();
			fileHandle.Read(pMByteBuffer.dynBuff.buff, 0, (int)(mHeaderSize - calcArchiveHeaderSizeNoFileHeader()));
			pMByteBuffer.length = mHeaderSize - calcArchiveHeaderSizeNoFileHeader ();

			return true;
		}

		public uint calcArchiveHeaderSizeNoFileHeader()
		{
			// д�� magic 
			// д�� endian 
			// д��ͷ���ܹ���С
			// д��汾
			// д���ļ�����
			return (uint)mMagic.Length + sizeof(byte) + sizeof(uint) + sizeof(uint) + sizeof(uint);
		}
	}
}
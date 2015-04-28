using System.Text;
using System.IO;
using System;
using SDK.Common;

namespace SDK.Lib
{
	public class FileHeader
	{
		protected byte m_pathLen;					// Ŀ¼���ȣ������� '\0'
		protected string m_pFullPath;
		protected string m_fileNamePath;			// �ļ�·������
		protected uint m_fileOffset;				// �ļ������� Archive �е�ƫ��
		protected uint m_fileSize;					// �ļ���С
		protected uint m_flags;						// ��ʶ�ֶ�

		public FileHeader()
		{
			
		}

        public string pFullPath
        {
            get
            {
                return m_pFullPath;
            }
            set
            {
                m_pFullPath = value;
            }
        }

        public uint fileSize
        {
            get
            {
                return m_fileSize;
            }
            set
            {
                m_fileSize = value;
            }
        }

		public void readHeaderFromArchiveFile(ByteBuffer ba)
		{
            ba.readUnsignedInt8(ref m_pathLen);
            ba.readMultiByte(ref m_fileNamePath, m_pathLen, Encoding.UTF8);
            ba.readUnsignedInt32(ref m_fileOffset);
            ba.readUnsignedInt32(ref m_fileSize);
            ba.readUnsignedInt32(ref m_flags);
		}

        public void readArchiveFile2Bytes(FileStream fileHandle, ref byte[] bytes)
		{
			fileHandle.Seek(m_fileOffset, SeekOrigin.Begin);	// �ƶ����ļ���ʼλ��

            uint readlength = (uint)fileHandle.Read(bytes, 0, (int)m_fileSize);
			if (readlength == m_fileSize)
			{
                if (!UtilPak.checkFlags(FileHeaderFlag.eFHF_CPS, ref m_flags))
				{
					
				}
				else	// ��Ҫ��ѹ
				{
					byte[] retChar = null;
                    MLzma.DecompressStrLZMA(bytes, m_fileSize, ref retChar, ref m_fileSize);
                    bytes = retChar;
				}
			}
		}
	}
}
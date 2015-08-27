using System.Text;
using System.IO;
using System;

namespace SDK.Lib
{
	public class FileHeader
	{
		protected byte m_pathLen;					// 目录长度，不包括 '\0'
		protected string m_pFullPath;               // 这个字段暂时没有使用
		protected string m_fileNamePath;			// 文件路径名字
		protected uint m_fileOffset;				// 文件在整个 Archive 中的偏移
		protected uint m_fileSize;					// 文件大小
		protected uint m_flags;						// 标识字段

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

        public string fileNamePath
        {
            get
            {
                return m_fileNamePath;
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
			fileHandle.Seek(m_fileOffset, SeekOrigin.Begin);	// 移动到文件开始位置

            uint readlength = (uint)fileHandle.Read(bytes, 0, (int)m_fileSize);
			if (readlength == m_fileSize)
			{
                if (!UtilPak.checkFlags(FileHeaderFlag.eFHF_CPS, ref m_flags))
				{
					
				}
				else	// 需要解压
				{
					byte[] retChar = null;
                    MLzma.DecompressStrLZMA(bytes, m_fileSize, ref retChar, ref m_fileSize);
                    bytes = retChar;
				}
			}
		}

        // 当前文件是否被压缩
        public bool bCompress()
        {
            return UtilPak.checkFlags(FileHeaderFlag.eFHF_CPS, ref m_flags);
        }
	}
}
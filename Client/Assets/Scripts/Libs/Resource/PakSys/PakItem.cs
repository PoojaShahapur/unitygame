using System.Collections.Generic;
using System.IO;
using System;

namespace SDK.Lib
{
	public class PakItem
	{
		protected ArchiveHeader m_pArchiveHeader;
		protected uint m_fileSize;			// 文件总共大小
		protected List<FileHeader> m_pFileVec;		// 整个文件列表
        public FileStream m_fs = null;      // 文件句柄

		public PakItem()
		{
			m_pArchiveHeader = new ArchiveHeader();
			m_pFileVec = new List<FileHeader>();
		}

		public void clearFileVec()
		{
			m_fileSize = 0;
			m_pArchiveHeader.clear();
			m_pFileVec.Clear();
		}

		public void readArchiveFileHeader()
		{
			ByteBuffer pMByteBuffer = new ByteBuffer();

            if (m_pArchiveHeader.readArchiveFileHeader(m_fs, pMByteBuffer))		// 检查 magic
			{
				FileHeader pFileHeader;
				for (uint idx = 0; idx < m_pArchiveHeader.m_fileCount; ++idx)
				{
					pFileHeader = new FileHeader();
					m_pFileVec.Add(pFileHeader);
					pFileHeader.readHeaderFromArchiveFile(pMByteBuffer);
				}
			}
		}

		public void readArchiveFile2Bytes(FileHeader fileHeader, ref byte[] bytes)
		{
            fileHeader.readArchiveFile2Bytes(m_fs, ref bytes);
		}

        public FileHeader getFileHeader(string path)
        {
            foreach(FileHeader item in m_pFileVec)
            {
                if (item.fileNamePath == path)
                {
                    return item;
                }
            }

            return null;
        }

        public void dispose()
        {
            m_fs.Dispose();
            m_fs.Close();
        }
	}
}
using System.Collections.Generic;
using System.IO;

namespace SDK.Lib
{
    /**
     * @brief 打包的资源系统 base
     */
    public class ABPakFileResItemBase : FileResItem
    {
        public PakItem m_pakItem;
        public Dictionary<string, byte[]> m_path2Bytes = new Dictionary<string,byte[]>();       // 路径到 bytes 内容的字典，从文件加载一次就不再加载，保存这份引用

        override public void init(LoadItem item)
        {
            m_pakItem = new PakItem();
            m_pakItem.m_fs = (item as ABPakLoadItem).m_fs;
            m_pakItem.readArchiveFileHeader();      // 获取打包头部信息
        }

        public override byte[] getBytes(string resname)
        {
            if(!m_path2Bytes.ContainsKey(resname))
            {
                FileHeader fileHeader = m_pakItem.getFileHeader(resname);
                byte[] bytes = new byte[fileHeader.fileSize];
                m_path2Bytes[resname] = bytes;

                m_pakItem.readArchiveFile2Bytes(fileHeader, ref bytes);
            }

            return m_path2Bytes[resname];
        }

        // 卸载
        override public void unload()
        {
            base.unload();
            m_pakItem.dispose();
        }
    }
}
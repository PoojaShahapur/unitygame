using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 打包的资源系统 base
     */
    public class ABPakFileResItemBase : FileResItem
    {
        public PakItem m_pakItem;
        public Dictionary<string, ABMemUnPakFileResItemBase> m_path2MemUnPakRes = new Dictionary<string, ABMemUnPakFileResItemBase>();       // 路径到 bytes 内容的字典，从文件加载一次就不再加载，保存这份引用

        override public void init(LoadItem item)
        {
            base.init(item);

            m_pakItem = new PakItem();
            m_pakItem.m_fs = (item as ABPakLoadItem).m_fs;
            m_pakItem.readArchiveFileHeader();      // 获取打包头部信息

            initMemUnPak();
        }

        protected void initMemUnPak()
        {
            foreach(ABMemUnPakFileResItemBase memUnPakFileResItem in m_path2MemUnPakRes.Values)
            {
                memUnPakFileResItem.initByPakRes();
            }
        }

        public override byte[] getBytes(string resName)
        {
            string unity3dName = Ctx.m_instance.m_pPakSys.path2PakDic[resName].m_unity3dName;

            byte[] bytes = null;

            FileHeader fileHeader = m_pakItem.getFileHeader(unity3dName);
            bytes = new byte[fileHeader.fileSize];
            m_pakItem.readArchiveFile2Bytes(fileHeader, ref bytes);
            if (fileHeader.bCompress())
            {
                byte[] tmpBytes = new byte[fileHeader.fileSize];
                Array.Copy(bytes, 0, tmpBytes, 0, fileHeader.fileSize);
                bytes = tmpBytes;
            }

            return bytes;
        }
    }
}
using SDK.Common;
using System;
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
        //public Dictionary<string, ABUnPakFileResItemBase> m_path2UnPakRes = new Dictionary<string, ABUnPakFileResItemBase>();       // 路径到 bytes 内容的字典，从文件加载一次就不再加载，保存这份引用

        override public void init(LoadItem item)
        {
            base.init(item);

            m_pakItem = new PakItem();
            m_pakItem.m_fs = (item as ABPakLoadItem).m_fs;
            m_pakItem.readArchiveFileHeader();      // 获取打包头部信息
        }

        //public override byte[] getBytes(string resname)
        //{
        //    byte[] bytes = null;
        //    if (m_path2UnPakRes.ContainsKey(resname))
        //    {
        //        if(m_path2UnPakRes[resname].m_bytes != null)
        //        {
        //            bytes = m_path2UnPakRes[resname].m_bytes;
        //        }
        //        else
        //        {
        //            FileHeader fileHeader = m_pakItem.getFileHeader(resname);
        //            bytes = new byte[fileHeader.fileSize];
        //            m_pakItem.readArchiveFile2Bytes(fileHeader, ref bytes);
        //            if(fileHeader.bCompress())
        //            {
        //                m_path2UnPakRes[resname].m_bytes = new byte[fileHeader.fileSize];
        //                Array.Copy(bytes, 0, m_path2UnPakRes[resname].m_bytes, 0, fileHeader.fileSize);
        //                bytes = m_path2UnPakRes[resname].m_bytes;
        //            }
        //            else
        //            {
        //                m_path2UnPakRes[resname].m_bytes = bytes;
        //            }
        //        }
        //    }

        //    return bytes;
        //}

        //public virtual ABUnPakFileResItemBase loadRes(string resname)
        //{
        //    return null;
        //}

        //// 卸载
        //override public void unload()
        //{
        //    base.unload();
        //    m_pakItem.dispose();
        //}

        public override byte[] getBytes(string resname)
        {
            string unity3dName = Ctx.m_instance.m_pPakSys.path2PakDic[resname].m_unity3dName;

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
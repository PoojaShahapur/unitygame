using System.IO;
namespace SDK.Lib
{
    /**
     * @brief 本地文件系统，直接从本地加载
     */
    public class FileResItem : ResItem
    {
        public FileStream m_fs = null;      // 文件句柄
    }
}
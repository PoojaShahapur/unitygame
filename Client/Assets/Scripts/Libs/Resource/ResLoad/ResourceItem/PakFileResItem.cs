using System.IO;

namespace SDK.Lib
{
    /**
     * @brief 打包的系统
     */
    public class PakFileResItem : FileResItem
    {
        public FileStream m_fs = null;      // 文件句柄
    }
}
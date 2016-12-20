using System.Collections;
using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 下载数据
     */
    public class DownloadData
    {
        // 因为资源有些需要协同程序，因此重复利用资源
        public Dictionary<string, DownloadItem> m_path2LDItem;       // 正在下载的内容 DownloadItem
        public ArrayList m_willLDItem;                           // 将要下载的 DownloadItem
        public ArrayList m_noUsedLDItem;                         // 没有被使用的 DownloadItem

        public DownloadData()
        {
            m_path2LDItem = new Dictionary<string, DownloadItem>();
            m_willLDItem = new ArrayList();
            m_noUsedLDItem = new ArrayList();
        }
    }
}
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
        public MDictionary<string, DownloadItem> mPath2LDItem;       // 正在下载的内容 DownloadItem
        public ArrayList mWillLDItem;                           // 将要下载的 DownloadItem
        public ArrayList mNoUsedLDItem;                         // 没有被使用的 DownloadItem

        public DownloadData()
        {
            mPath2LDItem = new MDictionary<string, DownloadItem>();
            mWillLDItem = new ArrayList();
            mNoUsedLDItem = new ArrayList();
        }
    }
}
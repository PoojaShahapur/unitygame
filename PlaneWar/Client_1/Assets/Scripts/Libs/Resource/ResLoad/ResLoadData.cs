using System.Collections;

namespace SDK.Lib
{
    public class ResLoadData
    {
        // 因为资源有些需要协同程序，因此重复利用资源
        public MDictionary<string, LoadItem> mPath2LDItem;       // 正在加载的内容 loaditem
        public ArrayList mWillLDItem;                           // 将要加载的 loaditem
        public ArrayList mNoUsedLDItem;                         // 没有被使用的 loaditem
        public MDictionary<string, ResItem> mPath2Res;
        public ArrayList mNoUsedResItem;                         // 没有被使用的 Res

        public ResLoadData()
        {
            mPath2LDItem = new MDictionary<string, LoadItem>();
            mPath2Res = new MDictionary<string, ResItem>();
            mWillLDItem = new ArrayList();
            mNoUsedLDItem = new ArrayList();
            mNoUsedResItem = new ArrayList();
        }
    }
}
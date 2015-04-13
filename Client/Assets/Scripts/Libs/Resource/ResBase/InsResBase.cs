using SDK.Common;
namespace SDK.Lib
{
    public class InsResBase : IDispatchObject
    {
        public uint m_refNum;                // 引用计数
        public bool m_isLoaded;              // 资源是否加载完成
        public bool m_isSucceed;             // 资源是否加载成功
        public string m_path;

        public void increaseRef()
        {
            ++m_refNum;
        }

        public void decreaseRef()
        {
            --m_refNum;
        }

        public string GetPath()
        {
            return m_path;
        }

        public string getPrefabName()         // 只有 Prefab 资源才实现这个函数
        {
            return "";
        }

        public uint refNum
        {
            get
            {
                return m_refNum;
            }
            set
            {
                m_refNum = value;
            }
        }

        public virtual void unload()
        {

        }
    }
}
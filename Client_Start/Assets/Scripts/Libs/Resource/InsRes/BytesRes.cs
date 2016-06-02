using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 二进制资源
     */
    public class BytesRes : InsResBase
    {
        protected byte[] m_bytes;

        public BytesRes()
        {

        }

        override protected void initImpl(ResItem res)
        {
            // 获取资源单独保存
            m_bytes = res.getBytes(res.getPrefabName());
            base.initImpl(res);
        }

        override public void unload()
        {
            m_bytes = null;
            base.unload();
        }

        public byte[] getBytes(string name)
        {
            return m_bytes;
        }
    }
}
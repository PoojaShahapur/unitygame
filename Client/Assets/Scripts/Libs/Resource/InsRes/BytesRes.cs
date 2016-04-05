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
            m_bytes = (res.getObject(res.getPrefabName()) as TextAsset).bytes;
            base.initImpl(res);
        }

        public byte[] getBytes(string name)
        {
            return m_bytes;
        }
    }
}
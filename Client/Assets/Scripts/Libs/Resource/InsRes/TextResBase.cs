using UnityEngine;

namespace SDK.Lib
{
    public class TextResBase : InsResBase
    {
        protected string m_text;

        public TextResBase()
        {

        }

        override protected void initImpl(ResItem res)
        {
            // 获取资源单独保存
            m_text = (res.getObject(res.getPrefabName()) as TextAsset).text;
            base.initImpl(res);
        }

        public string text
        {
            get
            {
                return m_text;
            }
            set
            {
                m_text = value;
            }
        }
    }
}
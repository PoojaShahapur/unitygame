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
            m_text = res.getText(res.getPrefabName());
            base.initImpl(res);
        }

        override public void unload()
        {
            m_text = null;
            base.unload();
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

        public string getText(string path)
        {
            return m_text;
        }
    }
}
using UnityEngine;

namespace SDK.Lib
{
    public class TextResBase : InsResBase
    {
        protected TextAsset mTextAsset;
        protected string m_text;

        public TextResBase()
        {

        }

        override protected void initImpl(ResItem res)
        {
            // 获取资源单独保存
            mTextAsset = res.getObject(res.getPrefabName()) as TextAsset;
            m_text = mTextAsset.text;
            base.initImpl(res);
        }

        override public void unload()
        {
            UtilApi.UnloadAsset(mTextAsset);

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
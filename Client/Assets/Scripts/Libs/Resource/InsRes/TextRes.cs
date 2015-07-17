using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 文本资源
     */
    public class TextRes : InsResBase
    {
        protected string m_text;

        public TextRes()
        {

        }

        override public void init(ResItem res)
        {
            // 获取资源单独保存
            m_text = (res.getObject(res.getPrefabName()) as TextAsset).text;
            base.init(res);
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
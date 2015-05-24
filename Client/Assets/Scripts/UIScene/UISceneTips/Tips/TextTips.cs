using SDK.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /**
     * @brief 普通文本提示
     */
    public class TextTips : TipsItemBase
    {
        protected AuxLabel m_desc;

        public TextTips(SceneTipsData data)
            : base(data)
        {
            
        }

        public void initWidget()
        {
            m_tipsItemRoot = UtilApi.TransFindChildByPObjAndPath(m_sceneTipsData.m_goRoot, "TextTips");
            m_desc = new AuxLabel(m_sceneTipsData.m_goRoot, "TextTips/Text");
        }

        public void showTips(Vector3 pos, string desc)
        {
            show();
            m_desc.text = desc;
            m_sceneTipsData.m_goRoot.transform.localPosition = pos;
        }
    }
}
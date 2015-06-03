using SDK.Common;
using UnityEngine;
namespace SDK.Lib
{
    /**
     * @brief 随从卡、法术卡
     */
    public class SceneCardPlayerRender : CommonCardRender
    {
        public SceneCardPlayerRender(SceneEntityBase entity_) :
            base(entity_)
        {

        }

        override public void setIdAndPnt(uint objId, GameObject pntGo_)
        {
            base.setIdAndPnt(objId, pntGo_);
            AuxLabel numText = null;
            numText = new AuxLabel(m_model.selfGo, "UIRoot/NumText");       // 卡牌数量
        }
    }
}
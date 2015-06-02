using SDK.Common;
using UnityEngine;
namespace SDK.Lib
{
    public class SceneCardPlayerRender : CardPlayerRender
    {
        override public void setIdAndPnt(uint objId, GameObject pntGo_)
        {
            base.setIdAndPnt(objId, pntGo_);
            AuxLabel numText = null;
            numText = new AuxLabel(m_model.selfGo, "UIRoot/NumText");       // 卡牌数量
        }
    }
}
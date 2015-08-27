using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 随从卡、法术卡，自己手牌渲染器， Enemy 使用 BlackCardRender
     */
    public class SelfHandCardRender : CanOutCardRender
    {
        public SelfHandCardRender(SceneEntityBase entity_) :
            base(entity_, (int)CardSubPartType.eTotal)
        {

        }

        override public void setIdAndPnt(uint objId, GameObject pntGo_)
        {
            base.setIdAndPnt(objId, pntGo_);
            AuxLabel numText = null;
            numText = new AuxLabel(m_model.selfGo, "UIRoot/NumText");       // 卡牌数量
        }

        override protected void addHandle()
        {
            base.addHandle();
            UtilApi.addPressHandle(gameObject(), onEntityDownUp);
            UtilApi.addDragOverHandle(gameObject(), onEntityDragOver);
            UtilApi.addDragOutHandle(gameObject(), onEntityDragOut);
        }
    }
}
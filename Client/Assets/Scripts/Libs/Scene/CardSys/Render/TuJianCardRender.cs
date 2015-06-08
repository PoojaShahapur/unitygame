using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 图鉴中的卡牌
     */
    public class TuJianCardRender : CommonCardRender
    {
        public TuJianCardRender(SceneEntityBase entity_) :
            base(entity_, (int)CardSubPartType.eTotal)
        {

        }
        // 这个是界面中卡牌创建流程， createCard 等同于 setIdAndPnt，所有 setIdAndPnt 中设置的值都需要在 createCard 中进行设置
        public void createCard(CardItemBase cardItem, GameObject pntGo_)
        {
            m_modelPath = Ctx.m_instance.m_dataPlayer.m_dataCard.m_sceneCardModelAttrItemList[cardItem.m_tableItemCard.m_type].m_handleModelPath;

            setTableItemAndPnt(cardItem.m_tableItemCard, pntGo_);

            AuxLabel numText = null;
            numText = new AuxLabel(m_model.selfGo, "UIRoot/NumText");       // 卡牌数量
            numText.text = cardItem.m_tujian.num.ToString();
        }
    }
}
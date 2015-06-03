using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 图鉴中的卡牌
     */
    public class TuJianCardRender : CommonCardRender
    {
        // 这个是界面中卡牌创建流程
        public void createCard(CardItemBase cardItem, GameObject pntGo_)
        {
            setTableItemAndPnt(cardItem.m_tableItemCard, pntGo_);

            AuxLabel numText = null;
            numText = new AuxLabel(m_model.selfGo, "UIRoot/NumText");       // 卡牌数量
            numText.text = cardItem.m_tujian.num.ToString();
        }
    }
}
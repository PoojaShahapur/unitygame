using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    public class TuJianCardRender : CardPlayerRender
    {
        virtual public void createCard(CardItemBase cardItem, GameObject pntGo_)
        {
            setTableItemAndPnt(cardItem.m_tableItemCard, pntGo_);

            AuxLabel numText = null;
            numText = new AuxLabel(m_model.selfGo, "UIRoot/NumText");       // 卡牌数量
            numText.text = cardItem.m_tujian.num.ToString();
        }
    }
}
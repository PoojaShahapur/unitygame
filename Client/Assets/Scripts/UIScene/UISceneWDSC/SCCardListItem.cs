using SDK.Common;
using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief 左边卡牌列表中的一个卡牌
     */
    public class SCCardListItem : InterActiveEntity
    {
        public CardItemBase m_cardItem; // 卡牌基本数据

        public SCCardListItem()
        {
            
        }

        public CardItemBase cardItem
        {
            set
            {
                m_cardItem = value;
            }
        }
    }
}
using SDK.Common;
using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief 收藏界面我的一个卡牌
     */
    public class SCUICardItem : ItemSceneBase
    {
        public CardItemBase m_cardItemBase; // 卡牌基本数据

        public CardItemBase cardItemBase
        {
            set
            {
                m_cardItemBase = value;
                m_prefab = (m_cardItemBase.m_tableItemCard.m_itemBody as TableCardItemBody).m_prefab;
                m_path = (m_cardItemBase.m_tableItemCard.m_itemBody as TableCardItemBody).path;
            }
        }
    }
}
using SDK.Common;
using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief 左边卡牌列表中的一个卡牌
     */
    public class SCCardListItem : ItemSceneBase
    {
        public CardItemBase m_cardItem; // 卡牌基本数据

        public SCCardListItem()
        {
            m_bNorm = false;
        }

        public CardItemBase cardItem
        {
            set
            {
                m_cardItem = value;
                m_prefab = Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardGroupModelAttrItem.m_prefabName;
                m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel] + m_prefab;
            }
        }
    }
}
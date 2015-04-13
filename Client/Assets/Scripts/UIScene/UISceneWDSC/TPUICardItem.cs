using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 一张显示的套牌
     */
    public class TPUICardItem : ItemSceneIOBase
    {
        public CardGroupItem m_cardGroupItem; // 卡牌基本数据

        public TPUICardItem()
        {
            m_bNorm = false;
        }

        public CardGroupItem cardGroupItem
        {
            set
            {
                m_cardGroupItem = value;
                m_path = Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardGroupModelAttrItem.m_path;
            }
        }

        protected override void onBtnClkOpen(GameObject go)
        {
            if(m_cardGroupItem != null)
            {
                m_cardGroupItem.reqCardList();
            }
        }
    }
}
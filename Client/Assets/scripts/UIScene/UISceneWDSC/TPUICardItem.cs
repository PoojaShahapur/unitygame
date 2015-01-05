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

        public CardGroupItem cardGroupItem
        {
            set
            {
                m_cardGroupItem = value;
                m_prefab = "pack";
                m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel] + m_prefab;
            }
        }

        protected virtual void onBtnClkOpen(GameObject go)
        {
            if(m_cardGroupItem != null)
            {
                m_cardGroupItem.reqCardList();
            }
        }
    }
}
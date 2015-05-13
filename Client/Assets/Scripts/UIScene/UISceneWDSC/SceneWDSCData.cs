using SDK.Common;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class SceneWDSCData
    {
        public ClassFilterPnl m_pClassFilterPnl = new ClassFilterPnl();            // 类型过滤面板
        public WdscCardSetPnl m_wdscCardSetPnl = new WdscCardSetPnl();
        public WdscCardPnl m_wdscCardPnl = new WdscCardPnl();
        public LeftBtnPnl m_leftBtnPnl = new LeftBtnPnl();

        public Action<SCUICardItemCom> m_onClkCard;
        public GameObject m_sceneUIParentGo;      // 场景 UI 结点
        public GameObject m_sceneUIGo;          // 场景 UI 结点

        public SceneWDSCData()
        {
            
        }

        public SCCardListItem createCard(uint id)
        {
            SCCardListItem item = new SCCardListItem();
            item.cardItem = Ctx.m_instance.m_dataPlayer.m_dataCard.m_id2CardDic[id];
            item.setGameObject(Ctx.m_instance.m_modelMgr.getGroupCardModel().InstantiateObject(""));
            return item;
        }
    }
}
using Game.Msg;
using SDK.Lib;
using SDK.Lib;
using System.Collections.Generic;
using UnityEngine;

namespace Fight
{
    /**
     * @brief 历史记录提示
     */
    public class HistoryTips : TipsItemBase
    {
        protected HistoryTipsCard m_cardItem;
        protected List<HistoryTipsCard> m_list;

        public HistoryTips(SceneTipsData data)
            : base(data)
        {
            m_cardItem = new HistoryTipsCard();
            m_list = new List<HistoryTipsCard>();
        }

        public override void hide()
        {
            base.hide();
            UtilApi.Destroy(m_cardItem.getGameObject());

            foreach (HistoryTipsCard item in m_list)
            {
                UtilApi.Destroy(item.getGameObject());
            }
        }

        public void initWidget()
        {
            m_tipsItemRoot = UtilApi.TransFindChildByPObjAndPath(m_sceneTipsData.m_goRoot, "HistoryTips");
        }

        public void showTips(Vector3 pos, stRetBattleHistoryInfoUserCmd data)
        {
            show();

            // 显示卡牌历史提示
            TableCardItemBody cardTableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, data.maincard.dwObjectID).m_itemBody as TableCardItemBody;

            GameObject tmpGO = null;
            //tmpGO = Ctx.m_instance.m_modelMgr.getSceneCardModel((CardType)cardTableItem.m_type).getObject();
            if (tmpGO != null)
            {
                m_cardItem.setGameObject(UtilApi.Instantiate(tmpGO) as GameObject);
                m_cardItem.transform.SetParent(m_tipsItemRoot.transform, false);
                UtilApi.setPos(m_cardItem.transform, new Vector3(-2.12f, 0, 0));
#if UNITY_5
                UtilApi.setRot(m_cardItem.transform, Quaternion.Euler(270, 0, 0));
#elif UNITY_4_6 || UNITY_4_5
                UtilApi.setRot(m_cardItem.transform, Quaternion.EulerRotation(270, 0, 0));
#endif
            }

            int idx = 0;
            for (idx = 0; idx < data.othercard.Length; ++idx)
            {
                cardTableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, data.othercard[idx].dwObjectID).m_itemBody as TableCardItemBody;

                //tmpGO = Ctx.m_instance.m_modelMgr.getSceneCardModel((CardType)cardTableItem.m_type).getObject();
                if (tmpGO != null)
                {
                    if (idx >= m_list.Count)
                    {
                        m_list.Add(new HistoryTipsCard());
                    }

                    m_list[idx].setGameObject(UtilApi.Instantiate(tmpGO) as GameObject);
                    m_list[idx].transform.SetParent(m_tipsItemRoot.transform, false);
                    UtilApi.setPos(m_cardItem.transform, new Vector3(-2.12f + 1.5f * (1 + idx), 0, 0));
#if UNITY_5
                    UtilApi.setRot(m_list[idx].transform, Quaternion.Euler(270, 0, 0));
#elif UNITY_4_6 || UNITY_4_5
                    UtilApi.setRot(m_list[idx].transform, Quaternion.EulerRotation(270, 0, 0));
#endif
                }
            }
        }
    }
}
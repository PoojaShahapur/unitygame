using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SDK.Common;
using SDK.Lib;

namespace Game.UI
{
    /// <summary>
    /// 对我的收藏的页进行控制
    /// </summary>
    public class wdscpage : InterActiveEntity
    {
        public SceneWDSCData m_sceneWDSCData = new SceneWDSCData();
        public UIGrid m_CardList = new UIGrid();            // 收藏卡牌数据
        public List<SCUICardItem> m_SCUICardItemList = new List<SCUICardItem>();
        public Text pagename;

        public override void Awake()
        {

        }

        public override void Start()
        {
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName(SceneSCPath.BtnPrePage), onPreBtnClk);       // 前一页
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName(SceneSCPath.BtnNextPage), onNextBtnClk);       // 后一页

            m_CardList.setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/page/CardList"));
            m_CardList.cellWidth = 0.00473f;
            m_CardList.cellHeight = 0.00663f;
            m_CardList.maxPerLine = 4;
        }

        /// <summary>
        /// 销毁8张牌
        /// </summary>
        void DestroyCrad()
        {
            GameObject go;
            for (int i = m_CardList.getChildCount() - 1; i >= 0; i--)
            {
                go = m_CardList.GetChild(i).gameObject;
                go.transform.parent = null;
                UtilApi.Destroy(go);
            }

            m_SCUICardItemList.Clear();
        }

        //对职业进行过滤
        public void onclass(EnPlayerCareer c)
        {
            DestroyCrad();

            m_sceneWDSCData.m_curTabPageIdx = (int)c;
            List<CardItemBase> list = Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardListArr[(int)c];
            GameObject tmpGO;
            GameObject go;

            if (Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardListArr[m_sceneWDSCData.m_curTabPageIdx].Count > 0)
            {
                int idx = 0;
                CardItemBase cardItem;
                SCUICardItem uicardItem;
                while (m_sceneWDSCData.m_pageArr[m_sceneWDSCData.m_curTabPageIdx].m_curPageIdx * (int)SCCardNumPerPage.eNum + idx < Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardListArr[m_sceneWDSCData.m_curTabPageIdx].Count && idx < (int)SCCardNumPerPage.eNum)
                {
                    cardItem = Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardListArr[m_sceneWDSCData.m_curTabPageIdx][m_sceneWDSCData.m_pageArr[m_sceneWDSCData.m_curTabPageIdx].m_curPageIdx * (int)SCCardNumPerPage.eNum + idx];

                    tmpGO = Ctx.m_instance.m_modelMgr.getSceneCardModel((CardType)cardItem.m_tableItemCard.m_type).getObject() as GameObject;
                    if (tmpGO != null)
                    {
                        //go = UtilApi.Instantiate(tmpGO, Vector3.zero, tmpGO.transform.rotation) as GameObject;
                        go = UtilApi.Instantiate(tmpGO) as GameObject;
                        m_CardList.AddChild(go.transform);
                        UtilApi.normalPos(go.transform);
                        uicardItem = new SCUICardItem();
                        uicardItem.setGameObject(go);
                        uicardItem.cardItemBase = cardItem;
                        m_SCUICardItemList.Add(uicardItem);
                        m_SCUICardItemList[m_SCUICardItemList.Count - 1].m_clkCB = m_sceneWDSCData.m_onClkCard;

                        UtilApi.updateCardDataNoChange(cardItem.m_tableItemCard, uicardItem.getGameObject());
                        UtilApi.updateCardDataChange(cardItem.m_tableItemCard, uicardItem.getGameObject());
                    }
                    ++idx;
                }

                m_CardList.Reposition();
            }

            m_sceneWDSCData.m_textPageNum.text = string.Format("第{0}页", m_sceneWDSCData.m_pageArr[m_sceneWDSCData.m_curTabPageIdx].m_curPageIdx + 1);
        }

        // 收藏中前一页
        public void onPreBtnClk()
        {
            if (canMovePre())
            {
                --m_sceneWDSCData.m_pageArr[m_sceneWDSCData.m_curTabPageIdx].m_curPageIdx;
                updatePageUI();
            }
        }

        // 收藏中后一页
        public void onNextBtnClk()
        {
            if (canMoveNext())
            {
                ++m_sceneWDSCData.m_pageArr[m_sceneWDSCData.m_curTabPageIdx].m_curPageIdx;
                updatePageUI();
            }
        }

        // 判断当前 TabPage 是否可以向前翻页
        public bool canMovePre()
        {
            return m_sceneWDSCData.m_pageArr[m_sceneWDSCData.m_curTabPageIdx].m_curPageIdx > 0;
        }

        // 判断当前的 TabPage 是否可以向后翻页
        public bool canMoveNext()
        {
            return ((m_sceneWDSCData.m_pageArr[m_sceneWDSCData.m_curTabPageIdx].m_curPageIdx + 1) * (int)SCCardNumPerPage.eNum < Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardListArr[m_sceneWDSCData.m_curTabPageIdx].Count);
        }

        public void updatePageUI()
        {
            if (m_sceneWDSCData.m_curTabPageIdx >= 0)
            {
                onclass((EnPlayerCareer)m_sceneWDSCData.m_curTabPageIdx);
            }
        }
    }
}
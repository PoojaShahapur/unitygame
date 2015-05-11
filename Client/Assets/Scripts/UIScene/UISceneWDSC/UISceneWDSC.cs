using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /**
     * @brief 扩展包
     */
    public class UISceneWDSC : SceneForm
    {
        protected SceneWDSCData m_sceneWDSCData = new SceneWDSCData();

        protected wdscjm m_wdscjm = new wdscjm();
        protected newCardSet m_newCardSet = new newCardSet();
        protected btn[] m_btnArr = new btn[(int)SceneWDSCBtnEnum.eBtnTotal];
        protected wdscpage m_wdscpage = new wdscpage();

        public List<cardset> m_taoPaiEntityList = new List<cardset>();      // 当前已经有的卡牌组
        public List<SCCardListItem> m_cardList = new List<SCCardListItem>();// 当前在编辑的卡牌列表
        public cardset m_curEditCardSet = null;                 // 当前正在编辑的卡牌组
        public UIGrid m_leftCardList = new UIGrid();            // 左边的卡牌列表
        public UIGrid m_leftCardGroupList = new UIGrid();       // 左边的卡牌组列表

        public override void onReady()
        {
            base.onReady();

            m_sceneWDSCData.m_onClkCard = onClkCard;

            getWidget();
            addEventHandle();

            psstNotifyAllCardTujianInfoCmd();
        }

        public override void onShow()
        {
            base.onShow();
            onclass(EnPlayerCareer.HERO_OCCUPATION_1);      // 切换到第一个职业
        }

        // 获取控件
        protected void getWidget()
        {
            m_leftCardList.setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/setbtn/LeftCardList"));
            m_leftCardList.cellWidth = 1.172f;
            m_leftCardList.cellHeight = 0.445f;
            m_leftCardList.m_hideZPos = -20f;
            m_leftCardList.hide();              // 默认隐藏

            m_leftCardGroupList.setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/setbtn/CardGroupList"));
            m_leftCardGroupList.cellWidth = 1.172f;
            m_leftCardGroupList.cellHeight = 0.445f;
            m_leftCardGroupList.m_hideZPos = -20f;

            m_wdscjm.setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm"));
            m_newCardSet.setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/setbtn/newSetBtn"));

            m_newCardSet.insEditCardGroup();

            m_btnArr[(int)SceneWDSCBtnEnum.eBtnBack] = new btn();
            m_btnArr[(int)SceneWDSCBtnEnum.eBtnBack].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/wdscfh/btn"));

            int idx = 0;

            idx = 0;
            while (idx < 10)
            {
                m_sceneWDSCData.m_tabBtnList[idx] = new ClassFilterBtn();
                m_sceneWDSCData.m_tabBtnList[idx].sceneWDSCData = m_sceneWDSCData;
                ++idx;
            }

            m_sceneWDSCData.m_tabBtnList[0].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/fs"));
            m_sceneWDSCData.m_tabBtnList[0].sceneWDSCData = m_sceneWDSCData;
            m_sceneWDSCData.m_tabBtnList[0].tag = 1;
            m_sceneWDSCData.m_tabBtnList[0].myClass = (EnPlayerCareer)1;

            m_sceneWDSCData.m_tabBtnList[1].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/sq"));
            m_sceneWDSCData.m_tabBtnList[1].sceneWDSCData = m_sceneWDSCData;
            m_sceneWDSCData.m_tabBtnList[1].tag = 2;
            m_sceneWDSCData.m_tabBtnList[1].myClass = (EnPlayerCareer)2;

            m_sceneWDSCData.m_tabBtnList[2].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/ms"));
            m_sceneWDSCData.m_tabBtnList[2].sceneWDSCData = m_sceneWDSCData;
            m_sceneWDSCData.m_tabBtnList[2].tag = 3;
            m_sceneWDSCData.m_tabBtnList[2].myClass = (EnPlayerCareer)3;

            m_sceneWDSCData.m_tabBtnList[3].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/dz"));
            m_sceneWDSCData.m_tabBtnList[3].sceneWDSCData = m_sceneWDSCData;
            m_sceneWDSCData.m_tabBtnList[3].tag = 4;
            m_sceneWDSCData.m_tabBtnList[3].myClass = (EnPlayerCareer)4;

            m_sceneWDSCData.m_tabBtnList[4].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/sm"));
            m_sceneWDSCData.m_tabBtnList[4].sceneWDSCData = m_sceneWDSCData;
            m_sceneWDSCData.m_tabBtnList[4].tag = 5;
            m_sceneWDSCData.m_tabBtnList[4].myClass = (EnPlayerCareer)5;

            m_sceneWDSCData.m_tabBtnList[5].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/ss"));
            m_sceneWDSCData.m_tabBtnList[5].sceneWDSCData = m_sceneWDSCData;
            m_sceneWDSCData.m_tabBtnList[5].tag = 6;
            m_sceneWDSCData.m_tabBtnList[5].myClass = (EnPlayerCareer)6;

            m_sceneWDSCData.m_tabBtnList[6].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/zs"));
            m_sceneWDSCData.m_tabBtnList[6].sceneWDSCData = m_sceneWDSCData;
            m_sceneWDSCData.m_tabBtnList[6].tag = 7;
            m_sceneWDSCData.m_tabBtnList[6].myClass = (EnPlayerCareer)7;

            m_sceneWDSCData.m_tabBtnList[7].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/lr"));
            m_sceneWDSCData.m_tabBtnList[7].sceneWDSCData = m_sceneWDSCData;
            m_sceneWDSCData.m_tabBtnList[7].tag = 8;
            m_sceneWDSCData.m_tabBtnList[7].myClass = (EnPlayerCareer)8;

            m_sceneWDSCData.m_tabBtnList[8].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/dly"));
            m_sceneWDSCData.m_tabBtnList[8].sceneWDSCData = m_sceneWDSCData;
            m_sceneWDSCData.m_tabBtnList[8].tag = 9;
            m_sceneWDSCData.m_tabBtnList[8].myClass = (EnPlayerCareer)9;

            m_sceneWDSCData.m_tabBtnList[9].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/zl"));
            m_sceneWDSCData.m_tabBtnList[9].sceneWDSCData = m_sceneWDSCData;
            m_sceneWDSCData.m_tabBtnList[9].tag = 0;
            m_sceneWDSCData.m_tabBtnList[9].myClass = (EnPlayerCareer)0;

            // 默认选择第一个，是 1 不是 0
            m_sceneWDSCData.onBtnClk(1);

            m_wdscpage.m_sceneWDSCData = m_sceneWDSCData;
            m_wdscpage.setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/page"));

            m_sceneWDSCData.m_textPageNum = UtilApi.getComByP<Text>(SceneSCPath.TextPageNum);
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName(SceneSCPath.BtnClose), onBtnClkClose);       // 返回
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName(SceneSCPath.BtnAddTaoPai), onBtnClkAddTaoPai);       // 新增套牌
        }

        protected void onBtnClkClose(GameObject go)
        {
            //(Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("wdscjm") as wdscjm).back();
            m_wdscjm.back();
        }

        protected void onBtnClkAddTaoPai(GameObject go)
        {
            UISceneMoShi uiMS = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneMoShi>(UISceneFormID.eUISceneMoShi);
            if (uiMS == null)
            {
                Ctx.m_instance.m_uiSceneMgr.loadSceneForm<UISceneMoShi>(UISceneFormID.eUISceneMoShi);
            }
            uiMS = Ctx.m_instance.m_uiSceneMgr.showSceneForm(UISceneFormID.eUISceneMoShi) as UISceneMoShi;
            uiMS.AddNewTaoPai();
        }

        public void showUI()
        {
            m_wdscjm.show();
        }

        // 所有卡牌
        public void psstNotifyAllCardTujianInfoCmd()
        {
            //if (Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardListArr[m_curTabPageIdx].Count > 0)
            //{
            //    int idx = 0;
            //    CardItemBase cardItem;
            //    while (idx < Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardListArr[m_curTabPageIdx].Count && idx < (int)SCCardNumPerPage.eNum)
            //    {
            //        cardItem = Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardListArr[m_curTabPageIdx][idx];
            //        m_SCUICardItemList[idx].cardItemBase = cardItem;
            //        m_SCUICardItemList[idx].m_clkCB = onClkCard;
            //        m_SCUICardItemList[idx].load();
            //        ++idx;
            //    }
            //}

            m_wdscpage.updatePageUI();
        }

        // 新增\数量改变,不包括删除, badd 指明是添加还是改变
        public void psstNotifyOneCardTujianInfoCmd(uint id, byte num, bool badd)
        {
            if(badd)
            {
                //newcardset(Ctx.m_instance.m_dataPlayer.m_dataCard.m_id2CardGroupDic[id]);
                // 增加一张卡牌
                m_wdscpage.updatePageUI();
            }
        }

        // 套牌列表
        public void psstRetCardGroupListInfoUserCmd()
        {
            if (Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardGroupListArr.Count > 0)
            {
                int idx = 0;
                CardGroupItem cardItem;
                while (idx < Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardGroupListArr.Count && idx < (int)SCTPNumPerPage.eNum)
                {
                    cardItem = Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardGroupListArr[idx];
                    //m_SCUITPItemList[idx].cardGroupItem = cardItem;
                    //m_SCUITPItemList[idx].load();
                    newcardset(cardItem, false);

                    ++idx;
                }
            }
        }

        // 一个套牌的卡牌列表，index 指明是哪个套牌的
        public void psstRetOneCardGroupInfoUserCmd(uint index, List<uint> list)
        {

        }

        // 新添加一个套牌
        public void psstRetCreateOneCardGroupUserCmd(CardGroupItem cardGroup)
        {
            //releaseAllTaoPai();
            //psstRetCardGroupListInfoUserCmd();

            //m_curEditCardInfo.clear();
            //m_curEditCardInfo.index = cardGroup.m_cardGroup.index;

            UISceneMoShi uiMS = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneMoShi>(UISceneFormID.eUISceneMoShi);
            if (uiMS != null)
            {
                newcardset(cardGroup);
            }
        }

        // 删除一个套牌
        public void psstRetDeleteOneCardGroupUserCmd(uint index)
        {
            //releaseAllTaoPai();
            //psstRetCardGroupListInfoUserCmd();
            int curIdx = 0;
            foreach(CardGroupItem item in Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardGroupListArr)
            {
                if(item.m_cardGroup.index == index)
                {
                    break;
                }

                ++curIdx;
            }
            delOneCardGroup(curIdx);
        }

        protected void releaseAllTaoPai()
        {
            int idx = 0;
            while (idx < (int)SCTPNumPerPage.eNum)
            {
                //m_SCUITPItemList[idx].unload();
                ++idx;
            }
        }

        public void newcardset(CardGroupItem cardGroup, bool bEnterEdit = true)
        {
            m_newCardSet.newcardset(cardGroup, bEnterEdit);
        }

        public void editset()
        {
            m_wdscjm.editset();
        }

        public void hidenewCardSet()
        {
            m_newCardSet.hide();
        }

        public void hideAllCard()
        {
            m_newCardSet.hideAllCard();
        }

        public void classfilterhide(EnPlayerCareer c)
        {
            foreach (ClassFilterBtn item in m_sceneWDSCData.m_tabBtnList)
            {
                item.classFilterHide(c);
            }
        }

        // 切换某个职业
        public void onclass(EnPlayerCareer myclass)
        {
            // 做动画，设置当前的也签
            m_sceneWDSCData.onBtnClk((int)myclass);
            m_wdscpage.onclass(myclass);
        }

        public void classfilter_gotoback()
        {
            foreach (ClassFilterBtn item in m_sceneWDSCData.m_tabBtnList)
            {
                item.gotoBack();
            }
        }

        public void newCardSet_goback()
        {
            m_newCardSet.goback();
        }

        public void cardset_goback()
        {
            foreach (cardset taoPai in Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneWDSC>(UISceneFormID.eUISceneWDSC).m_taoPaiEntityList)
            {
                taoPai.goback();
            }
        }

        // 请求保存卡牌
        public void reqSaveCard()
        {
            stReqSaveOneCardGroupUserCmd cmd = new stReqSaveOneCardGroupUserCmd();
            //cmd.index = m_curEditCardInfo.index;
            //cmd.id = m_curEditCardInfo.id;

            cmd.index = m_curEditCardSet.info.m_cardGroup.index;
            cmd.count = (ushort)m_curEditCardSet.info.m_cardList.Count;
            cmd.id = m_curEditCardSet.info.m_cardList;

            UtilMsg.sendMsg(cmd);
        }

        // 保存卡牌成功
        public void psstRetSaveOneCardGroupUserCmd(uint index)
        {
            // 保存当前卡牌
            getCardSetByIndex(index).copyFrom(m_curEditCardSet);
        }
        
        public cardset getCardSetByIndex(uint index)
        {
            foreach(cardset item in m_taoPaiEntityList)
            { 
                if(item.info.m_cardGroup.index == index)
                {
                    return item;
                }
            }

            return null;
        }

        public void delOneCardGroup(int p)
        {
            //int curidx = p;
            //把别的向上移动
            //for (; curidx < m_playersets.Count; curidx++)
            //{
            //    m_playersets[curidx].Translate(new Vector3(0, 0, 0.525f));
            //}
            //UtilApi.Destroy(m_playersets[p].gameObject);
            //m_playersets.Remove(m_playersets[p]);
            UtilApi.Destroy(m_taoPaiEntityList[p].getGameObject());
            m_taoPaiEntityList.RemoveAt(p);
            m_leftCardGroupList.Reposition();
            m_newCardSet.updatePos();
        }

        public void onClkCard(SCUICardItem ioItem)
        {
            if (wdscjm.nowMod == wdscmMod.editset)
            {
                if(m_curEditCardSet.info.m_cardList == null)
                {
                    m_curEditCardSet.info.m_cardList = new List<uint>();
                }
                if (m_curEditCardSet.info.m_cardList.IndexOf(ioItem.m_cardItemBase.m_tujian.id) == -1)
                {
                    if (m_curEditCardSet.info.m_cardList.Count < 30)
                    {
                        m_curEditCardSet.info.m_cardList.Add(ioItem.m_cardItemBase.m_tujian.id);
                        // 继续添加显示
                        m_cardList.Add(createCard(ioItem.m_cardItemBase.m_tujian.id));
                        m_leftCardList.AddChild(m_cardList[m_cardList.Count - 1].getGameObject().transform);
                    }
                }
            }
        }

        public void updateLeftCardList()
        {
            releaseLeftCardList();
            if(m_curEditCardSet != null && m_curEditCardSet.info.m_cardList != null)
            {
                int idx = 0;
                while(idx < m_curEditCardSet.info.m_cardList.Count)
                {
                    m_cardList.Add(createCard(m_curEditCardSet.info.m_cardList[idx]));
                    m_leftCardList.AddChild(m_cardList[idx].getGameObject().transform);
                    ++idx;
                }
            }
        }

        protected SCCardListItem createCard(uint id)
        {
            SCCardListItem item = new SCCardListItem();
            item.cardItem = Ctx.m_instance.m_dataPlayer.m_dataCard.m_id2CardDic[id];
            item.setGameObject(UtilApi.Instantiate(Ctx.m_instance.m_modelMgr.getGroupCardModel().getObject(), m_newCardSet.getGameObject().transform.position, m_newCardSet.getGameObject().transform.rotation) as GameObject);
            return item;
        }

        protected void releaseLeftCardList()
        {
            int idx = 0;
            while (idx < m_cardList.Count)
            {
                m_cardList[idx].getGameObject().transform.parent = null;
                UtilApi.Destroy(m_cardList[idx].getGameObject());
                ++idx;
            }
            m_cardList.Clear();
        }
    }
}
using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 扩展包
     */
    public class UISceneWDSC : SceneForm, IUISceneWDSC
    {
        protected wdscjm m_wdscjm = new wdscjm();
        protected newCardSet m_newCardSet = new newCardSet();
        protected btn[] m_btnArr = new btn[(int)SceneWDSCBtnEnum.eBtnTotal];
        protected classfilter[] m_tabBtnList = new classfilter[10];
        protected wdscpage m_wdscpage = new wdscpage();

        protected SCUICardItem[] m_SCUICardItemList = new SCUICardItem[(int)SCCardNumPerPage.eNum];     // 每一职业卡牌显示列表
        //protected TPUICardItem[] m_SCUITPItemList = new TPUICardItem[(int)SCTPNumPerPage.eNum];       // 套牌显示列表

        protected int m_curPageIdx;     // 当前显示的 Page 索引
        //protected CurEditCardInfo m_curEditCardInfo = new CurEditCardInfo();        // 当前编辑的卡牌信息

        public List<Transform> m_playersets = new List<Transform>();       // 卡牌组 Tranforms 以及最后的按钮
        public List<cardset> m_taoPaiEntityList = new List<cardset>();      // 当前已经有的卡牌
        public cardset m_curEditCardSet = null;            // 当前正在编辑的卡牌组
        public UIGrid m_leftCardList = new UIGrid();

        public override void onReady()
        {
            base.onReady();

            getWidget();
            addEventHandle();

            psstNotifyAllCardTujianInfoCmd();
        }

        public override void onShow()
        {
            base.onShow();
        }

        // 获取控件
        protected void getWidget()
        {
            m_wdscjm.setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm"));
            m_newCardSet.setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/setbtn/newSetBtn"));

            m_newCardSet.insEditCardGroup();

            m_btnArr[(int)SceneWDSCBtnEnum.eBtnBack] = new btn();
            m_btnArr[(int)SceneWDSCBtnEnum.eBtnBack].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/wdscfh/btn"));

            int idx = 0;
            while(idx < (int)SCCardNumPerPage.eNum)
            {
                m_SCUICardItemList[idx] = new SCUICardItem();
                m_SCUICardItemList[idx].m_tran = UtilApi.GoFindChildByPObjAndName("wdscjm/page/" + (idx + 1)).transform;
                ++idx;
            }
            //idx = 0;
            //while (idx < (int)SCTPNumPerPage.eNum)
            //{
            //    m_SCUITPItemList[idx] = new TPUICardItem();
            //    m_SCUITPItemList[idx].m_tran = UtilApi.GoFindChildByPObjAndName("wdscjm/kuan/kong/kong" + idx).transform;
            //    ++idx;
            //}

            idx = 0;
            while(idx < 10)
            {
                m_tabBtnList[idx] = new classfilter();
                ++idx;
            }

            m_tabBtnList[0].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/fs"));
            m_tabBtnList[1].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/sq"));
            m_tabBtnList[2].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/ms"));
            m_tabBtnList[3].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/dz"));
            m_tabBtnList[4].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/sm"));
            m_tabBtnList[5].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/ss"));
            m_tabBtnList[6].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/zs"));
            m_tabBtnList[7].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/lr"));
            m_tabBtnList[8].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/dly"));
            m_tabBtnList[9].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/zl"));

            m_wdscpage.setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/page"));

            //m_leftCardList.setGameObject();
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
            IUISceneMoShi uiMS = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneMoShi) as IUISceneMoShi;
            if (uiMS == null)
            {
                Ctx.m_instance.m_uiSceneMgr.loadSceneForm(UISceneFormID.eUISceneMoShi);
            }
            uiMS = Ctx.m_instance.m_uiSceneMgr.showSceneForm(UISceneFormID.eUISceneMoShi) as IUISceneMoShi;
            uiMS.AddNewTaoPai();
        }

        public void showUI()
        {
            m_wdscjm.show();
        }

        // 所有卡牌
        public void psstNotifyAllCardTujianInfoCmd()
        {
            if(Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardListArr[m_curPageIdx].Count > 0)
            {
                int idx = 0;
                CardItemBase cardItem;
                while(idx < Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardListArr[m_curPageIdx].Count && idx < (int)SCCardNumPerPage.eNum)
                {
                    cardItem = Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardListArr[m_curPageIdx][idx];
                    m_SCUICardItemList[idx].cardItemBase = cardItem;
                    m_SCUICardItemList[idx].m_clkCB = onClkCard;
                    m_SCUICardItemList[idx].load();
                    ++idx;
                }
            }
        }

        // 新增\数量改变,不包括删除, badd 指明是添加还是改变
        public void psstNotifyOneCardTujianInfoCmd(uint id, byte num, bool badd)
        {
            if(badd)
            {
                newcardset(Ctx.m_instance.m_dataPlayer.m_dataCard.m_id2CardGroupDic[id]);
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

            IUISceneMoShi uiMS = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneMoShi) as IUISceneMoShi;
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

        protected void releaseAllCard()
        {
            int idx = 0;
            while (idx < (int)SCCardNumPerPage.eNum)
            {
                m_SCUICardItemList[idx].unload();
                ++idx;
            }
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

        public void hide()
        {
            m_newCardSet.hide();
        }

        public void hideAllCard()
        {
            m_newCardSet.hideAllCard();
        }

        public void classfilterhide(EnPlayerCareer c)
        {
            foreach(classfilter item in m_tabBtnList)
            {
                item.classfilterhide(c);
            }
        }

        public void onclass(EnPlayerCareer myclass)
        {
            m_wdscpage.onclass(myclass);
        }

        public void classfilter_gotoback()
        {
            foreach (classfilter item in m_tabBtnList)
            {
                item.gotoback();
            }
        }

        public void newCardSet_goback()
        {
            m_newCardSet.goback();
        }

        public void cardset_goback()
        {
            foreach (cardset taoPai in (Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_taoPaiEntityList)
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
            int curidx = p;
            //把别的向上移动
            for (; curidx < m_playersets.Count; curidx++)
            {
                m_playersets[curidx].Translate(new Vector3(0, 0, 0.525f));
            }
            UtilApi.Destroy(m_playersets[p].gameObject);
            m_playersets.Remove(m_playersets[p]);
            m_taoPaiEntityList.RemoveAt(p);
        }

        public void onClkCard(ItemSceneIOBase ioItem)
        {
            if (wdscjm.nowMod == wdscmMod.editset)
            {
                SCUICardItem item = ioItem as SCUICardItem;
                if(m_curEditCardSet.info.m_cardList == null)
                {
                    m_curEditCardSet.info.m_cardList = new List<uint>();
                }
                if(m_curEditCardSet.info.m_cardList.IndexOf(item.m_cardItemBase.m_tujian.id) == -1)
                {
                    m_curEditCardSet.info.m_cardList.Add(item.m_cardItemBase.m_tujian.id);
                }
            }
        }
    }
}
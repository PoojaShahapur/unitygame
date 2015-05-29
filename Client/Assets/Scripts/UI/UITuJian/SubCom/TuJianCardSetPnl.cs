using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SDK.Common;
using SDK.Lib;
using Game.Msg;

namespace Game.UI
{
    /// <summary>
    /// 用来控制我的收藏时,卡牌的行为
    /// </summary>
    public enum WdscmTaoPaiMod
    {
        eTaoPaiMod_Look,
        eTaoPaiMod_Editset
    }

    public enum WdscCardSetPnl_BtnIndex
    {
        eBtnNewCardSet,
        eBtnRet,
        
        eBtnTotal,
    }

    /// <summary>
    /// 我的收藏右边的套牌面板
    /// </summary>
    public class TuJianCardSetPnl : TuJianPnlBase
    {
        public CardSetCom m_curEditCardSet = null;                 // 当前正在编辑的卡牌组，注意这个不是指向编辑的指针，而是拷贝的数据
        public List<TuJianCardSetCardItemCom> m_cardList = new List<TuJianCardSetCardItemCom>();// 当前在编辑的卡组中的卡牌列表
        public Dictionary<uint, TuJianCardSetCardItemCom> m_id2CardComDic = new Dictionary<uint,TuJianCardSetCardItemCom>();
        public List<CardSetCom> m_cardSetEntityList = new List<CardSetCom>();      // 当前已经有的卡牌组

        public AuxLayoutV m_cardSetCardLayoutV;            // 左边的卡牌列表
        public AuxLayoutV m_cardSetLayoutV;

        public WdscmTaoPaiMod m_curTaoPaiMod;          // 当前套牌模式

        protected AuxBasicButton[] m_btnArr = new AuxBasicButton[(int)WdscCardSetPnl_BtnIndex.eBtnTotal];
        protected AuxLabel m_cardSetCardCntText;        // 卡组中卡牌的数量

        public GameObject m_topEditCardPosGo;
        public CardSetCom m_curCardSet;                  // 当前操作的卡组 ID

        public TuJianCardSetPnl(TuJianData data) :
            base(data)
        {
            
        }

        public new void findWidget()
        {
            m_topEditCardPosGo = UtilApi.TransFindChildByPObjAndPath(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.GoTopEditCardPos);

            m_cardSetCardLayoutV = new AuxLayoutV();
            m_cardSetCardLayoutV.elemWidth = 285;
            m_cardSetCardLayoutV.elemHeight = 78;
            m_cardSetCardLayoutV.pntGo = UtilApi.TransFindChildByPObjAndPath(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.CardSetCardListParent);
            m_cardSetCardLayoutV.selfGo = UtilApi.TransFindChildByPObjAndPath(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.CardSetCardListCon);
            m_cardSetCardLayoutV.hideLayout();

            m_cardSetLayoutV = new AuxLayoutV();
            m_cardSetLayoutV.elemWidth = 285;
            m_cardSetLayoutV.elemHeight = 78;
            m_cardSetLayoutV.pntGo = UtilApi.TransFindChildByPObjAndPath(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.CardSetListParent);
            m_cardSetLayoutV.selfGo = UtilApi.TransFindChildByPObjAndPath(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.CardSetListCont);

            m_btnArr[(int)WdscCardSetPnl_BtnIndex.eBtnNewCardSet] = new AuxBasicButton(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.BtnNewCardSet);

            m_btnArr[(int)WdscCardSetPnl_BtnIndex.eBtnRet] = new AuxDynImageStaticGoButton(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.BtnRet);
            updateRetBtnImage(true);
            m_cardSetCardCntText = new AuxLabel(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.TextCardSetCardCnt);
        }

        public new void addEventHandle()
        {
            m_btnArr[(int)WdscCardSetPnl_BtnIndex.eBtnNewCardSet].addEventHandle(onBtnClkAddTaoPai);       // 新增套牌
            m_btnArr[(int)WdscCardSetPnl_BtnIndex.eBtnRet].addEventHandle(onBtnClkRet);       // 完成和返回
        }

        public new void init()
        {
            // 创建编辑卡牌
            m_curEditCardSet = new CardSetCom(m_tuJianData);

            // 加入已经有的卡牌
            foreach (CardGroupItem groupItem in Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardGroupListArr)
            {
                newCardSet(groupItem, false);
            }

            Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardSetChangedDisp.addEventHandle(updateCardSetCardCntText);
            updateCardSetCardCntText();
        }

        public new void dispose()
        {
            delAllCardGroup();
            releaseLeftCardList();
            Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardSetChangedDisp.removeEventHandle(updateCardSetCardCntText);
        }

        protected void updateRetBtnImage(bool bRet)
        {
            if (bRet)
            {
                (m_btnArr[(int)WdscCardSetPnl_BtnIndex.eBtnRet] as AuxDynImageStaticGoButton).auxDynImageStaticGOImage.setImageInfo(CVAtlasName.TuJianDyn, "fanhui_tujian");
                (m_btnArr[(int)WdscCardSetPnl_BtnIndex.eBtnRet] as AuxDynImageStaticGoButton).auxDynImageStaticGOImage.syncUpdateCom();
            }
            else
            {
                (m_btnArr[(int)WdscCardSetPnl_BtnIndex.eBtnRet] as AuxDynImageStaticGoButton).auxDynImageStaticGOImage.setImageInfo(CVAtlasName.TuJianDyn, "wancheng_anniu");
                (m_btnArr[(int)WdscCardSetPnl_BtnIndex.eBtnRet] as AuxDynImageStaticGoButton).auxDynImageStaticGOImage.syncUpdateCom();
            }
        }

        protected void onBtnClkRet(IDispatchObject dispObj)
        {
            back();
        }

        protected void onBtnClkAddTaoPai(IDispatchObject dispObj)
        {
            m_tuJianData.m_wdscCardPnl.toggleCardVisible(false);
            Ctx.m_instance.m_auxUIHelp.m_auxJobSelectData.enterJobSelectMode();
            Ctx.m_instance.m_uiMgr.loadAndShow<UIJobSelect>(UIFormID.eUIJobSelect);
        }

        // 进入编辑卡牌组模式
        public void startEditCardSetMode()
        {
            //让返回变完成
            //可加入卡牌
            m_curTaoPaiMod = WdscmTaoPaiMod.eTaoPaiMod_Editset;
            m_btnArr[(int)WdscCardSetPnl_BtnIndex.eBtnNewCardSet].hide();
            m_cardSetLayoutV.hideLayout();
            updateRetBtnImage(false);
        }

        protected void endEditCardSetMode()
        {
            reqSaveCard();  // 请求保存

            m_curEditCardSet.auxDynImageDynGoButton.hide();        // 当前编辑的卡牌组隐藏
            m_cardSetCardLayoutV.hideLayout();     // 当前卡牌组的卡牌列表隐藏

            m_btnArr[(int)WdscCardSetPnl_BtnIndex.eBtnNewCardSet].show();
            m_cardSetLayoutV.showLayout();
            m_curTaoPaiMod = WdscmTaoPaiMod.eTaoPaiMod_Look;        // 修改卡牌组编辑模式为不可加入卡牌
            updateCardSetCardCntText();
            updateRetBtnImage(true);
        }

        // 向后退回
        public void back()
        {
            switch (m_curTaoPaiMod)
            {
                case WdscmTaoPaiMod.eTaoPaiMod_Editset:
                {
                    endEditCardSetMode();
                }
                break;
                case WdscmTaoPaiMod.eTaoPaiMod_Look:
                {
                    Ctx.m_instance.m_camSys.m_boxCam.back();
                    m_tuJianData.m_form.exit();
                }
                break;
            }
        }

        // 显示
        public void show()
        {
            Ctx.m_instance.m_camSys.m_boxCam.push();
        }

        // 请求保存卡牌
        public void reqSaveCard()
        {
            stReqSaveOneCardGroupUserCmd cmd = new stReqSaveOneCardGroupUserCmd();

            cmd.index = m_curEditCardSet.m_cardGroupItem.m_cardGroup.index;
            if (m_curEditCardSet.bDiffForm(m_curCardSet))   // 如果发生改变
            {
                cmd.count = (ushort)m_curEditCardSet.m_cardGroupItem.m_cardList.Count;
                cmd.id = m_curEditCardSet.m_cardGroupItem.m_cardList;
                UtilMsg.sendMsg(cmd);
            }
        }

        // 保存卡牌成功
        public void psstRetSaveOneCardGroupUserCmd(uint index)
        {
            // 保存当前卡牌
            getCardSetByIndex(index).copyFrom(m_curEditCardSet);
        }

        public CardSetCom getCardSetByIndex(uint index)
        {
            foreach (CardSetCom item in m_cardSetEntityList)
            {
                if (item.m_cardGroupItem.m_cardGroup.index == index)
                {
                    return item;
                }
            }

            return null;
        }

        protected void delAllCardGroup()
        {
            while(m_cardSetEntityList.Count > 0)
            {
                delOneCardGroup(0);
            }
        }

        public void delOneCardGroup(int idx)
        {
            m_cardSetEntityList[idx].removeFromLayout(m_cardSetLayoutV);
            m_cardSetEntityList[idx].dispose();
            m_cardSetEntityList.RemoveAt(idx);
        }

        public void updateLeftCardList()
        {
            releaseLeftCardList();
            if (m_curEditCardSet != null && m_curEditCardSet.m_cardGroupItem.m_cardList != null)
            {
                int idx = 0;
                while (idx < m_curEditCardSet.m_cardGroupItem.m_cardList.Count)
                {
                    createCardSetCard(m_curEditCardSet.m_cardGroupItem.m_cardList[idx]);
                    ++idx;
                }
            }

            updateCardSetCardCntText();
        }

        public void releaseLeftCardList()
        {
            int idx = 0;
            while (idx < m_cardList.Count)
            {
                m_cardList[idx].removeFromLayout(m_cardSetCardLayoutV);
                m_cardList[idx].dispose();
                
                ++idx;
            }
            m_cardList.Clear();
            m_id2CardComDic.Clear();
        }

        public bool bCurEditCardSet(CardSetCom cardSet)
        {
            return m_curEditCardSet.Equals(cardSet);
        }

        // 所有卡牌组回到列表显示状态
        public void goback()
        {
            foreach (CardSetCom taoPai in m_cardSetEntityList)
            {
                taoPai.auxDynImageDynGoButton.show();
            }
        }

        public void hideAllCardSet()
        {
            foreach (CardSetCom item in m_cardSetEntityList)
            {
                item.auxDynImageDynGoButton.hide();
            }
        }

        protected void updateCardSetCardCntText(IDispatchObject dispObj = null)
        {
            if (m_curTaoPaiMod == WdscmTaoPaiMod.eTaoPaiMod_Look)
            {
                m_cardSetCardCntText.text = string.Format("{0}/{1}", Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardGroupListArr.Count, 9);   // 设置显示套牌数量
            }
            else
            {
                m_cardSetCardCntText.text = string.Format("{0}/{1}\n卡牌", m_cardList.Count, 30);
            }
        }

        // 添加一张卡牌到编辑的卡组
        public void addCard2EditCardSet(uint cardID)
        {
            if (m_curEditCardSet.m_cardGroupItem.m_cardList == null)
            {
                m_curEditCardSet.m_cardGroupItem.m_cardList = new List<uint>();
            }
            if (!bNameNumEqualTwo(cardID))
            {
                if (m_curEditCardSet.m_cardGroupItem.m_cardList.IndexOf(cardID) == -1)
                {
                    if (m_curEditCardSet.m_cardGroupItem.m_cardList.Count < 30)
                    {
                        m_curEditCardSet.m_cardGroupItem.m_cardList.Add(cardID);
                        // 继续添加显示
                        createCardSetCard(cardID);
                    }
                }
            }

            updateCardSetCardCntText();
        }

        // 创建一个卡牌组中的卡牌
        public void createCardSetCard(uint cardID)
        {
            if (!m_id2CardComDic.ContainsKey(cardID))
            {
                TableCardItemBody cardItem;
                TableItemBase tableItem;
                tableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, cardID);
                cardItem = tableItem.m_itemBody as TableCardItemBody;

                TuJianCardSetCardItemCom cardCom = new TuJianCardSetCardItemCom();
                m_cardList.Add(cardCom);
                m_id2CardComDic[cardID] = cardCom;
                cardCom.cardItem = cardItem;
                cardCom.add2Layout(m_cardSetCardLayoutV);
            }
            else
            {
                m_id2CardComDic[cardID].numImage.show();
            }
        }

        // 新建卡牌组， bEnterEdit 是否立即进入编辑模式
        public void newCardSet(CardGroupItem cardSet, bool bEnterEdit = true)
        {
            CardSetCom taopai = new CardSetCom(m_tuJianData);
            m_cardSetEntityList.Add(taopai);
            taopai.initByData(cardSet);
            taopai.add2Layout(m_cardSetLayoutV);
            if (bEnterEdit)
            {
                m_curEditCardSet.startEdit(taopai);
            }
        }

        // 编辑当前卡牌
        public void editCurCardSet()
        {
            m_curCardSet.reqCardListAndStartEdit();
        }

        public void delCardSet()
        {
            m_curCardSet.delCardSet();
        }

        // 一个套牌的卡牌列表，index 指明是哪个套牌的
        public void psstRetOneCardGroupInfoUserCmd(uint index, List<uint> list)
        {
            if(m_curTaoPaiMod == WdscmTaoPaiMod.eTaoPaiMod_Editset)   // 如果在编辑模式
            {
                if(m_curEditCardSet.m_cardGroupItem.m_cardGroup.index == index)       // 如果当前正在编辑这个套牌
                {
                    CardSetCom findSet = null;
                    foreach(CardSetCom cardSet in m_cardSetEntityList)
                    {
                        if(cardSet.m_cardGroupItem.m_cardGroup.index == index)
                        {
                            findSet = cardSet;
                            break;
                        }
                    }
                    if (findSet != null)
                    {
                        m_curEditCardSet.startEdit(findSet);
                    }
                }
            }
        }

        // 已经添加的名字的数量是否是两个
        protected bool bNameNumEqualTwo(uint cardID)
        {
            TableCardItemBody cardItem;
            TableCardItemBody cardListItem;
            TableItemBase tableItem;
            tableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, cardID);
            cardItem = tableItem.m_itemBody as TableCardItemBody;

            int cardNum = 0;

            foreach(uint cardidx in m_curEditCardSet.m_cardGroupItem.m_cardList)
            {
                tableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, cardidx);
                cardListItem = tableItem.m_itemBody as TableCardItemBody;
                if(cardListItem.m_name == cardItem.m_name)
                {
                    ++cardNum;
                }
            }

            return cardNum == 2;
        }
    }
}
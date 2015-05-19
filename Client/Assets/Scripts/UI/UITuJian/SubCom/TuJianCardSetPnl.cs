using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SDK.Common;
using SDK.Lib;
using Game.Msg;
using UnityEngine.UI;

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
        public List<TuJianCardListItem> m_cardList = new List<TuJianCardListItem>();// 当前在编辑的卡组中的卡牌列表
        public List<CardSetCom> m_cardSetEntityList = new List<CardSetCom>();      // 当前已经有的卡牌组

        public UIGrid m_leftCardList = new UIGrid();            // 左边的卡牌列表
        public AuxLayoutH m_cardSetLayoutH;

        public WdscmTaoPaiMod m_curTaoPaiMod;          // 当前套牌模式

        protected Button[] m_btnArr = new Button[(int)WdscCardSetPnl_BtnIndex.eBtnTotal];
        protected Text m_cardSetCardCntText;        // 卡组中卡牌的数量

        public GameObject m_topEditCardPosGo;
        public CardSetCom m_curCardSet;                  // 当前操作的卡组 ID

        public TuJianCardSetPnl(TuJianData data) :
            base(data)
        {
            
        }

        public new void findWidget()
        {
            m_topEditCardPosGo = UtilApi.TransFindChildByPObjAndPath(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.GoTopEditCardPos);

            m_leftCardList.setGameObject(UtilApi.TransFindChildByPObjAndPath(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.BtnAddCardList));
            m_leftCardList.cellWidth = 1.172f;
            m_leftCardList.cellHeight = 0.445f;
            m_leftCardList.m_hideZPos = -20f;
            m_leftCardList.hideGrid();              // 默认隐藏

            m_cardSetLayoutH = new AuxLayoutH();
            m_cardSetLayoutH.elemWidth = 285;
            m_cardSetLayoutH.elemHeight = 78;
            m_cardSetLayoutH.pntGo = UtilApi.TransFindChildByPObjAndPath(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.CardSetListCont);

            m_btnArr[(int)WdscCardSetPnl_BtnIndex.eBtnNewCardSet] = UtilApi.getComByP<Button>(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.BtnNewCardSet);
            UtilApi.addEventHandle(m_btnArr[(int)WdscCardSetPnl_BtnIndex.eBtnNewCardSet], onBtnClkAddTaoPai);       // 新增套牌

            m_btnArr[(int)WdscCardSetPnl_BtnIndex.eBtnRet] = UtilApi.getComByP<Button>(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.BtnRet);
            m_cardSetCardCntText = UtilApi.getComByP<Text>(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.TextCardSetCardCnt);
        }

        public new void addEventHandle()
        {
            UtilApi.addEventHandle(m_btnArr[(int)WdscCardSetPnl_BtnIndex.eBtnRet], onBtnClkRet);       // 新增套牌
        }

        public new void init()
        {
            insEditCardGroup();

            // 加入已经有的卡牌
            foreach (CardGroupItem groupItem in Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardGroupListArr)
            {
                newCardSet(groupItem, false);
            }
        }

        protected void onBtnClkRet()
        {
            m_tuJianData.m_wdscCardSetPnl.back();
        }

        protected void onBtnClkAddTaoPai()
        {
            Ctx.m_instance.m_uiMgr.loadAndShow<UIJobSelect>(UIFormID.eUIJobSelect);
        }

        // 进入编辑卡牌组模式
        public void startEditCardSetMode()
        {
            //让返回变完成
            //可加入卡牌
            m_curTaoPaiMod = WdscmTaoPaiMod.eTaoPaiMod_Editset;
            UtilApi.SetActive(m_btnArr[(int)WdscCardSetPnl_BtnIndex.eBtnNewCardSet].gameObject, false);
            m_cardSetLayoutH.hideLayout();
        }

        void endEditCardSetMode()
        {
            reqSaveCard();  // 请求保存

            m_curEditCardSet.hideCardSet();        // 当前编辑的卡牌组隐藏
            m_leftCardList.hideGrid();          // 当前卡牌组的卡牌列表隐藏

            UtilApi.SetActive(m_btnArr[(int)WdscCardSetPnl_BtnIndex.eBtnNewCardSet].gameObject, true);
            m_cardSetLayoutH.showLayout();

            m_curTaoPaiMod = WdscmTaoPaiMod.eTaoPaiMod_Look;        // 修改卡牌组编辑模式为不可加入卡牌
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
            cmd.count = (ushort)m_curEditCardSet.m_cardGroupItem.m_cardList.Count;
            cmd.id = m_curEditCardSet.m_cardGroupItem.m_cardList;

            UtilMsg.sendMsg(cmd);
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

        public void delOneCardGroup(int p)
        {
            m_cardSetEntityList.RemoveAt(p);
            m_cardSetLayoutH.removeAndDestroyElem(m_cardSetEntityList[p].getGameObject(), true);
        }

        public void updateLeftCardList()
        {
            releaseLeftCardList();
            if (m_curEditCardSet != null && m_curEditCardSet.m_cardGroupItem.m_cardList != null)
            {
                int idx = 0;
                while (idx < m_curEditCardSet.m_cardGroupItem.m_cardList.Count)
                {
                    m_cardList.Add(m_tuJianData.createCard(m_curEditCardSet.m_cardGroupItem.m_cardList[idx]));
                    m_leftCardList.AddChild(m_cardList[idx].getGameObject().transform);
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
                m_cardList[idx].getGameObject().transform.parent = null;
                UtilApi.Destroy(m_cardList[idx].getGameObject());
                ++idx;
            }
            m_cardList.Clear();
        }

        public bool bCurEditCardSet(CardSetCom cardSet)
        {
            return m_curEditCardSet.Equals(cardSet);
        }

        // 所有卡牌组回到列表显示状态
        public void goback()
        {
            foreach (CardSetCom taoPai in m_tuJianData.m_wdscCardSetPnl.m_cardSetEntityList)
            {
                taoPai.showCardSet();
            }
        }

        public void hideAllCardSet()
        {
            foreach (CardSetCom item in m_tuJianData.m_wdscCardSetPnl.m_cardSetEntityList)
            {
                item.hideCardSet();
            }
        }

        protected void updateCardSetCardCntText()
        {
            m_cardSetCardCntText.text = string.Format("{0}/{1}\n卡牌", m_cardList.Count, 30);
        }

        // 添加一张卡牌到编辑的卡组
        public void addCard2EditCardSet(uint cardID)
        {
            if (m_tuJianData.m_wdscCardSetPnl.m_curEditCardSet.m_cardGroupItem.m_cardList == null)
            {
                m_tuJianData.m_wdscCardSetPnl.m_curEditCardSet.m_cardGroupItem.m_cardList = new List<uint>();
            }
            if (m_tuJianData.m_wdscCardSetPnl.m_curEditCardSet.m_cardGroupItem.m_cardList.IndexOf(cardID) == -1)
            {
                if (m_tuJianData.m_wdscCardSetPnl.m_curEditCardSet.m_cardGroupItem.m_cardList.Count < 30)
                {
                    m_tuJianData.m_wdscCardSetPnl.m_curEditCardSet.m_cardGroupItem.m_cardList.Add(cardID);
                    // 继续添加显示
                    m_tuJianData.m_wdscCardSetPnl.m_cardList.Add(m_tuJianData.createCard(cardID));
                    m_tuJianData.m_wdscCardSetPnl.m_leftCardList.AddChild(m_tuJianData.m_wdscCardSetPnl.m_cardList[m_tuJianData.m_wdscCardSetPnl.m_cardList.Count - 1].getGameObject().transform);
                }
            }

            updateCardSetCardCntText();
        }

        // 新建卡牌组， bEnterEdit 是否立即进入编辑模式
        public void newCardSet(CardGroupItem cardSet, bool bEnterEdit = true)
        {
            GameObject go = Ctx.m_instance.m_uiPrefabMgr.syncGet<UIPrefabRes>(TuJianPath.CardSetPrefabPath).InstantiateObject(TuJianPath.CardSetPrefabPath);
            m_cardSetLayoutH.addElem(go, true);

            CardSetCom taopai = new CardSetCom(m_tuJianData);
            m_cardSetEntityList.Add(taopai);
            taopai.setGameObject(go);
            taopai.createNew(cardSet, bEnterEdit);
        }

        // 创建一个编辑的卡牌组
        public void insEditCardGroup()
        {
            GameObject go = Ctx.m_instance.m_uiPrefabMgr.syncGet<UIPrefabRes>(TuJianPath.CardSetPrefabPath).InstantiateObject(TuJianPath.CardSetPrefabPath);
            UtilApi.SetParent(go, m_tuJianData.m_sceneGo);

            m_curEditCardSet = new CardSetCom(m_tuJianData);
            m_curEditCardSet.setGameObject(go);
            m_curEditCardSet.hideCardSet();
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
    }
}
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
    public class WdscCardSetPnl : InterActiveEntity
    {
        public SceneWDSCData m_sceneWDSCData;

        public CardSetCom m_curEditCardSet = null;                 // 当前正在编辑的卡牌组，注意这个不是指向编辑的指针，而是拷贝的数据
        public List<SCCardListItem> m_cardList = new List<SCCardListItem>();// 当前在编辑的卡组中的卡牌列表
        public List<CardSetCom> m_cardSetEntityList = new List<CardSetCom>();      // 当前已经有的卡牌组

        public UIGrid m_leftCardList = new UIGrid();            // 左边的卡牌列表
        public UIGrid m_leftCardGroupList = new UIGrid();       // 左边的卡牌组列表

        //public NewCardSetBtn m_newCardSetBtn = new NewCardSetBtn();

        public WdscmTaoPaiMod m_curTaoPaiMod;          // 当前套牌模式
        public Vector3 showpostion = new Vector3(-1.532529f, 1.805149f, 0.860475f);

        protected Button[] m_btnArr = new Button[(int)WdscCardSetPnl_BtnIndex.eBtnTotal];
        protected Text m_cardSetCardCntText;        // 卡组中卡牌的数量

        protected GameObject m_setBtnGo;
        public GameObject m_topEditCardPosGo;

        public override void Start()
        {
            m_setBtnGo = UtilApi.GoFindChildByPObjAndName(SceneSCPath.SetBtn);
            m_topEditCardPosGo = UtilApi.GoFindChildByPObjAndName(SceneSCPath.GoTopEditCardPos);

            m_leftCardList.setGameObject(UtilApi.GoFindChildByPObjAndName(SceneSCPath.BtnAddCardList));
            m_leftCardList.cellWidth = 1.172f;
            m_leftCardList.cellHeight = 0.445f;
            m_leftCardList.m_hideZPos = -20f;
            m_leftCardList.hideGrid();              // 默认隐藏

            m_leftCardGroupList.setGameObject(UtilApi.GoFindChildByPObjAndName(SceneSCPath.BtnAddCardSetList));
            m_leftCardGroupList.cellWidth = 1.172f;
            m_leftCardGroupList.cellHeight = 0.445f;
            m_leftCardGroupList.m_hideZPos = -20f;

            //m_newCardSetBtn.m_sceneWDSCData = m_sceneWDSCData;
            //m_newCardSetBtn.setGameObject(UtilApi.GoFindChildByPObjAndName(SceneSCPath.BtnAddCardSet));

            insEditCardGroup();

            //UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName(SceneSCPath.BtnClose), onBtnClkRet);       // 返回
            //UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName(SceneSCPath.BtnAddCardSet), onBtnClkAddTaoPai);       // 新增套牌

            m_btnArr[(int)WdscCardSetPnl_BtnIndex.eBtnNewCardSet] = UtilApi.getComByP<Button>(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.BtnNewCardSet);
            UtilApi.addEventHandle(m_btnArr[(int)WdscCardSetPnl_BtnIndex.eBtnNewCardSet], onBtnClkAddTaoPai);       // 新增套牌

            m_btnArr[(int)WdscCardSetPnl_BtnIndex.eBtnRet] = UtilApi.getComByP<Button>(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.BtnRet);
            UtilApi.addEventHandle(m_btnArr[(int)WdscCardSetPnl_BtnIndex.eBtnRet], onBtnClkRet);       // 新增套牌

            m_cardSetCardCntText = UtilApi.getComByP<Text>(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.TextCardSetCardCnt);

            // 加入已经有的卡牌
            foreach (CardGroupItem groupItem in Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardGroupListArr)
            {
                newCardSet(groupItem, false);
            }
        }

        protected void onBtnClkRet()
        {
            m_sceneWDSCData.m_wdscCardSetPnl.back();
        }

        protected void onBtnClkAddTaoPai()
        {
            UISceneMoShi uiMS = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneMoShi>(UISceneFormID.eUISceneMoShi);
            if (uiMS == null)
            {
                Ctx.m_instance.m_uiSceneMgr.loadSceneForm<UISceneMoShi>(UISceneFormID.eUISceneMoShi);
            }
            uiMS = Ctx.m_instance.m_uiSceneMgr.showSceneForm(UISceneFormID.eUISceneMoShi) as UISceneMoShi;
            uiMS.AddNewCardSet();
        }

        // 进入编辑卡牌组模式
        public void startEditCardSetMode()
        {
            //让返回变完成
            //可加入卡牌
            m_curTaoPaiMod = WdscmTaoPaiMod.eTaoPaiMod_Editset;
            //m_newCardSetBtn.hide();    // hide 新建卡牌按钮
            UtilApi.SetActive(m_btnArr[(int)WdscCardSetPnl_BtnIndex.eBtnNewCardSet].gameObject, false);
            m_leftCardGroupList.hideGrid();     // 遍历所有的卡牌集合，进行隐藏
        }

        void endEditCardSetMode()
        {
            reqSaveCard();  // 请求保存

            m_curEditCardSet.hideCardSet();        // 当前编辑的卡牌组隐藏
            m_leftCardList.hideGrid();          // 当前卡牌组的卡牌列表隐藏

            //m_newCardSetBtn.goBack();          // 新建卡牌按钮显示
            UtilApi.SetActive(m_btnArr[(int)WdscCardSetPnl_BtnIndex.eBtnNewCardSet].gameObject, true);
            m_leftCardGroupList.showGrid();     // 显示所有卡牌
            m_sceneWDSCData.m_pClassFilterPnl.gotoBack();                // 显示职业过滤面板中的职业选择标签

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
                    iTween.MoveBy(gameObject, iTween.Hash(iT.MoveBy.amount, Vector3.down * 10, iT.MoveBy.time, 0.1f, iT.MoveBy.delay, 1));
                }
                break;
            }
        }

        // 显示
        public void show()
        {
            transform.position = showpostion;
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
            UtilApi.Destroy(m_cardSetEntityList[p].getGameObject());
            m_cardSetEntityList.RemoveAt(p);
            m_leftCardGroupList.Reposition();
            //m_newCardSetBtn.updatePos();
        }

        public void updateLeftCardList()
        {
            releaseLeftCardList();
            if (m_curEditCardSet != null && m_curEditCardSet.m_cardGroupItem.m_cardList != null)
            {
                int idx = 0;
                while (idx < m_curEditCardSet.m_cardGroupItem.m_cardList.Count)
                {
                    m_cardList.Add(m_sceneWDSCData.createCard(m_curEditCardSet.m_cardGroupItem.m_cardList[idx]));
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
            foreach (CardSetCom taoPai in m_sceneWDSCData.m_wdscCardSetPnl.m_cardSetEntityList)
            {
                taoPai.showCardSet();
            }
        }

        public void hideAllCardSet()
        {
            foreach (CardSetCom item in m_sceneWDSCData.m_wdscCardSetPnl.m_cardSetEntityList)
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
            if (m_sceneWDSCData.m_wdscCardSetPnl.m_curEditCardSet.m_cardGroupItem.m_cardList == null)
            {
                m_sceneWDSCData.m_wdscCardSetPnl.m_curEditCardSet.m_cardGroupItem.m_cardList = new List<uint>();
            }
            if (m_sceneWDSCData.m_wdscCardSetPnl.m_curEditCardSet.m_cardGroupItem.m_cardList.IndexOf(cardID) == -1)
            {
                if (m_sceneWDSCData.m_wdscCardSetPnl.m_curEditCardSet.m_cardGroupItem.m_cardList.Count < 30)
                {
                    m_sceneWDSCData.m_wdscCardSetPnl.m_curEditCardSet.m_cardGroupItem.m_cardList.Add(cardID);
                    // 继续添加显示
                    m_sceneWDSCData.m_wdscCardSetPnl.m_cardList.Add(m_sceneWDSCData.createCard(cardID));
                    m_sceneWDSCData.m_wdscCardSetPnl.m_leftCardList.AddChild(m_sceneWDSCData.m_wdscCardSetPnl.m_cardList[m_sceneWDSCData.m_wdscCardSetPnl.m_cardList.Count - 1].getGameObject().transform);
                }
            }

            updateCardSetCardCntText();
        }

        // 新建卡牌组， bEnterEdit 是否立即进入编辑模式
        public void newCardSet(CardGroupItem s, bool bEnterEdit = true)
        {
            GameObject go = Ctx.m_instance.m_modelMgr.getCardGroupModel().InstantiateObject("");
            m_leftCardGroupList.AddChild(go.transform);//插入到最后一位

            CardSetCom taopai = new CardSetCom();
            taopai.m_sceneWDSCData = m_sceneWDSCData;
            m_cardSetEntityList.Add(taopai);
            taopai.setGameObject(go);
            taopai.createNew(s, bEnterEdit);
        }

        public void insEditCardGroup()
        {
            GameObject go = Ctx.m_instance.m_modelMgr.getCardGroupModel().InstantiateObject("");
            UtilApi.SetParent(go, m_setBtnGo);

            m_curEditCardSet = new CardSetCom();
            m_curEditCardSet.m_sceneWDSCData = m_sceneWDSCData;
            m_curEditCardSet.setGameObject(go);
            m_curEditCardSet.hideCardSet();
        }
    }
}
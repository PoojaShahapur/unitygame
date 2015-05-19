using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SDK.Common;
using Game.Msg;
using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief 一个卡组
     */
    public class CardSetCom : TuJianPnlBase
    {
        public CardGroupItem m_cardGroupItem;             // 当前卡牌组对应的数据信息
        public int m_cardID;                // 卡组 ID
        public GameObject m_cardSetGo;

        public CardSetCom(TuJianData data):
           base(data)
        {

        }

        public new void findWidget()
        {
            m_cardSetGo = UtilApi.TransFindChildByPObjAndPath(m_tuJianData.m_form.m_GUIWin.m_uiRoot, getPath());
        }

        public new void addEventHandle()
        {
            UtilApi.addEventHandle(m_tuJianData.m_form.m_GUIWin.m_uiRoot, getPath(), OnMouseClick);
        }

        public GameObject getGameObject()
        {
            return m_cardSetGo;
        }

        public void setGameObject(GameObject go_)
        {
            m_cardSetGo = go_;
        }

        public string getPath()
        {
            string cardPath = string.Format("{0}/CardSet_Copy_{1}", TuJianPath.CardSetListCont, m_cardID);
            return cardPath;
        }

        public void OnMouseClick()
        {
            m_tuJianData.m_wdscCardSetPnl.m_curCardSet = this;
            m_tuJianData.m_cardSetEditPnl.showCardSetEdit();
        }

        public void reqCardListAndStartEdit()
        {
            m_cardGroupItem.reqCardList();
            m_tuJianData.m_wdscCardSetPnl.m_curEditCardSet.startEdit(this);
        }

        public void enterEditorMode()
        {
            moveSelf2Top();     // 移动自己到顶端
            m_tuJianData.m_wdscCardSetPnl.startEditCardSetMode();        // 卡牌组面板进入编辑模式
        }

        public void setInfo(CardGroupItem cardSet)
        {
            m_cardGroupItem = cardSet;
            if (cardSet.m_cardGroup.index == uint.MaxValue)
            {
                cardSet.m_cardGroup.name = "新建套牌";
            }
            setName(cardSet.m_cardGroup.name);
        }

        protected void setClass(EnPlayerCareer c)
        {
            m_cardGroupItem.m_cardGroup.occupation = (uint)c;
        }

        public void createNew(CardGroupItem s, bool bEnterEdit = true)
        {
            setInfo(s);
            if (bEnterEdit)
            {
                m_tuJianData.m_wdscCardSetPnl.m_curEditCardSet.startEdit(this);
            }
        }

        public void startEdit(CardSetCom cardSet)
        {
            // 卡牌组处理
            copyAndInitData(cardSet);
            enterEditorMode();
            // 当前编辑的卡牌列表处理
            m_tuJianData.m_wdscCardSetPnl.m_leftCardList.showGrid();
            m_tuJianData.m_wdscCardSetPnl.updateLeftCardList();
        }

        public void copyAndInitData(CardSetCom cardSet)
        {
            m_tuJianData.m_wdscCardSetPnl.m_curEditCardSet.copyFrom(cardSet);
            m_tuJianData.m_wdscCardSetPnl.m_curEditCardSet.setInfo(m_tuJianData.m_wdscCardSetPnl.m_curEditCardSet.m_cardGroupItem);      // 设置卡牌的显示信息
        }

        protected void setName(string n)
        {
            m_cardGroupItem.m_cardGroup.name = n;
        }

        protected void setPic(Material m)
        {
#if UNITY_5
            m_cardSetGo.transform.FindChild("pic").GetComponent<Renderer>().material = m;
#elif UNITY_4_6
            m_cardSetGo.transform.FindChild("pic").renderer.material = m;
#endif
        }

        // 将编辑的卡牌放到顶端
        protected void moveSelf2Top()
        {
            m_cardSetGo.transform.localPosition = m_tuJianData.m_wdscCardSetPnl.m_topEditCardPosGo.transform.localPosition;
            showCardSet();
        }

        public void hideCardSet()
        {
            UtilApi.SetActive(m_cardSetGo, false);
        }

        public void showCardSet()
        {
            UtilApi.SetActive(m_cardSetGo, true);
        }

        public void copyFrom(CardSetCom cards)
        {
            if (m_cardGroupItem == null)
            {
                m_cardGroupItem = new CardGroupItem();
            }
            this.m_cardGroupItem.copyFrom(cards.m_cardGroupItem);
        }

        public void delCardSet()
        {
            stReqDeleteOneCardGroupUserCmd cmd = new stReqDeleteOneCardGroupUserCmd();
            cmd.index = m_cardGroupItem.m_cardGroup.index;
            UtilMsg.sendMsg(cmd);
        }
    }
}
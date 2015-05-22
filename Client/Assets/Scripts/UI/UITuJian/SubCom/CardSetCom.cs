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
        public GameObject m_sceneGo;

        protected ImageItem m_imageItem;
        protected UIPrefabRes m_uiPrefabRes;

        public CardSetCom(TuJianData data):
           base(data)
        {

        }

        public void dispose()
        {
            Ctx.m_instance.m_uiPrefabMgr.unload(m_uiPrefabRes.GetPath());
            Ctx.m_instance.m_atlasMgr.unloadImage(m_imageItem);
            UtilApi.Destroy(m_sceneGo);
        }

        public new void findWidget()
        {
            m_sceneGo = UtilApi.TransFindChildByPObjAndPath(m_tuJianData.m_form.m_GUIWin.m_uiRoot, getPath());
        }

        public new void addEventHandle()
        {
            UtilApi.addEventHandle(m_tuJianData.m_form.m_GUIWin.m_uiRoot, getPath(), OnMouseClick);
        }

        public GameObject getGameObject()
        {
            return m_sceneGo;
        }

        public string getPath()
        {
            string cardPath = string.Format("{0}/CardSet_{1}", TuJianPath.CardSetListCont, m_cardGroupItem.m_cardGroup.index);
            return cardPath;
        }

        public string getName()
        {
            string cardName = string.Format("CardSet_{0}", m_cardGroupItem.m_cardGroup.index);
            return cardName;
        }

        public void OnMouseClick()
        {
            m_tuJianData.m_wdscCardSetPnl.m_curCardSet = this;
            Ctx.m_instance.m_uiMgr.loadAndShow<UITuJianCardSetMenu>(UIFormID.eUITuJianCardSetMenu);
        }

        public void reqCardListAndStartEdit()
        {
            m_cardGroupItem.reqCardList();
            m_tuJianData.m_wdscCardSetPnl.m_curEditCardSet.startEdit(this);
        }

        public void enterEditorMode()
        {
            showCardSet();
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
            updateImage();
        }

        protected void setClass(EnPlayerCareer c)
        {
            m_cardGroupItem.m_cardGroup.occupation = (uint)c;
        }

        public void createNew(CardGroupItem cardSet, bool bEnterEdit = true)
        {
            createSceneGo();
            setInfo(cardSet);
            UtilApi.setGOName(m_sceneGo, getName());

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
            m_tuJianData.m_wdscCardSetPnl.m_cardSetCardLayoutV.showLayout();
            m_tuJianData.m_wdscCardSetPnl.updateLeftCardList();
        }

        public void copyAndInitData(CardSetCom cardSet)
        {
            m_tuJianData.m_wdscCardSetPnl.m_curEditCardSet.copyFrom(cardSet);
            m_tuJianData.m_wdscCardSetPnl.m_curEditCardSet.setInfo(cardSet.m_cardGroupItem);      // 设置卡牌的显示信息
        }

        protected void setName(string n)
        {
            m_cardGroupItem.m_cardGroup.name = n;
        }

        protected void setPic(Material m)
        {
#if UNITY_5
            m_sceneGo.transform.FindChild("pic").GetComponent<Renderer>().material = m;
#elif UNITY_4_6
            m_sceneGo.transform.FindChild("pic").renderer.material = m;
#endif
        }

        public void hideCardSet()
        {
            UtilApi.SetActive(m_sceneGo, false);
        }

        public void showCardSet()
        {
            UtilApi.SetActive(m_sceneGo, true);
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

        public void add2Layout(AuxLayoutV cardLayoutV)
        {
            cardLayoutV.addElem(m_sceneGo, true);

            this.findWidget();
            this.addEventHandle();
        }

        public void add2Node(GameObject go_)
        {
            UtilApi.SetParent(m_sceneGo, go_, false);
        }

        public void createSceneGo()
        {
            m_uiPrefabRes = Ctx.m_instance.m_uiPrefabMgr.syncGet<UIPrefabRes>(TuJianPath.CardSetPrefabPath);
            m_sceneGo = m_uiPrefabRes.InstantiateObject(TuJianPath.CardSetPrefabPath);
        }

        public void updateImage()
        {
            m_imageItem = Ctx.m_instance.m_atlasMgr.getAndAsyncLoadImage("Atlas/TuJianDyn.asset", m_cardGroupItem.m_tableJobItemBody.m_cardSetRes);
            m_imageItem.setGoImage(m_sceneGo);
        }
    }
}
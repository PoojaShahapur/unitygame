using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SDK.Lib;
using Game.Msg;

namespace Game.UI
{
    /**
     * @brief 一个卡组
     */
    public class CardSetCom : TuJianPnlBase
    {
        public CardGroupItem m_cardGroupItem;             // 当前卡牌组对应的数据信息
        protected AuxDynImageDynGoButton m_auxDynImageDynGoButton;
        protected AuxLabel m_nameLbl;

        public CardSetCom(TuJianData data):
           base(data)
        {
            
        }

        new public void dispose()
        {
            if(m_auxDynImageDynGoButton != null)
            {
                m_auxDynImageDynGoButton.dispose();
            }
        }

        public AuxDynImageDynGoButton auxDynImageDynGoButton
        {
            get
            {
                return m_auxDynImageDynGoButton;
            }
        }

        public new void findWidget()
        {
            
        }

        public new void addEventHandle()
        {
            m_auxDynImageDynGoButton.addEventHandle(OnMouseClick);
        }

        public void OnMouseClick(IDispatchObject dispObj)
        {
            m_tuJianData.m_wdscCardSetPnl.m_curCardSet = this;
            Ctx.m_instance.m_auxUIHelp.m_auxTuJian.m_eTuJianMenu = ETuJianMenu.eCardSet;
            Ctx.m_instance.m_uiMgr.loadAndShow(UIFormID.eUITuJianTop);
        }

        public void reqCardListAndStartEdit()
        {
            m_cardGroupItem.reqCardList();
            m_tuJianData.m_wdscCardSetPnl.m_curEditCardSet.startEdit(this);
        }

        public void enterEditorMode()
        {
            m_auxDynImageDynGoButton.show();
            m_tuJianData.m_wdscCardSetPnl.startEditCardSetMode();        // 卡牌组面板进入编辑模式
        }

        // 使用数据初始化
        public void initByData(CardGroupItem cardSet)
        {
            m_cardGroupItem = cardSet;
            createSceneGo();
            updateInfo();
        }

        protected void updateInfo()
        {
            m_nameLbl.text = m_cardGroupItem.m_cardGroup.name;
        }

        public void startEdit(CardSetCom cardSet)
        {
            // 卡牌组处理
            copyData(cardSet);
            enterEditorMode();
            // 当前编辑的卡牌列表处理
            m_tuJianData.m_wdscCardSetPnl.m_cardSetCardLayoutV.showLayout();
            m_tuJianData.m_wdscCardSetPnl.updateLeftCardList();
        }

        public void copyData(CardSetCom cardSet)
        {
            m_tuJianData.m_wdscCardSetPnl.m_curEditCardSet.copyFrom(cardSet);
            m_tuJianData.m_wdscCardSetPnl.m_curEditCardSet.add2Node(m_tuJianData.m_wdscCardSetPnl.m_topEditCardPosGo);
        }

        public void copyFrom(CardSetCom cards)
        {
            if (m_cardGroupItem == null)
            {
                m_cardGroupItem = new CardGroupItem();
            }
            this.m_cardGroupItem.copyFrom(cards.m_cardGroupItem);
            createSceneGo();
            updateInfo();
        }

        public void delCardSet()
        {
            stReqDeleteOneCardGroupUserCmd cmd = new stReqDeleteOneCardGroupUserCmd();
            cmd.index = m_cardGroupItem.m_cardGroup.index;
            UtilMsg.sendMsg(cmd);
        }

        public void add2Layout(AuxLayoutV cardLayoutV)
        {
            cardLayoutV.addElem(m_auxDynImageDynGoButton.selfGo, true);

            this.findWidget();
            this.addEventHandle();
        }

        public void removeFromLayout(AuxLayoutV cardLayoutV)
        {
            cardLayoutV.removeElem(m_auxDynImageDynGoButton.selfGo, true);
        }

        public void add2Node(GameObject go_)
        {
            UtilApi.SetParent(m_auxDynImageDynGoButton.selfGo, go_, false);
        }

        public void createSceneGo()
        {
            if(m_auxDynImageDynGoButton == null)
            {
                m_auxDynImageDynGoButton = new AuxDynImageDynGoButton();
            }

            m_auxDynImageDynGoButton.prefabPath = TuJianPath.CardSetPrefabPath;
            m_auxDynImageDynGoButton.setImageInfo(CVAtlasName.TuJianDyn, m_cardGroupItem.m_tableJobItemBody.m_cardSetRes);
            m_auxDynImageDynGoButton.addImageLoadedHandle(onImageLoaded);
            m_auxDynImageDynGoButton.syncUpdateCom();
        }

        protected void onImageLoaded(IDispatchObject dispObj)
        {
            AuxComponent imageCom = dispObj as AuxComponent;
            m_nameLbl = new AuxLabel(imageCom.selfGo, TuJianPath.CardSetNameText);
        }

        public bool bDiffForm(CardSetCom rhv)
        {
            return m_cardGroupItem.bDiffForm(rhv.m_cardGroupItem);
        }
    }
}
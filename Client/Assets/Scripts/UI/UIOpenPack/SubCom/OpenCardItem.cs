using SDK.Lib;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /**
     * @brief 开卡包卡
     */
    public class OpenCardItem : CanOutCardRender
    {
        protected bool m_bOpened;       // 卡牌是否翻转
        protected DopeSheetAni m_fanKaAni;
        protected SpriteAni m_spriteAni;
        protected GameObject m_guangyunImg;
        public OpenCardItem() :
            base(null, (int)CardSubPartType.eTotal)
        {
            m_bOpened = false;
        }

        public void addEventHandle()
        {
            this.clickEntityDisp.addEventHandle(null, onClick);
        }

        public override void dispose()
        {
            base.dispose();

            if(m_fanKaAni != null)
            {
                m_fanKaAni.dispose();
                m_fanKaAni = null;
            }

            if (m_spriteAni != null)
            {
                m_spriteAni.dispose();
                m_spriteAni = null;
            }
        }

        override public void setIdAndPnt(uint objId, GameObject pntGo_)
        {
            base.setIdAndPnt(objId, pntGo_);
            TableCardItemBody tableBody = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, objId).m_itemBody as TableCardItemBody;
            updateLeftAttr(tableBody);
            UtilApi.setLayer(m_model.selfGo, Config.UIModelLayer);

            m_guangyunImg = UtilApi.TransFindChildByPObjAndPath(m_model.selfGo, "UIRoot/guangyunImg");
            m_spriteAni = Ctx.m_instance.m_spriteAniMgr.createAndAdd();
            m_spriteAni.selfGo = m_guangyunImg;
            if (0 == tableBody.m_quality)
                m_spriteAni.tableID = 18;
            else if (1 == tableBody.m_quality)
                m_spriteAni.tableID = 19;
            else if (2 == tableBody.m_quality)
                m_spriteAni.tableID = 20;
            else if (3 == tableBody.m_quality)
                m_spriteAni.tableID = 21;
            else
                m_spriteAni.tableID = 22;
            m_spriteAni.bLoop = true;
            m_spriteAni.playEndEventDispatch.addEventHandle(null, guangYunPlayEnd);
            m_spriteAni.syncUpdateCom();
            m_spriteAni.updateImage();
        }

        override protected void updateLeftAttr(TableCardItemBody tableBody)
        {
            AuxLabel raceText = new AuxLabel(m_model.selfGo, "UIRoot/NumText");
            UtilApi.SetActive(raceText.selfGo, false);
            base.updateLeftAttr(tableBody);
        }


        public void playFanPaiAni()
        {
            createAni();

            m_fanKaAni.setAniEndDisp(onFanPaiAniEnd);
            m_fanKaAni.stateId = 1;
            m_fanKaAni.play();
        }

        protected void createAni()
        {
            if (m_fanKaAni == null)
            {
                m_fanKaAni = new DopeSheetAni();
                string path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathSceneAnimatorController], "FanPai.asset");
                m_fanKaAni.setAniEndDisp(onFanPaiAniEnd);
                m_fanKaAni.setControlInfo(path);
                m_fanKaAni.setGO(this.gameObject());
                m_fanKaAni.syncUpdateControl();
            }
        }

        protected void onFanPaiAniEnd(NumAniBase ani)
        {
            m_bOpened = true;
            ++UIOpenPack.m_iOpenedNum;
            if (UIOpenPack.m_iOpenedNum == 5)
            {
                UIOpenPack.m_iOpenedNum = 0;
                UIOpenPack uiPack = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIOpenPack) as UIOpenPack;
                if (uiPack != null)
                {
                    UtilApi.SetActive(uiPack.m_okBtn.selfGo, true);
                }
            }
        }

        protected void onClick(IDispatchObject dispObj)
        {
            if (m_bOpened) return;
            m_spriteAni.play();
            m_guangyunImg.SetActive(true);
            this.clickEntityDisp.removeEventHandle(null, onClick);
            playFanPaiAni();
        }

        protected void guangYunPlayEnd(IDispatchObject dispObj)
        {
            m_spriteAni.updateImage();
            m_guangyunImg.SetActive(false);
        }
    }
}
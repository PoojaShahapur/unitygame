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
        protected btn[] m_btnArr = new btn[(int)SceneWDSCBtnEnum.eBtnTotal];

        protected SCUICardItem[] m_SCUICardItemList = new SCUICardItem[(int)SCCardNumPerPage.eNum];     // 每一职业卡牌显示列表
        protected TPUICardItem[] m_SCUITPItemList = new TPUICardItem[(int)SCTPNumPerPage.eNum];     // 套牌显示列表

        protected int m_curPageIdx;     // 当前显示的 Page 索引

        public override void onReady()
        {
            base.onReady();

            getWidget();
            addEventHandle();
        }

        public override void onShow()
        {
            base.onShow();
        }

        // 获取控件
        protected void getWidget()
        {
            m_wdscjm.setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm"));

            m_btnArr[(int)SceneWDSCBtnEnum.eBtnBack] = new btn();
            m_btnArr[(int)SceneWDSCBtnEnum.eBtnBack].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/wdscfh/btn"));

            int idx = 0;
            while(idx < (int)SCCardNumPerPage.eNum)
            {
                m_SCUICardItemList[idx] = new SCUICardItem();
                m_SCUICardItemList[idx].m_tran = UtilApi.GoFindChildByPObjAndName("wdscjm/page/" + (idx + 1)).transform;
                ++idx;
            }
            idx = 0;
            while (idx < (int)SCTPNumPerPage.eNum)
            {
                m_SCUITPItemList[idx] = new TPUICardItem();
                m_SCUITPItemList[idx].m_tran = UtilApi.GoFindChildByPObjAndName("wdscjm/kuan/kong/kong" + idx).transform;
                ++idx;
            }
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName(SceneSCPath.BtnClose), onBtnClkClose);       // 关闭
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
                    m_SCUICardItemList[idx].load();
                    ++idx;
                }
            }
        }

        // 新增\数量改变,不包括删除, badd 指明是添加还是改变
        public void psstNotifyOneCardTujianInfoCmd(uint id, byte num, bool badd)
        {
            releaseAllCard();
            psstNotifyAllCardTujianInfoCmd();
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
                    m_SCUITPItemList[idx].cardGroupItem = cardItem;
                    m_SCUITPItemList[idx].load();
                    ++idx;
                }
            }
        }

        // 一个套牌的卡牌列表，index 指明是哪个套牌的
        public void psstRetOneCardGroupInfoUserCmd(uint index, List<uint> list)
        {

        }

        // 新添加一个套牌
        public void psstRetCreateOneCardGroupUserCmd(stRetCreateOneCardGroupUserCmd msg)
        {
            releaseAllTaoPai();
            psstRetCardGroupListInfoUserCmd();
        }

        // 删除一个套牌
        public void psstRetDeleteOneCardGroupUserCmd(uint index)
        {
            releaseAllTaoPai();
            psstRetCardGroupListInfoUserCmd();
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
                m_SCUITPItemList[idx].unload();
                ++idx;
            }
        }
    }
}
using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System.Collections.Generic;
using System.IO;
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

        //protected SceneBtnBase[] m_btnArr = new SceneBtnBase[(int)SceneWDSCBtnEnum.eBtnTotal];

        public override void onReady()
        {
            base.onReady();

            findWidget();
            addEventHandle();

            m_sceneWDSCData.m_leftBtnPnl.hideAllJobBtn();

            psstNotifyAllCardTujianInfoCmd();
        }

        public override void onShow()
        {
            base.onShow();
            m_sceneWDSCData.m_wdscCardPnl.buildFilterList();        // 生成过滤列表
            m_sceneWDSCData.m_leftBtnPnl.updateByCareer((int)EnPlayerCareer.HERO_OCCUPATION_1);      // 切换到第一个职业
        }

        override public void onExit()
        {
            base.onExit();

            UtilApi.Destroy(m_sceneWDSCData.m_sceneUIGo);
            UtilApi.UnloadUnusedAssets();
        }

        // 获取控件
        protected void findWidget()
        {
            m_sceneWDSCData.m_sceneUIParentGo = UtilApi.GoFindChildByPObjAndName(SceneSCPath.WDSCSceneUI);

            loadSceneUI();          // 加载自己的场景 UI

            //m_btnArr[(int)SceneWDSCBtnEnum.eBtnBack] = new SceneBtnBase();
            //m_btnArr[(int)SceneWDSCBtnEnum.eBtnBack].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/wdscfh/btn"));

            // 我的收藏页
            m_sceneWDSCData.m_wdscCardPnl.m_sceneWDSCData = m_sceneWDSCData;
            m_sceneWDSCData.m_wdscCardPnl.setGameObject(UtilApi.GoFindChildByPObjAndName(SceneSCPath.WDSCPage));

            // 类型职业选择
            m_sceneWDSCData.m_pClassFilterPnl.m_sceneWDSCData = m_sceneWDSCData;
            m_sceneWDSCData.m_pClassFilterPnl.findWidget();

            m_sceneWDSCData.m_leftBtnPnl.m_sceneWDSCData = m_sceneWDSCData;
            m_sceneWDSCData.m_leftBtnPnl.findWidget();

            m_sceneWDSCData.m_wdscCardSetPnl.m_sceneWDSCData = m_sceneWDSCData;
            m_sceneWDSCData.m_wdscCardSetPnl.setGameObject(UtilApi.GoFindChildByPObjAndName(SceneSCPath.WDSCGO));
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            m_sceneWDSCData.m_leftBtnPnl.addEventHandle();
        }

        public void showUI()
        {
            m_sceneWDSCData.m_wdscCardSetPnl.show();
        }

        // 所有卡牌
        public void psstNotifyAllCardTujianInfoCmd()
        {
            m_sceneWDSCData.m_wdscCardPnl.updatePageUI();
        }

        // 新增\数量改变,不包括删除, badd 指明是添加还是改变
        public void psstNotifyOneCardTujianInfoCmd(uint id, byte num, bool badd)
        {
            if(badd)
            {
                // 增加一张卡牌
                m_sceneWDSCData.m_wdscCardPnl.updatePageUI();
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
                    newCardSet(cardItem, false);

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
            newCardSet(cardGroup);
        }

        // 删除一个套牌
        public void psstRetDeleteOneCardGroupUserCmd(uint index)
        {
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
                ++idx;
            }
        }

        public void newCardSet(CardGroupItem cardGroup, bool bEnterEdit = true)
        {
            m_sceneWDSCData.m_wdscCardSetPnl.newCardSet(cardGroup, bEnterEdit);
        }

        // 保存卡牌成功
        public void psstRetSaveOneCardGroupUserCmd(uint index)
        {
            // 保存当前卡牌
            m_sceneWDSCData.m_wdscCardSetPnl.psstRetSaveOneCardGroupUserCmd(index);
        }

        public void delOneCardGroup(int p)
        {
            m_sceneWDSCData.m_wdscCardSetPnl.delOneCardGroup(p);
        }

        public void loadSceneUI()
        {
            string name = Path.Combine(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUIScene], "UISceneWDSC/SceneUI.prefab");
            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            LocalFileSys.modifyLoadParam(name, param);
            param.m_loaded = onLoaded;
            param.m_failed = onFailed;
            param.m_loadNeedCoroutine = false;
            param.m_resNeedCoroutine = false;
            Ctx.m_instance.m_resLoadMgr.loadResources(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        // 加载一个表完成
        public void onLoaded(IDispatchObject resEvt)
        {
            IResItem res = resEvt as IResItem;
            string name = Path.Combine(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUIScene], "UISceneWDSC/SceneUI.prefab");
            m_sceneWDSCData.m_sceneUIGo = res.InstantiateObject(name);
            m_sceneWDSCData.m_sceneUIGo.name = SceneSCPath.SceneUIName;
            UtilApi.SetParent(m_sceneWDSCData.m_sceneUIGo, m_sceneWDSCData.m_sceneUIParentGo, false);
            m_sceneWDSCData.m_sceneUIGo.transform.localScale = new Vector3(0.003f, 0.003f, 1);
            m_sceneWDSCData.m_sceneUIGo.transform.localEulerAngles = new Vector3(90, 0, 0);
            UtilApi.onLoaded(resEvt);
        }

        public void onFailed(IDispatchObject resEvt)
        {
            UtilApi.onFailed(resEvt);
        }
    }
}
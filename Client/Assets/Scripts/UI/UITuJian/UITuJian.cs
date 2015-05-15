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
     * @brief 图鉴界面
     */
    public class UITuJian : Form
    {
        protected TuJianData m_tuJianData;

        public override void onReady()
        {
            m_tuJianData = new TuJianData(this);

            findWidget();
            addEventHandle();

            m_tuJianData.init();

            m_tuJianData.m_leftBtnPnl.hideAllJobBtn();
            psstNotifyAllCardTujianInfoCmd();
        }

        public override void onShow()
        {
            m_tuJianData.m_wdscCardPnl.buildFilterList();        // 生成过滤列表
            m_tuJianData.m_leftBtnPnl.updateByCareer((int)EnPlayerCareer.HERO_OCCUPATION_1);      // 切换到第一个职业
        }

        override public void onExit()
        {
            base.onExit();

            UtilApi.Destroy(m_tuJianData.m_form.m_GUIWin.m_uiRoot);
            UtilApi.UnloadUnusedAssets();
        }

        // 获取控件
        protected void findWidget()
        {
            m_tuJianData.findWidget();
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            m_tuJianData.addEventHandle();
        }

        public void showUI()
        {
            m_tuJianData.m_wdscCardSetPnl.show();
        }

        // 所有卡牌
        public void psstNotifyAllCardTujianInfoCmd()
        {
            m_tuJianData.m_wdscCardPnl.updatePageUI();
        }

        // 新增\数量改变,不包括删除, badd 指明是添加还是改变
        public void psstNotifyOneCardTujianInfoCmd(uint id, byte num, bool badd)
        {
            if(badd)
            {
                // 增加一张卡牌
                m_tuJianData.m_wdscCardPnl.updatePageUI();
            }
        }

        // 套牌列表
        public void psstRetCardGroupListInfoUserCmd()
        {
            if (Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardGroupListArr.Count > 0)
            {
                int idx = 0;
                CardGroupItem cardItem;
                while (idx < Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardGroupListArr.Count && idx < (int)TuJianNumPerPage.eNum)
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
            while (idx < (int)TuJianNumPerPage.eNum)
            {
                ++idx;
            }
        }

        public void newCardSet(CardGroupItem cardGroup, bool bEnterEdit = true)
        {
            m_tuJianData.m_wdscCardSetPnl.newCardSet(cardGroup, bEnterEdit);
        }

        // 保存卡牌成功
        public void psstRetSaveOneCardGroupUserCmd(uint index)
        {
            // 保存当前卡牌
            m_tuJianData.m_wdscCardSetPnl.psstRetSaveOneCardGroupUserCmd(index);
        }

        public void delOneCardGroup(int p)
        {
            m_tuJianData.m_wdscCardSetPnl.delOneCardGroup(p);
        }
    }
}
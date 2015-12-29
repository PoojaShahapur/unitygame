using UnityEngine;
using System.Collections.Generic;
using SDK.Lib;

namespace Game.UI
{
    public enum TuJianCardPnl_BtnIndex
    {
        eBtnPre,
        eBtnNext,

        eBtnTotal,
    }

    /// <summary>
    /// 我的收藏卡牌显示面板
    /// </summary>
    public class TuJianCardPnl : TuJianPnlBase
    {
        public UIGrid m_cardGrid = new UIGrid();            // 收藏卡牌数据
        public List<TuJianCardItemCom> m_SCUICardItemList = new List<TuJianCardItemCom>();

        public PageInfo[] m_pageArr = new PageInfo[(int)EnPlayerCareer.ePCTotal];

        public List<CardItemBase>[] m_filterCardListArr = new List<CardItemBase>[(int)EnPlayerCareer.ePCTotal];      // 每一个职业一个列表，过滤后的数据，主要用来显示
        public int m_filterMp = 8;          // 过滤的 Mp 值
        public AuxLabel m_textPageNum;

        protected GameObject m_cardGo;

        protected AuxBasicButton[] m_btnArr = new AuxBasicButton[(int)LeftBtnPnl_BtnIndex.eBtnJobTotal];
        public TuJianCardItemCom m_curClkTuJianCardItemCom;     // 当前点击的卡牌

        public TuJianCardPnl(TuJianData data) :
            base(data)
        {
            int idx = 0;
            while (idx < (int)EnPlayerCareer.ePCTotal)
            {
                m_pageArr[idx] = new PageInfo();
                ++idx;
            }

            for (idx = 0; idx < (int)EnPlayerCareer.ePCTotal; ++idx)
            {
                m_filterCardListArr[idx] = new List<CardItemBase>();
            }

            m_cardGo = UtilApi.createGameObject("TuJianCardGO");
            UtilApi.SetParent(m_cardGo, Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIModel], false);
            UtilApi.setGOName(m_cardGo, "CardListGo");
            UtilApi.setPos(m_cardGo.transform, new Vector3(-5.8f, 3.7f, 0.0f));
            UtilApi.setRot(m_cardGo.transform, new Vector3(270, 0, 0));
        }

        public int filterMp
        {
            get
            {
                return m_filterMp;
            }
            set
            {
                m_filterMp = value;
                buildFilterList();     // 生成过滤列表
                destroyAndUpdateCardList();

            }
        }

        public new void dispose()
        {
            destroyCrad();
            UtilApi.Destroy(m_cardGo);
        }

        public new void findWidget()
        {
            m_tuJianData.m_onClkCard = onClkCard;

            m_btnArr[(int)TuJianCardPnl_BtnIndex.eBtnPre] = new AuxBasicButton(m_tuJianData.m_form.m_guiWin.m_uiRoot, TuJianPath.BtnPrePage);
            m_btnArr[(int)TuJianCardPnl_BtnIndex.eBtnNext] = new AuxBasicButton(m_tuJianData.m_form.m_guiWin.m_uiRoot, TuJianPath.BtnNextPage);

            m_cardGrid.setGameObject(m_cardGo);
            m_cardGrid.cellWidth = 3.0f;
            m_cardGrid.cellHeight = 4.0f;
            m_cardGrid.maxPerLine = (int)TuJianCardNumPerPage.eCol;

            // 当前页号
            m_textPageNum = new AuxLabel(m_tuJianData.m_form.m_guiWin.m_uiRoot, TuJianPath.TextPageNum);
        }

        public new void addEventHandle()
        {
            m_btnArr[(int)TuJianCardPnl_BtnIndex.eBtnPre].addEventHandle(onPreBtnClk);       // 前一页
            m_btnArr[(int)TuJianCardPnl_BtnIndex.eBtnNext].addEventHandle(onNextBtnClk);       // 后一页
        }

        /// <summary>
        /// 销毁8张牌
        /// </summary>
        protected void destroyCrad()
        {
            foreach (var card in m_SCUICardItemList)
            {
                card.dispose();
            }

            m_SCUICardItemList.Clear();
        }

        public void destroyAndUpdateCardList()
        {
            destroyCrad();
            updateCardList();
            updatePreNextBtnState();
            updatePageNo();
        }

        protected void updateCardList()
        {
            if (m_filterCardListArr[m_tuJianData.m_pClassFilterPnl.m_tabBtnIdx].Count > 0)
            {
                int idx = 0;
                CardItemBase cardItem;
                TuJianCardItemCom uicardItem;
                while (bInRangeByPageAndIdx(m_tuJianData.m_pClassFilterPnl.m_tabBtnIdx, idx))
                {
                    cardItem = m_filterCardListArr[m_tuJianData.m_pClassFilterPnl.m_tabBtnIdx][m_pageArr[m_tuJianData.m_pClassFilterPnl.m_tabBtnIdx].m_curPageIdx * (int)TuJianCardNumPerPage.eNum + idx];

                    uicardItem = new TuJianCardItemCom(null);
                    uicardItem.createCard(cardItem, m_cardGrid.getGameObject());
                    m_SCUICardItemList.Add(uicardItem);
                    m_SCUICardItemList[m_SCUICardItemList.Count - 1].m_clkCB = m_tuJianData.m_onClkCard;

                    ++idx;
                }

                m_cardGrid.Reposition();
            }
        }

        public void updatePreNextBtnState()
        {
            if(canMovePre())
            {
                m_btnArr[(int)TuJianCardPnl_BtnIndex.eBtnPre].enable();
            }
            else
            {
                m_btnArr[(int)TuJianCardPnl_BtnIndex.eBtnPre].disable();
            }

            if (canMoveNext())
            {
                m_btnArr[(int)TuJianCardPnl_BtnIndex.eBtnNext].enable();
            }
            else
            {
                m_btnArr[(int)TuJianCardPnl_BtnIndex.eBtnNext].disable();
            }
        }

        public void updatePageNo()
        {
            m_textPageNum.text = (m_pageArr[m_tuJianData.m_pClassFilterPnl.m_tabBtnIdx].m_curPageIdx + 1).ToString();

            AuxLabel textPageNum = new AuxLabel(m_tuJianData.m_form.m_guiWin.m_uiRoot, TuJianPath.TextPageMaxNum);
            textPageNum.text = m_pageArr[m_tuJianData.m_pClassFilterPnl.m_tabBtnIdx].getTotalPageDesc().ToString();
        }

        // 收藏中前一页
        public void onPreBtnClk(IDispatchObject dispObj)
        {
            if (m_pageArr[m_tuJianData.m_pClassFilterPnl.m_tabBtnIdx].canMovePreInCurTagPage())      // 如果当前 TabPage 没有到开始
            {
                --m_pageArr[m_tuJianData.m_pClassFilterPnl.m_tabBtnIdx].m_curPageIdx;
                destroyAndUpdateCardList();
            }
            else
            {
                int curTagPage = getPreTagPage();
                if (bInTabPageRang(curTagPage))
                {
                    int curTagPageCnt = m_pageArr[curTagPage].totalPage;
                    m_pageArr[curTagPage].m_curPageIdx = curTagPageCnt - 1;      // 最后一页
                    m_tuJianData.m_leftBtnPnl.updateByCareer(curTagPage);
                }
            }
        }

        // 收藏中后一页
        public void onNextBtnClk(IDispatchObject dispObj)
        {
            if (m_pageArr[m_tuJianData.m_pClassFilterPnl.m_tabBtnIdx].canMoveNextInCurTagPage())
            {
                ++m_pageArr[m_tuJianData.m_pClassFilterPnl.m_tabBtnIdx].m_curPageIdx;
                destroyAndUpdateCardList();
            }
            else
            {
                int curTagPage = getNextTagPage();
                if (bInTabPageRang(curTagPage))
                {
                    m_pageArr[curTagPage].m_curPageIdx = 0;      // 第一页
                    m_tuJianData.m_leftBtnPnl.updateByCareer(curTagPage);
                }
            }
        }

        // 判断当前 TabPage 是否可以向前翻页
        public bool canMovePre()
        {
            if (m_pageArr[m_tuJianData.m_pClassFilterPnl.m_tabBtnIdx].canMovePreInCurTagPage())       // 当前页就可以移动
            {
                return true;
            }
            else        // 当前页不能移动，需要跨页移动
            {
                int curTagPage = getPreTagPage();
                if(bInTabPageRang(curTagPage))
                {
                    return true;
                }
            }

            return false;
        }

        // 是否可以移动到之前的 TabPage 
        protected int getPreTagPage()
        {
            int curTagPage = m_tuJianData.m_pClassFilterPnl.m_tabBtnIdx;
            while (--curTagPage >= 0)   // 如果 TaPage 前面还有
            {
                if (m_filterCardListArr[curTagPage].Count > 0)     // 如果当前页面有内容
                {
                    break;
                }
            }

            return curTagPage;
        }

        // 判断当前的 TabPage 是否可以向后翻页
        public bool canMoveNext()
        {
            if (m_pageArr[m_tuJianData.m_pClassFilterPnl.m_tabBtnIdx].canMoveNextInCurTagPage())
            {
                return true;
            }

            int curTagPage = getNextTagPage();
            if (bInTabPageRang(curTagPage))
            {
                return true;
            }

            return false;
        }

        protected int getNextTagPage()
        {
            int curTagPage = m_tuJianData.m_pClassFilterPnl.m_tabBtnIdx;
            while (++curTagPage < (int)EnPlayerCareer.ePCTotal)   // 如果 TaPage 后面还有
            {
                if (m_filterCardListArr[curTagPage].Count > 0)     // 如果当前页面有内容
                {
                    break;
                }
            }

            return curTagPage;
        }

        protected bool bInTabPageRang(int idx)
        {
            return (0 <= idx && idx < (int)EnPlayerCareer.ePCTotal);
        }

        public void updatePageUI()
        {
            if (m_tuJianData.m_pClassFilterPnl.m_tabBtnIdx >= 0)
            {
                destroyAndUpdateCardList();
            }
        }

        protected bool bInRangeByPageAndIdx(int page, int idx)
        {
            return (m_pageArr[page].m_curPageIdx * (int)TuJianCardNumPerPage.eNum + idx < m_filterCardListArr[page].Count && 
                    idx < (int)TuJianCardNumPerPage.eNum);
        }

        // 点击收藏界面中卡牌面板中的一张卡牌
        public void onClkCard(TuJianCardItemCom ioItem)
        {
            m_curClkTuJianCardItemCom = ioItem;
            Ctx.m_instance.m_auxUIHelp.m_auxTuJian.m_eTuJianMenu = ETuJianMenu.eCard;
            Ctx.m_instance.m_uiMgr.loadAndShow(UIFormID.eUITuJianTop);
        }

        public void addCurCard2CardSet()
        {
            if (m_tuJianData.m_wdscCardSetPnl.m_curTaoPaiMod == WdscmTaoPaiMod.eTaoPaiMod_Editset)
            {
                m_tuJianData.m_wdscCardSetPnl.addCard2EditCardSet(m_curClkTuJianCardItemCom.m_cardItemBase.m_tujian.id);
            }
        }

        public void buildFilterList()
        {
            int idx = 0;
            int idy = 0;

            for (idy = 0; idy < (int)EnPlayerCareer.ePCTotal; ++idy)
            {
                m_filterCardListArr[idy].Clear();

                for (idx = 0; idx < Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardListArr[idy].Count; ++idx)
                {
                    if (m_filterMp == 8)       // 全部
                    {
                        m_filterCardListArr[idy].Add(Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardListArr[idy][idx]);
                    }
                    else if (m_filterMp == 7)       // 大于等于 7 
                    {
                        if (Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardListArr[idy][idx].m_tableItemCard.m_magicConsume >= m_filterMp)
                        {
                            m_filterCardListArr[idy].Add(Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardListArr[idy][idx]);
                        }
                    }
                    else        // 等于
                    {
                        if (Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardListArr[idy][idx].m_tableItemCard.m_magicConsume == m_filterMp)
                        {
                            m_filterCardListArr[idy].Add(Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardListArr[idy][idx]);
                        }
                    }
                }

                m_filterCardListArr[idy].Sort(cmpCardFunc);            // 排序卡牌

                m_pageArr[idy].m_curPageIdx = 0;        // 重置索引
                m_pageArr[idy].totalPage = (m_filterCardListArr[idy].Count + ((int)TuJianCardNumPerPage.eNum - 1)) / (int)TuJianCardNumPerPage.eNum;
                m_pageArr[idy].m_cardCount = m_filterCardListArr[idy].Count;
            }
        }

        protected int cmpCardFunc(CardItemBase a, CardItemBase b)
        {
            int ret = 0;
            if(a.m_tableItemCard.m_magicConsume < b.m_tableItemCard.m_magicConsume)
            {
                ret = -1;
            }
            else if(a.m_tableItemCard.m_magicConsume > b.m_tableItemCard.m_magicConsume)
            {
                ret = 1;
            }
            else    // 相等
            {
                if(a.m_tujian.id < b.m_tujian.id)
                {
                    ret = -1;
                }
                else if(a.m_tujian.id > b.m_tujian.id)
                {
                    ret = 1;
                }
                else
                {
                    ret = 0;
                }
            }

            return ret;
        }

        public void toggleCardVisible(bool bShow)
        {
            UtilApi.SetActive(m_cardGo, bShow);
        }
    }
}
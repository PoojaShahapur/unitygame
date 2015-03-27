using SDK.Common;
using System;
using UnityEngine.UI;

namespace Game.UI
{
    public class SceneWDSCData
    {
        public classfilter[] m_tabBtnList = new classfilter[10];
        public int m_tabBtnIdx = -1;         // 当前点击的 Tab Btn 索引
        public PageInfo[] m_pageArr = new PageInfo[(int)EnPlayerCareer.ePCTotal];
        public int m_curTabPageIdx = -1;     // 当前显示的 TabPage 索引
        public Action<SCUICardItem> m_onClkCard;

        public Text m_textPageNum;

        public SceneWDSCData()
        {
            //m_curTabPageIdx = (int)EnPlayerCareer.HERO_OCCUPATION_1;        // 默认在第一个职业
            int idx = 0;
            while (idx < (int)EnPlayerCareer.ePCTotal)
            {
                m_pageArr[idx] = new PageInfo();
                ++idx;
            }
        }

        // 点击当前按钮
        public void onBtnClk(int btnidx)
        {
            if (btnidx != m_tabBtnIdx)
            {
                m_tabBtnIdx = btnidx;
                foreach (classfilter item in m_tabBtnList)
                {
                    if (item.tag == m_tabBtnIdx)
                    {
                        item.btnUpAni();
                    }
                    else
                    {
                        item.btnDownAni();
                    }
                }
            }
        }
    }
}
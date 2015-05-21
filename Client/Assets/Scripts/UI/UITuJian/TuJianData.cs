using SDK.Common;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class TuJianData
    {
        public Form m_form;

        public ClassFilterPnl m_pClassFilterPnl;            // 类型过滤面板
        public TuJianCardSetPnl m_wdscCardSetPnl;
        public TuJianCardPnl m_wdscCardPnl;
        public LeftBtnPnl m_leftBtnPnl;

        public Action<TuJianCardItemCom> m_onClkCard;

        public TuJianData(Form form)
        {
            m_form = form;

            m_pClassFilterPnl = new ClassFilterPnl(this);
            m_wdscCardSetPnl = new TuJianCardSetPnl(this);
            m_wdscCardPnl = new TuJianCardPnl(this);
            m_leftBtnPnl = new LeftBtnPnl(this);
        }

        public void dispose()
        {
            m_wdscCardPnl.dispose();
        }

        public void findWidget()
        {
            m_pClassFilterPnl.findWidget();
            m_wdscCardSetPnl.findWidget();
            m_wdscCardPnl.findWidget();
            m_leftBtnPnl.findWidget();
        }

        public void addEventHandle()
        {
            m_pClassFilterPnl.addEventHandle();
            m_wdscCardSetPnl.addEventHandle();
            m_wdscCardPnl.addEventHandle();
            m_leftBtnPnl.addEventHandle();
        }

        public void init()
        {
            m_pClassFilterPnl.init();
            m_wdscCardSetPnl.init();
            m_wdscCardPnl.init();
            m_leftBtnPnl.init();
        }
    }
}
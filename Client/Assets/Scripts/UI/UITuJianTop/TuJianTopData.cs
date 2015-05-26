using SDK.Common;
using UnityEngine;

namespace Game.UI
{
    public class TuJianTopData
    {
        public Form m_form;
        public TuJianCardHandlePnl m_tuJianCardHandlePnl;
        public TuJianCardSetMenuPnl m_tuJianCardSetMenuPnl;

        public TuJianTopData(UITuJianTop form)
        {
            m_form = form;
            if (Ctx.m_instance.m_auxUIHelp.m_auxTuJian.m_eTuJianMenu == ETuJianMenu.eCard)
            {
                m_tuJianCardHandlePnl = new TuJianCardHandlePnl(this);
            }
            else if (Ctx.m_instance.m_auxUIHelp.m_auxTuJian.m_eTuJianMenu == ETuJianMenu.eCardSet)
            {
                m_tuJianCardSetMenuPnl = new TuJianCardSetMenuPnl(this);
            }
        }

        public void findWidget()
        {
            if (Ctx.m_instance.m_auxUIHelp.m_auxTuJian.m_eTuJianMenu == ETuJianMenu.eCard)
            {
                m_tuJianCardHandlePnl.findWidget();
            }
            else if (Ctx.m_instance.m_auxUIHelp.m_auxTuJian.m_eTuJianMenu == ETuJianMenu.eCardSet)
            {
                m_tuJianCardSetMenuPnl.findWidget();
            }
        }

        public void addEventHandle()
        {
            if (Ctx.m_instance.m_auxUIHelp.m_auxTuJian.m_eTuJianMenu == ETuJianMenu.eCard)
            {
                m_tuJianCardHandlePnl.addEventHandle();
            }
            else if (Ctx.m_instance.m_auxUIHelp.m_auxTuJian.m_eTuJianMenu == ETuJianMenu.eCardSet)
            {
                m_tuJianCardSetMenuPnl.addEventHandle();
            }
        }

        public void init()
        {
            GameObject _go;
            _go = UtilApi.TransFindChildByPObjAndPath(m_form.m_GUIWin.m_uiRoot, TuJianTopComPath.Card_Root);
            UtilApi.SetActive(_go, false);

            _go = UtilApi.TransFindChildByPObjAndPath(m_form.m_GUIWin.m_uiRoot, TuJianTopComPath.CardSet_Root);
            UtilApi.SetActive(_go, false);

            if (Ctx.m_instance.m_auxUIHelp.m_auxTuJian.m_eTuJianMenu == ETuJianMenu.eCard)
            {
                m_tuJianCardHandlePnl.m_go = UtilApi.TransFindChildByPObjAndPath(m_form.m_GUIWin.m_uiRoot, TuJianTopComPath.Card_Root);
                m_tuJianCardHandlePnl.init();
            }
            else if (Ctx.m_instance.m_auxUIHelp.m_auxTuJian.m_eTuJianMenu == ETuJianMenu.eCardSet)
            {
                m_tuJianCardSetMenuPnl.m_go = UtilApi.TransFindChildByPObjAndPath(m_form.m_GUIWin.m_uiRoot, TuJianTopComPath.CardSet_Root);
                m_tuJianCardSetMenuPnl.init();
            }
        }

        public void dispose()
        {
            if (Ctx.m_instance.m_auxUIHelp.m_auxTuJian.m_eTuJianMenu == ETuJianMenu.eCard)
            {
                m_tuJianCardHandlePnl.dispose();
            }
            else if (Ctx.m_instance.m_auxUIHelp.m_auxTuJian.m_eTuJianMenu == ETuJianMenu.eCardSet)
            {
                m_tuJianCardSetMenuPnl.dispose();
            }
        }
    }
}
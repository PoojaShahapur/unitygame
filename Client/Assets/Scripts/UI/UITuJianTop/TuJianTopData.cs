using SDK.Lib;
using UnityEngine;

namespace Game.UI
{
    public class TuJianTopData
    {
        public Form m_form;
        public TuJianCardHandlePnl m_tuJianCardHandlePnl;
        public TuJianCardSetMenuPnl m_tuJianCardSetMenuPnl;
        public TuJianJobSelPnl m_tuJianJobSelPnl;
        public TuJianFilterMenuPnl m_tuJianFilterMenuPnl;

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
            else if (Ctx.m_instance.m_auxUIHelp.m_auxTuJian.m_eTuJianMenu == ETuJianMenu.eJobSel)
            {
                m_tuJianJobSelPnl = new TuJianJobSelPnl(this);
            }
            else if (Ctx.m_instance.m_auxUIHelp.m_auxTuJian.m_eTuJianMenu == ETuJianMenu.eFilter)
            {
                m_tuJianFilterMenuPnl = new TuJianFilterMenuPnl(this);
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
            else if (Ctx.m_instance.m_auxUIHelp.m_auxTuJian.m_eTuJianMenu == ETuJianMenu.eJobSel)
            {
                m_tuJianJobSelPnl.findWidget();
            }
            else if (Ctx.m_instance.m_auxUIHelp.m_auxTuJian.m_eTuJianMenu == ETuJianMenu.eFilter)
            {
                m_tuJianFilterMenuPnl.findWidget();
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
            else if (Ctx.m_instance.m_auxUIHelp.m_auxTuJian.m_eTuJianMenu == ETuJianMenu.eJobSel)
            {
                m_tuJianJobSelPnl.addEventHandle();
            }
            else if (Ctx.m_instance.m_auxUIHelp.m_auxTuJian.m_eTuJianMenu == ETuJianMenu.eFilter)
            {
                m_tuJianFilterMenuPnl.addEventHandle();
            }
        }

        public void init()
        {
            GameObject _go;
            _go = UtilApi.TransFindChildByPObjAndPath(m_form.m_guiWin.m_uiRoot, TuJianTopComPath.Card_Root);
            UtilApi.SetActive(_go, false);

            _go = UtilApi.TransFindChildByPObjAndPath(m_form.m_guiWin.m_uiRoot, TuJianTopComPath.CardSet_Root);
            UtilApi.SetActive(_go, false);

            _go = UtilApi.TransFindChildByPObjAndPath(m_form.m_guiWin.m_uiRoot, TuJianTopComPath.Job_Root);
            UtilApi.SetActive(_go, false);

            _go = UtilApi.TransFindChildByPObjAndPath(m_form.m_guiWin.m_uiRoot, TuJianTopComPath.Filter_Root);
            UtilApi.SetActive(_go, false);

            if (Ctx.m_instance.m_auxUIHelp.m_auxTuJian.m_eTuJianMenu == ETuJianMenu.eCard)
            {
                m_tuJianCardHandlePnl.m_go = UtilApi.TransFindChildByPObjAndPath(m_form.m_guiWin.m_uiRoot, TuJianTopComPath.Card_Root);
                m_tuJianCardHandlePnl.init();
            }
            else if (Ctx.m_instance.m_auxUIHelp.m_auxTuJian.m_eTuJianMenu == ETuJianMenu.eCardSet)
            {
                m_tuJianCardSetMenuPnl.m_go = UtilApi.TransFindChildByPObjAndPath(m_form.m_guiWin.m_uiRoot, TuJianTopComPath.CardSet_Root);
                m_tuJianCardSetMenuPnl.init();
            }
            else if (Ctx.m_instance.m_auxUIHelp.m_auxTuJian.m_eTuJianMenu == ETuJianMenu.eJobSel)
            {
                m_tuJianJobSelPnl.m_go = UtilApi.TransFindChildByPObjAndPath(m_form.m_guiWin.m_uiRoot, TuJianTopComPath.Job_Root);
                m_tuJianJobSelPnl.init();
            }
            else if (Ctx.m_instance.m_auxUIHelp.m_auxTuJian.m_eTuJianMenu == ETuJianMenu.eFilter)
            {
                m_tuJianFilterMenuPnl.m_go = UtilApi.TransFindChildByPObjAndPath(m_form.m_guiWin.m_uiRoot, TuJianTopComPath.Filter_Root);
                m_tuJianFilterMenuPnl.init();
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
            else if (Ctx.m_instance.m_auxUIHelp.m_auxTuJian.m_eTuJianMenu == ETuJianMenu.eJobSel)
            {
                m_tuJianJobSelPnl.dispose();
            }
            else if (Ctx.m_instance.m_auxUIHelp.m_auxTuJian.m_eTuJianMenu == ETuJianMenu.eFilter)
            {
                m_tuJianFilterMenuPnl.dispose();
            }
        }
    }
}
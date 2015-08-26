using Game.Msg;
using SDK.Lib;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI
{
    public class UITask : Form, IUITask
    {
        protected AuxBasicButton[] m_btnArr;
        public override void onReady()
        {
            base.onReady();
            m_btnArr = new AuxBasicButton[(int)TaskBtnNum.eBtnTotal];
            findWidget();
            addEventHandle();
        }

        public override void onShow()
        {
            base.onShow();
        }

        public override void onExit()
        {
            base.onExit();
        }

        protected void findWidget()
        {
            m_btnArr[(int)TaskBtnNum.eBtnTask1] = new AuxBasicButton(m_GUIWin.m_uiRoot, TaskComPath.BtnTask1);
            m_btnArr[(int)TaskBtnNum.eBtnTask2] = new AuxBasicButton(m_GUIWin.m_uiRoot, TaskComPath.BtnTask2);
            m_btnArr[(int)TaskBtnNum.eBtnTask3] = new AuxBasicButton(m_GUIWin.m_uiRoot, TaskComPath.BtnTask3);
            m_btnArr[(int)TaskBtnNum.eBtnBack] = new AuxBasicButton(m_GUIWin.m_uiRoot, TaskComPath.BtnBack);
            m_btnArr[(int)TaskBtnNum.eBtnBg] = new AuxBasicButton(m_GUIWin.m_uiRoot, TaskComPath.BtnBg);
            m_btnArr[(int)TaskBtnNum.eBtnTaskDes1] = new AuxBasicButton(m_GUIWin.m_uiRoot, TaskComPath.Task1);
            m_btnArr[(int)TaskBtnNum.eBtnTaskDes2] = new AuxBasicButton(m_GUIWin.m_uiRoot, TaskComPath.Task2);
            m_btnArr[(int)TaskBtnNum.eBtnTaskDes3] = new AuxBasicButton(m_GUIWin.m_uiRoot, TaskComPath.Task3);
        }

        protected void addEventHandle()
        {
            m_btnArr[(int)TaskBtnNum.eBtnTask1].addEventHandle(onBtnTask1Clk);
            m_btnArr[(int)TaskBtnNum.eBtnTask2].addEventHandle(onBtnTask2Clk);
            m_btnArr[(int)TaskBtnNum.eBtnTask3].addEventHandle(onBtnTask3Clk);
            m_btnArr[(int)TaskBtnNum.eBtnBack].addEventHandle(onBtnBackClk);
            m_btnArr[(int)TaskBtnNum.eBtnBg].addEventHandle(onTaskClk);
            m_btnArr[(int)TaskBtnNum.eBtnTaskDes1].addEventHandle(onTaskClk);
            m_btnArr[(int)TaskBtnNum.eBtnTaskDes2].addEventHandle(onTaskClk);
            m_btnArr[(int)TaskBtnNum.eBtnTaskDes3].addEventHandle(onTaskClk);
        }

        protected void onBtnTask1Clk(IDispatchObject dispObj)
        {
            m_btnArr[(int)TaskBtnNum.eBtnTaskDes1].selfGo.SetActive(true);
            m_btnArr[(int)TaskBtnNum.eBtnTask1].selfGo.SetActive(false);
            m_btnArr[(int)TaskBtnNum.eBtnTask2].selfGo.SetActive(false);
            m_btnArr[(int)TaskBtnNum.eBtnTask3].selfGo.SetActive(false);
        }

        protected void onBtnTask2Clk(IDispatchObject dispObj)
        {
            m_btnArr[(int)TaskBtnNum.eBtnTaskDes2].selfGo.SetActive(true);
            m_btnArr[(int)TaskBtnNum.eBtnTask1].selfGo.SetActive(false);
            m_btnArr[(int)TaskBtnNum.eBtnTask2].selfGo.SetActive(false);
            m_btnArr[(int)TaskBtnNum.eBtnTask3].selfGo.SetActive(false);
        }

        protected void onBtnTask3Clk(IDispatchObject dispObj)
        {
            m_btnArr[(int)TaskBtnNum.eBtnTaskDes3].selfGo.SetActive(true);
            m_btnArr[(int)TaskBtnNum.eBtnTask1].selfGo.SetActive(false);
            m_btnArr[(int)TaskBtnNum.eBtnTask2].selfGo.SetActive(false);
            m_btnArr[(int)TaskBtnNum.eBtnTask3].selfGo.SetActive(false);
        }

        protected void onTaskClk(IDispatchObject dispObj)
        {
            m_btnArr[(int)TaskBtnNum.eBtnTask1].selfGo.SetActive(true);
            m_btnArr[(int)TaskBtnNum.eBtnTask2].selfGo.SetActive(true);
            m_btnArr[(int)TaskBtnNum.eBtnTask3].selfGo.SetActive(true);

            m_btnArr[(int)TaskBtnNum.eBtnTaskDes1].selfGo.SetActive(false);
            m_btnArr[(int)TaskBtnNum.eBtnTaskDes2].selfGo.SetActive(false);
            m_btnArr[(int)TaskBtnNum.eBtnTaskDes3].selfGo.SetActive(false);
        }

        protected void onBtnBackClk(IDispatchObject dispObj)
        {
            exit();
        }
    }
}

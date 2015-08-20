using Fight;
using FightCore;
using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 迷宫界面
     */
    public class UIMaze : Form, IUITest
    {
        public override void onInit()
        {
            exitMode = false;         // 直接隐藏
            //hideOnCreate = true;
            base.onInit();
        }

        override public void onShow()
        {
            base.onShow();
        }
        
        // 初始化控件
        override public void onReady()
        {
            base.onReady();
            findWidget();
            addEventHandle();
        }

        // 每一次隐藏都会调用一次
        override public void onHide()
		{
            base.onHide();
		}

        // 每一次关闭都会调用一次
        override public void onExit()
        {
            base.onExit();
        }

        protected void findWidget()
        {
            
        }

        protected void addEventHandle()
        {
            UtilApi.addEventHandle(m_GUIWin.m_uiRoot, "BtnStart", onStartBtnClk);
        }

        protected void onStartBtnClk()
        {
            Ctx.m_instance.m_maze.mazeData.getWayPtList();
            Ctx.m_instance.m_maze.mazeData.setStartPos();
            Ctx.m_instance.m_maze.mazeData.startMove();
        }
    }
}
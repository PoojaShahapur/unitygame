using SDK.Lib;
using System.IO;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 迷宫界面
     */
    public class UIMaze : Form, IUITest
    {
        protected GameObject m_btnGo;
        protected GameObject m_btnResetGo;

        public override void onInit()
        {
            exitMode = false;         // 直接隐藏
            //hideOnCreate = true;
            base.onInit();
        }

        override public void onShow()
        {
            base.onShow();
            Ctx.m_instance.m_maze.mazeData.mazeOp.bStart = false;
            //Ctx.m_instance.m_maze.mazeData.mazePlayer.hide();       // 初始的时候隐藏

            RectTransform trans = m_btnGo.GetComponent<RectTransform>();
            if (Ctx.m_instance.m_maze.mazeData.curSceneIdx == (int)eSceneIndex.eFirst)
            {
                UtilApi.setRectPos(trans, new Vector3(-287, 70, 0));
            }
            else
            {
                UtilApi.setRectPos(trans, new Vector3(-449, 172, 0));
            }

            UtilApi.SetActive(m_btnResetGo, false);
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
            Ctx.m_instance.m_maze.mazeData.mazeOp.bStart = true;
            //Ctx.m_instance.m_maze.mazeData.mazePlayer.show();       // 初始的时候隐藏
            base.onHide();
		}

        // 每一次关闭都会调用一次
        override public void onExit()
        {
            base.onExit();
        }

        protected void findWidget()
        {
            m_btnGo = UtilApi.TransFindChildByPObjAndPath(m_GUIWin.m_uiRoot, "BtnStart");
            m_btnResetGo = UtilApi.TransFindChildByPObjAndPath(m_GUIWin.m_uiRoot, "BtnReset");
        }

        protected void addEventHandle()
        {
            UtilApi.addEventHandle(m_GUIWin.m_uiRoot, "BtnStart", onStartBtnClk);
            UtilApi.addEventHandle(m_GUIWin.m_uiRoot, "BtnReset", onResetBtnClk);
        }

        protected void onStartBtnClk()
        {
            // string path = Path.Combine(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathAudio], "BossDie.mp3");
            //Ctx.m_instance.m_soundMgr.play(path, false);

            string path = "";
            path = Path.Combine(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathAudio], "Ground.mp3");
            Ctx.m_instance.m_soundMgr.play(path);

            Ctx.m_instance.m_maze.mazeData.mazeScene.hide();

            Ctx.m_instance.m_maze.mazeData.mazePlayer.sceneEffect.stop();
            Ctx.m_instance.m_maze.mazeData.mazePlayer.sceneEffect.setTableID(31);
            Ctx.m_instance.m_maze.mazeData.mazePlayer.sceneEffect.play();

            Ctx.m_instance.m_maze.mazeData.getWayPtList();
            Ctx.m_instance.m_maze.mazeData.setStartPos();
            Ctx.m_instance.m_maze.mazeData.startMove();

            if (Ctx.m_instance.m_maze.mazeData.curSceneIdx == (int)eSceneIndex.eSecond)
            {
                Ctx.m_instance.m_maze.mazeData.roomInfo.showDarkWin();
                Ctx.m_instance.m_maze.mazeData.mazePlayer.show();
                MazePlayerTrackAniControl.sTime = 2.0f;
            }
            else
            {
                MazePlayerTrackAniControl.sTime = 1.0f;
            }

            exit();
        }

        // 重置按钮
        protected void onResetBtnClk()
        {
            Ctx.m_instance.m_maze.mazeData.roomInfo.resetInitState();
        }

        public void enterSecond()
        {
            RectTransform trans = m_btnGo.GetComponent<RectTransform>();
            UtilApi.setRectPos(trans, new Vector3(-449, 172, 0));
        }

        public void toggleResetBtn(bool bShow)
        {
            UtilApi.SetActive(m_btnResetGo, bShow);
        }
    }
}
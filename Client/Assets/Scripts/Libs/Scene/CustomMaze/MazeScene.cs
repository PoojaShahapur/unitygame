using Game.UI;
using UnityEngine;

namespace SDK.Lib
{
    public enum eSceneIndex
    {
        eFirst,
        eSecond
    }

    /**
     *@brief 场景逻辑
     */
    public class MazeScene
    {
        protected GameObject m_bigStartPnl;
        protected GameObject m_smallStarPnl_0;
        protected GameObject m_smallStarPnl_1;
        protected GameObject m_smallStarPnl_2;

        protected TimerItemBase m_bigStartTimer;
        protected TimerItemBase m_smallStar0fTimer;
        protected TimerItemBase m_smallStar1fTimer;
        protected TimerItemBase m_smallStar2fTimer;

        protected TimerItemBase m_enterTimer;

        public MazeScene()
        {

        }

        public void init()
        {
            if (Ctx.m_instance.m_maze.mazeData.bInFisrstScene())
            {
                m_bigStartPnl = UtilApi.GoFindChildByPObjAndName("RootGo/Plane_4/BigStartPnl");
                m_smallStarPnl_0 = UtilApi.GoFindChildByPObjAndName("RootGo/Plane_4/SmallStarPnl_0");
                m_smallStarPnl_1 = UtilApi.GoFindChildByPObjAndName("RootGo/Plane_4/SmallStarPnl_1");
                m_smallStarPnl_2 = UtilApi.GoFindChildByPObjAndName("RootGo/Plane_4/SmallStarPnl_2");
            }
        }

        public void hide()
        {
            if (Ctx.m_instance.m_maze.mazeData.bInFisrstScene())
            {
                UtilApi.SetActive(m_bigStartPnl, false);
                UtilApi.SetActive(m_smallStarPnl_0, false);
                UtilApi.SetActive(m_smallStarPnl_1, false);
                UtilApi.SetActive(m_smallStarPnl_2, false);
            }
        }

        public void show()
        {
            if (Ctx.m_instance.m_maze.mazeData.bInFisrstScene())
            {
                UtilApi.SetActive(m_bigStartPnl, true);
                UtilApi.SetActive(m_smallStarPnl_0, true);
                UtilApi.SetActive(m_smallStarPnl_1, true);
                UtilApi.SetActive(m_smallStarPnl_2, true);
            }
        }

        public void loadSecondScene()
        {
            UIMaze uiMaze = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIMaze) as UIMaze;
            if(uiMaze != null)
            {
                uiMaze.enterSecond();
            }
            Ctx.m_instance.m_soundMgr.unloadAll();
            Ctx.m_instance.m_maze.mazeData.curSceneIdx = (int)eSceneIndex.eSecond;
            Ctx.m_instance.m_sceneSys.loadScene("MazeSecond.unity", onResLoadScene);
        }

        public void onResLoadScene(Scene scene)
        {
            Ctx.m_instance.m_maze.mazeData.init();
        }

        public void showStar()
        {
            startBigStartTimer();
            startSmallStar0fTimer();
            startSmallStar1fTimer();
            startSmallStar2fTimer();
            startEnterTimer();
        }

        // 启动初始化定时器
        protected void startBigStartTimer()
        {
            if (m_bigStartTimer == null)
            {
                m_bigStartTimer = new TimerItemBase();
            }
            else
            {
                m_bigStartTimer.reset();        // 重置内部数据
            }

            m_bigStartTimer.m_internal = 0.5f;
            m_bigStartTimer.m_totalTime = 0.5f;
            m_bigStartTimer.m_timerDisp.setFuncObject(onBigStartTimerEndHandle);

            Ctx.m_instance.m_timerMgr.addObject(m_bigStartTimer);
        }

        protected void startSmallStar0fTimer()
        {
            if (m_smallStar0fTimer == null)
            {
                m_smallStar0fTimer = new TimerItemBase();
            }
            else
            {
                m_smallStar0fTimer.reset();        // 重置内部数据
            }

            m_smallStar0fTimer.m_internal = 1.0f;
            m_smallStar0fTimer.m_totalTime = 1.0f;
            m_smallStar0fTimer.m_timerDisp.setFuncObject(onSmallStar0fTimerEndHandle);

            Ctx.m_instance.m_timerMgr.addObject(m_smallStar0fTimer);
        }

        protected void startSmallStar1fTimer()
        {
            if (m_smallStar1fTimer == null)
            {
                m_smallStar1fTimer = new TimerItemBase();
            }
            else
            {
                m_smallStar1fTimer.reset();        // 重置内部数据
            }

            m_smallStar1fTimer.m_internal = 1.5f;
            m_smallStar1fTimer.m_totalTime = 1.5f;
            m_smallStar1fTimer.m_timerDisp.setFuncObject(onSmallStar1fTimerEndHandle);

            Ctx.m_instance.m_timerMgr.addObject(m_smallStar1fTimer);
        }

        protected void startSmallStar2fTimer()
        {
            if (m_smallStar2fTimer == null)
            {
                m_smallStar2fTimer = new TimerItemBase();
            }
            else
            {
                m_smallStar2fTimer.reset();        // 重置内部数据
            }

            m_smallStar2fTimer.m_internal = 2.0f;
            m_smallStar2fTimer.m_totalTime = 2.0f;
            m_smallStar2fTimer.m_timerDisp.setFuncObject(onSmallStar2fTimerEndHandle);

            Ctx.m_instance.m_timerMgr.addObject(m_smallStar2fTimer);
        }

        protected void startEnterTimer()
        {
            if (m_enterTimer == null)
            {
                m_enterTimer = new TimerItemBase();
            }
            else
            {
                m_enterTimer.reset();        // 重置内部数据
            }

            m_enterTimer.m_internal = 15.0f;
            m_enterTimer.m_totalTime = 15.0f;
            m_enterTimer.m_timerDisp.setFuncObject(onEnterTimerEndHandle);

            Ctx.m_instance.m_timerMgr.addObject(m_enterTimer);
        }

        protected void stopTimer()
        {
            if (m_bigStartTimer != null)
            {
                Ctx.m_instance.m_timerMgr.delObject(m_bigStartTimer);
            }
            if(m_smallStar0fTimer != null)
            {
                Ctx.m_instance.m_timerMgr.delObject(m_smallStar0fTimer);
            }
            if (m_smallStar1fTimer != null)
            {
                Ctx.m_instance.m_timerMgr.delObject(m_smallStar1fTimer);
            }
            if (m_smallStar2fTimer != null)
            {
                Ctx.m_instance.m_timerMgr.delObject(m_smallStar2fTimer);
            }
        }

        public void onBigStartTimerEndHandle(TimerItemBase timer)
        {
            UtilApi.SetActive(m_bigStartPnl, true);
        }

        public void onSmallStar0fTimerEndHandle(TimerItemBase timer)
        {
            UtilApi.SetActive(m_smallStarPnl_0, true);
        }

        public void onSmallStar1fTimerEndHandle(TimerItemBase timer)
        {
            UtilApi.SetActive(m_smallStarPnl_1, true);
        }

        public void onSmallStar2fTimerEndHandle(TimerItemBase timer)
        {
            UtilApi.SetActive(m_smallStarPnl_2, true);
        }

        public void onEnterTimerEndHandle(TimerItemBase timer)
        {
            if (Ctx.m_instance.m_maze.mazeData.curSceneIdx == (int)eSceneIndex.eFirst)
            {
                Ctx.m_instance.m_maze.mazeData.mazeScene.loadSecondScene();
            }
        }
    }
}
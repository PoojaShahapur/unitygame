using SDK.Common;
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

        public MazeScene()
        {

        }

        public void init()
        {
            if (Ctx.m_instance.m_maze.mazeData.bInFisrstScene())
            {
                m_bigStartPnl = UtilApi.GoFindChildByPObjAndName("RootGo/RightPnl/BigStartPnl");
                m_smallStarPnl_0 = UtilApi.GoFindChildByPObjAndName("RootGo/RightPnl/SmallStarPnl_0");
                m_smallStarPnl_1 = UtilApi.GoFindChildByPObjAndName("RootGo/RightPnl/SmallStarPnl_1");
                m_smallStarPnl_2 = UtilApi.GoFindChildByPObjAndName("RootGo/RightPnl/SmallStarPnl_2");
            }
        }

        public void hide()
        {
            UtilApi.SetActive(m_bigStartPnl, false);
            UtilApi.SetActive(m_smallStarPnl_0, false);
            UtilApi.SetActive(m_smallStarPnl_1, false);
            UtilApi.SetActive(m_smallStarPnl_2, false);
        }

        public void show()
        {
            UtilApi.SetActive(m_bigStartPnl, true);
            UtilApi.SetActive(m_smallStarPnl_0, true);
            UtilApi.SetActive(m_smallStarPnl_1, true);
            UtilApi.SetActive(m_smallStarPnl_2, true);
        }

        public void loadSecondScene()
        {
            Ctx.m_instance.m_maze.mazeData.curSceneIdx = (int)eSceneIndex.eSecond;
            Ctx.m_instance.m_sceneSys.loadScene("MazeSecond.unity", onResLoadScene);
        }

        public void onResLoadScene(Scene scene)
        {

        }
    }
}
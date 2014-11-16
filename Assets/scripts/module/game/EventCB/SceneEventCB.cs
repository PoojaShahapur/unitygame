using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace Game.Game
{
    public class SceneEventCB : ISceneEventCB
    {
        // 场景加载完成处理事件
        public void onLevelLoaded()
        {
            Ctx.m_instance.m_camSys.m_sceneCam.onSceneLoaded();
            // 创建
            IPlayerMain playerMain = Ctx.m_instance.m_playerMgr.createHero();
            Ctx.m_instance.m_playerMgr.add(playerMain);
            playerMain.setSkeleton("TestBeing");
        }
    }
}

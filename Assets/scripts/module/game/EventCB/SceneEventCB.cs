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
            Ctx.m_instance.m_playerMgr.addHero(playerMain);
            //playerMain.setSkeleton("DefaultAvatar_Unity_Body_Mesh");
            //playerMain.setSkeleton("DefaultAvatar");
            playerMain.setSkeleton("TestBeing");

            playerMain.setPartModel(PlayerModelDef.eModelHead, "DefaultAvatar_Unity_Body_Mesh", "Unity_Body_Mesh");
            playerMain.setPartModel(PlayerModelDef.eModelChest, "DefaultAvatar_Lw_Teeth_Mesh", "Lw_Teeth_Mesh");
            playerMain.setPartModel(PlayerModelDef.eModelWaist, "DefaultAvatar_Tounge_Mesh", "Tounge_Mesh");
            playerMain.setPartModel(PlayerModelDef.eModelLeg, "DefaultAvatar_Up_Teeth_Mesh", "Up_Teeth_Mesh");

            playerMain.addAiByID("1001");
        }
    }
}
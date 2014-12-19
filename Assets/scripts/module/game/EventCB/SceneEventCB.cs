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
            // 创建主角
            createHero();
            // 创建怪物
            //createMonster(new Vector3(0, 0, 0));
            //createMonster(new Vector3(10, 0, 0));
            //createMonster(new Vector3(6, 0, 0));
            createMonster(new Vector3(11, 0, 0));
            createMonster(new Vector3(12, 0, 0));
        }

        public void createHero()
        {
            IPlayerMain playerMain = Ctx.m_instance.m_playerMgr.createHero();
            Ctx.m_instance.m_playerMgr.addHero(playerMain);
            //playerMain.setSkeleton("DefaultAvatar_Unity_Body_Mesh");
            playerMain.setSkeleton("DefaultAvatar");
            //playerMain.setSkeleton("TestBeing");

            playerMain.setPartModel((int)PlayerModelDef.eModelHead, "DefaultAvatar_Unity_Body_Mesh", "Unity_Body_Mesh");
            playerMain.setPartModel((int)PlayerModelDef.eModelChest, "DefaultAvatar_Lw_Teeth_Mesh", "Lw_Teeth_Mesh");
            playerMain.setPartModel((int)PlayerModelDef.eModelWaist, "DefaultAvatar_Tounge_Mesh", "Tounge_Mesh");
            playerMain.setPartModel((int)PlayerModelDef.eModelLeg, "DefaultAvatar_Up_Teeth_Mesh", "Up_Teeth_Mesh");

            //playerMain.addAiByID("1001");
        }

        public void createMonster(Vector3 pos)
        {
            IMonster monster = Ctx.m_instance.m_monsterMgr.createMonster();
            Ctx.m_instance.m_monsterMgr.add(monster);
            monster.setSkeleton("DefaultAvatar");
            monster.setLocalPos(pos);
            monster.setPartModel((int)MonstersModelDef.eModelBody, "DefaultAvatar_Unity_Body_Mesh", "Unity_Body_Mesh");
            monster.addAiByID("1002");
        }
    }
}
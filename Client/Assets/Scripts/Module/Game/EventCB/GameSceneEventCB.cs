﻿using BehaviorLibrary;
using SDK.Lib;
using SDK.Lib;
using UnityEngine;

namespace Game.Game
{
    public class GameSceneEventCB : ISceneEventCB
    {
        // 场景加载完成处理事件
        public void onLevelLoaded()
        {
            //Ctx.m_instance.m_camSys.m_sceneCam.onSceneLoaded();
            // 创建主角
            //createHero();
            // 创建怪物
            //createMonster(new Vector3(0, 0, 0));
            //createMonster(new Vector3(10, 0, 0));
            //createMonster(new Vector3(6, 0, 0));
            //createMonster(new Vector3(11, 0, 0));
            //createMonster(new Vector3(12, 0, 0));
        }

        public void createHero()
        {
            PlayerMain playerMain = Ctx.m_instance.m_playerMgr.createHero();
            Ctx.m_instance.m_playerMgr.addHero(playerMain);
            //playerMain.setSkeleton("DefaultAvatar_Unity_Body_Mesh");
            playerMain.setSkeleton("DefaultAvatar");
            //playerMain.setSkeleton("TestBeing");

            playerMain.setPartModel((int)ePlayerModelType.eModelHead, "DefaultAvatar_Unity_Body_Mesh", "Unity_Body_Mesh");
            playerMain.setPartModel((int)ePlayerModelType.eModelChest, "DefaultAvatar_Lw_Teeth_Mesh", "Lw_Teeth_Mesh");
            playerMain.setPartModel((int)ePlayerModelType.eModelWaist, "DefaultAvatar_Tounge_Mesh", "Tounge_Mesh");
            playerMain.setPartModel((int)ePlayerModelType.eModelLeg, "DefaultAvatar_Up_Teeth_Mesh", "Up_Teeth_Mesh");

            //playerMain.addAiByID("1001");
        }

        public void createMonster(Vector3 pos)
        {
            Monster monster = Ctx.m_instance.m_monsterMgr.createMonster();
            Ctx.m_instance.m_monsterMgr.addMonster(monster);
            monster.setSkeleton("DefaultAvatar");
            monster.setLocalPos(pos);
            monster.setPartModel((int)eMonstersModelType.eModelBody, "DefaultAvatar_Unity_Body_Mesh", "Unity_Body_Mesh");
            monster.addAiByID(BTID.e1000);
        }
    }
}
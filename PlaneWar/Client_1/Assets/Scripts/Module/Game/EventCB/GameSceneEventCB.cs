using SDK.Lib;
using UnityEngine;

#if UNIT_TEST
using UnitTest;
#endif

namespace Game.Game
{
    public class GameSceneEventCB : ISceneEventCB
    {
        // 场景加载完成处理事件
        public void onLevelLoaded()
        {
            Ctx.mInstance.mLuaSystem.onSceneLoaded();

            // 关联相机
            GameObject cam = UtilApi.GoFindChildByName("MainCamera").gameObject;
            Ctx.mInstance.mCamSys.setMainCamera(cam.GetComponent<Camera>());

            Ctx.mInstance.mClipRect.setCam(cam.GetComponent<Camera>());

            Ctx.mInstance.mTileMgr.setWorldWidthHeight(
                (int)Ctx.mInstance.mSnowBallCfg.mXmlSnowBallCfg.mXmlItemMap.mWidth,
                (int)Ctx.mInstance.mSnowBallCfg.mXmlSnowBallCfg.mXmlItemMap.mWidth);

            Ctx.mInstance.mTwoDTerrain.setWorldWidthHeight(
                (int)Ctx.mInstance.mSnowBallCfg.mXmlSnowBallCfg.mXmlItemMap.mWidth,
                (int)Ctx.mInstance.mSnowBallCfg.mXmlSnowBallCfg.mXmlItemMap.mWidth);

            this.initEntity();

            // 创建主角
            //Ctx.mInstance.mPlayerMgr.createPlayerMain();
            // 创建机器人
            //Ctx.mInstance.mRobotMgr.CreateSnowFood();
            // 创建 SnowBall
            //Ctx.mInstance.mSnowBlockMgr.CreateASnowBlock();
            // 创建所有的雪球
            //Ctx.mInstance.mSnowBlockMgr.createAllSnowFood();
            // 创建所有的 Robot
            //Ctx.mInstance.mRobotMgr.createAllRobot();
            //Ctx.mInstance.mUiMgr.loadAndShow(UIFormId.eUIPack);

            // 场景加载完成，关闭登陆界面
            Ctx.mInstance.mLuaSystem.exitForm((int)SDK.Lib.UIFormId.eUIStartGame_Lua);
            runTest();
        }

        protected void runTest()
        {
            // 运行单元测试
#if UNIT_TEST
            TestMain pTestMain = new TestMain();
            pTestMain.run();
#endif
            //Ctx.mInstance.mNetMgr.openSocket("106.14.32.169", 20013);
        }

        protected void init()
        {

        }

        private Giant.NewItem newItem = new Giant.NewItem();
        // 实例化场景 Entity
        protected void initEntity()
        {
            foreach (var food in Ctx.mInstance.mDataPlayer.mDataHero.mRoom.foods)
            {
                newItem.objid = food.food_id;
                newItem.pos.Set(food.x, food.y);
                (Ctx.mInstance.mGameSys as GameSys).mGameNetHandleCB.HandleSceneCommand(newItem);
            }

            foreach (var player in Ctx.mInstance.mDataPlayer.mDataHero.mRoom.players)
            {
                (Ctx.mInstance.mGameSys as GameSys).mGameNetHandleCB.JoinTeam(player, player.id == Ctx.mInstance.mDataPlayer.mDataHero.mRoom.ms_and_id.id);
                //this.uiFight.OnNewPlayer(player.id);
            }

            GlobalEventCmd.onEnterWorld();

            Ctx.mInstance.mClipRect.setIsRectDirty(true);   // 强制重新裁剪
        }
    }
}
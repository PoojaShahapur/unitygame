using SDK.Lib;

namespace Game.Game
{
    /**
     * @brief 消息处理流程
     */
    public class ReqSceneInteractive
    {
        private static double lastReqSplitTime = 0.0f;
        private static double lastReqSwallowTime = 0.0f;
        private static double intervalTime = 0.1f;

        private static plane.PlaneMsg planeMsg = new plane.PlaneMsg();

        // 检查 Child 并且发送主角移动
        public static void checkChildAndSendPlayerMove()
        {
            
        }

        public static bool isPosOrRotateChanged()
        {
            bool ret = false;

            PlayerChildMgr playerChildMgr = null;
            if (Ctx.mInstance.mPlayerMgr.getHero() != null && Ctx.mInstance.mPlayerMgr.getHero().mPlayerSplitMerge != null)
                playerChildMgr = Ctx.mInstance.mPlayerMgr.getHero().mPlayerSplitMerge.mPlayerChildMgr;
            if (playerChildMgr == null)
                return ret;

            int idx = 0;
            int len = playerChildMgr.getEntityCount();
            PlayerChild playerChild = null;

            if (len > 0)
            {
                while (idx < len)
                {
                    playerChild = playerChildMgr.getEntityByIndex(idx) as PlayerChild;

                    if(BeingState.eBSSeparation == playerChild.getBeingState())
                    {
                        ret = true;
                        break;
                    }

                    ++idx;
                }
            }

            return ret;
        }

        // 设置主角是否停止
        public static void setMainChildStop(bool value)
        {
            
        }

        static private plane.MoveToMsg moveMsg = new plane.MoveToMsg();
        static public void sendHeroMove()
        {
            if (Ctx.mInstance.mPlayerMgr.isMainPosOrOrientChanged() && null != Ctx.mInstance.mPlayerMgr.getHero())
            {
                UnityEngine.Vector3 curPos = Ctx.mInstance.mPlayerMgr.getHero().getPos();
                moveMsg.x = curPos.x;
                moveMsg.y = curPos.y;

                //moveMsg.movings.Clear();

                //int index = 0;
                //int len = Ctx.mInstance.mPlayerMgr.getHero().mPlayerSplitMerge.mPlayerChildMgr.getEntityCount();
                //PlayerMainChild child = null;

                //while (index < len)
                //{
                //    child = Ctx.mInstance.mPlayerMgr.getHero().mPlayerSplitMerge.mPlayerChildMgr.getEntityByIndex(index) as PlayerMainChild;

                //    var move = new plane.MoveToMsg.OneMove();
                //    curPos = child.getPos();
                //    move.plane_id = child.getThisId();
                //    move.x = curPos.x;
                //    move.y = curPos.y;
                //    moveMsg.movings.Add(move);

                //    ++index;
                //}

                //请求转向
                moveMsg.angle = Ctx.mInstance.mPlayerMgr.getHero().getRotateEulerAngle().z;
                Ctx.mInstance.mServerHandler_GB.RequestSend("plane.Plane", "MoveTo", moveMsg);
            }
        }

        // 发射 Bullet
        static private Giant.TeamShoot teamShoot = new Giant.TeamShoot();
        static private plane.FireMsg fireMsg = new plane.FireMsg();
        public static void sendBullet()
        {
            if (null != Ctx.mInstance.mPlayerMgr.getHero())
            {
                UnityEngine.Vector3 pos = Ctx.mInstance.mPlayerMgr.getHero().getPos();

                fireMsg.bullet_team_id = Ctx.mInstance.mPlayerMgr.getHero().mBulletIDGentor.genNewId();
                fireMsg.angle = Ctx.mInstance.mPlayerMgr.getHero().getRotateEulerAngle().z;
                fireMsg.x = pos.x;
                fireMsg.y = pos.y;
                Ctx.mInstance.mServerHandler_GB.RequestSend("plane.Plane", "Fire", fireMsg);

                (Ctx.mInstance.mGameSys as GameSys).mGameNetHandleCB.HandleSceneCommand(Ctx.mInstance.mPlayerMgr.getHero().getThisId(), fireMsg);
            }
        }

        // 发送命中自己
        public static void sendHitSelfChild(FlyBullet bullet, PlayerMainChild child)
        {
            
        }

        // 发送命中别人
        public static void sendHitOtherChild(FlyBullet bullet, PlayerOtherChild child)
        {
            var cmd = new plane.HitMsg();
            cmd.bullet_id = bullet.getThisId();
            cmd.bullet_team_id = bullet.mFlyBulletFlock.getThisId();
            cmd.plane_id = child.getThisId();
            cmd.target_user_id = child.mParentPlayer.getThisId();
            Ctx.mInstance.mServerHandler_GB.RequestSend("plane.Plane", "Hit", cmd);
            //自己处理
            (Ctx.mInstance.mGameSys as GameSys).mGameNetHandleCB.HandleSceneCommand(bullet.mFlyBulletFlock.getOwnerPlayerThisId(), cmd);

            //增加飞机
            planeMsg.plane_id = bullet.mFlyBulletFlock.getOwnerPlayerLocalId();
            Ctx.mInstance.mServerHandler_GB.RequestSend("plane.Plane", "New", planeMsg);
            (Ctx.mInstance.mGameSys as GameSys).mGameNetHandleCB.HandleSceneCommand(bullet.mFlyBulletFlock.getOwnerPlayerThisId(), planeMsg, true);
            //scene.uiFight.OnPlayerChange(tranlge.team.objctid);
        }

        // 发送自己命中能源
        public static void sendHitEnergy(FlyBullet bullet, SnowBlock block)
        {
            //通知其他玩家
            var cmd = new plane.EatMsg();
            cmd.bullet_id = bullet.getThisId();
            cmd.bullet_team_id = bullet.mFlyBulletFlock.getThisId();
            cmd.food_id = block.getThisId();
            Ctx.mInstance.mServerHandler_GB.RequestSend("plane.Plane", "Eat", cmd);

            // 保存，因为 HandleSceneCommand 中释放资源
            uint localId = bullet.mFlyBulletFlock.getOwnerPlayerLocalId();
            uint thisId = bullet.mFlyBulletFlock.getOwnerPlayerThisId();

            //自己处理
            (Ctx.mInstance.mGameSys as GameSys).mGameNetHandleCB.HandleSceneCommand(bullet.mFlyBulletFlock.getOwnerPlayerThisId(), cmd);

            //增加飞机
            planeMsg.plane_id = localId;
            Ctx.mInstance.mServerHandler_GB.RequestSend("plane.Plane", "New", planeMsg);
            (Ctx.mInstance.mGameSys as GameSys).mGameNetHandleCB.HandleSceneCommand(thisId, planeMsg, true);
            //scene.uiFight.OnPlayerChange(team.team.objctid);
        }

        // 发送自己命中机器人
        public static void sendHitAI(FlyBullet bullet, ComputerBall robot)
        {
            
        }
    }
}
using SDK.Lib;

namespace Game.Game
{
    public class SceneRouteHandle : MsgRouteHandleBase
    {
        protected UnityEngine.Rect rect = new UnityEngine.Rect(0, 0, 100, 100);

        public SceneRouteHandle()
        {

        }

        override public void init()
        {
            this.addMsgRouteHandle(MsgRouteID.eMRID_NewItem, this.OnNewItem);
            this.addMsgRouteHandle(MsgRouteID.eMRID_JoinTeam, this.OnJoinTeam);
            this.addMsgRouteHandle(MsgRouteID.eMRID_HitTrangleCommand, this.OnHitTrangleCommand);
            this.addMsgRouteHandle(MsgRouteID.eMRID_HitItemCommand, this.OnHitItemCommand);
            this.addMsgRouteHandle(MsgRouteID.eMRID_TrangleTeamLeave, this.OnTrangleTeamLeave);
        }

        override public void dispose()
        {

        }

        protected void OnTrangleTeamChange(uint teamid)
        {
            //uiFight.OnPlayerChange(teamid);
        }

        protected void OnJoinTeam(IDispatchObject dispObj)
        {
            Giant.JoinTeam cmd = dispObj as Giant.JoinTeam;
            this.OnJoinTeam(cmd);
        }

        protected void OnJoinTeam(Giant.JoinTeam cmd)
        {
            Player player = null;
            if (cmd.isself)
            {
                //this.myTeam = SceneObject.Create<MainTrangleTeam>(mapT, "Prefabs/TrangleTeam", cmd);
                //this.myTeam.objctid = cmd.teamID;
                //teams.Add(cmd.teamID, myTeam);

                player = new PlayerMain();
            }
            else
            {
                //base.OnJoinTeam(cmd);
                player = new PlayerOther();
            }

            player.setThisId(cmd.teamID);
            // 初始化参数
            (Ctx.mInstance.mGameSys as GameSys).mGameRouteCB.mPlayerRouteHandle.OnLoadEntity(player, cmd);

            // 位置没有设置
            player.init();
        }

        protected void OnNewItem(IDispatchObject dispObj)
        {
            Giant.NewItem cmd = dispObj as Giant.NewItem;
            this.OnNewItem(cmd);
        }

        protected void OnNewItem(Giant.NewItem cmd)
        {
            //var item = SceneObject.Create<Giant.SceneFood>(mapT, "Prefabs/star");
            //item.objctid = cmd.objid;
            //cmd.pos.x = rect.xMin + rect.width * cmd.pos.x;
            //cmd.pos.y = rect.yMin + rect.height * cmd.pos.y;
            //item.pos = cmd.pos;
            //items.Add(cmd.objid, item);

            SnowBlock food = null;
            food = new SnowBlock();
            food.setThisId(cmd.objid);
            cmd.pos.x = rect.xMin + rect.width * cmd.pos.x;
            cmd.pos.y = rect.yMin + rect.height * cmd.pos.y;
            food.setPos(cmd.pos);
            food.init();
        }

        protected void OnHitTrangleCommand(IDispatchObject dispObj)
        {
            Giant.HitTrangleCommand cmd = dispObj as Giant.HitTrangleCommand;
            this.OnHitTrangleCommand(cmd);
        }

        protected void OnHitTrangleCommand(Giant.HitTrangleCommand cmd)
        {
            //Giant.TrangleTeam team;
            //Giant.TrangleTeam targetTeam;
            //if (teams.TryGetValue(cmd.teamID, out team) && teams.TryGetValue(cmd.targetTeamID, out targetTeam))
            //{
            //    var bullet = team.GetBullet(cmd.bulletTeamid, cmd.bulletid);
            //    if (bullet != null)
            //    {
            //        bullet.KillBy(DeathType.eKillByOther);

            //        var trangle = targetTeam.GetTrangle(cmd.targetTrangleID);
            //        if (trangle != null)
            //        {
            //            trangle.KillBy(DeathType.eKillByOther);
            //            this.OnTrangleTeamChange(cmd.targetTeamID);
            //        }
            //    }
            //}

            Player player = Ctx.mInstance.mPlayerMgr.getEntityByThisId(cmd.teamID) as Player;
            Player targetPlayer = Ctx.mInstance.mPlayerMgr.getEntityByThisId(cmd.targetTeamID) as Player;

            if (null != player && null != targetPlayer)
            {
                FlyBulletFlock flock = Ctx.mInstance.mFlyBulletFlockMgr.getBulletFlockByThisId(cmd.bulletTeamid);
                if (null != flock)
                {
                    FlyBullet bullet = flock.mFlyBulletMgr.getEntityByThisId(cmd.bulletid) as FlyBullet;
                    if (null != bullet)
                    {
                        bullet.dispose();
                    }
                }

                PlayerChild playerChild = targetPlayer.mPlayerSplitMerge.mPlayerChildMgr.getEntityByThisId(cmd.targetTrangleID) as PlayerChild;

                if (null != playerChild)
                {
                    playerChild.dispose();
                    this.OnTrangleTeamChange(cmd.targetTeamID);
                }
            }
        }

        protected void OnHitItemCommand(IDispatchObject dispObj)
        {
            Giant.HitItemCommand cmd = dispObj as Giant.HitItemCommand;
            this.OnHitItemCommand(cmd);
        }

        protected void OnHitItemCommand(Giant.HitItemCommand cmd)
        {
            //Giant.TrangleTeam team;
            //if (teams.TryGetValue(cmd.teamID, out team))
            //{
            //    var bullet = team.GetBullet(cmd.bulletTeamid, cmd.bulletid);
            //    if (bullet != null)
            //    {
            //        bullet.KillBy(Giant.DeathType.eKillByOther);
            //        Giant.SceneFood item;
            //        if (items.TryGetValue(cmd.itemID, out item))
            //        {
            //            item.KillBy(DeathType.eKillByOther);
            //        }
            //    }
            //}

            Player player = Ctx.mInstance.mPlayerMgr.getEntityByThisId(cmd.teamID) as Player;
            if (null != player)
            {
                FlyBulletFlock flock = Ctx.mInstance.mFlyBulletFlockMgr.getBulletFlockByThisId(cmd.bulletTeamid);
                if (null != flock)
                {
                    FlyBullet bullet = flock.mFlyBulletMgr.getEntityByThisId(cmd.bulletid) as FlyBullet;
                    if (null != bullet)
                    {
                        bullet.dispose();
                    }
                }

                SnowBlock food = Ctx.mInstance.mSnowBlockMgr.getEntityByThisId(cmd.itemID) as SnowBlock;
                if (null != food)
                {
                    food.dispose();
                }
            }
        }

        protected void OnTrangleTeamLeave(IDispatchObject dispObj)
        {
            Giant.TrangleTeamLeave cmd = dispObj as Giant.TrangleTeamLeave;
            this.OnTrangleTeamLeave(cmd);
        }

        protected void OnTrangleTeamLeave(Giant.TrangleTeamLeave cmd)
        {
            //Giant.TrangleTeam team;
            //if (teams.TryGetValue(cmd.teamID, out team))
            //{
            //    team.DestoryThis();
            //    teams.Remove(cmd.teamID);
            //}

            Player player = Ctx.mInstance.mPlayerMgr.getEntityByThisId(cmd.teamID) as Player;
            if (null != player)
            {
                player.dispose();
            }
        }
    }
}
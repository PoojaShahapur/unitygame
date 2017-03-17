using SDK.Lib;
using System.Collections.Generic;

namespace Game.Game
{
    public class PlayerRouteHandle : MsgRouteHandleBase
    {
        protected Giant.JoinTeam _teamInfo;

        public PlayerRouteHandle()
        {

        }

        override public void init()
        {
            this.addMsgRouteHandle(MsgRouteID.eMRID_MoveTeam, this.OnMoveTeam);
            this.addMsgRouteHandle(MsgRouteID.eMRID_NewTrangle, this.OnNewTrangle);
            this.addMsgRouteHandle(MsgRouteID.eMRID_RemoveTrangle, this.OnRemoveTrangle);
            this.addMsgRouteHandle(MsgRouteID.eMRID_TeamShoot, this.OnTeamShoot);

            this.addMsgRouteHandle(MsgRouteID.eMRID_MoveTeamBullet, this.OnMoveTeamBullet);
            this.addMsgRouteHandle(MsgRouteID.eMRID_BulletTimeOut, this.OnBulletTimeOut);
        }

        override public void dispose()
        {

        }

        public void OnLoadEntity(Player player, object obj)
        {
            this._teamInfo = obj as Giant.JoinTeam;
            player.setName(this._teamInfo.name);
            player.setMoveSpeed(this._teamInfo.moveSpeed);

            if (this._teamInfo.move.moves.Count == 0)
            {
                this._teamInfo.move.angle = 0;
                this._teamInfo.move.teamPos = UnityEngine.Vector2.zero;
                for (int i = 0; i < 3; ++i)
                {
                    this._teamInfo.move.moves.Add(player.mUniqueNumIdGen.genNewId(), UnityEngine.Vector2.zero);
                }
            }
            this.initTrangles(player);
        }

        public void initTrangles(Player player)
        {
            if (_teamInfo != null)
            {
                player.setPos(_teamInfo.move.teamPos);
                player.setInvincibleTime(_teamInfo.invincibleTime);
                foreach (var pair in _teamInfo.move.moves)
                {
                    PlayerChild trangle = AddTrangle(player, (int)pair.Key);
                    trangle.setPos(pair.Value);
                    trangle.setMoveSpeed(player.getMoveSpeed());
                }
                player.setRotateEulerAngle(new UnityEngine.Vector3(0, 0, _teamInfo.move.angle));
            }
        }

        public PlayerChild AddTrangle(Player player, int id = -1)
        {
            //int layer = LayerManager.OtherTrangle;
            //if (IsMainTeam)
            //    layer = LayerManager.MainTrangle;
            //var trangle = SceneObject.Create<Trangle>(transform, "Prefabs/trangle", null, layer);
            //trangle.objctid = (uint)(id < 0 ? (int)AllocTrangleID() : id);
            //trangle.team = this;
            //trangle.angle = this.angle;
            //trangle.UpdateTrigger();
            //trangles.Add(trangle.objctid, trangle);

            //trangle.GetComponent<Joint2D>().connectedBody = this.entityRigitBody;
            ////if (this.IsInvincible)
            ////   trangle.Blink(this.invincibleTime);
            //return trangle;

            PlayerChild trangle = null;

            if(EntityType.ePlayerMain == player.getEntityType())
            {
                trangle = new PlayerMainChild(player);
            }
            else
            {
                trangle = new PlayerOtherChild(player);
            }

            trangle.setThisId((uint)(id < 0 ? (int)player.mUniqueNumIdGen.genNewId() : id));
            trangle.setRotateEulerAngle(player.getRotateEulerAngle());
            trangle.setPos(player.getPos());
            trangle.setMoveSpeed(player.getMoveSpeed());
            trangle.init();

            return trangle;
        }

        protected void OnMoveTeam(IDispatchObject dispObj)
        {
            Giant.MoveTeam move = dispObj as Giant.MoveTeam;
            this.OnMoveTeam(move);
        }

        protected UnityEngine.Vector2 targetPos;
        protected float targetAngle;
        private uint updatePosFrame = 0;
        private float curSpeed = 0;
        private UnityEngine.Vector2 curMoveDir;
        protected void OnMoveTeam(Giant.MoveTeam move)
        {
            //if (trangles.Count == 0)
            //    this.pos = move.teamPos;
            //this.targetPos = move.teamPos;
            //this.targetAngle = move.angle;
            //this.updatePosFrame = 0;
            //this.curMoveDir = (this.targetPos - this.pos).normalized;
            //this.curSpeed = _teamInfo.moveSpeed;
            //if (!IsMainTeam)
            //{
            //    this.curSpeed = (this.targetPos - this.pos).magnitude / MainTrangleTeam.SynDeltaTime;
            //    _teamInfo.turnSpeed = Mathf.Abs(this.targetAngle - this.angle) / MainTrangleTeam.SynDeltaTime;
            //    foreach (var pair in move.moves)
            //    {
            //        var trangle = GetTrangle(pair.Key);
            //        if (trangle != null)
            //            trangle.pos = pair.Value;
            //    }
            //}

            Player player = Ctx.mInstance.mPlayerMgr.getEntityByThisId(move.teamID) as Player;

            if (player.mPlayerSplitMerge.mPlayerChildMgr.getEntityCount() == 0)
                player.setPos(move.teamPos);

            player.setDestPos(move.teamPos, false);
            player.setDestRotate(new UnityEngine.Vector3(0, 0, move.angle), false);
            player.setRotateNormalDir((new UnityEngine.Vector3(move.teamPos.x, move.teamPos.y, 0) - player.getPos()).normalized);
            player.setMoveSpeed(_teamInfo.moveSpeed);

            if (EntityType.ePlayerMain != player.getEntityType())
            {
                player.setMoveSpeed(_teamInfo.moveSpeed);

                PlayerOtherChild child = null;
                foreach (var pair in move.moves)
                {
                    child = player.mPlayerSplitMerge.mPlayerChildMgr.getEntityByThisId(pair.Key) as PlayerOtherChild;
                    if (child != null)
                    {
                        child.setDestPos(pair.Value, true);
                    }
                }
            }
        }

        protected void OnNewTrangle(IDispatchObject dispObj)
        {
            Giant.NewTrangle move = dispObj as Giant.NewTrangle;
            this.OnNewTrangle(move);
        }

        protected void OnNewTrangle(Giant.NewTrangle trangle)
        {
            Player player = Ctx.mInstance.mPlayerMgr.getEntityByThisId(trangle.teamID) as Player;

            AddTrangle(player, (int)trangle.trangleid);
        }

        protected void OnRemoveTrangle(IDispatchObject dispObj)
        {
            Giant.RemoveTrangle move = dispObj as Giant.RemoveTrangle;
            this.OnRemoveTrangle(move);
        }

        protected void OnRemoveTrangle(Giant.RemoveTrangle trangle)
        {
            Player player = Ctx.mInstance.mPlayerMgr.getEntityByThisId(trangle.teamID) as Player;

            RemoveTrangle(player, trangle.trangleid);
        }

        protected void OnTeamShoot(IDispatchObject dispObj)
        {
            Giant.TeamShoot move = dispObj as Giant.TeamShoot;
            this.OnTeamShoot(move);
        }

        public void OnTeamShoot(Giant.TeamShoot shoot)
        {
            //var bulletTeam = SceneObject.Create<BulletTeam>(transform.parent, "");
            //bulletTeam.objctid = shoot.bulletTeamID;
            //bulletTeam.team = scene.GetTrangleTeam(shoot.teamID);
            //bulletTeam.timeOut = shoot.timeOut;
            //bulletTeam.pos = shoot.pos;
            //bulletTeam.speed = shoot.speed;
            //foreach (var pair in trangles)
            //{
            //    var trangle = pair.Value;
            //    bulletTeam.AddBullet(trangle.objctid, trangle.pos);
            //}
            //bulletTeam.angle = shoot.angle;
            //bulletTeams.Add(shoot.bulletTeamID, bulletTeam);

            Player player = Ctx.mInstance.mPlayerMgr.getEntityByThisId(shoot.teamID) as Player;

            FlyBulletFlock team = new FlyBulletFlock();
            team.setThisId(shoot.bulletTeamID);
            team.setOwnerPlayerThisId(shoot.teamID);
            team.TimeOut = shoot.timeOut;
            team.setPos(shoot.pos);
            team.setMoveSpeed(shoot.speed);

            team.init();

            int index = 0;
            int len = player.mPlayerSplitMerge.mPlayerChildMgr.getEntityCount();
            PlayerChild child = null;
            FlyBullet bullet = null;

            while(index < len)
            {
                child = player.mPlayerSplitMerge.mPlayerChildMgr.getEntityByIndex(index) as PlayerChild;
                bullet = new FlyBullet(team);
                bullet.setThisId(child.getThisId());
                bullet.setPos(child.getPos());
                bullet.setRotateEulerAngle(child.getRotateEulerAngle());
                UnityEngine.Vector3 destpos = bullet.getPos() + child.getRotate() * new UnityEngine.Vector3(0, shoot.speed * shoot.timeOut, 0);
                bullet.setMoveSpeed(shoot.speed);

                bullet.init();
                bullet.setDestPos(destpos, false);

                ++index;
            }
        }

        protected void OnMoveTeamBullet(IDispatchObject dispObj)
        {
            Giant.MoveTeamBullet move = dispObj as Giant.MoveTeamBullet;
            this.OnMoveTeamBullet(move);
        }

        protected void OnMoveTeamBullet(Giant.MoveTeamBullet cmd)
        {
            Player player = Ctx.mInstance.mPlayerMgr.getEntityByThisId(cmd.teamID) as Player;

            FlyBulletFlock team = Ctx.mInstance.mFlyBulletFlockMgr.getBulletFlockByThisId(cmd.bulletID);
            if (null != team)
            {
                team.setPos(cmd.pos);
            }
        }

        protected void OnBulletTimeOut(IDispatchObject dispObj)
        {
            Giant.BulletTimeOut move = dispObj as Giant.BulletTimeOut;
            this.OnBulletTimeOut(move);
        }

        protected void OnBulletTimeOut(Giant.BulletTimeOut cmd)
        {
            Player player = Ctx.mInstance.mPlayerMgr.getEntityByThisId(cmd.teamID) as Player;

            FlyBulletFlock team = Ctx.mInstance.mFlyBulletFlockMgr.getBulletFlockByThisId(cmd.bulletID);
            if (null != team)
            {
                team.dispose();
            }
        }

        public void RemoveTrangle(Player player, List<uint> idList)
        {
            PlayerChild trangle = null;

            for (int i = idList.Count - 1; i >= 0; --i)
            {
                trangle = player.mPlayerSplitMerge.mPlayerChildMgr.getEntityByThisId(idList[i]) as PlayerChild;
                if (null != trangle)
                {
                    trangle.dispose();
                }
            }
        }

        protected virtual void RemoveTrangle(Player player, uint trangleid)
        {
            PlayerChild trangle = player.mPlayerSplitMerge.mPlayerChildMgr.getEntityByThisId(trangleid) as PlayerChild;

            if (null != trangle)
            {
                trangle.dispose();
            }
        }
    }
}
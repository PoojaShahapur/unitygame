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
            //this._teamInfo = obj as JoinTeam;
            //this.txtName = this.transform.GetComponentInChildren<Text>(true);
            //this.txtName.text = this._teamInfo.name;
            //this.angle = this._teamInfo.angle;
            //this.pos = this._teamInfo.pos;
            //foreach (var pair in _teamInfo.trangles)
            //{
            //    AddTrangle(pair.Key).pos = pair.Value;
            //}

            this._teamInfo = obj as Giant.JoinTeam;
            player.setName(this._teamInfo.name);
            player.setMoveSpeed(this._teamInfo.moveSpeed);
            player.setPos(this._teamInfo.pos);
            player.setRotateEulerAngle(new UnityEngine.Vector3(0, 0, this._teamInfo.angle));

            foreach (var pair in _teamInfo.trangles)
            {
                AddTrangle(player, pair.Key).setPos(pair.Value);
            }
        }

        public PlayerChild AddTrangle(Player player, uint id)
        {
            //int layer = LayerManager.OtherTrangle;
            //if (IsMainTeam)
            //    layer = LayerManager.MainTrangle;
            //var trangle = SceneObject.Create<Trangle>(transform, "Prefabs/trangle", null, layer);
            //trangle.objctid = id;
            //trangle.team = this;
            //trangle.angle = this.angle;
            //trangle.UpdateTrigger();
            //trangles.Add(trangle.objctid, trangle);

            //trangle.GetComponent<Joint2D>().connectedBody = this.entityRigitBody;
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

            trangle.setThisId(id);
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

        //protected float targetAngle;
        protected UnityEngine.Vector2 targetPos;
        private UnityEngine.Vector2 curMoveDir;
        private float curSpeed = 0;

        //移动同步消息
        private Queue<Giant.MoveTeam> moves = new Queue<Giant.MoveTeam>();
        //操作,需要在同步快照点处理
        private Dictionary<uint, List<Giant.Command>> commands = new Dictionary<uint, List<Giant.Command>>();
        protected void OnMoveTeam(Giant.MoveTeam move)
        {
            //if (move.teamPos.y < this.pos.y)
            //{
            //    Debug.LogError("move.teamPos.y:" + move.teamPos.y + "this.pos.y:" + this.pos.y);
            //}
            //进入同步队列
            moves.Enqueue(move);
        }

        protected void OnNewTrangle(IDispatchObject dispObj)
        {
            Giant.NewTrangle move = dispObj as Giant.NewTrangle;
            this.OnNewTrangle(move);
        }

        protected void OnNewTrangle(Giant.NewTrangle trangle)
        {
            Player player = Ctx.mInstance.mPlayerMgr.getEntityByThisId(trangle.teamID) as Player;

            AddTrangle(player, trangle.trangleid);
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

        protected void ProcessTeamShoot(Giant.TeamShoot shoot)
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

            while (index < len)
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

        protected void OnTeamShoot(IDispatchObject dispObj)
        {
            Giant.TeamShoot move = dispObj as Giant.TeamShoot;
            this.OnTeamShoot(move);
        }

        protected void OnTeamShoot(Giant.TeamShoot shoot)
        {
            //网络延迟,已经移动到服务器的同步点,才收到射击消息
            if (moves.Count == 0)
            {
                ProcessTeamShoot(shoot);
            }
            else
            {
                List<Giant.Command> cmds;
                if (!commands.TryGetValue(shoot.sframeid, out cmds))
                {
                    cmds = new List<Giant.Command>();
                    commands.Add(shoot.sframeid, cmds);
                }
                cmds.Add(shoot);
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
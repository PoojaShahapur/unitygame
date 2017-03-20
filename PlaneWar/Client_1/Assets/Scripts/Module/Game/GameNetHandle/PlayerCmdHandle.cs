using SDK.Lib;
using System.Collections.Generic;

namespace Game.Game
{
    public class PlayerCmdHandle
    {
        protected Giant.JoinTeam _teamInfo;

        public PlayerCmdHandle()
        {

        }

        public void init()
        {

        }

        public void dispose()
        {

        }

        public void OnLoadEntity(Player player, object obj)
        {
            this._teamInfo = obj as Giant.JoinTeam;
            player.setName(this._teamInfo.name);

            //if (this._teamInfo.move.moves.Count == 0)
            //{
            //    this._teamInfo.move.angle = 0;
            //    this._teamInfo.move.teamPos = UnityEngine.Vector2.zero;
            //    for (int i = 0; i < 3; ++i)
            //    {
            //        this._teamInfo.move.moves.Add(player.mUniqueNumIdGen.genNewId(), UnityEngine.Vector2.zero);
            //    }
            //}
            this.initTrangles(player);
        }

        public void initTrangles(Player player)
        {
            if (_teamInfo != null)
            {
                //player.setPos(_teamInfo.move.teamPos);
                //player.setInvincibleTime(_teamInfo.invincibleTime);
                //foreach (var pair in _teamInfo.move.moves)
                //{
                //    PlayerChild trangle = AddTrangle(player, (int)pair.Key);
                //    trangle.setPos(pair.Value);
                //}
                //player.setRotateEulerAngle(new UnityEngine.Vector3(0, 0, _teamInfo.move.angle));
            }
        }

        public PlayerChild AddTrangle(Player player, int id = -1)
        {
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
            trangle.init();

            return trangle;
        }

        protected UnityEngine.Vector2 targetPos;
        protected float targetAngle;
        private uint updatePosFrame = 0;
        private float curSpeed = 0;
        private UnityEngine.Vector2 curMoveDir;
        protected void OnMoveTeam(Player player, Giant.MoveTeam move)
        {
            if (player.mPlayerSplitMerge.mPlayerChildMgr.getEntityCount() == 0)
                player.setPos(move.teamPos);
            //this.targetPos = move.teamPos;
            //this.targetAngle = move.angle;
            //this.updatePosFrame = 0;
            //this.curMoveDir = (this.targetPos - this.pos).normalized;
            //this.curSpeed = _teamInfo.moveSpeed;
            if (EntityType.ePlayerMain != player.getEntityType())
            {
                //this.curSpeed = (this.targetPos - this.pos).magnitude / MainTrangleTeam.SynDeltaTime;
                //_teamInfo.turnSpeed = Mathf.Abs(this.targetAngle - this.angle) / MainTrangleTeam.SynDeltaTime;
                //foreach (var pair in move.moves)
                //{
                //    var trangle = GetTrangle(pair.Key);
                //    if (trangle != null)
                //        trangle.pos = pair.Value;
                //}
            }
        }

        protected void OnNewTrangle(Player player, Giant.NewTrangle trangle)
        {
            AddTrangle(player, (int)trangle.trangleid);
        }

        protected void OnRemoveTrangle(Player player, Giant.RemoveTrangle trangle)
        {
            RemoveTrangle(player, trangle.trangleid);
        }

        protected void OnTeamShoot(Player player, Giant.TeamShoot shoot)
        {
            //var bulletTeam = SceneObject.Create<BulletTeam>(transform.parent, "");
            //bulletTeam.objctid = shoot.bulletTeamID;
            //bulletTeam.team = scene.GetTrangleTeam(shoot.teamID);
            //bulletTeam.timeOut = shoot.timeOut;
            //bulletTeam.pos = shoot.pos;
            //bulletTeam.speed = shoot.speed;

            FlyBulletFlock team = new FlyBulletFlock();
            team.setThisId(shoot.bulletTeamID);
            team.setOwnerPlayerThisId(shoot.teamID);

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
                bullet.init();
            }
            //bulletTeam.angle = shoot.angle;
            //bulletTeams.Add(shoot.bulletTeamID, bulletTeam);
        }

        protected void OnMoveTeamBullet(Player player, Giant.MoveTeamBullet cmd)
        {
            FlyBulletFlock team = Ctx.mInstance.mFlyBulletFlockMgr.getBulletFlockByThisId(cmd.bulletID);
            if (null != team)
            {
                //team.pos = cmd.pos;
            }
        }

        protected void OnBulletTimeOut(Player player, Giant.BulletTimeOut cmd)
        {
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
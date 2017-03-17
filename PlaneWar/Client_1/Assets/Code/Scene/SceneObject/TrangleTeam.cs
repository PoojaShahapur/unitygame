using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Giant
{
    public class TrangleTeam : SceneTeam
    {
        private uint _trangleIDIndex = 0;
        protected JoinTeam _teamInfo;
        protected uint bulletID = 0;
        protected Text txtName;
        protected Dictionary<uint, BulletTeam> bulletTeams = new Dictionary<uint, BulletTeam>();
        protected Dictionary<uint, Trangle> trangles = new Dictionary<uint, Trangle>();

        public uint AllocTrangleID()
        {
            return _trangleIDIndex++;
        }

        public JoinTeam TeamInfo
        {
            get { return _teamInfo; }
        }


        public int TrangleCount
        {
            get
            {
                var count = 0;
                foreach (var pair in trangles)
                {
                    if (!pair.Value.IsDie)
                        ++count;
                }
                return count;
            }
        }

        public bool IsTrangleEmpty
        {
            get { return trangles.Count == 0; }
        }
        public bool IsMainTeam
        {
            get { return _teamInfo.isself; }
        }

        public Bullet GetBullet(uint teamid,uint bulletid)
        {
            BulletTeam team;
            if (bulletTeams.TryGetValue(teamid,out team))
            {
                return team.GetBullet(bulletid);
            }
            return null;
        }

        public Trangle GetTrangle(uint trangleid)
        {
            Trangle trangle;
            trangles.TryGetValue(trangleid, out trangle);
            return trangle;
        }


        public override float angle
        {
            set
            {
                this._updateAngle(trangles, value);
            }
        }

        public override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);
            this.BulletUpdate(dt);

            var myPos = this.pos;
            this.angle = (int)Mathf.MoveTowardsAngle(this.angle,targetAngle, _teamInfo.turnSpeed * dt) % 360;
            this.pos = myPos + curSpeed * dt * this.curMoveDir ;

            //处理延迟,自动往前跑
            if (!this.IsMainTeam)
            {
                if (++this.updatePosFrame == MainTrangleTeam.synFrame)
                {
                    this.targetPos = this.pos + MainTrangleTeam.SynDeltaTime * _teamInfo.moveSpeed * this.curMoveDir;
                    this.curSpeed = _teamInfo.moveSpeed;
                }
                //else if (this.updatePosFrame > MainTrangleTeam.synFrame)
                //{
                //    this.curSpeed = 0;
                //}
            }

            SceneObject.UpdateObject(this.trangles, dt);
            SceneObject.UpdateObject(this.bulletTeams, dt);
            SceneObject.RemoveDeathObject(bulletTeams);
            SceneObject.RemoveDeathObject(trangles);
        }

        public Trangle AddTrangle(int id = -1)
        {
            int layer = LayerManager.OtherTrangle;
            if (IsMainTeam)
                layer = LayerManager.MainTrangle;
            var trangle = SceneObject.Create<Trangle>(transform,"Prefabs/trangle",null,layer);
            trangle.objctid = (uint)(id < 0 ? (int)AllocTrangleID() : id);
            trangle.team = this;
            trangle.angle = this.angle;
            trangle.UpdateTrigger();
            trangles.Add(trangle.objctid, trangle);

            trangle.GetComponent<Joint2D>().connectedBody = this.entityRigitBody;
            //if (this.IsInvincible)
            //   trangle.Blink(this.invincibleTime);
            return trangle;
        }

        public void RemoveTrangle(List<uint> idList)
        {
            for (int i = idList.Count - 1; i >= 0; --i)
            {
                Trangle trangle = null;
                if (trangles.TryGetValue(idList[i],out trangle))
                {
                    trangle.DestoryThis();
                    trangles.Remove(idList[i]);
                }
            }
        }

        protected virtual void RemoveTrangle(uint trangleid)
        {
            Trangle trangle;
            if (trangles.TryGetValue(trangleid,out trangle))
            {
                trangle.DestoryThis();
                trangles.Remove(trangleid);
            }
        }

        public override void DestoryThis()
        {
            SceneObject.DestoryObject(trangles);
            SceneObject.DestoryObject(bulletTeams);
            base.DestoryThis();
        }

        protected override void OnLoadEntity(object obj)
        {
            this._teamInfo = obj as JoinTeam;
            this.txtName = this.transform.GetComponentInChildren<Text>(true);
            this.txtName.text = this._teamInfo.name;
            if (this._teamInfo.move.moves.Count == 0)
            {
                this._teamInfo.move.angle = 0;
                this._teamInfo.move.teamPos = Vector2.zero;
                for (int i = 0; i < 3; ++i)
                {
                    this._teamInfo.move.moves.Add(AllocTrangleID(), Vector2.zero);
                }
            }
            this.initTrangles();
        }

        protected void initTrangles()
        {
            if (_teamInfo != null)
            {
                this.pos = _teamInfo.move.teamPos;
                this.invincibleTime = _teamInfo.invincibleTime;
                foreach (var pair in _teamInfo.move.moves)
                {
                    var trangle = AddTrangle((int)pair.Key);
                    trangle.pos = pair.Value;
                }
                this.angle = _teamInfo.move.angle;
            }
        }
            

        protected Vector2 targetPos;
        protected float targetAngle;
        private uint updatePosFrame = 0;
        private float curSpeed = 0;
        private Vector2 curMoveDir;
        protected void OnMoveTeam(MoveTeam move)
        {
            if (trangles.Count == 0)
                this.pos = move.teamPos;
            this.targetPos = move.teamPos;
            this.targetAngle = move.angle;
            this.updatePosFrame = 0;
            this.curMoveDir = (this.targetPos - this.pos).normalized;
            this.curSpeed = _teamInfo.moveSpeed;
            if (!IsMainTeam)
            {
                this.curSpeed = (this.targetPos - this.pos).magnitude / MainTrangleTeam.SynDeltaTime;
                _teamInfo.turnSpeed = Mathf.Abs(this.targetAngle - this.angle) / MainTrangleTeam.SynDeltaTime;
                foreach (var pair in move.moves)
                {
                    var trangle = GetTrangle(pair.Key);
                    if (trangle != null)
                        trangle.pos = pair.Value;
                }
            }
        }

        protected void OnNewTrangle(NewTrangle trangle)
        {
            AddTrangle((int)trangle.trangleid);
        }

        protected void OnRemoveTrangle(RemoveTrangle trangle)
        {
            RemoveTrangle(trangle.trangleid);
        }

        protected void OnTeamShoot(TeamShoot shoot)
        {
            var bulletTeam = SceneObject.Create<BulletTeam>(transform.parent, "");
            bulletTeam.objctid = shoot.bulletTeamID;
            bulletTeam.team =  scene.GetTrangleTeam(shoot.teamID);
            bulletTeam.timeOut = shoot.timeOut;
            bulletTeam.pos = shoot.pos;
            bulletTeam.speed = shoot.speed;
            foreach (var pair in trangles)
            {
                var trangle = pair.Value;
                bulletTeam.AddBullet(trangle.objctid, trangle.pos);
            }
            bulletTeam.angle = shoot.angle;
            bulletTeams.Add(shoot.bulletTeamID, bulletTeam);
        }



        protected void OnMoveTeamBullet(MoveTeamBullet cmd)
        {
            BulletTeam team;
            if (bulletTeams.TryGetValue(cmd.bulletID,out team))
            {
                team.pos = cmd.pos;
            }
        }

        protected void OnBulletTimeOut(BulletTimeOut cmd)
        {
            BulletTeam team;
            if (bulletTeams.TryGetValue(cmd.bulletID, out team))
            {
                team.KillBy(DeathType.eKillByTimeOut);
            }
        } 

        //protected void OnDangerZoneKillTrangle(DangerZoneKillTrangle cmd)
        //{
        //    Trangle trangle;
        //    if (trangles.TryGetValue(cmd.trangleid,out trangle))
        //    {
        //        trangle.KillBy(DeathType.eKillByDangerZone);
        //    }
        //}
        private MoveTeamBullet bulletMove = new MoveTeamBullet();
        private BulletTimeOut bulletTimeOut = new BulletTimeOut();
        private void BulletUpdate(float dt)
        {
            if (bulletTeams.Count > 0)
            {
                bulletMove.frameid = scene.frame;
                bulletMove.teamID = this._teamInfo.teamID;

                bulletTimeOut.teamID = this._teamInfo.teamID;
                bulletTimeOut.frameid = scene.frame;

                foreach (var pair in bulletTeams)
                {
                    var bullet = pair.Value;
                    bullet.timeOut -= dt;
                    if (bullet.timeOut <= 0)
                    {
                        bulletTimeOut.bulletID = pair.Key;
                        scene.HandleSceneCommand(bulletTimeOut);
                    }
                    else
                    {
                        bulletMove.bulletID = pair.Key;
                        //匀速运动
                        bulletMove.pos = bullet.pos + bullet.speed * dt * bullet.dir;
                        scene.HandleSceneCommand(bulletMove);
                    }
                }
            }
        }

    }
}

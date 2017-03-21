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
            this.MoveUpdate(dt);
            SceneObject.UpdateObject(this.trangles, dt);
            SceneObject.UpdateObject(this.bulletTeams, dt);
            SceneObject.RemoveDeathObject(bulletTeams);
            SceneObject.RemoveDeathObject(trangles);
        }

        //同步移动
        private void MoveUpdate(float dt)
        {
            if (moves.Count > 0)
            {
                //if (moves.Count > 1)
                //{
                //    Debug.Log("moves.Count:" + moves.Count);
                //}
                var target = moves.Peek();
                var myPos = this.pos;

                //速度缩放
                float teamSpeed = 0;
                if (moves.Count > 1)
                    teamSpeed = this.moves.Count * _teamInfo.moveSpeed;
                else
                    teamSpeed = _teamInfo.moveSpeed;
                var trangleSpeed = teamSpeed * 2;
                var movPos = Vector2.MoveTowards(myPos, target.teamPos , teamSpeed * dt);
                this.pos = movPos;
                //var speed = Mathf.Max(360,Mathf.Abs(target.angle - this.angle) / 0.05f);
                this.angle = Mathf.MoveTowardsAngle(this.angle, target.angle, _teamInfo.turnSpeed * dt);

                bool trangleMoveOK = true;
                if (!this.IsMainTeam)
                {
                    foreach (var pair in target.moves)
                    {
                        var trangle = GetTrangle(pair.Key);
                        if (trangle != null)
                        {
                            var pos = trangle.pos;
                            if (pos != pair.Value)
                            {
                                pos = Vector2.MoveTowards(pos, pair.Value, trangleSpeed * dt);
                                trangle.pos = pos;
                                if (pos != pair.Value) trangleMoveOK = false;
                            }
                        }
                    }
                }

                //已经移动到同步点
                if (trangleMoveOK && Mathf.Approximately(this.angle,target.angle) && movPos == target.teamPos)
                {
                    moves.Dequeue();

                    //处理本帧事件
                    List<Command> cmds = null;
                    if (commands.TryGetValue(target.sframeid,out cmds))
                    {
                        foreach (var cmd in cmds)
                        {
                            if (cmd is TeamShoot)
                                ProcessTeamShoot(cmd as TeamShoot);
                        }
                    }
                    

                    //解决卡顿问题,离目标点最后一帧的移动距离不够,进行补齐
                    dt = dt *(1 - (Vector2.Distance(movPos, myPos) / (teamSpeed * dt)));
                    this.MoveUpdate(dt);
                }
            }
            else
            {
                //网络延迟,继续往前移动
                this.pos = this.pos + _teamInfo.moveSpeed * dt * this.dir.normalized;
            }
        }

        public Trangle AddTrangle(uint id)
        {
            int layer = LayerManager.OtherTrangle;
            if (IsMainTeam)
                layer = LayerManager.MainTrangle;
            var trangle = SceneObject.Create<Trangle>(transform,"Prefabs/trangle",null,layer);
            trangle.objctid = id;
            trangle.team = this;
            trangle.angle = this.angle;
            trangle.UpdateTrigger();
            trangles.Add(trangle.objctid, trangle);

            trangle.GetComponent<Joint2D>().connectedBody = this.entityRigitBody;
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
            this.angle = this._teamInfo.angle;
            this.pos = this._teamInfo.pos;
            foreach (var pair in _teamInfo.trangles)
            {
                AddTrangle(pair.Key).pos = pair.Value;
            }
        }
            
        //protected float targetAngle;
        protected Vector2 targetPos;
        private Vector2 curMoveDir;
        private float curSpeed = 0;

        //移动同步消息
        private Queue<MoveTeam> moves = new Queue<MoveTeam>();
        //操作,需要在同步快照点处理
        private Dictionary<uint, List<Command>> commands = new Dictionary<uint, List<Command>>();
        protected void OnMoveTeam(MoveTeam move)
        {
            //if (move.teamPos.y < this.pos.y)
            //{
            //    Debug.LogError("move.teamPos.y:" + move.teamPos.y + "this.pos.y:" + this.pos.y);
            //}
            //进入同步队列
            moves.Enqueue(move);
        }

        protected void OnNewTrangle(NewTrangle trangle)
        {
            AddTrangle(trangle.trangleid);
        }

        protected void OnRemoveTrangle(RemoveTrangle trangle)
        {
            RemoveTrangle(trangle.trangleid);
        }

        private void ProcessTeamShoot(TeamShoot shoot)
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

        protected void OnTeamShoot(TeamShoot shoot)
        {
            //网络延迟,已经移动到服务器的同步点,才收到射击消息
            if (moves.Count == 0)
            {
                ProcessTeamShoot(shoot);
            }
            else
            {
                List<Command> cmds;
                if (!commands.TryGetValue(shoot.sframeid,out cmds))
                {
                    cmds = new List<Command>();
                    commands.Add(shoot.sframeid, cmds);
                }
                cmds.Add(shoot);
            }
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

        private void BulletUpdate(float dt)
        {
            if (bulletTeams.Count > 0)
            {
                MoveTeamBullet bulletMove = new MoveTeamBullet();
                BulletTimeOut bulletTimeOut = new BulletTimeOut();
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

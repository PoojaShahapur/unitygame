using UnityEngine;
using System.Collections.Generic;

namespace Giant
{
    public interface TeamController
    {
        Vector2 GetDir();
        bool GetShoot();
        void SetDanger(bool danger);
        void SetShootCD(float cd);
    }

    //FightScene 才有MainTrangleTeam

    public class MainTrangleTeam : TrangleTeam
    {
        private FightScene scene;
        protected class Pair<T,V>
        {
            public T Key;
            public V Value;
        }
        protected List<Pair<uint, float>> dangerTrangles = new List<Pair<uint, float>>();
        public TeamController controller { set; get; }
        private float shootCD = 0;
        static public readonly int  synFrame = 3;
        static public float SynDeltaTime
        {
            get
            {
                return Time.fixedDeltaTime * synFrame;
            }
        }

        protected override void OnLoadEntity(object obj)
        {
            //var info = obj as JoinTeam;
            this.scene = SceneObject.scene as FightScene;
            base.OnLoadEntity(obj);
            this.txtName.gameObject.SetActive(false);
        }

        public override void OnUpdate(float dt)
        {
            if (scene != null && controller != null)
            {
                this.DangerZoneTrangleUpdate(dt);
                if (scene.frame % synFrame == 0)
                {
                    this.MoveUpdate();
                }
                this.ShootUpdate(dt);
            }
            base.OnUpdate(dt);
        }

        private plane.PlaneMsg planeMsg = new plane.PlaneMsg();
        private void DangerZoneTrangleUpdate(float dt)
        {

            if (dangerTrangles.Count > 0)
            {
                for (int i = dangerTrangles.Count - 1; i >= 0; --i)
                {
                    var pair = dangerTrangles[i];
                    if (!trangles.ContainsKey(pair.Key) || trangles[pair.Key].IsDie || pair.Value <= 0)
                    {
                        dangerTrangles.RemoveAt(i);
                    }
                    else
                    {
                        var value = Mathf.MoveTowards(pair.Value, 0, dt);
                        dangerTrangles[i].Value = value;
                        if (value == 0)
                        {
                            planeMsg.plane_id = pair.Key;
                            Game.instance.handler.RequestSend("plane.Plane", "Remove", planeMsg);
                            scene.HandleSceneCommand(objctid, planeMsg, false);
                        }
                    }
                }
            }

            controller.SetDanger(dangerTrangles.Count > 0);
        }

        protected void OnTrangleInDangerZone(TrangleInDangerZone cmd)
        {
            Trangle trangle;
            if (trangles.TryGetValue(cmd.trangleid, out trangle))
            {
                var dangerItem = dangerTrangles.Find((pair) =>
                {
                    return pair.Key == cmd.trangleid;
                });
                if (dangerItem == null)
                {
                    dangerItem = new Pair<uint, float>();
                    dangerItem.Key = cmd.trangleid;
                    dangerItem.Value = cmd.liveTime;
                    dangerTrangles.Add(dangerItem);
                }
            }
        }

        protected void OnTrangleOutDangerZone(TrangleOutDangerZone cmd)
        {
            var dangerItem = dangerTrangles.Find((pair) =>
            {
                return pair.Key == cmd.trangleid;
            });
            if (dangerItem != null)
                dangerTrangles.Remove(dangerItem);
        }
 
        private plane.FireMsg fireMsg = new plane.FireMsg();
        private void ShootUpdate(float dt)
        {
            if (shootCD > 0)
            {
                shootCD -= dt;
                controller.SetShootCD(shootCD / _teamInfo.shootCD);
            }
            if (controller.GetShoot())
            {
                if (shootCD <= 0)
                {
                    var pos = this.pos;
                    fireMsg.bullet_team_id = bulletID++;
                    fireMsg.angle = this.angle;
                    fireMsg.x = pos.x;
                    fireMsg.y = pos.y;
                    Game.instance.handler.RequestSend("plane.Plane", "Fire", fireMsg);
                    //scene.HandleSceneCommand(this.objctid, fireMsg);

                    shootCD = _teamInfo.shootCD;
                }
            }
        }

        private plane.MoveToSmallPlaneMsg moveMsg = new plane.MoveToSmallPlaneMsg();
        private plane.TurnToMsg turnMsg = new plane.TurnToMsg();
        private void MoveUpdate()
        {
            var handler = Game.instance.handler;

            //小飞机请求移动
            moveMsg.movings.Clear();
            foreach (var pair in trangles)
            {
                var move = new plane.OneMove();
                var pos = pair.Value.pos;
                move.plane_id = pair.Key;
                move.x = pos.x;
                move.y = pos.y;
                moveMsg.movings.Add(move);
                handler.RequestSend("plane.Plane", "MoveTo", moveMsg);
            }

            //请求转向
            var curDir = controller.GetDir();
            if (curDir != Vector2.zero)
            {
                turnMsg.angle = Quaternion.FromToRotation(Vector2.up, curDir).eulerAngles.z;
                handler.RequestSend("plane.Plane", "TurnTo", turnMsg);
            }
        }

        //protected override void RemoveTrangle(uint trangleid)
        //{
        //    dangerTrangles.Remove
        //    base.RemoveTrangle(trangleid);
        //}

    }
}

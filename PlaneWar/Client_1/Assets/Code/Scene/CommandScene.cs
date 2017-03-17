using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using GameBox.Framework;
using GameBox.Service.AssetManager;

namespace Giant
{
    public class CommandScene : Scene
    {
        //private Queue<Command> commandQueue;
        protected Rect rect = new Rect(-50,-25,100,50);
        protected Transform cameraT;
        protected Transform mapT;
        protected Dictionary<uint,TrangleTeam> teams = new Dictionary<uint, TrangleTeam>();
        protected Dictionary<uint, SceneFood> items = new Dictionary<uint, SceneFood>();
        public MainTrangleTeam myTeam { set; get; }
        public uint frame { private set; get; }

        protected bool _IsMainTeam(uint teamid)
        {
            return teamid == myTeam.objctid;
        }

        private static object[] cmdParam = new object[1];
        public void HandleSceneCommand(Command cmd)
        {
            if (!TryHandleSceneCommand(this,cmd))
            {
                var tCmd = cmd as TeamCommand;
                if (tCmd != null)
                {
                    TrangleTeam team;
                    if (teams.TryGetValue(tCmd.teamID, out team))
                    {
                        TryHandleSceneCommand(team, cmd);
                    }
                }
            }
        }

        protected virtual void OnTrangleTeamChange(uint teamid) { }
        private bool TryHandleSceneCommand(object obj, Command cmd)
        {
            var method = obj.GetType().GetMethod("On" + cmd.GetType().Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (method != null)
            {
                cmdParam[0] = cmd;
                method.Invoke(obj, cmdParam);
                return true;
            }
            return false;
        }


        virtual protected void OnJoinTeam(JoinTeam cmd)
        {
            var team = SceneObject.Create<TrangleTeam>(mapT, "Prefabs/TrangleTeam", cmd);
            team.objctid = cmd.teamID;
            teams.Add(cmd.teamID, team);
        }

        protected void OnNewItem(NewItem cmd)
        {
            var item = SceneObject.Create<SceneFood>(mapT, "Prefabs/star");
            item.objctid = cmd.objid;
            cmd.pos.x = rect.xMin + rect.width * cmd.pos.x;
            cmd.pos.y = rect.yMin + rect.height * cmd.pos.y;
            item.pos = cmd.pos;
            items.Add(cmd.objid, item);
        }

        protected void OnHitTrangleCommand(HitTrangleCommand cmd)
        {
            TrangleTeam team;
            TrangleTeam targetTeam;
            if (teams.TryGetValue(cmd.teamID, out team) && teams.TryGetValue(cmd.targetTeamID, out targetTeam))
            {
                var bullet = team.GetBullet(cmd.bulletTeamid, cmd.bulletid);
                if (bullet != null)
                {
                    bullet.KillBy(DeathType.eKillByOther);

                    var trangle = targetTeam.GetTrangle(cmd.targetTrangleID);
                    if (trangle != null)
                    {
                        trangle.KillBy(DeathType.eKillByOther);
                        this.OnTrangleTeamChange(cmd.targetTeamID);
                    }
                }
            }
        }

        protected void OnHitItemCommand(HitItemCommand cmd)
        {
            TrangleTeam team;
            if (teams.TryGetValue(cmd.teamID, out team))
            {
                var bullet = team.GetBullet(cmd.bulletTeamid, cmd.bulletid);
                if (bullet != null)
                {
                    bullet.KillBy(DeathType.eKillByOther);
                    SceneFood item;
                    if (items.TryGetValue(cmd.itemID, out item))
                    {
                        item.KillBy(DeathType.eKillByOther);
                    }
                }
            }
        }

        protected void OnTrangleTeamLeave(TrangleTeamLeave cmd)
        {
            TrangleTeam team;
            if (teams.TryGetValue(cmd.teamID,out team))
            {
                team.DestoryThis();
                teams.Remove(cmd.teamID);
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            cameraT = Camera.main.transform;

            var asset = Game.instance.assetManager.Load("Prefabs/FightScene.prefab", AssetType.PREFAB);
            var scene = GameObject.Instantiate(asset.Cast<GameObject>());
            asset.Dispose();
            mapT = scene.transform;
            this.frame = 0;
        }

        public override void OnLeave()
        {
            base.OnLeave();
            SceneObject.DestoryObject(items);
            SceneObject.DestoryObject(teams);
            if (mapT)
                GameObject.Destroy(mapT.gameObject);
        }

        public TrangleTeam GetTrangleTeam(uint teamid)
        {
            TrangleTeam team;
            teams.TryGetValue(teamid, out team);
            return team;
        }

        public override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);
            foreach (var pair in teams)
                pair.Value.OnUpdate(dt);
            SceneObject.RemoveDeathObject(teams);
            SceneObject.RemoveDeathObject(items);
            ++this.frame;
        }

        public virtual void OnTriggerEnterDangerZone(Trangle trangle)
        {

        }
        public virtual void OnTriggerExitDangerZone(Trangle trangle)
        {
        }
    }
}

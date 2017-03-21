using UnityEngine;
using GameBox.Service.ObjectPool;

namespace Giant
{
    public class Bullet : SceneMoveObject
    {
        public BulletTeam team { set; get; }
        protected override void OnLoadEntity(object param)
        {

        }

        //只处理主角的碰撞
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (SceneObject.scene is FightScene && team.team.IsMainTeam)
            {
                var scene = SceneObject.scene as FightScene;
                var item = collision.transform.GetComponent<SceneFood>();
                if (item != null && !item.IsInvincible && !item.IsDie)
                {
                    //通知其他玩家
                    var cmd = new plane.EatMsg();
                    cmd.bullet_id = this.objctid;
                    cmd.bullet_team_id = team.objctid;
                    cmd.food_id = item.objctid;
                    Game.instance.handler.RequestSend("plane.Plane", "Eat", cmd);
                    //自己处理
                    //scene.HandleSceneCommand(team.team.objctid, cmd);
                }

                var tranlge = collision.transform.GetComponent<Trangle>();
                if (tranlge != null && !tranlge.team.IsMainTeam && !tranlge.IsInvincible && !tranlge.IsDie)
                {
                    var cmd = new plane.HitMsg();
                    cmd.bullet_id = this.objctid;
                    cmd.bullet_team_id = team.objctid;
                    cmd.plane_id = tranlge.objctid;
                    cmd.target_user_id = tranlge.team.objctid;
                    Game.instance.handler.RequestSend("plane.Plane", "Hit", cmd);
                    //自己处理
                    //scene.HandleSceneCommand(team.team.objctid, cmd);
                }
            }
        }
    }

}
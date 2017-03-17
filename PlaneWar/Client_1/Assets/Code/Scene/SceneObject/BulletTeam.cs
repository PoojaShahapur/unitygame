using UnityEngine;
using System.Collections.Generic;

namespace Giant
{
    public class BulletTeam : SceneTeam
    {
        public TrangleTeam team { set; get;}
        //移动速度
        public float speed { set; get; }
        public float timeOut { set; get;}
        protected Dictionary<uint, Bullet> bullets = new Dictionary<uint, Bullet>();

        public override float angle
        {
            set
            {
                this._updateAngle<Bullet>(bullets, value);
            }
        }

        public override void DestoryThis()
        {
            SceneObject.DestoryObject(bullets);
            base.DestoryThis();
        }

        public Bullet GetBullet(uint bulletid)
        {
            Bullet bullet;
            if (bullets.TryGetValue(bulletid, out bullet))
            {
                return bullet;
            }
            return null;
        }

        public void AddBullet(uint id,Vector2 pos)
        {
            var layer = LayerManager.OtherBullet;
            if (team.IsMainTeam)
                layer = LayerManager.MainBullet;
            var bullet = SceneObject.Create<Bullet>(transform, "Prefabs/bullet",null ,layer);
            bullet.team = this;
            bullet.objctid = id;
            bullet.pos = pos;
            bullets.Add(id, bullet);
        }

        public override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);
            SceneObject.RemoveDeathObject(bullets);
        }
    }
}
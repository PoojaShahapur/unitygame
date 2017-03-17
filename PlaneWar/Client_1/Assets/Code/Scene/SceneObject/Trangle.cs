using System;
using UnityEngine;
using GameBox.Framework;
using GameBox.Service.AssetManager;

namespace Giant
{
    public class Trangle : SceneMoveObject
    {
        public TrangleTeam team { set; get; }

        public Vector2 targetPos;

        protected override void OnLoadEntity(object param)
        {
            this.entity.sortingOrder = 2;
            base.OnLoadEntity(param);
        }

        public override void DestoryThis()
        {
            _setEntityAlpha(1);
            this.GetComponent<Joint2D>().connectedBody = null;
            base.DestoryThis();
        }

        public void UpdateTrigger()
        {
            var collider = this.entity.GetComponent<PolygonCollider2D>();
            if (collider != null)
                collider.isTrigger = !team.IsMainTeam;
        }

        public override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);
            //var myPos = this.pos;
            //if (targetPos != null && targetPos != myPos)
            //{
            //    this.pos = myPos + (dt * team.TeamInfo.moveSpeed * (targetPos - myPos).normalized);
            //}
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var obj = collision.gameObject;
            if (team.IsMainTeam && obj.layer == LayerManager.DangerZoneLayer)
            {
                SceneObject.scene.OnTriggerEnterDangerZone(this);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var obj = collision.gameObject;
            if (team.IsMainTeam && obj.layer == LayerManager.DangerZoneLayer)
            {
                SceneObject.scene.OnTriggerExitDangerZone(this);
            }
        }
    }
}

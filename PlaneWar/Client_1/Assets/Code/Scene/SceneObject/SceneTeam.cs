using UnityEngine;
using System.Collections.Generic;
namespace Giant
{
    public class SceneTeam : SceneMoveObject
    {
        private Vector2 _dir;

        public override Vector2 dir
        {
            get
            {
                return this._dir;
            }
        }

        protected void _updateAngle<T>(Dictionary<uint,T> team,float angle)
            where T : SceneMoveObject
        {
            this._angle = angle;
            bool updateDir = false;
            foreach (var pair in team)
            {
                pair.Value.angle = this._angle;
                if (!updateDir)
                {
                    _dir = pair.Value.dir;
                    updateDir = true;
                }
            }
        }
    }
}

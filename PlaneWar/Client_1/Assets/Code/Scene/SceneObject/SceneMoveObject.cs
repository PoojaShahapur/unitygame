using UnityEngine;


namespace Giant
{
    abstract public class SceneMoveObject : SceneObject
    {
        //当前方向
        protected float _angle = 0;
        public virtual float angle
        {
            get
            {
                return _angle;
            }

            set
            {
                transform.eulerAngles = new Vector3(0, 0, value);
                _angle = value;
            }
        }

        virtual public Vector2 dir
        {
            get
            {
                return transform.up;
            }
        }
    }
}
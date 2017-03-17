using UnityEngine;
using System.Collections.Generic;
using GameBox.Framework;
using GameBox.Service.AssetManager;

namespace Giant
{
    public enum DeathType
    {
        eNone,
        eKillByOther,
        eKillByDangerZone,
        eKillByTimeOut,
    }

    abstract public class SceneObject : MonoBehaviour
    {
        public static CommandScene scene { set; get; }
        public DeathType deathType { get; private set; }
        public uint objctid { set; get; }
        //无敌时间
        protected float invincibleTime { set; get; }
        public bool IsInvincible
        {
            get
            {
                return this.invincibleTime > 0;
            }
        }
        public bool IsDie
        {
            get
            {
                return deathType != DeathType.eNone;
            }
        }

        public void KillBy(DeathType type, object killer = null)
        {
            if (IsInvincible) return;
            this.deathType = type;
        }

        virtual public Vector2 pos
        {
            set
            {
                transform.localPosition = value;
            }
            get
            {
                return transform.localPosition;
            }
        }
        public SpriteRenderer entity { protected set; get; }
        public Rigidbody2D entityRigitBody { protected set; get; }
        private string prefab = null;
        private float blinkTime = 0;
        private static readonly float blinkInterval = 0.3f;
        private static readonly float blinkMinAlpha = 0.3f;
        public static T Create<T>(Transform parent, string prefab, object param = null,int layer = -1)
            where T : SceneObject
        {
            GameObject obj = null;
            if (!string.IsNullOrEmpty(prefab))
            {
                //var asset = Game.instance.assetManager.Load(prefab + ".prefab", AssetType.PREFAB);
                //obj = GameObject.Instantiate(asset.Cast<GameObject>());
                //asset.Dispose();

                obj = Singleton.GetInstance<PoolManager>().Load(prefab);
                //obj = Singleton.GetInstance<PrefabFactory>().Pick(prefab);
            }
            else
            {
                obj = new GameObject();
            }
            if (layer >= 0)
                obj.layer = layer;
            var objT = obj.transform;
            objT.SetParent(parent);
            objT.localScale = Vector3.one;
            objT.localPosition = Vector3.zero;
            objT.localEulerAngles = Vector3.zero;

            var sceneObj = obj.AddComponent<T>();
            sceneObj.prefab = prefab;
            sceneObj.entity = obj.GetComponent<SpriteRenderer>();
            if (sceneObj.entity != null)
                sceneObj.entity.sortingOrder = 1;
            sceneObj.entityRigitBody = obj.GetComponent<Rigidbody2D>();

            sceneObj.OnLoadEntity(param);
            obj.name = sceneObj.GetType().Name;
            return sceneObj;
        }

        /// <summary>
        ///闪烁
        /// </summary>
        /// <param name="time">闪烁时间,单位:秒</param>
        public void Blink(float time)
        {
            this.blinkTime = Mathf.Round(time / blinkInterval / 2.0f) * blinkInterval * 2.0f;
            if (entity != null)
            {
                _setEntityAlpha(1.0f);
            }
        }

        protected void _setEntityAlpha(float a)
        {
            var c = entity.color;
            c.a = a;
            entity.color = c;
        }

        virtual protected void OnLoadEntity(object param) { }
        virtual public void DestoryThis()
        {
            GameObject.Destroy(this);
            //Singleton.GetInstance<PrefabFactory>().Drop(this.prefab, gameObject);
            Singleton.GetInstance<PoolManager>().Unload(gameObject);
        }
        virtual public void OnUpdate(float dt)
        {
            if (this.invincibleTime > 0)
                this.invincibleTime = Mathf.MoveTowards(this.invincibleTime, 0, dt);
            if (entity != null && this.blinkTime > 0)
            {
                _blinkUpdate(dt);
            }
        }

        static protected List<uint> _removeBuffer = new List<uint>();
        static public void RemoveDeathObject<T>(Dictionary<uint,T> objs)
            where T: SceneObject
        {
            _removeBuffer.Clear();
            foreach (var pair in objs)
            {
                if (pair.Value.IsDie)
                {
                    _removeBuffer.Add(pair.Key);
                    pair.Value.DestoryThis();
                }
            }

            for (int i = 0; i < _removeBuffer.Count; ++i)
                objs.Remove(_removeBuffer[i]);
        }

        static public void UpdateObject<T>(Dictionary<uint,T> objs,float dt)
            where T: SceneObject
        {
            foreach (var pair in objs)
            {
                if (!pair.Value.IsDie)
                {
                    pair.Value.OnUpdate(dt);
                }
            }
        }

        static public void DestoryObject<T>(Dictionary<uint,T> objs)
            where T: SceneObject
        {
            foreach (var pair in objs)
            {
                if (pair.Value != null)
                    pair.Value.DestoryThis();
            }
            objs.Clear();
        }

        private void _blinkUpdate(float dt)
        {
            this.blinkTime -= dt;
            var value = this.blinkTime / blinkInterval;
            var intValue = Mathf.FloorToInt(value);
            float alpha = 0;
            if (intValue % 2 == 0)
                alpha = Mathf.Lerp(blinkMinAlpha, 1, 1.0f - (value - intValue) / blinkInterval);
            else
                alpha = Mathf.Lerp(blinkMinAlpha, 1, (value - intValue) / blinkInterval);
            _setEntityAlpha(alpha);
        }
    }
}
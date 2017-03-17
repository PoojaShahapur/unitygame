using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Giant
{
    public class RecycleGameObject : MonoBehaviour,IDisposable
    {
        public GameObjectPool pool { set; get; }
        public string TypeName
        {
            get
            {
                if (pool != null)
                    return pool.TypeName;
                return "";
            }
        }

        virtual public void Reset()
        {
            if (pool != null)
                pool.ResetTransform(this);
        }

        virtual public void Dispose()
        {
            var obj = this.gameObject;
            if (obj != null)
                GameObject.Destroy(obj);
        }
    }
}

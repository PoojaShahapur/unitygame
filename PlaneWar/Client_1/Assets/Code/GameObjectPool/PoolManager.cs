using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameBox.Framework;

namespace Giant
{
    public class PoolManager
    {
        private Dictionary<string, GameObjectPool> pools = new Dictionary<string, GameObjectPool>();
        public GameObjectPool GetPool(string path)
        {
            GameObjectPool pool;
            if (!pools.TryGetValue(path, out pool))
            {
                pool = new GameObjectPool(path);
                pools.Add(path, pool);
            }
            return pool;
        }

        public void CreatePool(string path, uint num)
        {
            GetPool(path).Preload(num);
        }

        public GameObject Load(string path)
        {
            return GetPool(path).Load();
        }

        public void Unload(GameObject obj)
        {
            var recObj = obj.GetComponent<RecycleGameObject>();
            if (recObj != null)
            {
                GetPool(recObj.TypeName).Unload(recObj);
            }
            else
            {
                GameObject.Destroy(obj);
            }
        }

        public void Clear(string path)
        {
            GameObjectPool pool;
            if (pools.TryGetValue(path, out pool))
            {
                pool.Dispose();
                pools.Remove(path);
            }
        }

        public void Clear()
        {
            foreach (var pair in pools)
            {
                pair.Value.Dispose();
            }
            pools.Clear();
        }
    }
}

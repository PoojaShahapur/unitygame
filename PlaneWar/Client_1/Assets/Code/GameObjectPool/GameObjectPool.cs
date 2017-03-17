using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GameBox.Framework;
using GameBox.Service.AssetManager;

namespace Giant
{
    public class GameObjectPool : IDisposable
    {
        public string TypeName { get; private set; }
        private IAsset asset;
        private Stack<RecycleGameObject> objectCaches = new Stack<RecycleGameObject>();
        public GameObjectPool(string path)
        {
            this.TypeName = path;
            this.asset = ServiceCenter.GetService<IAssetManager>().Load(this.TypeName, AssetType.PREFAB);
            if (this.asset == null)
                throw new Exception(string.Format("Load Prefab {0} Error!", this.TypeName));
        }

        public void ResetTransform(RecycleGameObject obj)
        {
            var t = obj.transform;
            var prefabT = asset.Cast<GameObject>().transform;
            t.SetParent(null);
            t.position = prefabT.position;
            t.rotation = prefabT.rotation;
            t.localScale = prefabT.localScale;
        }

        public void Preload(uint num)
        {
            for (var i = 0; i < num; ++i)
            {
                var obj = Create();
                obj.SetActive(false);
                objectCaches.Push(obj.GetComponent<RecycleGameObject>());
            }
        }

        public void PreloadAsyn(uint num, System.Action<float> processCall = null)
        {
            Game.instance.StartCoroutine(_PreloadAsyn(num, processCall));
        }

        private IEnumerator _PreloadAsyn(uint num, System.Action<float> processCall)
        {
            for (var i = 0; i < num; ++i)
            {
                Preload(1);
                if (processCall != null)
                    processCall.Invoke((float)(i + 1) / (float)num);
                yield return null;
            }
            yield return null;
        }


        private GameObject Create()
        {
            var obj = GameObject.Instantiate(this.asset.Cast<GameObject>());
            var recObj = obj.GetComponent<RecycleGameObject>();
            if (recObj != null)
                recObj.pool = this;
            return obj;
        }

        public GameObject Load()
        {
            GameObject obj = null;
            if (objectCaches.Count > 0)
            {
                var recObj = objectCaches.Pop();
                obj = recObj.gameObject;
                obj.SetActive(this.asset.Cast<GameObject>().activeSelf);
            }
            else
            {
                obj = Create();
            }
            return obj;
        }

        public void Unload(GameObject obj)
        {
            var recycle = obj.GetComponent<RecycleGameObject>();
            if (recycle != null)
            {
                this.Unload(obj);
            }
        }

        public void Unload(RecycleGameObject obj)
        {
            if (obj != null && obj.TypeName == this.TypeName)
            {
                obj.gameObject.SetActive(false);
                obj.Reset();
                objectCaches.Push(obj);
            }
        }

        public void Dispose()
        {
            while (objectCaches.Count > 0)
            {
                var obj = objectCaches.Pop();
                obj.Dispose();
            }
            if (asset != null)
                asset.Dispose();
            asset = null;
        }
    }
}
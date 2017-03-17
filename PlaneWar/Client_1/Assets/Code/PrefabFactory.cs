using UnityEngine;
using GameBox.Service.ObjectPool;
using System;
using GameBox.Framework;
using GameBox.Service.AssetManager;

public class PrefabFactory : IRecycleProcesser, IRecycleFactory
{
    #region IRecycleFactory
    public bool SupportCreating
    {
        get {
            return true;
        }
    }

    public object CreateObject(string type)
    {
        var asset = this.assetManager.Load(type, AssetType.PREFAB);
        return GameObject.Instantiate(asset.Cast<GameObject>());
    }

    public void CreateObjectAsync(string type, Action<object> handler)
    {
        this.assetManager.LoadAsync(type, AssetType.PREFAB, asset =>
        {
            handler(GameObject.Instantiate(asset.Cast<GameObject>()));
        });
    }

    public void DestroyObject(object recycleObject)
    {
        GameObject.Destroy(recycleObject as GameObject);
    }

    public void ReclaimObject(object recycleObject)
    {
        var go = recycleObject as GameObject;
        go.SetActive(false);
        go.transform.SetParent(null);
    }

    public void RecoverObject(object recycleObject)
    {
        var go = recycleObject as GameObject;
        go.SetActive(true);
    }
    #endregion

    public void Initialize()
    {
        this.assetManager = ServiceCenter.GetService<IAssetManager>();
        this.pool = ServiceCenter.GetService<IRecycleManager>().Create("Prefab", this, this);
    }

    public void Preload(string path, int count)
    {
        this.pool.Preload(path, count);
    }

    public void PreloadAsync(string path, int count, Action callback)
    {
        this.pool.PreloadAsync(path, count, callback);
    }

    public GameObject Pick(string path)
    {
        return this.pool.Pick<GameObject>(path);
    }

    public void Drop(string path, GameObject go)
    {
        this.pool.Drop(path, go);
    }

    private IAssetManager assetManager = null;
    private IRecyclePool pool = null;
}

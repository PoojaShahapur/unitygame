using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Common
{
    public interface IResItem : IEventDispatch, IDispatchObject
    {
        bool HasLoaded();
        string GetPath();
        GameObject InstantiateObject(string resname);
        UnityEngine.Object getObject(string resname);
        string getPrefabName();         // 只有 Prefab 资源才实现这个函数
    }
}
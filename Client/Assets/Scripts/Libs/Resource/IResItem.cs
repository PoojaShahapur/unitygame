using SDK.Common;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    public interface IResItem : IEventDispatch, IDispatchObject
    {
        bool HasLoaded();
        string GetPath();
        GameObject InstantiateObject(string resName);
        UnityEngine.Object getObject(string resName);
        string getPrefabName();         // 只有 Prefab 资源才实现这个函数
        byte[] getBytes(string resName);            // 获取字节数据
        string getText(string resName);             // 获取文本数据
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Common
{
    public interface IRes : IEventDispatch
    {
        bool HasLoaded();
        string GetPath();
        GameObject InstantiateObject(string resname);
        UnityEngine.Object getObject(string resname);
    }
}
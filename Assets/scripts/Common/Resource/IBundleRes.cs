using System;
using UnityEngine;
using System.Collections.Generic;

namespace SDK.Common
{
    interface IBundleRes : IRes
    {
        GameObject InstantiateObject(string resname);
        UnityEngine.Object gerObject(string resname);
    }
}

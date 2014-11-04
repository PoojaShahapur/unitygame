using System.Collections;
using UnityEngine;

namespace SDK.Common
{
    public interface ICoroutineMgr
    {
        Coroutine StartCoroutine(IEnumerator routine);
    }
}
using System.Collections;
using UnityEngine;

/**
 * @brief 这个模块永远也不动态更新，一旦修改必须重新下载客户端，包括最基本的启动代码
 */
public class Entry : MonoBehaviour
{
    void Start()
    {
        // 必然从服务器下载第一个模块
        StartCoroutine(downloadStart());
    }

    // 下载 Start 模块
    IEnumerator downloadStart()
    {
        long curTime = System.DateTime.Now.Ticks;
        System.TimeSpan timeSpan = new System.TimeSpan(curTime);

        WWW www = WWW.LoadFromCacheOrDownload("http://127.0.0.1/UnityServer/Start.unity3d", (int)timeSpan.TotalSeconds);
        yield return www;

        // 使用预设加载
        AssetBundle bundle = www.assetBundle;
#if UNITY_5
        // Unity5
        UnityEngine.Object bt = bundle.LoadAsset("Assets/Resources/Module/Start");
#elif UNITY_4_6 || UNITY_4_5
        // Unity4
        UnityEngine.Object bt = bundle.Load("Assets/Resources/Module/Start");
#endif
        UnityEngine.GameObject go = Instantiate(bt) as GameObject;
        bundle.Unload(false);
        bundle = null;
        www.Dispose();
        www = null;
    }
}
using System.Collections;
using UnityEngine;

class StreamingAssetsLoad : MonoBehaviour
{
    void Start()
    {
        //StartCoroutine(TestLoadAssetBundles());
        StartCoroutine(TestLoadText());
    }

    protected string getLocalReadDir()
    {
#if UNITY_EDITOR
        string filepath = "file://" + Application.dataPath + "/StreamingAssets/";
#elif UNITY_IPHONE
        string filepath = Application.dataPath +"/Raw/";
#elif UNITY_ANDROID
        string filepath = "jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_STANDALONE_WIN
        string filepath = "file://" + Application.dataPath +"/StreamingAssets/";
#elif UNITY_WEBPLAYER
        string filepath = "file://" + Application.dataPath +"/StreamingAssets/";
#else
        string filepath = "file://" + Application.dataPath +"/StreamingAssets/";
#endif

        return filepath;
    }

    protected string getWWWStreamingAssetsPath()
    {
#if UNITY_EDITOR
        string filepath = "file:///" + Application.streamingAssetsPath;
#elif UNITY_IPHONE
        string filepath = "file:///" + Application.streamingAssetsPath;
#elif UNITY_ANDROID
        string filepath = Application.streamingAssetsPath;
#elif UNITY_STANDALONE_WIN
        string filepath = "file:///" + Application.streamingAssetsPath;
#elif UNITY_WEBPLAYER
        string filepath = "file:///" + Application.streamingAssetsPath;
#else
        string filepath = "file:///" + Application.streamingAssetsPath;
#endif

        return filepath;
    }

    protected string getAssetBundlesStreamingAssetsPath()
    {
#if UNITY_EDITOR
        string filepath = Application.streamingAssetsPath;
#elif UNITY_IPHONE
        string filepath = Application.streamingAssetsPath;
#elif UNITY_ANDROID
        string filepath = Application.dataPath + "!assets";
#elif UNITY_STANDALONE_WIN
        string filepath = Application.streamingAssetsPath;
#elif UNITY_WEBPLAYER
        string filepath = Application.streamingAssetsPath;
#else
        string filepath = Application.streamingAssetsPath;
#endif

        return filepath;
    }

    protected IEnumerator TestLoadText()
    {
        //string path = getLocalReadDir() + "aaa.txt";
        string path = getWWWStreamingAssetsPath() + "/aaa.txt";

        Debug.Log(string.Format("Path {0}", path));
        WWW www = new WWW(path);

        yield return www;

        if(string.IsNullOrEmpty(www.error) && www.isDone)
        {
            Debug.Log("Loaded");

            if(www.size > 0)
            {
                Debug.Log(string.Format("Load Size is {0} ", www.size));
                Debug.Log(string.Format("Load Text is {0} ", www.text));
            }
        }
        else
        {
            Debug.Log("Failed");
        }
    }

    protected IEnumerator TestLoadAssetBundles()
    {
        //string path = getLocalReadDir() + "testcube.unity3d";
        string path = getAssetBundlesStreamingAssetsPath() + "/testcube.unity3d";
        string assetPath = "assets/resources/model/testcube.prefab";
        AssetBundle assetBundle = null;
        GameObject tmpGo = null;
        GameObject insGo = null;
        AssetBundleRequest assetBundleRequest = null;

        AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(path);
        yield return req;

        if(req.isDone && req.assetBundle != null)
        {
            Debug.Log("Loaded");
            assetBundle = req.assetBundle;
            assetBundleRequest = assetBundle.LoadAssetAsync<GameObject>(assetPath);
            yield return assetBundleRequest;
            if(assetBundleRequest.isDone)
            {
                tmpGo = assetBundleRequest.asset as GameObject;
                insGo = UnityEngine.Object.Instantiate<GameObject>(tmpGo);
            }
        }
        else
        {
            Debug.Log("Failed");
            yield break;
        }

        assetBundle.Unload(false);
    }
}
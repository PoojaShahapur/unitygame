using UnityEngine;
using System.Collections;
using System.IO;
using SDK.Lib;

namespace Game.Main
{
    /**
     * @brief 这个模块主要是加载代码基础模块，然后加载游戏功能模块，然后加载资源
     */
    public class MainModule
    {
        private string mAppURL = "http://127.0.0.1/StreamingAssets/Module/App.unity3d";
        private string mAppName = "NoDestroy";
        private string mAppPath = "Assets/Resources/Module/App.prefab";
        private int mLoadType;
        public MonoBehaviour mEntry;

        public void run()
        {
            if (MacroDef.PKG_RES_LOAD)
            {
                mLoadType = 1;
            }
            else
            {
                mLoadType = 0;
            }

            if (mLoadType == 0)
            {
                loadFromDefaultAssetBundle();
            }
            else if (mLoadType == 1)   // 直接动本地文件加载
            {
                loadFromAssetBundle();
            }
            else if (mLoadType == 2)
            {
                mEntry.StartCoroutine(DownloadAppAsset());
            }
        }

        // Update is called once per frame
        //void Update()
        //{

        //}

        //void OnGUI()
        //{
        //    //设置按钮中文字的颜色  
        //    GUI.color = Color.green;
        //    //设置按钮的背景色  
        //    GUI.backgroundColor = Color.red;

        //    if (GUI.Button(new Rect(100, 100, 300, 40), "点击加载游戏代码，开始游戏"))
        //    {
        //        if (mLoadType == 0)
        //        {
        //            loadFromDefaultAssetBundle();
        //        }
        //        else if (mLoadType == 1)
        //        {
        //            loadFromAssetBundle();
        //        }
        //        else if (mLoadType == 2)
        //        {
        //            StartCoroutine(DownloadAppAsset());
        //        }
        //    }
        //}

        protected void loadFromDefaultAssetBundle()
        {
            mAppURL = "Module/MainModule";

            UnityEngine.Object prefabObj = Resources.Load(mAppURL);
            if (prefabObj)
            {
                GameObject noDestroy = UnityEngine.Object.Instantiate(prefabObj) as GameObject;
                noDestroy.name = mAppName;            // 程序里面获取都是按照 "NoDestroy" 获取名字的
                GameObject appGo = noDestroy.transform.Find("AppGo").gameObject;
                appGo.AddComponent<AppRoot>();
            }
        }

        // CreateFromFile(注意这种方法只能用于standalone程序）这是最快的加载方法
        // AssetBundle.CreateFromFile 这个函数仅支持未压缩的资源。这是加载资产包的最快方式。自己被这个函数坑了好几次，一定是非压缩的资源，如果压缩式不能加载的，加载后，内容也是空的
        protected void loadFromAssetBundle()
        {
            mAppURL = Path.Combine(MainUtil.getLocalReadDir(), "Module/App.unity3d");

            //AssetBundle assetBundle = AssetBundle.CreateFromFile(mAppURL);
            byte[] bytes = MainUtil.LoadFileByte(mAppURL);
            AssetBundle assetBundle = AssetBundle.LoadFromMemory(bytes);

            if (assetBundle != null)
            {
                //string[] nameList = assetBundle.GetAllAssetNames();
#if UNITY_5
                // Unity5
                Object bt = assetBundle.LoadAsset(mAppPath);
#elif UNITY_4_6 || UNITY_4_5
                // Unity4
                Object bt = assetBundle.Load(mAppPath);
#endif
                GameObject appGo = UnityEngine.Object.Instantiate(bt) as GameObject;
                appGo.name = mAppName;            // 程序里面获取都是按照 "App" 获取名字的
                GameObject noDestroy = GameObject.Find("NoDestroy");
                Object.DontDestroyOnLoad(noDestroy);
                appGo.transform.parent = noDestroy.transform;
                assetBundle.Unload(false);
            }
            else
            {
                Debug.Log("Module/App.unity3d 加载失败");
            }
        }

        // 下载 app 模块
        IEnumerator DownloadAppAsset()
        {
            mAppURL = "http://127.0.0.1/StreamingAssets/Module/App.unity3d";

            //下载场景，加载场景
            WWW app3w = WWW.LoadFromCacheOrDownload(mAppURL, 15);
            yield return app3w;

            // 使用预设加载
            AssetBundle bundle = app3w.assetBundle;
#if UNITY_5
            // Unity5
            Object bt = bundle.LoadAsset(mAppName);
#elif UNITY_4_6 || UNITY_4_5
            // Unity4
            Object bt = bundle.Load(mAppName);
#endif
            GameObject appGo = UnityEngine.Object.Instantiate(bt) as GameObject;
            appGo.name = mAppName;            // 程序里面获取都是按照 "App" 获取名字的
            GameObject noDestroy = GameObject.Find("NoDestroy");
            Object.DontDestroyOnLoad(noDestroy);
            appGo.transform.parent = noDestroy.transform;
            bundle.Unload(false);
            yield return null;
        }

        // 加载所有的模块
        protected void loadAllModule()
        {

        }

        // 加载主要场景
        protected void loadMainScene()
        {

        }
    }
}
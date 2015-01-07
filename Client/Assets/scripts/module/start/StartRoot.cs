using SDK.Lib;
using UnityEngine;
using System.Collections;

namespace SDK.Lib
{
    /**
     * @brief 这个模块主要是加载代码基础模块，然后加载游戏功能模块，然后加载资源
     */
    public class StartRoot : MonoBehaviour
    {
        private string m_appURL = "http://127.0.0.1/StreamingAssets/Module/App.unity3d";
        private string m_appName = "App";
        private int m_loadType;

        void Awake()
        {
            //Application.targetFrameRate = 30;
        }

        // Use this for initialization
        void Start()
        {
            m_loadType = 0;

            if (0 == m_loadType)
            {
                m_appURL = "Module/App";
            }
            else if(m_loadType == 2)
            {
                m_appURL = "http://127.0.0.1/StreamingAssets/Module/App.unity3d";
            }
            else
            {
                //m_appURL = Application.dataPath + "/StreamingAssets/Module/App.unity3d";
                m_appURL = "file://" + Application.dataPath + "/StreamingAssets/Module/App.unity3d";
            }

            if (m_loadType == 0)
            {
                loadFromDefaultAssetBundle();
            }
            else if (m_loadType == 1)
            {
                loadFromAssetBundle();
            }
            else if (m_loadType == 2)
            {
                StartCoroutine(DownloadAppAsset());
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        //void OnGUI()
        //{
        //    //设置按钮中文字的颜色  
        //    GUI.color = Color.green;
        //    //设置按钮的背景色  
        //    GUI.backgroundColor = Color.red;

        //    if (GUI.Button(new Rect(100, 100, 300, 40), "点击加载游戏代码，开始游戏"))
        //    {
        //        if (m_loadType == 0)
        //        {
        //            loadFromDefaultAssetBundle();
        //        }
        //        else if (m_loadType == 1)
        //        {
        //            loadFromAssetBundle();
        //        }
        //        else if (m_loadType == 2)
        //        {
        //            StartCoroutine(DownloadAppAsset());
        //        }
        //    }
        //}

        protected void loadFromDefaultAssetBundle()
        {
            UnityEngine.Object prefabObj = Resources.Load(m_appURL);
            if (prefabObj)
            {
                GameObject appGo = Instantiate(prefabObj) as GameObject;
                appGo.name = m_appName;            // 程序里面获取都是按照 "App" 获取名字的
                GameObject noDestroy = GameObject.Find("NoDestroy");
                appGo.transform.parent = noDestroy.transform;
            }
        }

        // CreateFromFile(注意这种方法只能用于standalone程序）这是最快的加载方法
        // AssetBundle.CreateFromFile 这个函数仅支持未压缩的资源。这是加载资产包的最快方式。自己被这个函数坑了好几次，一定是非压缩的资源，如果压缩式不能加载的，加载后，内容也是空的
        protected void loadFromAssetBundle()
        {
            AssetBundle assetBundle = AssetBundle.CreateFromFile(m_appURL);
            if (assetBundle != null)
            {
                // Unity5
                // Object bt = assetBundle.LoadAsset(m_appName);
                // Unity4
                Object bt = assetBundle.Load(m_appName);
                GameObject appGo = Instantiate(bt) as GameObject;
                appGo.name = m_appName;            // 程序里面获取都是按照 "App" 获取名字的
                GameObject noDestroy = GameObject.Find("NoDestroy");
                appGo.transform.parent = noDestroy.transform;
                assetBundle.Unload(false);
            }
        }

        // 下载 app 模块
        IEnumerator DownloadAppAsset()
        {
            //下载场景，加载场景
            WWW app3w = WWW.LoadFromCacheOrDownload(m_appURL, 15);
            yield return app3w;

            // 使用预设加载
            AssetBundle bundle = app3w.assetBundle;
            // Unity5
            // Object bt = bundle.LoadAsset(m_appName);
            // Unity4
            Object bt = bundle.Load(m_appName);
            GameObject appGo = Instantiate(bt) as GameObject;
            appGo.name = m_appName;            // 程序里面获取都是按照 "App" 获取名字的
            GameObject noDestroy = GameObject.Find("NoDestroy");
            appGo.transform.parent = noDestroy.transform;
            bundle.Unload(false);
            yield return null;
        }
    }
}
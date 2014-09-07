using San.Guo;
using UnityEngine;
using System.Collections;

namespace San.Guo
{
    /**
     * @brief 这个模块主要是加载代码基础模块，然后加载游戏功能模块，然后加载资源
     */
    public class StartRoot : MonoBehaviour
    {
        private string m_SceneURL = "http://127.0.0.1/app.unity3d";
        private string m_levelName = "app";
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnGUI()
        {
            //设置按钮中文字的颜色  
            GUI.color = Color.green;
            //设置按钮的背景色  
            GUI.backgroundColor = Color.red;

            if (GUI.Button(new Rect(100, 100, 300, 40), "点击加载游戏代码，开始游戏"))
            {
                StartCoroutine(DownloadAssetAndScene());
            }
        }

        IEnumerator DownloadAssetAndScene()
        {
            //下载场景，加载场景
            WWW scene = WWW.LoadFromCacheOrDownload(m_SceneURL, 1);
            yield return scene;

            AsyncOperation async = Application.LoadLevelAsync(m_levelName);
            yield return async;
        }
    }
}
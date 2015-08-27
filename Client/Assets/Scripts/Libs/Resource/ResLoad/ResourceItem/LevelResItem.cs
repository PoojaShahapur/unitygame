using System;
using UnityEngine;
using System.Collections;

namespace SDK.Lib
{
    public class LevelResItem : ResItem
    {
        protected string m_levelName;

        public LevelResItem()
        {
            
        }

        public string levelName
        {
            get
            {
                return m_levelName;
            }
            set
            {
                m_levelName = value;
            }
        }

        override public void init(LoadItem item)
        {
            base.init(item);

            // 如果是打包成 AssetBundle ，然后放在本地磁盘，要加载的话，需要 WWW 打开，AssetBundle.CreateFromFile 是不行的
            //string path;
            //path = Application.dataPath + "/" + m_path;
            //AssetBundle asset = AssetBundle.CreateFromFile(path);
            //asset.LoadAll();
            //Object[] resArr = asset.LoadAllAssets();
            if (m_resNeedCoroutine)
            {
                Ctx.m_instance.m_coroutineMgr.StartCoroutine(initAssetByCoroutine());
            }
            else
            {
                initAsset();
                //Ctx.m_instance.m_coroutineMgr.StartCoroutine(initAssetNextFrame());
            }
        }

        override public void reset()
        {
            base.reset();
        }

        // Resources.Load就是从一个缺省打进程序包里的AssetBundle里加载资源，而一般AssetBundle文件需要你自己创建，运行时 动态加载，可以指定路径和来源的。
        protected void initAsset()
        {
            //string path = Application.dataPath + "/" + m_path;
            //string path = m_path;       // 注意这个是场景打包的时候场景的名字，不是目录，这个场景一定要 To add a level to the build settings use the menu File->Build Settings...
            Application.LoadLevel(m_levelName);

            refCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            refCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        // 奇怪，Level 加载完成后立马获取里面的 GameObject ，有的时候可以，有的时候获取不到，因此间隔一帧后再获取
        protected IEnumerator initAssetNextFrame()
        {
            Application.LoadLevel(m_levelName);

            yield return new WaitForEndOfFrame();
            refCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            refCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        protected IEnumerator initAssetByCoroutine()
        {
            //string path = Application.dataPath + "/" + m_path;
            AsyncOperation asyncOpt = Application.LoadLevelAsync(m_levelName);
            yield return asyncOpt;

            // 确保场景资源都创建出来
            //bool bEmptyCreated = false;
            //while ((bEmptyCreated = (GameObject.Find("EmptyObject") != null)) == false)
            //{
                //yield return new WaitForSeconds(2);
            //    yield return 1;
            //}

            // asyncOpt.progress == 1.0f
            if (null != asyncOpt && asyncOpt.isDone)
            {
                refCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                refCountResLoadResultNotify.resLoadState.setFailed();
            }

            refCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }
    }
}
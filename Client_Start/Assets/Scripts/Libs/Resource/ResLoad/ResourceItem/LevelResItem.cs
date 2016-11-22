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
                Ctx.mInstance.mCoroutineMgr.StartCoroutine(initAssetByCoroutine());
            }
            else
            {
                initAsset();
                //Ctx.mInstance.mCoroutineMgr.StartCoroutine(initAssetNextFrame());
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

            bool isSuccess = true;

#if UNITY_4 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
            if (Application.CanStreamedLevelBeLoaded(m_levelName))
            {
                isSuccess = true;
                Application.LoadLevel(m_levelName);
            }
            else
            {
                isSuccess = false;
            }
#else
            if (Application.CanStreamedLevelBeLoaded(m_levelName))
            {
                isSuccess = true;
                UnityEngine.SceneManagement.SceneManager.LoadScene(m_levelName);
            }
            else
            {
                isSuccess = false;
            }
#endif

            // Level 加载完成后 Application.loadedLevelName 记录的仍然是加载 Level 之前的场景的名字，不能使用这个字段进行判断
            // if (Application.loadedLevelName == m_levelName)
            //{
            //    Debug.Log("aaa");
            //}

            if (isSuccess)
            {
                Ctx.mInstance.mLogSys.log(string.Format("LevelResItem::initAsset, Success", m_origPath), LogTypeId.eLogResLoader);

                m_refCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                Ctx.mInstance.mLogSys.log(string.Format("LevelResItem::initAsset, Failed", m_origPath), LogTypeId.eLogResLoader);

                m_refCountResLoadResultNotify.resLoadState.setFailed();
            }

            m_refCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        // 奇怪，Level 加载完成后立马获取里面的 GameObject ，有的时候可以，有的时候获取不到，因此间隔一帧后再获取
        protected IEnumerator initAssetNextFrame()
        {
#if UNITY_4 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
            Application.LoadLevel(m_levelName);
#else
            UnityEngine.SceneManagement.SceneManager.LoadScene(m_levelName);
#endif

            yield return new WaitForEndOfFrame();

            m_refCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            m_refCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        protected IEnumerator initAssetByCoroutine()
        {
            //string path = Application.dataPath + "/" + m_path;
#if UNITY_4 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
            AsyncOperation asyncOpt = Application.LoadLevelAsync(m_levelName);
#else
            AsyncOperation asyncOpt = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(m_levelName);
#endif

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
                Ctx.mInstance.mLogSys.log(string.Format("LevelResItem::initAssetByCoroutine, LoadScene Success, Path is", m_origPath), LogTypeId.eLogResLoader);

                m_refCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                Ctx.mInstance.mLogSys.log(string.Format("LevelResItem::initAssetByCoroutine, LoadScene Fail, Path is", m_origPath), LogTypeId.eLogResLoader);

                m_refCountResLoadResultNotify.resLoadState.setFailed();
            }

            m_refCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        public override void unload(bool unloadAllLoadedObjects = true)
        {
            // 场景卸载需要调用这个函数吗，内部应该会自己调用吧，为了安全，自己调用一次
            UtilApi.UnloadUnusedAssets();           // 卸载共享资源

            base.unload(unloadAllLoadedObjects);
        }
    }
}
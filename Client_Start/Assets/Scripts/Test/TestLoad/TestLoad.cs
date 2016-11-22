using SDK.Lib;
using UnityEngine;

namespace UnitTest
{
    public class TestLoad
    {
        public void run()
        {
            //testModelLoad();
            //testUILoad();
            //testAsyncLoadImage();
            //testSyncLoadRefCount();
            //testAsyncLoadAtlasRefCount();
            //testAsyncLoadUIPrefabRefCount();
            //testLoadBT();
            //testLoadAnimatorController();
            //testLoadAnimatorControllerPrefab();
            //testLoadScriptAnimatorControllerPrefab();
            //testScriptController();

            testLoadPreafab();
            //testLoadText();
            //this.testTextureLoader();
            //this.testDownload();
            //this.testLuaLoad();

            //this.testTextLoaderAndStream();
        }

        protected void testModelLoad()
        {
            LoadParam param;
            param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
            param.setPath("Model/log.prefab");
            param.m_loadEventHandle = onLoadEventHandle;
            Ctx.mInstance.mModelMgr.load<ModelRes>(param);
            Ctx.mInstance.mPoolSys.deleteObj(param);
        }

        public virtual void onLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            if (res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                
            }
            else if (res.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                
            }
        }

        protected void testUILoad()
        {
            UnityEngine.Object obj = Resources.Load("UI/UITuJian/CardSetCard");
        }

        protected void testAsyncLoadImage()
        {
            LoadParam param;
            param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
            param.setPath(CVAtlasName.TuJianDyn);
            param.m_subPath = "ka1_paizu";
            param.m_loadEventHandle = onImageLoadEventHandle;
            Ctx.mInstance.mAtlasMgr.getAndLoadImage(param);
            Ctx.mInstance.mPoolSys.deleteObj(param);
        }

        public virtual void onImageLoadEventHandle(IDispatchObject dispObj)
        {
            ImageItem image = dispObj as ImageItem;
        }

        public virtual void onUIPrefabLoadEventHandle(IDispatchObject dispObj)
        {
            PrefabRes uiPrefab = dispObj as PrefabRes;
        }

        public void testSyncLoadRefCount()
        {
            PrefabRes aaa = Ctx.mInstance.mPrefabMgr.getAndSyncLoad<PrefabRes>("UI/UIChat/UIChat.prefab");
            PrefabRes bbb = Ctx.mInstance.mPrefabMgr.getAndSyncLoad<PrefabRes>("UI/UIChat/UIChat.prefab");
        }

        public void testAsyncLoadUIPrefabRefCount()
        {
            LoadParam param;
            param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
            param.setPath("UI/UIChat/UIChat.prefab");
            param.m_loadEventHandle = onUIPrefabLoadEventHandle;
            PrefabRes aaa = Ctx.mInstance.mPrefabMgr.getAndLoad<PrefabRes>(param);
            Ctx.mInstance.mPoolSys.deleteObj(param);

            Ctx.mInstance.mPrefabMgr.unload(aaa.getResUniqueId(), onUIPrefabLoadEventHandle);

            param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
            param.setPath("UI/UIChat/UIChat.prefab");
            param.m_loadEventHandle = onUIPrefabLoadEventHandle;
            PrefabRes bbb = Ctx.mInstance.mPrefabMgr.getAndLoad<PrefabRes>(param);
            Ctx.mInstance.mPoolSys.deleteObj(param);

            Ctx.mInstance.mPrefabMgr.unload(bbb.getResUniqueId(), onUIPrefabLoadEventHandle);
        }

        public void testAsyncLoadAtlasRefCount()
        {
            LoadParam param;
            param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
            param.setPath(CVAtlasName.TuJianDyn);
            param.m_subPath = "ka1_paizu";
            param.m_loadEventHandle = onImageLoadEventHandle;
            ImageItem aaa = Ctx.mInstance.mAtlasMgr.getAndLoadImage(param);
            Ctx.mInstance.mPoolSys.deleteObj(param);

            param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
            param.setPath(CVAtlasName.TuJianDyn);
            param.m_subPath = "ka1_paizu";
            param.m_loadEventHandle = onImageLoadEventHandle;
            ImageItem bbb = Ctx.mInstance.mAtlasMgr.getAndLoadImage(param);
            Ctx.mInstance.mPoolSys.deleteObj(param);

            Ctx.mInstance.mAtlasMgr.unloadImage(aaa, onImageLoadEventHandle);
            Ctx.mInstance.mAtlasMgr.unloadImage(bbb, onImageLoadEventHandle);
        }

        protected void testLoadAnimatorController()
        {
            LoadParam param;
            param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
            param.setPath("Animation/Scene/CommonCard.controller");
            param.m_loadNeedCoroutine = false;
            param.m_resNeedCoroutine = false;
            ResItem bbb = Ctx.mInstance.mResLoadMgr.getAndLoad(param);
            System.Type type = bbb.getObject("").GetType();
            Ctx.mInstance.mLogSys.log(string.Format("类型名字 {0}", type.FullName));
            Ctx.mInstance.mPoolSys.deleteObj(param);

            //GameObject _go = UtilApi.createGameObject("AnimatorController");
            //Animator animator = _go.AddComponent<Animator>();
            //animator.runtimeAnimatorController = bbb.getObject() as ;
        }

        protected void testLoadAnimatorControllerPrefab()
        {
            LoadParam param;
            param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
            param.setPath("Animation/Scene/Control.prefab");
            param.m_loadNeedCoroutine = false;
            param.m_resNeedCoroutine = false;
            PrefabResItem bbb = Ctx.mInstance.mResLoadMgr.getAndLoad(param) as PrefabResItem;
            Ctx.mInstance.mPoolSys.deleteObj(param);

            GameObject _go = UtilApi.createGameObject("AnimatorController");
            Animator animator = _go.AddComponent<Animator>();
            GameObject _insObj = bbb.InstantiateObject("");
            RuntimeAnimatorController controlCom = _insObj.GetComponent<Animator>().runtimeAnimatorController;
            //_insObj.GetComponent<Animator>().runtimeAnimatorController = null;
            RuntimeAnimatorController copyCom = RuntimeAnimatorController.Instantiate(controlCom);
            animator.runtimeAnimatorController = copyCom;
            //UtilApi.SetParent(_insObj, _go);
            UtilApi.Destroy(_insObj);
        }

        protected void testLoadScriptAnimatorControllerPrefab()
        {
            string path = string.Format("{0}{1}", Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathSceneAnimatorController], "SelfCardAni.asset");
            LoadParam param;
            param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
            param.setPath(path);
            param.m_loadNeedCoroutine = false;
            param.m_resNeedCoroutine = false;
            PrefabResItem bbb = Ctx.mInstance.mResLoadMgr.getAndLoad(param) as PrefabResItem;
            Ctx.mInstance.mPoolSys.deleteObj(param);
        }

        protected void testScriptController()
        {
            string path = string.Format("{0}{1}", Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathSceneAnimatorController], "SelfCardAni.asset");
            ControllerRes res = Ctx.mInstance.mControllerMgr.getAndSyncLoad<ControllerRes>(path);
            RuntimeAnimatorController copyCom = res.InstantiateController();
            res.DestroyControllerInstance(copyCom);
            Ctx.mInstance.mControllerMgr.unload(res.getResUniqueId(), null);
        }

        protected void testLoadPreafab()
        {
            AuxPrefabLoader loader = new AuxPrefabLoader("", false);
            loader.syncLoad("Model/TestCube.prefab");
            loader.dispose();
            loader = null;
        }

        protected void testLoadText()
        {
            AuxTextLoader loader = new AuxTextLoader();
            loader.syncLoad("XmlConfig/ReadMe.txt");
            loader.getText();
        }

        protected void testDownload()
        {
            AuxDownloader auxDownload = new AuxDownloader();
            auxDownload.download("XmlConfig/ReadMe.txt", null, 84);
        }

        // 测试加载 Lua 
        protected void testLuaLoad()
        {
            TextAsset textAsset = Resources.Load<TextAsset>("AuxComponent");
            if(textAsset != null)
            {
                if(textAsset.bytes != null || textAsset.text != null)
                {
                    string str = textAsset.text;
                }
            }
        }

        protected void testTextLoaderAndStream()
        {
            AuxTextLoader sync_loader_a = new AuxTextLoader();
            sync_loader_a.syncLoad("XmlConfig/Test_a.txt");

            Ctx.mInstance.mLogSys.log(string.Format("XmlConfig/Test_a Sync Content {0} ", sync_loader_a.getText()), LogTypeId.eLogTestRL);

            sync_loader_a.dispose();


            AuxTextLoader sync_loader_b = new AuxTextLoader();
            sync_loader_b.syncLoad("XmlConfig/Test_b.txt");

            Ctx.mInstance.mLogSys.log(string.Format("XmlConfig/Test_b Sync Content {0}", sync_loader_b.getText()), LogTypeId.eLogTestRL);

            sync_loader_b.dispose();


            AuxTextLoader async_loader_a = new AuxTextLoader();
            async_loader_a.asyncLoad("XmlConfig/Test_a.txt", onAsyncLoad_a);


            AuxTextLoader async_loader_b = new AuxTextLoader();
            async_loader_b.asyncLoad("XmlConfig/Test_b.txt", onAsyncLoad_b);

            MDataStream dataStream_a = new MDataStream(MFileSys.msDataStreamResourcesPath + "/XmlConfig/Test_a.txt", onDataStreamResourceLoaded);

            MDataStream dataStream_b = new MDataStream(MFileSys.msDataStreamStreamingAssetsPath + "/XmlConfig/Test_b.txt", onDataStreamStreamingAssetsLoaded);
        }

        protected void onAsyncLoad_a(IDispatchObject dispObj)
        {
            AuxTextLoader async_loader_a = dispObj as AuxTextLoader;

            Ctx.mInstance.mLogSys.log(string.Format("XmlConfig/Test_a Async Content {0} ", async_loader_a.getText()), LogTypeId.eLogTestRL);

            async_loader_a.dispose();
        }

        protected void onAsyncLoad_b(IDispatchObject dispObj)
        {
            AuxTextLoader async_loader_b = dispObj as AuxTextLoader;

            Ctx.mInstance.mLogSys.log(string.Format("XmlConfig/Test_b Async Content {0} ", async_loader_b.getText()), LogTypeId.eLogTestRL);

            async_loader_b.dispose();
        }

        protected void onDataStreamResourceLoaded(IDispatchObject dispObj)
        {
            MDataStream dataStream = dispObj as MDataStream;
            if (dataStream.isValid())
            {
                string text = dataStream.readText();
                Ctx.mInstance.mLogSys.log(string.Format("XmlConfig/Test_a.txt DataStream Content {0} ", text), LogTypeId.eLogResLoader);
            }
            else
            {
                Ctx.mInstance.mLogSys.log("Open File Failed", LogTypeId.eLogResLoader);
            }

            dataStream.dispose();
        }

        protected void onDataStreamStreamingAssetsLoaded(IDispatchObject dispObj)
        {
            MDataStream dataStream = dispObj as MDataStream;
            if (dataStream.isValid())
            {
                string text = dataStream.readText();
                Ctx.mInstance.mLogSys.log(string.Format("XmlConfig/Test_b.txt DataStream Content {0} ", text), LogTypeId.eLogResLoader);
            }
            else
            {
                Ctx.mInstance.mLogSys.log("Open File Failed", LogTypeId.eLogResLoader);
            }

            dataStream.dispose();
        }

        protected void testTextureLoader()
        {
            AuxTextureLoader loader = new AuxTextureLoader();
            loader.syncLoad("Materials/Textures/Terrain/haidi01.png");
            loader.dispose();
            loader = null;
        }
    }
}
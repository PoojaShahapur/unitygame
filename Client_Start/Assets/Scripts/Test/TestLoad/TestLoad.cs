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
        }

        protected void testModelLoad()
        {
            LoadParam param;
            param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.setPath("Model/log.prefab");
            param.m_loadEventHandle = onLoadEventHandle;
            Ctx.m_instance.m_modelMgr.load<ModelRes>(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
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
            param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.setPath(CVAtlasName.TuJianDyn);
            param.m_subPath = "ka1_paizu";
            param.m_loadEventHandle = onImageLoadEventHandle;
            Ctx.m_instance.m_atlasMgr.getAndLoadImage(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
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
            PrefabRes aaa = Ctx.m_instance.m_prefabMgr.getAndSyncLoad<PrefabRes>("UI/UIChat/UIChat.prefab");
            PrefabRes bbb = Ctx.m_instance.m_prefabMgr.getAndSyncLoad<PrefabRes>("UI/UIChat/UIChat.prefab");
        }

        public void testAsyncLoadUIPrefabRefCount()
        {
            LoadParam param;
            param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.setPath("UI/UIChat/UIChat.prefab");
            param.m_loadEventHandle = onUIPrefabLoadEventHandle;
            PrefabRes aaa = Ctx.m_instance.m_prefabMgr.getAndLoad<PrefabRes>(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);

            Ctx.m_instance.m_prefabMgr.unload(aaa.getResUniqueId(), onUIPrefabLoadEventHandle);

            param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.setPath("UI/UIChat/UIChat.prefab");
            param.m_loadEventHandle = onUIPrefabLoadEventHandle;
            PrefabRes bbb = Ctx.m_instance.m_prefabMgr.getAndLoad<PrefabRes>(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);

            Ctx.m_instance.m_prefabMgr.unload(bbb.getResUniqueId(), onUIPrefabLoadEventHandle);
        }

        public void testAsyncLoadAtlasRefCount()
        {
            LoadParam param;
            param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.setPath(CVAtlasName.TuJianDyn);
            param.m_subPath = "ka1_paizu";
            param.m_loadEventHandle = onImageLoadEventHandle;
            ImageItem aaa = Ctx.m_instance.m_atlasMgr.getAndLoadImage(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);

            param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.setPath(CVAtlasName.TuJianDyn);
            param.m_subPath = "ka1_paizu";
            param.m_loadEventHandle = onImageLoadEventHandle;
            ImageItem bbb = Ctx.m_instance.m_atlasMgr.getAndLoadImage(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);

            Ctx.m_instance.m_atlasMgr.unloadImage(aaa, onImageLoadEventHandle);
            Ctx.m_instance.m_atlasMgr.unloadImage(bbb, onImageLoadEventHandle);
        }

        protected void testLoadAnimatorController()
        {
            LoadParam param;
            param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.setPath("Animation/Scene/CommonCard.controller");
            param.m_loadNeedCoroutine = false;
            param.m_resNeedCoroutine = false;
            ResItem bbb = Ctx.m_instance.m_resLoadMgr.getAndLoad(param);
            System.Type type = bbb.getObject("").GetType();
            Ctx.m_instance.m_logSys.log(string.Format("类型名字 {0}", type.FullName));
            Ctx.m_instance.m_poolSys.deleteObj(param);

            //GameObject _go = UtilApi.createGameObject("AnimatorController");
            //Animator animator = _go.AddComponent<Animator>();
            //animator.runtimeAnimatorController = bbb.getObject() as ;
        }

        protected void testLoadAnimatorControllerPrefab()
        {
            LoadParam param;
            param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.setPath("Animation/Scene/Control.prefab");
            param.m_loadNeedCoroutine = false;
            param.m_resNeedCoroutine = false;
            PrefabResItem bbb = Ctx.m_instance.m_resLoadMgr.getAndLoad(param) as PrefabResItem;
            Ctx.m_instance.m_poolSys.deleteObj(param);

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
            string path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathSceneAnimatorController], "SelfCardAni.asset");
            LoadParam param;
            param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.setPath(path);
            param.m_loadNeedCoroutine = false;
            param.m_resNeedCoroutine = false;
            PrefabResItem bbb = Ctx.m_instance.m_resLoadMgr.getAndLoad(param) as PrefabResItem;
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        protected void testScriptController()
        {
            string path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathSceneAnimatorController], "SelfCardAni.asset");
            ControllerRes res = Ctx.m_instance.m_controllerMgr.getAndSyncLoad<ControllerRes>(path);
            RuntimeAnimatorController copyCom = res.InstantiateController();
            res.DestroyControllerInstance(copyCom);
            Ctx.m_instance.m_controllerMgr.unload(res.getResUniqueId(), null);
        }
    }
}
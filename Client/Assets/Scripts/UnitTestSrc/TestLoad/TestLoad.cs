using BehaviorLibrary;
using SDK.Lib;
using UnityEngine;

namespace UnitTestSrc
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
            testLoadSkillAction();
        }

        protected void testModelLoad()
        {
            LoadParam param;
            param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.m_path = "Model/log.prefab";
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
            LocalFileSys.modifyLoadParam(CVAtlasName.TuJianDyn, param);
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
            UIPrefabRes uiPrefab = dispObj as UIPrefabRes;
        }

        public void testSyncLoadRefCount()
        {
            UIPrefabRes aaa = Ctx.m_instance.m_uiPrefabMgr.getAndSyncLoad<UIPrefabRes>("UI/UIChat/UIChat.prefab");
            UIPrefabRes bbb = Ctx.m_instance.m_uiPrefabMgr.getAndSyncLoad<UIPrefabRes>("UI/UIChat/UIChat.prefab");
        }

        public void testAsyncLoadUIPrefabRefCount()
        {
            LoadParam param;
            param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            LocalFileSys.modifyLoadParam("UI/UIChat/UIChat.prefab", param);
            param.m_loadEventHandle = onUIPrefabLoadEventHandle;
            UIPrefabRes aaa = Ctx.m_instance.m_uiPrefabMgr.getAndLoad<UIPrefabRes>(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);

            Ctx.m_instance.m_uiPrefabMgr.unload(aaa.GetPath(), onUIPrefabLoadEventHandle);

            param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            LocalFileSys.modifyLoadParam("UI/UIChat/UIChat.prefab", param);
            param.m_loadEventHandle = onUIPrefabLoadEventHandle;
            UIPrefabRes bbb = Ctx.m_instance.m_uiPrefabMgr.getAndLoad<UIPrefabRes>(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);

            Ctx.m_instance.m_uiPrefabMgr.unload(bbb.GetPath(), onUIPrefabLoadEventHandle);
        }

        public void testAsyncLoadAtlasRefCount()
        {
            LoadParam param;
            param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            LocalFileSys.modifyLoadParam(CVAtlasName.TuJianDyn, param);
            param.m_subPath = "ka1_paizu";
            param.m_loadEventHandle = onImageLoadEventHandle;
            ImageItem aaa = Ctx.m_instance.m_atlasMgr.getAndLoadImage(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);

            param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            LocalFileSys.modifyLoadParam(CVAtlasName.TuJianDyn, param);
            param.m_subPath = "ka1_paizu";
            param.m_loadEventHandle = onImageLoadEventHandle;
            ImageItem bbb = Ctx.m_instance.m_atlasMgr.getAndLoadImage(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);

            Ctx.m_instance.m_atlasMgr.unloadImage(aaa, onImageLoadEventHandle);
            Ctx.m_instance.m_atlasMgr.unloadImage(bbb, onImageLoadEventHandle);
        }

        public void testLoadBT()
        {
            BehaviorTreeRes bt = Ctx.m_instance.m_aiSystem.behaviorTreeMgr.getAndSyncLoadBT(BTID.e1000);
            Ctx.m_instance.m_aiSystem.behaviorTreeMgr.unload(bt.GetPath(), null);
        }

        protected void testLoadAnimatorController()
        {
            LoadParam param;
            param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            LocalFileSys.modifyLoadParam("Animation/Scene/CommonCard.controller", param);
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
            LocalFileSys.modifyLoadParam("Animation/Scene/Control.prefab", param);
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
            LocalFileSys.modifyLoadParam(path, param);
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
            Ctx.m_instance.m_controllerMgr.unload(res.GetPath(), null);
        }

        protected void testLoadSkillAction()
        {
            string _path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathSkillAction], "1000.xml");
            Ctx.m_instance.m_skillActionMgr.getAndSyncLoad<SkillActionRes>(_path);
        }
    }
}
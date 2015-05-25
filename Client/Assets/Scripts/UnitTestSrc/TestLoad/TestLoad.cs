using SDK.Common;
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
            testAsyncLoadUIPrefabRefCount();
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
    }
}
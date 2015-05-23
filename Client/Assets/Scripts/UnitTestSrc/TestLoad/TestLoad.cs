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
            //testLoad();
            testAsyncLoadImage();
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

        protected void testLoad()
        {
            UnityEngine.Object obj = Resources.Load("UI/UITuJian/CardSet");
        }

        protected void testAsyncLoadImage()
        {
            LoadParam param;
            param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.m_path = CVAtlasName.TuJianDyn;
            param.m_subPath = "ka1_paizu";
            param.m_loadEventHandle = onImageLoadEventHandle;
            Ctx.m_instance.m_atlasMgr.loadImage(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        public virtual void onImageLoadEventHandle(IDispatchObject dispObj)
        {
            ImageItem image = dispObj as ImageItem;
        }
    }
}
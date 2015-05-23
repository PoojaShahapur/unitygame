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
            testLoad();
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

        public virtual void onLoadEventHandle(IDispatchObject dispObj)            // 资源加载成功
        {
            ResItem res = dispObj as ResItem;
            if (res.resLoadState.hasSuccessLoaded())
            {
                
            }
            else if (res.resLoadState.hasFailed())
            {
                
            }
        }

        protected void testLoad()
        {
            UnityEngine.Object obj = Resources.Load("UI/UITuJian/CardSet");
        }
    }
}
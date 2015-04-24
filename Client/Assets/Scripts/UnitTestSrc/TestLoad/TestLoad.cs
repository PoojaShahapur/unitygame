using SDK.Common;
using SDK.Lib;
namespace UnitTestSrc
{
    public class TestLoad
    {
        public void run()
        {
            testModelLoad();
        }

        protected void testModelLoad()
        {
            LoadParam param;
            param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.m_path = "Model/log.prefab";
            param.m_loaded = onLoaded;
            param.m_failed = onFailed;
            Ctx.m_instance.m_modelMgr.load<ModelRes>(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        public virtual void onLoaded(IDispatchObject resEvt)            // 资源加载成功
        {

        }

        public virtual void onFailed(IDispatchObject resEvt)            // 资源加载成功
        {

        }
    }
}
using SDK.Common;
using SDK.Lib;
namespace UnitTestSrc
{
    /**
     * @brief 测试动画
     */
    public class TestAni
    {
        public void run()
        {
            testAnim();
        }

        // 测试 .anim 动画
        protected void testAnim()
        {
            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            string resPath = "Anim/boxcampush";
            LocalFileSys.modifyLoadParam(resPath, param);
            param.m_loaded = onLoaded;
            param.m_failed = onFailed;
            Ctx.m_instance.m_resLoadMgr.loadResources(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        protected void onLoaded(IDispatchObject resEvt)
        {
            IResItem res = resEvt as IResItem;
            res.InstantiateObject("Anim/boxcampush");

            Ctx.m_instance.m_resLoadMgr.unload("Anim/boxcampush");
        }

        protected void onFailed(IDispatchObject resEvt)
        {
            Ctx.m_instance.m_resLoadMgr.unload("Anim/boxcampush");
        }
    }
}
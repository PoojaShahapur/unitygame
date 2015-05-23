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
            param.m_loadEventHandle = onLoadEventHandle;
            Ctx.m_instance.m_resLoadMgr.loadResources(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        protected void onLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            if (res.resLoadState.hasSuccessLoaded())
            {
                res.InstantiateObject("Anim/boxcampush");
            }
            else if (res.resLoadState.hasFailed())
            {

            }

            Ctx.m_instance.m_resLoadMgr.unload("Anim/boxcampush");
        }
    }
}
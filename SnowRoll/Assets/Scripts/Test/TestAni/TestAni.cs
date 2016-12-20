using SDK.Lib;
using UnityEngine;

namespace UnitTest
{
    /**
     * @brief 测试动画
     */
    public class TestAni
    {
        public void run()
        {
            //testAnim();
            //testDopeSheetAni();
        }

        // 测试 .anim 动画
        protected void testAnim()
        {
            LoadParam param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
            string resPath = "Anim/boxcampush";
            param.setPath(resPath);
            param.mLoadEventHandle = onLoadEventHandle;
            Ctx.mInstance.mResLoadMgr.loadAsset(param);
            Ctx.mInstance.mPoolSys.deleteObj(param);
        }

        protected void onLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            if (res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                res.InstantiateObject("Anim/boxcampush");
            }
            else if (res.refCountResLoadResultNotify.resLoadState.hasFailed())
            {

            }

            Ctx.mInstance.mResLoadMgr.unload("Anim/boxcampush", onLoadEventHandle);
        }

        protected void testDopeSheetAni()
        {
            DopeSheetAni ani = new DopeSheetAni();
            //GameObject _go = UtilApi.createGameObject("AnimatorController");
            GameObject _go = UtilApi.CreatePrimitive(PrimitiveType.Cube);
            ani.setControlInfo("Animation/Scene/CardModel.asset");
            ani.setGO(_go);
            ani.syncUpdateControl();
            ani.stateId = 2;
            ani.play();
            //ani.stop();
            //ani.dispose();
            //UtilApi.Destroy(_go);
        }
    }
}
using SDK.Lib;

namespace UnitTest
{
    public class TestCoroutinePrefabIns
    {
        protected PrefabRes mPrefabRes;
        protected AuxPrefabLoader mAuxPrefabLoader;

        public void run()
        {
            //testIns();
            testPrefabComponent();
        }

        protected void testIns()
        {
            string path = "Model/Character/ChangCard";
            mPrefabRes = Ctx.mInstance.mPrefabMgr.getAndAsyncLoadRes(path, onResLoaded);
        }

        protected void onResLoaded(IDispatchObject dispObj)
        {
            mPrefabRes = dispObj as PrefabRes;
            ResInsEventDispatch evt = null;
            if (mPrefabRes.hasSuccessLoaded())
            {
                for (int idx = 0; idx < 1000; ++idx)
                {
                    evt = new ResInsEventDispatch();
                    evt.addEventHandle(null, onResIns);
                    mPrefabRes.InstantiateObject(mPrefabRes.getPrefabName(), false, UtilMath.ZeroVec3, UtilMath.UnitQuat, evt);
                }
            }
            else if(mPrefabRes.hasFailed())
            {
                Ctx.mInstance.mPrefabMgr.unload(mPrefabRes.getResUniqueId(), onResLoaded);
            }
        }

        protected void onResIns(IDispatchObject dispObj)
        {
            ResInsEventDispatch disp = dispObj as ResInsEventDispatch;
        }

        public void testPrefabComponent()
        {
            mAuxPrefabLoader = new AuxPrefabLoader();
            string path = "Model/Character/ChangCard";
            mAuxPrefabLoader.asyncLoad(path, onPrefabComLoaded);
        }

        public void onPrefabComLoaded(IDispatchObject dispObj)
        {
            mAuxPrefabLoader = dispObj as AuxPrefabLoader;
        }
    }
}
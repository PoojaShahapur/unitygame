using SDK.Lib;

namespace UnitTest
{
    public class TestCoroutinePrefabIns
    {
        protected PrefabRes mPrefabRes;
        protected AuxPrefabComponent mAuxPrefabComponent;

        public void run()
        {
            //testIns();
            testPrefabComponent();
        }

        protected void testIns()
        {
            string path = "Model/Character/ChangCard";
            mPrefabRes = Ctx.m_instance.m_prefabMgr.getAndAsyncLoadRes(path, onResLoaded);
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
                    evt.addEventHandle(onResIns);
                    mPrefabRes.InstantiateObject(mPrefabRes.GetPath(), evt);
                }
            }
            else if(mPrefabRes.hasFailed())
            {
                Ctx.m_instance.m_prefabMgr.unload(mPrefabRes.GetPath(), onResLoaded);
            }
        }

        protected void onResIns(IDispatchObject dispObj)
        {
            ResInsEventDispatch disp = dispObj as ResInsEventDispatch;
        }

        public void testPrefabComponent()
        {
            mAuxPrefabComponent = new AuxPrefabComponent();
            string path = "Model/Character/ChangCard";
            mAuxPrefabComponent.asyncLoad(path, onPrefabComLoaded);
        }

        public void onPrefabComLoaded(IDispatchObject dispObj)
        {
            mAuxPrefabComponent = dispObj as AuxPrefabComponent;
        }
    }
}
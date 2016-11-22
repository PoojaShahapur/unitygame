using UnityEngine;

namespace SDK.Lib
{
    public class ControllerRes : InsResBase
    {
        protected SOAnimatorController mController;
        protected RuntimeAnimatorController mInsController;

        override protected void initImpl(ResItem res)
        {
            this.mController = res.getObject(res.getPrefabName()) as SOAnimatorController;
            base.initImpl(res);
        }

        public RuntimeAnimatorController InstantiateController()
        {
            this.mInsController = UtilApi.Instantiate(this.mController.mAnimatorController) as RuntimeAnimatorController;
            return this.mInsController;
        }

        public void DestroyControllerInstance(RuntimeAnimatorController insController_)
        {
            UtilApi.Destroy(insController_);
        }
    }
}
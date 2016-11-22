using UnityEngine;

namespace SDK.Lib
{
    public class SOAnimatorController : ScriptableObject
    {
        public RuntimeAnimatorController mAnimatorController;

        public void addAnimator(string path, RuntimeAnimatorController animatorController_)
        {
            this.mAnimatorController = animatorController_;
        }

        public void unload()
        {
            UtilApi.UnloadAsset(this.mAnimatorController);
        }
    }
}
using UnityEngine;
using System.Collections.Generic;
using SDK.Common;

namespace SDK.Lib
{
    public class SOAnimatorController : ScriptableObject
    {
        public RuntimeAnimatorController m_animatorController;

        public void addAnimator(string path, RuntimeAnimatorController animatorController_)
        {
            m_animatorController = animatorController_;
        }

        public void unload()
        {
            UtilApi.UnloadAsset(m_animatorController);
        }
    }
}
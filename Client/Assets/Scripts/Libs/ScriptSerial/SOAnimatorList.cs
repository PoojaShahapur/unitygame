using UnityEngine;
using System.Collections.Generic;
using SDK.Common;

namespace SDK.Lib
{
    public class SOAnimatorList : ScriptableObject
    {
        [System.Serializable]
        public class SerialObject
        {
            public RuntimeAnimatorController m_animatorController;
            public string m_path;
        }

        public List<SerialObject> m_objList = new List<SerialObject>();

        public void addAnimator(string path, RuntimeAnimatorController animatorController_)
        {
            SerialObject obj = new SerialObject();
            obj.m_animatorController = animatorController_;
            obj.m_path = path;
            m_objList.Add(obj);
        }

        public void unload()
        {
            if (m_objList != null)
            {
                foreach (SerialObject sprite in m_objList)
                {
                    UtilApi.UnloadAsset(sprite.m_animatorController);
                }
                m_objList = null;
            }
        }
    }
}
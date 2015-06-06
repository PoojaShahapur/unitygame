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
            public Animator m_animator;
            public string m_path;
        }

        public List<SerialObject> m_objList = new List<SerialObject>();

        public void addAnimator(string path, Animator animator_)
        {
            SerialObject obj = new SerialObject();
            obj.m_animator = animator_;
            obj.m_path = path;
            m_objList.Add(obj);
        }

        public void unload()
        {
            if (m_objList != null)
            {
                foreach (SerialObject sprite in m_objList)
                {
                    UtilApi.UnloadAsset(sprite.m_animator);
                }
                m_objList = null;
            }
        }
    }
}
using UnityEngine;
using System.Collections.Generic;

namespace SDK.Lib
{
    public class SOSpriteList : ScriptableObject
    {
        [System.Serializable]
        public class SerialObject
        {
            public Sprite m_sprite;
            public string m_path;
        }

        public List<SerialObject> m_objList = new List<SerialObject>();

        public void addSprite(string path, Sprite sprite)
        {
            SerialObject obj = new SerialObject();
            obj.m_sprite = sprite;
            obj.m_path = path;
            m_objList.Add(obj);
        }

        public void unload()
        {
            if (m_objList != null)
            {
                foreach (SerialObject sprite in m_objList)
                {
                    UtilApi.UnloadAsset(sprite.m_sprite.texture);
                }
                m_objList = null;
            }
        }
    }
}
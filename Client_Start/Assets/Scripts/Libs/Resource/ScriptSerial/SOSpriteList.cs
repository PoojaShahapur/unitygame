using UnityEngine;
using System.Collections.Generic;

namespace SDK.Lib
{
    public class SOSpriteList : ScriptableObject
    {
        [System.Serializable]
        public class SerialObject
        {
            public Sprite mSprite;
            public string mPath;
        }

        public List<SerialObject> mObjList = new List<SerialObject>();

        public void addSprite(string path, Sprite sprite)
        {
            SerialObject obj = new SerialObject();
            obj.mSprite = sprite;
            obj.mPath = path;
            this.mObjList.Add(obj);
        }

        public void unload()
        {
            if (this.mObjList != null)
            {
                foreach (SerialObject sprite in this.mObjList)
                {
                    UtilApi.UnloadAsset(sprite.mSprite.texture);
                }
                this.mObjList = null;
            }
        }
    }
}
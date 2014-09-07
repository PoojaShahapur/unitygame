using System;
using UnityEngine;
using System.Collections;

namespace SDK.Lib
{
    public class LevelRes : Res
    {
        protected string m_levelName;

        //public LevelRes(string path, string lvlname)
        //    : base(path)
        public LevelRes()
        {
            //m_levelName = lvlname;
        }

        public string levelName
        {
            get
            {
                return m_levelName;
            }
            set
            {
                m_levelName = value;
            }
        }

        override public void init(LoadItem item)
        {
            StartCoroutine(initAssetByCoroutine());
        }

        override public IEnumerator initAssetByCoroutine()
        {
            AsyncOperation async = Application.LoadLevelAsync(m_levelName);
            yield return async;

            if(onInited != null)
            {
                onInited(this);
            }
        }

        override public void reset()
        {
            base.reset();
            m_levelName = "";
        }
    }
}
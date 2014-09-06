using System;
using UnityEngine;
using System.Collections;

namespace San.Guo
{
    public class LevelRes : Res
    {
        protected string m_levelName;

        public LevelRes(string path, string lvlname)
            : base(path)
        {
            m_levelName = lvlname;
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
            StartCoroutine(initAsset());
        }

        override public IEnumerator initAsset()
        {
            AsyncOperation async = Application.LoadLevelAsync(m_levelName);
            yield return async;

            if(onInited != null)
            {
                onInited();
            }
        }
    }
}
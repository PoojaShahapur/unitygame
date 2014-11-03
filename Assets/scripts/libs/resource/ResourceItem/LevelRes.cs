using System;
using UnityEngine;
using System.Collections;

namespace SDK.Lib
{
    public class LevelRes : Res
    {
        public LevelRes()
        {
            
        }

        override public void init(LoadItem item)
        {
            initAsset();
        }

        override public void initAsset()
        {
            //if(onInited != null)
            //{
            //    onInited(this);
            //}
        }

        override public void reset()
        {
            base.reset();
        }
    }
}
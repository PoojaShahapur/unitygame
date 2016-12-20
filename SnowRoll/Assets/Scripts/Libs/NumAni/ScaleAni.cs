using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    public class ScaleAni : ITweenAniBase
    {
        // 目标信息
        protected Vector3 mDestScale;     // 最终缩放

        public ScaleAni()
        {

        }

        public Vector3 destScale
        {
            set
            {
                mDestScale = value;
            }
        }

        public override void play()
        {
            base.play();
            buildAniParam();
        }

        protected void buildAniParam()
        {
            Hashtable args;
            args = new Hashtable();
            base.buildAniBasicParam(args);

            args["scale"] = mDestScale;
            args["time"] = m_time;
            args["easetype"] = m_easeType;
            args["looptype"] = m_loopType;
            incItweenCount();
            iTween.ScaleTo(mGo, args);
        }
    }
}
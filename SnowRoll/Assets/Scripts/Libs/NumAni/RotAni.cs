using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    public class RotAni : ITweenAniBase
    {
        // 目标信息
        protected Vector3 mDestRot;       // 最终旋转

        public RotAni()
        {

        }

        public Vector3 destRot
        {
            set
            {
                mDestRot = value;
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

            args["rotation"] = mDestRot;
            args["time"] = mTime;
            args["islocal"] = true;
            args["easetype"] = mEaseType;
            args["looptype"] = mLoopType;
            incItweenCount();
            iTween.RotateTo(mGo, args);
        }
    }
}
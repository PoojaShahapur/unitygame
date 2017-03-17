using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    public class PosAni : ITweenAniBase
    {
        // 目标信息
        protected Vector3 mDestPos;       // 最终位置

        public Vector3 destPos
        {
            get
            {
                return mDestPos;
            }
            set
            {
                mDestPos = value;
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

            args["position"] = mDestPos;
            args["time"] = mTime;
            args["islocal"] = true;

            args["easetype"] = mEaseType;
            args["looptype"] = mLoopType;
            incItweenCount();
            iTween.MoveTo(mGo, args);
        }
    }
}
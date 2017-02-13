﻿using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 数字动画，移动、缩放
     */
    public class STAni : ITweenAniBase
    {
        // 目标信息
        protected Vector3 mDestPos;       // 最终位置
        protected Vector3 mDestScale;     // 最终缩放

        public STAni()
        {

        }

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

            args["position"] = mDestPos;
            args["time"] = mTime;
            args["islocal"] = true;

            args["easetype"] = mEaseType;
            args["looptype"] = mLoopType;
            //args["method"] = "to";
            //args["type"] = "color";
            incItweenCount();
            iTween.MoveTo(mGo, args);

            args = new Hashtable();
            base.buildAniBasicParam(args);
            args["scale"] = mDestScale;
            args["time"] = mTime;
            args["easetype"] = mEaseType;
            args["looptype"] = mLoopType;
            incItweenCount();
            iTween.ScaleTo(mGo, args);
        }
    }
}
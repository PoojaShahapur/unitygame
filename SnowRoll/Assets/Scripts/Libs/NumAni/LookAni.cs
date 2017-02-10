using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    public class LookAni : ITweenAniBase
    {
        protected Vector3 mLooktarget;         // 观察的位置
        protected string mAxis;                // 轴
        protected float mSpeed;                // 旋转速度 iTween.cs , if(tweenArguments.Contains("speed")){ speed 暂时不用

        public LookAni()
        {
            mAxis = "y";
        }

        public Vector3 looktarget
        {
            get
            {
                return mLooktarget;
            }
            set
            {
                mLooktarget = value;
            }
        }

        public string axis
        {
            get
            {
                return mAxis;
            }
            set
            {
                mAxis = value;
            }
        }

        public float speed
        {
            get
            {
                return mSpeed;
            }
            set
            {
                mSpeed = value;
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

            args["looktarget"] = looktarget;
            args["time"] = mTime;
            args["islocal"] = true;

            args["easetype"] = mEaseType;
            args["looptype"] = mLoopType;

            args["axis"] = mAxis;
            //args["speed"] = "speed";

            incItweenCount();
            iTween.MoveTo(mGo, args);
        }
    }
}
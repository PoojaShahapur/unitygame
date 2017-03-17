using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief ITween 动画基类
     */
    public class ITweenAniBase : NumAniBase
    {
        protected float mTime = 0.5f;      // 动画时间

        protected GameObject mDispGo;
        protected string mMethodName;

        protected iTween.EaseType mEaseType = iTween.EaseType.easeOutExpo; // iTween.EaseType.easeOutElastic;
        protected iTween.LoopType mLoopType = iTween.LoopType.none;

        protected int mItweenCount = 0;        // 一个动画启动的 Itween 的个数

        override public void setDispGo(GameObject go)
        {
            mDispGo = go;
        }

        override public void setMethodName(string str)
        {
            mMethodName = str;
        }

        override public void setTime(float value)
        {
            mTime = value;
        }

        override protected void buildAniBasicParam(Hashtable args)
        {
            args["oncompletetarget"] = mDispGo;
            args["oncomplete"] = mMethodName;
            args["oncompleteparams"] = this;
        }

        override public void setEaseType(iTween.EaseType value)
        {
            mEaseType = value;
        }

        override public void setLoopType(iTween.LoopType value)
        {
            mLoopType = value;
        }

        // 递增补间动画数量
        override public void incItweenCount()
        {
            ++mItweenCount;
        }

        override public int decItweenCount()
        {
            return --mItweenCount;
        }
    }
}
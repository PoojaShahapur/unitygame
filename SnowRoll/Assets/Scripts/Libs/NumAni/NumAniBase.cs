using System;
using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 数字动画基类
     */
    public abstract class NumAniBase
    {
        protected GameObject mGo;
        protected Action<NumAniBase> mAniEndDisp;  // 外部回调逻辑
        protected bool mIsPlaying = false;

        public NumAniBase()
        {

        }

        virtual public void dispose()
        {

        }

        virtual public void setGO(GameObject go)
        {
            mGo = go;
        }

        public void setAniEndDisp(Action<NumAniBase> disp)
        {
            mAniEndDisp = disp;
        }

        public Action<NumAniBase> getAniEndDisp()
        {
            return mAniEndDisp;
        }

        public bool bAniEndDispNotNull()
        {
            return mAniEndDisp != null;
        }

        virtual public void setDispGo(GameObject go)
        {
            
        }

        virtual public void setMethodName(string str)
        {
            
        }

        virtual public void setTime(float value)
        {
            
        }

        public virtual void play()
        {
            mIsPlaying = true;
        }

        public bool isPlaying()
        {
            return mIsPlaying;
        }

        public virtual void stop()
        {
            mIsPlaying = false;
        }

        public virtual void pause()
        {
            mIsPlaying = false;
        }

        virtual protected void buildAniBasicParam(Hashtable args)
        {

        }

        virtual public void setEaseType(iTween.EaseType value)
        {
            
        }

        virtual public void setLoopType(iTween.LoopType value)
        {
            
        }

        // 递增补间动画数量
        virtual public void incItweenCount()
        {
            
        }

        virtual public int decItweenCount()
        {
            return 0;
        }
    }
}
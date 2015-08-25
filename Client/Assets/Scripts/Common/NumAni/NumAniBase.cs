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
        protected GameObject m_go;
        protected Action<NumAniBase> m_aniEndDisp;  // 外部回调逻辑
        protected bool m_bPlaying = false;

        public NumAniBase()
        {

        }

        virtual public void dispose()
        {

        }

        virtual public void setGO(GameObject go)
        {
            m_go = go;
        }

        public void setAniEndDisp(Action<NumAniBase> disp)
        {
            m_aniEndDisp = disp;
        }

        public Action<NumAniBase> getAniEndDisp()
        {
            return m_aniEndDisp;
        }

        public bool bAniEndDispNotNull()
        {
            return m_aniEndDisp != null;
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
            m_bPlaying = true;
        }

        public bool isPlaying()
        {
            return m_bPlaying;
        }

        public virtual void stop()
        {
            m_bPlaying = false;
        }

        public virtual void pause()
        {
            m_bPlaying = false;
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
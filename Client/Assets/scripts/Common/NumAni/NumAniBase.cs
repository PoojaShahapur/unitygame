using System;
using System.Collections;
using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 数字动画基类
     */
    public abstract class NumAniBase
    {
        protected float m_time = 0.5f;      // 动画时间

        protected GameObject m_go;
        protected Action<NumAniBase> m_aniEndDisp;  // 外部回调逻辑

        protected GameObject m_dispGo;
        protected string m_methodName;

        protected iTween.EaseType m_easeType = iTween.EaseType.easeOutElastic;
        protected iTween.LoopType m_loopType = iTween.LoopType.none;

        protected bool m_bPlaying = false;
        protected int m_itweenCount = 0;        // 一个动画启动的 Itween 的个数

        public NumAniBase()
        {

        }

        public void setGO(GameObject go)
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

        public void setDispGo(GameObject go)
        {
            m_dispGo = go;
        }

        public void setMethodName(string str)
        {
            m_methodName = str;
        }

        public void setTime(float value)
        {
            m_time = value;
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

        }

        public virtual void pause()
        {

        }

        protected void buildAniBasicParam(Hashtable args)
        {
            args["oncompletetarget"] = m_dispGo;
            args["oncomplete"] = m_methodName;
            args["oncompleteparams"] = this;
        }

        public void setEaseType(iTween.EaseType value)
        {
            m_easeType = value;
        }

        public void setLoopType(iTween.LoopType value)
        {
            m_loopType = value;
        }

        // 递增补间动画数量
        public void incItweenCount()
        {
            ++m_itweenCount;
        }

        public int decItweenCount()
        {
            return --m_itweenCount;
        }
    }
}
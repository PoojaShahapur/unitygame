using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief ITween 动画基类
     */
    public class ITweenAniBase : NumAniBase
    {
        protected float m_time = 0.5f;      // 动画时间

        protected GameObject m_dispGo;
        protected string m_methodName;

        protected iTween.EaseType m_easeType = iTween.EaseType.easeOutExpo; // iTween.EaseType.easeOutElastic;
        protected iTween.LoopType m_loopType = iTween.LoopType.none;

        protected int m_itweenCount = 0;        // 一个动画启动的 Itween 的个数

        override public void setDispGo(GameObject go)
        {
            m_dispGo = go;
        }

        override public void setMethodName(string str)
        {
            m_methodName = str;
        }

        override public void setTime(float value)
        {
            m_time = value;
        }

        override protected void buildAniBasicParam(Hashtable args)
        {
            args["oncompletetarget"] = m_dispGo;
            args["oncomplete"] = m_methodName;
            args["oncompleteparams"] = this;
        }

        override public void setEaseType(iTween.EaseType value)
        {
            m_easeType = value;
        }

        override public void setLoopType(iTween.LoopType value)
        {
            m_loopType = value;
        }

        // 递增补间动画数量
        override public void incItweenCount()
        {
            ++m_itweenCount;
        }

        override public int decItweenCount()
        {
            return --m_itweenCount;
        }
    }
}
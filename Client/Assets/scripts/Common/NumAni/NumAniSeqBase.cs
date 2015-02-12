using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Common
{
    public class NumAniSeqBase
    {
        protected GameObject m_go;
        protected List<NumAniBase> m_numAniList = new List<NumAniBase>();
        protected Action<NumAniSeqBase> m_aniSeqEndDisp;          // 动画结束分发

        public void destroy()
        {
            m_numAniList.Clear();
            m_aniSeqEndDisp = null;
            m_go.transform.parent = null;
            UtilApi.Destroy(m_go);

            m_go = null;
        }

        public void stop()
        {

        }

        public void pause()
        {

        }

        public void addOneNumAni(NumAniBase ani)
        {
            m_numAniList.Add(ani);
            ani.setDispGo(m_go);
            ani.setMethodName("onAniEnd");
        }

        public void setAniSeqEndDisp(Action<NumAniSeqBase> disp)
        {
            m_aniSeqEndDisp = disp;
        }

        protected void endCurAni(NumAniBase ani)
        {
            if (ani.bAniEndDispNotNull())
            {
                ani.getAniEndDisp()(ani);
            }
            m_numAniList.Remove(ani);
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 动画
     */
    public class NumAniSeqBehaviour : MonoBehaviour
    {
        public Action<NumAniBase> onAniEndDisp;

        // 一个动画播放完成
        public void onAniEnd(NumAniBase ani)
        {
            if(onAniEndDisp != null)
            {
                onAniEndDisp(ani);
            }
        }
    }

    /**
     * @breif 数字动画序列，线性序列，仅支持顺序执行的
     */
    public class NumAniSeq
    {
        protected GameObject m_go;
        protected List<NumAniBase> m_numAniList = new List<NumAniBase>();
        protected Action<NumAniSeq> m_aniSeqEndDisp;          // 动画结束分发

        public NumAniSeq()
        {
            m_go = new GameObject("NumAniSeq");
            m_go.transform.parent = Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App].transform;
            NumAniSeqBehaviour seqBeh = m_go.AddComponent<NumAniSeqBehaviour>();
            seqBeh.onAniEndDisp = onAniEndDisp;
        }

        public void destroy()
        {
            m_numAniList.Clear();
            m_aniSeqEndDisp = null;
            m_go.transform.parent = null;
            UtilApi.Destroy(m_go);

            m_go = null;
        }

        public void play()
        {
            nextAni();
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

        public void setAniSeqEndDisp(Action<NumAniSeq> disp)
        {
            m_aniSeqEndDisp = disp;
        }

        protected void onAniEndDisp(NumAniBase ani)
        {
            endCurAni(ani);
            if(!nextAni())
            {
                if(m_aniSeqEndDisp != null)
                {
                    m_aniSeqEndDisp(this);
                }
            }
        }

        protected void endCurAni(NumAniBase ani)
        {
            if (ani.bAniEndDispNotNull())
            {
                ani.getAniEndDisp()(ani);
            }
            m_numAniList.Remove(ani);
        }

        // 成功开始下一个动画返回 true ，否则返回 false
        protected bool nextAni()
        {
            if(m_numAniList.Count > 0)
            {
                if (!m_numAniList[0].isPlaying())
                {
                    m_numAniList[0].play();
                    return true;
                }
            }

            return false;
        }
    }
}
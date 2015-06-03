using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Common
{
    /**
     * @breif 数字动画序列，线性序列，仅支持顺序执行的
     */
    public class NumAniSequence : NumAniSeqBase
    {
        public NumAniSequence()
        {
            m_go = UtilApi.createGameObject("NumAniSeq");
            m_go.transform.parent = Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App].transform;
            NumAniSeqBehaviour seqBeh = m_go.AddComponent<NumAniSeqBehaviour>();
            seqBeh.onAniEndDisp = onAniEndDisp;
        }

        public void play()
        {
            nextAni();
        }

        protected void onAniEndDisp(NumAniBase ani)
        {
            if (ani.decItweenCount() == 0)      // 如果 ITween 全部播放完成
            {
                endCurAni(ani);
                if (!nextAni())
                {
                    if (m_aniSeqEndDisp != null)
                    {
                        m_aniSeqEndDisp(this);
                    }
                }
            }
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
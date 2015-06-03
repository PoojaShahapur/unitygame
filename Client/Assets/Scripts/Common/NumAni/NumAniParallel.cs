using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Common
{
    /**
     * @breif 数字动画序列，同时执行，这个没有回调，因为 ITween 会自动释放相同的 ITween 
     */
    public class NumAniParallel : NumAniSeqBase
    {
        public NumAniParallel()
        {
            m_go = UtilApi.createGameObject("NumAniSeq");
            m_go.transform.parent = Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_App].transform;
            NumAniSeqBehaviour seqBeh = m_go.AddComponent<NumAniSeqBehaviour>();
            seqBeh.onAniEndDisp = onAniEndDisp;
        }

        public void play()
        {
            foreach (NumAniBase ani in m_numAniList)
            {
                if(!ani.isPlaying())
                {
                    ani.play();
                }
            }

            m_numAniList.Clear();
        }

        protected void onAniEndDisp(NumAniBase ani)
        {
            
        }
    }
}
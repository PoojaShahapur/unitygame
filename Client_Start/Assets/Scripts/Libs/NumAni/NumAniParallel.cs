namespace SDK.Lib
{
    /**
     * @breif 数字动画序列，同时执行，只监听最后一个动画结束，因为 ITween 会自动释放相同的 ITween 
     */
    public class NumAniParallel : NumAniSeqBase
    {
        protected NumAniBase m_lastAni;

        public NumAniParallel()
        {
            mGo = UtilApi.createGameObject("NumAniSeq");
            mGo.transform.parent = Ctx.mInstance.mLayerMgr.mPath2Go[NotDestroyPath.ND_CV_App].transform;
            NumAniSeqBehaviour seqBeh = mGo.AddComponent<NumAniSeqBehaviour>();
            seqBeh.onAniEndDisp = onAniEndDisp;
        }

        public void play()
        {
            if (m_numAniList.Count > 0)
            {
                m_lastAni = m_numAniList[0];

                foreach (NumAniBase ani in m_numAniList)
                {
                    if (!ani.isPlaying())
                    {
                        ani.play();
                    }
                }

                m_numAniList.Clear();
            }
            else
            {
                if (m_aniSeqEndDisp != null)
                {
                    m_aniSeqEndDisp(this);
                }
            }
        }

        protected void onAniEndDisp(NumAniBase ani)
        {
            if (ani.decItweenCount() == 0)      // 如果 ITween 全部播放完成
            {
                if (UtilApi.isAddressEqual(m_lastAni, ani))
                {
                    if (m_aniSeqEndDisp != null)
                    {
                        m_aniSeqEndDisp(this);
                    }
                }
            }
        }
    }
}
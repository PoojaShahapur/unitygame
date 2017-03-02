namespace SDK.Lib
{
    /**
     * @breif 数字动画序列，同时执行，只监听最后一个动画结束，因为 ITween 会自动释放相同的 ITween 
     */
    public class NumAniParallel : NumAniSeqBase
    {
        protected NumAniBase mLastAni;

        public NumAniParallel()
        {
            mGo = UtilApi.createGameObject("NumAniSeq");
            mGo.transform.parent = Ctx.mInstance.mLayerMgr.mPath2Go[NotDestroyPath.ND_CV_App].transform;
            NumAniSeqBehaviour seqBeh = mGo.AddComponent<NumAniSeqBehaviour>();
            seqBeh.onAniEndDisp = onAniEndDisp;
        }

        public void play()
        {
            if (mNumAniList.Count > 0)
            {
                mLastAni = mNumAniList[0];

                foreach (NumAniBase ani in mNumAniList)
                {
                    if (!ani.isPlaying())
                    {
                        ani.play();
                    }
                }

                mNumAniList.Clear();
            }
            else
            {
                if (mAniSeqEndDisp != null)
                {
                    mAniSeqEndDisp(this);
                }
            }
        }

        protected void onAniEndDisp(NumAniBase ani)
        {
            if (ani.decItweenCount() == 0)      // 如果 ITween 全部播放完成
            {
                if (UtilApi.isAddressEqual(mLastAni, ani))
                {
                    if (mAniSeqEndDisp != null)
                    {
                        mAniSeqEndDisp(this);
                    }
                }
            }
        }
    }
}
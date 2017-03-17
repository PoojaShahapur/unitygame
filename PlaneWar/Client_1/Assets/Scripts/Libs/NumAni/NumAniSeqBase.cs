using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    public class NumAniSeqBase
    {
        protected GameObject mGo;
        protected List<NumAniBase> mNumAniList = new List<NumAniBase>();
        protected Action<NumAniSeqBase> mAniSeqEndDisp;          // 动画结束分发

        public void destroy()
        {
            mNumAniList.Clear();
            mAniSeqEndDisp = null;
            mGo.transform.parent = null;
            UtilApi.Destroy(mGo);

            mGo = null;
        }

        public void stop()
        {

        }

        public void pause()
        {

        }

        public void addOneNumAni(NumAniBase ani)
        {
            mNumAniList.Add(ani);

            if (ani is ITweenAniBase)   // 如果是补间动画
            {
                ani.setDispGo(mGo);
                ani.setMethodName("onAniEnd");
            }
            else if(ani is DopeSheetAni)
            {
                ani.setAniEndDisp(onOneAniEndHandle);
            }
        }

        public void setAniSeqEndDisp(Action<NumAniSeqBase> disp)
        {
            mAniSeqEndDisp = disp;
        }

        protected void endCurAni(NumAniBase ani)
        {
            if (ani.bAniEndDispNotNull())
            {
                ani.getAniEndDisp()(ani);
            }
            ani.stop();                 // 停止动画
            mNumAniList.Remove(ani);
        }

        virtual protected void onOneAniEndHandle(NumAniBase ani)
        {

        }
    }
}
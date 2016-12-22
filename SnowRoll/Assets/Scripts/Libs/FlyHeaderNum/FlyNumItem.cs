using System;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @beirf 一项数字，可能有动画
     */
    public class FlyNumItem
    {
        protected NumResItem mNum;         // 数字
        protected NumAniParallel mNumAni;  // 数字动画
        protected Action<FlyNumItem> mAniEndDisp;

        public void setNum(int num)
        {
            this.mNum = new NumResItem();
            this.mNum.setNum(num);
        }

        public void setPos(Vector3 pos)
        {
            this.mNum.setPos(pos);
        }

        public void setDisp(Action<FlyNumItem> disp)
        {
            this.mAniEndDisp = disp;
        }

        public void setParent(GameObject parentGo)
        {
            this.mNum.setParent(parentGo);
        }

        public void play()
        {
            // 动画
            //PosAni posAni = new PosAni();
            //m_numAni = new NumAniParallel();
            //m_numAni.addOneNumAni(posAni);
            //posAni.setTime(5);
            //posAni.setGO(mNum.getParentGo());
            //posAni.destPos = mNum.getPos() + new Vector3(0, 3, 0);
            //m_numAni.play();

            // 启动定时器
            TimerItemBase timer = new TimerItemBase();
            timer.mInternal = 1;
            timer.mTotalTime = 1;
            timer.mTimerDisp.setFuncObject(endTimer);
            timer.startTimer();
        }

        protected void endTimer(TimerItemBase timer)
        {
            this.mNum.dispose();

            if(this.mAniEndDisp != null)
            {
                this.mAniEndDisp(this);
            }
        }
    }
}
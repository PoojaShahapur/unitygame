using UnityEngine;

namespace SDK.Lib
{
     /**
      * @brief Unity 自己的 DopeSheet 动画
      */
    public class DopeSheetAni : NumAniBase
    {
        protected AnimatorControl mAnimatorControl;    // 动画控制器
        protected int mStateId;        // 播放状态 Id

        public DopeSheetAni()
        {
            mAnimatorControl = new AnimatorControl();
            mAnimatorControl.oneAniPlayEndDisp.addEventHandle(null, onOneAniPlayEnd);
            mStateId = 0;
        }

        // 释放资源
        override public void dispose()
        {
            mAnimatorControl.dispose();
            mAnimatorControl = null;
        }

        public AnimatorControl animatorControl
        {
            get
            {
                return mAnimatorControl;
            }
        }

        public int stateId
        {
            get
            {
                return mStateId;
            }
            set
            {
                mStateId = value;
            }
        }

        public void setControlInfo(string path)
        {
            mAnimatorControl.setControlInfo(path);
        }

        override public void setGO(GameObject go_)
        {
            base.setGO(go_);
            mAnimatorControl.selfGo = go_;
        }

        // 同步更新控制器
        public void syncUpdateControl()
        {
            mAnimatorControl.syncUpdateControl();
        }

        // 一个动画播放结束
        public void onOneAniPlayEnd(IDispatchObject dispObj)
        {
            if(bAniEndDispNotNull())
            {
                mAniEndDisp(this);
            }

            stop();
        }

        override public void play()
        {
            base.play();
            mAnimatorControl.play(stateId);
        }

        override public void stop()
        {
            base.stop();
            mAnimatorControl.stop();
        }

        override public void pause()
        {
            base.pause();
        }

        public void enable()
        {
            mAnimatorControl.enable();
        }

        public void disable()
        {
            mAnimatorControl.disable();
        }
    }
}
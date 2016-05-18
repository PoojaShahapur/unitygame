using UnityEngine;

namespace SDK.Lib
{
     /**
      * @brief Unity 自己的 DopeSheet 动画
      */
    public class DopeSheetAni : NumAniBase
    {
        protected AnimatorControl m_animatorControl;    // 动画控制器
        protected int m_stateId;        // 播放状态 Id

        public DopeSheetAni()
        {
            m_animatorControl = new AnimatorControl();
            m_animatorControl.oneAniPlayEndDisp.addEventHandle(null, onOneAniPlayEnd);
            m_stateId = 0;
        }

        // 释放资源
        override public void dispose()
        {
            m_animatorControl.dispose();
            m_animatorControl = null;
        }

        public AnimatorControl animatorControl
        {
            get
            {
                return m_animatorControl;
            }
        }

        public int stateId
        {
            get
            {
                return m_stateId;
            }
            set
            {
                m_stateId = value;
            }
        }

        public void setControlInfo(string path)
        {
            m_animatorControl.setControlInfo(path);
        }

        override public void setGO(GameObject go_)
        {
            base.setGO(go_);
            m_animatorControl.selfGo = go_;
        }

        // 同步更新控制器
        public void syncUpdateControl()
        {
            m_animatorControl.syncUpdateControl();
        }

        // 一个动画播放结束
        public void onOneAniPlayEnd(IDispatchObject dispObj)
        {
            if(bAniEndDispNotNull())
            {
                m_aniEndDisp(this);
            }

            stop();
        }

        override public void play()
        {
            base.play();
            m_animatorControl.play(stateId);
        }

        override public void stop()
        {
            base.stop();
            m_animatorControl.stop();
        }

        override public void pause()
        {
            base.pause();
        }

        public void enable()
        {
            m_animatorControl.enable();
        }

        public void disable()
        {
            m_animatorControl.disable();
        }
    }
}
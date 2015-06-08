using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief Unity 最新的动画系统，只使用一个 StateId 控制状态切换， 1 状态是播放状态， 0 状态是停止状态
     */
    public class AnimatorControl : IDispatchObject
    {
        protected Animator m_animator;
        protected int m_stateId = 0;
        protected float m_stateDampTime = 0.1f;
        protected EventDispatch m_oneAniPlayEndDisp;    // 一个动画播放结束
        protected TimerItemBase m_nextFrametimer;       // 需要下一帧才能获取的数据
        protected TimerItemBase m_oneAniEndTimer;       // 一个动画结束定时器
        protected bool m_startPlay;     // 是否直接播放

        public AnimatorControl()
        {
            m_stateId = Animator.StringToHash("StateId");
            m_oneAniPlayEndDisp = new AddOnceAndCallOnceEventDispatch();
            m_startPlay = false;
        }

        public void dispose()
        {
            if(m_animator != null)
            {
                UtilApi.Destroy(m_animator.runtimeAnimatorController);
            }

            if(m_nextFrametimer != null)
            {
                Ctx.m_instance.m_timerMgr.delObject(m_nextFrametimer);
                m_nextFrametimer = null;
            }

            if (m_oneAniEndTimer != null)
            {
                Ctx.m_instance.m_timerMgr.delObject(m_oneAniEndTimer);
                m_oneAniEndTimer = null;
            }
        }

        public Animator animator
        {
            get
            {
                return m_animator;
            }
            set
            {
                m_animator = value;
            }
        }

        public EventDispatch oneAniPlayEndDisp
        {
            get
            {
                return m_oneAniPlayEndDisp;
            }
            set
            {
                m_oneAniPlayEndDisp = value;
            }
        }

        // 当前是否处于某个动画
        public bool bInAnimator(string aniName, int layerIdx)
        {
            AnimatorStateInfo state = m_animator.GetCurrentAnimatorStateInfo(layerIdx);
            return state.IsName(aniName);
        }

        // 当前是否在 Transition 
        public bool bInTransition(int layerIdx)
        {
            bool inTransition = m_animator.IsInTransition(0);
            return inTransition;
        }

        public void SetInteger(int id, int value)
        {
            m_animator.SetInteger(m_stateId, value);
            startNextFrameTimer();
        }

        // 启动下一帧定时器
        protected void startNextFrameTimer()
        {
            if (m_nextFrametimer == null)
            {
                m_nextFrametimer = new TimerItemBase();
                m_nextFrametimer.m_timerDisp = onNextFrameHandle;
            }

            m_nextFrametimer.m_internal = 0;
            m_nextFrametimer.m_totalTime = 0;
            Ctx.m_instance.m_timerMgr.addObject(m_nextFrametimer);
        }

        protected void startOneAniEndTimer()
        {
            if (m_oneAniEndTimer == null)
            {
                m_oneAniEndTimer = new TimerItemBase();
                m_oneAniEndTimer.m_timerDisp = onTimerInitCardHandle;
            }

            AnimatorStateInfo state = m_animator.GetCurrentAnimatorStateInfo(0);
            // 这个地方立马获取数据是获取不到的，需要等待下一帧才能获取到正确的数据
            m_oneAniEndTimer.m_internal = state.length;
            m_oneAniEndTimer.m_totalTime = m_oneAniEndTimer.m_internal;

            Ctx.m_instance.m_timerMgr.addObject(m_oneAniEndTimer);
        }

        public void onNextFrameHandle(TimerItemBase timer)
        {
            startOneAniEndTimer();
        }

        public void onTimerInitCardHandle(TimerItemBase timer)
        {
            m_oneAniPlayEndDisp.dispatchEvent(this);
            // chechParams();
        }

        // 测试获取各种参数
        protected void chechParams()
        {
            // AnimatorStateInfo.length 和 AnimatorClipInfo.clip.length 是一样的
            AnimatorStateInfo state = m_animator.GetCurrentAnimatorStateInfo(0);
            AnimatorClipInfo[] clipArr = m_animator.GetCurrentAnimatorClipInfo(0);
            bool aaa = bInTransition(0);
        }
    }
}
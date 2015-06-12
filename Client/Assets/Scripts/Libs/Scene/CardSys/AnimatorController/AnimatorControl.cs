﻿using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief Unity 最新的动画系统，只使用一个 StateId 控制状态切换， 1 状态是播放状态， 0 状态是停止状态
     */
    public class AnimatorControl : IDispatchObject
    {
        protected Animator m_animator;
        protected int m_stateHashId = 0;
        protected int m_stateValue;
        protected float m_stateDampTime = 0.1f;
        protected EventDispatch m_oneAniPlayEndDisp;    // 一个动画播放结束
        protected FrameTimerItem m_nextFrametimer;       // 需要下一帧才能获取的数据
        protected FrameTimerItem m_idleStateFrametimer;       // 0 状态监测
        protected TimerItemBase m_oneAniEndTimer;       // 一个动画结束定时器
        protected bool m_startPlay;     // 是否直接播放
        //protected AnimatorStateInfo m_state;
        protected bool m_bIdleStateDetect;      // 是否在 Idle State 状态监测中

        public AnimatorControl()
        {
            m_stateHashId = Animator.StringToHash("StateId");
            m_oneAniPlayEndDisp = new AddOnceEventDispatch();
            m_startPlay = false;
            m_bIdleStateDetect = false;
        }

        public void dispose()
        {
            if(m_animator != null)
            {
                UtilApi.Destroy(m_animator.runtimeAnimatorController);
            }

            if(m_nextFrametimer != null)
            {
                Ctx.m_instance.m_frameTimerMgr.delObject(m_nextFrametimer);
                m_nextFrametimer = null;
            }

            if (m_oneAniEndTimer != null)
            {
                Ctx.m_instance.m_timerMgr.delObject(m_oneAniEndTimer);
                m_oneAniEndTimer = null;
            }

            m_oneAniPlayEndDisp.clearEventHandle();
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
                //m_state = m_animator.GetCurrentAnimatorStateInfo(0);
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
            if (m_stateValue == value)
            {
                return;
            }

            if ((m_stateValue != 0 && value != 0) || value == 0)        // 如果两个有时间长度的动画切换状态，或者直接切换到 Idle 状态
            {
                idleStateSetInteger(m_stateHashId, 0);
            }
            else if (0 == m_stateValue)          // 如果当前状态已经是 Idle State 
            {
                if (!m_bIdleStateDetect)          // 如果 Idle State 状态没在监测中
                {
                    normalStateSetInteger(id, value);
                }
            }
        }

        //  Idle State 设置状态
        protected void idleStateSetInteger(int id, int value)
        {
            m_animator.applyRootMotion = true;  // 只有 Idle State 状态下才能自己移动
            m_stateValue = value;           // 保存状态值
            m_animator.SetInteger(m_stateHashId, value);
            startIdleStateFrameTimer();     // 启动 Idle State 监测
        }

        // 非 Idle State 设置状态
        protected void normalStateSetInteger(int id, int value)
        {
            m_animator.applyRootMotion = false;         // 非 Idle State 状态下，有动画控制运动
            m_stateValue = value;
            m_animator.SetInteger(m_stateHashId, value);
            startNextFrameTimer();
        }

        // 启动默认状态定时器
        protected void startIdleStateFrameTimer()
        {
            if (m_idleStateFrametimer == null)
            {
                m_idleStateFrametimer = new FrameTimerItem();
                m_idleStateFrametimer.m_timerDisp = onIdleStateFrameHandle;
                m_idleStateFrametimer.m_internal = 1;
                m_idleStateFrametimer.m_bInfineLoop = true;
            }
            else
            {
                m_idleStateFrametimer.reset();
            }

            m_bIdleStateDetect = true;
            Ctx.m_instance.m_frameTimerMgr.addObject(m_idleStateFrametimer);
        }

        // 启动下一帧定时器
        protected void startNextFrameTimer()
        {
            if (m_nextFrametimer == null)
            {
                m_nextFrametimer = new FrameTimerItem();
                m_nextFrametimer.m_timerDisp = onNextFrameHandle;
                m_nextFrametimer.m_internal = 1;
                m_nextFrametimer.m_bInfineLoop = true;
            }
            else
            {
                m_nextFrametimer.reset();
            }

            Ctx.m_instance.m_frameTimerMgr.addObject(m_nextFrametimer);
        }

        protected void startOneAniEndTimer()
        {
            if (m_oneAniEndTimer == null)
            {
                m_oneAniEndTimer = new TimerItemBase();
                m_oneAniEndTimer.m_timerDisp = onTimerAniEndHandle;
            }
            else
            {
                m_oneAniEndTimer.reset();
            }

            AnimatorStateInfo state = m_animator.GetCurrentAnimatorStateInfo(0);
            // 这个地方立马获取数据是获取不到的，需要等待下一帧才能获取到正确的数据
            Ctx.m_instance.m_logSys.log(string.Format("当前长度 {0}", state.length));
            m_oneAniEndTimer.m_internal = state.length;
            m_oneAniEndTimer.m_totalTime = m_oneAniEndTimer.m_internal;

            Ctx.m_instance.m_timerMgr.addObject(m_oneAniEndTimer);
        }

        // 默认状态监测处理器
        public void onIdleStateFrameHandle(FrameTimerItem timer)
        {
            Ctx.m_instance.m_logSys.log(string.Format("Idle 当前帧 {0}", timer.m_curFrame));
            if (canStopIdleFrameTimer())
            {
                m_bIdleStateDetect = false;
                timer.m_disposed = true;
                if (m_stateValue != 0)
                {
                    normalStateSetInteger(m_stateHashId, m_stateValue);
                }
            }
        }

        public void onNextFrameHandle(FrameTimerItem timer)
        {
            Ctx.m_instance.m_logSys.log(string.Format("当前帧 {0}", timer.m_curFrame));
            if (canStopNextFrameTimer())
            {
                timer.m_disposed = true;
                startOneAniEndTimer();
            }
        }

        // 定时器动画结束处理函数
        public void onTimerAniEndHandle(TimerItemBase timer)
        {
            m_oneAniPlayEndDisp.dispatchEvent(this);
            // chechParams();
        }

        protected bool canStopIdleFrameTimer()
        {
            AnimatorStateInfo state = m_animator.GetCurrentAnimatorStateInfo(0);
            Ctx.m_instance.m_logSys.log(string.Format("Idle 当前检测长度 {0}", state.length));
            return (state.length == 0);
        }

        protected bool canStopNextFrameTimer()
        {
            //return (m_state.length > 0);
            AnimatorStateInfo state = m_animator.GetCurrentAnimatorStateInfo(0);
            Ctx.m_instance.m_logSys.log(string.Format("当前检测长度 {0}", state.length));
            return (state.length > 0);
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
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief Unity 最新的动画系统，只使用一个 StateId 控制状态切换， 1 状态是播放状态， 0 状态是停止状态
     */
    public class AnimatorControl : IDispatchObject
    {
        protected ControllerRes m_controlRes;
        protected bool m_bNeedReload;       // 需要重新加载资源
        protected bool m_selfGoChanged;     // GameObject 对象改变
        protected string m_controlPath;
        protected GameObject m_selfGo;      // 拥有动画控制器的场景 GameObject

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
            m_bNeedReload = false;
            m_controlPath = null;
            m_selfGoChanged = false;
        }

        public void dispose()
        {
            if (m_controlRes != null)
            {
                Ctx.m_instance.m_controllerMgr.unload(m_controlRes.GetPath(), null);
                m_controlRes = null;
            }

            if(m_animator != null)
            {
                UtilApi.Destroy(m_animator.runtimeAnimatorController);
            }

            if(m_nextFrametimer != null)
            {
                Ctx.m_instance.m_frameTimerMgr.removeFrameTimer(m_nextFrametimer);
                m_nextFrametimer = null;
            }
            if (m_idleStateFrametimer != null)
            {
                Ctx.m_instance.m_frameTimerMgr.removeFrameTimer(m_idleStateFrametimer);
                m_idleStateFrametimer = null;
            }
            if (m_oneAniEndTimer != null)
            {
                Ctx.m_instance.m_timerMgr.removeTimer(m_oneAniEndTimer);
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

        public GameObject selfGo
        {
            get
            {
                return m_selfGo;
            }
            set
            {
                if (m_selfGo != value)
                {
                    m_selfGoChanged = true;
                }
                m_selfGo = value;
            }
        }

        public void enable()
        {
            if(m_stateValue != 0 || m_bIdleStateDetect)     // 如果状态值不是 0 ，或者当前在 Idle State 检测中
            {
                m_animator.enabled = true;
            }
        }

        public void disable()
        {
            if (m_animator.enabled)
            {
                m_animator.enabled = false;
            }
        }

        public void setControlInfo(string path)
        {
            if (m_controlPath != path)
            {
                m_controlPath = path;
                m_bNeedReload = true;
            }
        }

        // 同步更新控制器
        public void syncUpdateControl()
        {
            if (m_bNeedReload)
            {
                if (m_controlRes != null)
                {
                    Ctx.m_instance.m_controllerMgr.unload(m_controlRes.GetPath(), null);
                    m_controlRes = null;

                    if (m_animator != null)
                    {
                        UtilApi.Destroy(m_animator.runtimeAnimatorController);
                    }
                }

                m_controlRes = Ctx.m_instance.m_controllerMgr.getAndSyncLoad<ControllerRes>(m_controlPath);
            }
            if (m_selfGoChanged)
            {
                UtilApi.AddAnimatorComponent(m_selfGo);
                m_animator = m_selfGo.GetComponent<Animator>();
            }

            if (m_bNeedReload || m_selfGoChanged)
            {
                m_animator.runtimeAnimatorController = m_controlRes.InstantiateController();

                if(m_stateValue == 0 && canStopIdleFrameTimer())       // 如果当前在 Idle 状态，并且已经完成到 Idle 状态的切换
                {
                    m_animator.enabled = false;
                }
            }

            m_bNeedReload = false;
            m_selfGoChanged = false;
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

        protected void SetInteger(int id, int value)
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

        public void play(int value)
        {
            SetInteger(m_stateHashId, value);
        }

        public void stop()
        {
            SetInteger(m_stateHashId, 0);
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
            m_animator.enabled = true;
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
            Ctx.m_instance.m_frameTimerMgr.addFrameTimer(m_idleStateFrametimer);
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

            Ctx.m_instance.m_frameTimerMgr.addFrameTimer(m_nextFrametimer);
        }

        protected void startOneAniEndTimer()
        {
            if (m_oneAniEndTimer == null)
            {
                m_oneAniEndTimer = new TimerItemBase();
                m_oneAniEndTimer.m_timerDisp.setFuncObject(onTimerAniEndHandle);
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

            Ctx.m_instance.m_timerMgr.addTimer(m_oneAniEndTimer);
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
                else
                {
                    m_animator.enabled = false;         // 切换到空闲状态的时候，关闭，这样才能缩放
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
            //return (state.length == 0);
            return state.normalizedTime >= 1.0f;    // Unity4 使用这个判断动画是否结束， Unity5 可以和 UE4 一样，使用事件
        }

        protected bool canStopNextFrameTimer()
        {
            //return (m_state.length > 0);
            AnimatorStateInfo state = m_animator.GetCurrentAnimatorStateInfo(0);
            Ctx.m_instance.m_logSys.log(string.Format("当前检测长度 {0}", state.length));
            //return (state.length > 0);
            return state.normalizedTime >= 1.0f;
        }

        // 测试获取各种参数
        protected void chechParams()
        {
            // AnimatorStateInfo.length 和 AnimatorClipInfo.clip.length 是一样的
            AnimatorStateInfo state = m_animator.GetCurrentAnimatorStateInfo(0);
            AnimatorClipInfo[] clipArr = m_animator.GetCurrentAnimatorClipInfo(0);
            bool aaa = bInTransition(0);
        }

        /**
         * @brief 给一个 Animator 中的 AnimationClip 添加事件，这个可以直接在编辑器 DopeSheet 或者直接在动画资源中直接添加
         */
        public void AddEvent(string clipName, string funcName, float time)
        {
            if(m_animator == null)
            {
                return;
            }

            AnimationClip[] animClip = m_animator.runtimeAnimatorController.animationClips;
            if(animClip.Length == 0)
            {
                return;
            }

            AnimationEvent animEvt = new AnimationEvent();
            animEvt.functionName = "AnimEventCall";
            animEvt.stringParameter = funcName;
            animEvt.time = time;

            for(int idx = 0; idx < animClip.Length; ++idx)
            {
                if(animClip[idx].name.Equals(clipName))
                {
                    animClip[idx].AddEvent(animEvt);
                    break;
                }
            }
        }

        /**
         * @breif 事件回调函数
         */
        protected void AnimEventCall(string funcName)
        {
            // 执行事件的一些出来
        }

        // 倒着播放动画
        protected void playFromBackToFront()
        {
            AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
            //m_animator.SetTime
            m_animator.speed = -1.0f;
        }
    }
}
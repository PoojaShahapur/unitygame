using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief Unity 最新的动画系统，只使用一个 StateId 控制状态切换， 1 状态是播放状态， 0 状态是停止状态
     */
    public class AnimatorControl : IDispatchObject
    {
        protected ControllerRes mControlRes;
        protected bool mIsNeedReload;       // 需要重新加载资源
        protected bool mSelfGoChanged;     // GameObject 对象改变
        protected string mControlPath;
        protected GameObject mSelfGo;      // 拥有动画控制器的场景 GameObject

        protected Animator mAnimator;
        protected int mStateHashId = 0;
        protected int mStateValue;
        protected float mStateDampTime = 0.1f;

        protected EventDispatch mOneAniPlayEndDisp;    // 一个动画播放结束
        protected FrameTimerItem mNextFrametimer;       // 需要下一帧才能获取的数据
        protected FrameTimerItem mIdleStateFrametimer;       // 0 状态监测
        protected TimerItemBase mOneAniEndTimer;       // 一个动画结束定时器

        protected bool mStartPlay;     // 是否直接播放
        //protected AnimatorStateInfo mState;
        protected bool mIsIdleStateDetect;      // 是否在 Idle State 状态监测中

        public AnimatorControl()
        {
            this.mStateHashId = Animator.StringToHash("StateId");
            this.mOneAniPlayEndDisp = new AddOnceEventDispatch();
            this.mStartPlay = false;
            this.mIsIdleStateDetect = false;
            this.mIsNeedReload = false;
            this.mControlPath = null;
            this.mSelfGoChanged = false;
        }

        public void dispose()
        {
            if (this.mControlRes != null)
            {
                Ctx.mInstance.mControllerMgr.unload(this.mControlRes.getResUniqueId(), null);
                this.mControlRes = null;
            }

            if(this.mAnimator != null)
            {
                UtilApi.Destroy(this.mAnimator.runtimeAnimatorController);
            }

            if(this.mNextFrametimer != null)
            {
                Ctx.mInstance.mFrameTimerMgr.removeFrameTimer(this.mNextFrametimer);
                this.mNextFrametimer = null;
            }
            if (this.mIdleStateFrametimer != null)
            {
                Ctx.mInstance.mFrameTimerMgr.removeFrameTimer(this.mIdleStateFrametimer);
                this.mIdleStateFrametimer = null;
            }
            if (this.mOneAniEndTimer != null)
            {
                this.mOneAniEndTimer.stopTimer();
                this.mOneAniEndTimer = null;
            }

            this.mOneAniPlayEndDisp.clearEventHandle();
        }

        public Animator animator
        {
            get
            {
                return this.mAnimator;
            }
            set
            {
                this.mAnimator = value;
                //mState = m_animator.GetCurrentAnimatorStateInfo(0);
            }
        }

        public EventDispatch oneAniPlayEndDisp
        {
            get
            {
                return this.mOneAniPlayEndDisp;
            }
            set
            {
                this.mOneAniPlayEndDisp = value;
            }
        }

        public GameObject selfGo
        {
            get
            {
                return this.mSelfGo;
            }
            set
            {
                if (this.mSelfGo != value)
                {
                    this.mSelfGoChanged = true;
                }
                this.mSelfGo = value;
            }
        }

        public void enable()
        {
            if(this.mStateValue != 0 || this.mIsIdleStateDetect)     // 如果状态值不是 0 ，或者当前在 Idle State 检测中
            {
                this.mAnimator.enabled = true;
            }
        }

        public void disable()
        {
            if (this.mAnimator.enabled)
            {
                this.mAnimator.enabled = false;
            }
        }

        public void setControlInfo(string path)
        {
            if (this.mControlPath != path)
            {
                this.mControlPath = path;
                this.mIsNeedReload = true;
            }
        }

        // 同步更新控制器
        public void syncUpdateControl()
        {
            if (this.mIsNeedReload)
            {
                if (this.mControlRes != null)
                {
                    Ctx.mInstance.mControllerMgr.unload(this.mControlRes.getResUniqueId(), null);
                    this.mControlRes = null;

                    if (this.mAnimator != null)
                    {
                        UtilApi.Destroy(this.mAnimator.runtimeAnimatorController);
                    }
                }

                this.mControlRes = Ctx.mInstance.mControllerMgr.getAndSyncLoad<ControllerRes>(this.mControlPath);
            }
            if (this.mSelfGoChanged)
            {
                UtilApi.AddAnimatorComponent(this.mSelfGo);
                this.mAnimator = this.mSelfGo.GetComponent<Animator>();
            }

            if (this.mIsNeedReload || this.mSelfGoChanged)
            {
                this.mAnimator.runtimeAnimatorController = this.mControlRes.InstantiateController();

                if(this.mStateValue == 0 && canStopIdleFrameTimer())       // 如果当前在 Idle 状态，并且已经完成到 Idle 状态的切换
                {
                    this.mAnimator.enabled = false;
                }
            }

            this.mIsNeedReload = false;
            this.mSelfGoChanged = false;
        }

        // 当前是否处于某个动画
        public bool bInAnimator(string aniName, int layerIdx)
        {
            AnimatorStateInfo state = this.mAnimator.GetCurrentAnimatorStateInfo(layerIdx);
            return state.IsName(aniName);
        }

        // 当前是否在 Transition 
        public bool bInTransition(int layerIdx)
        {
            bool inTransition = this.mAnimator.IsInTransition(0);
            return inTransition;
        }

        protected void SetInteger(int id, int value)
        {
            if (this.mStateValue == value)
            {
                return;
            }

            if ((this.mStateValue != 0 && value != 0) || value == 0)        // 如果两个有时间长度的动画切换状态，或者直接切换到 Idle 状态
            {
                idleStateSetInteger(this.mStateHashId, 0);
            }
            else if (0 == this.mStateValue)          // 如果当前状态已经是 Idle State 
            {
                if (!this.mIsIdleStateDetect)          // 如果 Idle State 状态没在监测中
                {
                    normalStateSetInteger(id, value);
                }
            }
        }

        public void play(int value)
        {
            SetInteger(this.mStateHashId, value);
        }

        public void stop()
        {
            SetInteger(this.mStateHashId, 0);
        }

        //  Idle State 设置状态
        protected void idleStateSetInteger(int id, int value)
        {
            this.mAnimator.applyRootMotion = true;  // 只有 Idle State 状态下才能自己移动
            this.mStateValue = value;           // 保存状态值
            this.mAnimator.SetInteger(this.mStateHashId, value);
            startIdleStateFrameTimer();     // 启动 Idle State 监测
        }

        // 非 Idle State 设置状态
        protected void normalStateSetInteger(int id, int value)
        {
            this.mAnimator.enabled = true;
            this.mAnimator.applyRootMotion = false;         // 非 Idle State 状态下，有动画控制运动
            this.mStateValue = value;
            this.mAnimator.SetInteger(this.mStateHashId, value);
            startNextFrameTimer();
        }

        // 启动默认状态定时器
        protected void startIdleStateFrameTimer()
        {
            if (this.mIdleStateFrametimer == null)
            {
                this.mIdleStateFrametimer = new FrameTimerItem();
                this.mIdleStateFrametimer.mTimerDisp = onIdleStateFrameHandle;
                this.mIdleStateFrametimer.mInternal = 1;
                this.mIdleStateFrametimer.mIsInfineLoop = true;
            }
            else
            {
                this.mIdleStateFrametimer.reset();
            }

            this.mIsIdleStateDetect = true;
            Ctx.mInstance.mFrameTimerMgr.addFrameTimer(this.mIdleStateFrametimer);
        }

        // 启动下一帧定时器
        protected void startNextFrameTimer()
        {
            if (this.mNextFrametimer == null)
            {
                this.mNextFrametimer = new FrameTimerItem();
                this.mNextFrametimer.mTimerDisp = onNextFrameHandle;
                this.mNextFrametimer.mInternal = 1;
                this.mNextFrametimer.mIsInfineLoop = true;
            }
            else
            {
                this.mNextFrametimer.reset();
            }

            Ctx.mInstance.mFrameTimerMgr.addFrameTimer(this.mNextFrametimer);
        }

        protected void startOneAniEndTimer()
        {
            if (this.mOneAniEndTimer == null)
            {
                this.mOneAniEndTimer = new TimerItemBase();
                this.mOneAniEndTimer.mTimerDisp.setFuncObject(onTimerAniEndHandle);
            }
            else
            {
                this.mOneAniEndTimer.reset();
            }

            AnimatorStateInfo state = this.mAnimator.GetCurrentAnimatorStateInfo(0);
            // 这个地方立马获取数据是获取不到的，需要等待下一帧才能获取到正确的数据
            //Ctx.mInstance.mLogSys.log(string.Format("Current length {0}", state.length));
            this.mOneAniEndTimer.mInternal = state.length;
            this.mOneAniEndTimer.mTotalTime = mOneAniEndTimer.mInternal;

            this.mOneAniEndTimer.startTimer();
        }

        // 默认状态监测处理器
        public void onIdleStateFrameHandle(FrameTimerItem timer)
        {
            //Ctx.mInstance.mLogSys.log(string.Format("Idle current frame {0}", timer.mCurFrame));
            if (canStopIdleFrameTimer())
            {
                this.mIsIdleStateDetect = false;
                timer.mDisposed = true;
                if (this.mStateValue != 0)
                {
                    normalStateSetInteger(this.mStateHashId, this.mStateValue);
                }
                else
                {
                    this.mAnimator.enabled = false;         // 切换到空闲状态的时候，关闭，这样才能缩放
                }
            }
        }

        public void onNextFrameHandle(FrameTimerItem timer)
        {
            //Ctx.mInstance.mLogSys.log(string.Format("Current frame {0}", timer.mCurFrame));
            if (canStopNextFrameTimer())
            {
                timer.mDisposed = true;
                startOneAniEndTimer();
            }
        }

        // 定时器动画结束处理函数
        public void onTimerAniEndHandle(TimerItemBase timer)
        {
            this.mOneAniPlayEndDisp.dispatchEvent(this);
            // chechParams();
        }

        protected bool canStopIdleFrameTimer()
        {
            AnimatorStateInfo state = this.mAnimator.GetCurrentAnimatorStateInfo(0);
            //Ctx.mInstance.mLogSys.log(string.Format("Idle current check length {0}", state.length));
            //return (state.length == 0);
            return state.normalizedTime >= 1.0f;    // Unity4 使用这个判断动画是否结束， Unity5 可以和 UE4 一样，使用事件
        }

        protected bool canStopNextFrameTimer()
        {
            //return (mState.length > 0);
            AnimatorStateInfo state = this.mAnimator.GetCurrentAnimatorStateInfo(0);
            //Ctx.mInstance.mLogSys.log(string.Format("Current check length {0}", state.length));
            //return (state.length > 0);
            return state.normalizedTime >= 1.0f;
        }

        // 测试获取各种参数
        protected void chechParams()
        {
            // AnimatorStateInfo.length 和 AnimatorClipInfo.clip.length 是一样的
            AnimatorStateInfo state = this.mAnimator.GetCurrentAnimatorStateInfo(0);
            AnimatorClipInfo[] clipArr = this.mAnimator.GetCurrentAnimatorClipInfo(0);
            bool aaa = bInTransition(0);
        }

        /**
         * @brief 给一个 Animator 中的 AnimationClip 添加事件，这个可以直接在编辑器 DopeSheet 或者直接在动画资源中直接添加
         */
        public void AddEvent(string clipName, string funcName, float time)
        {
            if(this.mAnimator == null)
            {
                return;
            }

            AnimationClip[] animClip = this.mAnimator.runtimeAnimatorController.animationClips;
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
            AnimatorStateInfo stateInfo = this.mAnimator.GetCurrentAnimatorStateInfo(0);
            // 这个设置成 -1 没什么用处，需要把 Animator 编辑器中的动画节点的 Speed 属性改成 -1，默认是 1
            this.mAnimator.speed = -1.0f;
        }
    }
}
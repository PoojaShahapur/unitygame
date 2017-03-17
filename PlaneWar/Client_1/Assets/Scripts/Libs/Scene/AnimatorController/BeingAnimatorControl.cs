using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 生物动画控制器
     */
    public class BeingAnimatorControl
    {
        protected BeingEntity mEntity;
        protected Animator mAnimator;
        protected int mStateHashId;

        public BeingAnimatorControl(BeingEntity entity)
        {
            this.mStateHashId = Animator.StringToHash("StateId");
            this.mEntity = entity;
        }

        public void init()
        {

        }

        public void dispose()
        {
            //if (this.mAnimator != null)
            //{
            //    UtilApi.Destroy(this.mAnimator.runtimeAnimatorController);
            //}
        }

        public Animator getAnimator()
        {
            return this.mAnimator;
        }

        public void setAnimator(Animator value)
        {
            this.mAnimator = value;

            this.addPlayerEvent();
        }

        // 添加 Player 动画事件
        public void addPlayerEvent()
        {
            this.AddEvent(CVAnimState.AttackStr, CVAnimState.AttackStr);
            this.AddEvent(CVAnimState.SplitStr, CVAnimState.SplitStr);
        }

        public void enable()
        {
            if (!this.mAnimator.enabled)
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

        // 当前是否处于某个动画
        public bool isInAnimator(string aniName, int layerIdx)
        {
            AnimatorStateInfo state = this.mAnimator.GetCurrentAnimatorStateInfo(layerIdx);
            return state.IsName(aniName);
        }

        // 当前是否在 Transition 
        public bool isInTransition(int layerIdx)
        {
            bool inTransition = this.mAnimator.IsInTransition(0);
            return inTransition;
        }

        protected void SetInteger(int id, int value)
        {
            if (null != this.mAnimator)
            {
                this.mAnimator.SetInteger(id, value);
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

        /**
         * @brief 给一个 Animator 中的 AnimationClip 添加事件，这个可以直接在编辑器 DopeSheet 或者直接在动画资源中直接添加
         */
        public void AddEvent(string clipName, string stringParameter)
        {
            if (this.mAnimator == null)
            {
                return;
            }

            AuxAnimUserData animData = UtilApi.AddComponent<AuxAnimUserData>(this.mAnimator.gameObject);
            animData.setBeingEntity(this.mEntity);

            AnimationClip[] animClip = this.mAnimator.runtimeAnimatorController.animationClips;
            if (animClip.Length == 0)
            {
                return;
            }

            AnimationEvent animEvt = new AnimationEvent();
            animEvt.functionName = "AnimEventCall";
            // animEvt.intParameter = 12345;            // 给整型参数
            // animEvt.objectReferenceParameter = act as Object;
            animEvt.stringParameter = stringParameter;          // 回调的时候，会作为参数回传回来
            animEvt.messageOptions = SendMessageOptions.RequireReceiver;

            for (int idx = 0; idx < animClip.Length; ++idx)
            {
                if (animClip[idx].name.Equals(clipName))
                {
                    animEvt.time = animClip[idx].length;
                    animClip[idx].AddEvent(animEvt);
                    break;
                }
            }
        }

        /**
         * @breif 事件回调函数，需要添加到 
         */
        protected void AnimEventCall(string funcName)
        {
            // 执行事件
        }
    }
}
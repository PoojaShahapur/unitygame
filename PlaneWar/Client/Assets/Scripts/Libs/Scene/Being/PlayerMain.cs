namespace SDK.Lib
{
    /**
	 * @brief 主角
	 */
    public class PlayerMain : Player
	{
        public MutilRigidCalcPolicy mMutilRigidCalcPolicy;

        public PlayerMain()
		{
            this.mTypeId = "PlayerMain";
            this.mEntityType = EntityType.ePlayerMain;
            this.mEntityUniqueId = Ctx.mInstance.mPlayerMgr.genNewStrId();
            this.mMovement = new PlayerMainMovement(this);
            this.mAttack = new PlayerMainAttack(this);
            this.mPlayerSplitMerge = new PlayerMainSplitMerge(this);

            this.mMutilRigidCalcPolicy = new MutilRigidCalcPolicy();
        }

        override public void initRender()
        {
            mRender = new PlayerMainRender(this);
            mRender.init();
            //this.mRender = null;        // 不需要渲染器
        }

        protected override void onPreInit()
        {
            base.onPreInit();

            this.hide();    // PlayerMain 不显示，仅仅记录数据

            this.mMovement.init();
            this.mAttack.init();
            this.mPlayerSplitMerge.init();

            //Ctx.mInstance.mGlobalDelegate.mMainChildMassChangedDispatch.addEventHandle(null, this.onChildMassChanged);
            this.mMutilRigidCalcPolicy.init();
        }

        protected override void onPostInit()
        {
            base.onPostInit();

            //this.mPlayerSplitMerge.startSplit();            
        }

        override public void dispose()
        {
            base.dispose();

            this.mMutilRigidCalcPolicy.dispose();
            Ctx.mInstance.mPlayerMgr.removeHero();
        }

        override public void autoHandle()
        {
            base.autoHandle();

            Ctx.mInstance.mPlayerMgr.addHero(this);
        }

        protected override void onPreTick(float delta)
        {
            base.onPreTick(delta);
        }

        protected override void onPostTick(float delta)
        {
            base.onPostTick(delta);

            this.mMutilRigidCalcPolicy.onTick(delta);
        }

        public void emitSnowBlock()
        {
            this.mPlayerSplitMerge.emitSnowBlock();
        }

        override public void setName(string name)
        {
            base.setName(name);

            this.mPlayerSplitMerge.setName();
        }

        public void onChildChanged()
        {
            bool isChange = this.mPlayerSplitMerge.updateCenterPos();
            if(isChange) Ctx.mInstance.mGlobalDelegate.mMainChildChangedDispatch.dispatchEvent(this);
        }

        // Child 质量发生变化
        public void onChildMassChanged(IDispatchObject disp)
        {
            float totalRadius = this.mPlayerSplitMerge.getAllChildMass();
        }

        public void moveForwardByOrient(UnityEngine.Vector2 orient)
        {
            Ctx.mInstance.mCommonData.setIsMoveToCenter(false);

            UnityEngine.Vector3 dir = UtilApi.convPosByMode(new UnityEngine.Vector3(orient.x, 0, orient.y));
            UnityEngine.Quaternion quad = UtilMath.getRotateByOrient(dir) * (this.mMovement as PlayerMainMovement).getForwardRotate();

            this.setDestRotate(quad.eulerAngles, true);
            (this.mMovement as PlayerMainMovement).moveForward();
        }

        // 摇杆按下，但是不移动
        public void onPressButNoMove()
        {
            if (BeingState.eBSMoveCenter != this.mBeingState)
            {
                Ctx.mInstance.mCommonData.setIsMoveToCenter(true);

                this.stopMove();
                this.mPlayerSplitMerge.moveToCenter();
            }
        }

        public void stopMove()
        {
            (this.mMovement as PlayerMainMovement).stopMove();
        }

        public bool isChildEnableRigidByThisId(uint thisId)
        {
            PlayerMainChild child = this.mPlayerSplitMerge.mPlayerChildMgr.getEntityByThisId(thisId) as PlayerMainChild;
            return this.mMutilRigidCalcPolicy.isChildEnableRigidByThisId(child);
        }
    }
}
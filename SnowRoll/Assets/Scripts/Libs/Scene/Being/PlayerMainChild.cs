namespace SDK.Lib
{
    public class PlayerMainChild : PlayerChild
    {
        protected uint mLastMergedTime;    // 最后一次融合时间

        public PlayerMainChild(Player parentPlayer)
            : base(parentPlayer)
        {
            this.mTypeId = "PlayerMainChild";
            this.mEntityType = EntityType.ePlayerMainChild;
            this.mMovement = new PlayerMainChildMovement(this);
            this.mAttack = new PlayerMainChildAttack(this);
            this.mEntityUniqueId = Ctx.mInstance.mPlayerMgr.genChildNewStrId();

            this.setLastMergedTime();
        }

        override public void initRender()
        {
            mRender = new PlayerMainChildRender(this);
            mRender.init();
        }

        public override void preInit()
        {
            base.preInit();

            this.mMovement.init();
            this.mAttack.init();
        }

        public void postUpdate()
        {
            if(this.mRender.isValid())
            {
                this.setRenderPos(this.mRender.getPos());
            }
        }

        override public void setBallRadius(float size, bool immScale = false, bool isCalcMass = false)
        {
            float curRadius = this.mBallRadius;

            base.setBallRadius(size, immScale, isCalcMass);

            if(0 != size && curRadius != this.mBallRadius && !UtilMath.isInvalidNum(size))
            {
                Ctx.mInstance.mGlobalDelegate.mMainChildMassChangedDispatch.dispatchEvent(this);
            }
        }

        override public void setBeingState(BeingState state)
        {
            base.setBeingState(state);

            if(BeingState.eBSBirth == this.mBeingState)
            {
                this.mMoveSpeedFactor = 5;
            }
            else
            {
                this.mMoveSpeedFactor = 1;
            }
        }

        public uint getLastMergedTime()
        {
            return this.mLastMergedTime;
        }

        // 自己当前是否在分裂目标点的后面
        public bool isBehindTargetPoint()
        {
            if (UtilMath.isABehindC(this.getPos(), this.mParentPlayer.getPos(), this.mParentPlayer.mPlayerSplitMerge.getTargetPoint()))
            {
                return true;
            }

            return false;
        }

        // 是否可以执行合并操作，能否合并只有一个冷却时间条件
        override public bool canMerge()
        {
            return UtilLogic.canMerge(this.mLastMergedTime);
        }

        override public bool canMergeWithOther(BeingEntity other)
        {
            bool ret = false;

            if(EntityType.ePlayerMainChild == other.getEntityType())
            {
                float bigRadius = 0;

                // 判断半径
                if (this.mBallRadius > other.getBallRadius())
                {
                    bigRadius = this.mBallRadius;
                }
                else
                {
                    bigRadius = other.getBallRadius();
                }

                // 判断中心点距离
                if (UtilMath.squaredDistance(this.mPos, other.getPos()) <= bigRadius * bigRadius)
                {
                    ret = true;
                }
            }

            return ret;
        }

        override public void mergeWithOther(BeingEntity bBeingEntity)
        {
            PlayerMainChild aChild = this;
            PlayerMainChild bChild = bBeingEntity as PlayerMainChild;

            if (aChild.getBallRadius() > bChild.getBallRadius())
            {
                aChild.setBallRadius(UtilMath.getNewRadiusByRadius(aChild.getBallRadius(), bChild.getBallRadius()));
                bChild.dispose();
            }
            else
            {
                bChild.setBallRadius(UtilMath.getNewRadiusByRadius(bChild.getBallRadius(), aChild.getBallRadius()));
                aChild.dispose();
            }
        }

        override public void addParentOrientChangedhandle()
        {
            (this.mMovement as PlayerMainChildMovement).addParentOrientChangedhandle();
        }

        override public void setLastMergedTime()
        {
            this.mLastMergedTime = UtilApi.getUTCSec();
        }

        // 接触跟随，但是不能融合
        override public void contactWithAndFollowButNotMerge(BeingEntity bBeing)
        {
            PlayerMainChild otherChild = bBeing as PlayerMainChild;
            if(null != otherChild)
            {
                UnityEngine.Quaternion quad = otherChild.getDestRotate();
                this.setNotMergeRotate(quad);
            }
        }
    }
}
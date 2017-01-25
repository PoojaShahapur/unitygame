namespace SDK.Lib
{
	/**
	 * @brief 玩家管理器
	 */
    public class PlayerMgr : EntityMgrBase
	{
        protected PlayerMain mHero;
        protected UniqueStrIdGen mChildUniqueStrIdGen;

        public PlayerTarget mPlayerTarget;
        protected TimerItemBase mRepeatTimer;

        protected int mCurNum;
        protected int mMaxNum;

        protected bool mIsEmitSnowBall; // 吐雪块
        protected float mEmitTimeStamp;  // 吐雪块时间戳

        protected UnityEngine.Vector2 MoveVec; //移动方向

        //protected MDictionary<uint, uint> mSnowId2MainChildIdDic;   // 吃的雪块 Id 到 PlayerMainChild Id 映射
        //protected MDictionary<uint, uint> mOtherChildId2MainChildIdDic;
        //protected MDictionary<uint, uint> mPlayerSnowId2MainChildIdDic;

        public PlayerMgr()
		{
            this.mUniqueStrIdGen = new UniqueStrIdGen(UniqueStrIdGen.PlayerPrefix, 0);
            this.mChildUniqueStrIdGen = new UniqueStrIdGen(UniqueStrIdGen.PlayerChildPrefix, 0);

            this.mCurNum = 0;
            this.mMaxNum = 10;
            this.mIsEmitSnowBall = false;
            this.mEmitTimeStamp = 0;
            this.MoveVec = UnityEngine.Vector2.zero;

            //this.mSnowId2MainChildIdDic = new MDictionary<uint, uint>();
            //this.mOtherChildId2MainChildIdDic = new MDictionary<uint, uint>();
            //this.mPlayerSnowId2MainChildIdDic = new MDictionary<uint, uint>();
        }

        public void setMoveVec(UnityEngine.Vector2 value)
        {
            this.MoveVec = value;
        }

        public UnityEngine.Vector2 getMoveVec()
        {
            return this.MoveVec;
        }

        override protected void onTickExec(float delta)
        {
            base.onTickExec(delta);
            // 检查是否发送移动消息
            if (Ctx.mInstance.mCommonData.isClickSplit())
            {
                Game.Game.ReqSceneInteractive.checkChildAndSendPlayerMove();
            }
            this.emitSnowBlock(delta);
        }

        public void postUpdate()
        {
            if (null != this.mHero)
            {
                this.mHero.mPlayerSplitMerge.mPlayerChildMgr.postUpdate();
            }
        }

        public PlayerMain createHero()
        {
            return new PlayerMain();
        }

        public void addHero(PlayerMain hero)
        {
            this.mHero = hero as PlayerMain;
            this.addPlayer(this.mHero);
        }

        public void removeHero()
        {
            this.removePlayer(this.mHero);
            this.mHero = null;
        }

        public PlayerMain getHero()
        {
            return mHero;
        }

        override public void init()
        {
            base.init();
            //this.startCreatOtherTimer();
        }

        public void addPlayer(Player player)
        {
            this.addEntity(player);
        }

        public void removePlayer(Player player)
        {
            this.removeEntity(player);
            --mMaxNum;
        }

        public string genChildNewStrId()
        {
            return mChildUniqueStrIdGen.genNewStrId();
        }

        public void createPlayerMain()
        {
            mHero = new PlayerMain();
            mHero.init();
            mHero.setDestPos(new UnityEngine.Vector3(50, 1.3f, 50f), true);
            mHero.setDestRotate(UnityEngine.Quaternion.identity.eulerAngles, true);
        }

        // 进行分裂
        public void startSplit()
        {
            //if(null != this.mHero)
            //{
            //    this.mHero.mPlayerSplitMerge.startSplit();
            //}

            Ctx.mInstance.mCommonData.setClickSplit(true);
            Game.Game.ReqSceneInteractive.sendSplit();
        }

        public void setPlayerTargetPos(UnityEngine.Vector3 pos)
        {
            if(null == this.mPlayerTarget)
            {
                this.mPlayerTarget = new PlayerTarget();
                this.mPlayerTarget.init();
            }

            this.mPlayerTarget.setPos(pos);
        }

        public void startEmitSnowBlock()
        {
            this.mIsEmitSnowBall = true;
            Game.Game.ReqSceneInteractive.sendShit();
        }

        // 吐雪球
        public void emitSnowBlock(float delta)
        {
            //if (null != this.mHero)
            //{
            //    this.mHero.emitSnowBlock();
            //}
            if (this.mIsEmitSnowBall)
            {
                this.mEmitTimeStamp = this.mEmitTimeStamp + delta;

                if (this.mEmitTimeStamp>= Ctx.mInstance.mSnowBallCfg.mEmitInterval)
                {
                    Game.Game.ReqSceneInteractive.sendShit();
                    this.mEmitTimeStamp = this.mEmitTimeStamp - Ctx.mInstance.mSnowBallCfg.mEmitInterval;
                }
            }
        }

        public void stopEmitSnowBlock()
        {
            this.mIsEmitSnowBall = false;
            this.mEmitTimeStamp = 0;
        }

        protected void startCreatOtherTimer()
        {
            mRepeatTimer = new TimerItemBase();
            mRepeatTimer.mInternal = 1.0f;
            mRepeatTimer.mIsInfineLoop = true;
            mRepeatTimer.mTimerDisp.setFuncObject(onRepeatTimerTick);
            this.mRepeatTimer.startTimer();
        }

        protected void onRepeatTimerTick(TimerItemBase timer)
        {
            this.createOneOtherPlayer();
        }

        // 创建一个 Other Player
        protected void createOneOtherPlayer()
        {
            if(null != this.mHero && this.mCurNum < this.mMaxNum)
            {
                UnityEngine.Vector3 pos = this.mHero.getPos() +  this.mHero.getRotate() * (UtilMath.UnitCircleRandom() * 10);
                pos.y = 1.3f;

                PlayerOther player = new PlayerOther();
                player.setPos(pos);
                player.init();

                ++this.mCurNum;
            }
        }

        public bool isHeroMoving()
        {
            if (null != this.mHero)
            {
                return (BeingState.eBSWalk == this.mHero.getBeingState() ||
                        BeingState.eBSSeparation == this.mHero.getBeingState() ||
                        BeingState.eBSBirth == this.mHero.getBeingState() ||
                        BeingState.eBSIOControlWalk == this.mHero.getBeingState()
                    );
            }

            return false;
        }

        // 获取 Child
        public PlayerChild getChildByThisId(uint thisId)
        {
            PlayerChild child = null;

            int idx = 0;
            int len = this.getEntityCount();

            while(idx < len)
            {
                child = (this.getEntityByIndex(idx) as Player).mPlayerSplitMerge.mPlayerChildMgr.getEntityByThisId(thisId) as PlayerChild;
                if(null != child)
                {
                    break;
                }

                ++idx;
            }

            return child;
        }

        //public void eatSnowing(uint snowId, uint mainChildId)
        //{
        //    this.mSnowId2MainChildIdDic[snowId] = mainChildId;
        //}

        //public void eatSnowed(uint snowId)
        //{
        //    if (this.mSnowId2MainChildIdDic.ContainsKey(snowId))
        //    {
        //        if (null != this.mHero)
        //        {
        //            PlayerMainChild child = this.mHero.mPlayerSplitMerge.mPlayerChildMgr.getEntityByThisId(this.mSnowId2MainChildIdDic[snowId]) as PlayerMainChild;
        //            if(null != child)
        //            {
        //                //child.setFreezeXZ(false);
        //            }
        //        }

        //        this.mSnowId2MainChildIdDic.Remove(snowId);
        //    }
        //}

        //public void eatOtherChilding(uint snowId, uint mainChildId)
        //{
        //    this.mOtherChildId2MainChildIdDic[snowId] = mainChildId;
        //}

        //public void eatOtherChilded(uint snowId)
        //{
        //    if (this.mOtherChildId2MainChildIdDic.ContainsKey(snowId))
        //    {
        //        if (null != this.mHero)
        //        {
        //            PlayerMainChild child = this.mHero.mPlayerSplitMerge.mPlayerChildMgr.getEntityByThisId(this.mOtherChildId2MainChildIdDic[snowId]) as PlayerMainChild;
        //            if (null != child)
        //            {
        //                //child.setFreezeXZ(false);
        //            }
        //        }

        //        this.mOtherChildId2MainChildIdDic.Remove(snowId);
        //    }
        //}

        //public void eatPlayerSnowing(uint snowId, uint mainChildId)
        //{
        //    this.mPlayerSnowId2MainChildIdDic[snowId] = mainChildId;
        //}

        //public void eatPlayerSnowed(uint snowId)
        //{
        //    if (this.mPlayerSnowId2MainChildIdDic.ContainsKey(snowId))
        //    {
        //        if (null != this.mHero)
        //        {
        //            PlayerMainChild child = this.mHero.mPlayerSplitMerge.mPlayerChildMgr.getEntityByThisId(this.mPlayerSnowId2MainChildIdDic[snowId]) as PlayerMainChild;
        //            if (null != child)
        //            {
        //                //child.setFreezeXZ(false);
        //            }
        //        }

        //        this.mPlayerSnowId2MainChildIdDic.Remove(snowId);
        //    }
        //}

        //public bool isEatSnowOrPlayrOther()
        //{
        //    return this.mOtherChildId2MainChildIdDic.Count() > 0 || this.mPlayerSnowId2MainChildIdDic.Count() > 0;
        //}
    }
}
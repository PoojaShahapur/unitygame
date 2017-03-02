namespace SDK.Lib
{
    public class PlayerMainSplitMerge : PlayerSplitMerge
    {
        protected MList<MergeItem> mMergeList;
        protected MDictionary<string, MergeItem> mMergeDic;
        protected MList<MergeItem> mTmpMergedList;  // 临时存放合并的列表
        protected MList<MergeItem> mTmpMergedDeleteList;
        private MVector3 OldCenterPosition;         // 上一次的中心点
        private float OldMaxCameraLength;           // 上一次的相机拉伸参考值

        public PlayerMainSplitMerge(Player mPlayer)
            : base(mPlayer)
        {
            mMergeList = new MList<MergeItem>();
            mMergeDic = new MDictionary<string, MergeItem>();
            mTmpMergedList = new MList<MergeItem>();
            mTmpMergedDeleteList = new MList<MergeItem>();
            OldCenterPosition = MVector3.ZERO;
            OldMaxCameraLength = 0.0f;
        }

        public override void onTick(float delta)
        {
            base.onTick(delta);
            this.onMergeTick(delta);
        }

        protected void onMergeTick(float delta)
        {
            mTmpMergedList.Clear();
            mTmpMergedDeleteList.Clear();
            int idx = 0;
            //bool isMerged = false;

            while (idx < mMergeList.Count())
            {
                if (mMergeList[idx].isInRange())
                {
                    if (mMergeList[idx].canMerge())
                    {
                        mTmpMergedList.Add(mMergeList[idx]);
                    }
                }
                else
                {
                    mTmpMergedDeleteList.Add(mMergeList[idx]);
                }
                ++idx;
            }

            idx = 0;
            while (idx < mTmpMergedList.Count())
            {
                //isMerged = true;

                mTmpMergedList[idx].merge();
                mMergeList.Remove(mTmpMergedList[idx]);
                this.mMergeDic.Remove(mTmpMergedList[idx].getMergeAId());
                this.mMergeDic.Remove(mTmpMergedList[idx].getMergeBId());

                ++idx;
            }

            idx = 0;
            while (idx < mTmpMergedDeleteList.Count())
            {
                mTmpMergedDeleteList[idx].onExceedRange();

                mMergeList.Remove(mTmpMergedDeleteList[idx]);
                this.mMergeDic.Remove(mTmpMergedDeleteList[idx].getMergeAId());
                this.mMergeDic.Remove(mTmpMergedDeleteList[idx].getMergeBId());

                ++idx;
            }
        }

        override protected void onFirstSplit()
        {
            base.onFirstSplit();

            this.mRangeBox.clear();
            this.mRangeBox.setExtents(this.mEntity.getPos().x, this.mEntity.getPos().y, this.mEntity.getPos().z);

            this.splitOne(null, true);

            this.calcTargetLength();
            this.calcTargetPoint();
        }

        // 每一次分裂确定一次目标点，其它时候不改变目标点
        override protected void onNoFirstSplit()
        {
            // 这个分裂修改列表数据，因此只查找前面的数据
            int idx = 0;
            int num = this.mPlayerChildMgr.getEntityCount();
            PlayerChild player;
            bool ischanged = false;

            while (idx < num && Ctx.mInstance.mSnowBallCfg.isLessMaxNum(this.mPlayerChildMgr.getEntityCount()))
            {
                player = this.mPlayerChildMgr.getEntityByIndex(idx) as PlayerChild;
                if (player.canSplit())
                {
                    if (!ischanged)
                    {
                        ischanged = true;
                        this.mRangeBox.clear();
                    }

                    this.splitOne(player, false);
                }
                else
                {
                    if (MacroDef.ENABLE_LOG)
                    {
                        Ctx.mInstance.mLogSys.log("PlayerMainSplitMerge::onNoFirstSplit, Can not Split", LogTypeId.eLogSplitMergeEmit);
                    }
                }

                ++idx;
            }

            if (ischanged)
            {
                if (Ctx.mInstance.mSnowBallCfg.isGreatEqualMaxNum(this.mPlayerChildMgr.getEntityCount()))
                {
                    if (MacroDef.ENABLE_LOG)
                    {
                        Ctx.mInstance.mLogSys.log("PlayerMainSplitMerge::onNoFirstSplit, Split GreatEqual Max", LogTypeId.eLogSplitMergeEmit);
                    }
                }

                // 设置自己到中心点
                //this.mEntity.setPos(this.mRangeBox.getCenter().toNative());
                //this.calcTargetLength();
                //this.calcTargetPoint();

                (this.mEntity as PlayerMain).onChildChanged();
            }
        }

        protected void splitOne(PlayerChild player, bool isFirst)
        {
            PlayerMainChild child;
            UnityEngine.Vector3 pos;

            child = new PlayerMainChild(this.mEntity);
            child.init();

            UnityEngine.Vector3 initPos;
            float splitRadius = 0;

            if (isFirst)
            {
                initPos = this.mEntity.getPos();
                child.setPos(initPos);
                // 设置 Child 分裂半径
                child.setBallRadius(this.mEntity.getBallRadius());
                (child as BeingEntity).setMoveSpeed((this.mEntity as BeingEntity).getMoveSpeed());

                // TODO:Test
                //child.setBallRadius(30);
            }
            else
            {
                // 设置分裂半径
                splitRadius = (player as BeingEntity).getSplitRadius();
                child.setBallRadius(splitRadius, true);
                player.setBallRadius(splitRadius);

                // 设置位置
                pos = player.getPos();
                this.mRangeBox.setExtents(pos.x, pos.y, pos.z);

                initPos = pos + player.getRotate() * new UnityEngine.Vector3(0, 0, player.getBallRadius() + child.getBallRadius() + Ctx.mInstance.mSnowBallCfg.mSplitRelStartPos);
                //initPos = player.getPos() + UtilMath.UnitCircleRandom();
                //child.setPos(player.getPos());
                child.setPos(initPos);

                initPos = initPos + player.getRotate() * new UnityEngine.Vector3(0, 0, Ctx.mInstance.mSnowBallCfg.mSplitRelDist);
                (child as BeingEntity).setDestPosForBirth(initPos, false);
            }

            // 添加 Child 事件
            (child.mMovement as PlayerMainChildMovement).addParentOrientChangedhandle();
        }

        //override public void addSplitChild(PlayerChild playerChild)
        //{
        //    // 添加 Child 事件
        //    (playerChild.mMovement as PlayerMainChildMovement).addParentOrientChangedhandle();
        //    (this.mEntity as PlayerMain).onChildChanged();
        //}

        override public MergeItem addMerge(PlayerChild aChild, PlayerChild bChild)
        {
            string keyOne;
            string keyTwo;
            MergeItem mergeItem;

            keyOne = aChild.getEntityUniqueId();

            if (!mMergeDic.ContainsKey(keyOne))
            {
                keyTwo = bChild.getEntityUniqueId();

                if (!mMergeDic.ContainsKey(keyTwo))
                {
                    mergeItem = new MergeItem(this.mEntity as PlayerMain);

                    mMergeDic[keyOne] = mergeItem;
                    mMergeDic[keyTwo] = mergeItem;
                    this.mMergeList.Add(mergeItem);

                    aChild.setBeingSubState(BeingSubState.eBSSContactMerge);
                    bChild.setBeingSubState(BeingSubState.eBSSContactMerge);

                    mergeItem.setMergeBeingEntityId(keyOne, keyTwo);
                    mergeItem.setMergeBeingEntityThisId(aChild.getThisId(), bChild.getThisId());
                    mergeItem.adjustTimeStamp();
                    mergeItem.setDistance(aChild, bChild);
                    mergeItem.onAddMerge();

                    if (MacroDef.ENABLE_LOG)
                    {
                        Ctx.mInstance.mLogSys.log(string.Format("PlayerMainSplitMerge::addMerge, add new item, aThisId = {0}, bThisId = {1}", aChild.getThisId(), bChild.getThisId()), LogTypeId.eLogSplitMergeEmit);
                    }
                }
                else
                {
                    mergeItem = mMergeDic[keyTwo];

                    if (MacroDef.ENABLE_LOG)
                    {
                        Ctx.mInstance.mLogSys.log(string.Format("PlayerMainSplitMerge::addMerge, already exist second, aThisId = {0}, bThisId = {1}", aChild.getThisId(), bChild.getThisId()), LogTypeId.eLogSplitMergeEmit);
                    }
                }
            }
            else
            {
                mergeItem = mMergeDic[keyOne];

                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log(string.Format("PlayerMainSplitMerge::addMerge, already exist first, aThisId = {0}, bThisId = {1}", aChild.getThisId(), bChild.getThisId()), LogTypeId.eLogSplitMergeEmit);
                }
            }

            if(MacroDef.ENABLE_LOG)
            {
                Ctx.mInstance.mLogSys.log(string.Format("PlayerMainSplitMerge::addMerge, aThisId = {0}, bThisId = {1}", aChild.getThisId(), bChild.getThisId()), LogTypeId.eLogMergeBug);
            }

            return mergeItem;
        }

        override public void removeMerge(PlayerChild aChild, PlayerChild bChild)
        {
            string keyOne;
            string keyTwo;

            keyOne = aChild.getEntityUniqueId();

            if (mMergeDic.ContainsKey(keyOne))
            {
                this.mMergeList.Remove(mMergeDic[keyOne]);
                this.mMergeDic.Remove(keyOne);

                keyTwo = bChild.getEntityUniqueId();
                this.mMergeDic.Remove(keyTwo);

                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log(string.Format("PlayerMainSplitMerge::removeMerge, two param, exist, aThisId = {0}, bThisId = {1}", aChild.getThisId(), bChild.getThisId()), LogTypeId.eLogSplitMergeEmit);
                }
            }
            else
            {
                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log(string.Format("PlayerMainSplitMerge::removeMerge, two param, not exist, aThisId = {0}, bThisId = {1}", aChild.getThisId(), bChild.getThisId()), LogTypeId.eLogSplitMergeEmit);
                }
            }

            if(null != aChild)
            {
                aChild.setBeingSubState(BeingSubState.eBSSNone);
            }
            if (null != bChild)
            {
                bChild.setBeingSubState(BeingSubState.eBSSNone);
            }

            if (MacroDef.ENABLE_LOG)
            {
                Ctx.mInstance.mLogSys.log(string.Format("PlayerMainSplitMerge::removeMerge, aThisId = {0}, bThisId = {1}", aChild.getThisId(), bChild.getThisId()), LogTypeId.eLogMergeBug);
            }
        }

        override public void removeMerge(PlayerChild aChild)
        {
            string keyOne;
            string keyTwo;
            PlayerMainChild bChild = null;
            keyOne = aChild.getEntityUniqueId();

            if (mMergeDic.ContainsKey(keyOne))
            {
                this.mMergeList.Remove(mMergeDic[keyOne]);
                this.mMergeDic.Remove(keyOne);

                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log(string.Format("PlayerMainSplitMerge::removeMerge, one param, not exist, aThisId = {0}", aChild.getThisId()), LogTypeId.eLogSplitMergeEmit);
                }

                if (aChild.getThisId() == mMergeDic[keyOne].mMergeAThisId)
                {
                    bChild = Ctx.mInstance.mPlayerMgr.getHero().mPlayerSplitMerge.mPlayerChildMgr.getEntityByThisId((uint)mMergeDic[keyOne].mMergeBThisId) as PlayerMainChild;
                }
                else
                {
                    bChild = Ctx.mInstance.mPlayerMgr.getHero().mPlayerSplitMerge.mPlayerChildMgr.getEntityByThisId((uint)mMergeDic[keyOne].mMergeAThisId) as PlayerMainChild;
                }

                if (null != bChild)
                {
                    keyTwo = bChild.getEntityUniqueId();
                    this.mMergeDic.Remove(keyTwo);

                    if (MacroDef.ENABLE_LOG)
                    {
                        Ctx.mInstance.mLogSys.log(string.Format("PlayerMainSplitMerge::removeMerge, one param, not exist, bThisId = {0}", bChild.getThisId()), LogTypeId.eLogSplitMergeEmit);
                    }
                }
            }

            if (null != aChild)
            {
                aChild.setBeingSubState(BeingSubState.eBSSNone);
            }
            if (null != bChild)
            {
                bChild.setBeingSubState(BeingSubState.eBSSNone);
            }
        }

        override public bool isExistMerge(PlayerChild aChild, PlayerChild bChild)
        {
            string keyOne;
            bool ret = false;

            keyOne = aChild.getEntityUniqueId();
            ret = mMergeDic.ContainsKey(keyOne);

            return ret;
        }

        override public void emitSnowBlock()
        {
            PlayerMainChild child;
            // 这个分裂修改列表数据，因此只查找前面的数据
            int idx = 0;
            int num = this.mPlayerChildMgr.getEntityCount();

            UnityEngine.Vector3 startPos;
            UnityEngine.Vector3 endPos;
            float emitRadius = 1;
            bool isEmited = false;

            while (idx < num)
            {
                child = this.mPlayerChildMgr.getEntityByIndex(idx) as PlayerMainChild;
                if (child.canEmitSnow())
                {
                    isEmited = true;

                    emitRadius = child.getEmitSnowSize();
                    child.reduceMassBy(Ctx.mInstance.mSnowBallCfg.mEmitSnowMass);

                    startPos = child.getPos() + child.getRotate() * new UnityEngine.Vector3(0, 0, child.getBallRadius() + emitRadius + Ctx.mInstance.mSnowBallCfg.mEmitRelStartPos);
                    endPos = startPos + child.getRotate() * new UnityEngine.Vector3(0, 0, Ctx.mInstance.mSnowBallCfg.mEmitRelDist);

                    child.setBeingState(BeingState.eBSSplit);
                    Ctx.mInstance.mPlayerSnowBlockMgr.emitOne(startPos, endPos, child.getRotate(), emitRadius);
                }
                else
                {
                    if (MacroDef.ENABLE_LOG)
                    {
                        Ctx.mInstance.mLogSys.log("Can not Emit", LogTypeId.eLogSplitMergeEmit);
                    }
                }

                ++idx;
            }

            if (isEmited)
            {
                (this.mEntity as PlayerMain).onChildChanged();
            }
        }

        override public void setDestPos(UnityEngine.Vector3 pos, bool immePos)
        {
            int total = this.mPlayerChildMgr.getEntityCount();
            int index = 0;
            Player player = null;
            while (index < total)
            {
                player = this.mPlayerChildMgr.getEntityByIndex(index) as Player;
                player.setDestPos(player.getPos() + (this.mEntity as Player).getDeltaPos(), immePos);
                ++index;
            }

            this.mRangeBox.clear();
            this.mRangeBox.setExtents(pos.x, pos.y, pos.z);

            this.calcTargetLength();
            this.calcTargetPoint();
        }

        override public void setName()
        {
            int total = this.mPlayerChildMgr.getEntityCount();
            int index = 0;
            Player player = null;
            while (index < total)
            {
                player = this.mPlayerChildMgr.getEntityByIndex(index) as Player;
                player.setName(this.mEntity.getName());
                ++index;
            }
        }

        // 更新中心点位置
        override public bool updateCenterPos()
        {
            bool isCenterPosChanged = false;
            this.mRangeBox.clear();

            int total = this.mPlayerChildMgr.getEntityCount();
            int index = 0;
            Player player = null;
            UnityEngine.Vector3 pos;

            while (index < total)
            {
                player = this.mPlayerChildMgr.getEntityByIndex(index) as Player;
                pos = player.getPos();
                this.mRangeBox.setExtents(pos.x, pos.y, pos.z);

                ++index;
            }
            
            if (!MVector3.Equals(OldCenterPosition, this.mRangeBox.getCenter()))
            {
                isCenterPosChanged = true;
                this.mEntity.setPos(this.mRangeBox.getCenter().toNative());
                OldCenterPosition = this.mRangeBox.getCenter();
                this.calcTargetLength();
                this.calcTargetPoint();
            }

            bool isLengthChanged = false;
            float curLength = Ctx.mInstance.mPlayerMgr.getHero().mPlayerSplitMerge.getMaxCameraLength();
            if (UtilMath.Abs(OldMaxCameraLength - curLength) > UtilMath.EPSILON)
            {
                OldMaxCameraLength = curLength;
                isLengthChanged = true;
            }
            
            return isCenterPosChanged || isLengthChanged;
        }

        override public float getAllChildMass()
        {
            float totlaMass = 0;
            int total = this.mPlayerChildMgr.getEntityCount();
            int index = 0;
            Player player = null;

            while (index < total)
            {
                player = this.mPlayerChildMgr.getEntityByIndex(index) as Player;
                if (BeingSubState.eBSSMerge != player.getBeingSubState() &&
                    !player.isClientDispose())
                {
                    totlaMass += UtilMath.getMassByRadius(player.getBallRadius());
                }

                ++index;
            }

            return totlaMass;
        }

        override  public bool isCanSplit()
        {
            bool ret = false;
            int total = this.mPlayerChildMgr.getEntityCount();
            int index = 0;
            Player player = null;

            while (index < total)
            {
                player = this.mPlayerChildMgr.getEntityByIndex(index) as Player;
                if (BeingSubState.eBSSMerge != player.getBeingSubState() &&
                    !player.isClientDispose())
                {
                    if (UtilMath.getMassByRadius(player.getBallRadius()) >= Ctx.mInstance.mSnowBallCfg.mCanSplitMass)
                    {
                        ret = true;
                        break;
                    }
                }

                ++index;
            }
            return ret;
        }

        override public bool isCanEmit()
        {
            bool ret = false;
            int total = this.mPlayerChildMgr.getEntityCount();
            int index = 0;
            Player player = null;

            while (index < total)
            {
                player = this.mPlayerChildMgr.getEntityByIndex(index) as Player;
                if (BeingSubState.eBSSMerge != player.getBeingSubState() &&
                    !player.isClientDispose())
                {
                    if (UtilMath.getMassByRadius(player.getBallRadius()) >= Ctx.mInstance.mSnowBallCfg.mCanEmitSnowMass)
                    {
                        ret = true;
                        break;
                    }
                }

                ++index;
            }
            return ret;
        }

        override public void moveToCenter()
        {
            UnityEngine.Vector3 targetPoint;
            targetPoint = this.getCenterPoint();

            int total = this.mPlayerChildMgr.getEntityCount();
            int index = 0;
            Player player = null;

            while (index < total)
            {
                player = this.mPlayerChildMgr.getEntityByIndex(index) as Player;
                player.setDestPosForMoveCenter(targetPoint, false);

                ++index;
            }
        }
    }
}
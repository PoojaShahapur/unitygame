namespace SDK.Lib
{
    public class PlayerMainSplitMerge : PlayerSplitMerge
    {
        protected MList<MergeItem> mMergeList;
        protected System.Collections.Generic.Dictionary<string, MergeItem> mMergeDic;
        protected MList<MergeItem> mTmpMergedList;  // 临时存放合并的列表

        public PlayerMainSplitMerge(Player mPlayer)
            : base(mPlayer)
        {
            mMergeList = new MList<MergeItem>();
            mMergeDic = new System.Collections.Generic.Dictionary<string, MergeItem>();
            mTmpMergedList = new MList<MergeItem>();
        }

        public override void onTick(float delta)
        {
            base.onTick(delta);

            this.onMergeTick(delta);
        }

        protected void onMergeTick(float delta)
        {
            mTmpMergedList.Clear();
            int idx = 0;

            while (idx < mMergeList.Count())
            {
                if (mMergeList[idx].canMerge())
                {
                    mTmpMergedList.Add(mMergeList[idx]);
                }
                ++idx;
            }

            idx = 0;
            while (idx < mTmpMergedList.Count())
            {
                mTmpMergedList[idx].merge();
                mMergeList.Remove(mTmpMergedList[idx]);
                this.mMergeDic.Remove(mTmpMergedList[idx].getMergeAId());
                this.mMergeDic.Remove(mTmpMergedList[idx].getMergeBId());

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
            bool isClear = false;

            while (idx < num && Ctx.mInstance.mSnowBallCfg.isLessMaxNum(this.mPlayerChildMgr.getEntityCount()))
            {
                if(!isClear)
                {
                    isClear = true;
                    this.mRangeBox.clear();
                }

                player = this.mPlayerChildMgr.getEntityByIndex(idx) as PlayerChild;
                if (player.canSplit())
                {
                    this.splitOne(player, false);
                }

                ++idx;
            }

            // 设置自己到中心点
            this.mEntity.setPos(this.mRangeBox.getCenter().toNative());
            this.calcTargetLength();
            this.calcTargetPoint();
        }

        protected void splitOne(PlayerChild player, bool isFirst)
        {
            PlayerMainChild child;
            UnityEngine.Vector3 pos;

            child = new PlayerMainChild(this.mEntity);
            child.init();

            UnityEngine.Vector3 initPos;

            if (isFirst)
            {
                initPos = this.mEntity.getPos();
                child.setPos(initPos);
                // 设置 Child 分裂半径
                child.setEatSize(this.mEntity.getEatSize());
                (child as BeingEntity).setMoveSpeed((this.mEntity as BeingEntity).getMoveSpeed());
            }
            else
            {
                pos = player.getPos();
                this.mRangeBox.setExtents(pos.x, pos.y, pos.z);

                initPos = pos + player.getRotate() * new UnityEngine.Vector3(0, 0, player.getEatSize() + 5);
                //initPos = player.getPos() + UtilMath.UnitCircleRandom();
                //child.setPos(player.getPos());
                child.setPos(initPos);

                initPos = pos + player.getRotate() * new UnityEngine.Vector3(0, 0, player.getEatSize() + 10);
                (child as BeingEntity).setDestPosForBirth(initPos, false);

                child.setEatSize(player.getEatSize());
                (child as BeingEntity).setMoveSpeed(player.getMoveSpeed() * 2.0f);

                // 设置分裂半径
                player.setEatSize(player.getEatSize());
            }

            // 添加 Child 事件
            ((this.mEntity as PlayerMain).mMovement as PlayerMainMovement).addOrientChangedHandle((child.mMovement as PlayerMainChildMovement).handleParentOrientChanged);
            ((this.mEntity as PlayerMain).mMovement as PlayerMainMovement).addPosChangedHandle((child.mMovement as PlayerMainChildMovement).handleParentPosChanged);
            ((this.mEntity as PlayerMain).mMovement as PlayerMainMovement).addOrientStopChangedHandle((child.mMovement as PlayerMainChildMovement).handleParentOrientStopChanged);
            ((this.mEntity as PlayerMain).mMovement as PlayerMainMovement).addPosStopChangedHandle((child.mMovement as PlayerMainChildMovement).handleParentPosStopChanged);
        }

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
                    mergeItem.adjustTimeStamp();
                }
                else
                {
                    mergeItem = mMergeDic[keyTwo];
                }
            }
            else
            {
                mergeItem = mMergeDic[keyOne];
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
            UnityEngine.Vector3 relPos = new UnityEngine.Vector3(0, 0, 5);
            UnityEngine.Vector3 destPos;

            while (idx < num)
            {
                child = this.mPlayerChildMgr.getEntityByIndex(idx) as PlayerMainChild;
                destPos = child.getRotate() * relPos;
                Ctx.mInstance.mPlayerSnowBlockMgr.emitOne(child.getPos(), child.getPos() + destPos, child.getRotate(), child.getEmitSnowSize());

                ++idx;
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
                player.setDestPos(pos, immePos);
                ++index;
            }

            this.mRangeBox.clear();
            this.mRangeBox.setExtents(pos.x, pos.y, pos.z);

            this.calcTargetLength();
            this.calcTargetPoint();
        }
    }
}
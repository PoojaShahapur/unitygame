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

            PlayerMainChild child;
            child = new PlayerMainChild(this.mEntity);
            child.init();
            //UnityEngine.Vector3 initPos = this.mEntity.getPos() + this.mEntity.getRotate() * new UnityEngine.Vector3(0, 0, -this.mEntity.getEatSize());
            UnityEngine.Vector3 initPos = this.mEntity.getPos();
            child.setPos(initPos);
            // 设置 Child 分裂半径
            child.setEatSize(this.mEntity.getEatSize());

            // 自己不设置分裂半径
            //this.mEntity.setEatSize(this.mEntity.getEatSize());

            // 添加 Child 事件
            ((this.mEntity as PlayerMain).mMovement as PlayerMainMovement).addOrientChangedHandle((child.mMovement as PlayerMainChildMovement).handleParentOrientChanged);
            ((this.mEntity as PlayerMain).mMovement as PlayerMainMovement).addPosChangedHandle((child.mMovement as PlayerMainChildMovement).handleParentPosChanged);
            ((this.mEntity as PlayerMain).mMovement as PlayerMainMovement).addOrientStopChangedHandle((child.mMovement as PlayerMainChildMovement).handleParentOrientStopChanged);
            ((this.mEntity as PlayerMain).mMovement as PlayerMainMovement).addPosStopChangedHandle((child.mMovement as PlayerMainChildMovement).handleParentPosStopChanged);

            this.calcTargetLength();
            this.calcTargetPoint();
            //this.updateChildDestDir();
        }

        // 每一次分裂确定一次目标点，其它时候不改变目标点
        override protected void onNoFirstSplit()
        {
            PlayerMainChild child;
            // 这个分裂修改列表数据，因此只查找前面的数据
            int idx = 0;
            int num = this.mPlayerChildMgr.getEntityCount();
            PlayerChild player;
            UnityEngine.Vector3 pos;

            this.mRangeBox.clear();

            while (idx < num)
            {
                //int childIdx = 0;
                //while (childIdx < 5)
                //{
                    player = this.mPlayerChildMgr.getEntityByIndex(idx) as PlayerChild;
                    pos = player.getPos();
                    this.mRangeBox.setExtents(pos.x, pos.y, pos.z);

                    child = new PlayerMainChild(this.mEntity);
                    child.init();

                    UnityEngine.Vector3 initPos = player.getPos() + player.getRotate() * new UnityEngine.Vector3(0, 0, player.getEatSize() + 5);
                    //UnityEngine.Vector3 initPos = player.getPos() + UtilMath.UnitCircleRandom();
                    child.setPos(player.getPos());
                    (child as BeingEntity).setDestPosForBirth(initPos, false);
                    child.setEatSize(player.getEatSize());

                    // 设置分裂半径
                    player.setEatSize(player.getEatSize());

                    // 添加 Child 事件
                    ((this.mEntity as PlayerMain).mMovement as PlayerMainMovement).addOrientChangedHandle((child.mMovement as PlayerMainChildMovement).handleParentOrientChanged);
                    ((this.mEntity as PlayerMain).mMovement as PlayerMainMovement).addPosChangedHandle((child.mMovement as PlayerMainChildMovement).handleParentPosChanged);
                    ((this.mEntity as PlayerMain).mMovement as PlayerMainMovement).addOrientStopChangedHandle((child.mMovement as PlayerMainChildMovement).handleParentOrientStopChanged);
                    ((this.mEntity as PlayerMain).mMovement as PlayerMainMovement).addPosStopChangedHandle((child.mMovement as PlayerMainChildMovement).handleParentPosStopChanged);

                    //++childIdx;
                //}
                ++idx;
            }

            // 设置自己到中心点
            this.mEntity.setPos(this.mRangeBox.getCenter().toNative());
            this.calcTargetLength();
            this.calcTargetPoint();
            //this.updateChildDestDir();
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

                    aChild.setBeingState(BeingState.BSContactMerge);
                    bChild.setBeingState(BeingState.BSContactMerge);

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
    }
}
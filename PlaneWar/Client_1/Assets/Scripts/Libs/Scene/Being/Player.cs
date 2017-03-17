namespace SDK.Lib
{
	/**
	 * @brief 场景中的玩家
	 */
	public class Player : BeingEntity
	{
        public PlayerSplitMerge mPlayerSplitMerge;

        public UniqueNumIdGen mUniqueNumIdGen;      // 子弹唯一 Id 生成
        public UniqueNumIdGen mBulletIDGentor;

        // 位置改变量，主要是暂时移动 child，以后改通知为服务器 child 位置，就不用这样修改了
        protected UnityEngine.Vector3 mDeltaPos;

        private bool mCanMove = true;
        public int mPlaneIndex = -1;//皮肤ID

        public Player()
		{
            //mSkinAniModel.m_modelList = new SkinSubModel[(int)ePlayerModelType.eModelTotal];
            //int idx = 0;
            //while (idx < (int)ePlayerModelType.eModelTotal)
            //{
            //    mSkinAniModel.m_modelList[idx] = new SkinSubModel();
            //    ++idx;
            //}

            this.mUniqueNumIdGen = new UniqueNumIdGen(0);
            this.mBulletIDGentor = new UniqueNumIdGen(0);
        }

        public void setCanMove(bool _canmove)
        {
            mCanMove = _canmove;
        }

        public bool getCanMove()
        {
            return mCanMove;
        }

        override protected void onPreInit()
        {
            base.onPreInit();

            if(null != this.mAnimFSM)
            {
                this.mAnimFSM.init();
            }
            if(null != this.mAnimatorControl)
            {
                this.mAnimatorControl.init();
            }
            if (null != this.mPlayerSplitMerge)
            {
                this.mPlayerSplitMerge.init();
            }
        }

        override public void dispose()
        {
            base.dispose();
            //Ctx.mInstance.mPlayerMgr.removeEntity(this);
        }

        override public void onDestroy()
        {
            if (null != this.mPlayerSplitMerge)
            {
                this.mPlayerSplitMerge.dispose();
                this.mPlayerSplitMerge = null;
            }

            base.onDestroy();
        }

        override public void autoHandle()
        {
            base.autoHandle();

            //Ctx.mInstance.mPlayerMgr.addEntity(this);
        }

        override public void initRender()
        {
            mRender = new PlayerRender(this);
            mRender.init();
        }

        protected override void onPreTick(float delta, TickMode tickMode)
        {
            base.onPreTick(delta, tickMode);
        }

        protected override void onPostTick(float delta, TickMode tickMode)
        {
            base.onPostTick(delta, tickMode);

            if (null != this.mAnimFSM)
            {
                this.mAnimFSM.UpdateFSM();
            }

            if (null != this.mPlayerSplitMerge)
            {
                this.mPlayerSplitMerge.onTick(delta, tickMode);
            }
        }

        override public void setPos(UnityEngine.Vector3 pos)
        {
            UnityEngine.Vector3 origPos = this.mPos;
            base.setPos(pos);
            this.mDeltaPos = this.mPos - origPos;
        }

        override public void setDestPos(UnityEngine.Vector3 pos, bool immePos)
        {
            base.setDestPos(pos, immePos);

            // 调整 Child 的位置
            //if (null != this.mPlayerSplitMerge)
            //{
            //    this.mPlayerSplitMerge.setDestPos(pos, immePos);
            //}
        }

        public UnityEngine.Vector3 getDeltaPos()
        {
            return this.mDeltaPos;
        }

        override public bool canEatOther(BeingEntity other)
        {
            bool ret = false;

            // 雪快必然可以被吃掉，玩家吐出的雪块也必然可以被吃
            if (other.getEntityType() == EntityType.eSnowBlock ||
                other.getEntityType() == EntityType.ePlayerSnowBlock)
            {
                float bigRadius = 0;

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
            else
            {
                ret = base.canEatOther(other);
            }

            return ret;
        }

        // 获取所有的 ChildMovement
        public PlayerMovement[] getAllChildMovement()
        {
            if(null != this.mPlayerSplitMerge)
            {
                return this.mPlayerSplitMerge.getAllChildMovement();
            }

            return null;
        }

        public MList<SceneEntityBase> getChildList()
        {
            if (null != this.mPlayerSplitMerge)
            {
                return this.mPlayerSplitMerge.mPlayerChildMgr.getSceneEntityList();
            }

            return null;
        }

        //public void addSplitChild(PlayerChild playerChild)
        //{
        //    if(null != this.mPlayerSplitMerge)
        //    {
        //        this.mPlayerSplitMerge.addSplitChild(playerChild);
        //    }
        //}

        public bool isExistThisId(uint thisId)
        {
            if(null != this.mPlayerSplitMerge)
            {
                return this.mPlayerSplitMerge.mPlayerChildMgr.isExistThisId(thisId);
            }

            return false;
        }

        public bool getIsDead()
        {
            return 0 == this.mPlayerSplitMerge.mPlayerChildMgr.getEntityCount();
        }
    }
}
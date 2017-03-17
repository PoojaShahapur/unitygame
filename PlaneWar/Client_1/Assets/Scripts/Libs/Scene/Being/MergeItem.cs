namespace SDK.Lib
{
    /**
     * @brief 融合 Item
     */
    public class MergeItem
    {
        protected double mTimeStamp;  // 时间戳
        protected string mMergeAId;
        protected string mMergeBId;

        public uint mMergeAThisId;
        public uint mMergeBThisId;

        protected PlayerMain mPlayerMain;
        protected float mDistance;
        protected double mCurTimeInterval;

        public MergeItem(PlayerMain player)
        {
            this.mTimeStamp = 0;
            this.mPlayerMain = player;
        }

        public void adjustTimeStamp()
        {
            this.mTimeStamp = UtilApi.getFloatUTCSec();
        }

        public bool isInRange()
        {
            bool ret = false;

            PlayerMainChild aChild = this.mPlayerMain.mPlayerSplitMerge.mPlayerChildMgr.getEntityByUniqueId(mMergeAId) as PlayerMainChild;
            PlayerMainChild bChild = this.mPlayerMain.mPlayerSplitMerge.mPlayerChildMgr.getEntityByUniqueId(mMergeBId) as PlayerMainChild;

            if (null != aChild && null != bChild)
            {
                float dist = UtilMath.squaredDistance(aChild.getPos(), bChild.getPos());

                if(dist <= this.mDistance + Ctx.mInstance.mSnowBallCfg.mMergeRange)
                {
                    ret = true;
                }
            }

            if (MacroDef.ENABLE_LOG)
            {
                if (ret)
                {
                    Ctx.mInstance.mLogSys.log(string.Format("MergeItem::isInRange, InRange OK, aThisId = {0}, bThisId = {1}", aChild.getThisId(), bChild.getThisId()), LogTypeId.eLogMergeBug);
                }
            }

            return ret;
        }

        public bool canMerge()
        {
            bool ret = false;

            PlayerMainChild aChild = this.mPlayerMain.mPlayerSplitMerge.mPlayerChildMgr.getEntityByUniqueId(mMergeAId) as PlayerMainChild;
            PlayerMainChild bChild = this.mPlayerMain.mPlayerSplitMerge.mPlayerChildMgr.getEntityByUniqueId(mMergeBId) as PlayerMainChild;

            if (null != aChild && null != bChild)
            {
                ret = UtilLogic.canContactMerge(this.mTimeStamp);

                this.mCurTimeInterval = UtilApi.getFloatUTCSec() - this.mTimeStamp;

                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log(string.Format("MergeItem::canMerge, aThisId = {0}, bThisId = {1}, merge cantact time = {2}, left time = {3}, ret = {4}", aChild.getThisId(), bChild.getThisId(), Ctx.mInstance.mSnowBallCfg.mMergeContactTime, this.mCurTimeInterval, ret.ToString()), LogTypeId.eLogScene);
                }
            }
            else
            {
                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log("MergeItem::canMerge, can not find being", LogTypeId.eLogScene);
                }
            }

            if (MacroDef.ENABLE_LOG)
            {
                if (ret)
                {
                    Ctx.mInstance.mLogSys.log(string.Format("MergeItem::canMerge, Start merge, aThisId = {0}, bThisId = {1}", aChild.getThisId(), bChild.getThisId()), LogTypeId.eLogMergeBug);
                }
            }

            return ret;
        }

        public string getMergeAId()
        {
            return this.mMergeAId;
        }

        public string getMergeBId()
        {
            return this.mMergeBId;
        }

        public void setMergeBeingEntityId(string aId, string bId)
        {
            this.mMergeAId = aId;
            this.mMergeBId = bId;
        }

        public void setMergeBeingEntityThisId(uint aId, uint bId)
        {
            this.mMergeAThisId = aId;
            this.mMergeBThisId = bId;
        }

        public void setDistance(PlayerChild aChild, PlayerChild bChild)
        {
            this.mDistance = UtilMath.squaredDistance(aChild.getPos(), bChild.getPos());
        }

        public void merge()
        {
            PlayerMainChild aChild = this.mPlayerMain.mPlayerSplitMerge.mPlayerChildMgr.getEntityByUniqueId(mMergeAId) as PlayerMainChild;
            PlayerMainChild bChild = this.mPlayerMain.mPlayerSplitMerge.mPlayerChildMgr.getEntityByUniqueId(mMergeBId) as PlayerMainChild;

            if (null != aChild && null != bChild)
            {
                //if (aChild.getBallRadius() > bChild.getBallRadius())
                //{
                //    aChild.setBallRadius(UtilMath.getNewRadiusByRadius(aChild.getBallRadius(), bChild.getBallRadius()));
                //    bChild.dispose();
                //}
                //else
                //{
                //    bChild.setBallRadius(UtilMath.getNewRadiusByRadius(bChild.getBallRadius(), aChild.getBallRadius()));
                //    aChild.dispose();
                //}

                //aChild.setIsMerge(true);
                //bChild.setIsMerge(true);

                aChild.setBeingSubState(BeingSubState.eBSSReqServerMerge);
                bChild.setBeingSubState(BeingSubState.eBSSReqServerMerge);

                //Game.Game.ReqSceneInteractive.sendMerge(aChild, bChild);

                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log(string.Format("MergeItem::merge, Send merge msg, aThisId = {0}, bThisId = {1}", aChild.getThisId(), bChild.getThisId()), LogTypeId.eLogMergeBug);
                }
            }
            else
            {
                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log("MergeItem::merge, merge fail, aChild or bChild is null", LogTypeId.eLogMergeBug);
                }
            }
        }

        // 添加融合
        public void onAddMerge()
        {
            PlayerMainChild aChild = this.mPlayerMain.mPlayerSplitMerge.mPlayerChildMgr.getEntityByUniqueId(mMergeAId) as PlayerMainChild;
            PlayerMainChild bChild = this.mPlayerMain.mPlayerSplitMerge.mPlayerChildMgr.getEntityByUniqueId(mMergeBId) as PlayerMainChild;

            if (null != aChild && null != bChild)
            {
                float squaredDistance = UtilMath.squaredDistance(aChild.getPos(), bChild.getPos());

                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log(string.Format("MergeItem::onAddMerge, aThisId = {0}, bThisId = {1}, aX = {2}, aY = {3}, aZ = {4}, bX = {5}, bY = {6}, bZ = {7}, CurSquaredDist = {8}, ContactSquaredDist = {9}", aChild.getThisId(), bChild.getThisId(), aChild.getPos().x, aChild.getPos().y, aChild.getPos().z, bChild.getPos().x, bChild.getPos().y, bChild.getPos().z, squaredDistance, this.mDistance), LogTypeId.eLogScene);
                }
            }
            else
            {
                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log("MergeItem::onExceedRange, can not find being", LogTypeId.eLogScene);
                }
            }
        }

        // 将要移除
        public void onExceedRange()
        {
            PlayerMainChild aChild = this.mPlayerMain.mPlayerSplitMerge.mPlayerChildMgr.getEntityByUniqueId(mMergeAId) as PlayerMainChild;
            PlayerMainChild bChild = this.mPlayerMain.mPlayerSplitMerge.mPlayerChildMgr.getEntityByUniqueId(mMergeBId) as PlayerMainChild;

            if (null != aChild && null != bChild)
            {
                if (null != aChild)
                {
                    aChild.setBeingSubState(BeingSubState.eBSSNone);
                }
                if (null != bChild)
                {
                    bChild.setBeingSubState(BeingSubState.eBSSNone);
                }

                float squaredDistance = UtilMath.squaredDistance(aChild.getPos(), bChild.getPos());

                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log(string.Format("MergeItem::onExceedRange, aThisId = {0}, bThisId = {1}, aX = {2}, aY = {3}, aZ = {4}, bX = {5}, bY = {6}, bZ = {7}, CurSquaredDist = {8}, ContactSquaredDist = {9}", aChild.getThisId(), bChild.getThisId(), aChild.getPos().x, aChild.getPos().y, aChild.getPos().z, bChild.getPos().x, bChild.getPos().y, bChild.getPos().z, squaredDistance, this.mDistance), LogTypeId.eLogScene);
                }
            }
            else
            {
                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log("MergeItem::onExceedRange, can not find being", LogTypeId.eLogScene);
                }
            }
        }
    }
}
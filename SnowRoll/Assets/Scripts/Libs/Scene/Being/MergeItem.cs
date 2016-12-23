namespace SDK.Lib
{
    /**
     * @brief 融合 Item
     */
    public class MergeItem
    {
        protected uint mTimeStamp;  // 时间戳
        protected string mMergeAId;
        protected string mMergeBId;

        protected PlayerMain mPlayerMain;

        public MergeItem(PlayerMain player)
        {
            this.mTimeStamp = 0;
            this.mPlayerMain = player;
        }

        public void adjustTimeStamp()
        {
            this.mTimeStamp = UtilApi.getUTCSec();
        }

        public bool canMerge()
        {
            return UtilLogic.canMerge(this.mTimeStamp);
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

        public void merge()
        {
            PlayerMainChild aChild = this.mPlayerMain.mPlayerSplitMerge.mPlayerChildMgr.getEntityByUniqueId(mMergeAId) as PlayerMainChild;
            PlayerMainChild bChild = this.mPlayerMain.mPlayerSplitMerge.mPlayerChildMgr.getEntityByUniqueId(mMergeBId) as PlayerMainChild;

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
    }
}
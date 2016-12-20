namespace SDK.Lib
{
    public class PlayerOtherSplitMerge : PlayerSplitMerge
    {
        public PlayerOtherSplitMerge(Player mPlayer)
            : base(mPlayer)
        {

        }

        override protected void onFirstSplit()
        {
            base.onFirstSplit();

            PlayerOtherChild child;
            child = new PlayerOtherChild(this.mEntity);
            child.init();
            child.setPos(this.mEntity.getPos());
        }

        override protected void onNoFirstSplit()
        {
            PlayerOtherChild child;
            // 这个分裂修改列表数据，因此只查找前面的数据
            int idx = 0;
            int num = mPlayerChildMgr.getEntityCount();

            while (idx < num)
            {
                child = new PlayerOtherChild(this.mEntity);
                child.init();
                child.setPos(this.mEntity.getPos());

                ++idx;
            }
        }
    }
}
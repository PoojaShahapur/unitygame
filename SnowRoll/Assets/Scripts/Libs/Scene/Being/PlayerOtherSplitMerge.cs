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
        }

        override public void removeFormParent(Player childPlayer)
        {
            base.removeFormParent(childPlayer);

            // 如果没有 PlayerChild，直接释放 PlayerOther
            if(0 == this.mPlayerChildMgr.getEntityCount())
            {
                this.mEntity.dispose();
            }
        }
    }
}
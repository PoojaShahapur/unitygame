namespace SDK.Lib
{
    public class PlayerMainSplitMerge : PlayerSplitMerge
    {
        public PlayerMainSplitMerge(Player mPlayer)
            : base(mPlayer)
        {

        }

        override protected void onFirstSplit()
        {
            base.onFirstSplit();

            PlayerMainChild child;
            child = new PlayerMainChild(mParentPlayer);
            child.init();
            //UnityEngine.Vector3 initPos = this.mEntity.getPos() + this.mEntity.getRotate() * new UnityEngine.Vector3(0, 0, this.mEntity.getEatSize());
            UnityEngine.Vector3 initPos = this.mEntity.getPos();
            child.setOriginal(initPos);
            //child.setEatSize(this.mEntity.getEatSize() / 2);

            // 设置分裂半径
            this.mEntity.setEatSize(this.mEntity.getEatSize());
        }

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
                player = this.mPlayerChildMgr.getEntityByIndex(idx) as PlayerChild;
                pos = player.getPos();
                this.mRangeBox.setExtents(pos.x, pos.y, pos.z);

                child = new PlayerMainChild(mParentPlayer);
                child.init();

                UnityEngine.Vector3 initPos = player.getPos() + player.getRotate() * new UnityEngine.Vector3(0, 0, player.getEatSize() + 5);
                child.setOriginal(initPos);
                child.setEatSize(player.getEatSize() / 2);

                // 设置分裂半径
                player.setEatSize(this.mEntity.getEatSize() / 2);

                ++idx;
            }

            // 设置自己到中心点
            this.mEntity.setOriginal(this.mRangeBox.getCenter().toNative());
            this.calcTargetLength();
            this.updateChildDestDir();
        }
    }
}
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
            this.updateChildDestDir();
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

                //UnityEngine.Vector3 initPos = player.getPos() + player.getRotate() * new UnityEngine.Vector3(0, 0, player.getEatSize() + 5);
                UnityEngine.Vector3 initPos = player.getPos() + UtilMath.UnitCircleRandom();
                child.setOriginal(initPos);
                child.setEatSize(player.getEatSize() / 2);

                // 设置分裂半径
                player.setEatSize(player.getEatSize() / 2);

                // 添加 Child 事件
                ((this.mEntity as PlayerMain).mMovement as PlayerMainMovement).addOrientChangedHandle((child.mMovement as PlayerMainChildMovement).handleParentOrientChanged);
                ((this.mEntity as PlayerMain).mMovement as PlayerMainMovement).addPosChangedHandle((child.mMovement as PlayerMainChildMovement).handleParentPosChanged);
                ((this.mEntity as PlayerMain).mMovement as PlayerMainMovement).addOrientStopChangedHandle((child.mMovement as PlayerMainChildMovement).handleParentOrientStopChanged);
                ((this.mEntity as PlayerMain).mMovement as PlayerMainMovement).addPosStopChangedHandle((child.mMovement as PlayerMainChildMovement).handleParentPosStopChanged);

                ++idx;
            }

            // 设置自己到中心点
            this.mEntity.setOriginal(this.mRangeBox.getCenter().toNative());
            this.calcTargetLength();
            this.calcTargetPoint();
            this.updateChildDestDir();
        }
    }
}
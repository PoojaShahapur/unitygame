namespace SDK.Lib
{
    public class PlayerChildMovement : PlayerMovement
    {
        public PlayerChildMovement(SceneEntityBase entity)
            : base(entity)
        {

        }

        // 移动暂停
        override public void movePause()
        {
            base.movePause();
            this.setIsMoveToDest(false);
        }

        override public void onArriveDestPos()
        {
            base.onArriveDestPos();

            // 如果是被合并掉的
            if (BeingSubState.eBSSMerge == (this.mEntity as BeingEntity).getBeingSubState())
            {
                Ctx.mInstance.mLogSys.log(string.Format("PlayerChildMovement::onArriveDestPos, Merge success, dispose thisId = {0}", this.mEntity.getThisId()), LogTypeId.eLogSplitMergeEmit);

                this.mEntity.dispose();
            }
        }
    }
}
namespace SDK.Lib
{
    /**
    * @brief Player 吐出来的积雪块
    */
    public class PlayerSnowBlockMgr : EntityMgrBase
    {
        public PlayerSnowBlockMgr()
        {
            mUniqueStrIdGen = new UniqueStrIdGen(UniqueStrIdGen.PlayerSnowBlockPrefix, 0);
        }

        override protected void onTickExec(float delta)
        {
            base.onTickExec(delta);
        }

        override public void init()
        {
            base.init();
        }

        public override void dispose()
        {
            base.dispose();
        }

        public void addPlayerSnowBlock(PlayerSnowBlock playerSnowBlock)
        {
            this.addEntity(playerSnowBlock);
        }

        public void removePlayerSnowBlock(PlayerSnowBlock playerSnowBlock)
        {
            this.removeEntity(playerSnowBlock);
        }

        // 发射出一个 PlayerSnowBlock
        public void emitOne(UnityEngine.Vector3 srcPos, UnityEngine.Vector3 destPos, UnityEngine.Quaternion rot, float emitSnowSize)
        {
            PlayerSnowBlock playerSnowBlock = new PlayerSnowBlock();
            playerSnowBlock.init();

            playerSnowBlock.setDestRotate(rot.eulerAngles, true);
            playerSnowBlock.setPos(srcPos);
            playerSnowBlock.setDestPos(destPos, false);
            playerSnowBlock.setBallRadius(emitSnowSize);
        }

        public void changeThisId(uint srcThisId, PlayerSnowBlock playerSnowBlock)
        {
            base.changeThisId(srcThisId, playerSnowBlock.getThisId(), playerSnowBlock);
        }

        public bool isExistNumId(uint numId)
        {
            string uniqueId = this.genStrIdById(numId);
            return this.mId2EntityDic.ContainsKey(uniqueId);
        }
    }
}
namespace SDK.Lib
{
    public class PlayerOtherChild : PlayerChild
    {
        public PlayerOtherChild(Player parentPlayer)
            : base(parentPlayer)
        {
            this.mTypeId = "PlayerOtherChild";
            this.mEntityType = EntityType.ePlayerOtherChild;
            this.mMovement = new PlayerOtherChildMovement(this);
            this.mAttack = new PlayerOtherChildAttack(this);
            this.mEntityUniqueId = Ctx.mInstance.mPlayerMgr.genChildNewStrId();
        }

        override public void initRender()
        {
            if (!this.isPrefabPathValid())
            {
                this.setPrefabPath(Ctx.mInstance.mSnowBallCfg.getRandomBallOtherTex());
            }

            mRender = new PlayerOtherChildRender(this);
            mRender.init();
        }

        override public void setPos(UnityEngine.Vector3 pos)
        {
            base.setPos(pos);

            // 如果 Hero ，没有移动的时候，才更新，如果 Hero 在移动，直接通过相机移动更新
            if (!Ctx.mInstance.mPlayerMgr.isHeroMoving())
            {
                if (null != this.mHud)
                {
                    this.mHud.onPosChanged();
                }
            }
        }
    }
}
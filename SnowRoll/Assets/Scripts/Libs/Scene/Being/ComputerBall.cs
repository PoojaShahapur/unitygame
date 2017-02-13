namespace SDK.Lib
{
    public class ComputerBall : BeingEntity
    {
        public ComputerBall()
        {
            this.mTypeId = "ComputerBall";
            this.mEntityType = EntityType.eComputerBall;
            this.mEntityUniqueId = Ctx.mInstance.mComputerBallMgr.genNewStrId();

            this.mMovement = new ComputerBallMovement(this);
            this.mAttack = new ComputerBallAttack(this);
        }

        override public void dispose()
        {
            base.dispose();

            Ctx.mInstance.mComputerBallMgr.removeComputerBall(this);
        }

        protected override void onPostInit()
        {
            base.onPostInit();

            this.mHud = Ctx.mInstance.mHudSystem.createHud(this);
        }

        override public void autoHandle()
        {
            base.autoHandle();

            Ctx.mInstance.mComputerBallMgr.addComputerBall(this);
        }

        override public void initRender()
        {
            if (!this.isPrefabPathValid())
            {
                this.setPrefabPath(Ctx.mInstance.mSnowBallCfg.getRandomBallOtherTex());
            }

            mRender = new ComputerBallRender(this);
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
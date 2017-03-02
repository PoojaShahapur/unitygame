namespace SDK.Lib
{
    public class FlyBulletAttack : BeingEntityAttack
    {
        public FlyBulletAttack(BeingEntity entity)
            : base(entity)
        {

        }

        override public void overlapToEnter(BeingEntity bBeingEntity, UnityEngine.Collision collisionInfo)
        {
            this.handleEnter(bBeingEntity);
        }

        override public void overlapToStay(BeingEntity bBeingEntity, UnityEngine.Collision collisionInfo)
        {
            
        }

        override public void overlapToExit(BeingEntity bBeingEntity, UnityEngine.Collision collisionInfo)
        {

        }

        override public void overlapToEnter2D(BeingEntity bBeingEntity, UnityEngine.Collision2D collisionInfo)
        {
            this.handleEnter(bBeingEntity);
        }

        override public void overlapToStay2D(BeingEntity bBeingEntity, UnityEngine.Collision2D collisionInfo)
        {
            
        }

        override public void overlapToExit2D(BeingEntity bBeingEntity, UnityEngine.Collision2D collisionInfo)
        {

        }

        protected void handleEnter(BeingEntity bBeingEntity)
        {
            // 自己的 Bullet
            if ((this.mEntity as FlyBullet).isSelfBullet())
            {
                if (EntityType.eSnowBlock == bBeingEntity.getEntityType())
                {
                    this.hitSnowBlock(bBeingEntity);
                }
            }
            else
            {
                if (EntityType.ePlayerMainChild == bBeingEntity.getEntityType())
                {
                    this.hitPlayerMainChild(bBeingEntity);
                }
            }
        }

        protected void handleStay(BeingEntity bBeingEntity)
        {

        }

        protected void handleExit(BeingEntity bBeingEntity)
        {

        }

        // 地上走动雪块
        public void hitSnowBlock(BeingEntity bBeingEntity)
        {
            Ctx.mInstance.mLogSys.log("FlyBulletAttack::hitSnowBlock", LogTypeId.eLogSimHitBullet);

            Game.Game.ReqSceneInteractive.sendHitEnergy(this.mEntity as FlyBullet, bBeingEntity as SnowBlock);
        }

        // 其它飞机
        public void hitPlayerOtherChild(BeingEntity bBeingEntity)
        {
            Ctx.mInstance.mLogSys.log("FlyBulletAttack::hitPlayerOtherChild", LogTypeId.eLogSimHitBullet);
        }

        // 机器人
        public void hitComputerBall(BeingEntity bBeingEntity)
        {
            Ctx.mInstance.mLogSys.log("FlyBulletAttack::hitComputerBall", LogTypeId.eLogSimHitBullet);
        }

        public void hitPlayerMainChild(BeingEntity bBeingEntity)
        {
            Ctx.mInstance.mLogSys.log("FlyBulletAttack::hitPlayerMainChild", LogTypeId.eLogSimHitBullet);

            Game.Game.ReqSceneInteractive.sendHitSelfChild(this.mEntity as FlyBullet, bBeingEntity as PlayerMainChild);
        }
    }
}
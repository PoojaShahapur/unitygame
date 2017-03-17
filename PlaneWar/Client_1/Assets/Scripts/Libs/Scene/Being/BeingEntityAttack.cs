namespace SDK.Lib
{
    /**
     * @brief 攻击控制器，控制攻击逻辑
     */
    public class BeingEntityAttack
    {
        protected BeingEntity mEntity;

        public BeingEntityAttack(BeingEntity entity)
        {
            mEntity = entity;
        }

        virtual public void init()
        {

        }

        virtual public void dispose()
        {

        }

        virtual public void onTick(float delta, TickMode tickMode)
        {

        }

        virtual public void overlapToEnter(BeingEntity bBeingEntity, UnityEngine.Collision collisionInfo)
        {
            if (UtilApi.isInFakePos(this.mEntity.getPos()))
            {
                return;
            }

            if (EntityType.ePlayerMainChild == bBeingEntity.getEntityType())
            {
                bBeingEntity.overlapToEnter(this.mEntity, collisionInfo);
            }
            else if (EntityType.eFlyBullet == bBeingEntity.getEntityType())
            {
                bBeingEntity.overlapToEnter(this.mEntity, collisionInfo);
            }
        }

        virtual public void overlapToStay(BeingEntity bBeingEntity, UnityEngine.Collision collisionInfo)
        {
            if (UtilApi.isInFakePos(this.mEntity.getPos()))
            {
                return;
            }

            if (EntityType.ePlayerMainChild == bBeingEntity.getEntityType())
            {
                bBeingEntity.overlapToStay(this.mEntity, collisionInfo);
            }
            else if (EntityType.eFlyBullet == bBeingEntity.getEntityType())
            {
                bBeingEntity.overlapToStay(this.mEntity, collisionInfo);
            }
        }

        virtual public void overlapToExit(BeingEntity bBeingEntity, UnityEngine.Collision collisionInfo)
        {
            if (UtilApi.isInFakePos(this.mEntity.getPos()))
            {
                return;
            }

            if (EntityType.ePlayerMainChild == bBeingEntity.getEntityType())
            {
                bBeingEntity.overlapToExit(this.mEntity, collisionInfo);
            }
            else if (EntityType.eFlyBullet == bBeingEntity.getEntityType())
            {
                bBeingEntity.overlapToExit(this.mEntity, collisionInfo);
            }
        }

        virtual public void overlapToEnter2D(BeingEntity bBeingEntity, UnityEngine.Collision2D collisionInfo)
        {
            if (UtilApi.isInFakePos(this.mEntity.getPos()))
            {
                return;
            }

            if (EntityType.eFlyBullet == bBeingEntity.getEntityType())
            {
                bBeingEntity.overlapToEnter2D(this.mEntity, collisionInfo);
            }
        }

        virtual public void overlapToStay2D(BeingEntity bBeingEntity, UnityEngine.Collision2D collisionInfo)
        {
            if (UtilApi.isInFakePos(this.mEntity.getPos()))
            {
                return;
            }

            if (EntityType.ePlayerMainChild == bBeingEntity.getEntityType())
            {
                bBeingEntity.overlapToStay2D(this.mEntity, collisionInfo);
            }
            else if (EntityType.eFlyBullet == bBeingEntity.getEntityType())
            {
                bBeingEntity.overlapToStay2D(this.mEntity, collisionInfo);
            }
        }

        virtual public void overlapToExit2D(BeingEntity bBeingEntity, UnityEngine.Collision2D collisionInfo)
        {
            if (UtilApi.isInFakePos(this.mEntity.getPos()))
            {
                return;
            }

            if (EntityType.ePlayerMainChild == bBeingEntity.getEntityType())
            {
                bBeingEntity.overlapToExit2D(this.mEntity, collisionInfo);
            }
            else if (EntityType.eFlyBullet == bBeingEntity.getEntityType())
            {
                bBeingEntity.overlapToExit2D(this.mEntity, collisionInfo);
            }
        }
    }
}
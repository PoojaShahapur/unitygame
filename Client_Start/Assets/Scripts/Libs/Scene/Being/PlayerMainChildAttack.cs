namespace SDK.Lib
{
    public class PlayerMainChildAttack : BeingEntityAttack
    {
        public PlayerMainChildAttack(BeingEntity entity)
            : base(entity)
        {

        }

        override public void overlapToEnter(BeingEntity bBeingEntity, UnityEngine.Collision collisionInfo)
        {
            if (bBeingEntity.getEntityType() == EntityType.eSnowBlock)
            {
                this.eateSnowBlock(bBeingEntity);
            }
            else if (bBeingEntity.getEntityType() == EntityType.ePlayerOther)
            {
                this.eatePlayerOther(bBeingEntity);
            }
            else if (bBeingEntity.getEntityType() == EntityType.eRobot)
            {
                EatState state = EatState.Nothing_Happen;

                Player bPlayer = bBeingEntity as Player;

                if (this.mEntity.canEatOther(bBeingEntity))
                {
                    state = EatState.Eat_Other;
                }
                else if (bBeingEntity.canEatOther(this.mEntity))
                {
                    state = EatState.Eaten_ByOther;
                }

                if (EatState.Nothing_Happen != state)
                {
                    //计算缩放比率            
                    float newBallRadius = UtilLogic.getRadiusByMass(UtilLogic.getMassByRadius(this.mEntity.getEatSize()) + UtilLogic.getMassByRadius(bBeingEntity.getEatSize()));

                    if (state == EatState.Eat_Other)//吃掉对方
                    {
                        // 吃掉机器人，修改自己的数据
                        this.mEntity.setEatSize(newBallRadius);
                        bBeingEntity.dispose();      // 删除玩家
                    }
                    else if (EatState.Eaten_ByOther == state)//被吃掉
                    {
                        bBeingEntity.setEatSize(newBallRadius);
                        this.mEntity.dispose();
                    }
                }
            }
        }

        override public void overlapToStay(BeingEntity bBeingEntity, UnityEngine.Collision collision)
        {
            // 如果和 PlayerMainChild 碰撞
            //if (EntityType.ePlayerMainChild == bBeingEntity.getEntityType() ||
            //    EntityType.ePlayerMain == bBeingEntity.getEntityType())
            if (EntityType.ePlayerMainChild == bBeingEntity.getEntityType())
            {
                if (this.mEntity.isNeedReduceSpeed() || bBeingEntity.isNeedReduceSpeed())
                {
                    if (UtilMath.isBehindCollidePoint(this.mEntity.getPos(), this.mEntity.getForward(), collision))
                    {
                        // 需要减小速度
                        this.mEntity.setMoveSpeed(bBeingEntity.getMoveSpeed());
                    }
                    else
                    {
                        bBeingEntity.setMoveSpeed(this.mEntity.getMoveSpeed());
                    }
                }
                else if (this.mEntity.canMerge() && bBeingEntity.canMerge())
                {
                    (this.mEntity as PlayerMainChild).mParentPlayer.mPlayerSplitMerge.addMerge(this.mEntity as PlayerChild, bBeingEntity as PlayerChild);
                }
            }
            else if(EntityType.ePlayerOtherChild == bBeingEntity.getEntityType())
            {
                // 如果碰撞 PlayerOtherChild
            }
        }

        override public void overlapToExit(BeingEntity bBeingEntity, UnityEngine.Collision collision)
        {
            // 如果和 PlayerMainChild 碰撞
            if (EntityType.ePlayerMainChild == bBeingEntity.getEntityType())
            {
                (this.mEntity as PlayerMainChild).mParentPlayer.mPlayerSplitMerge.removeMerge(this.mEntity as PlayerChild, bBeingEntity as PlayerChild);
            }
        }

        // 雪块
        public void eateSnowBlock(BeingEntity bBeingEntity)
        {
            this.mEntity.cellCall("eatSnowBlock", bBeingEntity.getId());
        }

        // 玩家之间互吃
        public void eatePlayerOther(BeingEntity bBeingEntity)
        {
            byte otherIsGod = (byte)bBeingEntity.getEntity().getDefinedProperty("isGod");

            if (this.mEntity.canEatOther(bBeingEntity) && 0 == otherIsGod)
            {
                this.mEntity.cellCall("eatSnowBlock", bBeingEntity.getId());
            }
        }
    }
}
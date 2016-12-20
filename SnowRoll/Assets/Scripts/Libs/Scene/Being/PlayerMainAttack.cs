namespace SDK.Lib
{
    public class PlayerMainAttack : BeingEntityAttack
    {
        public PlayerMainAttack(BeingEntity entity)
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
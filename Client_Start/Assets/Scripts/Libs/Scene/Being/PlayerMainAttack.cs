namespace SDK.Lib
{
    public class PlayerMainAttack : BeingEntityAttack
    {
        public PlayerMainAttack(BeingEntity entity)
            : base(entity)
        {

        }

        override public void overlapTo(BeingEntity bBeingEntity)
        {
            if (bBeingEntity.getEntityType() == EntityType.eSnowBlock)
            {
                this.eateSnowBlock(bBeingEntity);
            }
            else if (bBeingEntity.getEntityType() == EntityType.ePlayerOther ||
                     bBeingEntity.getEntityType() == EntityType.eRobot)
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

                if (EatState.Nothing_Happen == state) return;

                //计算缩放比率            
                float newBallRadius = UtilLogic.getRadiusByMass(UtilLogic.getMassByRadius(this.mEntity.getEatSize()) + UtilLogic.getMassByRadius(bBeingEntity.getEatSize()));

                if (state == EatState.Eat_Other)//吃掉对方
                {
                    // 吃掉机器人，修改自己的数据
                    this.mEntity.setEatSize(newBallRadius);
                    //++(this.mEntity as Player).m_swallownum;
                    bBeingEntity.dispose();      // 删除玩家
                }
                else if (EatState.Eaten_ByOther == state)//被吃掉
                {
                    bBeingEntity.setEatSize(newBallRadius);
                    //++(bBeingEntity as Player).m_swallownum;
                    this.mEntity.dispose();
                }
            }
        }

        // 雪块
        public void eateSnowBlock(BeingEntity bBeingEntity)
        {
            this.mEntity.cellCall("eatSnowBlock", bBeingEntity.getId());
        }
    }
}
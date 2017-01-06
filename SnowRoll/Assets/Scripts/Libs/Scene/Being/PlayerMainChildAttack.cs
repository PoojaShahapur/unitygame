namespace SDK.Lib
{
    public class PlayerMainChildAttack : PlayerChildAttack
    {
        public PlayerMainChildAttack(BeingEntity entity)
            : base(entity)
        {

        }

        override public void overlapToEnter(BeingEntity bBeingEntity, UnityEngine.Collision collisionInfo)
        {
            if (!(this.mEntity as BeingEntity).canInterActive(bBeingEntity))
            {
                return;
            }
        }

        override public void overlapToStay(BeingEntity bBeingEntity, UnityEngine.Collision collisionInfo)
        {
            if (!(this.mEntity as BeingEntity).canInterActive(bBeingEntity))
            {
                return;
            }

            if (EntityType.eSnowBlock == bBeingEntity.getEntityType())
            {
                this.eatSnowBlock(bBeingEntity);
            }
            else if (EntityType.ePlayerOther == bBeingEntity.getEntityType())
            {
                this.eatPlayerOther(bBeingEntity);
            }
            else if (EntityType.ePlayerOtherChild == bBeingEntity.getEntityType())
            {
                this.eatPlayerOtherChild(bBeingEntity);
            }
            else if (EntityType.eRobot == bBeingEntity.getEntityType())
            {
                this.eatRobot(bBeingEntity);
            }
            else if (EntityType.ePlayerMainChild == bBeingEntity.getEntityType())
            {
                this.eatPlayerMainChild(bBeingEntity, collisionInfo);
            }
            else if (EntityType.ePlayerSnowBlock == bBeingEntity.getEntityType())
            {
                this.eatPlayerSnowBlock(bBeingEntity);
            }
        }

        override public void overlapToExit(BeingEntity bBeingEntity, UnityEngine.Collision collisionInfo)
        {
            if (!(this.mEntity as BeingEntity).canInterActive(bBeingEntity))
            {
                return;
            }

            // 如果和 PlayerMainChild 碰撞
            if (EntityType.ePlayerMainChild == bBeingEntity.getEntityType())
            {
                //(this.mEntity as PlayerMainChild).mParentPlayer.mPlayerSplitMerge.removeMerge(this.mEntity as PlayerChild, bBeingEntity as PlayerChild);
            }
        }

        // 雪块
        public void eatSnowBlock(BeingEntity bBeingEntity)
        {
            if (this.mEntity.canEatOther(bBeingEntity))
            {
                bBeingEntity.setClientDispose(true);

                if (!MacroDef.DEBUG_NOTNET)
                {
                    this.mEntity.cellCall("eatSnowBlock", bBeingEntity.getThisId());
                    //(this.mEntity as PlayerChild).mParentPlayer.cellCall("eatSnowBlock", bBeingEntity.getThisId());
                }
                else
                {
                    // TODO:客户端自己模拟
                    //float newRadius = UtilMath.getNewRadiusByRadius(this.mEntity.getBallRadius(), bBeingEntity.getBallRadius());

                    float newRadius = UtilMath.getEatSnowNewRadiusByRadius(this.mEntity.getBallRadius());

                    this.mEntity.setBeingState(BeingState.eBSAttack);
                    this.mEntity.setBallRadius(newRadius);
                    bBeingEntity.dispose();
                }
            }
        }

        // 玩家之间互吃
        public void eatPlayerOther(BeingEntity bBeingEntity)
        {
            byte otherIsGod = (byte)bBeingEntity.getEntity().getDefinedProperty("isGod");

            if (this.mEntity.canEatOther(bBeingEntity) && 0 == otherIsGod)
            {
                bBeingEntity.setClientDispose(true);
                this.mEntity.cellCall("eatSnowBlock", bBeingEntity.getThisId());
                //(this.mEntity as PlayerChild).mParentPlayer.cellCall("eatSnowBlock", bBeingEntity.getThisId());
            }
        }

        // 玩家之间互吃
        public void eatPlayerOtherChild(BeingEntity bBeingEntity)
        {
            byte otherIsGod = 0;

            if (!MacroDef.DEBUG_NOTNET)
            {
                otherIsGod = (byte)bBeingEntity.getEntity().getDefinedProperty("isGod");
            }

            if (this.mEntity.canEatOther(bBeingEntity) && 0 == otherIsGod)
            {
                bBeingEntity.setClientDispose(true);

                if (!MacroDef.DEBUG_NOTNET)
                {
                    this.mEntity.cellCall("eatSnowBlock", bBeingEntity.getThisId());
                    //(this.mEntity as PlayerChild).mParentPlayer.cellCall("eatSnowBlock", bBeingEntity.getThisId());
                }
                else
                {
                    // TODO:客户端自己模拟
                    float newRadius = UtilMath.getNewRadiusByRadius(this.mEntity.getBallRadius(), bBeingEntity.getBallRadius());

                    this.mEntity.setBallRadius(newRadius);
                    bBeingEntity.dispose();
                }
            }
        }

        // 吃玩家吐出的雪块
        public void eatPlayerSnowBlock(BeingEntity bBeingEntity)
        {
            bBeingEntity.setClientDispose(true);

            if (!MacroDef.DEBUG_NOTNET)
            {
                this.mEntity.cellCall("eatSnowBlock", bBeingEntity.getThisId());
                //Ctx.mInstance.mPlayerMgr.getHero().cellCall("eatSnowBlock", bBeingEntity.getThisId());
            }
            else
            {
                float newRadius = UtilMath.getNewRadiusByRadius(this.mEntity.getBallRadius(), bBeingEntity.getBallRadius());

                this.mEntity.setBeingState(BeingState.eBSAttack);
                this.mEntity.setBallRadius(newRadius);
                bBeingEntity.dispose();
            }
        }

        // 碰撞 PlayerMainChild
        public void eatPlayerMainChild(BeingEntity bBeingEntity, UnityEngine.Collision collisionInfo)
        {
            // 如果可以合并
            if (this.mEntity.canMerge() && bBeingEntity.canMerge())
            {
                //(this.mEntity as PlayerMainChild).mParentPlayer.mPlayerSplitMerge.addMerge(this.mEntity as PlayerChild, bBeingEntity as PlayerChild);
                // 如果现在能进行合并
                if(this.mEntity.canMergeWithOther(bBeingEntity))
                {
                    bBeingEntity.setClientDispose(true);
                    this.mEntity.setClientDispose(true);

                    //this.mEntity.mergeWithOther(bBeingEntity);
                    Game.Game.ReqSceneInteractive.sendMerge(this.mEntity, bBeingEntity);
                }
            }
            else if (this.mEntity.isNeedReduceSpeed() || bBeingEntity.isNeedReduceSpeed())
            {
                if (UtilMath.isBehindCollidePoint(this.mEntity.getPos(), this.mEntity.getForward(), collisionInfo))
                {
                    // 需要减小速度
                    this.mEntity.setMoveSpeed(bBeingEntity.getMoveSpeed());
                }
                else
                {
                    bBeingEntity.setMoveSpeed(this.mEntity.getMoveSpeed());
                }
            }
        }

        // 吃 Robot
        protected void eatRobot(BeingEntity bBeingEntity)
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
                float newBallRadius = UtilMath.getNewRadiusByRadius(this.mEntity.getBallRadius(), bBeingEntity.getBallRadius());

                if (state == EatState.Eat_Other)//吃掉对方
                {
                    // 吃掉机器人，修改自己的数据
                    this.mEntity.setBallRadius(newBallRadius);
                    bBeingEntity.dispose();      // 删除玩家
                }
                else if (EatState.Eaten_ByOther == state)//被吃掉
                {
                    bBeingEntity.setBallRadius(newBallRadius);
                    this.mEntity.dispose();
                }
            }
        }
    }
}
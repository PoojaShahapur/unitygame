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
            
        }

        override public void overlapToStay(BeingEntity bBeingEntity, UnityEngine.Collision collisionInfo)
        {
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
            else if (EntityType.ePlayerMainChild == bBeingEntity.getEntityType())
            {
                this.eatPlayerMainChild(bBeingEntity);
            }
            else if (EntityType.ePlayerSnowBlock == bBeingEntity.getEntityType())
            {
                this.eatPlayerSnowBlock(bBeingEntity);
            }
            else if (EntityType.eComputerBall == bBeingEntity.getEntityType())
            {
                this.eatComputerBall(bBeingEntity);
            }
            else if (EntityType.eFlyBullet == bBeingEntity.getEntityType())
            {
                // 如果是 FlyBullet ，直接转向 FlyBullet 处理
                bBeingEntity.overlapToStay(this.mEntity, collisionInfo);
            }
        }

        override public void overlapToExit(BeingEntity bBeingEntity, UnityEngine.Collision collisionInfo)
        {
            
        }

        //override public void overlapToEnter2D(BeingEntity bBeingEntity, UnityEngine.Collision2D collisionInfo)
        //{
        //    if (EntityType.eFlyBullet == bBeingEntity.getEntityType())
        //    {
        //        // 如果是 FlyBullet ，直接转向 FlyBullet 处理
        //        bBeingEntity.overlapToEnter2D(this.mEntity, collisionInfo);
        //    }
        //}

        override public void overlapToStay2D(BeingEntity bBeingEntity, UnityEngine.Collision2D collisionInfo)
        {
            
        }

        override public void overlapToExit2D(BeingEntity bBeingEntity, UnityEngine.Collision2D collisionInfo)
        {

        }

        // 雪块
        public void eatSnowBlock(BeingEntity bBeingEntity)
        {
            if (this.mEntity.canEatOther(bBeingEntity))
            {
                if (!(bBeingEntity as BeingEntity).getIsEatedByOther())
                {
                    if (MacroDef.ENABLE_LOG)
                    {
                        Ctx.mInstance.mLogSys.log(string.Format("PlayerMainChildAttack::eatSnowBlock, MainChildThisId = {0}, SnowBallThisId = {1}", this.mEntity.getThisId(), bBeingEntity.getThisId()), LogTypeId.eLogScene);
                    }

                    (bBeingEntity as BeingEntity).setIsEatedByOther(true);
                    (bBeingEntity as BeingEntity).forceHide();  // 客户端自己隐藏

                    if (!MacroDef.DEBUG_NOTNET)
                    {
                        //this.mEntity.cellCall("eatSnowBlock", bBeingEntity.getThisId());
                    }
                    else
                    {
                        float newRadius = UtilMath.getEatSnowNewRadiusByRadius(this.mEntity.getBallRadius());

                        this.mEntity.setBeingState(BeingState.eBSAttack);
                        this.mEntity.setBallRadius(newRadius);
                        bBeingEntity.dispose();
                    }
                }
            }
        }

        // 玩家之间互吃
        public void eatPlayerOther(BeingEntity bBeingEntity)
        {
            byte otherIsGod = (byte)bBeingEntity.getIsGod();

            if (this.mEntity.canEatOther(bBeingEntity) && 0 == otherIsGod)
            {
                bBeingEntity.setClientDispose(true);
                //this.mEntity.cellCall("eatSnowBlock", bBeingEntity.getThisId());
            }
        }

        // 玩家之间互吃
        public void eatPlayerOtherChild(BeingEntity bBeingEntity)
        {
            byte otherIsGod = 0;

            if (!MacroDef.DEBUG_NOTNET)
            {
                //otherIsGod = (byte)bBeingEntity.getEntity().getDefinedProperty("isGod");
            }

            if (this.mEntity.canEatOther(bBeingEntity) && 0 == otherIsGod)
            {
                if (!(bBeingEntity as PlayerChild).getIsEatedByOther())
                {
                    if (MacroDef.ENABLE_LOG)
                    {
                        Ctx.mInstance.mLogSys.log(string.Format("PlayerMainChildAttack::eatPlayerOtherChild, MainChildThisId = {0}, OtherChildThisId = {1}", this.mEntity.getThisId(), bBeingEntity.getThisId()), LogTypeId.eLogScene);
                    }

                    (bBeingEntity as PlayerChild).setIsEatedByOther(true);

                    if (!MacroDef.DEBUG_NOTNET)
                    {
                        //this.mEntity.cellCall("eatSnowBlock", bBeingEntity.getThisId());
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
        }

        // 吃玩家吐出的雪块
        public void eatPlayerSnowBlock(BeingEntity bBeingEntity)
        {
            // 如果正在请求服务器融合，也不在处理，这个时候很有可能被融合掉了
            if (!this.mEntity.isReqServerMerge())
            {
                if (!(bBeingEntity as BeingEntity).getIsEatedByOther())
                {
                    (bBeingEntity as BeingEntity).setIsEatedByOther(true);
                    (bBeingEntity as BeingEntity).forceHide();  // 客户端自己隐藏

                    if (MacroDef.ENABLE_LOG)
                    {
                        Ctx.mInstance.mLogSys.log(string.Format("PlayerMainChildAttack::eatPlayerSnowBlock, MainChildThisId = {0}, PlayerSnowBlockThisId = {1}", this.mEntity.getThisId(), bBeingEntity.getThisId()), LogTypeId.eLogScene);
                    }

                    if (!MacroDef.DEBUG_NOTNET)
                    {
                        if (0 != bBeingEntity.getThisId())
                        {
                            //this.mEntity.cellCall("eatSnowBlock", bBeingEntity.getThisId());
                        }
                        else
                        {
                            (bBeingEntity as BeingEntity).setIsEatedByServer(true);
                            (bBeingEntity as PlayerSnowBlock).setOwnerThisId(this.mEntity.getThisId());
                        }
                    }
                    else
                    {
                        float newRadius = UtilMath.getNewRadiusByRadius(this.mEntity.getBallRadius(), bBeingEntity.getBallRadius());

                        this.mEntity.setBeingState(BeingState.eBSAttack);
                        this.mEntity.setBallRadius(newRadius);
                        bBeingEntity.dispose();
                    }
                }
                else if ((bBeingEntity as BeingEntity).getIsEatedByServer())
                {
                    (bBeingEntity as BeingEntity).setIsEatedByServer(false);
                    //this.mEntity.cellCall("eatSnowBlock", bBeingEntity.getThisId());
                }
            }
        }

        // 碰撞 PlayerMainChild
        public void eatPlayerMainChild(BeingEntity bBeingEntity)
        {
            if (MacroDef.ENABLE_LOG)
            {
                Ctx.mInstance.mLogSys.log("PlayerMainChildAttack::eatPlayerMainChild, enter eatPlayerMainChild", LogTypeId.eLogMergeBug);
            }
        }

        public void eatComputerBall(BeingEntity bBeingEntity)
        {
            if (this.mEntity.canEatOther(bBeingEntity))
            {
                if (!(bBeingEntity as BeingEntity).getIsEatedByOther())
                {
                    (bBeingEntity as BeingEntity).setIsEatedByOther(true);

                    if (MacroDef.ENABLE_LOG)
                    {
                        Ctx.mInstance.mLogSys.log(string.Format("PlayerMainChildAttack::eatComputerBall, Ball eat computer ball, MainChildThisId = {0}, ComputerBallThisId = {1}", this.mEntity.getThisId(), bBeingEntity.getThisId()), LogTypeId.eLogScene);
                    }

                    if (!MacroDef.DEBUG_NOTNET)
                    {
                        //this.mEntity.cellCall("eatSnowBlock", bBeingEntity.getThisId());
                    }
                    else
                    {
                        float newRadius = UtilMath.getEatSnowNewRadiusByRadius(this.mEntity.getBallRadius());

                        this.mEntity.setBeingState(BeingState.eBSAttack);
                        this.mEntity.setBallRadius(newRadius);
                        bBeingEntity.dispose();
                    }
                }
            }
            else if (bBeingEntity.canEatOther(this.mEntity))
            {
                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log(string.Format("PlayerMainChildAttack::eatComputerBall, computer ball eat ball, MainChildThisId = {0}, ComputerBallThisId = {1}", this.mEntity.getThisId(), bBeingEntity.getThisId()), LogTypeId.eLogScene);
                }

                //this.mEntity.cellCall("eatenByComputer", bBeingEntity.getThisId());
            }
        }
    }
}
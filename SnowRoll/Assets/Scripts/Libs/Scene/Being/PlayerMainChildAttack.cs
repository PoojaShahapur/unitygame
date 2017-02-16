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
            //if (EntityType.ePlayerMainChild == bBeingEntity.getEntityType())
            //{
            //    if (this.mEntity.canMerge() && bBeingEntity.canMerge())
            //    {
            //        (this.mEntity as PlayerMainChild).mParentPlayer.mPlayerSplitMerge.addMerge(this.mEntity as PlayerChild, bBeingEntity as PlayerChild);
            //    }
            //}
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
                this.eatPlayerMainChild(bBeingEntity, collisionInfo);
            }
            else if (EntityType.ePlayerSnowBlock == bBeingEntity.getEntityType())
            {
                this.eatPlayerSnowBlock(bBeingEntity);
            }
            else if (EntityType.eComputerBall == bBeingEntity.getEntityType())
            {
                this.eatComputerBall(bBeingEntity);
            }
        }

        override public void overlapToExit(BeingEntity bBeingEntity, UnityEngine.Collision collisionInfo)
        {
            // 如果和 PlayerMainChild 碰撞
            if (EntityType.ePlayerMainChild == bBeingEntity.getEntityType())
            {
                //(this.mEntity as PlayerMainChild).mParentPlayer.mPlayerSplitMerge.removeMerge(this.mEntity as PlayerChild, bBeingEntity as PlayerChild);
                //this.mEntity.clearContactNotMerge();
                //bBeingEntity.clearContactNotMerge();

                //this.mEntity.setFreezeXZ(false);
                //bBeingEntity.setFreezeXZ(false);
            }
        }

        // 雪块
        public void eatSnowBlock(BeingEntity bBeingEntity)
        {
            // 因为碰撞后，还需要判断距离接近到一定程度才吃掉雪球，因此不能直接关闭碰撞
            //this.mEntity.setFreezeXZ(true);
            //UtilApi.enableCollider<UnityEngine.SphereCollider>(bBeingEntity.getGameObject(), false);

            if (this.mEntity.canEatOther(bBeingEntity))
            {
                if (!(bBeingEntity as BeingEntity).getIsEatedByOther())
                {
                    if (MacroDef.ENABLE_LOG)
                    {
                        Ctx.mInstance.mLogSys.log(string.Format("PlayerMainChildAttack::eatSnowBlock, MainChildThisId = {0}, SnowBallThisId = {1}", this.mEntity.getThisId(), bBeingEntity.getThisId()), LogTypeId.eLogScene);
                    }

                    //bBeingEntity.setClientDispose(true);
                    (bBeingEntity as BeingEntity).setIsEatedByOther(true);
                    (bBeingEntity as BeingEntity).forceHide();  // 客户端自己隐藏
                    //Ctx.mInstance.mPlayerMgr.eatSnowing(bBeingEntity.getThisId(), this.mEntity.getThisId());

                    if (!MacroDef.DEBUG_NOTNET)
                    {
                        this.mEntity.cellCall("eatSnowBlock", bBeingEntity.getThisId());
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
        }

        // 玩家之间互吃
        public void eatPlayerOther(BeingEntity bBeingEntity)
        {
            byte otherIsGod = (byte)bBeingEntity.getEntity().getDefinedProperty("isGod");

            if (this.mEntity.canEatOther(bBeingEntity) && 0 == otherIsGod)
            {
                bBeingEntity.setClientDispose(true);
                this.mEntity.cellCall("eatSnowBlock", bBeingEntity.getThisId());
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

            //this.mEntity.setFreezeXZ(true);
            //UtilApi.enableCollider<UnityEngine.SphereCollider>(bBeingEntity.getGameObject(), false);

            if (this.mEntity.canEatOther(bBeingEntity) && 0 == otherIsGod)
            {
                if (!(bBeingEntity as PlayerChild).getIsEatedByOther())
                {
                    if (MacroDef.ENABLE_LOG)
                    {
                        Ctx.mInstance.mLogSys.log(string.Format("PlayerMainChildAttack::eatPlayerOtherChild, MainChildThisId = {0}, OtherChildThisId = {1}", this.mEntity.getThisId(), bBeingEntity.getThisId()), LogTypeId.eLogScene);
                    }

                    //bBeingEntity.setClientDispose(true);
                    (bBeingEntity as PlayerChild).setIsEatedByOther(true);
                    //Ctx.mInstance.mPlayerMgr.eatOtherChilding(bBeingEntity.getThisId(), this.mEntity.getThisId());

                    if (!MacroDef.DEBUG_NOTNET)
                    {
                        this.mEntity.cellCall("eatSnowBlock", bBeingEntity.getThisId());
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
            else if(bBeingEntity.canEatOther(this.mEntity))
            {
                //bBeingEntity.setClientDispose(true);
                (this.mEntity as PlayerChild).setIsEatedByOther(true);
            }
        }

        // 吃玩家吐出的雪块
        public void eatPlayerSnowBlock(BeingEntity bBeingEntity)
        {
            //id为0说明是客户端自己生成的雪块，不处理
            //if(0 == bBeingEntity.getThisId())
            //{
            //    return;
            //}
            //bBeingEntity.setClientDispose(true);
            //Ctx.mInstance.mPlayerMgr.eatPlayerSnowing(bBeingEntity.getThisId(), this.mEntity.getThisId());
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
                        this.mEntity.cellCall("eatSnowBlock", bBeingEntity.getThisId());
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
            else if((bBeingEntity as BeingEntity).getIsEatedByServer())
            {
                (bBeingEntity as BeingEntity).setIsEatedByServer(false);
                this.mEntity.cellCall("eatSnowBlock", bBeingEntity.getThisId());
            }
        }

        // 碰撞 PlayerMainChild
        public void eatPlayerMainChild(BeingEntity bBeingEntity, UnityEngine.Collision collisionInfo)
        {
            if (MacroDef.ENABLE_LOG)
            {
                Ctx.mInstance.mLogSys.log("PlayerMainChildAttack::eatPlayerMainChild, enter eatPlayerMainChild", LogTypeId.eLogMergeBug);
            }

            if (this.mEntity.canMerge() && bBeingEntity.canMerge())
            {
                (this.mEntity as PlayerMainChild).mParentPlayer.mPlayerSplitMerge.addMerge(this.mEntity as PlayerChild, bBeingEntity as PlayerChild);
            }

            // 如果可以合并
            //if (this.mEntity.canMerge() && bBeingEntity.canMerge())
            //{
            //    (this.mEntity as PlayerMainChild).mParentPlayer.mPlayerSplitMerge.addMerge(this.mEntity as PlayerChild, bBeingEntity as PlayerChild);

            //    //this.mEntity.setFreezeXZ(true);
            //    //bBeingEntity.setFreezeXZ(true);

            //    // 如果现在能进行合并
            //    if (this.mEntity.canMergeWithOther(bBeingEntity))
            //    {
            //        bBeingEntity.setClientDispose(true);
            //        this.mEntity.setClientDispose(true);

            //        //this.mEntity.mergeWithOther(bBeingEntity);
            //        Game.Game.ReqSceneInteractive.sendMerge(this.mEntity, bBeingEntity);
            //    }
            //}
            //else if (this.mEntity.isNeedReduceSpeed() || bBeingEntity.isNeedReduceSpeed())
            //{
            //    if (UtilMath.isBehindCollidePoint(this.mEntity.getPos(), this.mEntity.getForward(), collisionInfo))
            //    {
            //        // 需要减小速度
            //        this.mEntity.setContactNotMergeSpeed(bBeingEntity.getMoveSpeed());
            //        this.mEntity.contactWithAndFollowButNotMerge(bBeingEntity);
            //    }
            //    else
            //    {
            //        bBeingEntity.setContactNotMergeSpeed(this.mEntity.getMoveSpeed());
            //        bBeingEntity.contactWithAndFollowButNotMerge(this.mEntity);
            //    }
            //}
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
                        this.mEntity.cellCall("eatSnowBlock", bBeingEntity.getThisId());
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

                this.mEntity.cellCall("eatenByComputer", bBeingEntity.getThisId());
            }
        }
    }
}
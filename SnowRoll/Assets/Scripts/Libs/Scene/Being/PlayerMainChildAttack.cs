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
                this.eatPlayerMainChild(bBeingEntity, collisionInfo);
            }
            else if (EntityType.ePlayerSnowBlock == bBeingEntity.getEntityType())
            {
                this.eatPlayerSnowBlock(bBeingEntity);
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

                UtilApi.freezeRigidBodyXZPos(this.mEntity.getGameObject(), false);
            }
            else if (EntityType.ePlayerOtherChild == bBeingEntity.getEntityType())
            {
                UtilApi.freezeRigidBodyXZPos(this.mEntity.getGameObject(), false);
            }
            else if (EntityType.eSnowBlock == bBeingEntity.getEntityType())
            {
                UtilApi.freezeRigidBodyXZPos(this.mEntity.getGameObject(), false);
            }
        }

        // 雪块
        public void eatSnowBlock(BeingEntity bBeingEntity)
        {
            UtilApi.freezeRigidBodyXZPos(bBeingEntity.getGameObject(), true);

            if (this.mEntity.canEatOther(bBeingEntity))
            {
                bBeingEntity.setClientDispose(true);

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

            UtilApi.freezeRigidBodyXZPos(this.mEntity.getGameObject(), true);

            if (this.mEntity.canEatOther(bBeingEntity) && 0 == otherIsGod)
            {
                bBeingEntity.setClientDispose(true);

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
            else if(bBeingEntity.canEatOther(this.mEntity))
            {
                bBeingEntity.setClientDispose(true);
            }
        }

        // 吃玩家吐出的雪块
        public void eatPlayerSnowBlock(BeingEntity bBeingEntity)
        {
            bBeingEntity.setClientDispose(true);

            if (!MacroDef.DEBUG_NOTNET)
            {
                this.mEntity.cellCall("eatSnowBlock", bBeingEntity.getThisId());
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

                UtilApi.freezeRigidBodyXZPos(this.mEntity.getGameObject(), true);

                // 如果现在能进行合并
                if (this.mEntity.canMergeWithOther(bBeingEntity))
                {
                    bBeingEntity.setClientDispose(true);
                    this.mEntity.setClientDispose(true);

                    //this.mEntity.mergeWithOther(bBeingEntity);
                    Game.Game.ReqSceneInteractive.sendMerge(this.mEntity, bBeingEntity);
                }
            }
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
    }
}
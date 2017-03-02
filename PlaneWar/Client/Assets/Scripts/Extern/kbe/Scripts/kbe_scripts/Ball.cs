using SDK.Lib;

namespace KBEngine
{
    /**
     * @brief 场景中的一个玩家分裂
     */
    public class Ball : KBEngine.GameObject
    {
        protected System.Int32 mOwnerId;

        public Ball()
        {

        }

        public override void __init__()
        {
            mOwnerId = (System.Int32)this.getDefinedProperty("ownereid");

            if (null == this.mEntity_SDK)
            {
                SDK.Lib.Player player = SDK.Lib.Ctx.mInstance.mPlayerMgr.getEntityByThisId((uint)mOwnerId) as SDK.Lib.Player;

                object uniqueIdObj = this.getDefinedProperty("uniqueid");
                ulong uniqueId = 0;
                PlayerChild childParent = null; // 谁分裂当前的 Ball
                if(null != uniqueIdObj)
                {
                    uniqueId = (ulong)uniqueIdObj;
                    childParent = Ctx.mInstance.mPlayerMgr.getChildByThisId((uint)uniqueId); // 谁分裂当前的 Ball
                }

                // 只有主角才有 Avatar ，其它玩家是没有 Avatar ，需要自己创建 Avatar
                if (this.isOwnerPlayer())
                {
                    if (null == player)
                    {
                        if (MacroDef.ENABLE_LOG)
                        {
                            Ctx.mInstance.mLogSys.log("Ball::init, can not find player main", LogTypeId.eLogSceneInterActive);
                        }
                    }
                    else
                    {
                        this.mEntity_SDK = new SDK.Lib.PlayerMainChild(player);
                        (this.mEntity_SDK as BeingEntity).addParentOrientChangedhandle();

                        if (MacroDef.ENABLE_LOG)
                        {
                            Ctx.mInstance.mLogSys.log(string.Format("Ball::init, PlayerMainChild Created, eid = {0} ownerId = {1}", this.id, mOwnerId), LogTypeId.eLogSceneInterActive);
                        }
                    }
                }
                else
                {
                    if (null == player)
                    {
                        if (MacroDef.ENABLE_LOG)
                        {
                            Ctx.mInstance.mLogSys.log("Ball::init, can not find player other", LogTypeId.eLogSceneInterActive);
                        }

                        // 自己创建一个默认的
                        player = new PlayerOther();
                        player.setThisId((uint)mOwnerId);
                        player.init();
                    }

                    this.mEntity_SDK = new SDK.Lib.PlayerOtherChild(player);

                    if (MacroDef.ENABLE_LOG)
                    {
                        Ctx.mInstance.mLogSys.log(string.Format("Ball::init, PlayerOtherChild Created, eid = {0} ownerId = {1}", this.id, mOwnerId), LogTypeId.eLogSceneInterActive);
                    }
                }

                this.mEntity_SDK.setEntity_KBE(this);
                this.mEntity_SDK.setThisId((uint)this.id);

                if(null != childParent)
                {
                    (this.mEntity_SDK as BeingEntity).setPrefabPath(childParent.getPrefabPath());
                }

                this.mEntity_SDK.init();

                UnityEngine.Vector3 fromPos = (UnityEngine.Vector3)this.getDefinedProperty("frompos");
                UnityEngine.Vector3 toPos = (UnityEngine.Vector3)this.getDefinedProperty("topos");

                if (UnityEngine.Vector3.zero != fromPos)
                {
                    (this.mEntity_SDK as PlayerChild).setPos(fromPos);
                    (this.mEntity_SDK as PlayerChild).setDestPosForBirth(toPos, false);
                }
                else
                {
                    this.mEntity_SDK.setPos(this.position);

                    int childnum = player.mPlayerSplitMerge.mPlayerChildMgr.getEntityCount();
                    if (1 == childnum && this.isOwnerPlayer())
                    {
                        // 1个说明刚出生，此时方向向前
                        Ctx.mInstance.mCommonData.resetEulerAngles_xy();
                        player.setDestRotate(UnityEngine.Vector3.zero, true);
                    }
                }

                (this.mEntity_SDK as SDK.Lib.BeingEntity).setRotateEulerAngle(this.direction);

                float radius = (float)getDefinedProperty("radius");
                (this.mEntity_SDK as BeingEntity).setMass(radius);

                // 如果是自己分裂的 Ball ，需要出发事件
                if (null != player && EntityType.ePlayerMain == player.getEntityType())
                {
                    (player as PlayerMain).onChildChanged();

                    //if (Ctx.mInstance.mPlayerMgr.getHero().mPlayerSplitMerge.mPlayerChildMgr.getEntityCount() > 1)
                    //{
                    //    if (Ctx.mInstance.mCommonData.isMoveToCenter())
                    //    {
                    //        //(this.mEntity_SDK as SDK.Lib.BeingEntity).moveToCenter();
                    //        // 统一修改一次目标点
                    //        //Ctx.mInstance.mPlayerMgr.getHero().mPlayerSplitMerge.moveToCenter();
                    //    }
                    //}
                }
            }
        }

        public override void onDestroy()
        {
            if (isOwnerPlayer())
            {
                KBEngine.Event.deregisterIn(this);
            }

            if (null != mEntity_SDK)
            {
                if (BeingSubState.eBSSMerge != (this.mEntity_SDK as BeingEntity).getBeingSubState())
                {
                    this.mEntity_SDK.dispose();
                    this.mEntity_SDK = null;
                }
            }
        }

        public override void SetPosition(object old)
        {
            base.SetPosition(old);

            if (null != mEntity_SDK)
            {
                this.mEntity_SDK.setPos(this.position);
            }
        }

        public override void SetDirection(object old)
        {
            base.SetDirection(old);

            if (null != mEntity_SDK)
            {
                this.mEntity_SDK.setRotateEulerAngle(this.direction);
            }
        }

        public virtual void updatePlayer(float x, float y, float z, float yaw)
        {
            position.x = x;
            position.y = y;
            position.z = z;

            direction.z = yaw;
        }

        // 拥有者是否是 MainPlayer
        public bool isOwnerPlayer()
        {
            bool ret = false;
            if (mOwnerId == KBEngineApp.app.entity_id)
            {
                ret = true;
            }

            return ret;
        }

        // 设置拥有者 Id
        //public void set_ownereid(System.Int32 ownerId)
        //{

        //}

        // 设置分裂开始位置
        //public void set_frompos(UnityEngine.Vector3 pos)
        //{
        //    // 只有分裂的时候，这个值才会有，如果出生的时候，这个值不会有
        //    if(pos != UnityEngine.Vector3.zero)
        //    {
        //        UnityEngine.Vector3 initPos = (UnityEngine.Vector3)this.getDefinedProperty("frompos");
        //        UnityEngine.Vector3 toPos = (UnityEngine.Vector3)this.getDefinedProperty("topos");

        //        (this.mEntity_SDK as PlayerChild).setPos(initPos);
        //        (this.mEntity_SDK as PlayerChild).setDestPosForBirth(toPos, false);
        //    }
        //}
        
        public override void onEnterWorld()
        {
            base.onEnterWorld();
        }

        public override void onLeaveWorld()
        {
            base.onLeaveWorld();
            
            // 如果在融合中，不能删除
            if (null != this.mEntity_SDK)
            {
                if (BeingSubState.eBSSMerge != (this.mEntity_SDK as BeingEntity).getBeingSubState())
                {
                    //if (!isOwnerPlayer())
                    //{
                    //    SDK.Lib.Ctx.mInstance.mPlayerMgr.eatOtherChilded(this.mEntity_SDK.getThisId());
                    //}

                    //嘲讽
                    if (isOwnerPlayer() && null != Ctx.mInstance.mPlayerMgr && null != Ctx.mInstance.mPlayerMgr.getHero()
                       && Ctx.mInstance.mPlayerMgr.getHero().mPlayerSplitMerge.mPlayerChildMgr.getEntityCount() == 1)
                    {
                        Ctx.mInstance.mLuaSystem.receiveToLua_KBE("ShowEmoticon", null);
                    }

                    this.mEntity_SDK.dispose();
                    this.mEntity_SDK = null;
                }
            }
        }

        // 自己的 Ball 大小改变的时候，需要更新摄像机
        override public void set_radius(float radius)
        {
            base.set_radius(radius);

            if (isOwnerPlayer())
            {
                SDK.Lib.Player player = SDK.Lib.Ctx.mInstance.mPlayerMgr.getEntityByThisId((uint)mOwnerId) as SDK.Lib.Player;
                (player as PlayerMain).onChildChanged();
            }
        }

        public override void set_name(object old)
        {
            string name = getDefinedProperty("name") as string;
            (this.mEntity_SDK as BeingEntity).setName(name);
        }

        public override void server_set_position(object old)
        {
            /*
            base.server_set_position(old);

            if (isOwnerPlayer())
            {
                KBEngineApp.app.entityServerPos(position);
                Ctx.mInstance.mPlayerMgr.getHero().onChildChanged();
            }
            */
        }
    }
}
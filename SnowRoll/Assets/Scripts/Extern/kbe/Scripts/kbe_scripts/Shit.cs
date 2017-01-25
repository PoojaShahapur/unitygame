using SDK.Lib;

namespace KBEngine
{
    /**
     * @brief 吐出的雪球
     */
    public class Shit : KBEngine.GameObject
    {
        public Shit()
        {

        }

        public override void __init__()
        {
            if (null == this.mEntity_SDK)
            {
                UnityEngine.Vector3 frompos = (UnityEngine.Vector3)this.getDefinedProperty("frompos");
                UnityEngine.Vector3 topos = (UnityEngine.Vector3)this.getDefinedProperty("topos");

                object uniqueIdObj = this.getDefinedProperty("uniqueid");
                ulong uniqueId = 0;
                uint aId = 0;
                uint bId = 0;
                if (null != uniqueIdObj)
                {
                    uniqueId = (ulong)uniqueIdObj;
                    UtilMath.getSingleId(uniqueId, ref aId, ref bId);
                }

                if (!Ctx.mInstance.mPlayerSnowBlockMgr.isExistNumId(aId))
                {
                    Ctx.mInstance.mLogSys.log("Shit::__init__, Shit not find, create new", LogTypeId.eLogSceneInterActive);

                    this.mEntity_SDK = new PlayerSnowBlock();
                    this.mEntity_SDK.setRotateEulerAngle(this.direction);
                    this.mEntity_SDK.setPos(frompos);
                    (this.mEntity_SDK as BeingEntity).setDestPos(topos, false);
                    (this.mEntity_SDK as BeingEntity).setEntity_KBE(this);
                    this.mEntity_SDK.setThisId((uint)this.id);
                    this.mEntity_SDK.init();

                    Ctx.mInstance.mPlayerSnowBlockMgr.addEntity(this.mEntity_SDK);
                }
                else
                {
                    Ctx.mInstance.mLogSys.log("Shit::__init__, Shit find, not create new", LogTypeId.eLogSceneInterActive);

                    this.mEntity_SDK = Ctx.mInstance.mPlayerSnowBlockMgr.getEntityByUniqueId(Ctx.mInstance.mPlayerSnowBlockMgr.genStrIdById(bId));
                    uint preThisId = this.mEntity_SDK.getThisId();
                    this.mEntity_SDK.setThisId((uint)this.id);
                    Ctx.mInstance.mPlayerSnowBlockMgr.changeThisId(preThisId, this.mEntity_SDK as PlayerSnowBlock);
                }

                Ctx.mInstance.mPlayerMgr.getHero().onChildChanged();

                Ctx.mInstance.mLogSys.log(string.Format("Shit::__init__, Shit Created, eid = {0} uniqueId = {1}", this.id, uniqueId), LogTypeId.eLogSceneInterActive);
            }
        }

        public override void onDestroy()
        {
            if (isPlayer())
            {
                KBEngine.Event.deregisterIn(this);
            }

            if (null != mEntity_SDK)
            {
                mEntity_SDK.dispose();
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

        public override void onEnterWorld()
        {
            base.onEnterWorld();
        }

        public override void onLeaveWorld()
        {
            base.onLeaveWorld();

            if (null != this.mEntity_SDK)
            {
                //SDK.Lib.Ctx.mInstance.mPlayerMgr.eatPlayerSnowed(this.mEntity_SDK.getThisId());
                this.mEntity_SDK.dispose();
                this.mEntity_SDK = null;
            }
        }
    }
}
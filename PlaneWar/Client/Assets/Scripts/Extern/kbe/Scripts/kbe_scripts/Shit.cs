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
                this.emitSnowBlock();
            }
        }

        protected void emitSnowBlock()
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

            if (frompos != UnityEngine.Vector3.zero)
            {
                Ctx.mInstance.mSnowBallCfg.getShitPos(aId, ref frompos, ref topos);
            }

            //别人吐出的雪块
            if (MacroDef.ENABLE_LOG)
            {
                Ctx.mInstance.mLogSys.log("Shit::emitSnowBlock, Shit not find, create new", LogTypeId.eLogSceneInterActive);
            }

            this.mEntity_SDK = new PlayerSnowBlock();
            this.mEntity_SDK.setRotateEulerAngle(this.direction);

            if (frompos == UnityEngine.Vector3.zero)    // 进出九屏 frompos 为零
            {
                this.mEntity_SDK.setPos(this.position);
            }
            else    // 第一次吐 frompos 为不为零
            {
                this.mEntity_SDK.setPos(frompos);
                (this.mEntity_SDK as BeingEntity).setDestPos(topos, false);
            }

            (this.mEntity_SDK as BeingEntity).setEntity_KBE(this);
            this.mEntity_SDK.setThisId((uint)this.id);
            this.mEntity_SDK.init();

            Ctx.mInstance.mPlayerSnowBlockMgr.addPlayerSnowBlock(this.mEntity_SDK as PlayerSnowBlock);

            if (Ctx.mInstance.mPlayerMgr.isHeroChildByThisId(aId))
            {
                Ctx.mInstance.mPlayerMgr.getHero().onChildChanged();
            }

            if (MacroDef.ENABLE_LOG)
            {
                Ctx.mInstance.mLogSys.log(string.Format("Shit::emitSnowBlock, Shit Created, eid = {0} uniqueId = {1}", this.id, uniqueId), LogTypeId.eLogSceneInterActive);
            }
        }

        public override void onDestroy()
        {
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
                this.mEntity_SDK.dispose();
                this.mEntity_SDK = null;
            }
        }
    }
}
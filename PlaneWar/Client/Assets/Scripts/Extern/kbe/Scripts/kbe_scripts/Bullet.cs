using SDK.Lib;

namespace KBEngine
{
    /**
     * @brief 发射的子弹
     */
    public class Bullet : KBEngine.GameObject
    {
        protected System.Int32 mOwnerId;

        public Bullet()
        {

        }

        public override void __init__()
        {
            if (null == this.mEntity_SDK)
            {
                this.emitFlyBullet();
            }
        }

        protected void emitFlyBullet()
        {
            this.mOwnerId = (System.Int32)this.getDefinedProperty("ownereid");

            this.mEntity_SDK = new FlyBullet();
            (this.mEntity_SDK as FlyBullet).setOwnerThisId((uint)this.mOwnerId);

            UnityEngine.Vector3 euler = UtilApi.invConvRotByMode(UtilMath.getRotateByOrient(UtilApi.convRotByMode(this.direction)).eulerAngles);
            this.mEntity_SDK.setRotateEulerAngle_FromKBE(euler);

            this.mEntity_SDK.setPos_FromKBE(this.position);

           (this.mEntity_SDK as BeingEntity).setEntity_KBE(this);
            this.mEntity_SDK.setThisId((uint)this.id);
            this.mEntity_SDK.init();

            if (MacroDef.ENABLE_LOG)
            {
                Ctx.mInstance.mLogSys.log(string.Format("Bullet::emitFlyBullet, Shit Created, eid = {0} ownereid = {1}", this.id, this.mOwnerId), LogTypeId.eLogSceneInterActive);
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
                this.mEntity_SDK.setPos_FromKBE(this.position);
            }
        }

        public override void SetDirection(object old)
        {
            base.SetDirection(old);

            if (null != mEntity_SDK)
            {
                this.mEntity_SDK.setRotateEulerAngle_FromKBE(this.direction);
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

        public override void set_movespeed(float movespeed)
        {
            (this.mEntity_SDK as BeingEntity).setMoveSpeed(movespeed);
        }
    }
}
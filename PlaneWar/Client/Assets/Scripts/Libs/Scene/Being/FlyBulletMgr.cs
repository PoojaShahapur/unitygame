namespace SDK.Lib
{
    /**
    * @brief FlyBullet 管理器
    */
    public class FlyBulletMgr : EntityMgrBase
    {
        public FlyBulletMgr()
        {
            mUniqueStrIdGen = new UniqueStrIdGen(UniqueStrIdGen.PlayerSnowBlockPrefix, 0);
        }

        override protected void onTickExec(float delta)
        {
            base.onTickExec(delta);
        }

        override public void init()
        {
            base.init();
        }

        public override void dispose()
        {
            base.dispose();
        }

        public void addFlyBullet(FlyBullet flyBullet)
        {
            this.addEntity(flyBullet);
        }

        public void removeFlyBullet(FlyBullet flyBullet)
        {
            this.removeEntity(flyBullet);
        }

        // 发射出一个 FlyBullet
        public void emitOne(UnityEngine.Vector3 srcPos, UnityEngine.Vector3 destPos, UnityEngine.Quaternion rot)
        {
            FlyBullet flyBullet = new FlyBullet();
            flyBullet.init();

            flyBullet.setDestRotate(rot.eulerAngles, true);
            flyBullet.setPos(srcPos);
            flyBullet.setDestPos(destPos, false);
        }
    }
}
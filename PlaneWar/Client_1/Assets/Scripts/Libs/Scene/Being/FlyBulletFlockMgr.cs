namespace SDK.Lib
{
    public class FlyBulletFlockMgr : TickObjectNoPriorityMgr, IPriorityObject
    {
        protected MDictionary<uint, FlyBulletFlock> mId2EntityDic;

        public FlyBulletFlockMgr()
        {
            this.mId2EntityDic = new MDictionary<uint, FlyBulletFlock>();
        }

        override public void init()
        {
            base.init();
        }

        override public void dispose()
        {
            base.dispose();
        }

        public void addBulletFlock(ITickedObject tickObj, float priority = 0.0f)
        {
            this.addObject(tickObj as IDelayHandleItem, priority);
            this.mId2EntityDic.Add((tickObj as FlyBulletFlock).getThisId(), tickObj as FlyBulletFlock);
        }

        public void removeBulletFlock(ITickedObject tickObj)
        {
            this.removeObject(tickObj as IDelayHandleItem);
            this.mId2EntityDic.Remove((tickObj as FlyBulletFlock).getThisId());
        }

        virtual public FlyBulletFlock getBulletFlockByThisId(uint thisId)
        {
            FlyBulletFlock obj = null;

            if(this.mId2EntityDic.ContainsKey(thisId))
            {
                obj = this.mId2EntityDic.value(thisId);
            }

            return obj;
        }
    }
}
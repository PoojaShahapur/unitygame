using SDK.Common;

namespace SDK.Lib
{
	/**
	 * @brief 掉落物管理器   
	 */
    public class FObjectMgr : EntityMgrBase
	{
        public FObjectMgr()
		{

		}

        override protected void onTickExec(float delta)
        {
            base.onTickExec(delta);
        }

        public void addFObject(BeingEntity being)
        {
            this.addObject(being);
        }

        public void removeFObject(BeingEntity being)
        {
            this.delObject(being);
        }
	}
}
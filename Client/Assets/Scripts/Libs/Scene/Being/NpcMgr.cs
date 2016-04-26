namespace SDK.Lib
{
	/**
	 * @brief 所有的 npc 
	 */
    public class NpcMgr : EntityMgrBase
	{
        public NpcMgr() 
		{

		}

        override protected void onTickExec(float delta)
        {
            base.onTickExec(delta);
        }

        public void addNpc(BeingEntity being)
        {
            this.addObject(being);
        }

        public void removeNpc(BeingEntity being)
        {
            this.removeObject(being);
        }
	}
}
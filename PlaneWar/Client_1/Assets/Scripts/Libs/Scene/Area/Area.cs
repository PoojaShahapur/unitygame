namespace SDK.Lib
{
    /**
     * @brief 逻辑一屏
     */
    public class Area : EntityMgrBase
    {
        public Area()
        {
            init();
        }

        override public void init()
        {
            base.init();
        }

        override public void addEntity(SceneEntityBase entity)
        {
            this.addEntity(entity);
            entity.setArea(this);
        }

        public void clearArea()
        {
            int idx = 0;
            int len = mSceneEntityList.Count();
            while(idx < len)
            {
                mSceneEntityList[idx].dispose();
                ++idx;
            }
        }
    }
}
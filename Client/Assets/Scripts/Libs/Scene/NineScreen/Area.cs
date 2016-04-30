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

        public void init()
        {
            
        }

        override public void addEntity(SceneEntityBase entity)
        {
            this.addEntity(entity);
            entity.setArea(this);
        }

        public void clearArea()
        {
            int idx = 0;
            int len = m_sceneEntityList.Count();
            while(idx < len)
            {
                m_sceneEntityList[idx].dispose();
                ++idx;
            }
        }
    }
}
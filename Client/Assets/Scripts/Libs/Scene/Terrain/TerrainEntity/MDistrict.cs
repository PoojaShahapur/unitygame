namespace SDK.Lib
{
    /**
     * @brief 场景区域
     */
    public class MDistrict
    {
        protected MList<SceneEntityBase> mEntityList;   // Entity 列表

        public MDistrict()
        {
            init();
        }

        public void init()
        {
            mEntityList = new MList<SceneEntityBase>();
        }

        // 添加一个 Entity
        public void addEntity(SceneEntityBase entity)
        {
            if(mEntityList.IndexOf(entity) == -1)
            {
                mEntityList.Add(entity);
            }
            else
            {
                Ctx.m_instance.m_logSys.log("SceneEntityBase already Exist", LogTypeId.eLogMSceneManager);
            }
        }

        // 移除一个 Entity
        public void removeEntity(SceneEntityBase entity)
        {
            if (mEntityList.IndexOf(entity) != -1)
            {
                mEntityList.Remove(entity);
            }
            else
            {
                Ctx.m_instance.m_logSys.log("SceneEntityBase not Exist", LogTypeId.eLogMSceneManager);
            }
        }
    }
}
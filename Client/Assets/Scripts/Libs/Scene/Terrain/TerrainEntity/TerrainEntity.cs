namespace SDK.Lib
{
    /**
     * @brief 场景实体
     */
    public class TerrainEntity
    {
        protected MDistrict[] mDistrictArr;     // 地形区域列表

        public TerrainEntity()
        {
            init();
        }

        public void init()
        {
            int size = Ctx.m_instance.mTerrainGlobalOption.getTreeNodeSize();
            mDistrictArr = new MDistrict[size * size];
        }

        public MDistrict getTerrainDistrictByPos(float posX, float posZ)
        {
            int idx = UtilMath.floorToInt(posX / Ctx.m_instance.mTerrainGlobalOption.mTerrainWorldSize);
            int idy = UtilMath.floorToInt(posX / Ctx.m_instance.mTerrainGlobalOption.mTerrainWorldSize);
            int size = Ctx.m_instance.mTerrainGlobalOption.getTreeNodeSize();
            int lineIndex = idy * size + idx;

            if (mDistrictArr[lineIndex] == null)
            {
                mDistrictArr[lineIndex] = new MDistrict();
            }

            return mDistrictArr[lineIndex];
        }

        // 添加一个 Entity
        public void addEntity(SceneEntityBase entity)
        {
            MDistrict district = getTerrainDistrictByPos(entity.getWorldPosX(), entity.getWorldPosZ());
            if (district != null)
            {
                district.addEntity(entity);
            }
            else
            {
                Ctx.m_instance.m_logSys.log("District is Null", LogTypeId.eLogMSceneManager);
            }
        }

        // 移除一个 Entity
        public void removeEntity(SceneEntityBase entity)
        {
            MDistrict district = getTerrainDistrictByPos(entity.getWorldPosX(), entity.getWorldPosZ());
            if (district != null)
            {
                district.removeEntity(entity);
            }
            else
            {
                Ctx.m_instance.m_logSys.log("District is Null", LogTypeId.eLogMSceneManager);
            }
        }
    }
}
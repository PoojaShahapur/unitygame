namespace SDK.Lib
{
    /**
     * @brief 场景实体
     */
    public class TerrainEntity
    {
        protected int mPosX;
        protected int mPosY;

        protected MDistrict[] mDistrictArr;     // 地形区域列表

        public TerrainEntity(int posX, int posY)
        {
            mPosX = posX;
            mPosY = posY;

            init();
        }

        public void init()
        {
            int size = Ctx.m_instance.mTerrainGlobalOption.getTreeNodeCount();
            mDistrictArr = new MDistrict[size * size];
        }

        public int getPosX()
        {
            return mPosX;
        }

        public int getPosY()
        {
            return mPosY;
        }

        public MDistrict getTerrainDistrictByPos(float posX, float posZ)
        {
            int idx = UtilMath.floorToInt(posX % Ctx.m_instance.mTerrainGlobalOption.mTerrainWorldSize / Ctx.m_instance.mTerrainGlobalOption.getTreeNodeWorldSize());
            int idy = UtilMath.floorToInt(posX % Ctx.m_instance.mTerrainGlobalOption.mTerrainWorldSize / Ctx.m_instance.mTerrainGlobalOption.getTreeNodeWorldSize());
            int size = Ctx.m_instance.mTerrainGlobalOption.getTreeNodeCount();
            int lineIndex = idy * size + idx;

            if (mDistrictArr[lineIndex] == null)
            {
                mDistrictArr[lineIndex] = new MDistrict(this, idx, idy);
                MTerrain terrain = Ctx.m_instance.m_terrainGroup.getTerrain(mPosX, mPosY);
                MTerrainQuadTreeNode node = terrain.getTerrainQuadTreeNode(idx * Ctx.m_instance.mTerrainGlobalOption.getTreeNodeHalfSize(), idy * Ctx.m_instance.mTerrainGlobalOption.getTreeNodeHalfSize());
                mDistrictArr[lineIndex].attachToTreeNode(node);
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
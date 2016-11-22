namespace SDK.Lib
{
    /**
     * @brief 场景实体
     */
    public class TerrainEntity : GObject
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
            int size = Ctx.mInstance.mTerrainGlobalOption.getTreeNodeCount();
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
            int idx = UtilMath.floorToInt(posX % Ctx.mInstance.mTerrainGlobalOption.mTerrainWorldSize / Ctx.mInstance.mTerrainGlobalOption.getTreeNodeWorldSize());
            int idy = UtilMath.floorToInt(posX % Ctx.mInstance.mTerrainGlobalOption.mTerrainWorldSize / Ctx.mInstance.mTerrainGlobalOption.getTreeNodeWorldSize());
            int size = Ctx.mInstance.mTerrainGlobalOption.getTreeNodeCount();
            int lineIndex = idy * size + idx;

            if (mDistrictArr[lineIndex] == null)
            {
                mDistrictArr[lineIndex] = new MDistrict(this, idx, idy);
                MTerrain terrain = Ctx.mInstance.mTerrainGroup.getTerrain(mPosX, mPosY);
                MTerrainQuadTreeNode node = terrain.getTerrainQuadTreeNode(idx * Ctx.mInstance.mTerrainGlobalOption.getTreeNodeHalfSize(), idy * Ctx.mInstance.mTerrainGlobalOption.getTreeNodeHalfSize());
                mDistrictArr[lineIndex].attachToTreeNode(node);
            }

            return mDistrictArr[lineIndex];
        }

        // 添加一个 Entity
        public void addEntity(SceneEntityBase entity)
        {
            MDistrict district = getTerrainDistrictByPos(entity.getWorldPosX(), entity.getWorldPosY());
            if (district != null)
            {
                district.addEntity(entity);
            }
            else
            {
                Ctx.mInstance.mLogSys.log("District is Null", LogTypeId.eLogMSceneManager);
            }
        }

        // 移除一个 Entity
        public void removeEntity(SceneEntityBase entity)
        {
            MDistrict district = getTerrainDistrictByPos(entity.getWorldPosX(), entity.getWorldPosY());
            if (district != null)
            {
                district.removeEntity(entity);
            }
            else
            {
                Ctx.mInstance.mLogSys.log("District is Null", LogTypeId.eLogMSceneManager);
            }
        }
    }
}
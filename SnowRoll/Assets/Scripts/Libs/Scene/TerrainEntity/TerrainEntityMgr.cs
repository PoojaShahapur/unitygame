using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 地形实体管理器
     */
    public class TerrainEntityMgr : GObject
    {
        protected MDictionary<uint, TerrainEntity> mId2TerrainEntityDic;     // 地图 Id 到 TerrainEntity Dic

        public TerrainEntityMgr()
        {
            init();
        }

        public void init()
        {
            mId2TerrainEntityDic = new MDictionary<uint, TerrainEntity>();
        }

        // 添加一个 Entity
        public void addEntity(SceneEntityBase entity)
        {
            TerrainEntity terrainEntity = getTerrainEntityByPos(entity.getWorldPosX(), entity.getWorldPosY());
            if(terrainEntity != null)
            {
                terrainEntity.addEntity(entity);
            }
            else
            {
            }
        }

        // 移除一个 Entity
        public void removeEntity(SceneEntityBase entity)
        {
            TerrainEntity terrainEntity = getTerrainEntityByPos(entity.getWorldPosX(), entity.getWorldPosY());
            if (terrainEntity != null)
            {
                terrainEntity.removeEntity(entity);
            }
            else
            {
            }
        }

        public TerrainEntity getTerrainEntityByPos(float posX, float posZ)
        {
            int idx = UtilMath.floorToInt(posX / Ctx.mInstance.mTerrainGlobalOption.mTerrainWorldSize);
            int idy = UtilMath.floorToInt(posX / Ctx.mInstance.mTerrainGlobalOption.mTerrainWorldSize);
            uint key = UtilApi.packIndex(idx, idy);

            if (!mId2TerrainEntityDic.ContainsKey(key))
            {
                mId2TerrainEntityDic[key] = new TerrainEntity(idx, idy);
            }

            return mId2TerrainEntityDic[key];
        }
    }
}
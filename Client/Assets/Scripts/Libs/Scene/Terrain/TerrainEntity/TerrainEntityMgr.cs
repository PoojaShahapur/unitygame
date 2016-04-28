using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 地形实体管理器
     */
    public class TerrainEntityMgr
    {
        protected Dictionary<uint, TerrainEntity> mId2TerrainEntityDic;     // 地图 Id 到 TerrainEntity Dic

        public TerrainEntityMgr()
        {
            init();
        }

        public void init()
        {
            mId2TerrainEntityDic = new Dictionary<uint, TerrainEntity>();
        }

        // 添加一个 Entity
        public void addEntity(SceneEntityBase entity)
        {
            TerrainEntity terrainEntity = getTerrainEntityByPos(entity.getWorldPosX(), entity.getWorldPosZ());
            if(terrainEntity != null)
            {
                terrainEntity.addEntity(entity);
            }
            else
            {
                Ctx.m_instance.m_logSys.log("TerrainEntity is Null", LogTypeId.eLogMSceneManager);
            }
        }

        // 移除一个 Entity
        public void removeEntity(SceneEntityBase entity)
        {
            TerrainEntity terrainEntity = getTerrainEntityByPos(entity.getWorldPosX(), entity.getWorldPosZ());
            if (terrainEntity != null)
            {
                terrainEntity.removeEntity(entity);
            }
            else
            {
                Ctx.m_instance.m_logSys.log("TerrainEntity is Null", LogTypeId.eLogMSceneManager);
            }
        }

        public TerrainEntity getTerrainEntityByPos(float posX, float posZ)
        {
            int idx = UtilMath.floorToInt(posX / Ctx.m_instance.mTerrainGlobalOption.mTerrainWorldSize);
            int idy = UtilMath.floorToInt(posX / Ctx.m_instance.mTerrainGlobalOption.mTerrainWorldSize);
            uint key = UtilApi.packIndex(idx, idy);

            if (!mId2TerrainEntityDic.ContainsKey(key))
            {
                mId2TerrainEntityDic[key] = new TerrainEntity(idx, idy);
            }

            return mId2TerrainEntityDic[key];
        }
    }
}
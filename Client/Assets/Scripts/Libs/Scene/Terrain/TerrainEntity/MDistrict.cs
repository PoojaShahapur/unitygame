using System;

namespace SDK.Lib
{
    /**
     * @brief 场景区域
     */
    public class MDistrict
    {
        protected int mPosX;
        protected int mPosY;
        protected TerrainEntity mTerrainEntity;
        protected MTerrainQuadTreeNode mTreeNode;
        protected bool mIsVisible;    // 是否可视

        protected MList<SceneEntityBase> mEntityList;   // Entity 列表

        public MDistrict(TerrainEntity terrainEntity, int posX, int posY)
        {
            mTerrainEntity = terrainEntity;
            mPosX = posX;
            mPosY = posY;

            init();
        }

        public void init()
        {
            mIsVisible = false;
            mEntityList = new MList<SceneEntityBase>();
        }

        // 添加一个 Entity
        public void addEntity(SceneEntityBase entity)
        {
            if(mEntityList.IndexOf(entity) == -1)
            {
                mEntityList.Add(entity);
                if(mIsVisible)
                {
                    entity.show();
                }
                else
                {
                    entity.hide();
                }
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

        public void attachToTreeNode(MTerrainQuadTreeNode node)
        {
            mTreeNode = node;
            mTreeNode.getTreeNodeStateNotify().addShowEventHandle(onTerrainNodeShow);
            mTreeNode.getTreeNodeStateNotify().addHideEventHandle(onTerrainNodeShow);
            mIsVisible = mTreeNode.isSceneGraphVisible();
        }

        // 一个地形节点显示
        public void onTerrainNodeShow(IDispatchObject treeNode)
        {
            mIsVisible = true;
        }

        // 一个地形节点隐藏
        public void onTerrainNodeHide(IDispatchObject treeNode)
        {
            mIsVisible = false;
        }
    }
}
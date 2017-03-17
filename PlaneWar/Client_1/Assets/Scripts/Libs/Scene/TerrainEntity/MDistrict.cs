using System;

namespace SDK.Lib
{
    /**
     * @brief 场景区域
     */
    public class MDistrict : EntityMgrBase
    {
        protected int mPosX;
        protected int mPosY;
        protected TerrainEntity mTerrainEntity;
        protected MTerrainQuadTreeNode mTreeNode;
        protected bool mIsVisible;    // 是否可视

        public MDistrict(TerrainEntity terrainEntity, int posX, int posY)
        {
            mTerrainEntity = terrainEntity;
            mPosX = posX;
            mPosY = posY;

            init();
        }

        override public void init()
        {
            mIsVisible = false;
        }

        // 添加一个 Entity
        override public void addEntity(SceneEntityBase entity)
        {
            base.addEntity(entity);

            entity.setDistrict(this);
            if (mIsVisible)
            {
                entity.show();
            }
            else
            {
                entity.hide();
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
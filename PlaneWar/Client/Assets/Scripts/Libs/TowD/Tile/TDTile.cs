namespace SDK.Lib
{
    /**
     * @brief 逻辑 Tile
     */
    public class TDTile
    {
        protected TileEntityMgr mEntityMgr;     // 保存 Tile 内的所有 Entity
        protected int mTileindex;       // Tile 索引
        protected bool mIsVisible;      // 是否可见
        protected bool mIsFullVisible;  // 是否完全可见

        public TDTile()
        {
            this.init();
        }

        public void init()
        {
            this.mIsVisible = false;
            this.mIsFullVisible = false;

            this.mEntityMgr = new TileEntityMgr();
            this.mEntityMgr.init();
        }

        public void dispose()
        {
            this.mEntityMgr.dispose();
        }

        public void setTileIndex(int index)
        {
            this.mTileindex = index;
        }

        public int getTileIndex()
        {
            return this.mTileindex;
        }

        public void setIsVisible(bool value)
        {
            this.mIsVisible = value;
        }

        public bool isVisible()
        {
            return this.mIsVisible;
        }

        public void setIsFullVisible(bool value)
        {
            this.mIsFullVisible = value;
        }

        public bool isFullVisible()
        {
            return this.mIsFullVisible;
        }

        // 显示
        public void show()
        {
            if (!this.mIsVisible)
            {
                this.mIsVisible = true;
                //this.updateShow();
                this.updateVisible();
            }
        }

        // 隐藏
        public void hide()
        {
            if (this.mIsVisible)
            {
                this.mIsVisible = false;
                this.updateHide();
            }
        }

        // 更新显示
        public void updateShow()
        {
            int index = 0;
            int len = this.mEntityMgr.getEntityCount();

            SceneEntityBase entity = null;

            while (index < len)
            {
                entity = this.mEntityMgr.getEntityByIndex(index);

                if (entity.isEnableVisible())
                {
                    if (entity.IsVisible())
                    {
                        entity.onEnterScreenRange();
                    }
                }

                ++index;
            }
        }

        // 更新隐藏
        public void updateHide()
        {
            int index = 0;
            int len = this.mEntityMgr.getEntityCount();

            SceneEntityBase entity = null;

            while (index < len)
            {
                entity = this.mEntityMgr.getEntityByIndex(index);

                if (entity.IsVisible())
                {
                    entity.onLeaveScreenRange();
                }

                ++index;
            }
        }

        // 更新可视化
        public void updateVisible()
        {
            int index = 0;
            int len = this.mEntityMgr.getEntityCount();

            SceneEntityBase entity = null;

            while (index < len)
            {
                entity = this.mEntityMgr.getEntityByIndex(index);

                if (Ctx.mInstance.mClipRect.isPosVisible(entity.getPos()))
                {
                    if (entity.isEnableVisible())
                    {
                        if (entity.IsVisible())
                        {
                            entity.onEnterScreenRange();
                        }
                    }
                }
                else
                {
                    if (entity.IsVisible())
                    {
                        entity.onLeaveScreenRange();
                    }
                }

                ++index;
            }
        }

        // 更新一个 Entity 位置改变
        public void updateEntity(SceneEntityBase entity)
        {
            TDTile preTile = entity.getTile();

            if (preTile != this)
            {
                if (null != preTile)
                {
                    preTile.removeEntity(entity);
                }

                this.mEntityMgr.addEntity(entity);
                entity.setTile(this);

                // 直接更新，其实可以区分 Dynamic 和 Static Entity， Dynamic 直接调用下面接口，而 Static 不用调用下面的接口，目前不区分了
                this.updateVisible(entity);
            }
        }

        public void removeEntity(SceneEntityBase entity)
        {
            if(null != entity)
            {
                this.mEntityMgr.removeEntity(entity);
            }
        }

        public void clearTile()
        {
            this.mEntityMgr.clearAll();
        }

        public void updateVisible(SceneEntityBase entity)
        {
            // Tile 可能部分或者全部可见
            if (this.mIsVisible)
            {
                if(entity.isEnableVisible())
                {
                    if(entity.IsVisible())
                    {
                        // 进一步判断是否 Entity 真的可见
                        if (Ctx.mInstance.mClipRect.isPosVisible(entity.getPos()))
                        {
                            entity.onEnterScreenRange();
                        }
                        else
                        {
                            entity.onLeaveScreenRange();
                        }
                    }
                }
            }
            else
            {
                if (entity.IsVisible())
                {
                    entity.onLeaveScreenRange();
                }
            }
        }
    }
}
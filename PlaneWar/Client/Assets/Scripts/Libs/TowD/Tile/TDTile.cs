namespace SDK.Lib
{
    /**
     * @brief 逻辑 Tile
     */
    public class TDTile
    {
        protected EntityMgrBase mEntityMgr;     // 保存 Tile 内的所有 Entity
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

            this.mEntityMgr.init();
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
            if(!this.mIsVisible)
            {
                this.mIsVisible = true;
                this.updateVisible();
            }
        }

        // 隐藏
        public void hide()
        {
            if(this.mIsVisible)
            {
                this.mIsVisible = false;
                this.updateVisible();
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
                    if (entity.isWillVisible())
                    {
                        if (!entity.IsVisible())
                        {
                            entity.show();
                        }
                    }
                }
                else
                {
                    if (entity.IsVisible())
                    {
                        entity.hide();
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

                // Tile 可能部分或者全部可见
                if(this.mIsVisible)
                {
                    if(!entity.IsVisible())
                    {
                        if(entity.isWillVisible())
                        {
                            // 进一步判断是否 Entity 真的可见
                            if(Ctx.mInstance.mClipRect.isPosVisible(entity.getPos()))
                            {
                                entity.show();
                            }
                        }
                    }
                }
                else
                {
                    if (entity.IsVisible())
                    {
                        entity.hide();
                    }
                }
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
    }
}
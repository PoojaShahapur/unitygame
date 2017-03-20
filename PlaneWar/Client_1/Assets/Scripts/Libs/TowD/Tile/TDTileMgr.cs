namespace SDK.Lib
{
    public enum VisibleTileIndex
    {
        eVTI_Pre = 0,   // 之前
        eVTI_Cur = 1,   // 当前
        eVTI_Total = 2,   // 总数
    }

    /**
     * @brief Tile 管理器，原点在左下角
     */
    public class TDTileMgr
    {
        protected int mOneTileWidth;
        protected int mOneTileDepth;
        protected int mWorldWidth;
        protected int mWorldDepth;
        protected int mTileWidthSize;
        protected int mTileDepthSize;
        protected TDTile[] mTileArray;
        protected int mTileTotal;   // 总共 Tile 数量

        protected MList<TDTile>[] mVisibleTileListArray;
        protected int mCurVislbleTileIndex;
        protected int mPreVislbleTileIndex;

        protected MList<SceneEntityBase> mDirtyEntityList;    // 位置信息更新了，但是场景还没有更新的 Entity

        public TDTileMgr()
        {
            
        }

        virtual public void init()
        {
            // 初始化之前一定要设置大小
            this.setOneTileWidthHeight(4, 4);
            //this.setWorldWidthHeight(3000, 3000);

            //this.setWorldWidthHeight((int)Ctx.mInstance.mSnowBallCfg.mXmlSnowBallCfg.mXmlItemMap.mWidth, (int)Ctx.mInstance.mSnowBallCfg.mXmlSnowBallCfg.mXmlItemMap.mWidth);

            //this.mTileWidthSize = this.mWorldWidth / this.mOneTileWidth;
            //this.mTileDepthSize = this.mWorldDepth / this.mOneTileDepth;

            //this.mTileArray = new TDTile[this.mTileWidthSize * this.mTileDepthSize];

            this.mVisibleTileListArray = new MList<TDTile>[(int)VisibleTileIndex.eVTI_Total];
            this.mVisibleTileListArray[0] = new MList<TDTile>();
            this.mVisibleTileListArray[0].setIsSpeedUpFind(true);
            this.mVisibleTileListArray[1] = new MList<TDTile>();
            this.mVisibleTileListArray[1].setIsSpeedUpFind(true);
            this.mCurVislbleTileIndex = 0;
            this.mPreVislbleTileIndex = 1;

            this.buildDirtyList();
        }

        virtual public void dispose()
        {
            //不进入游戏直接退出，mTileArray为空会报错
            if (mTileArray == null)
                return;

            int index = 0;
            int len = mTileArray.Length;

            while(index < len)
            {
                if(null != this.mTileArray[index])
                {
                    this.mTileArray[index].dispose();
                    this.mTileArray[index] = null;
                }

                ++index;
            }
        }

        public void setOneTileWidthHeight(int width, int height)
        {
            this.mOneTileWidth = width;
            this.mOneTileDepth = height;
        }

        public int getTileWidth()
        {
            return this.mOneTileWidth;
        }

        public int getTileDepth()
        {
            return this.mOneTileDepth;
        }

        public void setWorldWidthHeight(int width, int height)
        {
            this.mWorldWidth = width;
            this.mWorldDepth = height;

            this.mTileWidthSize = this.mWorldWidth / this.mOneTileWidth;
            this.mTileDepthSize = this.mWorldDepth / this.mOneTileDepth;

            if(0 != this.mWorldWidth % this.mOneTileWidth)
            {
                this.mTileWidthSize += 1;
            }
            if (0 != this.mWorldDepth % this.mOneTileDepth)
            {
                this.mTileDepthSize += 1;
            }

            this.mTileTotal = this.mTileWidthSize * this.mTileDepthSize;

            this.buildTileArray();
        }

        virtual protected void buildDirtyList()
        {
            this.mDirtyEntityList = new MList<SceneEntityBase>();
        }

        virtual protected void buildTileArray()
        {
            this.mTileArray = new TDTile[this.mTileTotal];
        }

        virtual protected TDTile createTile()
        {
            TDTile tile = new TDTile();
            return tile;
        }

        protected int checkTileXRange(int tileX)
        {
            if (tileX >= this.mTileWidthSize)
            {
                tileX = this.mTileWidthSize - 1;
            }

            if (tileX < 0)
            {
                tileX = 0;
            }

            return tileX;
        }

        protected int checkTileYRange(int tileY)
        {
            if (tileY >= this.mTileDepthSize)
            {
                tileY = this.mTileDepthSize - 1;
            }

            if (tileY < 0)
            {
                tileY = 0;
            }

            return tileY;
        }

        // 转换位置到 Tile 索引
        protected int convPos2TileIndex(float x, float y, float z)
        {
            int tileIndex = 0;

            tileIndex = UtilMath.floorToInt(y / this.mOneTileDepth) * this.mTileWidthSize + UtilMath.floorToInt(x / this.mOneTileWidth);

            return tileIndex;
        }

        // 转换位置到 Tile 索引
        protected int convPos2TileIndex(UnityEngine.Vector3 pos)
        {
            int tileIndex = 0;

            int tileX = UtilMath.floorToInt(pos.x / this.mOneTileWidth);
            int tileY = UtilMath.floorToInt(pos.y / this.mOneTileDepth);

            tileX = this.checkTileXRange(tileX);
            tileY = this.checkTileXRange(tileY);
            
            tileIndex = tileY * this.mTileWidthSize + tileX;

            return tileIndex;
        }

        // 转换一维 Tile 索引到二维 Tile 索引
        public void convOneTileIndex2Two(int tileIndex, ref int x, ref int y)
        {
            x = tileIndex % this.mTileWidthSize;
            y = tileIndex / this.mTileWidthSize;
        }

        // 转换 Tile 索引到位置，这个是左下角的 Tile 位置
        public UnityEngine.Vector3 convTileIndex2Pos(int tileIndex)
        {
            UnityEngine.Vector3 pos = UnityEngine.Vector3.zero;

            int tileX = 0;
            int tileY = 0;
            this.convOneTileIndex2Two(tileIndex, ref tileX, ref tileY);

            pos.x = tileX * this.mOneTileWidth;
            pos.y = tileY * this.mOneTileDepth;

            return pos;
        }

        public int getTileIndexByEntity(SceneEntityBase entity)
        {
            int tileIndex = 0;
            tileIndex = this.convPos2TileIndex(entity.getPos());

            return tileIndex;
        }

        // 通过 Tile 索引获取对应的 Tile
        protected TDTile getTileByIndex(int tileIndex)
        {
            TDTile tile = null;

            if (tileIndex >= 0 && tileIndex < this.mTileTotal)
            {
                if (null == this.mTileArray[tileIndex])
                {
                    tile = this.createTile();
                    this.mTileArray[tileIndex] = tile;
                    tile.setTileIndex(tileIndex);
                    tile.init();
                }
                else
                {
                    tile = this.mTileArray[tileIndex];
                }
            }

            return tile;
        }

        public TDTile getTileByPos(UnityEngine.Vector3 pos)
        {
            TDTile tile = null;

            int tileIndex = this.convPos2TileIndex(pos);
            tile = this.getTileByIndex(tileIndex);

            return tile;
        }

        protected void flipVisibleIndex()
        {
            // 交换缓冲索引
            this.mCurVislbleTileIndex = (this.mCurVislbleTileIndex + 1) % (int)VisibleTileIndex.eVTI_Total;
            this.mPreVislbleTileIndex = (this.mCurVislbleTileIndex + 1) % (int)VisibleTileIndex.eVTI_Total;

            // 清除当前
            this.mVisibleTileListArray[this.mCurVislbleTileIndex].Clear();
        }

        // 更新一个 Entity 位置改变
        public void updateEntity(SceneEntityBase entity)
        {
            int tileIndex = this.getTileIndexByEntity(entity);
            TDTile tile = this.getTileByIndex(tileIndex);

            if (null != tile)
            {
                tile.updateEntity(entity);
                this.mDirtyEntityList.Add(entity);
            }
        }

        // 更新一个 ClipRect 位置改变
        public void updateClipRect(TDClipRect clipRect)
        {
            this.flipVisibleIndex();

            int minX = (int)(clipRect.getLeft() / this.mOneTileWidth);
            int maxX = (int)(clipRect.getRight() / this.mOneTileWidth);
            int minY = (int)(clipRect.getBottom() / this.mOneTileDepth);
            int maxY = (int)(clipRect.getTop() / this.mOneTileDepth);

            minX = this.checkTileXRange(minX);
            maxX = this.checkTileXRange(maxX);
            minY = this.checkTileYRange(minY);
            maxY = this.checkTileYRange(maxY);

            int indexX = minX;
            int indexY = minY;
            int curIndex = 0;
            TDTile tile = null;

            while (indexY <= maxY)
            {
                indexX = minX;

                while(indexX <= maxX)
                {
                    curIndex = indexY * mTileWidthSize + indexX;
                    tile = this.getTileByIndex(curIndex);

                    if (null != tile)
                    {
                        this.mVisibleTileListArray[this.mCurVislbleTileIndex].Add(tile);
                        this.mVisibleTileListArray[this.mPreVislbleTileIndex].Remove(tile);

                        // 可见处理
                        if (!tile.isVisible())  // 如果之前不可见
                        {
                            tile.show();
                        }
                        else if (!tile.isFullVisible())     // 如果之前没有完全可见
                        {
                            tile.updateVisible();
                        }
                        else
                        {
                            // 之前完全可见，现在不完全可见
                            if (indexY == minY ||
                                indexY == maxY ||
                                indexX == minX ||
                                indexX == maxY)
                            {
                                tile.updateVisible();
                            }
                        }

                        // 完全可见
                        if (indexY > minY &&
                            indexY < maxY &&
                            indexX > minX &&
                            indexX < maxY)
                        {
                            tile.setIsFullVisible(true);
                        }
                        else
                        {
                            tile.setIsFullVisible(false);
                        }

                        // 直接更新 Tile 内的可见性
                        //tile.updateVisible();
                    }

                    ++indexX;
                }

                ++indexY;
            }

            int hideIndex = 0;
            int hideLen = this.mVisibleTileListArray[this.mPreVislbleTileIndex].Count();

            while(hideIndex < hideLen)
            {
                tile = this.mVisibleTileListArray[this.mPreVislbleTileIndex].get(hideIndex);
                tile.hide();

                ++hideIndex;
            }

            this.mVisibleTileListArray[this.mPreVislbleTileIndex].Clear();

            // 如果可见的，已经在上面更新了，这个只要把不可见的隐藏就行了，这些不可见主要是移动到了没有更新的 Tile 里面了
            //this.updateDirtyEntity();
        }

        // 更新 Dirty Entity
        public void updateDirtyEntity()
        {
            int index = 0;
            int len = this.mDirtyEntityList.Count();
            SceneEntityBase entity = null;

            while (index < len)
            {
                entity = this.mDirtyEntityList.get(index);

                if(null != entity.getTile())
                {
                    entity.getTile().updateEntity(entity);
                }

                ++index;
            }

            this.mDirtyEntityList.Clear();
        }
    }
}
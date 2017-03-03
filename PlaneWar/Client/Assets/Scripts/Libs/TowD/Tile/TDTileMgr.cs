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

        protected MList<TDTile>[] mVisibleTileListArray;
        protected int mCurVislbleTileIndex;
        protected int mPreVislbleTileIndex;

        public TDTileMgr()
        {
            this.init();
        }

        public void init()
        {
            this.mOneTileWidth = 30;
            this.mOneTileDepth = 30;

            this.mWorldWidth = 30000;
            this.mWorldDepth = 30000;

            this.mTileWidthSize = this.mWorldWidth / this.mOneTileWidth;
            this.mTileDepthSize = this.mWorldDepth / this.mOneTileDepth;

            this.mTileArray = new TDTile[this.mTileWidthSize * this.mTileDepthSize];

            this.mVisibleTileListArray = new MList<TDTile>[(int)VisibleTileIndex.eVTI_Total];
            this.mVisibleTileListArray[0].setIsSpeedUpFind(true);
            this.mVisibleTileListArray[1].setIsSpeedUpFind(true);
            this.mCurVislbleTileIndex = 0;
            this.mPreVislbleTileIndex = 1;
        }

        protected void flipVisibleIndex()
        {
            this.mCurVislbleTileIndex = (this.mCurVislbleTileIndex + 1) % (int)VisibleTileIndex.eVTI_Total;
            this.mPreVislbleTileIndex = (this.mCurVislbleTileIndex + 1) % (int)VisibleTileIndex.eVTI_Total;

            MList<TDTile> tmp = this.mVisibleTileListArray[0];
            this.mVisibleTileListArray[0] = this.mVisibleTileListArray[1];
            this.mVisibleTileListArray[1] = tmp;

            this.mVisibleTileListArray[this.mCurVislbleTileIndex].Clear();
        }

        // 更新一个 Entity 位置改变
        public void updateEntity(SceneEntityBase entity)
        {
            int tileIndex = this.getEntityTileId(entity);
            TDTile tile = this.getTileByIndex(tileIndex);
            tile.updateEntity(entity);
        }

        // 更新一个 ClipRect 位置改变
        public void updateClipRect(TDClipRect clipRect)
        {
            this.flipVisibleIndex();

            int minX = (int)(clipRect.getLeft() / this.mOneTileWidth);
            int maxX = (int)(clipRect.getRight() / this.mOneTileWidth);
            int minY = (int)(clipRect.getBottom() / this.mOneTileDepth);
            int maxY = (int)(clipRect.getTop() / this.mOneTileDepth);

            int indexX = minX;
            int indexY = minY;
            int curIndex = 0;
            TDTile tile = null;

            while (indexY <= maxY)
            {
                while(indexX <= maxX)
                {
                    curIndex = indexY * mTileWidthSize + indexX;
                    tile = this.getTileByIndex(curIndex);

                    this.mVisibleTileListArray[this.mCurVislbleTileIndex].Add(tile);
                    this.mVisibleTileListArray[this.mPreVislbleTileIndex].Remove(tile);

                    tile.setIsVisible(true);

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
                        if(indexY == minY ||
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
        }

        public int getEntityTileId(SceneEntityBase entity)
        {
            int ret = 0;
            ret = UtilMath.floorToInt(entity.getWorldPosY() / this.mOneTileDepth) * this.mTileWidthSize + UtilMath.floorToInt(entity.getWorldPosX() / this.mOneTileWidth);

            return ret;
        }

        // 通过 Tile 索引获取对应的 Tile
        protected TDTile getTileByIndex(int tileIndex)
        {
            TDTile tile = null;

            if (null == this.mTileArray[tileIndex])
            {
                tile = new TDTile();
                this.mTileArray[tileIndex] = tile;
                tile.init();
            }
            else
            {
                tile = this.mTileArray[tileIndex];
            }

            return tile;
        }

        public TDTile getTileByPos(MVector3 pos)
        {
            TDTile tile = null;

            int x = UtilMath.floorToInt(pos.x / this.mOneTileWidth);
            int y = UtilMath.floorToInt(pos.y / this.mOneTileDepth);

            int tileIndex = y * this.mTileWidthSize + x;
            tile = this.getTileByIndex(tileIndex);

            return tile;
        }
    }
}
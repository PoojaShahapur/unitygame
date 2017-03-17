namespace SDK.Lib
{
    /**
     * @brief 2D Terrain
     */
    public class MTwoDTerrain : TDTileMgr
    {
        public MTwoDTerrain()
        {

        }

        override public void init()
        {
            base.init();

            this.setOneTileWidthHeight(8, 8);
        }

        override public void dispose()
        {

        }

        override protected void buildDirtyList()
        {

        }

        override protected void buildTileArray()
        {
            this.mTileArray = new MTwoDDistrict[this.mTileTotal];
        }

        override protected TDTile createTile()
        {
            TDTile tile = new MTwoDDistrict();
            return tile;
        }
    }
}
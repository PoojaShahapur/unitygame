namespace SDK.Lib
{
    /**
     * @brief Terrain District
     */
    public class MTwoDDistrict : TDTile
    {
        protected MTwoDDistrictRender mRender;
        protected bool mIsInScreenRange;    // �Ƿ�����Ļ��Χ����Ҫ���Ƿ���������ʾ�ɼ�
        protected UnityEngine.Vector3 mPos;

        public MTwoDDistrict()
        {

        }

        override public void init()
        {
            base.init();

            this.mRender = new MTwoDDistrictRender(this);
            this.mRender.init();

            this.mRender.setPos(this.mPos);

            if (this.mIsInScreenRange)
            {
                if (null != this.mRender)
                {
                    this.mRender.onEnterScreenRange();
                }
            }
        }

        override public void dispose()
        {
            this.mRender.dispose();

            base.dispose();
        }

        override public void setTileIndex(int index)
        {
            base.setTileIndex(index);

            this.setPos(Ctx.mInstance.mTwoDTerrain.convTileIndex2Pos(index) + new UnityEngine.Vector3(Ctx.mInstance.mTwoDTerrain.getTileWidth() / 2, Ctx.mInstance.mTwoDTerrain.getTileDepth() / 2, 0));
        }

        override public void updateShow()
        {
            this.onEnterScreenRange();
        }

        override public void updateHide()
        {
            this.onLeaveScreenRange();
        }

        override public void updateVisible()
        {
            this.onEnterScreenRange();
        }

        // ����ɼ�
        public void onEnterScreenRange()
        {
            if(MacroDef.ENABLE_LOG)
            {
                Ctx.mInstance.mLogSys.log(string.Format("MTwoDDistrict::onEnterScreenRange, one, tileId = {0}", this.mTileindex), LogTypeId.eLogTwoDTerrain);
            }

            if (!this.mIsInScreenRange)
            {
                this.mIsInScreenRange = true;

                if (null != this.mRender)
                {
                    this.mRender.onEnterScreenRange();
                }

                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log(string.Format("MTwoDDistrict::onEnterScreenRange, two, tileId = {0}", this.mTileindex), LogTypeId.eLogTwoDTerrain);
                }
            }
        }

        // �뿪�ɼ�
        public void onLeaveScreenRange()
        {
            if (MacroDef.ENABLE_LOG)
            {
                Ctx.mInstance.mLogSys.log(string.Format("MTwoDDistrict::onLeaveScreenRange, one, tileId = {0}", this.mTileindex), LogTypeId.eLogTwoDTerrain);
            }

            if (this.mIsInScreenRange)
            {
                this.mIsInScreenRange = false;

                if (null != this.mRender)
                {
                    this.mRender.onLeaveScreenRange();
                }

                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log(string.Format("MTwoDDistrict::onLeaveScreenRange, two, tileId = {0}", this.mTileindex), LogTypeId.eLogTwoDTerrain);
                }
            }
        }

        public UnityEngine.Vector3 getPos()
        {
            return this.mPos;
        }

        public void setPos(UnityEngine.Vector3 pos)
        {
            this.mPos = pos;

            if (null != this.mRender)
            {
                this.mRender.setPos(this.mPos);
            }
        }
    }
}
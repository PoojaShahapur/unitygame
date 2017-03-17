namespace SDK.Lib
{
    /**
     * @brief 手工设置顶点的 Mesh Render
     */
    public class MeshRender : AuxComponent
    {
        protected MSubGeometryBase mSubGeometry;       // 子几何顶点数据
        protected bool mSubGeomDirty;                  // SubGeometry 数据是否是过时的数据

        protected float mXPos;     // X 位置，记录位置调试使用
        protected float mZPos;     // Z 位置，记录位置调试使用
        protected int mXTile;      // Tile 的 X 位置，调试使用
        protected int mZTile;      // Tile 的 Z 位置，调试使用

        public MeshRender(MSubGeometryBase subGeometry_ = null)     // 需要的材质在自己的子类中去操作
        {
            mSubGeometry = subGeometry_;
        }

        override protected void onPntChanged()
        {
            linkSelf2Parent();
        }

        // 如果改变
        override protected void onSelfChanged()
        {
            base.onSelfChanged();
            moveToPos(mXPos, mZPos);
        }

        /**
         * @brief 修改 SubGeometry 数据
         */
        public void setSubGeometry(MSubGeometryBase subGeometry_)
        {
            mSubGeometry = subGeometry_;
        }

        public void setTileXZ(int xTile, int zTile)
        {
            mXTile = xTile;
            mZTile = zTile;

            if (this.selfGo != null)
            {
                this.setSelfName("MeshRender" + "_" + mXTile + "_" + mZTile);
            }
        }

        /**
         * @brief 局部空间移动 Render
         */
        public void moveToPos(float xPos, float zPos)
        {
            mXPos = xPos;
            mZPos = zPos;

            if (this.selfGo != null)
            {
                UtilApi.setPos(this.selfGo.transform, new UnityEngine.Vector3(xPos, 0, zPos));
            }
        }

        /**
         * @brief 子类实现具体的显示
         */
        override public void show()
        {
            if(this.mSelfGo == null)
            {
                this.selfGo = UtilApi.createGameObject("MeshRender");
            }
            base.show();
        }
    }
}
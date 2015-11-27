namespace SDK.Lib
{
    /**
     * @brief 手工设置顶点的 Mesh Render
     */
    public class MeshRender : AuxComponent
    {
        protected MSubGeometryBase m_subGeometry;       // 子几何顶点数据
        protected bool m_subGeomDirty;                  // SubGeometry 数据是否是过时的数据

        public MeshRender(MSubGeometryBase subGeometry_ = null)     // 需要的材质在自己的子类中去操作
        {
            m_subGeometry = subGeometry_;
            m_selfGo = UtilApi.createGameObject("MeshRender");
        }

        override protected void onPntChanged()
        {
            linkSelf2Parent();
        }

        /**
         * @brief 修改 SubGeometry 数据
         */
        public void setSubGeometry(MSubGeometryBase subGeometry_)
        {
            m_subGeometry = subGeometry_;
        }

        /**
         * @brief 子类实现具体的显示
         */
        virtual public void render()
        {

        }
    }
}
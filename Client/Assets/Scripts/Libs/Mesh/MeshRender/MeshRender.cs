namespace SDK.Lib
{
    /**
     * @brief 手工设置顶点的 Mesh Render
     */
    public class MeshRender : AuxComponent
    {
        protected MSubGeometryBase m_subGeometry;       // 子几何顶点数据

        public MeshRender(MatRes matRes_ = null)
        {
            m_selfGo = UtilApi.createGameObject("MeshRender");
        }

        override protected void onPntChanged()
        {
            linkSelf2Parent();
        }

        public void setSubGeometry(MSubGeometryBase subGeometry_)
        {
            m_subGeometry = subGeometry_;
        }

        virtual public void render()
        {

        }
    }
}
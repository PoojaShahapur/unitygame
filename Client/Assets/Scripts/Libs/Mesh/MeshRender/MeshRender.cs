namespace SDK.Lib
{
    /**
     * @brief 手工设置顶点的 Mesh Render
     */
    public class MeshRender : AuxComponent
    {
        protected MSubGeometryBase m_subGeometry;       // 子几何顶点数据

        public MeshRender()
        {
            m_selfGo = UtilApi.createGameObject("AreaRenderBase");
        }

        override protected void onPntChanged()
        {
            linkSelf2Parent();
        }
    }
}
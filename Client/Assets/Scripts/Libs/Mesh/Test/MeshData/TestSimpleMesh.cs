namespace SDK.Lib
{
    /**
     * @brief 这个是简单的 Mesh 显示
     */
    public class TestSimpleMesh : MTestMesh
    {
        protected TestSimpleSubGeometry m_subGeometry;            // SubGeometry 数据

        public TestSimpleMesh(MTestGeometry geometry, TestMeshRender meshRender_)
            : base(geometry, meshRender_)
        {

        }

        public void buildMesh()
        {
            m_subGeometry = new TestSimpleSubGeometry();
            this.getGeometry().addSubGeometry(m_subGeometry);

            m_subGeometry.buildGeometry();
        }
    }
}
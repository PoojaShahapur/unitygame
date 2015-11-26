namespace SDK.Lib
{
    public class SimpleMesh : MMesh
    {
        protected SimpleSubGeometry m_subGeometry;            // SubGeometry 数据

        public SimpleMesh(MGeometry geometry, MatRes material = null)
            : base(geometry, material)
        {

        }

        public void buildMesh()
        {
            m_subGeometry = new SimpleSubGeometry();
            this.getGeometry().addSubGeometry(m_subGeometry);

            m_subGeometry.buildGeometry();
        }

        public void render()
        {

        }
    }
}
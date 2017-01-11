namespace SDK.Lib
{
    /**
     * @brief 这个是简单的 Mesh 显示
     */
    public class SimpleMesh : MMesh
    {
        protected SimpleSubGeometry mSubGeometry;            // SubGeometry 数据

        public SimpleMesh(MGeometry geometry, MeshRender meshRender_)
            : base(geometry, meshRender_)
        {

        }

        public void buildMesh()
        {
            mSubGeometry = new SimpleSubGeometry();
            this.getGeometry().addSubGeometry(mSubGeometry);

            mSubGeometry.buildGeometry();
        }
    }
}
namespace SDK.Lib
{
    public class MTestQuadTreeNodeProxy : MTestNodeProxy
    {
        protected MTestSubMesh m_submesh;           // 四叉树保存的节点关联的显示数据

        public void setSubMesh(MTestSubMesh submesh)
        {
            m_submesh = submesh;
        }

        public MTestSubMesh getSubMesh()
        {
            return m_submesh;
        }

        override public void show()
        {
            base.show();
            m_submesh.show();
        }

        override public void hide()
        {
            base.hide();
            m_submesh.hide();
        }

        override public void updateMesh(MTestTerrain terrain, int tileIndex)
        {
            m_submesh = terrain.getTileMesh(tileIndex);
        }

        override public MAxisAlignedBox getAABox()
        {
            return m_submesh.getAABox();
        }
    }
}
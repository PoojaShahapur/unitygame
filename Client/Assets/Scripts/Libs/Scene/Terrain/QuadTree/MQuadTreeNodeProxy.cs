namespace SDK.Lib
{
    public class MQuadTreeNodeProxy : MNodeProxy
    {
        protected MSubMesh m_submesh;           // 四叉树保存的节点关联的显示数据

        public void setSubMesh(MSubMesh submesh)
        {
            m_submesh = submesh;
        }

        public MSubMesh getSubMesh()
        {
            return m_submesh;
        }

        override public void show()
        {
            base.show();
        }

        override public void hide()
        {
            base.hide();
        }

        override public void updateMesh(TerrainPage terrain, int tileIndex)
        {
            m_submesh = terrain.getTileMesh(tileIndex);
        }
    }
}
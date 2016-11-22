using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 顶点列表都是四边形，四边形排列如下
     */
    public class QuadMeshRender : AuxComponent
    {
        protected int m_vertexNum;
        protected int m_vertexIndex;
        protected Vector3[] m_vertices;
        protected Vector2[] m_uv;
        protected Color[] m_color;
        protected Vector3[] m_normals;
        protected int[] m_triangles;

        protected Mesh m_mesh;
        protected MeshFilter m_meshFilter;
        protected MeshRenderer m_renderer;
        protected Material m_dynamicMat;

        protected bool m_bDirty;

        protected float m_xPos;     // X 位置，记录位置调试使用
        protected float m_zPos;     // Z 位置，记录位置调试使用
        protected int m_xTile;      // Tile 的 X 位置，调试使用
        protected int m_zTile;      // Tile 的 Z 位置，调试使用

        public QuadMeshRender(int vertexNum)
        {
            m_vertexNum = vertexNum;
            m_vertexIndex = 0;
            m_bDirty = false;
            m_vertices = new Vector3[vertexNum];
            m_triangles = new int[vertexNum/4 * 2 * 3];
        }

        public void setTileXZ(int xTile, int zTile)
        {
            m_xTile = xTile;
            m_zTile = zTile;
        }

        public void moveToPos(float xPos, float zPos)
        {
            m_xPos = xPos;
            m_zPos = zPos;
            UtilApi.setPos(this.selfGo.transform, new Vector3(xPos, 0, zPos));
        }

        public void clear()
        {
            m_vertexIndex = 0;
        }

        public void addVertex(Vector3 vertex)
        {
            m_vertices[m_vertexIndex] = vertex;
            ++m_vertexIndex;
        }

        public void addVertex(float x, float y, float z)
        {
            addVertex(new Vector3(x, y, z));
        }

        public void addColor(Color color)
        {

        }

        public void addUV(Vector2 uv)
        {

        }

        /**
         * 0 - 1
         * |   |
         * 2 - 3
         */
        public void buildIndexA()
        {
            int vertexIdx = 0;
            int index = 0;
            for(vertexIdx = 0; vertexIdx < m_vertexNum; vertexIdx += 4)
            {
                m_triangles[index++] = vertexIdx;
                m_triangles[index++] = vertexIdx + 1;
                m_triangles[index++] = vertexIdx + 3;

                m_triangles[index++] = vertexIdx;
                m_triangles[index++] = vertexIdx + 3;
                m_triangles[index++] = vertexIdx + 2;
            }
        }

        /**
         * 0 - 1
         * |   |
         * 3 - 2
         */
        public void buildIndexB()
        {
            int vertexIdx = 0;
            int index = 0;
            for (vertexIdx = 0; vertexIdx < m_vertexNum; vertexIdx += 4)
            {
                m_triangles[index++] = vertexIdx;
                m_triangles[index++] = vertexIdx + 1;
                m_triangles[index++] = vertexIdx + 2;

                m_triangles[index++] = vertexIdx;
                m_triangles[index++] = vertexIdx + 2;
                m_triangles[index++] = vertexIdx + 3;
            }
        }

        public void uploadGeometry()
        {
            if(this.selfGo == null)
            {
                this.selfGo = UtilApi.createGameObject("QuadMeshRender" + "_" + m_xTile + "_" + m_zTile);
            }
            // 清除mesh信息，下面可以做相应的mesh动画
            if(m_mesh == null)
            {
                m_mesh = new Mesh();
            }

            m_mesh.Clear();
            m_mesh.vertices = m_vertices;
            m_mesh.uv = m_uv;
            m_mesh.colors = m_color;
            m_mesh.normals = m_normals;
            m_mesh.triangles = m_triangles;
            m_mesh.RecalculateNormals();
            m_mesh.RecalculateBounds();

            if (m_meshFilter == null)
            {
                m_meshFilter = this.selfGo.GetComponent<MeshFilter>();
            }
            if (m_meshFilter == null)
            {
                m_meshFilter = this.selfGo.AddComponent<MeshFilter>();
            }

            this.selfGo.GetComponent<MeshFilter>().mesh = m_mesh;
            if (m_renderer == null)
            {
                m_renderer = this.selfGo.GetComponent<MeshRenderer>();
            }

            if (m_renderer == null)
            {
                m_renderer = this.selfGo.AddComponent<MeshRenderer>();
                m_renderer.enabled = true;
            }

            //Shader shader = Shader.Find("Mobile/Diffuse");
            //m_dynamicMat = new Material(shader);
            MatRes mat = Ctx.mInstance.mMatMgr.getAndSyncLoadRes("Materials/Mesh/FrustumMesh");
            UtilApi.createMatIns(ref m_dynamicMat, mat.getMat());

            if (m_renderer != null)
            {
                //m_renderer.sharedMaterials = new Material[] { m_dynamicMat };
                m_renderer.material = m_dynamicMat;
            }
        }
    }
}
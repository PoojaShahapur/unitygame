using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 顶点列表都是四边形，四边形排列如下
     */
    public class MAABBMeshRender : AuxComponent
    {
        protected int m_vertexNum;
        protected int m_vertexIndex;
        protected Vector3[] m_vertices;
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

        protected string mName;

        public MAABBMeshRender()
        {
            m_vertexNum = 8;
            m_vertexIndex = 0;
            m_bDirty = false;
            m_vertices = new Vector3[m_vertexNum];
            m_triangles = new int[6 * 2 * 3];
        }

        public void setTileXZ(int xTile, int zTile)
        {
            m_xTile = xTile;
            m_zTile = zTile;
        }

        public void setName(string value)
        {
            mName = value;
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

        public void addVertex(ref MAxisAlignedBox aabb)
        {
            MVector3[] corners = aabb.getAllCorners();
            for(int idx = 0; idx < corners.Length; ++idx)
            {
                m_vertices[idx] = corners[idx].toNative();
            }
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
         * Near  1 - 2       far  5 - 4
         *       |   |            |   |
         *       0 - 3            6 - 7
         */
        public void buildIndex()
        {
            int index = 0;
            // Top
            m_triangles[index++] = 1;
            m_triangles[index++] = 5;
            m_triangles[index++] = 4;

            m_triangles[index++] = 1;
            m_triangles[index++] = 4;
            m_triangles[index++] = 2;

            // Bottom
            m_triangles[index++] = 0;
            m_triangles[index++] = 3;
            m_triangles[index++] = 7;

            m_triangles[index++] = 0;
            m_triangles[index++] = 7;
            m_triangles[index++] = 6;

            // Left
            m_triangles[index++] = 0;
            m_triangles[index++] = 6;
            m_triangles[index++] = 5;

            m_triangles[index++] = 0;
            m_triangles[index++] = 5;
            m_triangles[index++] = 1;

            // Right
            m_triangles[index++] = 2;
            m_triangles[index++] = 4;
            m_triangles[index++] = 7;

            m_triangles[index++] = 2;
            m_triangles[index++] = 7;
            m_triangles[index++] = 3;

            // Near
            m_triangles[index++] = 0;
            m_triangles[index++] = 1;
            m_triangles[index++] = 2;

            m_triangles[index++] = 0;
            m_triangles[index++] = 2;
            m_triangles[index++] = 3;

            // Far
            m_triangles[index++] = 4;
            m_triangles[index++] = 5;
            m_triangles[index++] = 6;

            m_triangles[index++] = 4;
            m_triangles[index++] = 6;
            m_triangles[index++] = 7;
        }

        public void uploadGeometry()
        {
            if (this.selfGo == null)
            {
                this.selfGo = UtilApi.createGameObject("AABBMeshRender" + "_" + m_xTile + "_" + m_zTile);
            }
            // 清除mesh信息，下面可以做相应的mesh动画
            if (m_mesh == null)
            {
                m_mesh = new Mesh();
            }

            m_mesh.Clear();
            m_mesh.vertices = m_vertices;
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

            MatRes mat = Ctx.mInstance.mMatMgr.getAndSyncLoadRes("Materials/Mesh/AABBMesh");

            UtilApi.createMatIns(ref m_dynamicMat, mat.getMat());

            if (m_renderer != null)
            {
                m_renderer.material = m_dynamicMat;
            }
        }
    }
}
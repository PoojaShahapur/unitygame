using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 顶点列表都是四边形，四边形排列如下
     */
    public class MAABBMeshRender : AuxComponent
    {
        protected int mVertexNum;
        protected int mVertexIndex;
        protected Vector3[] mVertices;
        protected int[] mTriangles;

        protected Mesh mMesh;
        protected MeshFilter mMeshFilter;
        protected MeshRenderer mRenderer;
        protected Material mDynamicMat;

        protected bool mIsDirty;

        protected float mXPos;     // X 位置，记录位置调试使用
        protected float mZPos;     // Z 位置，记录位置调试使用
        protected int mXTile;      // Tile 的 X 位置，调试使用
        protected int mZTile;      // Tile 的 Z 位置，调试使用

        protected string mName;

        public MAABBMeshRender()
        {
            mVertexNum = 8;
            mVertexIndex = 0;
            mIsDirty = false;
            mVertices = new Vector3[mVertexNum];
            mTriangles = new int[6 * 2 * 3];
        }

        public void setTileXZ(int xTile, int zTile)
        {
            mXTile = xTile;
            mZTile = zTile;
        }

        public void setName(string value)
        {
            mName = value;
        }

        public void moveToPos(float xPos, float zPos)
        {
            mXPos = xPos;
            mZPos = zPos;
            UtilApi.setPos(this.selfGo.transform, new Vector3(xPos, 0, zPos));
        }

        public void clear()
        {
            mVertexIndex = 0;
        }

        public void addVertex(ref MAxisAlignedBox aabb)
        {
            MVector3[] corners = aabb.getAllCorners();
            for(int idx = 0; idx < corners.Length; ++idx)
            {
                mVertices[idx] = corners[idx].toNative();
            }
        }

        public void addVertex(Vector3 vertex)
        {
            mVertices[mVertexIndex] = vertex;
            ++mVertexIndex;
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
            mTriangles[index++] = 1;
            mTriangles[index++] = 5;
            mTriangles[index++] = 4;

            mTriangles[index++] = 1;
            mTriangles[index++] = 4;
            mTriangles[index++] = 2;

            // Bottom
            mTriangles[index++] = 0;
            mTriangles[index++] = 3;
            mTriangles[index++] = 7;

            mTriangles[index++] = 0;
            mTriangles[index++] = 7;
            mTriangles[index++] = 6;

            // Left
            mTriangles[index++] = 0;
            mTriangles[index++] = 6;
            mTriangles[index++] = 5;

            mTriangles[index++] = 0;
            mTriangles[index++] = 5;
            mTriangles[index++] = 1;

            // Right
            mTriangles[index++] = 2;
            mTriangles[index++] = 4;
            mTriangles[index++] = 7;

            mTriangles[index++] = 2;
            mTriangles[index++] = 7;
            mTriangles[index++] = 3;

            // Near
            mTriangles[index++] = 0;
            mTriangles[index++] = 1;
            mTriangles[index++] = 2;

            mTriangles[index++] = 0;
            mTriangles[index++] = 2;
            mTriangles[index++] = 3;

            // Far
            mTriangles[index++] = 4;
            mTriangles[index++] = 5;
            mTriangles[index++] = 6;

            mTriangles[index++] = 4;
            mTriangles[index++] = 6;
            mTriangles[index++] = 7;
        }

        public void uploadGeometry()
        {
            if (this.selfGo == null)
            {
                this.selfGo = UtilApi.createGameObject("AABBMeshRender" + "_" + mXTile + "_" + mZTile);
            }
            // 清除mesh信息，下面可以做相应的mesh动画
            if (mMesh == null)
            {
                mMesh = new Mesh();
            }

            mMesh.Clear();
            mMesh.vertices = mVertices;
            mMesh.triangles = mTriangles;
            mMesh.RecalculateNormals();
            mMesh.RecalculateBounds();

            if (mMeshFilter == null)
            {
                mMeshFilter = this.selfGo.GetComponent<MeshFilter>();
            }
            if (mMeshFilter == null)
            {
                mMeshFilter = this.selfGo.AddComponent<MeshFilter>();
            }

            this.selfGo.GetComponent<MeshFilter>().mesh = mMesh;
            if (mRenderer == null)
            {
                mRenderer = this.selfGo.GetComponent<MeshRenderer>();
            }

            if (mRenderer == null)
            {
                mRenderer = this.selfGo.AddComponent<MeshRenderer>();
                mRenderer.enabled = true;
            }

            MatRes mat = Ctx.mInstance.mMatMgr.getAndSyncLoadRes("Materials/Mesh/AABBMesh", null);

            UtilApi.createMatIns(ref mDynamicMat, mat.getMat());

            if (mRenderer != null)
            {
                mRenderer.material = mDynamicMat;
            }
        }
    }
}
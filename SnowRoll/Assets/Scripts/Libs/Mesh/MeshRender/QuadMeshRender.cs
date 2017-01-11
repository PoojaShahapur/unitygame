using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 顶点列表都是四边形，四边形排列如下
     */
    public class QuadMeshRender : AuxComponent
    {
        protected int mVertexNum;
        protected int mVertexIndex;
        protected Vector3[] mVertices;
        protected Vector2[] mUv;
        protected Color[] mColor;
        protected Vector3[] mNormals;
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

        public QuadMeshRender(int vertexNum)
        {
            mVertexNum = vertexNum;
            mVertexIndex = 0;
            mIsDirty = false;
            mVertices = new Vector3[vertexNum];
            mTriangles = new int[vertexNum/4 * 2 * 3];
        }

        public void setTileXZ(int xTile, int zTile)
        {
            mXTile = xTile;
            mZTile = zTile;
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
         * 0 - 1
         * |   |
         * 2 - 3
         */
        public void buildIndexA()
        {
            int vertexIdx = 0;
            int index = 0;
            for(vertexIdx = 0; vertexIdx < mVertexNum; vertexIdx += 4)
            {
                mTriangles[index++] = vertexIdx;
                mTriangles[index++] = vertexIdx + 1;
                mTriangles[index++] = vertexIdx + 3;

                mTriangles[index++] = vertexIdx;
                mTriangles[index++] = vertexIdx + 3;
                mTriangles[index++] = vertexIdx + 2;
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
            for (vertexIdx = 0; vertexIdx < mVertexNum; vertexIdx += 4)
            {
                mTriangles[index++] = vertexIdx;
                mTriangles[index++] = vertexIdx + 1;
                mTriangles[index++] = vertexIdx + 2;

                mTriangles[index++] = vertexIdx;
                mTriangles[index++] = vertexIdx + 2;
                mTriangles[index++] = vertexIdx + 3;
            }
        }

        public void uploadGeometry()
        {
            if(this.selfGo == null)
            {
                this.selfGo = UtilApi.createGameObject("QuadMeshRender" + "_" + mXTile + "_" + mZTile);
            }
            // 清除mesh信息，下面可以做相应的mesh动画
            if(mMesh == null)
            {
                mMesh = new Mesh();
            }

            mMesh.Clear();
            mMesh.vertices = mVertices;
            mMesh.uv = mUv;
            mMesh.colors = mColor;
            mMesh.normals = mNormals;
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

            //Shader shader = Shader.Find("Mobile/Diffuse");
            //mDynamicMat = new Material(shader);
            MatRes mat = Ctx.mInstance.mMatMgr.getAndSyncLoadRes("Materials/Mesh/FrustumMesh", null);
            UtilApi.createMatIns(ref mDynamicMat, mat.getMat());

            if (mRenderer != null)
            {
                //mRenderer.sharedMaterials = new Material[] { mDynamicMat };
                mRenderer.material = mDynamicMat;
            }
        }
    }
}
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 仅仅有漫反射渲染
     */
    public class TerrainTileRender : AuxComponent
    {
        // Unity 规定一个 Mesh 顶点最多不能超过 65000 个顶点，注意是顶点数量，不是内存数量
        protected static int MAX_VERTEX_PER_MESH = 65000;

        protected Material mMaterial;         // 使用的共享材质
        protected Texture mTexture;           // 使用的纹理
        protected Shader mShader;             // 动态材质使用的纹理

        protected Transform mTrans;           // 渲染位置信息
        protected Mesh mMesh;                 // mesh 信息
        protected MeshFilter mFilter;         // 绘制使用的 MeshFilter
        protected MeshRenderer mRenderer;     // mesh 渲染使用的 Render
        protected Material mDynamicMat;       // 实例化的动态材质，显示使用的材质

        protected bool mRebuildMat = true;    // 是否重新生成材质
        protected int mRenderQueue = 100;    // 渲染队列
        protected int mTriangles = 0;         // 渲染的三角形的数量

        protected string mMatPreStr;           // 材质前缀字符
        protected string mMeshName;            // Mesh 的名字
        protected MTerrainQuadTreeNode mTreeNode;
        protected bool mIsAutoBuildNormal;
        protected bool mIsBuildGromAndMat;
        protected bool mNeedPhysicsCollide;    // 是否需要物理碰撞

        public TerrainTileRender(MTerrainQuadTreeNode treeNode)
            : base(null)
        {
            mTreeNode = treeNode;
            mMatPreStr = "Dyn_";
            mMeshName = "Dyn_Mesh";
            mIsAutoBuildNormal = false;
            mIsBuildGromAndMat = false;
            mNeedPhysicsCollide = true;
        }

        public void setTreeNode(MTerrainQuadTreeNode treeNode)
        {
            mTreeNode = treeNode;
        }

        /**
         * @brief 渲染队列
         */
        public int renderQueue
        {
            get
            {
                return mRenderQueue;
            }
            set
            {
                if (mRenderQueue != value)
                {
                    mRenderQueue = value;

                    if (mDynamicMat != null)
                    {
                        mDynamicMat.renderQueue = value;
#if UNITY_EDITOR
                        if (mRenderer != null)
                        {
                            mRenderer.enabled = isActive;
                        }
#endif
                    }
                }
            }
        }

        /**
         * @brief 排序
         */
        public int sortingOrder
        {
            get
            {
                return (mRenderer != null) ? mRenderer.sortingOrder : 0;
            }
            set
            {
                if (mRenderer != null && mRenderer.sortingOrder != value)
                {
                    mRenderer.sortingOrder = value;
                }
            }
        }

        /**
         * @brief 渲染队列数量
         */
        public int finalRenderQueue
        {
            get
            {
                return (mDynamicMat != null) ? mDynamicMat.renderQueue : mRenderQueue;
            }
        }

#if UNITY_EDITOR
        public bool isActive
        {
            get
            {
                return m_active;
            }
            set
            {
                if (m_active != value)
                {
                    m_active = value;

                    if (mRenderer != null)
                    {
                        mRenderer.enabled = value;
                        UtilApi.SetDirty(this.mSelfGo);
                    }
                }
            }
        }
        bool m_active = true;
#endif

        /**
         * @brief 转换信息
         */
        public Transform cachedTransform
        {
            get
            {
                if (mTrans == null)
                {
                    mTrans = this.mSelfGo.transform;
                }
                return mTrans;
            }
        }

        /**
         * @brief 基本的公用 Material
         */
        public Material baseMaterial
        {
            get
            {
                return mMaterial;
            }
            set
            {
                if (mMaterial != value)
                {
                    mMaterial = value;
                    mRebuildMat = true;
                }
            }
        }

        /**
         * @brief 使用的显示 Material
         */
        public Material dynamicMaterial
        {
            get
            {
                return mDynamicMat;
            }
        }

        /**
         * @brief 设置 Texture 
         */
        public Texture mainTexture
        {
            get
            {
                return mTexture;
            }
            set
            {
                mTexture = value;
                if (mDynamicMat != null)
                {
                    mDynamicMat.mainTexture = value;
                }
            }
        }

        /**
         * @brief shader 设置
         */
        public Shader shader
        {
            get
            {
                return mShader;
            }
            set
            {
                if (mShader != value)
                {
                    mShader = value;
                    mRebuildMat = true;
                }
            }
        }

        /**
         * @brief 获取三角形的数量
         */
        public int triangles
        {
            get
            {
                return (mMesh != null) ? mTriangles : 0;
            }
        }

        /**
         * @brief 更新几何信息
         */
        public void UpdateGeometry()
        {
            int vertexCount = mTreeNode.getVertexDataCount();
            // 缓存所有的组件
            if (mFilter == null)
            {
                mFilter = this.mSelfGo.GetComponent<MeshFilter>();
            }
            if (mFilter == null)
            {
                mFilter = this.mSelfGo.AddComponent<MeshFilter>();
            }

            if (vertexCount < MAX_VERTEX_PER_MESH)  // 顶点数量判断
            {
                // 创建 mesh
                if (mMesh == null)
                {
                    mMesh = new Mesh();
                    mMesh.hideFlags = HideFlags.DontSave;
                    mMesh.name = (mMaterial != null) ? mMatPreStr + mMaterial.name : mMeshName;
                    mMesh.MarkDynamic();
                }

                mTriangles = mTreeNode.getTriangleCount();

                if (mMesh.vertexCount != vertexCount)
                {
                    mMesh.Clear();
                }

                mMesh.vertices = mTreeNode.getVertexData();
                mMesh.uv = mTreeNode.getUVData();
                //mMesh.colors32 = mTreeNode.getVectexColorData();
                mMesh.triangles = mTreeNode.getIndexData();
                mMesh.normals = mTreeNode.getVertexNormalsData();
                mMesh.tangents = mTreeNode.getVertexTangentsData();

                if (mIsAutoBuildNormal)
                {
                    mMesh.RecalculateNormals();
                    mMesh.RecalculateBounds();
                }

                mFilter.mesh = mMesh;
            }
            else
            {
                mTriangles = 0;
                if (mFilter.mesh != null)
                {
                    mFilter.mesh.Clear();
                }
                Debug.LogError("Too many vertices on one Mesh" + vertexCount);
            }

            if (mRenderer == null)
            {
                mRenderer = this.mSelfGo.GetComponent<MeshRenderer>();
            }

            if (mRenderer == null)
            {
                mRenderer = this.mSelfGo.AddComponent<MeshRenderer>();
#if UNITY_EDITOR
                mRenderer.enabled = isActive;
#endif
            }

            if(mNeedPhysicsCollide)
            {
                if(this.selfGo.GetComponent<MeshCollider>() == null)
                {
                    this.selfGo.AddComponent<MeshCollider>();
                }
            }

            mTreeNode.clear();
        }

        /**
         * @brief 初始化时候调用
         */
        override public void init()
        {
            mRebuildMat = true;
        }

        /**
         * @brief 释放资源
         */
        override public void dispose()
        {
            mMaterial = null;
            mTexture = null;

            if (mRenderer != null)
            {
                //mRenderer.sharedMaterials = new Material[] { };
                mRenderer.sharedMaterial = null;
            }

            UtilApi.DestroyImmediate(mDynamicMat);
            mDynamicMat = null;

            UtilApi.DestroyImmediate(mMesh);
            mMesh = null;
        }

        void createInsMaterial()
        {
            // 直接拷贝共享材质
            if (mMaterial != null)
            {
                mDynamicMat = new Material(mMaterial);
                mDynamicMat.name = mMatPreStr + mMaterial.name;
                mDynamicMat.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
                mDynamicMat.CopyPropertiesFromMaterial(mMaterial);

                string[] keywords = mMaterial.shaderKeywords;
                for (int i = 0; i < keywords.Length; ++i)
                {
                    mDynamicMat.EnableKeyword(keywords[i]);
                }

                mDynamicMat.renderQueue = mRenderQueue;

                // 更新渲染
                if (mRenderer != null)
                {
                    //mRenderer.sharedMaterials = new Material[] { mDynamicMat };
                    //mRenderer.material = mDynamicMat;
                    mRenderer.sharedMaterial = mDynamicMat;
                }
            }
        }

        public void setTmplMaterial(Material mat)
        {
            mMaterial = mat;
        }

        /**
         * @brief 渲染
         */
        override public void show()
        {
            if (!this.IsVisible())
            {
                if (this.mSelfGo == null)
                {
                    //this.selfGo = UtilApi.createGameObject("MeshRender");
                    this.selfGo = UtilApi.createGameObject("MeshRender" + "_" + mTreeNode.getNameStr());
                    //UtilApi.setStatic(this.selfGo, true);
                    this.selfGo.layer = UtilApi.NameToLayer(mTreeNode.getLayerStr());
                }

                base.show();

                if (!mIsBuildGromAndMat)
                {
                    UpdateGeometry();
                    createInsMaterial();
                    mIsBuildGromAndMat = true;
                }

                moveToPos();
            }
        }

        /**
         * @brief 局部空间移动 Render
         */
        public void moveToPos()
        {
            if (this.selfGo != null)
            {
                //UtilApi.setPos(this.selfGo.transform, new UnityEngine.Vector3(mTreeNode.getLocalCentre().x, 0, mTreeNode.getLocalCentre().z));
                UtilApi.setPos(this.selfGo.transform, mTreeNode.getWorldPos().toNative());
            }
        }
    }
}
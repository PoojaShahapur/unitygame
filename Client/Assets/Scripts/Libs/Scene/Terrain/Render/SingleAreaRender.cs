using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 仅仅有漫反射渲染
     */
    public class SingleAreaRender : MeshRender
    {
        protected AreaBase m_area;

        protected Material mMaterial;         // 使用的共享材质
        protected Texture mTexture;           // 使用的纹理
        protected Shader mShader;             // 动态材质使用的纹理

        protected Transform mTrans;           // 渲染位置信息
        protected Mesh mMesh;                 // mesh 信息
        protected MeshFilter mFilter;         // 绘制使用的 MeshFilter
        protected MeshRenderer mRenderer;     // mesh 渲染使用的 Render
        protected Material mDynamicMat;       // 实例化的动态材质，显示使用的材质

        protected bool mRebuildMat = true;    // 是否重新生成材质
        protected int mRenderQueue = 3000;    // 渲染队列
        protected int mTriangles = 0;         // 渲染的三角形的数量

        protected string m_shaderName;          // shader 的名字
        protected string m_matPreStr;           // 材质前缀字符
        protected string m_meshName;            // Mesh 的名字
        protected string m_texName;             // Texture 名字

        protected MatRes m_matRes;                      // 材质资源
        protected TextureRes m_texRes;                  // 纹理资源

        public SingleAreaRender(MatRes matRes_ = null)
            : base(matRes_)
        {
            m_shaderName = "Mobile/Diffuse";
            m_texName = "Terrain/terrain_diffuse.jpg";
            m_matPreStr = "Dyn_";
            m_meshName = "Dyn_Mesh";
        }

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
                return mActive;
            }
            set
            {
                if (mActive != value)
                {
                    mActive = value;

                    if (mRenderer != null)
                    {
                        mRenderer.enabled = value;
                        UtilApi.SetDirty(m_selfGo);
                    }
                }
            }
        }
        bool mActive = true;
#endif

        public Transform cachedTransform
        {
            get
            {
                if (mTrans == null)
                {
                    mTrans = m_selfGo.transform;
                }
                return mTrans;
            }
        }

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

        public Material dynamicMaterial
        {
            get
            {
                return mDynamicMat;
            }
        }

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

        public int triangles
        {
            get
            {
                return (mMesh != null) ? mTriangles : 0;
            }
        }

        void CreateMaterial()
        {
            string shaderName = (mShader != null) ? mShader.name : ((mMaterial != null) ? mMaterial.shader.name : m_shaderName);
            
            shader = Shader.Find(shaderName);

            // 如果没有加载到 Shader，就是用默认的
            if (shader == null)
            {
                shader = Shader.Find("Mobile/Diffuse");
            }

            // 直接拷贝共享材质
            if (mMaterial != null)
            {
                mDynamicMat = new Material(mMaterial);
                mDynamicMat.name = m_matPreStr + mMaterial.name;
                mDynamicMat.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
                mDynamicMat.CopyPropertiesFromMaterial(mMaterial);

                string[] keywords = mMaterial.shaderKeywords;
                for (int i = 0; i < keywords.Length; ++i)
                {
                    mDynamicMat.EnableKeyword(keywords[i]);
                }

                // 如果 Shader 有效，赋值给动态材质
                if (shader != null)
                {
                    mDynamicMat.shader = shader;
                }
            }
            else
            {
                mDynamicMat = new Material(shader);
                mDynamicMat.name = m_matPreStr + shader.name;
                mDynamicMat.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
            }
        }

        Material RebuildMaterial()
        {
            // 释放老的材质
            UtilApi.DestroyImmediate(mDynamicMat);

            // 创建新的材质
            CreateMaterial();
            mDynamicMat.renderQueue = mRenderQueue;

            // 赋值主要的纹理
            if (mTexture != null)
            {
                mDynamicMat.mainTexture = mTexture;
            }

            // 更新渲染
            if (mRenderer != null)
            {
                mRenderer.sharedMaterials = new Material[] { mDynamicMat };
            }
            return mDynamicMat;
        }

        void UpdateMaterials()
        {
            // 如果裁剪应该被使用，需要查找一个替换的 shader 
            if (mRebuildMat || mDynamicMat == null)
            {
                RebuildMaterial();
                mRebuildMat = false;
            }
            else if (mRenderer.sharedMaterial != mDynamicMat)
            {
#if UNITY_EDITOR
                Debug.LogError("Hmm... This point got hit!");
#endif
                mRenderer.sharedMaterials = new Material[] { mDynamicMat };
            }
        }

        public void UpdateGeometry()
        {
            int vertexCount = m_subGeometry.getVertexDataCount();
            // 缓存所有的组件
            if (mFilter == null)
            {
                mFilter = m_selfGo.GetComponent<MeshFilter>();
            }
            if (mFilter == null)
            {
                mFilter = m_selfGo.AddComponent<MeshFilter>();
            }

            if (vertexCount < 200000)
            {
                bool trim = true;           // 是否顶点数据改变过
                // 创建 mesh
                if (mMesh == null)
                {
                    mMesh = new Mesh();
                    mMesh.hideFlags = HideFlags.DontSave;
                    mMesh.name = (mMaterial != null) ? m_matPreStr + mMaterial.name : m_meshName;
                    mMesh.MarkDynamic();
                }

                mTriangles = m_subGeometry.getTriangleCount();

                if (mMesh.vertexCount != vertexCount)
                {
                    mMesh.Clear();
                }

                mMesh.vertices = m_subGeometry.getVertexDataArray();
                mMesh.uv = m_subGeometry.getUVDataArray();
                mMesh.colors32 = m_subGeometry.getVectexColorArray();

                mMesh.normals = m_subGeometry.getVertexNormalArray();
                mMesh.tangents = m_subGeometry.getVertexTangentArray();
                
                mMesh.triangles = m_subGeometry.getIndexData().ToArray();

                if (trim)
                {
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
                Debug.LogError("Too many vertices on one panel: " + vertexCount);
            }

            if (mRenderer == null)
            {
                mRenderer = m_selfGo.GetComponent<MeshRenderer>();
            }

            if (mRenderer == null)
            {
                mRenderer = m_selfGo.AddComponent<MeshRenderer>();
#if UNITY_EDITOR
                mRenderer.enabled = isActive;
#endif
            }

            m_subGeometry.clear();
        }

        /**
         * @brief 更新纹理
         */
        public void UpdateTexture()
        {
            m_texRes = Ctx.m_instance.m_texMgr.getAndSyncLoad<TextureRes>(m_texName);
            mainTexture = m_texRes.getTexture();
        }

        void OnEnable()
        {
            mRebuildMat = true;
        }

        void OnDisable()
        {
            mMaterial = null;
            mTexture = null;

            if (mRenderer != null)
                mRenderer.sharedMaterials = new Material[] { };

            NGUITools.DestroyImmediate(mDynamicMat);
            mDynamicMat = null;
        }

        void OnDestroy()
        {
            UtilApi.DestroyImmediate(mMesh);
            mMesh = null;
        }

        override public void render()
        {
            UpdateGeometry();
            UpdateMaterials();
            UpdateTexture();
        }
    }
}
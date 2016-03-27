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

        protected Material m_material;         // 使用的共享材质
        protected Texture m_texture;           // 使用的纹理
        protected Shader m_shader;             // 动态材质使用的纹理

        protected Transform m_trans;           // 渲染位置信息
        protected Mesh m_mesh;                 // mesh 信息
        protected MeshFilter m_filter;         // 绘制使用的 MeshFilter
        protected MeshRenderer m_renderer;     // mesh 渲染使用的 Render
        protected Material m_dynamicMat;       // 实例化的动态材质，显示使用的材质

        protected bool m_rebuildMat = true;    // 是否重新生成材质
        protected int m_renderQueue = 3000;    // 渲染队列
        protected int m_triangles = 0;         // 渲染的三角形的数量

        protected string m_matPreStr;           // 材质前缀字符
        protected string m_meshName;            // Mesh 的名字
        protected MTerrainQuadTreeNode m_treeNode;
        protected bool m_isAutoBuildNormal = false;
        protected bool m_isBuildGromAndMat;

        public TerrainTileRender(MTerrainQuadTreeNode treeNode)
            : base(null)
        {
            m_treeNode = treeNode;
            m_matPreStr = "Dyn_";
            m_meshName = "Dyn_Mesh";
            m_isAutoBuildNormal = false;
            m_isBuildGromAndMat = false;
        }

        /**
         * @brief 渲染队列
         */
        public int renderQueue
        {
            get
            {
                return m_renderQueue;
            }
            set
            {
                if (m_renderQueue != value)
                {
                    m_renderQueue = value;

                    if (m_dynamicMat != null)
                    {
                        m_dynamicMat.renderQueue = value;
#if UNITY_EDITOR
                        if (m_renderer != null)
                        {
                            m_renderer.enabled = isActive;
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
                return (m_renderer != null) ? m_renderer.sortingOrder : 0;
            }
            set
            {
                if (m_renderer != null && m_renderer.sortingOrder != value)
                {
                    m_renderer.sortingOrder = value;
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
                return (m_dynamicMat != null) ? m_dynamicMat.renderQueue : m_renderQueue;
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

                    if (m_renderer != null)
                    {
                        m_renderer.enabled = value;
                        UtilApi.SetDirty(m_selfGo);
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
                if (m_trans == null)
                {
                    m_trans = m_selfGo.transform;
                }
                return m_trans;
            }
        }

        /**
         * @brief 基本的公用 Material
         */
        public Material baseMaterial
        {
            get
            {
                return m_material;
            }
            set
            {
                if (m_material != value)
                {
                    m_material = value;
                    m_rebuildMat = true;
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
                return m_dynamicMat;
            }
        }

        /**
         * @brief 设置 Texture 
         */
        public Texture mainTexture
        {
            get
            {
                return m_texture;
            }
            set
            {
                m_texture = value;
                if (m_dynamicMat != null)
                {
                    m_dynamicMat.mainTexture = value;
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
                return m_shader;
            }
            set
            {
                if (m_shader != value)
                {
                    m_shader = value;
                    m_rebuildMat = true;
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
                return (m_mesh != null) ? m_triangles : 0;
            }
        }

        /**
         * @brief 更新几何信息
         */
        public void UpdateGeometry()
        {
            int vertexCount = m_treeNode.getVertexDataCount();
            // 缓存所有的组件
            if (m_filter == null)
            {
                m_filter = m_selfGo.GetComponent<MeshFilter>();
            }
            if (m_filter == null)
            {
                m_filter = m_selfGo.AddComponent<MeshFilter>();
            }

            if (vertexCount < MAX_VERTEX_PER_MESH)  // 顶点数量判断
            {
                // 创建 mesh
                if (m_mesh == null)
                {
                    m_mesh = new Mesh();
                    m_mesh.hideFlags = HideFlags.DontSave;
                    m_mesh.name = (m_material != null) ? m_matPreStr + m_material.name : m_meshName;
                    m_mesh.MarkDynamic();
                }

                m_triangles = m_treeNode.getTriangleCount();

                if (m_mesh.vertexCount != vertexCount)
                {
                    m_mesh.Clear();
                }

                m_mesh.vertices = m_treeNode.getVertexData();
                m_mesh.uv = m_treeNode.getUVData();
                //m_mesh.colors32 = m_treeNode.getVectexColorData();
                m_mesh.triangles = m_treeNode.getIndexData();
                m_mesh.normals = m_treeNode.getVertexNormalsData();
                m_mesh.tangents = m_treeNode.getVertexTangentsData();

                if (m_isAutoBuildNormal)
                {
                    m_mesh.RecalculateNormals();
                    m_mesh.RecalculateBounds();
                }

                m_filter.mesh = m_mesh;
            }
            else
            {
                m_triangles = 0;
                if (m_filter.mesh != null)
                {
                    m_filter.mesh.Clear();
                }
                Debug.LogError("Too many vertices on one Mesh" + vertexCount);
            }

            if (m_renderer == null)
            {
                m_renderer = m_selfGo.GetComponent<MeshRenderer>();
            }

            if (m_renderer == null)
            {
                m_renderer = m_selfGo.AddComponent<MeshRenderer>();
#if UNITY_EDITOR
                m_renderer.enabled = isActive;
#endif
            }

            m_treeNode.clear();
        }

        /**
         * @brief 初始化时候调用
         */
        void init()
        {
            m_rebuildMat = true;
        }

        /**
         * @brief 释放资源
         */
        override public void dispose()
        {
            m_material = null;
            m_texture = null;

            if (m_renderer != null)
            {
                m_renderer.sharedMaterials = new Material[] { };
            }

            UtilApi.DestroyImmediate(m_dynamicMat);
            m_dynamicMat = null;

            UtilApi.DestroyImmediate(m_mesh);
            m_mesh = null;
        }

        void createInsMaterial()
        {
            // 直接拷贝共享材质
            if (m_material != null)
            {
                m_dynamicMat = new Material(m_material);
                m_dynamicMat.name = m_matPreStr + m_material.name;
                m_dynamicMat.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
                m_dynamicMat.CopyPropertiesFromMaterial(m_material);

                string[] keywords = m_material.shaderKeywords;
                for (int i = 0; i < keywords.Length; ++i)
                {
                    m_dynamicMat.EnableKeyword(keywords[i]);
                }

                m_dynamicMat.renderQueue = m_renderQueue;

                // 更新渲染
                if (m_renderer != null)
                {
                    //m_renderer.sharedMaterials = new Material[] { m_dynamicMat };
                    m_renderer.material = m_dynamicMat;
                }
            }
        }

        public void setTmplMaterial(Material mat)
        {
            m_material = mat;
        }

        /**
         * @brief 渲染
         */
        override public void show()
        {
            if (!this.IsVisible())
            {
                if (m_selfGo == null)
                {
                    //this.selfGo = UtilApi.createGameObject("MeshRender");
                    this.selfGo = UtilApi.createGameObject("MeshRender" + "_" + m_treeNode.getNameStr());
                    this.selfGo.layer = UtilApi.NameToLayer(m_treeNode.getLayerStr());
                }

                base.show();

                if (!m_isBuildGromAndMat)
                {
                    UpdateGeometry();
                    createInsMaterial();
                    m_isBuildGromAndMat = true;
                }
            }
        }

        // 如果改变
        override protected void onSelfChanged()
        {
            base.onSelfChanged();
            moveToPos();
        }

        /**
         * @brief 局部空间移动 Render
         */
        public void moveToPos()
        {
            if (this.selfGo != null)
            {
                UtilApi.setPos(this.selfGo.transform, new UnityEngine.Vector3(m_treeNode.getLocalCentre().x, 0, m_treeNode.getLocalCentre().z));
            }
        }
    }
}
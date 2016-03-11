using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 仅仅有漫反射渲染
     */
    public class SingleTileRender : MeshRender
    {
        // Unity 规定一个 Mesh 顶点最多不能超过 65000 个顶点，注意是顶点数量，不是内存数量
        protected static int MAX_VERTEX_PER_MESH = 65000;

        protected TileBase m_tile;

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

        protected string m_shaderName;          // shader 的名字
        protected string m_matPreStr;           // 材质前缀字符
        protected string m_meshName;            // Mesh 的名字
        protected string m_texName;             // Texture 名字

        protected MatRes m_matRes;                      // 材质资源
        protected TextureRes m_texRes;                  // 纹理资源

        public SingleTileRender(MSubGeometryBase subGeometry_ = null)
            : base(subGeometry_)
        {
            m_shaderName = "Mobile/Diffuse";
            m_texName = "Texture/Terrain/terrain_diffuse.jpg";
            m_matPreStr = "Dyn_";
            m_meshName = "Dyn_Mesh";
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
         * @brief 创建材质
         */
        void CreateMaterial()
        {
            string shaderName = (m_shader != null) ? m_shader.name : ((m_material != null) ? m_material.shader.name : m_shaderName);
            
            shader = Shader.Find(shaderName);

            // 如果没有加载到 Shader，就是用默认的
            if (shader == null)
            {
                shader = Shader.Find("Mobile/Diffuse");
            }

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

                // 如果 Shader 有效，赋值给动态材质
                if (shader != null)
                {
                    m_dynamicMat.shader = shader;
                }
            }
            else
            {
                m_dynamicMat = new Material(shader);
                m_dynamicMat.name = m_matPreStr + shader.name;
                m_dynamicMat.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
            }
        }

        /**
         * @brief 重新生成材质
         */
        Material RebuildMaterial()
        {
            // 释放老的材质
            UtilApi.DestroyImmediate(m_dynamicMat);

            // 创建新的材质
            CreateMaterial();
            m_dynamicMat.renderQueue = m_renderQueue;

            // 赋值主要的纹理
            if (m_texture != null)
            {
                m_dynamicMat.mainTexture = m_texture;
            }

            // 更新渲染
            if (m_renderer != null)
            {
                m_renderer.sharedMaterials = new Material[] { m_dynamicMat };
            }
            return m_dynamicMat;
        }

        /**
         * @brief 更新材质
         */
        void UpdateMaterials()
        {
            // 如果裁剪应该被使用，需要查找一个替换的 shader 
            if (m_rebuildMat || m_dynamicMat == null)
            {
                RebuildMaterial();
                m_rebuildMat = false;
            }
            else if (m_renderer.sharedMaterial != m_dynamicMat)
            {
#if UNITY_EDITOR
                Debug.LogError("share material not equal !");
#endif
                m_renderer.sharedMaterials = new Material[] { m_dynamicMat };
            }
        }

        /**
         * @brief 更新几何信息
         */
        public void UpdateGeometry()
        {
            int vertexCount = m_subGeometry.getVertexDataCount();
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
                bool trim = true;           // 是否顶点数据改变过
                // 创建 mesh
                if (m_mesh == null)
                {
                    m_mesh = new Mesh();
                    m_mesh.hideFlags = HideFlags.DontSave;
                    m_mesh.name = (m_material != null) ? m_matPreStr + m_material.name : m_meshName;
                    m_mesh.MarkDynamic();
                }

                m_triangles = m_subGeometry.getTriangleCount();

                if (m_mesh.vertexCount != vertexCount)
                {
                    m_mesh.Clear();
                }

                m_mesh.vertices = m_subGeometry.getVertexData();
                m_mesh.uv = m_subGeometry.getUVData();
                m_mesh.colors32 = m_subGeometry.getVectexColorArray();

                m_mesh.normals = m_subGeometry.getVertexNormalsData();
                m_mesh.tangents = m_subGeometry.getVertexTangentsData();

                m_mesh.triangles = m_subGeometry.getIndexData();

                //Ctx.m_instance.m_fileSys.serializeArray<Vector3>("buildVertex.txt", m_mesh.vertices, 1);
                //Ctx.m_instance.m_fileSys.serializeArray<int>("buildIndex.txt", m_mesh.triangles, 3);
                //Ctx.m_instance.m_fileSys.serializeArray<Vector2>("buildVU.txt", m_mesh.uv, 1);

                //Ctx.m_instance.m_fileSys.serializeArray<Vector3>("buildNormal.txt", m_mesh.normals, 1);
                //Ctx.m_instance.m_fileSys.serializeArray<Vector4>("buildTangent.txt", m_mesh.tangents, 1);

                if (trim)
                {
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

            NGUITools.DestroyImmediate(m_dynamicMat);
            m_dynamicMat = null;

            UtilApi.DestroyImmediate(m_mesh);
            m_mesh = null;
        }

        /**
         * @brief 渲染
         */
        override public void show()
        {
            base.show();
            UpdateGeometry();
            UpdateMaterials();
            UpdateTexture();
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 地形 Area Render
     */
    public class AreaRenderBase : AuxComponent
    {
        protected AreaBase m_area;

        public BetterList<Vector3> verts = new BetterList<Vector3>();
        public BetterList<Vector3> norms = new BetterList<Vector3>();
        public BetterList<Vector4> tans = new BetterList<Vector4>();
        public BetterList<Vector2> uvs = new BetterList<Vector2>();
        public BetterList<Color32> cols = new BetterList<Color32>();

        Material mMaterial;     // Material used by this draw call
        Texture mTexture;       // Main texture used by the material
        Shader mShader;     // Shader used by the dynamically created material

        Transform mTrans;           // Cached transform
        Mesh mMesh;         // First generated mesh
        MeshFilter mFilter;     // Mesh filter for this draw call
        MeshRenderer mRenderer;     // Mesh renderer for this screen
        Material mDynamicMat;   // Instantiated material
        int[] mIndices;     // Cached indices

        bool mRebuildMat = true;
        int mRenderQueue = 3000;
        int mTriangles = 0;

        public bool isDirty = false;

        public AreaRenderBase()
        {
            m_selfGo = UtilApi.createGameObject("AreaRenderBase");
        }

        override protected void onPntChanged()
        {
            linkSelf2Parent();
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
                        if (mRenderer != null) mRenderer.enabled = isActive;
#endif
                    }
                }
            }
        }

        public int sortingOrder
        {
            get { return (mRenderer != null) ? mRenderer.sortingOrder : 0; }
            set { if (mRenderer != null && mRenderer.sortingOrder != value) mRenderer.sortingOrder = value; }
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
                        NGUITools.SetDirty(m_selfGo);
                    }
                }
            }
        }
        bool mActive = true;
#endif

        public Transform cachedTransform { get { if (mTrans == null) mTrans = m_selfGo.transform; return mTrans; } }

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

        public Material dynamicMaterial { get { return mDynamicMat; } }

        public Texture mainTexture
        {
            get
            {
                return mTexture;
            }
            set
            {
                mTexture = value;
                if (mDynamicMat != null) mDynamicMat.mainTexture = value;
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

        public int triangles { get { return (mMesh != null) ? mTriangles : 0; } }

        void CreateMaterial()
        {
            string shaderName = (mShader != null) ? mShader.name :
                ((mMaterial != null) ? mMaterial.shader.name : "Unlit/Transparent Colored");

            // Figure out the normal shader's name
            shaderName = shaderName.Replace("GUI/Text Shader", "Unlit/Text");

            if (shaderName.Length > 2)
            {
                if (shaderName[shaderName.Length - 2] == ' ')
                {
                    int index = shaderName[shaderName.Length - 1];
                    if (index > '0' && index <= '9') shaderName = shaderName.Substring(0, shaderName.Length - 2);
                }
            }

            if (shaderName.StartsWith("Hidden/"))
                shaderName = shaderName.Substring(7);

            // Legacy functionality
            const string soft = " (SoftClip)";
            shaderName = shaderName.Replace(soft, "");

            const string textureClip = " (TextureClip)";
            shaderName = shaderName.Replace(textureClip, "");

            shader = Shader.Find(shaderName);

            // Always fallback to the default shader
            if (shader == null) shader = Shader.Find("Mobile/Diffuse");

            if (mMaterial != null)
            {
                mDynamicMat = new Material(mMaterial);
                mDynamicMat.name = "[NGUI] " + mMaterial.name;
                mDynamicMat.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
                mDynamicMat.CopyPropertiesFromMaterial(mMaterial);

                string[] keywords = mMaterial.shaderKeywords;
                for (int i = 0; i < keywords.Length; ++i)
                    mDynamicMat.EnableKeyword(keywords[i]);

                // If there is a valid shader, assign it to the custom material
                if (shader != null)
                {
                    mDynamicMat.shader = shader;
                }
            }
            else
            {
                mDynamicMat = new Material(shader);
                mDynamicMat.name = "[NGUI] " + shader.name;
                mDynamicMat.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
            }
        }

        Material RebuildMaterial()
        {
            // Destroy the old material
            NGUITools.DestroyImmediate(mDynamicMat);

            // Create a new material
            CreateMaterial();
            mDynamicMat.renderQueue = mRenderQueue;

            // Assign the main texture
            if (mTexture != null) mDynamicMat.mainTexture = mTexture;

            // Update the renderer
            if (mRenderer != null) mRenderer.sharedMaterials = new Material[] { mDynamicMat };
            return mDynamicMat;
        }

        void UpdateMaterials()
        {
            // If clipping should be used, we need to find a replacement shader
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
            int count = verts.size;

            // Safety check to ensure we get valid values
            if (count > 0 && (count == uvs.size && count == cols.size) && (count % 4) == 0)
            {
                // Cache all components
                if (mFilter == null) mFilter = m_selfGo.GetComponent<MeshFilter>();
                if (mFilter == null) mFilter = m_selfGo.AddComponent<MeshFilter>();

                if (verts.size < 65000)
                {
                    // Populate the index buffer
                    int indexCount = (count >> 1) * 3;
                    bool setIndices = (mIndices == null || mIndices.Length != indexCount);

                    // Create the mesh
                    if (mMesh == null)
                    {
                        mMesh = new Mesh();
                        mMesh.hideFlags = HideFlags.DontSave;
                        mMesh.name = (mMaterial != null) ? "[NGUI] " + mMaterial.name : "[NGUI] Mesh";
                        mMesh.MarkDynamic();
                        setIndices = true;
                    }

                    // If the buffer length doesn't match, we need to trim all buffers
                    bool trim = (uvs.buffer.Length != verts.buffer.Length) ||
                        (cols.buffer.Length != verts.buffer.Length) ||
                        (norms.buffer != null && norms.buffer.Length != verts.buffer.Length) ||
                        (tans.buffer != null && tans.buffer.Length != verts.buffer.Length);

                    // Non-automatic render queues rely on Z position, so it's a good idea to trim everything
                    if (!trim)
                        trim = (mMesh == null || mMesh.vertexCount != verts.buffer.Length);

                    // NOTE: Apparently there is a bug with Adreno devices:
                    // http://www.tasharen.com/forum/index.php?topic=8415.0
#if !UNITY_ANDROID
                    // If the number of vertices in the buffer is less than half of the full buffer, trim it
                    if (!trim && (verts.size << 1) < verts.buffer.Length) trim = true;
#endif
                    mTriangles = (verts.size >> 1);

                    if (trim || verts.buffer.Length > 65000)
                    {
                        if (trim || mMesh.vertexCount != verts.size)
                        {
                            mMesh.Clear();
                            setIndices = true;
                        }

                        mMesh.vertices = verts.ToArray();
                        mMesh.uv = uvs.ToArray();
                        mMesh.colors32 = cols.ToArray();

                        if (norms != null) mMesh.normals = norms.ToArray();
                        if (tans != null) mMesh.tangents = tans.ToArray();
                    }
                    else
                    {
                        if (mMesh.vertexCount != verts.buffer.Length)
                        {
                            mMesh.Clear();
                            setIndices = true;
                        }

                        mMesh.vertices = verts.buffer;
                        mMesh.uv = uvs.buffer;
                        mMesh.colors32 = cols.buffer;

                        if (norms != null) mMesh.normals = norms.buffer;
                        if (tans != null) mMesh.tangents = tans.buffer;
                    }

                    if (setIndices)
                    {
                        mIndices = GenerateCachedIndexBuffer(count, indexCount);
                        mMesh.triangles = mIndices;
                    }

                    if (trim)
                        mMesh.RecalculateBounds();

                    mFilter.mesh = mMesh;
                }
                else
                {
                    mTriangles = 0;
                    if (mFilter.mesh != null) mFilter.mesh.Clear();
                    Debug.LogError("Too many vertices on one panel: " + verts.size);
                }

                if (mRenderer == null) mRenderer = m_selfGo.GetComponent<MeshRenderer>();

                if (mRenderer == null)
                {
                    mRenderer = m_selfGo.AddComponent<MeshRenderer>();
#if UNITY_EDITOR
                    mRenderer.enabled = isActive;
#endif
                }
                UpdateMaterials();
            }
            else
            {
                if (mFilter.mesh != null) mFilter.mesh.Clear();
                Debug.LogError("UIWidgets must fill the buffer with 4 vertices per quad. Found " + count);
            }

            verts.Clear();
            uvs.Clear();
            cols.Clear();
            norms.Clear();
            tans.Clear();
        }

        const int maxIndexBufferCache = 10;

        static List<int[]> mCache = new List<int[]>(maxIndexBufferCache);

        int[] GenerateCachedIndexBuffer(int vertexCount, int indexCount)
        {
            for (int i = 0, imax = mCache.Count; i < imax; ++i)
            {
                int[] ids = mCache[i];
                if (ids != null && ids.Length == indexCount)
                    return ids;
            }

            int[] rv = new int[indexCount];
            int index = 0;

            for (int i = 0; i < vertexCount; i += 4)
            {
                rv[index++] = i;
                rv[index++] = i + 1;
                rv[index++] = i + 2;

                rv[index++] = i + 2;
                rv[index++] = i + 3;
                rv[index++] = i;
            }

            if (mCache.Count > maxIndexBufferCache) mCache.RemoveAt(0);
            mCache.Add(rv);
            return rv;
        }

        void OnEnable() { mRebuildMat = true; }

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
            NGUITools.DestroyImmediate(mMesh);
            mMesh = null;
        }

        public void buildMesh()
        {
            mRebuildMat = true;

            verts = new BetterList<Vector3>();

            // 第一次填充这个值，竟然不显示
            //verts.Add(new Vector3(1, 1, 1));
            //verts.Add(new Vector3(2, 2, 2));
            //verts.Add(new Vector3(3, 3, 3));
            //verts.Add(new Vector3(4, 4, 4));

            verts.Add(new Vector3(0, 0, 0));
            verts.Add(new Vector3(1, 0, 0));
            verts.Add(new Vector3(0, 1, 0));
            verts.Add(new Vector3(1, 1, 0));

            norms = new BetterList<Vector3>();
            //norms.Add(new Vector3(1, 1, 1));
            //norms.Add(new Vector3(2, 2, 2));
            //norms.Add(new Vector3(3, 3, 3));
            //norms.Add(new Vector3(4, 4, 4));

            //norms = null;

            tans = new BetterList<Vector4>();
            //tans.Add(new Vector4(1, 1, 1, 1));
            //tans.Add(new Vector4(2, 2, 2, 1));
            //tans.Add(new Vector4(3, 3, 3, 1));
            //tans.Add(new Vector4(4, 4, 4, 1));

            //tans = null;

            uvs = new BetterList<Vector2>();
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(1, 1));

            cols = new BetterList<Color32>();
            cols.Add(new Color32(255, 0, 0, 1));
            cols.Add(new Color32(0, 255, 0, 1));
            cols.Add(new Color32(0, 0, 255, 1));
            cols.Add(new Color32(0, 0, 255, 1));

            UpdateGeometry();
        }
    }
}
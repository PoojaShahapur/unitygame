using UnityEngine;

/**
 * @brief 这个需要自己拖动一个立方体到场景中，然后将这个脚本拖动到 GameObject 上，运行后，立方体变成一个面了
 */
public class HandleMesh : MonoBehaviour
{
    public Vector3[] m_vertices;
    public Vector2[] m_uv;
    public Color[] mColor;
    public Vector3[] m_normals;
    public int[] m_triangles;

    protected Mesh m_mesh;
    protected MeshFilter m_meshFilter;
    MeshRenderer m_renderer;
    Material m_dynamicMat;

    protected bool m_bUpdated;

    void Start()
    {
        m_bUpdated = false;
        m_mesh = new Mesh();
        m_vertices = new Vector3[4]
        {
            new Vector3(0, 0, 0), // 左下
            new Vector3(1, 0, 0), // 右下
            new Vector3(0, 1, 0), // 左上
            new Vector3(1, 1, 0), // 右上
        };

        m_uv = new Vector2[4] // 顶点映射的uv位置
        {
            new Vector3(0, 0), // 顶点0的纹理坐标
            new Vector3(1, 0),
            new Vector3(0, 1),
            new Vector3(1, 1),
        };

        // 左手系
        //m_triangles = new int[6] // 两个三角面的连接
        //{
        //    0, 1, 2,// 通过顶点012连接形成的三角面
        //    1, 3, 2,// 通过顶点132连接形成的三角面
        //};

        // 右手系
        m_triangles = new int[6] // 两个三角面的连接
        {
            0, 2, 3,// 通过顶点012连接形成的三角面
            0, 3, 1,// 通过顶点132连接形成的三角面
        };

        mColor = new Color[4]
        {
            new Color(255, 0, 0, 1),
            new Color(0, 255, 0, 1),
            new Color(0, 0, 255, 1),
            new Color(0, 0, 255, 1),
        };

        if (!m_bUpdated)
        {
            m_bUpdated = true;
            fill();
        }
    }

    void Update()
    {
        //fill();
    }

    protected void fill()
    {
        // 清除mesh信息，下面可以做相应的mesh动画
        m_mesh.Clear();
        m_mesh.vertices = m_vertices;
        m_mesh.uv = m_uv;
        m_mesh.colors = mColor;
        m_mesh.normals = m_normals;
        m_mesh.triangles = m_triangles;
        m_mesh.RecalculateNormals();
        m_mesh.RecalculateBounds();

        if (m_meshFilter == null)
        {
            m_meshFilter = gameObject.GetComponent<MeshFilter>();
        }
        if (m_meshFilter == null)
        {
            m_meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        GetComponent<MeshFilter>().mesh = m_mesh;
        if (m_renderer == null)
        {
            m_renderer = gameObject.GetComponent<MeshRenderer>();
        }

        if (m_renderer == null)
        {
            m_renderer = gameObject.AddComponent<MeshRenderer>();
#if UNITY_EDITOR
            m_renderer.enabled = true;
#endif
        }

        Shader shader = Shader.Find("Mobile/Diffuse");
        //Shader shader = Shader.Find("Standard");
        m_dynamicMat = new Material(shader);
        //m_dynamicMat.mainTexture = mTexture;
        if (m_renderer != null)
        {
            m_renderer.sharedMaterials = new Material[] { m_dynamicMat };
        }
    }
}
using UnityEngine;

/**
 * @brief 这个需要自己拖动一个立方体到场景中，然后将这个脚本拖动到 GameObject 上，运行后，立方体变成一个面了
 */
public class MonoMesh : MonoBehaviour
{
    public Vector3[] m_vertices;
    public Vector2[] m_uv;
    public Color[] mColor;
    public Vector3[] m_normals;
    public int[] m_triangles;

    private Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
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
        m_triangles = new int[6] // 两个三角面的连接
        {
            0, 1, 2,// 通过顶点012连接形成的三角面
            1, 3, 2,// 通过顶点132连接形成的三角面
        };

        // 右手系
        //m_triangles = new int[6] // 两个三角面的连接
        //{
        //    0, 2, 1,// 通过顶点012连接形成的三角面
        //    1, 2, 3,// 通过顶点132连接形成的三角面
        //};
    }

    void Update()
    {
        // 清除mesh信息，下面可以做相应的mesh动画
        mesh.Clear();
        mesh.vertices = m_vertices;
        mesh.uv = m_uv;
        mesh.colors = mColor;
        mesh.normals = m_normals;
        mesh.triangles = m_triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}
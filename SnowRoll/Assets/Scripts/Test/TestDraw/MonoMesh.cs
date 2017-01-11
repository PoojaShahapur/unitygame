using UnityEngine;

/**
 * @brief 这个需要自己拖动一个立方体到场景中，然后将这个脚本拖动到 GameObject 上，运行后，立方体变成一个面了
 */
public class MonoMesh : MonoBehaviour
{
    public Vector3[] mVertices;
    public Vector2[] mUv;
    public Color[] mColor;
    public Vector3[] mNormals;
    public int[] mTriangles;

    private Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        mVertices = new Vector3[4]
        {
            new Vector3(0, 0, 0), // 左下
            new Vector3(1, 0, 0), // 右下
            new Vector3(0, 1, 0), // 左上
            new Vector3(1, 1, 0), // 右上
        };

        mUv = new Vector2[4] // 顶点映射的uv位置
        {
            new Vector3(0, 0), // 顶点0的纹理坐标
            new Vector3(1, 0),
            new Vector3(0, 1),
            new Vector3(1, 1),
        };

        // 左手系
        mTriangles = new int[6] // 两个三角面的连接
        {
            0, 1, 2,// 通过顶点012连接形成的三角面
            1, 3, 2,// 通过顶点132连接形成的三角面
        };

        // 右手系
        //mTriangles = new int[6] // 两个三角面的连接
        //{
        //    0, 2, 1,// 通过顶点012连接形成的三角面
        //    1, 2, 3,// 通过顶点132连接形成的三角面
        //};
    }

    void Update()
    {
        // 清除mesh信息，下面可以做相应的mesh动画
        mesh.Clear();
        mesh.vertices = mVertices;
        mesh.uv = mUv;
        mesh.colors = mColor;
        mesh.normals = mNormals;
        mesh.triangles = mTriangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}
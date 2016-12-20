using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 这个是简单的 SubGeometry 显示
     */
    public class SimpleSubGeometry : MSubGeometry
    {
        public SimpleSubGeometry()
        {
            
        }

        public void buildGeometry()
        {
            this.setAutoDeriveVertexNormals(true);
            this.setAutoDeriveVertexTangents(true);

            Vector3[] vertices = new Vector3[4];

            // 第一次填充这个值，这是一条直线，不显示的
            //vertices.Add(1, 1, 1);
            //vertices.Add(2, 2, 2);
            //vertices.Add(3, 3, 3);
            //vertices.Add(4, 4, 4);

            vertices[0] = new Vector3(0, 0, 0);
            vertices[1] = new Vector3(1, 0, 0);
            vertices[2] = new Vector3(0, 1, 0);
            vertices[3] = new Vector3(1, 1, 0);
            this.updateVertexData(vertices);

            int[] indexs = new int[6];
            indexs[0] = 0;
            indexs[1] = 1;
            indexs[2] = 2;

            indexs[3] = 1;
            indexs[4] = 3;
            indexs[5] = 2;
            this.updateIndexData(indexs);

            Vector2[] uvs = new Vector2[4];
            uvs[0] = new Vector2(0, 0);
            uvs[1] = new Vector2(1, 0);
            uvs[2] = new Vector2(0, 1);
            uvs[3] = new Vector2(1, 1);
            this.updateUVData(uvs);

            MList<float> cols = new MList<float>();
            cols.Add(255, 0, 0, 1);
            cols.Add(0, 255, 0, 1);
            cols.Add(0, 0, 255, 1);
            cols.Add(0, 0, 255, 1);
        }
    }
}
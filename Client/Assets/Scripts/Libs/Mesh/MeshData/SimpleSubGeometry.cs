namespace SDK.Lib
{
    public class SimpleSubGeometry : MSubGeometry
    {
        public SimpleSubGeometry()
        {
            
        }

        public void buildGeometry()
        {
            this.setAutoDeriveVertexNormals(true);
            this.setAutoDeriveVertexTangents(true);

            MList<float> vertices = new MList<float>();

            // 第一次填充这个值，这是一条直线，不显示的
            //vertices.Add(1, 1, 1);
            //vertices.Add(2, 2, 2);
            //vertices.Add(3, 3, 3);
            //vertices.Add(4, 4, 4);

            vertices.Add(0, 0, 0);
            vertices.Add(1, 0, 0);
            vertices.Add(0, 1, 0);
            vertices.Add(1, 1, 0);
            this.updateVertexData(vertices);

            MList<int> indexs = new MList<int>();
            indexs.Add(0, 1, 2);
            indexs.Add(1, 3, 2);
            this.updateIndexData(indexs);

            MList<float> uvs = new MList<float>();
            uvs.Add(0, 0);
            uvs.Add(1, 0);
            uvs.Add(0, 1);
            uvs.Add(1, 1);
            this.updateUVData(uvs);

            MList<float> cols = new MList<float>();
            cols.Add(255, 0, 0, 1);
            cols.Add(0, 255, 0, 1);
            cols.Add(0, 0, 255, 1);
            cols.Add(0, 0, 255, 1);
        }
    }
}
using UnityEngine;

namespace SDK.Lib
{
    /**
     * brief 镜头基类
     */
    public class MLensBase
    {
        protected MMatrix3D m_matrix3D;     // 镜头变换矩阵        
        protected float m_farDist;          // Far 裁剪距离，默认 10000
        protected float m_nearDist;         // Near 裁剪距离，默认 100
        protected float m_aspectRatio;      // x/y 视口比例，默认 1.3333
        protected bool m_matrixInvalid;     // 投影矩阵是否无效
        protected MList<float> m_frustumCorners;    // 存放 Frustum 的四面体的八个顶点
        protected QuadMeshRender m_frustumRender;   // Frustum 渲染
        protected bool m_bShowBoundBox;             // 是否显示

        protected MLensBase()
        {
            m_bShowBoundBox = true;
            m_frustumRender = new QuadMeshRender(24);
            m_matrix3D = new MMatrix3D();

            m_farDist = 10000;
            m_nearDist = 100;

            m_aspectRatio = 1.3333f;
            m_matrixInvalid = true;
            m_frustumCorners = new MList<float>(8 * 3);         // Frustum 六面体的八个顶点，每个顶点占用 3 个单元
            for(int idx = 0; idx < 8 * 3; ++idx)
            {
                m_frustumCorners.Add(0);
            }
        }

        public bool getShowBoundBox()
        {
            return m_bShowBoundBox;
        }

        /**
         * @brief 设置 FOV，子类实现
         */
        virtual public void setFieldOfView(float value)
        {
            
        }

        public void setFarDist(float value)
        {
            m_farDist = value;
        }

        public void setNearDist(float value)
        {
            m_nearDist = value;
        }

        public void setAspectRatio(float value)
        {
            m_aspectRatio = value;
        }

        /**
         * @brief 设置焦距，子类实现
         */
        virtual public void setFocalLength(float value)
        {
            
        }

        /**
         * @brief 更新投影矩阵，子类实现
         */
        virtual public void updateMatrix()
		{
			
        }

        virtual public void buildPanel()
        {

        }

        public void updateFrustumRender()
        {
            m_frustumRender.clear();

            // 前面
            m_frustumRender.addVertex(m_frustumCorners[0], m_frustumCorners[1], m_frustumCorners[2]);
            m_frustumRender.addVertex(m_frustumCorners[3], m_frustumCorners[4], m_frustumCorners[5]);
            m_frustumRender.addVertex(m_frustumCorners[6], m_frustumCorners[7], m_frustumCorners[8]);
            m_frustumRender.addVertex(m_frustumCorners[9], m_frustumCorners[10], m_frustumCorners[11]);

            // 后面
            m_frustumRender.addVertex(m_frustumCorners[15], m_frustumCorners[16], m_frustumCorners[17]);
            m_frustumRender.addVertex(m_frustumCorners[12], m_frustumCorners[13], m_frustumCorners[14]);
            m_frustumRender.addVertex(m_frustumCorners[21], m_frustumCorners[22], m_frustumCorners[23]);
            m_frustumRender.addVertex(m_frustumCorners[18], m_frustumCorners[19], m_frustumCorners[20]);

            // 左面
            m_frustumRender.addVertex(m_frustumCorners[12], m_frustumCorners[13], m_frustumCorners[14]);
            m_frustumRender.addVertex(m_frustumCorners[0], m_frustumCorners[1], m_frustumCorners[2]);
            m_frustumRender.addVertex(m_frustumCorners[9], m_frustumCorners[10], m_frustumCorners[11]);
            m_frustumRender.addVertex(m_frustumCorners[21], m_frustumCorners[22], m_frustumCorners[23]);

            // 右面
            m_frustumRender.addVertex(m_frustumCorners[3], m_frustumCorners[4], m_frustumCorners[5]);
            m_frustumRender.addVertex(m_frustumCorners[15], m_frustumCorners[16], m_frustumCorners[17]);
            m_frustumRender.addVertex(m_frustumCorners[18], m_frustumCorners[19], m_frustumCorners[20]);
            m_frustumRender.addVertex(m_frustumCorners[6], m_frustumCorners[7], m_frustumCorners[8]);

            // 顶面
            m_frustumRender.addVertex(m_frustumCorners[0], m_frustumCorners[1], m_frustumCorners[2]);
            m_frustumRender.addVertex(m_frustumCorners[12], m_frustumCorners[13], m_frustumCorners[14]);
            m_frustumRender.addVertex(m_frustumCorners[15], m_frustumCorners[16], m_frustumCorners[17]);
            m_frustumRender.addVertex(m_frustumCorners[3], m_frustumCorners[4], m_frustumCorners[5]);

            // 底面
            m_frustumRender.addVertex(m_frustumCorners[21], m_frustumCorners[22], m_frustumCorners[23]);
            m_frustumRender.addVertex(m_frustumCorners[9], m_frustumCorners[10], m_frustumCorners[11]);
            m_frustumRender.addVertex(m_frustumCorners[6], m_frustumCorners[7], m_frustumCorners[8]);
            m_frustumRender.addVertex(m_frustumCorners[18], m_frustumCorners[19], m_frustumCorners[20]);

            m_frustumRender.buildIndexB();
            m_frustumRender.uploadGeometry();
        }

        /**
         * brief 测试输出投影矩阵
         */
        public void testLogMatrix(MMatrix3D projMat)
        {
            string str = "";
            for (int y = 0; y < 4; ++y)
            {
                for (int x = 0; x < 4; ++x)
                {
                    str += string.Format("m{0}{1} = ", y, x);
                    str += string.Format("{0} ,", projMat.m[y, x]);
                }
            }

            Debug.Log(str);
        }
    }
}
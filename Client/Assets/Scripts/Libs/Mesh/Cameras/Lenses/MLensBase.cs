using UnityEngine;

namespace SDK.Lib
{
    /**
     * brief 镜头基类
     */
    public class MLensBase
    {
        protected MMatrix3D m_matrix3D;     // 镜头变换矩阵
        // Far 裁剪距离，默认 10000
        protected float m_farDist;
        // Near 裁剪距离，默认 100
        protected float m_nearDist;
        // x/y viewport ratio - default 1.3333
        protected float m_aspectRatio;
        // 存放 Frustum 的四面体的八个顶点
        protected bool m_matrixInvalid;
        protected MList<float> m_frustumCorners;

        protected MLensBase()
        {
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
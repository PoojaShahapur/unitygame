using UnityEngine;
namespace SDK.Common
{
    /**
     * @brief 坐标转换
     */
    public class CoordConv
    {
        protected Plane mPlane;
        protected bool m_initPanel;
        protected Vector3 m_currentPos;         // 当前鼠标场景中的位置
        protected Ray m_ray;
        protected float m_dist = 0f;

        public Vector3 getCurMouseScenePos()
        {
            if (!m_initPanel)
            {
                m_initPanel = true;
                mPlane = new Plane(Vector3.up, Vector3.zero);
            }

            m_ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (mPlane.Raycast(m_ray, out m_dist))
            {
                m_currentPos = m_ray.GetPoint(m_dist);
            }

            return m_currentPos;
        }
    }
}
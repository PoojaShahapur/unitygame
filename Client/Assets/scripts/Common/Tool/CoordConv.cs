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
        protected RaycastHit m_hit;

        // 获取鼠标当前位置
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

        // 获取当前鼠标下的 GameObject
        public GameObject getUnderGameObject()
        {
            //定义一条从主相机射向鼠标位置的一条射向
            m_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //判断射线是否发生碰撞               
            if (Physics.Raycast(m_ray, out m_hit, 100))
            {
                return m_hit.collider.gameObject;
            }

            return null;
        }
    }
}
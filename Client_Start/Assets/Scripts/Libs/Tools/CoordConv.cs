using UnityEngine;

namespace SDK.Lib
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
        protected int m_layMask;

        public CoordConv()
        {
            m_layMask = UtilApi.NameToLayer("UGUI");
        }

        // 获取鼠标当前位置
        public Vector3 getCurTouchScenePos()
        {
            if (!m_initPanel)
            {
                m_initPanel = true;
                mPlane = new Plane(Vector3.up, Vector3.zero);
            }

            // 这个地方是能获取鼠标每时每刻的位置，触碰是不能获取到的
            m_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //m_ray = UICamera.currentRay;

            if (mPlane.Raycast(m_ray, out m_dist))
            {
                m_currentPos = m_ray.GetPoint(m_dist);
            }

            return m_currentPos;
        }

        // 获取当前鼠标下的 GameObject
        public GameObject getUnderGameObject()
        {
            /*
            //定义一条从主相机射向鼠标位置的一条射向
            m_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //判断射线是否发生碰撞               
            if (Physics.Raycast(m_ray, out m_hit, 100))
            {
                return m_hit.collider.gameObject;
            }

            return null;
            */

            if(UICamera.isOverUI)
            {
                return UICamera.hoveredObject;
            }

            return null;
        }

        // 获取屏幕中心点与 Plane 相交的点
        public Vector3 getScreenCenterInPlanePos()
        {
            if (!m_initPanel)
            {
                m_initPanel = true;
                mPlane = new Plane(Vector3.up, Vector3.zero);
            }

            m_ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

            if (mPlane.Raycast(m_ray, out m_dist))
            {
                m_currentPos = m_ray.GetPoint(m_dist);
            }

            return m_currentPos;
        }

        // 通过屏幕点获取光线远场景的交点
        public Vector3 getScenePointByScreenPoint(Vector3 screenPoint, Camera currentCamera)
        {
            Ray ray = currentCamera.ScreenPointToRay(screenPoint);
            RaycastHit lastHit;
            int mask = UtilApi.NameToLayer("Everything");
            float dist = 10000;
            Vector3 lastWorldPosition = Vector3.zero;

            if (Physics.Raycast(ray, out lastHit, dist, mask))
            {
                lastWorldPosition = lastHit.point;
            }

            return lastWorldPosition;
        }
    }
}
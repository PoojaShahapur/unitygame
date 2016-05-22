using SDK.Lib;
using UnityEngine;

namespace UnitTest
{
    public class TestIOControl
    {
        protected Vector2 m_lastFirPos;
        protected Vector2 m_curFirPos;

        protected Vector2 m_lastSndPos;
        protected Vector2 m_curSndPos;

        public void run()
        {
            this.testDrag();
        }

        protected void testDrag()
        {
            // 测试拖动的时候，如果一个 GameObject 开始拖动，一定要设置 Raycast 不能点选到，否则不能检测到拖动 GameObject 后面的 GameObject
        }

        // 测试同时进行多个触摸
        protected void testMultiTouch()
        {
            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                m_curFirPos = touch.position;

                if(Input.touchCount > 1)
                {
                    touch = Input.GetTouch(1);
                    m_curSndPos = touch.position;

                    float curDist = Vector2.Distance(m_curFirPos, m_curSndPos);
                    float lastDist = Vector2.Distance(m_lastFirPos, m_lastSndPos);
                    float delta = curDist - lastDist;
                    // 分发多触屏改变
                    // disp
                }
            }

            m_lastFirPos = m_curFirPos;
            m_lastSndPos = m_curSndPos;
        }
    }
}
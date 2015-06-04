using System;

namespace SDK.Lib
{
    /**
     * @brief 可以移动的特效
     */
    public class MoveEffect : EffectBase
    {
        protected EventDispatch m_moveDestEventDispatch;         // 移动到目标事件分发，注意不是

        public MoveEffect()
        {
            m_moveDestEventDispatch = new AddOnceAndCallOnceEventDispatch();
        }

        public EventDispatch moveDestEventDispatch
        {
            get
            {
                return m_moveDestEventDispatch;
            }
        }

        public override void dispose()
        {
            m_moveDestEventDispatch.clearEventHandle();
            m_moveDestEventDispatch = null;

            base.dispose();
        }

        override public void addMoveDestEventHandle(Action<IDispatchObject> dispObj)
        {
            m_moveDestEventDispatch.addEventHandle(dispObj);
        }
    }
}
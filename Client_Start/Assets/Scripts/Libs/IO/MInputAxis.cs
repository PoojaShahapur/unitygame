namespace SDK.Lib
{
    /**
     * @brief Edit -- Project Settings -- Input 中设置的 Axis
     */
    public class MInputAxis : IDispatchObject
    {
        // Edit -- Project Settings -- Input 中设置的 Axis
        public static MInputAxis Horizontal = new MInputAxis("Horizontal");
        public static MInputAxis Vertical = new MInputAxis("Vertical");

        protected string mAxisName;
        protected float mLastValue;
        protected float mCurrentValue;

        private AddOnceEventDispatch mOnAxisBeganDispatch;         // Axis 开始
        private AddOnceEventDispatch mOnAxisMovedDispatch;         // Axis 状态，但是移动
        private AddOnceEventDispatch mOnAxisStationaryDispatch;    // Axis 状态但是不移动
        private AddOnceEventDispatch mOnAxisEndedDispatch;         // Axis 结束
        private AddOnceEventDispatch mOnAxisCanceledDispatch;      // Axis 取消
        private AddOnceEventDispatch mOnAxisPressedDispatch;       // Axis 按下

        public MInputAxis(string axisName)
        {
            this.mAxisName = axisName;
            this.mLastValue = 0;
            this.mCurrentValue = 0;

            this.mOnAxisBeganDispatch = new AddOnceEventDispatch();
            this.mOnAxisMovedDispatch = new AddOnceEventDispatch();
            this.mOnAxisStationaryDispatch = new AddOnceEventDispatch();
            this.mOnAxisEndedDispatch = new AddOnceEventDispatch();
            this.mOnAxisCanceledDispatch = new AddOnceEventDispatch();
        }

        public float getCurrentValue()
        {
            return this.mCurrentValue;
        }

        public void onTick(float delta)
        {
            this.mLastValue = this.mCurrentValue;
            this.mCurrentValue = UnityEngine.Input.GetAxis(this.mAxisName);

            if (0 != this.mCurrentValue)
            {
                if (0 == this.mLastValue)
                {
                    this.handleAxisBegan();
                }
                else if(this.mLastValue != this.mCurrentValue)
                {
                    this.handleAxisMoved();
                    this.handleAxisPressed();
                }
                else
                {
                    this.handleAxisStationary();
                    this.handleAxisPressed();
                }
            }
            else
            {
                if (0 != this.mLastValue)
                {
                    this.handleAxisEnded();
                }
                else
                {
                    
                }
            }
        }

        public bool isPosChanged()
        {
            return this.mLastValue != this.mCurrentValue;
        }

        public void addAxisListener(EventId evtID, MAction<IDispatchObject> handle)
        {
            if (EventId.AXISBEGIN_EVENT == evtID)
            {
                this.mOnAxisBeganDispatch.addEventHandle(null, handle);
            }
            else if (EventId.AXISMOVED_EVENT == evtID)
            {
                this.mOnAxisMovedDispatch.addEventHandle(null, handle);
            }
            else if (EventId.AXISSTATIONARY_EVENT == evtID)
            {
                this.mOnAxisStationaryDispatch.addEventHandle(null, handle);
            }
            else if (EventId.AXISENDED_EVENT == evtID)
            {
                this.mOnAxisEndedDispatch.addEventHandle(null, handle);
            }
            else if (EventId.AXISCANCELED_EVENT == evtID)
            {
                this.mOnAxisCanceledDispatch.addEventHandle(null, handle);
            }
            else if (EventId.AXISPRESSED_EVENT == evtID)
            {
                this.mOnAxisPressedDispatch.addEventHandle(null, handle);
            }
        }

        public void removeAxisListener(EventId evtID, MAction<IDispatchObject> handle)
        {
            if (EventId.AXISBEGIN_EVENT == evtID)
            {
                this.mOnAxisBeganDispatch.removeEventHandle(null, handle);
            }
            else if (EventId.AXISMOVED_EVENT == evtID)
            {
                this.mOnAxisMovedDispatch.removeEventHandle(null, handle);
            }
            else if (EventId.AXISSTATIONARY_EVENT == evtID)
            {
                this.mOnAxisStationaryDispatch.removeEventHandle(null, handle);
            }
            else if (EventId.AXISENDED_EVENT == evtID)
            {
                this.mOnAxisEndedDispatch.removeEventHandle(null, handle);
            }
            else if (EventId.AXISCANCELED_EVENT == evtID)
            {
                this.mOnAxisCanceledDispatch.removeEventHandle(null, handle);
            }
            else if (EventId.AXISPRESSED_EVENT == evtID)
            {
                this.mOnAxisPressedDispatch.removeEventHandle(null, handle);
            }
        }

        // 是否还有需要处理的事件
        public bool hasEventHandle()
        {
            if (this.mOnAxisBeganDispatch.hasEventHandle())
            {
                return true;
            }
            if (this.mOnAxisMovedDispatch.hasEventHandle())
            {
                return true;
            }
            if (this.mOnAxisStationaryDispatch.hasEventHandle())
            {
                return true;
            }
            if (this.mOnAxisEndedDispatch.hasEventHandle())
            {
                return true;
            }
            if (this.mOnAxisCanceledDispatch.hasEventHandle())
            {
                return true;
            }
            if (this.mOnAxisPressedDispatch.hasEventHandle())
            {
                return true;
            }

            return false;
        }

        public void handleAxisBegan()
        {
            if (null != this.mOnAxisBeganDispatch)
            {
                this.mOnAxisBeganDispatch.dispatchEvent(this);
            }
        }

        public void handleAxisMoved()
        {
            if (this.isPosChanged())
            {
                if (null != this.mOnAxisMovedDispatch)
                {
                    this.mOnAxisMovedDispatch.dispatchEvent(this);
                }
            }
        }

        public void handleAxisStationary()
        {
            if (null != this.mOnAxisStationaryDispatch)
            {
                this.mOnAxisStationaryDispatch.dispatchEvent(this);
            }
        }

        public void handleAxisEnded()
        {
            if (null != this.mOnAxisEndedDispatch)
            {
                this.mOnAxisEndedDispatch.dispatchEvent(this);
            }
        }

        public void handleAxisCanceled()
        {
            if (null != this.mOnAxisCanceledDispatch)
            {
                this.mOnAxisCanceledDispatch.dispatchEvent(this);
            }
        }

        public void handleAxisPressed()
        {
            if (null != this.mOnAxisPressedDispatch)
            {
                this.mOnAxisPressedDispatch.dispatchEvent(this);
            }
        }
    }
}
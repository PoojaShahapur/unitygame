using UnityEngine;

namespace SDK.Lib
{
    public class MMouseOrTouch : IDispatchObject
    {
        public KeyCode mKey = KeyCode.None;
        public Vector2 mPos;             // Current position of the mouse or touch event
        public Vector2 mLastPos;         // Previous position of the mouse or touch event
        public Vector2 mDelta;           // Delta since last update
        public Vector2 mTotalDelta;      // Delta since the event started being tracked

        public Camera mPressedCam;       // Camera that the OnPress(true) was fired with

        public GameObject mLast;         // Last object under the touch or mouse
        public GameObject mCurrent;      // Current game object under the touch or mouse
        public GameObject mPressed;      // Last game object to receive OnPress
        public GameObject mDragged;      // Game object that's being dragged

        public float mPressTime = 0f;    // When the touch event started
        public float mClickTime = 0f;    // The last time a click event was sent out

        public MClickNotification mClickNotification = MClickNotification.Always;
        public bool mTouchBegan = true;
        public bool mPressStarted = false;
        public bool mDragStarted = false;
        public int mIgnoreDelta = 0;

        protected float mSensitivity;   // 灵敏度

        /// <summary>
        /// Delta time since the touch operation started.
        /// </summary>
        public float deltaTime { get { return UtilIO.time - mPressTime; } }

        public MMouseOrTouch()
        {
            this.mSensitivity = 0.1f;
        }

        /// <summary>
        /// Returns whether this touch is currently over a UI element.
        /// </summary>
        public bool isOverUI
        {
            get
            {
                // UGUI UI 判断当前摄像机是否在处理 UI 事件
                return UtilApi.IsPointerOverGameObjectRaycast();
            }
        }

        // 获取 X 方向移动的距离
        public float getXOffset()
        {
            return (mPos.x - mLastPos.x) * this.mSensitivity;
        }

        // 获取 X 方向移动的距离
        public float getYOffset()
        {
            return (mPos.y - mLastPos.y) * this.mSensitivity;
        }

        public bool isPosChanged()
        {
            if (this.mPos.x != this.mLastPos.x || this.mPos.y != this.mLastPos.y)
            {
                return true;
            }

            return false;
        }
    }
}
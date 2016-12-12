using UnityEngine;

namespace SDK.Lib
{
    public class MMouseOrTouch
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

        /// <summary>
        /// Delta time since the touch operation started.
        /// </summary>
        public float deltaTime { get { return UtilIO.time - mPressTime; } }

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
    }
}
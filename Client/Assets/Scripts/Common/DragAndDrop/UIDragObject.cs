//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Allows dragging of the specified target object by mouse or touch, optionally limiting it to be within the UIPanel's clipped rectangle.
/// </summary>

/**
 * @brief 拖动与当前组件的 enable 是否开启无关
 */
//[ExecuteInEditMode]
//[AddComponentMenu("NGUI/Interaction/Drag Object")]
public class UIDragObject : MonoBehaviour
{
	public enum DragEffect
	{
		None,
		Momentum,
		MomentumAndSpring,
	}

	/// <summary>
	/// Target object that will be dragged.
	/// </summary>

	public Transform target;

	/// <summary>
	/// Panel that will be used for constraining the target.
	/// </summary>

	//public UIPanel panelRegion;

	/// <summary>
	/// Scale value applied to the drag delta. Set X or Y to 0 to disallow dragging in that direction.
	/// </summary>

	public Vector3 dragMovement { get { return scale; } set { scale = value; } }

	/// <summary>
	/// Momentum added from the mouse scroll wheel.
	/// </summary>

	public Vector3 scrollMomentum = Vector3.zero;

	/// <summary>
	/// Whether the dragging will be restricted to be within the parent panel's bounds.
	/// </summary>

	public bool restrictWithinPanel = false;

	/// <summary>
	/// Rectangle to be used as the draggable object's bounds. If none specified, all widgets' bounds get added up.
	/// </summary>

	//public UIRect contentRect = null;

	/// <summary>
	/// Effect to apply when dragging.
	/// </summary>

	public DragEffect dragEffect = DragEffect.MomentumAndSpring;

	/// <summary>
	/// How much momentum gets applied when the press is released after dragging.
	/// </summary>

	public float momentumAmount = 35f;

	// Obsolete property. Use 'dragMovement' instead.
	[SerializeField] protected Vector3 scale = new Vector3(1f, 0f, 1f);

	// Obsolete property. Use 'scrollMomentum' instead.
	[SerializeField][HideInInspector] float scrollWheelFactor = 0f;

	Plane mPlane;
	Vector3 mTargetPos;
	Vector3 mLastPos;
	Vector3 mMomentum = Vector3.zero;
	Vector3 mScroll = Vector3.zero;
	Bounds mBounds;
	int mTouchID = 0;
	bool mStarted = false;
	bool mPressed = false;

    // 开始拖动的事件
    public Action m_startDragDisp;
    // 拖动中分发
    public Action m_moveDisp;
    // 拖动结束回调
    public Action m_dropEndDisp;
    // 是否可以移动
    public Func<bool> m_canMoveDisp;
    public Vector3 m_planePt = Vector3.zero;

	/// <summary>
	/// Auto-upgrade the legacy data.
	/// </summary>

	void OnEnable ()
	{
		if (scrollWheelFactor != 0f)
		{
			scrollMomentum = scale * scrollWheelFactor;
			scrollWheelFactor = 0f;
		}

		//if (contentRect == null && target != null && Application.isPlaying)
		//{
		//	UIWidget w = target.GetComponent<UIWidget>();
		//	if (w != null) contentRect = w;
		//}
	}

	void OnDisable () { mStarted = false; }

	/// <summary>
	/// Find the panel responsible for this object.
	/// </summary>

	void FindPanel ()
	{
		//panelRegion = (target != null) ? UIPanel.Find(target.transform.parent) : null;
		//if (panelRegion == null) restrictWithinPanel = false;
        restrictWithinPanel = false;
	}

	/// <summary>
	/// Recalculate the bounds of the dragged content.
	/// </summary>

	void UpdateBounds ()
	{
        //if (contentRect)
        //{
        //    Transform t = panelRegion.cachedTransform;
        //    Matrix4x4 toLocal = t.worldToLocalMatrix;
        //    Vector3[] corners = contentRect.worldCorners;
        //    for (int i = 0; i < 4; ++i) corners[i] = toLocal.MultiplyPoint3x4(corners[i]);
        //    mBounds = new Bounds(corners[0], Vector3.zero);
        //    for (int i = 1; i < 4; ++i) mBounds.Encapsulate(corners[i]);
        //}
        //else
        //{
        //    mBounds = NGUIMath.CalculateRelativeWidgetBounds(panelRegion.cachedTransform, target);
        //}
	}

	/// <summary>
	/// Create a plane on which we will be performing the dragging.
	/// </summary>

	void OnPress (bool pressed)
	{
		if (enabled && NGUITools.GetActive(gameObject) && target != null)
		{
			if (pressed)
			{
				if (!mPressed)
				{
					// Remove all momentum on press
					mTouchID = UICamera.currentTouchID;
					mPressed = true;
					mStarted = false;
					CancelMovement();

					//if (restrictWithinPanel && panelRegion == null) FindPanel();
					if (restrictWithinPanel) UpdateBounds();

					// Disable the spring movement
					CancelSpring();

					// Create the plane to drag along
					//Transform trans = UICamera.currentCamera.transform;
					//mPlane = new Plane((panelRegion != null ? panelRegion.cachedTransform.rotation : trans.rotation) * Vector3.back, UICamera.lastWorldPosition);
                    //mPlane = new Plane(trans.rotation * Vector3.back, UICamera.lastWorldPosition);
                    //mPlane = new Plane(Vector3.up, Vector3.zero);
                    mPlane = new Plane(Vector3.up, m_planePt);
				}
			}
			else if (mPressed && mTouchID == UICamera.currentTouchID)
			{
				mPressed = false;
                CancelMovement();

				if (restrictWithinPanel && dragEffect == DragEffect.MomentumAndSpring)
				{
					//if (panelRegion.ConstrainTargetToBounds(target, ref mBounds, false))
					//	CancelMovement();
				}

                if(m_dropEndDisp != null)
                {
                    m_dropEndDisp();
                }
			}
		}
	}

	/// <summary>
	/// Drag the object along the plane.
	/// </summary>

	void OnDrag (Vector2 delta)
	{
		if (mPressed && mTouchID == UICamera.currentTouchID && enabled && NGUITools.GetActive(gameObject) && target != null)
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;

			Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
			float dist = 0f;

			if (mPlane.Raycast(ray, out dist))
			{
				Vector3 currentPos = ray.GetPoint(dist);
				Vector3 offset = currentPos - mLastPos;
				mLastPos = currentPos;

				if (!mStarted)
				{
					mStarted = true;
					offset = Vector3.zero;
                    if(m_startDragDisp != null)
                    {
                        m_startDragDisp();
                    }
				}

				if (offset.x != 0f || offset.z != 0f)
				{
					//offset = target.InverseTransformDirection(offset);
					offset.Scale(scale);
					//offset = target.TransformDirection(offset);
				}

				// Adjust the momentum
				if (dragEffect != DragEffect.None)
					mMomentum = Vector3.Lerp(mMomentum, mMomentum + offset * (0.01f * momentumAmount), 0.67f);

				// Adjust the position and bounds
				Vector3 before = target.localPosition;
				Move(offset);

				// We want to constrain the UI to be within bounds
				if (restrictWithinPanel)
				{
					mBounds.center = mBounds.center + (target.localPosition - before);

					// Constrain the UI to the bounds, and if done so, immediately eliminate the momentum
					//if (dragEffect != DragEffect.MomentumAndSpring && panelRegion.ConstrainTargetToBounds(target, ref mBounds, true))
					//	CancelMovement();
				}
			}
		}
	}

	/// <summary>
	/// Move the dragged object by the specified amount.
	/// </summary>

	void Move (Vector3 worldDelta)
	{
        //if (panelRegion != null)
        //{
        //    mTargetPos += worldDelta;
        //    target.position = mTargetPos;

        //    Vector3 after = target.localPosition;
        //    after.x = Mathf.Round(after.x);
        //    after.y = Mathf.Round(after.y);
        //    target.localPosition = after;

        //    UIScrollView ds = panelRegion.GetComponent<UIScrollView>();
        //    if (ds != null) ds.UpdateScrollbars(true);
        //}
		//else target.position += worldDelta;
        if ((m_canMoveDisp != null && m_canMoveDisp()) || m_canMoveDisp == null)
        {
            worldDelta.y = 0;           // 不计算 y 值
            target.position += worldDelta;

            if (m_moveDisp != null)
            {
                m_moveDisp();
            }
        }
	}

	/// <summary>
	/// Apply the dragging momentum.
	/// </summary>

	void LateUpdate ()
	{
#if UNITY_EDITOR
		if (!Application.isPlaying) return;
#endif
		if (target == null) return;
		float delta = RealTime.deltaTime;

		mMomentum -= mScroll;
		mScroll = NGUIMath.SpringLerp(mScroll, Vector3.zero, 20f, delta);

		// No momentum? Exit.
		if (mMomentum.magnitude < 0.0001f) return;

		if (!mPressed)
		{
			// Apply the momentum
			//if (panelRegion == null) FindPanel();

			Move(NGUIMath.SpringDampen(ref mMomentum, 9f, delta));

            //if (restrictWithinPanel && panelRegion != null)
            //{
            //    UpdateBounds();

            //    if (panelRegion.ConstrainTargetToBounds(target, ref mBounds, dragEffect == DragEffect.None))
            //    {
            //        CancelMovement();
            //    }
            //    else CancelSpring();
            //}

			// Dampen the momentum
			NGUIMath.SpringDampen(ref mMomentum, 9f, delta);

			// Cancel all movement (and snap to pixels) at the end
			if (mMomentum.magnitude < 0.0001f) CancelMovement();
		}
		else NGUIMath.SpringDampen(ref mMomentum, 9f, delta);
	}

	/// <summary>
	/// Cancel all movement.
	/// </summary>

	public void CancelMovement ()
	{
        //if (target != null)
        //{
        //    Vector3 pos = target.localPosition;
        //    pos.x = Mathf.RoundToInt(pos.x);
        //    pos.y = Mathf.RoundToInt(pos.y);
        //    pos.z = Mathf.RoundToInt(pos.z);
        //    target.localPosition = pos;
        //}
		mTargetPos = (target != null) ? target.position : Vector3.zero;
		mMomentum = Vector3.zero;
		mScroll = Vector3.zero;
	}

	/// <summary>
	/// Cancel the spring movement.
	/// </summary>

	public void CancelSpring ()
	{
		//SpringPosition sp = target.GetComponent<SpringPosition>();
		//if (sp != null) sp.enabled = false;
	}

	/// <summary>
	/// If the object should support the scroll wheel, do it.
	/// </summary>

	void OnScroll (float delta)
	{
		if (enabled && NGUITools.GetActive(gameObject))
			mScroll -= scrollMomentum * (delta * 0.05f);
	}

    // 是否是拖动的，有时候可能移动时补间动画
    public bool isDrag()
    {
        return mPressed;
    }

    // 重置数据
    public void reset()
    {
        mTouchID = 0;
        mPressed = false;
        mStarted = false;
        CancelMovement();
    }
}

using UnityEngine;

/// <summary>
/// Attach this script to a child of a draggable window to make it tilt as it's dragged.
/// Look at how it's used in Example 6.
/// </summary>

//[AddComponentMenu("NGUI/Examples/Window Drag Tilt")]
public class WindowDragTilt : MonoBehaviour
{
	public int updateOrder = 0;
	public float degrees = 30f;

	Vector3 mLastPos;
	Transform mTrans;
	float mAngleY = 0f;
    float mAngleZ = 0f;

	void OnEnable ()
	{
		mTrans = transform;
		mLastPos = mTrans.position;
	}

	void Update ()
	{
		Vector3 deltaPos = mTrans.position - mLastPos;
		mLastPos = mTrans.position;

		mAngleY += deltaPos.x * degrees;
		mAngleY = NGUIMath.SpringLerp(mAngleY, 0f, 20f, Time.deltaTime);

        mAngleZ += deltaPos.x * degrees;
        mAngleZ = NGUIMath.SpringLerp(mAngleZ, 0f, 20f, Time.deltaTime);

        mTrans.localRotation = Quaternion.Euler(0f, mAngleY, -mAngleZ);
	}
}

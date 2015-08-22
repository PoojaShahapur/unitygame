using UnityEngine;

/// <summary>
/// Attach this script to a child of a draggable window to make it tilt as it's dragged.
/// Look at how it's used in Example 6.
/// </summary>

/**
 * @brief 拖动与当前组件的 enable 是否开启有关
 */
//[AddComponentMenu("NGUI/Examples/Window Drag Tilt")]
public class WindowDragTilt : MonoBehaviour
{
	public int updateOrder = 0;
	public float degrees = 30f;

	protected Vector3 mLastPos;
    protected Transform mTrans;

    protected float mAngleX = 0f;
    protected float mAngleY = 0f;
    protected float mAngleZ = 0f;

	void OnEnable ()
	{
        resetPos();
	}

	void Update ()
	{
        //if(canUpdate())
        //{
            titleUpdate();
        //}
	}

    protected void titleUpdate()
    {
        Vector3 deltaPos = mTrans.position - mLastPos;
        mLastPos = mTrans.position;

        mAngleX += deltaPos.z * degrees;
        mAngleX = NGUIMath.SpringLerp(mAngleX, 0f, 20f, Time.deltaTime);

        //mAngleY += deltaPos.x * degrees;
        //mAngleY = NGUIMath.SpringLerp(mAngleY, 0f, 20f, Time.deltaTime);

        mAngleZ += deltaPos.x * degrees;
        mAngleZ = NGUIMath.SpringLerp(mAngleZ, 0f, 20f, Time.deltaTime);

        //mTrans.localRotation = Quaternion.Euler(0f, mAngleY, -mAngleZ);
        mTrans.localRotation = Quaternion.Euler(mAngleX, 0, -mAngleZ);
    }

    protected bool canUpdate()
    {
        // 只有在按下拖放的时候才起多用
        if (gameObject.GetComponent<UIDragObject>() != null)
        {
            return gameObject.GetComponent<UIDragObject>().isDrag();
        }
        return false;
    }

    public void resetPos()
    {
        mAngleX = 0f;
        mAngleY = 0f;
        mAngleZ = 0f;
        mTrans = transform;
        mLastPos = mTrans.position;
    }
}
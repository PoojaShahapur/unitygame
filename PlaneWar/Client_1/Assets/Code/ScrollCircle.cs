using UnityEngine;
using UnityEngine.UI;

public class ScrollCircle : ScrollRect
{
    protected float mRadius = 0f;

    protected override void Start()
    {
        base.Start();
        //计算摇杆块的半径
        mRadius = (transform as RectTransform).sizeDelta.x * 0.5f;
    }

    public override void OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        //base.OnDrag(eventData);
        Vector2 contentPostion;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.transform as RectTransform, eventData.position, null, out contentPostion);
        if (contentPostion.magnitude > mRadius)
        {
            contentPostion = contentPostion.normalized * mRadius;
            SetContentAnchoredPosition(contentPostion);
        }
        else
        {
            this.content.anchoredPosition = contentPostion;
        }
    }
}

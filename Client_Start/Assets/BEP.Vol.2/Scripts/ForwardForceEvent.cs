using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ForwardForceEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public UnityEngine.UI.Scrollbar fward_force_Op;

    // 延迟时间  
    private float delay = 0.15f;

    // 按钮是否是按下状态  
    private bool isDown = false;

    // 按钮最后一次是被按住状态时候的时间  
    private float lastIsDownTime;

    void Update()
    {
        // 如果按钮是被按下状态  
        if (isDown)
        {
            // 当前时间 -  按钮最后一次被按下的时间 > 延迟时间0.2秒  
            if (Time.time - lastIsDownTime > delay)
            {
                // 触发长按方法  
                CreatePlayer._Instace.SetIsPressForwardForceBtn(true);
                // 记录按钮最后一次被按下的时间  
                lastIsDownTime = Time.time;
            }

            CreatePlayer._Instace.SetForwardForce(fward_force_Op.value);
        }
    }

    // 当按钮被按下后系统自动调用此方法  
    public void OnPointerDown(PointerEventData eventData)
    {
        isDown = true;
        lastIsDownTime = Time.time;
    }

    // 当按钮抬起的时候自动调用此方法  
    public void OnPointerUp(PointerEventData eventData)
    {
        isDown = false;
        CreatePlayer._Instace.SetIsPressForwardForceBtn(false);
    }

    // 当鼠标从按钮上离开的时候自动调用此方法  
    public void OnPointerExit(PointerEventData eventData)
    {
        isDown = false;
        CreatePlayer._Instace.SetIsPressForwardForceBtn(false);
    }
}

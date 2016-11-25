using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {
    public GUISkin guiSkin; // choose a guiStyle (Important!)
    
    public Color color = Color.white;   // choose font color/size
    public float fontSize = 10;
    private bool isVisible = false;

    void OnBecameVisible()
    {
        //可见状态下你要执行 的东西
        isVisible = true;
    }

    void OnBecameInvisible()
    {
        //不可见状态下你要执行的东西
        isVisible = false;
    }

    void OnGUI()
    {
        if (!isVisible) return;
        var food = GetComponent<Food>();
        
        string tempName = "";
        tempName = this.gameObject.GetComponent<Food>().entity.m_name;        

        //var myCollider = GetComponent<Collider>();        
        Vector3 ballHeadWorldPosition = new Vector3(transform.position.x, transform.position.y +  transform.localScale.y / 2 , transform.position.z);//获得球头顶的位置
        Vector2 ballHeadSurfacePosition = Camera.main.WorldToScreenPoint(ballHeadWorldPosition);//转换成2D坐标
        ballHeadSurfacePosition = new Vector2(ballHeadSurfacePosition.x, Screen.height - ballHeadSurfacePosition.y);//获得真实坐标
        //             boxPosition = Camera.main.WorldToScreenPoint(transform.position);
        //             boxPosition.y = Screen.height - boxPosition.y;
        //             boxPosition.x -= boxW * 0.1f;
        //             boxPosition.y -= boxH * 0.5f;
        //             guiSkin.box.fontSize = 10;       
        GUI.skin = guiSkin;
        GUI.contentColor = color;

        Vector2 nameSize = guiSkin.label.CalcSize(new GUIContent(tempName));

        //GUI.Box(new Rect(boxPosition.x - content.x / 2 * offsetX, boxPosition.y + offsetY, content.x, content.y), playerName);        
        GUI.Label(new Rect(ballHeadSurfacePosition.x - nameSize.x / 2, ballHeadSurfacePosition.y - nameSize.y, nameSize.x, nameSize.y), tempName);        
    }
}

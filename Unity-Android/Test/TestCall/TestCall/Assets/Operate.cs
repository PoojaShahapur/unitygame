using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * @brief http://blog.csdn.net/crazy1235/article/details/46733221
 */
public class Operate : MonoBehaviour
{
    public Transform target;
    public Text label;
    public bool flag = true;
    /// <summary>
    /// 定义旋转速度
    /// </summary>
    public float RotateSpeed = 45;
    // Use this for in  itialization
    void Start()
    {
        //Debug.Log("hello");
        this.name = "Manager";
        GetData();
    }
    /// <summary>
    /// 通过调用android中的方法获取name，并为label赋值
    /// </summary>
    void GetData()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        string name = jo.Call<string>("getName", "成功调用android方法");
        label.text = name;
    }

    // Update is called once per frame
    void Update()
    {
        //target.Rotate (Vector3.up * Time.deltaTime * RotateSpeed);
    }
    void OnClick()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        if (flag)
        {
            target.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            flag = false;
            //label.text = "123456";
        }
        else
        {
            target.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            flag = true;
            //label.text = "000000";
        }
    }
    /// <summary>
    /// 顶掉之前的scene
    /// </summary>
    void Unload()
    {
        Application.LoadLevel(1);
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("makePauseUnity");
    }

    /// <summary>
    /// 放大
    /// </summary>
    void ZoomIn()
    {
        target.localScale = new Vector3(0.75f, 0.75f, 0.75f);
    }

    /// <summary>
    /// 缩小
    /// </summary>
    void ZoomOut()
    {
        target.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
}
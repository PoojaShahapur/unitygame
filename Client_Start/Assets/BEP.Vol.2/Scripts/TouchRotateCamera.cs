using UnityEngine;
using System.Collections;

public class TouchRotateCamera : MonoBehaviour
{
    public float distance_Z = 10.0f;//主相机与目标物体之间的水平距离
    public float distance_Y = 0.5f;//主相机与目标物体之间的垂直距离
    private float eulerAngles_x = 0.0f;
    private float eulerAngles_y = 0.0f;

    //初始位置
    private float old_distance_Z = 10.0f;
    private float old_distance_Y = 0.5f;

    public float xSpeed = 600.0f;
    public float ySpeed = 600.0f;
    public float yMinLimit = 10.0f;
    public float yMaxLimit = 90.0f;

    public float MoveSensitivity = 1.5f;
    public float limit_radius_value = 50.0f;//超过后维持球大小不变
    public float MoveSensitivity2 = 1.2f;
    public float limit_radius_value2 = 70.0f;
    private float critical_value = 0.0f;

    private float XChange;
    private float rotatespeed = 5.0f;
    private float YChange;

    public UnityEngine.UI.Scrollbar fward_force_Op;
    private float fward_force_Op_x_min = 1.0f;
    private float fward_force_Op_x_max = 1.0f;
    private float fward_force_Op_y_min = 1.0f;
    private float fward_force_Op_y_max = 1.0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        eulerAngles_x = angles.y;
        eulerAngles_y = angles.x;

        old_distance_Y = distance_Y;
        old_distance_Z = distance_Z;

        critical_value = Mathf.Pow(limit_radius_value, MoveSensitivity);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;//设置屏幕永远亮着

        //按钮四个角的屏幕坐标 顺序是左下、左上、右上、右下
        Vector3[] corners = new Vector3[4];
        fward_force_Op.GetComponent<RectTransform>().GetWorldCorners(corners);
        fward_force_Op_x_min = corners[0].x;
        fward_force_Op_x_max = corners[2].x;
        fward_force_Op_y_min = corners[0].y;
        fward_force_Op_y_max = corners[2].y;
    }

    void Update()
    {
        //Chose();
        if (CreatePlayer._Instace.player.GetComponent<Transform>() && CreatePlayer._Instace.player.GetComponent<Transform>().GetComponent<Player>().controlType != ControlType.GravitytouchControl)
            return;        

        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).position.x >= fward_force_Op_x_min && Input.GetTouch(0).position.x <= fward_force_Op_x_max &&
                Input.GetTouch(0).position.y>= fward_force_Op_y_min && Input.GetTouch(0).position.y <= fward_force_Op_y_max)
            {
                //Debug.Log("触摸在ui上 " + " x: " + Input.mousePosition.x + " y: " + Input.mousePosition.y + "  x_min: " + fward_force_Op_x_min + "  x_max: " + fward_force_Op_x_max + "  y_min: " + fward_force_Op_y_min + "  y_max: " + fward_force_Op_y_max);
            }
            else
            {
                eulerAngles_x += Input.GetTouch(0).deltaPosition.x * xSpeed * Time.deltaTime / 60;
                eulerAngles_y -= Input.GetTouch(0).deltaPosition.y * ySpeed * Time.deltaTime / 60;
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    if (Input.GetTouch(0).deltaPosition.x > rotatespeed)
                    {
                        XChange = rotatespeed;
                    }
                    else if (Input.GetTouch(0).deltaPosition.x < -rotatespeed)
                    {
                        XChange = -rotatespeed;
                    }
                    else
                    {
                        XChange = Input.GetTouch(0).deltaPosition.x;
                    }

                    if (Input.GetTouch(0).deltaPosition.y > rotatespeed)
                    {
                        YChange = rotatespeed;
                    }
                    else if (Input.GetTouch(0).deltaPosition.y < -rotatespeed)
                    {
                        YChange = -rotatespeed;
                    }
                    else
                    {
                        YChange = Input.GetTouch(0).deltaPosition.y;
                    }
                }
                eulerAngles_x += XChange;
                if (XChange > 0)
                {
                    XChange -= Time.deltaTime * rotatespeed;
                }
                else if (XChange < 0)
                {
                    XChange += Time.deltaTime * rotatespeed;
                }

                eulerAngles_y += YChange;
                if (YChange > 0)
                {
                    YChange -= Time.deltaTime * rotatespeed;
                }
                else if (YChange < 0)
                {
                    YChange += Time.deltaTime * rotatespeed;
                }

                log.logHelper.DebugLog("X: " + XChange + "   Y: " + YChange);
            }
        }        
        
        //屏幕缩放
        /*if (Input.touchCount > 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began)
            {
                StartDis = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                Vector2 tempPosition1 = Input.GetTouch(0).position;
                Vector2 tempPosition2 = Input.GetTouch(1).position;

                float dis = Vector2.Distance(tempPosition1, tempPosition2);
                if (dis < StartDis)
                {
                    if (distance < 13)
                    {
                        distance += Time.deltaTime * 7;
                    }
                }
                else if (dis > StartDis)
                {
                    if (distance > 3)
                    {
                        distance -= Time.deltaTime * 7;
                    }
                }
            }
        }*/
    }

    void LateUpdate()
    {
        if (CreatePlayer._Instace.GetIsJustCreate())
        {
            ResetDefaultValue();
            CreatePlayer._Instace.SetIsJustCreate(false);
        }

        if (CreatePlayer._Instace.player.GetComponent<Transform>() && CreatePlayer._Instace.player.GetComponent<Transform>().GetComponent<Player>().controlType == ControlType.GravitytouchControl)
        {
            if (eulerAngles_y < yMinLimit)
            {
                eulerAngles_y = yMinLimit;
            }
            if (eulerAngles_y > yMaxLimit)
            {
                eulerAngles_y = yMaxLimit;
            }
            eulerAngles_y = ClampAngle(eulerAngles_y, yMinLimit, yMaxLimit);

            //中心位置
            Vector3 centerPos = CreatePlayer._Instace.GetCenterPosition();
            //缩放参照距离
            float radius = CreatePlayer._Instace.GetScaleDistance(centerPos);

            //等比缩放相机位置
            float cur_distance_Z = this.distance_Z * radius;
            if (radius <= limit_radius_value)
                cur_distance_Z -= Mathf.Pow(radius, MoveSensitivity);
            else if (radius > limit_radius_value && radius <= limit_radius_value2)
                cur_distance_Z -= Mathf.Pow(radius, MoveSensitivity2);
            else
                cur_distance_Z -= (radius / limit_radius_value) * critical_value;

            float cur_distance_Y = this.distance_Y * radius;

            transform.rotation = Quaternion.Euler(eulerAngles_y, eulerAngles_x, 0);
            transform.position = transform.rotation * new Vector3(0.0f, cur_distance_Y, -cur_distance_Z) + centerPos;
            
            //旋转玩家角度，x轴不变 
            //player.GetComponent<Transform>().rotation = (this.transform.rotation);
            Vector3 eulerAngles_cam = this.transform.rotation.eulerAngles;
            Vector3 eulerAngles = new Vector3(0, eulerAngles_cam.y, eulerAngles_cam.z);
            CreatePlayer._Instace.player.GetComponent<Transform>().eulerAngles = eulerAngles;
            CreatePlayer._Instace.RefreshChildrensRotation(eulerAngles);
        }
    }

    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
    void ResetDefaultValue()
    {
        distance_Z = old_distance_Z;
        distance_Y = old_distance_Y;
    }

    void Chose()
    {
        if (Input.touchCount == 1)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
               
            }
        }
    }
}

using UnityEngine;
using System.Collections;

//操作方式
public enum ControlType
{
    KeyBoardControl,
    GravitytouchControl,
}

public class Player : MonoBehaviour
{
    public ControlType controlType = ControlType.GravitytouchControl;
    private float horizontal_move = 0.0f;
    private float vertical_move = 0.0f;    

    private GameObject player1;//该子物体用于添加表现雪球旋转
    private GameObject cmr;

    //V = k * m + b //速度与质量关系式
    public float MoveSpeed_k = 10.0f;//k = 10 / r
    public float MoveSpeed_b = 2.0f;
    private float MoveSpeed = 10.0f;

    public float forceSensitivity = 1.0f;//加速倾斜效果，使得倾斜分量迅速增加或减少

    // Use this for initialization
    void Start()
    {
        double _speed = MoveSpeed_k / Mathf.Sqrt(this.GetComponent<Transform>().localScale.x) + MoveSpeed_b;
        MoveSpeed = (float)System.Math.Round(_speed, 3);

        player1 = transform.FindChild("Player1").gameObject;
        cmr = GameObject.FindGameObjectWithTag("MainCamera").gameObject;

        this.GetComponent<Food>().entity.m_isOnGround = true;
        this.GetComponent<Food>().entity.m_isRobot = false;
        this.GetComponent<Food>().entity.m_canEatRate = this.GetComponent<Food>().canEatRate;      
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        //Edit -> Project Setting -> Input 里面定义以下数值
        if (!this.GetComponent<Food>().IsOnGround())
        {
            //log.logHelper.DebugLog("玩家不在地上,不加力");
            return;
        }

        if (CreatePlayer._Instace.GetIsDontMove()) return;

        //电脑方向键控制
        if (controlType == ControlType.KeyBoardControl)
        {
            horizontal_move = Input.GetAxis("Horizontal");
            vertical_move = Input.GetAxis("Vertical");
            //按住前向按钮
            if(CreatePlayer._Instace.GetIsPressForwardForceBtn())
            {
                vertical_move = -CreatePlayer._Instace.GetForwardForce();
            }

            //if (horizontal_move != 0 || vertical_move != 0)
            //    log.logHelper.DebugLog(this.name + "   Mass: " + this.GetComponent<Rigidbody>().mass + "   半径： " + this.GetComponent<Transform>().localScale.x + "   速度: " + this.GetComponent<Rigidbody>().velocity.magnitude + "   施加力: " + MoveSpeed);

            this.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(horizontal_move, 0, vertical_move) * MoveSpeed, ForceMode.Impulse);
            
            if(vertical_move == 0 && horizontal_move == 0)
            {

            }else
            {
                //Vector3 startRotation = player1.transform.rotation.eulerAngles;
                //Vector3 endRotation = new Vector3(0, (Mathf.Atan2(-vertical_move, horizontal_move) * Mathf.Rad2Deg) + cmr.transform.rotation.eulerAngles.y + 90, 0);
                //player1.transform.rotation = Quaternion.FromToRotation(startRotation, endRotation);
                player1.transform.rotation = Quaternion.Euler(0, (Mathf.Atan2(-vertical_move, horizontal_move) * Mathf.Rad2Deg) + cmr.transform.rotation.eulerAngles.y + 90, 0);
            }
        }

        //手机重力控制
        if (controlType == ControlType.GravitytouchControl)
        {
            horizontal_move = Input.acceleration.y;
            vertical_move = Input.acceleration.x;
            horizontal_move *= forceSensitivity;
            vertical_move *= forceSensitivity;
            if (horizontal_move < -1) horizontal_move = -1;
            if (horizontal_move > 1) horizontal_move = 1;
            if (vertical_move < -1) vertical_move = -1;
            if (vertical_move > 1) vertical_move = 1;

            //按住前向按钮，施加一个向前最大的力
            if (CreatePlayer._Instace.GetIsPressForwardForceBtn())
            {
                horizontal_move = CreatePlayer._Instace.GetForwardForce();
            }

            log.logHelper.DebugLog(CreatePlayer._Instace.GetForwardForce().ToString());
            Vector3 force = new Vector3(vertical_move, 0, horizontal_move);
            var transform = this.GetComponent<Transform>();
           
            //添加力
            this.GetComponent<Rigidbody>().AddRelativeForce(force * MoveSpeed, ForceMode.Impulse);
            if (vertical_move == 0 && horizontal_move == 0)
            {

            }
            else
            {
                player1.transform.rotation = Quaternion.Euler(0, (Mathf.Atan2(-horizontal_move, vertical_move) * Mathf.Rad2Deg) + cmr.transform.rotation.eulerAngles.y + 90, 0);
            }
        }
        
        double _speed = MoveSpeed_k / Mathf.Sqrt(this.GetComponent<Transform>().localScale.x) + MoveSpeed_b;
        MoveSpeed = (float)System.Math.Round(_speed, 3);
    }
}

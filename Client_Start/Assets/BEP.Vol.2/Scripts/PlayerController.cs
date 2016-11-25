using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    //用于解决雪球自身旋转反向问题，但是导致其他问题，暂时放置不用

    public GameObject cam;
    private Vector3 camForward;    
    private Vector3 move;
    private Rigidbody m_Rigidbody;
    private float m_MaxAngularVelocity = 25;

    //V = k * m + b //速度与质量关系式
    public float MoveSpeed_k = 10.0f;//k = 10 / r
    public float MoveSpeed_b = 2.0f;
    private float MoveSpeed = 10.0f;    

    // Use this for initialization
    void Start()
    {
        MoveSpeed = MoveSpeed_k / this.GetComponent<Transform>().localScale.x + MoveSpeed_b;

        m_Rigidbody = GetComponent<Rigidbody>();
        // Set the maximum angular velocity.
        GetComponent<Rigidbody>().maxAngularVelocity = m_MaxAngularVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.rotation = new Quaternion(this.transform.rotation.x, cmr.transform.rotation.y, transform.rotation.z, transform.rotation.z);
        //this.transform.rotation = cmr.transform.rotation;
        float horizontal_move = Input.GetAxis("Horizontal");
        float vertical_move = Input.GetAxis("Vertical");
        
        // calculate move direction
        if (cam != null)
        {
            camForward = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1)).normalized;
            move = (vertical_move * camForward + horizontal_move * cam.transform.right).normalized;
        }
        else
        {
            move = (vertical_move * Vector3.forward + horizontal_move * Vector3.right).normalized;
        }

        //改变重量 
        this.GetComponent<Rigidbody>().mass = this.GetComponent<Transform>().localScale.x;
        //添加力 
        this.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(horizontal_move, 0, vertical_move) * MoveSpeed);
    }

    private void FixedUpdate()
    {         
        MoveSpeed = MoveSpeed_k / this.GetComponent<Transform>().localScale.x + MoveSpeed_b;
        Move(move);
    }
    
    public void Move(Vector3 moveDirection)
    {
        // Otherwise add force in the move direction.
        //m_Rigidbody.AddForce(moveDirection * 2);
        m_Rigidbody.AddTorque(new Vector3(moveDirection.z, 0, -moveDirection.x) * 2);        
    }
}

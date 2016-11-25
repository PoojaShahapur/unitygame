using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Assets.BEP.Vol._2.Scripts;

public class Food : MonoBehaviour
{
    public SceneEntity entity = new SceneEntity();        
    public float canEatRate = 0.85f;//半径比率小于canEatRate的雪球可吞食    
    public float autoriseRate = 0.001f;//雪球滚动增长率，速度小于max_velocity时线性关系增长，超过后按照autoriseRate增长
    public float max_velocity = 200.0f;

    public bool IsOnGround()
    {
        return entity.m_isOnGround;        
    }

    public bool amIRobot()
    {
        return entity.m_isRobot;
    }

    public void SetIsRobot(bool isrobot)
    {
        entity.m_isRobot = isrobot;
    }

    public void setMyNumber(uint number)
    {
        entity.m_charid = number;
    }

    public void setMyName(string name)
    {
        entity.m_name = name;
    }

    public void setEntity(GameObject obj)
    {
        entity.m_object = obj;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {            
            entity.m_isOnGround = true;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            entity.m_isOnGround = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
		bool isGround = collision.collider.CompareTag ("Ground");
		//log.logHelper.DebugLog (entity.m_name +  "和" + collision.gameObject.name + "退出碰撞" + ",isground=" + isGround.ToString());
        if (collision.collider.CompareTag("Ground"))
        {			
            entity.m_isOnGround = false;
        }
    }

    void Start()
    {
        SceneEntity.sCanEatRate = canEatRate;
        SceneEntity.sAutoRiseRate = autoriseRate;
        SceneEntity.sMaxVelocity = max_velocity;
        entity.m_object = this.gameObject;
        //log.logHelper.DebugLog(entity.m_name + " Start();");
        entity.Start();
    }

    void FixedUpdate()
    {
        entity.onLoop();
    }

    // 将该 GameObject 从排行榜中移除
    void OnDestroy()
    {
        entity.OnDestroy();
        if (CreateRobot.Instance != null && amIRobot())
        {
            CreateRobot.Instance.subFoodsNum(entity.m_name, entity.m_charid);
        }
    }
}

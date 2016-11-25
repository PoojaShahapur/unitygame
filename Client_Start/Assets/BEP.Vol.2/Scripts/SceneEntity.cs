using UnityEngine;
using System.Collections;
using System;

namespace Assets.BEP.Vol._2.Scripts
{
    // 场景物件基类
    [System.Serializable]
    public class SceneEntity
    {
        public bool m_isRobot;//是否为机器人
        [NonSerialized]
        public float m_canEatRate;//可以吃的比率
        public uint m_charid = 0;//物件ID，为0的是玩家        
        public string m_name;//物件名
        public float m_radius;//半径
        public uint m_swallownum = 0;//吞食数量
        [NonSerialized]
        public bool m_isOnGround;//=true代表在地上

        [NonSerialized]
        public UnityEngine.GameObject m_object;
        [NonSerialized]
        public static float sAutoRiseRate;//最大滚动增长率,必须滚动才增长,静止不增长
                                          //雪球滚动增长率，速度小于sMaxVelocity时线性关系增长，超过后按照sAutoRiseRate增长
        [NonSerialized]
        public static float sMaxVelocity;
        [NonSerialized]
        public static float sCanEatRate;//可以吞食的比例     

        [NonSerialized]
        private Vector3 curPos;//这2个位置用于判断球是否在移动,判断是判断x1==x2,z1==z2
        [NonSerialized]
        private Vector3 lastPos;

        public SceneEntity()
        {
        }

        // 将物件从排序管理器中移除
        public void OnDestroy()
        {
            GameObjectManager.getInstance().removeEntityByCharID(m_charid);
        }

        // 将物件添加到排序管理器
        public void Start()
        {
            curPos = m_object.GetComponent<Transform>().position;
            lastPos = curPos;
            GameObjectManager.getInstance().setEntityByRadius(m_object.GetComponent<UnityEngine.Transform>().localScale.x, this);
        }

        // 目前主要是增长重量的逻辑
        public void onLoop()
        {
            m_radius = m_object.GetComponent<Transform>().localScale.x;

            float x_velocity = m_object.GetComponent<Rigidbody>().velocity.x;
            float z_velocity = m_object.GetComponent<Rigidbody>().velocity.z;

            //if (!m_isRobot && (x_velocity != 0 || z_velocity != 0))
            //    log.logHelper.DebugLog(m_object.name + "  玩家xz平面速度     " + x_velocity + "    : " + z_velocity);

            //未知原因，刚体突然会有一个速度，但看不到明显移动，加个速度判断
            if (Mathf.Abs(x_velocity) > 0.3f || Mathf.Abs(z_velocity) > 0.3f)
            {
                //自动增长速率 v = k * x
                float velocity_value_xz = Mathf.Sqrt(x_velocity * x_velocity + z_velocity * z_velocity);
                float x = velocity_value_xz / sMaxVelocity;
                if (x > 1) x = 1;
                float cur_rise_rate = sAutoRiseRate * x;
                //雪球自动增长
                m_object.GetComponent<Transform>().localScale += new Vector3(cur_rise_rate, cur_rise_rate, cur_rise_rate);
                GameObjectManager.getInstance().setEntityByRadius(m_object.GetComponent<Transform>().localScale.x, this);
            }
            else
            {
                //if (!m_isRobot) log.logHelper.DebugLog("玩家静止了     " + m_radius + "   x: " + m_object.GetComponent<Rigidbody>().velocity.x + "   z: " + m_object.GetComponent<Rigidbody>().velocity.z);
            }
        }
    }
}


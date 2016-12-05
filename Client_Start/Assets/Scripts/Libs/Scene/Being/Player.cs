using UnityEngine;

namespace SDK.Lib
{
	/**
	 * @brief 场景中的玩家
	 */
	public class Player : BeingEntity
	{
        protected MList<PlayerChild> mChildrenList;    // 保存分裂的 Child

        public Player()
		{
            //m_skinAniModel.m_modelList = new SkinSubModel[(int)ePlayerModelType.eModelTotal];
            //int idx = 0;
            //while (idx < (int)ePlayerModelType.eModelTotal)
            //{
            //    m_skinAniModel.m_modelList[idx] = new SkinSubModel();
            //    ++idx;
            //}
            mChildrenList = new MList<PlayerChild>();
        }

        override public void init()
        {
            base.init();

            this.Start_Player();
        }

        // 构造完成 Player 后，在初始化 PlayerRender
        public override void onInit()
        {
            base.onInit();
        }

        override public void dispose()
        {
            base.dispose();
            //Ctx.mInstance.mPlayerMgr.removeEntity(this);
        }

        override public void autoHandle()
        {
            base.autoHandle();

            //Ctx.mInstance.mPlayerMgr.addEntity(this);
        }

        override public void initRender()
        {
            mRender = new PlayerRender(this);
            mRender.init();
        }

        public override void onPreTick(float delta)
        {
            base.onPreTick(delta);

            this.onLoop();
        }

        override public bool canEatOther(BeingEntity other)
        {
            bool ret = false;
            // 雪快必然可以被吃掉
            if(other.getEntityType() == EntityType.eSnowBlock)
            {
                ret = true;
            }
            else
            {
                ret = base.canEatOther(other);
            }

            return ret;
        }

        //--------------------------------------
        public bool m_isRobot;//是否为机器人

        public uint m_charid = 0;//物件ID，为0的是玩家        
        public string m_name;//物件名
        public float m_radius;//半径
        public uint m_swallownum = 0;//吞食数量
        public bool m_isOnGround;//=true代表在地上

        //public UnityEngine.GameObject m_object;
        public static float sAutoRiseRate = 0.001f;//最大滚动增长率,必须滚动才增长,静止不增长
                                          //雪球滚动增长率，速度小于sMaxVelocity时线性关系增长，超过后按照sAutoRiseRate增长
        public static float sMaxVelocity = 200.0f;
        public static float sCanEatRate = 0.85f;//可以吞食的比例     

        private Vector3 curPos;//这2个位置用于判断球是否在移动,判断是判断x1==x2,z1==z2
        private Vector3 lastPos;

        // 将物件从排序管理器中移除
        public void OnDestroy()
        {
            //GameObjectManager.getInstance().removeEntityByCharID(m_charid);
        }

        // 将物件添加到排序管理器
        public void Start_Player()
        {
            //curPos = m_object.GetComponent<Transform>().position;
            curPos = this.transform().position;
            lastPos = curPos;
            //GameObjectManager.getInstance().setEntityByRadius(m_object.GetComponent<UnityEngine.Transform>().localScale.x, this);
        }

        // 目前主要是增长重量的逻辑
        public void onLoop()
        {
            //m_radius = m_object.GetComponent<Transform>().localScale.x;
            m_radius = this.transform().localScale.x;

            //float x_velocity = m_object.GetComponent<Rigidbody>().velocity.x;
            //float x_velocity = this.getRigidbody().velocity.x;
            float x_velocity = 1;
            //float z_velocity = m_object.GetComponent<Rigidbody>().velocity.z;
            //float z_velocity = this.getRigidbody().velocity.z;
            float z_velocity = 1;

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
                this.transform().localScale += new Vector3(cur_rise_rate, cur_rise_rate, cur_rise_rate);
                //GameObjectManager.getInstance().setEntityByRadius(m_object.GetComponent<Transform>().localScale.x, this);
            }
            else
            {
                //if (!m_isRobot) log.logHelper.DebugLog("玩家静止了     " + m_radius + "   x: " + m_object.GetComponent<Rigidbody>().velocity.x + "   z: " + m_object.GetComponent<Rigidbody>().velocity.z);
            }
        }

        public bool IsOnGround()
        {
            return this.m_isOnGround;
        }

        public bool amIRobot()
        {
            return this.m_isRobot;
        }

        public void SetIsRobot(bool isrobot)
        {
            this.m_isRobot = isrobot;
        }

        public void setMyNumber(uint number)
        {
            this.m_charid = number;
        }

        public void setMyName(string name)
        {
            this.m_name = name;
        }
    }
}
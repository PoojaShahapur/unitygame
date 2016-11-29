namespace SDK.Lib
{
	/**
	 * @brief 玩家管理器
	 */
    public class PlayerMgr : EntityMgrBase
	{
        protected PlayerMain m_hero;

        public PlayerMgr()
		{

		}

        override protected void onTickExec(float delta)
        {
            base.onTickExec(delta);
        }

        public PlayerMain createHero()
        {
            return new PlayerMain();
        }

        public void addHero(PlayerMain hero)
        {
            m_hero = hero as PlayerMain;
            addEntity(m_hero);
        }

        public PlayerMain getHero()
        {
            return m_hero;
        }

        //--------------------------------
        //public GameObject PlayrPrefab;
        //public float xlimit_min = 100;
        //public float xlimit_max = 900;
        //public float zlimit_min = 100;
        //public float zlimit_max = 900;
        //public float y_height = 1.0f;

        //public string playerName = "雪球";

        //public static CreatePlayer _Instace;
        //public GameObject player;
        //public List<ChildrenItemInfo> childrenList = new List<ChildrenItemInfo>();

        //private uint create_times = 0;//生成次数
        //private bool is_niubi = true;//x秒无敌真男人时间
        //private bool is_dontmove = false;//重生禁止移动状态
        //private bool is_justcreate = false;//是否新生成

        //public int auto_relive_seconds = 5;//总无敌时间
        //private int cur_auto_relive_seconds = 5;//当前剩余无敌时间

        //private bool is_press_forward_force_btn = false;//是否长按了前进的按钮
        //private float forward_force = 0;//向前力的大小

        //public uint GetTimes()
        //{
        //    return create_times;
        //}

        //public bool IsRelive()
        //{
        //    return create_times > 1;
        //}

        //public bool GetIsNiuBi()
        //{
        //    return is_niubi;
        //}

        //public void SetIsNiuBi(bool _niubi)
        //{
        //    is_niubi = _niubi;
        //}

        //public bool GetIsDontMove()
        //{
        //    return is_dontmove;
        //}

        //public void SetIsDontMove(bool _dontmove)
        //{
        //    is_dontmove = _dontmove;
        //}

        //public bool GetIsJustCreate()
        //{
        //    return is_justcreate;
        //}

        //public void SetIsJustCreate(bool _justcreate)
        //{
        //    is_justcreate = _justcreate;
        //}

        //public bool GetIsPressForwardForceBtn()
        //{
        //    return is_press_forward_force_btn;
        //}

        //public void SetIsPressForwardForceBtn(bool _press)
        //{
        //    is_press_forward_force_btn = _press;
        //}

        //public float GetForwardForce()
        //{
        //    return forward_force;
        //}

        //public void SetForwardForce(float _forward_force)
        //{
        //    forward_force = _forward_force;
        //}

        public void init()
        {
            OnCreatePlayer();
        }

        // Update is called once per frame
        //void Update()
        //{
        //    ShowReliveTime();
        //    RefreshChildrenPosition();
        //}

        //void RefreshChildrenPosition()
        //{
        //    foreach (var child in childrenList)
        //    {
        //        float player_radius = player.gameObject.GetComponent<MeshFilter>().mesh.bounds.size.x * player.GetComponent<Transform>().localScale.x;
        //        float child_radius = child.childrenObj.GetComponent<MeshFilter>().mesh.bounds.size.x * child.childrenObj.GetComponent<Transform>().localScale.x;
        //        float x = player.GetComponent<Transform>().position.x + player_radius + child_radius + child.startX;
        //        float z = player.GetComponent<Transform>().position.z + player_radius + child_radius + child.startZ;

        //        float y = child.childrenObj.GetComponent<Transform>().position.y;
        //        child.childrenObj.GetComponent<Transform>().position = new Vector3(x, y, z);
        //    }
        //}

        //public void RefreshChildrensRotation(Vector3 eulerangles)
        //{
        //    foreach (var child in childrenList)
        //    {
        //        child.childrenObj.GetComponent<Transform>().eulerAngles = eulerangles;
        //    }
        //}

        //private float totalTime = 0;
        //private void ShowReliveTime()//复活倒计时
        //{
        //    if (!is_niubi) return;
        //    //累加每帧消耗时间
        //    //totalTime += Time.deltaTime;
        //    totalTime += Ctx.mInstance.mSystemTimeData.deltaSec;
        //    if (totalTime >= 1)//每过1秒执行一次
        //    {
        //        cur_auto_relive_seconds--;
        //        totalTime = 0;
        //    }

        //    //真男人时间结束
        //    if (0 == cur_auto_relive_seconds)
        //    {
        //        SetIsNiuBi(false);
        //    }
        //}

        ////还剩余liftseconds可以牛逼一下
        //public void SetLeftSeconds(int liftseconds)
        //{
        //    cur_auto_relive_seconds = liftseconds;
        //}

        public void OnCreatePlayer()
        {
            //float x = Random.Range(xlimit_min, xlimit_max);
            float x = UtilApi.rangRandom(PlayerMain.xlimit_min, PlayerMain.xlimit_max);
            //float z = Random.Range(zlimit_min, zlimit_max);
            float z = UtilApi.rangRandom(PlayerMain.zlimit_min, PlayerMain.zlimit_max);

            //player = Instantiate(PlayrPrefab, new Vector3(x, y_height, z), Quaternion.identity) as GameObject;
            m_hero = new PlayerMain();
            m_hero.init();
            m_hero.setOriginal(new UnityEngine.Vector3(x, PlayerMain.y_height, z));
            m_hero.setRotation(UnityEngine.Quaternion.identity);

            //if (player != null)
            if (m_hero != null)
            {
                m_hero.SetIsJustCreate(true);
                ++m_hero.create_times;
                m_hero.cur_auto_relive_seconds = m_hero.auto_relive_seconds;

                string tempName = "";
                //tempName = PlayerPrefs.GetString("myname");
                tempName = Ctx.mInstance.mSystemSetting.getString("myname");
                if (tempName == "")
                {
                    tempName = m_hero.playerName;
                }

                //player.GetComponent<Food>().SetIsRobot(false);
                m_hero.SetIsRobot(false);
                //player.name = tempName;
                m_hero.setSelfName(tempName);
                //player.GetComponent<Food>().entity.m_charid = 0;//自己的charid为0
                m_hero.m_charid = 0;//自己的charid为0
                //player.GetComponent<Food>().entity.m_canEatRate = player.GetComponent<Food>().canEatRate;
                //player.GetComponent<Food>().setMyName(player.name);
                m_hero.setMyName(tempName);
                //player.GetComponent<Food>().setEntity(player);
                //player.GetComponent<Food>().setEntity(player);

                //GameObjectManager.getInstance().setEntityByRadius(player.GetComponent<Transform>().localScale.x, player.GetComponent<Food>().entity);
            }
        }

        //public Vector3 GetCenterPosition()
        //{
        //    Vector3 center = new Vector3(0.0f, 0.0f, 0.0f);
        //    float x_min = float.MaxValue, x_max = float.MinValue, z_min = float.MaxValue, z_max = float.MinValue;
        //    foreach (var obj in childrenList)
        //    {
        //        if (obj.childrenObj.GetComponent<Transform>().position.x < x_min) x_min = obj.childrenObj.GetComponent<Transform>().position.x;
        //        if (obj.childrenObj.GetComponent<Transform>().position.x > x_max) x_max = obj.childrenObj.GetComponent<Transform>().position.x;
        //        if (obj.childrenObj.GetComponent<Transform>().position.z < z_min) z_min = obj.childrenObj.GetComponent<Transform>().position.z;
        //        if (obj.childrenObj.GetComponent<Transform>().position.z > z_max) z_max = obj.childrenObj.GetComponent<Transform>().position.z;
        //    }
        //    if (player.GetComponent<Transform>().position.x < x_min) x_min = player.GetComponent<Transform>().position.x;
        //    if (player.GetComponent<Transform>().position.x > x_max) x_max = player.GetComponent<Transform>().position.x;
        //    if (player.GetComponent<Transform>().position.z < z_min) z_min = player.GetComponent<Transform>().position.z;
        //    if (player.GetComponent<Transform>().position.z > z_max) z_max = player.GetComponent<Transform>().position.z;

        //    center.x = (x_max + x_min) / 2;
        //    center.z = (z_max + z_min) / 2;

        //    return center;
        //}

        //public float GetScaleDistance(Vector3 center)
        //{
        //    float distance = 0.0f;
        //    float x_2 = MathToolManager.getSquare(player.GetComponent<Transform>().position.x - center.x);
        //    float z_2 = MathToolManager.getSquare(player.GetComponent<Transform>().position.z - center.z);
        //    float curRadius = player.GetComponent<Transform>().localScale.x;
        //    float radius = player.gameObject.GetComponent<MeshFilter>().mesh.bounds.size.x * curRadius;
        //    distance = Mathf.Sqrt(x_2 + z_2) + radius;

        //    foreach (var obj in childrenList)
        //    {
        //        x_2 = MathToolManager.getSquare(obj.childrenObj.GetComponent<Transform>().position.x - center.x);
        //        z_2 = MathToolManager.getSquare(obj.childrenObj.GetComponent<Transform>().position.z - center.z);
        //        curRadius = obj.childrenObj.GetComponent<Transform>().localScale.x;
        //        radius = obj.childrenObj.gameObject.GetComponent<MeshFilter>().mesh.bounds.size.x * curRadius;
        //        float _distance = Mathf.Sqrt(x_2 + z_2) + radius;
        //        if (_distance > distance)
        //            distance = _distance;
        //    }

        //    return distance;
        //}
    }
}
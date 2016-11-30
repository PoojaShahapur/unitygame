using UnityEngine;

namespace SDK.Lib
{
    public class RobotMgr : EntityMgrBase
    {
        public RobotMgr()
        {

        }

        override protected void onTickExec(float delta)
        {
            base.onTickExec(delta);
        }

        //public GameObject SFood;
        public float xlimit_min = 120;
        public float xlimit_max = 190;
        public float zlimit_min = 230;
        public float zlimit_max = 250;
        public float y_height = 1.65f;

        public int small_Max_Num = 2;
        public float small_Min_Radius = 0.5f;
        public float small_Max_Radius = 1.5f;

        public int big_Max_Num = 2;
        public float big_Min_Radius = 3.0f;
        public float big_Max_Radius = 5.0f;

        private uint foodsNum = 0;

        public float startTime = 1.0f;//开始startTime时长后开始产生
        public float createTime = 6.0f;//雪球产生间隔

        public static CreateRobot Instance;
        private bool createFinish = false;
        public MList<CreateParam> toBeCreateNames = new MList<CreateParam>();
        //public GameObject player;//玩家
        private string randomName = "哈帅酷妹美吊炸天炫霸动萌可爱傻笨猪";
        private string[] allNameArray;//所有可用的名字列表
        protected TimerItemBase mStartTimer;
        protected TimerItemBase mRepeatTimer;

        //void Awake()
        //{
        //    Instance = this;
        //}

        // Use this for initialization
        override public void init()
        {
            // 冒泡得到所有2个字的名字 11 * 10 = 110
            //InvokeRepeating("CreateSnowFood", startTime, createTime);
            // 启动定时器
            mStartTimer = new TimerItemBase();
            mStartTimer.mInternal = startTime;
            mStartTimer.mTotalTime = startTime;
            mStartTimer.mTimerDisp.setFuncObject(onStartTimerEnd);
            Ctx.mInstance.mTimerMgr.addTimer(mStartTimer);

            MList<string> allAvaliableNames = new MList<string>();
            getAllPermutation(randomName, ref allAvaliableNames);
            allNameArray = allAvaliableNames.ToArray();
            new System.Random().Shuffle<string>(allNameArray);
        }

        protected void onStartTimerEnd(TimerItemBase timer)
        {
            mRepeatTimer = new TimerItemBase();
            mRepeatTimer.mInternal = startTime;
            mRepeatTimer.mTotalTime = startTime;
            mRepeatTimer.mTimerDisp.setFuncObject(onRepeatTimerTick);
            Ctx.mInstance.mTimerMgr.addTimer(mRepeatTimer);
        }

        protected void onRepeatTimerTick(TimerItemBase timer)
        {
            this.CreateSnowFood();
        }

        // 并非完全的排列组合,就冒泡吧, N*(N-1)种
        void getAllPermutation(string name, ref MList<string> nameList)
        {
            for (int i = 0; i < name.Length; ++i)
            {
                for (int j = i; j < name.Length; ++j)
                {
                    nameList.Add(name[i].ToString() + name[j].ToString());
                }
            }

            for (int i = 0; i < name.Length; ++i)
            {
                for (int j = i; j < name.Length && (j + 1) < name.Length; ++j)
                {
                    nameList.Add(name[i].ToString() + name[j].ToString() + name[j + 1].ToString());
                }
            }
        }

        public void CreateSnowFood()
        {
            if (!createFinish && foodsNum < small_Max_Num + big_Max_Num)
            {
                float scaleRate = 1.0f;
                if (foodsNum < small_Max_Num)
                {
                    scaleRate = UtilApi.rangRandom(small_Min_Radius, small_Max_Radius);
                }
                else//小球已用完
                {
                    scaleRate = UtilApi.rangRandom(big_Min_Radius, big_Max_Radius);
                }

                float x = UtilApi.rangRandom(xlimit_min, xlimit_max);
                float z = UtilApi.rangRandom(zlimit_min, zlimit_max);
                Robot robot = new Robot();
                robot.init();
                robot.setOriginal(new UnityEngine.Vector3(x, y_height * scaleRate, z));
                robot.setRotation(Quaternion.identity);

                robot.transform().localScale = new UnityEngine.Vector3(scaleRate, scaleRate, scaleRate);//缩放
                robot.SetIsRobot(true);
                //robot.GetComponent<AI>().SetPlayer(CreatePlayer._Instace.player);
                robot.setSelfName(allNameArray[(int)foodsNum]);
                //log.logHelper.DebugLog("CreateSnowFood" + food.name);
                robot.m_charid = foodsNum + 1;
                robot.setMyName(allNameArray[(int)foodsNum]);
                //Debug.Log(food.name + "y位置：" + food.transform.position.y + "   半径：" + scaleRate);

                ++foodsNum;
            }
            else
            {
                createFinish = true;
                //GameObjectManager.getInstance().CreateSnowBall();
                this.CreateSnowBall();
            }
        }

        public void CreateSnowFoodWithNameCharID(string name, uint charid)
        {
            if (foodsNum < small_Max_Num + big_Max_Num)
            {
                float scaleRate = 1.0f;
                if (foodsNum < small_Max_Num)
                {
                    scaleRate = UtilApi.rangRandom(small_Min_Radius, small_Max_Radius);
                }
                else//小球已用完
                {
                    scaleRate = UtilApi.rangRandom(big_Min_Radius, big_Max_Radius);
                }

                float x = UtilApi.rangRandom(xlimit_min, xlimit_max);
                float z = UtilApi.rangRandom(zlimit_min, zlimit_max);

                Robot robot = new Robot();
                robot.init();
                robot.setOriginal(new Vector3(x, y_height * scaleRate, z));
                robot.setRotation(Quaternion.identity);

                robot.transform().localScale = new Vector3(scaleRate, scaleRate, scaleRate);//缩放
                robot.SetIsRobot(true);
                //robot.GetComponent<AI>().SetPlayer(CreatePlayer._Instace.player);
                robot.setSelfName(name);
                //log.logHelper.DebugLog("创建物件名" + name);
                robot.m_charid = charid;
                robot.setMyName(name);
                //Debug.Log(food.name + "y位置：" + food.transform.position.y + "   半径：" + scaleRate);

                ++foodsNum;
            }
        }

        public void ResetFoodsNum()
        {
            foodsNum = 0;
        }

        public void subFoodsNum(string name, uint charid)
        {
            if (foodsNum >= 1)
            {
                --foodsNum;
            }
            toBeCreateNames.Add(new CreateParam(name, charid));
        }

        public void CreateSnowBall()
        {
            // 如果有要创建的小球,创建下            
            for (int i = 0; i < this.toBeCreateNames.Count(); ++i)
            {
                this.CreateSnowFoodWithNameCharID(
                    CreateRobot.Instance.toBeCreateNames[i].name
                    , CreateRobot.Instance.toBeCreateNames[i].charid);
            }
            this.toBeCreateNames.Clear();
        }
    }
}
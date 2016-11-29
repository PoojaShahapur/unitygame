using UnityEngine;

namespace SDK.Lib
{
    public class SnowBlockMgr : EntityMgrBase
    {
        public SnowBlockMgr()
        {

        }

        override protected void onTickExec(float delta)
        {
            base.onTickExec(delta);
        }

        //public GameObject SnowBlockPrefab;
        public float xlimit_min = 120;
        public float xlimit_max = 830;
        public float zlimit_min = 130;
        public float zlimit_max = 820;
        public float y_height = 0.9f;

        public uint blockNum = 80;
        public float refreshTime = 3.0f;
        public float blockMass = 100.0f;//雪块质量

        public static CreateSnowBlock Instance;
        protected TimerItemBase mStartTimer;

        //void Awake()
        //{
        //    Instance = this;
        //}

        // Use this for initialization
        void Start()
        {
            //StartCoroutine(CreateSnowFood());
            Ctx.mInstance.mCoroutineMgr.StartCoroutine(CreateSnowFood());
        }

        System.Collections.IEnumerator CreateSnowFood()
        {
            uint _num = 0;
            while (_num < blockNum)
            {
                CreateASnowBlock();
                ++_num;
            }

            yield return 0;
        }

        public void CreateASnowBlock()
        {
            float x = UtilApi.rangRandom(xlimit_min, xlimit_max);
            float z = UtilApi.rangRandom(zlimit_min, zlimit_max);
            //GameObject food = Instantiate(SnowBlockPrefab, new Vector3(x, y_height, z), Quaternion.identity) as GameObject;
            SnowBlock snowBlock = new SnowBlock();
            snowBlock.init();
            snowBlock.setOriginal(new Vector3(x, y_height, z));
            snowBlock.setRotation(Quaternion.identity);
        }

        public void RefreshSnowBlock()
        {
            //Invoke("CreateASnowBlock", refreshTime);
            mStartTimer = new TimerItemBase();
            mStartTimer.mInternal = refreshTime;
            mStartTimer.mTotalTime = refreshTime;
            mStartTimer.mTimerDisp.setFuncObject(onStartTimerEnd);
            Ctx.mInstance.mTimerMgr.addTimer(mStartTimer);
        }

        protected void onStartTimerEnd(TimerItemBase timer)
        {
            this.CreateASnowBlock();
        }
    }
}
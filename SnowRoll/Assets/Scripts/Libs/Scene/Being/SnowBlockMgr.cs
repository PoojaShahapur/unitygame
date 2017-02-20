﻿using UnityEngine;

namespace SDK.Lib
{
    public class SnowBlockMgr : EntityMgrBase
    {
        public SnowBlockMgr()
        {
            mUniqueStrIdGen = new UniqueStrIdGen(UniqueStrIdGen.SnowBlockPrefix, 0);
        }

        override protected void onTickExec(float delta)
        {
            base.onTickExec(delta);
        }

        public float xlimit_min = 120;
        public float xlimit_max = 830;
        public float zlimit_min = 130;
        public float zlimit_max = 820;
        public float y_height = 0.9f;

        public uint blockNum = 80;
        public float refreshTime = 3.0f;
        public float blockMass = 100.0f;//雪块质量

        protected TimerItemBase mStartTimer;

        override public void init()
        {
            //StartCoroutine(CreateSnowFood());
            //Ctx.mInstance.mCoroutineMgr.StartCoroutine(CreateSnowFood());
        }

        public void addSnowBlock(SnowBlock snowBlock)
        {
            this.addEntity(snowBlock);
        }

        public void removeSnowBlock(SnowBlock snowBlock)
        {
            this.removeEntity(snowBlock);
        }

        public void createAllSnowFood()
        {
            Ctx.mInstance.mCoroutineMgr.StartCoroutine(CreateSnowFood());
        }

        System.Collections.IEnumerator CreateSnowFood()
        {
            uint _num = 0;
            while (_num < blockNum)
            {
                CreateASnowBlock();
                yield return 1;
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
            snowBlock.setPos(new Vector3(x, y_height, z));
            snowBlock.setRotation(Quaternion.identity);
        }

        public void RefreshSnowBlock()
        {
            //Invoke("CreateASnowBlock", refreshTime);
            mStartTimer = new TimerItemBase();
            mStartTimer.mInternal = refreshTime;
            mStartTimer.mTotalTime = refreshTime;
            mStartTimer.mTimerDisp.setFuncObject(onStartTimerEnd);
            this.mStartTimer.startTimer();
        }

        protected void onStartTimerEnd(TimerItemBase timer)
        {
            this.CreateASnowBlock();
        }
    }
}
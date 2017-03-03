namespace SDK.Lib
{
    /**
     * * @brief 雪球的一些配置
     */
    public class SnowBallCfg
    {
        public XmlSnowBallCfg mXmlSnowBallCfg;

        public float mCanAttackRate;//可以吃的比率
        public float mMassFactor;       // 客户端显示质量转换因子
        public float mRealMassFactor;       // 游戏中实际质量转换因子

        public float mK;     // 目标点 K 因子
        public float mN;     // 目标点 N 因子
        public float mCanSplitMass;       // 可以分裂的雪块质量
        public int mMaxSnowNum;             // 最大雪块的数量
        public float mSplitRelStartPos;          // 开始相对位置
        public float mSplitRelDist;            // 移动距离

        public float mMergeContactTime;     // 可以合并的接触时间
        public float mMergeCoolTime;        // 小球可以再次融合的时间间隔

        public float mEmitSnowMass;         // 吐积雪块的质量
        public float mCanEmitSnowMass;       // 吐积雪块的最小半径
        public float mEmitRelStartPos;          // 开始相对位置
        public float mEmitRelDist;            // 移动距离
        public float mEmitInterval;         //长按吐雪块时间间隔

        //速度 v = k / m + b
        public float mMoveSpeed_k;
        public float mMoveSpeed_b;
        public float mSlowMoveSpeed;
        public float mScaleSpeed;

        // 吞食雪块计算公式 r = r + (1 + 1 / (10 + Mathf.Pow(r, A)))
        public float mA;

        //相机控制参数 计算公式见配置文件
        public float mCameraDistance_Z; //初始相距相机z值
        public float mCameraChangeFactor_Z;//变化因子
        public float mLimitRadius; //临界半径
        public float mCameraDistance_Y; //初始相距相机y值
        public float mCameraChangeFactor_Y;//变化因子

        //商店
        public XmlItemGoods[] shape; //外形
        public XmlItemGoods[] child; //鱼仔

        public const float msSeparateFactor = 8;
        public float mMergeRange;       // 在这个范围内，说明一直在融合
        public float mMergeSpeedFactor; // 融合的时候，速度变化因子

        public float mBallCollideRadius;    // 初始球的碰撞半径
        public float mSnowBlockCollideRadius;    // 初始雪块的碰撞半径
        public float mShitCollideRadius;    // 初始吐出的雪块的碰撞半径

        public string[] mBallSelfTexArray;      // BallTex 数组
        public string[] mBallOtherTexArray;     // BallTex 数组
        public string[] mComputerBallTexArray;     // ComputerBallTex 数组
        public string[] mSnowBlockTexArray;      // SnowBlockTex 数组
        public MList<TileInfo> mBallTileTexList;
        public MList<TileInfo> mSnowBlockTexList;

        public int mMergeMinSpeed;      // 融合最小

        public float mMinShotSeconds;
        public float mMaxShotSeconds;
        public int mMaxShotNum;
        public float mShotInteval;

        public SnowBallCfg()
        {
            this.mCanAttackRate = 1.0f;
            this.mA = 0.5f;

            this.mK = 10;
            this.mN = 10;
            this.mMaxSnowNum = 200;
            this.mSplitRelStartPos = 5;
            this.mSplitRelDist = 10;

            this.mMergeContactTime = 10;
            this.mMergeCoolTime = 10;

            this.mEmitSnowMass = 1;
            this.mCanEmitSnowMass = 0.1f;
            this.mEmitRelStartPos = 5;
            this.mEmitRelDist = 10;
            this.mEmitInterval = 1.0f;

            this.mMassFactor = 1;
            this.mRealMassFactor = 2.0f;

            this.mMoveSpeed_k = 10.0f;
            this.mMoveSpeed_b = 10.0f;
            this.mSlowMoveSpeed = 5.0f;
            this.mScaleSpeed = 10.0f;

            this.mCameraDistance_Z = 10.0f;
            this.mCameraChangeFactor_Z = 15.0f;
            this.mLimitRadius = 50.0f;
            this.mCameraDistance_Y = 0.5f;
            this.mCameraChangeFactor_Y = 0.5f;

            this.mMergeRange = 1;
            this.mMergeSpeedFactor = 1;

            this.mBallCollideRadius = 1.08f;
            this.mSnowBlockCollideRadius = 0.002f;
            this.mShitCollideRadius = 1;

            this.mMergeMinSpeed = 30;

            this.mMinShotSeconds = 0.5f;
            this.mMaxShotSeconds = 2.5f;
            this.mMaxShotNum = 100;
            this.mShotInteval = 0.02f;
        }

        public void preInit()
        {
            this.init();
        }

        public void init()
        {
            //this.mXmlSnowBallCfg = XmlSnowBallCfg.loadAndRetXml<XmlSnowBallCfg>(XmlCfgID.eXmlSnowBallCfg);

            this.mCanAttackRate = this.mXmlSnowBallCfg.mXmlItemAttack.mFactor;
            this.mA = this.mXmlSnowBallCfg.mXmlItemAttack.mA;
            
            this.mMassFactor = this.mXmlSnowBallCfg.mXmlItemInit.mMassFactor;
            this.mRealMassFactor = this.mXmlSnowBallCfg.mXmlItemInit.mRealMassFactor;
            this.mBallCollideRadius = this.mXmlSnowBallCfg.mXmlItemInit.mBallCollideRadius;
            this.mSnowBlockCollideRadius = this.mXmlSnowBallCfg.mXmlItemInit.mSnowBlockCollideRadius;
            this.mShitCollideRadius = this.mXmlSnowBallCfg.mXmlItemInit.mShitCollideRadius;

            this.mK = this.mXmlSnowBallCfg.mXmlItemSplit.mK;
            this.mN = this.mXmlSnowBallCfg.mXmlItemSplit.mN;
            this.mCanSplitMass = this.mXmlSnowBallCfg.mXmlItemSplit.mCanSplitMass;
            this.mMaxSnowNum = this.mXmlSnowBallCfg.mXmlItemSplit.mMax;
            this.mSplitRelStartPos = this.mXmlSnowBallCfg.mXmlItemSplit.mRelStartPos;
            this.mSplitRelDist = this.mXmlSnowBallCfg.mXmlItemSplit.mRelDist;

            this.mMergeContactTime = this.mXmlSnowBallCfg.mXmlItemMerge.mContactTime;
            this.mMergeCoolTime = this.mXmlSnowBallCfg.mXmlItemMerge.mCoolTime;
            this.mMergeRange = this.mXmlSnowBallCfg.mXmlItemMerge.mRange;
            this.mMergeSpeedFactor = this.mXmlSnowBallCfg.mXmlItemMerge.mSpeedFactor;

            this.mEmitSnowMass = this.mXmlSnowBallCfg.mXmlItemEmit.mSnowMass;
            this.mCanEmitSnowMass = this.mXmlSnowBallCfg.mXmlItemEmit.mCanEmitSnowMass;
            this.mEmitRelStartPos = this.mXmlSnowBallCfg.mXmlItemEmit.mRelStartPos;
            this.mEmitRelDist = this.mXmlSnowBallCfg.mXmlItemEmit.mRelDist;
            this.mEmitInterval = this.mXmlSnowBallCfg.mXmlItemEmit.mInterval;

            this.mMoveSpeed_k = this.mXmlSnowBallCfg.mXmlItemMoveSpeed.mMoveSpeed_k;
            this.mMoveSpeed_b = this.mXmlSnowBallCfg.mXmlItemMoveSpeed.mMoveSpeed_b;
            this.mSlowMoveSpeed = this.mXmlSnowBallCfg.mXmlItemMoveSpeed.mSlowMoveSpeed;
            this.mScaleSpeed = this.mXmlSnowBallCfg.mXmlItemMoveSpeed.mScaleSpeed;

            this.mCameraDistance_Z = this.mXmlSnowBallCfg.mXmlItemCameraControl.mCameraDistance_Z;
            this.mCameraChangeFactor_Z = this.mXmlSnowBallCfg.mXmlItemCameraControl.mCameraChangeFactor_Z;
            this.mLimitRadius = this.mXmlSnowBallCfg.mXmlItemCameraControl.mLimitRadius;
            this.mCameraDistance_Y = this.mXmlSnowBallCfg.mXmlItemCameraControl.mCameraDistance_Y;
            this.mCameraChangeFactor_Y = this.mXmlSnowBallCfg.mXmlItemCameraControl.mCameraChangeFactor_Y;

            this.shape = this.mXmlSnowBallCfg.mXmlShop.shape;
            this.child = this.mXmlSnowBallCfg.mXmlShop.child;

            this.mBallSelfTexArray = UtilStr.split(ref this.mXmlSnowBallCfg.mXmlItemBallSelfTex.mSrc, ',');
            this.mBallOtherTexArray = UtilStr.split(ref this.mXmlSnowBallCfg.mXmlItemBallOtherTex.mSrc, ',');
            this.mComputerBallTexArray = UtilStr.split(ref this.mXmlSnowBallCfg.mXmlItemComputerBallTex.mSrc, ',');
            this.mSnowBlockTexArray = UtilStr.split(ref this.mXmlSnowBallCfg.mXmlItemSnowBlockTex.mSrc, ',');

            this.mMinShotSeconds = this.mXmlSnowBallCfg.mXmlItemShotControl.mMinSeconds;
            this.mMaxShotSeconds = this.mXmlSnowBallCfg.mXmlItemShotControl.mMaxSeconds;
            this.mMaxShotNum = this.mXmlSnowBallCfg.mXmlItemShotControl.mMaxNum;
            this.mShotInteval = (this.mMaxShotSeconds - this.mMinShotSeconds) / mMaxShotNum;

            //int idx = 0;
            //int len = this.mXmlSnowBallCfg.mXmlItemBallTex.mTileInfoList.length();
            //this.mBallTileTexList = new MList<TileInfo>();

            //while (idx < len)
            //{
            //    this.mBallTileTexList.Add(this.mXmlSnowBallCfg.mXmlItemBallTex.mTileInfoList[idx] as TileInfo);

            //    ++idx;
            //}

            //idx = 0;
            //len = this.mXmlSnowBallCfg.mXmlItemSnowBlockTex.mTileInfoList.length();
            //this.mSnowBlockTexList = new MList<TileInfo>();

            //while (idx < len)
            //{
            //    this.mSnowBlockTexList.Add(this.mXmlSnowBallCfg.mXmlItemSnowBlockTex.mTileInfoList[idx] as TileInfo);

            //    ++idx;
            //}
        }

        public void dispose()
        {

        }

        // 是否小于最大雪球的数量
        public bool isLessMaxNum(int num)
        {
            return num < mMaxSnowNum;
        }

        // 是否大于等于
        public bool isGreatEqualMaxNum(int num)
        {
            return num >= mMaxSnowNum;
        }

        public string getRandomBallSelfTex()
        {
            string ret = "";

            if (this.mBallSelfTexArray.Length > 0)
            {
                int min = 0;
                int max = this.mBallSelfTexArray.Length;

                int cur = UtilMath.RangeRandom(min, max);
                ret = this.mBallSelfTexArray[cur];
            }

            return ret;
        }

        public string getRandomBallOtherTex()
        {
            string ret = "";

            if (this.mBallOtherTexArray.Length > 0)
            {
                int min = 0;
                int max = this.mBallOtherTexArray.Length;

                int cur = UtilMath.RangeRandom(min, max);
                ret = this.mBallOtherTexArray[cur];
            }

            return ret;
        }

        public string getRandomComputerBallTex()
        {
            string ret = "";

            if (this.mComputerBallTexArray.Length > 0)
            {
                int min = 0;
                int max = this.mComputerBallTexArray.Length;

                int cur = UtilMath.RangeRandom(min, max);
                ret = this.mComputerBallTexArray[cur];
            }

            return ret;
        }

        public string getRandomSnowBlockTex()
        {
            string ret = "";

            if (this.mSnowBlockTexArray.Length > 0)
            {
                int min = 0;
                int max = this.mSnowBlockTexArray.Length;

                int cur = UtilMath.RangeRandom(min, max);
                ret = this.mSnowBlockTexArray[cur];
            }

            return ret;
        }

        public TileInfo getRandomBallTexTile()
        {
            TileInfo ret = null;

            if (this.mBallTileTexList.Count() > 0)
            {
                int min = 0;
                int max = this.mBallTileTexList.Count() - 1;

                int cur = UtilMath.RangeRandom(min, max);
                ret = this.mBallTileTexList[cur];
            }

            return ret;
        }

        public TileInfo getRandomSnowBlockTexTile()
        {
            TileInfo ret = null;

            if (this.mSnowBlockTexList.Count() > 0)
            {
                int min = 0;
                int max = this.mSnowBlockTexList.Count() - 1;

                int cur = UtilMath.RangeRandom(min, max);
                ret = this.mSnowBlockTexList[cur];
            }

            return ret;
        }

        // 计算吐雪块的开始和结束位置
        public void getShitPos(uint thisId, ref UnityEngine.Vector3 startPos, ref UnityEngine.Vector3 endPos)
        {
            UnityEngine.Vector3 curPos;
            float emitRadius = 1;

            PlayerChild playerChild = null;
            playerChild = Ctx.mInstance.mPlayerMgr.getHeroChildByThisId(thisId);

            if (null == playerChild)
            {
                playerChild = Ctx.mInstance.mPlayerMgr.getChildByThisId(thisId);
            }

            if(null != playerChild)
            {
                emitRadius = playerChild.getEmitSnowWorldSize();

                curPos = playerChild.getPos();

                startPos = playerChild.getPos() + playerChild.getRotate() * new UnityEngine.Vector3(0, 0, playerChild.getBallWorldRadius() + emitRadius + Ctx.mInstance.mSnowBallCfg.mEmitRelStartPos);
                startPos = Ctx.mInstance.mSceneSys.adjustPosInRange(startPos);

                endPos = startPos + playerChild.getRotate() * new UnityEngine.Vector3(0, 0, Ctx.mInstance.mSnowBallCfg.mEmitRelDist);
                endPos = Ctx.mInstance.mSceneSys.adjustPosInRange(endPos);
            }
        }
    }
}
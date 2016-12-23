namespace SDK.Lib
{
    /**
     * * @brief 雪球的一些配置
     */
    public class SnowBallCfg
    {
        public XmlSnowBallCfg mXmlSnowBallCfg;

        public float mCanAttackRate;//可以吃的比率
        public float mInitSnowRadius;   // 初始雪球大小
        public float mMassFactor;       // 质量转换因子

        public float mK;     // 目标点 K 因子
        public float mN;     // 目标点 N 因子
        public float mCanSplitFactor;       // 可以分裂的因子
        public int mMaxSnowNum;             // 最大雪块的数量
        public float mSplitRelStartPos;          // 开始相对位置
        public float mSplitRelDist;            // 移动距离

        public float mMergeContactTime;     // 可以合并的接触时间
        public float mMergeCoolTime;        // 小球可以再次融合的时间间隔

        public float mEmitSnowMass;         // 吐积雪块的质量
        public float mEmitSnowRadius;       // 吐积雪块的最小半径
        public float mEmitRelStartPos;          // 开始相对位置
        public float mEmitRelDist;            // 移动距离

        //速度 v = k / m + b
        public float mMoveSpeed_k;
        public float mMoveSpeed_b;

        public const float msSeparateFactor = 8;

        public SnowBallCfg()
        {
            this.mCanAttackRate = 1.0f;
            this.mInitSnowRadius = 1;

            this.mK = 10;
            this.mN = 10;
            this.mMaxSnowNum = 200;
            this.mSplitRelStartPos = 5;
            this.mSplitRelDist = 10;

            this.mMergeContactTime = 10;
            this.mMergeCoolTime = 10;

            this.mEmitSnowMass = 1;
            this.mEmitSnowRadius = 0.1f;
            this.mEmitRelStartPos = 5;
            this.mEmitRelDist = 10;

            this.mMassFactor = 1;

            this.mMoveSpeed_k = 10.0f;
            this.mMoveSpeed_b = 10.0f;
        }

        public void init()
        {
            this.mXmlSnowBallCfg = XmlSnowBallCfg.loadAndRetXml<XmlSnowBallCfg>(XmlCfgID.eXmlSnowBallCfg);

            this.mCanAttackRate = this.mXmlSnowBallCfg.mXmlItemAttack.mFactor;

            this.mInitSnowRadius = this.mXmlSnowBallCfg.mXmlItemInit.mRadius;
            this.mMassFactor = this.mXmlSnowBallCfg.mXmlItemInit.mMassFactor;

            this.mK = this.mXmlSnowBallCfg.mXmlItemSplit.mK;
            this.mN = this.mXmlSnowBallCfg.mXmlItemSplit.mN;
            this.mCanSplitFactor = this.mXmlSnowBallCfg.mXmlItemSplit.mCanSplitFactor;
            this.mMaxSnowNum = this.mXmlSnowBallCfg.mXmlItemSplit.mMax;
            this.mSplitRelStartPos = this.mXmlSnowBallCfg.mXmlItemSplit.mRelStartPos;
            this.mSplitRelDist = this.mXmlSnowBallCfg.mXmlItemSplit.mRelDist;

            this.mMergeContactTime = this.mXmlSnowBallCfg.mXmlItemMerge.mContactTime;
            this.mMergeCoolTime = this.mXmlSnowBallCfg.mXmlItemMerge.mCoolTime;

            this.mEmitSnowMass = this.mXmlSnowBallCfg.mXmlItemEmit.mSnowMass;
            this.mEmitSnowRadius = this.mXmlSnowBallCfg.mXmlItemEmit.mSnowRadius;
            this.mEmitRelStartPos = this.mXmlSnowBallCfg.mXmlItemEmit.mRelStartPos;
            this.mEmitRelDist = this.mXmlSnowBallCfg.mXmlItemEmit.mRelDist;

            this.mMoveSpeed_k = this.mXmlSnowBallCfg.mXmlItemMoveSpeed.mMoveSpeed_k;
            this.mMoveSpeed_b = this.mXmlSnowBallCfg.mXmlItemMoveSpeed.mMoveSpeed_b;
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
    }
}
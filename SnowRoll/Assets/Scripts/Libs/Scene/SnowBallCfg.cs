namespace SDK.Lib
{
    /**
     * * @brief 雪球的一些配置
     */
    public class SnowBallCfg
    {
        public float mCanEatRate;//可以吃的比率
        public float mK;     // 目标点 K 因子
        public float mN;     // 目标点 N 因子

        public float mMergeContactTime;     // 可以合并的接触时间
        public float mMergeCoolTime;        // 小球可以再次融合的时间间隔

        public float mEmitSnowMinMass;      // 吐积雪块的最小质量
        public float mEmitSnowFactor;       // 吐积雪块的因子
        public float mInitSnowMass;         // 初始雪球大小
        public float mCanEmitMultiple;      // 可以吐雪块的倍数

        public int mMaxSnowNum;            // 最大雪块的数量

        public SnowBallCfg()
        {
            this.mCanEatRate = 1.0f;
            this.mK = 10;
            this.mN = 10;
            this.mMergeContactTime = 10;
            this.mMergeCoolTime = 10;
            this.mEmitSnowMinMass = 1;
            this.mEmitSnowFactor = 0.1f;

            this.mInitSnowMass = 1;
            this.mCanEmitMultiple = 4;
            this.mMaxSnowNum = 200;
        }

        public void init()
        {

        }

        public void dispose()
        {

        }

        // 是否小于最大雪球的数量
        public bool isLessMaxNum(int num)
        {
            return num < mMaxSnowNum;
        }
    }
}
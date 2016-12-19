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

        public SnowBallCfg()
        {
            this.mCanEatRate = 1.0f;
            this.mK = 10;
            this.mN = 10;
            this.mMergeContactTime = 1;
            this.mMergeCoolTime = 1;
        }

        public void init()
        {

        }

        public void dispose()
        {

        }
    }
}
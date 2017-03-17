namespace SDK.Lib
{
    /**
     * @brief 碰撞通道标志
     */
    public enum CollideChannelFlag
    {
        ePlayerMain = (1 << (int)EntityType.ePlayerMain),
        eRobot = (1 << (int)EntityType.eRobot),
        eSnowBlock = (1 << (int)EntityType.eSnowBlock),
    }


    /**
     * @brief 碰撞矩阵
     */
    public class CollideMatrix
    {
        // 目前通道最多只能是 8 个
        protected byte[] mCollideMatrix;

        public CollideMatrix()
        {
            mCollideMatrix = new byte[(int)EntityType.eCount];
        }

        public void init()
        {
            mCollideMatrix[(int)EntityType.ePlayerMain] = (byte)CollideChannelFlag.eRobot |
                                                          (byte)CollideChannelFlag.eSnowBlock;

            mCollideMatrix[(int)EntityType.eRobot] = (byte)CollideChannelFlag.ePlayerMain |
                                                     (byte)CollideChannelFlag.eRobot |
                                                     (byte)CollideChannelFlag.eSnowBlock;

            mCollideMatrix[(int)EntityType.eSnowBlock] = (byte)CollideChannelFlag.ePlayerMain |
                                                         (byte)CollideChannelFlag.eRobot |
                                                         (byte)CollideChannelFlag.eSnowBlock;
        }

        public void dispose()
        {

        }
    }
}
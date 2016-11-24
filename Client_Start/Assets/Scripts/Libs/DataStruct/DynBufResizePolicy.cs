namespace SDK.Lib
{
    public class DynBufResizePolicy
    {
        // 获取一个最近的大小
        static public uint getCloseSize(uint needSize, uint capacity, uint maxCapacity)
        {
            uint ret = 0;
            if (capacity > needSize)        // 使用 > ，不适用 >= ，浪费一个自己，方便判断
            {
                ret = capacity;
            }
            else
            {
                ret = 2 * capacity;
                while (ret < needSize && ret < maxCapacity)
                {
                    ret *= 2;
                }

                if (ret > maxCapacity)
                {
                    ret = maxCapacity;
                }

                if (ret < needSize)      // 分配失败
                {
                    Ctx.mInstance.mLogSys.error(string.Format("Malloc byte buffer failed，cannot malloc {0} byte buffer", needSize));
                }
            }

            return ret;
        }
    }
}
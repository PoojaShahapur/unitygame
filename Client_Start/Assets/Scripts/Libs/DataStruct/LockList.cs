using System;

namespace SDK.Lib
{
    /**
     * @brief 线程安全列表， T 是 Object ，便于使用 Equal 比较地址
     */
    public class LockList<T>
    {
        protected DynBuffer<T> mDynamicBuffer;
        protected MMutex mVisitMutex;
        protected T mRetItem;

        public LockList(string name, uint initCapacity = 32/*DataCV.INIT_ELEM_CAPACITY*/, uint maxCapacity = 8 * 1024 * 1024/*DataCV.MAX_CAPACITY*/)
        {
            mDynamicBuffer = new DynBuffer<T>(initCapacity, maxCapacity);
            mVisitMutex = new MMutex(false, name);
        }

        public uint Count 
        { 
            get
            {
                using (MLock mlock = new MLock(mVisitMutex))
                {
                    return mDynamicBuffer.mSize;
                }
            }
        }

        public T this[int index] 
        { 
            get
            {
                using (MLock mlock = new MLock(mVisitMutex))
                {
                    if (index < mDynamicBuffer.mSize)
                    {
                        return mDynamicBuffer.mBuffer[index];
                    }
                    else
                    {
                        return default(T);
                    }
                }
            }

            set
            {
                using (MLock mlock = new MLock(mVisitMutex))
                {
                    mDynamicBuffer.mBuffer[index] = value;
                }
            }
        }

        public void Add(T item)
        {
            using (MLock mlock = new MLock(mVisitMutex))
            {
                if (mDynamicBuffer.mSize >= mDynamicBuffer.mCapacity)
                {
                    mDynamicBuffer.extendDeltaCapicity(1);
                }

                mDynamicBuffer.mBuffer[mDynamicBuffer.mSize] = item;
                ++mDynamicBuffer.mSize;
            }
        }

        public bool Remove(T item)
        {
            using (MLock mlock = new MLock(mVisitMutex))
            {
                int idx = 0;
                foreach (var elem in mDynamicBuffer.mBuffer)
                {
                    if(item.Equals(elem))       // 地址比较
                    {
                        this.RemoveAt(idx);
                        return true;
                    }

                    ++idx;
                }
                return false;
            }
        }

        public T RemoveAt(int index)
        {
            using (MLock mlock = new MLock(mVisitMutex))
            {
                if (index < mDynamicBuffer.mSize)
                {
                    mRetItem = mDynamicBuffer.mBuffer[index];

                    if (index < mDynamicBuffer.mSize)
                    {
                        if (index != mDynamicBuffer.mSize - 1 && 1 != mDynamicBuffer.mSize) // 如果删除不是最后一个元素或者总共就大于一个元素
                        {
                            Array.Copy(mDynamicBuffer.mBuffer, index + 1, mDynamicBuffer.mBuffer, index, mDynamicBuffer.mSize - 1 - index);
                        }

                        --mDynamicBuffer.mSize;
                    }
                }
                else
                {
                    mRetItem = default(T);
                }

                return mRetItem;
            }
        }

        public int IndexOf(T item)
        {
            using (MLock mlock = new MLock(mVisitMutex))
            {
                int idx = 0;
                foreach (var elem in mDynamicBuffer.mBuffer)
                {
                    if (item.Equals(elem))       // 地址比较
                    {
                        this.RemoveAt(idx);
                        return idx;
                    }

                    ++idx;
                }
                return -1;
            }
        }
    }
}
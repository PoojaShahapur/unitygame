using System;

namespace SDK.Lib
{
    /**
     * @brief 动态增长的缓冲区，不是环形的，从 0 开始增长的
     */
    public class DynBuffer<T>
    {
        public uint mCapacity;         // 分配的内存空间大小，单位大小是字节
        public uint mMaxCapacity;      // 最大允许分配的存储空间大小 
        public uint mSize;              // 存储在当前缓冲区中的数量
        public T[] mBuffer;            // 当前环形缓冲区

        public DynBuffer(uint initCapacity = 1 * 1024/*DataCV.INIT_CAPACITY*/, uint maxCapacity = 8 * 1024 * 1024/*DataCV.MAX_CAPACITY*/)      // mono 模板类中使用常亮报错， vs 可以
        {
            mMaxCapacity = maxCapacity;
            mCapacity = initCapacity;
            mSize = 0;
            mBuffer = new T[mCapacity];
        }

        public T[] buffer
        {
            get
            {
                return mBuffer;
            }
            set
            {
                mBuffer = value;
                mCapacity = (uint)mBuffer.Length;
            }
        }

        public uint maxCapacity
        {
            get
            {
                return mMaxCapacity;
            }
            set
            {
                mMaxCapacity = value;
            }
        }

        public uint capacity
        {
            get
            {
                return mCapacity;
            }
            set
            {
                if (value == mCapacity)
                {
                    return;
                }
                if (value < this.size)       // 不能分配比当前已经占有的空间还小的空间
                {
                    return;
                }
                T[] tmpbuff = new T[value];   // 分配新的空间
                Array.Copy(mBuffer, 0, tmpbuff, 0, mSize);  // 这个地方是 mSize 还是应该是 mCapacity，如果是 CircleBuffer 好像应该是 mCapacity，如果是 ByteBuffer ，好像应该是 mCapacity。但是 DynBuffer 只有 ByteBuffer 才会使用这个函数，因此使用 mSize 就行了，但是如果使用 mCapacity 也没有问题
                mBuffer = tmpbuff;
                mCapacity = value;
            }
        }

        public uint size
        {
            get
            {
                return mSize;
            }
            set
            {
                if (value > this.capacity)
                {
                    extendDeltaCapicity(value - size);
                }
                mSize = value;
            }
        }

        public void extendDeltaCapicity(uint delta)
        {
            capacity = DynBufResizePolicy.getCloseSize(size + delta, capacity, maxCapacity);
        }
    }
}
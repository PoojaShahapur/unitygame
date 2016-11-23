using System;

/**
 *@brief 环形缓冲区，不支持多线程写操作
 */
namespace SDK.Lib
{
    /**
     *@brief 浪费一个自己，这样判断也好判断，并且索引也不用减 1 ，因此浪费一个字节
     */
    public class CircularBuffer
    {
        protected DynBuffer<byte> mDynBuffer;
        protected uint mFirst;             // 当前缓冲区数据的第一个索引
        protected uint mLast;              // 当前缓冲区数据的最后一个索引的后面一个索引，浪费一个字节
        protected ByteBuffer mTmpBA;        // 临时数据

        public CircularBuffer(uint initCapacity = BufferCV.INIT_CAPACITY, uint maxCapacity = BufferCV.MAX_CAPACITY)
        {
            mDynBuffer = new DynBuffer<byte>(initCapacity, maxCapacity);

            mFirst = 0;
            mLast = 0;

            mTmpBA = new ByteBuffer();
        }

        public uint first
        {
            get
            {
                return mFirst;
            }
        }

        public uint last
        {
            get
            {
                return mLast;
            }
        }

        public byte[] buff
        {
            get
            {
                return mDynBuffer.mBuffer;
            }
        }

        public uint size
        {
            get
            {
                return mDynBuffer.mSize;
            }
            set
            {
                mDynBuffer.size = value;
            }
        }

        public bool isLinearized()
        {
            if (size == 0)
            {
                return true;
            }

            return mFirst < mLast;
        }

        public bool empty()
        {
            return mDynBuffer.mSize == 0;
        }

        public uint capacity()
        {
            return mDynBuffer.mCapacity;
        }

        public bool full()
        { 
            return capacity() == size;
        }

        // 清空缓冲区
        public void clear()
        {
            mDynBuffer.mSize = 0;
            mFirst = 0;
            mLast = 0;
        }

        /**
         * @brief 将数据尽量按照存储地址的从小到大排列
         */
        public void linearize()
        {
            if (empty())        // 没有数据
            {
                return;
            }
            if (isLinearized())      // 数据已经是在一块连续的内存空间
            {
                return;
            }
            else
            {
                // 数据在两个不连续的内存空间中
                char[] tmp = new char[mLast];
                Array.Copy(mDynBuffer.mBuffer, 0, tmp, 0, mLast);  // 拷贝一段内存空间中的数据到 tmp
                Array.Copy(mDynBuffer.mBuffer, mFirst, mDynBuffer.mBuffer, 0, mDynBuffer.mCapacity - mFirst);  // 拷贝第一段数据到 0 索引位置
                Array.Copy(tmp, 0, mDynBuffer.mBuffer, mDynBuffer.mCapacity - mFirst, mLast);      // 拷贝第二段数据到缓冲区

                mFirst = 0;
                mLast = this.size;
            }
        }

        /**
         * @brief 更改存储内容空间大小
         */
        protected void setCapacity(uint newCapacity) 
        {
            if (newCapacity == capacity())
            {
                return;
            }
            if (newCapacity < size)       // 不能分配比当前已经占有的空间还小的空间
            {
                return;
            }
            byte[] tmpbuff = new byte[newCapacity];   // 分配新的空间
            if (isLinearized()) // 如果是在一段内存空间
            {
                // 已经是线性空间了仍然将数据移动到索引 0 的位置
                Array.Copy(mDynBuffer.mBuffer, mFirst, tmpbuff, 0, mDynBuffer.mSize);
            }
            else    // 如果在两端内存空间
            {
                Array.Copy(mDynBuffer.mBuffer, mFirst, tmpbuff, 0, mDynBuffer.mCapacity - mFirst);
                Array.Copy(mDynBuffer.mBuffer, 0, tmpbuff, mDynBuffer.mCapacity - mFirst, mLast);
            }

            mFirst = 0;
            mLast = mDynBuffer.mSize;
            mDynBuffer.mCapacity = newCapacity;
            mDynBuffer.mBuffer = tmpbuff;
        }

        /**
         *@brief 能否添加 num 长度的数据
         */
        protected bool canAddData(uint num)
        {
            if (mDynBuffer.mCapacity - mDynBuffer.mSize > num) // 浪费一个字节，不用 >= ，使用 > 
            {
                return true;
            }

            return false;
        }

        /**
         *@brief 向存储空尾部添加一段内容
         */
        public void pushBackArr(byte[] items, uint start, uint len)
        {
            if (!canAddData(len)) // 存储空间必须要比实际数据至少多 1
            {
                uint closeSize = DynBufResizePolicy.getCloseSize(len + mDynBuffer.mSize, mDynBuffer.mCapacity, mDynBuffer.mMaxCapacity);
                setCapacity(closeSize);
            }

            if (isLinearized())
            {
                if (len <= (mDynBuffer.mCapacity - mLast))
                {
                    Array.Copy(items, start, mDynBuffer.mBuffer, mLast, len);
                }
                else
                {
                    Array.Copy(items, start, mDynBuffer.mBuffer, mLast, mDynBuffer.mCapacity - mLast);
                    Array.Copy(items, mDynBuffer.mCapacity - mLast, mDynBuffer.mBuffer, 0, len - (mDynBuffer.mCapacity - mLast));
                }
            }
            else
            {
                Array.Copy(items, start, mDynBuffer.mBuffer, mLast, len);
            }

            mLast += len;
            mLast %= mDynBuffer.mCapacity;

            mDynBuffer.mSize += len;
        }

        public void pushBackBA(ByteBuffer bu)
        {
            //pushBack(bu.dynBuff.buff, bu.position, bu.bytesAvailable);
            pushBackArr(bu.dynBuff.buff, 0, bu.length);
        }

        /**
         *@brief 向存储空头部添加一段内容
         */
        protected void pushFrontArr(byte[] items)
        {
            if (!canAddData((uint)items.Length)) // 存储空间必须要比实际数据至少多 1
            {
                uint closeSize = DynBufResizePolicy.getCloseSize((uint)items.Length + mDynBuffer.mSize, mDynBuffer.mCapacity, mDynBuffer.mMaxCapacity);
                setCapacity(closeSize);
            }

            if (isLinearized())
            {
                if (items.Length <= mFirst)
                {
                    Array.Copy(items, 0, mDynBuffer.mBuffer, mFirst - items.Length, items.Length);
                }
                else
                {
                    Array.Copy(items, items.Length - mFirst, mDynBuffer.mBuffer, 0, mFirst);
                    Array.Copy(items, 0, mDynBuffer.mBuffer, mDynBuffer.mCapacity - (items.Length - mFirst), items.Length - mFirst);
                }
            }
            else
            {
                Array.Copy(items, 0, mDynBuffer.mBuffer, mFirst - items.Length, items.Length);
            }

            if (items.Length <= mFirst)
            {
                mFirst -= (uint)items.Length;
            }
            else
            {
                mFirst = mDynBuffer.mCapacity - ((uint)items.Length - mFirst);
            }
            mDynBuffer.mSize += (uint)items.Length;
        }

        /**
         * @brief 从 CB 中读取内容，并且将数据删除
         */
        public void popFrontBA(ByteBuffer bytearray, uint len)
        {
            frontBA(bytearray, len);
            popFrontLen(len);
        }

        // 仅仅是获取数据，并不删除
        public void frontBA(ByteBuffer bytearray, uint len)
        {
            bytearray.clear();          // 设置数据为初始值
            if (mDynBuffer.mSize >= len)          // 头部占据 4 个字节
            {
                if (isLinearized())      // 在一段连续的内存
                {
                    bytearray.writeBytes(mDynBuffer.mBuffer, mFirst, len);
                }
                else if (mDynBuffer.mCapacity - mFirst >= len)
                {
                    bytearray.writeBytes(mDynBuffer.mBuffer, mFirst, len);
                }
                else
                {
                    bytearray.writeBytes(mDynBuffer.mBuffer, mFirst, mDynBuffer.mCapacity - mFirst);
                    bytearray.writeBytes(mDynBuffer.mBuffer, 0, len - (mDynBuffer.mCapacity - mFirst));
                }
            }

            bytearray.position = 0;        // 设置数据读取起始位置
        }

        /**
         * @brief 从 CB 头部删除数据
         */
        public void popFrontLen(uint len)
        {
            if (isLinearized())  // 在一段连续的内存
            {
                mFirst += len;
            }
            else if (mDynBuffer.mCapacity - mFirst >= len)
            {
                mFirst += len;
            }
            else
            {
                mFirst = len - (mDynBuffer.mCapacity - mFirst);
            }

            mDynBuffer.mSize -= len;
        }

        // 向自己尾部添加一个 CircularBuffer
        public void pushBackCB(CircularBuffer rhv)
        {
            if(mDynBuffer.mCapacity - mDynBuffer.mSize < rhv.size)
            {
                uint closeSize = DynBufResizePolicy.getCloseSize(rhv.size + mDynBuffer.mSize, mDynBuffer.mCapacity, mDynBuffer.mMaxCapacity);
                setCapacity(closeSize);
            }
            //this.mSize += rhv.size;
            //this.mLast = this.mSize;

            //mTmpBA.clear();
            rhv.frontBA(mTmpBA, rhv.size);
            pushBackBA(mTmpBA);

            //if (rhv.isLinearized()) // 如果是在一段内存空间
            //{
            //    Array.Copy(rhv.buff, rhv.first, mBuffer, 0, rhv.size);
            //}
            //else    // 如果在两端内存空间
            //{
            //    Array.Copy(rhv.buff, rhv.first, mBuffer, 0, rhv.capacity() - rhv.first);
            //    Array.Copy(mBuffer, 0, mBuffer, rhv.capacity() - rhv.first, rhv.last);
            //}
            //rhv.clear();
        }
    }
}
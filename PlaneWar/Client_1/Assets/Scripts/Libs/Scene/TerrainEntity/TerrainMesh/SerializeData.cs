using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 基本信息
     */
    public class SerializeBasic
    {
        public float mWorldSize;
        public ushort mTerrainSize;
        public float mInputScale;
        public float mInputBias;
        public ushort mMaxBatchSize;
        public ushort mMinBatchSize;

        public int getBadicSize()
        {
            return sizeof(float) +
                   sizeof(ushort) +
                   sizeof(float) +
                   sizeof(float) +
                   sizeof(ushort) +
                   sizeof(ushort);
        }

        public void serialize(ByteBuffer buffer)
        {
            buffer.writeFloat(mWorldSize);
            buffer.writeUnsignedInt16(mTerrainSize);
            buffer.writeFloat(mInputScale);
            buffer.writeFloat(mInputBias);
            buffer.writeUnsignedInt16(mMaxBatchSize);
            buffer.writeUnsignedInt16(mMinBatchSize);
        }

        public void deserialize(ByteBuffer buffer)
        {
            buffer.readFloat(ref mWorldSize);
            buffer.readUnsignedInt16(ref mTerrainSize);
            buffer.readFloat(ref mInputScale);
            buffer.readFloat(ref mInputBias);
            buffer.readUnsignedInt16(ref mMaxBatchSize);
            buffer.readUnsignedInt16(ref mMinBatchSize);
        }
    }

    /**
     * @breif 序列化头
     */
    public class SerializeHeader
    {
        public string mUniqueId;
        public int mOffset;

        public void serialize(ByteBuffer buffer, int uniqueIdSize)
        {
            //buffer.writeMultiByte(mUniqueId, Encoding.UTF8, uniqueIdSize);
            buffer.writeMultiByte(mUniqueId, GkEncode.eUTF8, uniqueIdSize);
            buffer.writeInt32(mOffset);
        }

        public void deserialize(ByteBuffer buffer, int uniqueIdSize, int offsetSize)
        {
            //buffer.readMultiByte(ref mUniqueId, (uint)uniqueIdSize, Encoding.UTF8);
            buffer.readMultiByte(ref mUniqueId, (uint)uniqueIdSize, GkEncode.eUTF8);
            buffer.readInt32(ref mOffset);

            //Debug.Log(mUniqueId);
            //Debug.Log(mUniqueId.Length);
            //Debug.Log(mUniqueId[4]);
            int idx = 0;
            idx = mUniqueId.IndexOf("\0");
            if(idx != -1)
            {
                mUniqueId = mUniqueId.Substring(0, idx);
            }
        }
    }

    /**
     * @breif 序列化需要的数据
     */
    public class SerializeData
    {
        public MDictionary<string, SerializeHeader> mHeaderDic;
        public int mHeaderSize;     // 总共的 Node 的个数
        public int mSizePerHeader;  // 每一个头部大小
        public int mTotalHeaderSize;// 总共头部的字节数
        public int mUniqueIdSize;
        public int mOffsetSize;
        public string mTerrainId;

        public ByteBuffer mByteBuffer;
        public BytesRes mByteRes;
        public SerializeBasic mSerializeBasic;

        public SerializeData()
        {
            mUniqueIdSize = 12;
            mOffsetSize = 4;
            mHeaderDic = new MDictionary<string, SerializeHeader>();
            mSizePerHeader = 16;    // 12 个 UniqueId 4 个 offset
            mSerializeBasic = new SerializeBasic();
        }

        public void setHeaderSize(int size)
        {
            mHeaderSize = size;
        }

        public void setTerrainId(string terrainId)
        {
            mTerrainId = terrainId;
        }

        public SerializeHeader getSerialHeader(string uniqueId)
        {
            if(!mHeaderDic.ContainsKey(uniqueId))
            {
                mHeaderDic[uniqueId] = new SerializeHeader();
            }

            return mHeaderDic[uniqueId];
        }

        public void calcHeaderSize()
        {
            mTotalHeaderSize = mHeaderSize * mSizePerHeader;
        }

        public int getBasicAndHeaderSize()
        {
            //return mSerializeBasic.getBadicSize() + mTotalHeaderSize;
            return mTotalHeaderSize;
        }

        public void deserializeHeader()
        {
            if (mByteBuffer == null)
            {
                mByteBuffer = new ByteBuffer();
                mByteBuffer.dynBuffer.maxCapacity = 1000 * 1024 * 1024;
                string path = string.Format("TerrainData/{0}_{1}.bytes", "Map", mTerrainId);
                mByteRes = Ctx.mInstance.mBytesResMgr.getAndSyncLoadRes(path, null);
            }

            byte[] bytes = mByteRes.getBytes("");
            if (bytes != null)
            {
                mByteBuffer.clear();
                mByteBuffer.writeBytes(bytes, 0, (uint)bytes.Length);
                mByteBuffer.setPos(0);
            }

            int idx = 0;
            SerializeHeader serializeHeader = null;
            while (idx < mHeaderSize)
            {
                serializeHeader = new SerializeHeader();
                serializeHeader.deserialize(mByteBuffer, mUniqueIdSize, mOffsetSize);
                if(!mHeaderDic.ContainsKey(serializeHeader.mUniqueId))
                {
                    mHeaderDic[serializeHeader.mUniqueId] = serializeHeader;
                }
                ++idx;
            }
        }

        public void deserializeVertexData(string uniqueId, ref MVertexDataRecord record)
        {
            if(mHeaderDic.ContainsKey(uniqueId))
            {
                mByteBuffer.setPos((uint)mHeaderDic[uniqueId].mOffset);
                record.cpuVertexData.readVertData(mByteBuffer);
            }
        }

        public void deserializeAABB(string uniqueId, int offset, ref MAxisAlignedBox aabb)
        {
            if (mHeaderDic.ContainsKey(uniqueId))
            {
                mByteBuffer.setPos((uint)(mHeaderDic[uniqueId].mOffset + offset));
                mByteBuffer.readAABB(ref aabb);
            }
        }

        public void save(string terrainId, ByteBuffer headerBuffer, ByteBuffer vertexBuffer)
        {
            string path = string.Format("{0}/Resources/TerrainData/{1}_{2}.bytes", Application.dataPath, "Map", terrainId);
            FileStream fileStream;
            try
            {
                if (File.Exists(@path))                  // 如果文件存在
                {
                    File.Delete(@path);
                }

                fileStream = new FileStream(path, FileMode.Create);
                fileStream.Write(headerBuffer.dynBuffer.buffer, 0, (int)headerBuffer.length);
                fileStream.Write(vertexBuffer.dynBuffer.buffer, 0, (int)vertexBuffer.length);
                fileStream.Close();
                fileStream.Dispose();
            }
            catch (Exception e)
            {
            }
        }
    }
}
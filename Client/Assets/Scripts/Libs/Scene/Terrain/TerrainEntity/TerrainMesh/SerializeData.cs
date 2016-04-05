using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @breif 序列化头
     */
    public class SerializeHeader
    {
        public string mUniqueId;
        public int mOffset;

        public void deserilize(ByteBuffer buffer, int uniqueIdSize, int offsetSize)
        {
            buffer.readMultiByte(ref mUniqueId, (uint)uniqueIdSize, Encoding.UTF8);
            buffer.readInt32(ref mOffset);
        }
    }

    /**
     * @breif 序列化需要的数据
     */
    public class SerializeData
    {
        public Dictionary<string, SerializeHeader> m_headerDic;
        public int mHeaderSize;     // 总动的 Node 的个数
        public int mSizePerHeader;  // 每一个头部大小
        public int mTotalHeaderSize;// 总共头部的字节数
        public int mUniqueIdSize;
        public int mOffsetSize;

        public ByteBuffer mByteBuffer;
        public BytesRes m_byteRes;

        public SerializeData()
        {
            mUniqueIdSize = 12;
            mOffsetSize = 4;
            m_headerDic = new Dictionary<string, SerializeHeader>();
            mSizePerHeader = 16;    // 12 个 UniqueId 4 个 offset
        }

        public SerializeHeader getSerialHeader(string uniqueId)
        {
            if(!m_headerDic.ContainsKey(uniqueId))
            {
                m_headerDic[uniqueId] = new SerializeHeader();
            }

            return m_headerDic[uniqueId];
        }

        public void calcHeaderSize()
        {
            mTotalHeaderSize = mHeaderSize * mSizePerHeader;
        }

        public void deserializeHeader()
        {
            if (mByteBuffer == null)
            {
                mByteBuffer = new ByteBuffer();
            }

            m_byteRes = Ctx.m_instance.m_bytesResMgr.getAndSyncLoadRes("XmlConfig/1000.xml");

            byte[] bytes = m_byteRes.getBytes("");
            if (bytes != null)
            {
                mByteBuffer.clear();
                mByteBuffer.writeBytes(bytes, 0, (uint)bytes.Length);
            }

            int idx = 0;
            SerializeHeader serializeHeader = null;
            while (idx < mHeaderSize)
            {
                serializeHeader = new SerializeHeader();
                serializeHeader.deserilize(mByteBuffer, mUniqueIdSize, mOffsetSize);
                if(!m_headerDic.ContainsKey(serializeHeader.mUniqueId))
                {
                    m_headerDic[serializeHeader.mUniqueId] = serializeHeader;
                }
                ++idx;
            }
        }

        public void deserializeVertexData(string uniqueId, ref MVertexDataRecord record, ref MAxisAlignedBox aabb)
        {
            if(m_headerDic.ContainsKey(uniqueId))
            {
                mByteBuffer.setPos((uint)m_headerDic[uniqueId].mOffset);
                record.cpuVertexData.readVertData(mByteBuffer);
                mByteBuffer.readAABB(ref aabb);
            }
        }

        public void save(ByteBuffer headerBuffer, ByteBuffer vertexBuffer)
        {
            string path = string.Format("{0}/{1}.data", Application.dataPath, "map");
            FileStream fileStream;
            try
            {
                if (File.Exists(@path))                  // 如果文件存在
                {
                    File.Delete(@path);
                }

                fileStream = new FileStream(path, FileMode.Create);
                fileStream.Write(headerBuffer.dynBuff.buff, 0, headerBuffer.dynBuff.buff.Length);
                fileStream.Write(vertexBuffer.dynBuff.buff, 0, vertexBuffer.dynBuff.buff.Length);
            }
            catch (Exception e)
            {
                Ctx.m_instance.m_logSys.log(string.Format("{0}\n{1}", e.Message, e.StackTrace));
            }
        }
    }
}
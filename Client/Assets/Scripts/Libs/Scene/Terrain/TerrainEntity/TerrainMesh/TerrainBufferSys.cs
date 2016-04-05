using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 地形缓冲系统
     */
    public class TerrainBufferSys
    {
        protected Dictionary<string, MVertexDataRecord> mVertBuff;
        protected Dictionary<string, MAxisAlignedBox> mVertAABB;

        protected SerializeData mSerializeData;

        public TerrainBufferSys()
        {
            mVertBuff = new Dictionary<string, MVertexDataRecord>();
            mVertAABB = new Dictionary<string, MAxisAlignedBox>();
        }

        public MVertexDataRecord getVertData(string key)
        {
            if(mVertBuff.ContainsKey(key))
            {
                return mVertBuff[key];
            }
            else
            {
                MVertexDataRecord record = new MVertexDataRecord();
                MAxisAlignedBox aabb = new MAxisAlignedBox();

                //mSerializeData.deserializeVertexData(key, ref record, ref aabb);
                //mVertBuff[key] = record;
                //mVertAABB[key] = aabb;
            }

            return null;
        }

        public void addVertData(string key, MVertexDataRecord vertData)
        {
            if (!mVertBuff.ContainsKey(key))
            {
                mVertBuff[key] = vertData;
            }
            else
            {
                Debug.Log("Error");
            }
        }

        public bool getAABB(string key, ref MAxisAlignedBox aabb)
        {
            if (mVertAABB.ContainsKey(key))
            {
                aabb = mVertAABB[key];
                return true;
            }

            return false;
        }

        public void addAABB(string key, ref MAxisAlignedBox aabb)
        {
            if (!mVertAABB.ContainsKey(key))
            {
                mVertAABB[key] = aabb;
            }
            else
            {
                Debug.Log("Error");
            }
        }

        public void deserialize()
        {
            if(mSerializeData == null)
            {
                mSerializeData = new SerializeData();
            }

            mSerializeData.deserializeHeader();
        }
    }
}
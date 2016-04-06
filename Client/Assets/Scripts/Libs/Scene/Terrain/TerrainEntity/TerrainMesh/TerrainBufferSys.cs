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
        protected Dictionary<string, TerrainTileRender> mTerrainTileRenderDic;
        protected TerrainMat mTerrainMat;

        protected SerializeData mSerializeData;
        protected bool mIsReadHeader;       // 是否读取了头部
        protected MImportData mMImportData;

        protected bool mIsReadFile; // 是否从文件读取所有的数据

        public TerrainBufferSys()
        {
            mVertBuff = new Dictionary<string, MVertexDataRecord>();
            mVertAABB = new Dictionary<string, MAxisAlignedBox>();
            mTerrainTileRenderDic = new Dictionary<string, TerrainTileRender>();
            mTerrainMat = new TerrainMat();
            mIsReadHeader = false;
            mIsReadFile = true;
            mMImportData = new MImportData();
            setHeaderSize(((mMImportData.terrainSize - 1) / (mMImportData.maxBatchSize - 1)) * ((mMImportData.terrainSize - 1) / (mMImportData.maxBatchSize - 1)));
        }

        public bool IsReadFile()
        {
            return mIsReadFile;
        }

        public void loadNeedRes()
        {
            deserialize();
            loadMat();
        }

        public void loadMat()
        {
            mTerrainMat = new TerrainMat();
            mTerrainMat.setDiffuseMap(mMImportData.diffusePath);
            if (!mMImportData.isUseSplatMap)
            {
                mTerrainMat.loadDiffuseMat();
            }
            else
            {
                float mUVMultiplier = mMImportData.worldSize / mMImportData.detailWorldSize;
                mTerrainMat.setUVMultiplier(mUVMultiplier);
                mTerrainMat.loadSplatDiffuseMat();
            }
        }

        public bool getVertData(string key, ref MVertexDataRecord record)
        {
            if (!mIsReadFile)
            {
                return false;
            }

            if (mVertBuff.ContainsKey(key))
            {
                record = mVertBuff[key];
                return true;
            }
            else
            {
                record = new MVertexDataRecord();

                mSerializeData.deserializeVertexData(key, ref record);
                mVertBuff[key] = record;
                return true;
            }

            //return false;
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
            if(!mIsReadFile)
            {
                return false;
            }
            if (mVertAABB.ContainsKey(key))
            {
                aabb = mVertAABB[key];
                return true;
            }
            else
            {
                aabb = new MAxisAlignedBox(MAxisAlignedBox.Extent.EXTENT_FINITE);
                mSerializeData.deserializeAABB(key, mMImportData.calcTotalByte(), ref aabb);
                mVertAABB[key] = aabb;
                return true;
            }

            //return false;
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

        public bool getTerrainTileRender(string key, ref TerrainTileRender render)
        {
            if(!mIsReadFile)
            {
                return false;
            }
            if (mTerrainTileRenderDic.ContainsKey(key))
            {
                render = mTerrainTileRenderDic[key];
                mTerrainTileRenderDic.Remove(key);
                return true;
            }
            else
            {
                render = new TerrainTileRender(null);
                render.setTmplMaterial(getMatTmpl());
            }

            return false;
        }

        public void addTerrainTileRender(string key, ref TerrainTileRender render)
        {
            if (!mIsReadFile)
            {
                return;
            }

            if (!mTerrainTileRenderDic.ContainsKey(key))
            {
                mTerrainTileRenderDic[key] = render;
                render = null;
            }
            else
            {
                Debug.Log("Error");
            }
        }

        public bool getTerrainMat(ref TerrainMat mat)
        {
            if (!mIsReadFile)
            {
                return false;
            }

            if (mTerrainMat != null)
            {
                mat = mTerrainMat;
                return true;
            }

            return false;
        }

        public void addTerrainMat(ref TerrainMat mat)
        {
            if (mTerrainMat == null)
            {
                mTerrainMat = mat;
            }
            else
            {
                Debug.Log("Error");
            }
        }

        public void deserialize()
        {
            if (!mIsReadHeader)
            {
                if (mSerializeData == null)
                {
                    mSerializeData = new SerializeData();
                }

                mSerializeData.deserializeHeader();
                mIsReadHeader = true;
            }
        }

        public void setHeaderSize(int size)
        {
            if (mSerializeData == null)
            {
                mSerializeData = new SerializeData();
            }
            mSerializeData.setHeaderSize(size);
        }

        public Material getMatTmpl()
        {
            if (!mMImportData.isUseSplatMap)
            {
                return mTerrainMat.getDiffuseMaterial();
            }
            else
            {
                return mTerrainMat.getSplatMaterial();
            }
        }
    }
}
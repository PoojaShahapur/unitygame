using System.Collections.Generic;
using System.Security;
using UnityEngine;

namespace SDK.Lib
{
    public class TerrainBuffer
    {
        protected MDictionary<string, MVertexDataRecord> mVertBuff;
        protected MDictionary<string, MAxisAlignedBox> mVertAABB;
        protected MDictionary<string, MList<TerrainTileRender>> mTerrainTileRenderDic;
        protected TerrainMat mTerrainMat;

        protected SerializeData mSerializeData;
        protected bool mIsReadHeader;       // 是否读取了头部
        protected MImportData mMImportData;

        public TerrainBuffer()
        {
            mVertBuff = new MDictionary<string, MVertexDataRecord>();
            mVertAABB = new MDictionary<string, MAxisAlignedBox>();
            mTerrainTileRenderDic = new MDictionary<string, MList<TerrainTileRender>>();
            mTerrainMat = new TerrainMat();
            mIsReadHeader = false;
            mMImportData = new MImportData();
        }

        public void loadNeedRes()
        {
            setHeaderSize(((mMImportData.terrainSize - 1) / (mMImportData.maxBatchSize - 1)) * ((mMImportData.terrainSize - 1) / (mMImportData.maxBatchSize - 1)));

            mTerrainMat = new TerrainMat();

            mMImportData.parseXml();
            mTerrainMat.initSplatPath(mMImportData);
            deserialize();
            loadMat();
        }

        public void loadNeedResByXml(string terrainId, SecurityElement itemElem)
        {
            setHeaderSize(((mMImportData.terrainSize - 1) / (mMImportData.maxBatchSize - 1)) * ((mMImportData.terrainSize - 1) / (mMImportData.maxBatchSize - 1)));

            mTerrainMat = new TerrainMat();

            mMImportData.setTerrainId(terrainId);
            mMImportData.parseXmlNode(itemElem);
            mTerrainMat.initSplatPath(mMImportData);
            deserialize();
            loadMat();
        }

        public void loadMat()
        {
            if (!mMImportData.isUseSplatMap)
            {
                mTerrainMat.setDiffuseMap(mMImportData.diffusePath);
                mTerrainMat.loadDiffuseMat();
            }
            else
            {
                //float mUVMultiplier = mMImportData.worldSize / mMImportData.detailWorldSize;
                //mTerrainMat.setUVMultiplier(mUVMultiplier);
                mTerrainMat.loadSplatDiffuseMat();
            }
        }

        public bool getVertData(string key, ref MVertexDataRecord record)
        {
            if (!Ctx.mInstance.mTerrainGlobalOption.mIsReadFile)
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
                //Debug.Log("Error");
            }
        }

        public bool getAABB(string key, ref MAxisAlignedBox aabb)
        {
            if (!Ctx.mInstance.mTerrainGlobalOption.mIsReadFile)
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
                //Debug.Log("Error");
            }
        }

        public bool getTerrainTileRender(string key, ref TerrainTileRender render)
        {
            if (!Ctx.mInstance.mTerrainGlobalOption.mIsReadFile)
            {
                return false;
            }
            if (mTerrainTileRenderDic.ContainsKey(key) && mTerrainTileRenderDic[key].length() > 0)
            {
                render = mTerrainTileRenderDic[key][0];
                mTerrainTileRenderDic[key].Remove(render);
                return true;
            }
            else
            {
                render = new TerrainTileRender(null);
                render.pntGo = Ctx.mInstance.mSceneNodeGraph.mSceneNodes[(int)eSceneNodeId.eSceneTerrainRoot];
                render.setTmplMaterial(getMatTmpl());
            }

            return false;
        }

        public void addTerrainTileRender(string key, ref TerrainTileRender render)
        {
            if (!Ctx.mInstance.mTerrainGlobalOption.mIsReadFile)
            {
                return;
            }

            if (!mTerrainTileRenderDic.ContainsKey(key))
            {
                mTerrainTileRenderDic[key] = new MList<TerrainTileRender>();
            }
            if (mTerrainTileRenderDic[key].IndexOf(render) == -1)
            {
                mTerrainTileRenderDic[key].Add(render);
            }
            render = null;
        }

        public bool getTerrainMat(ref TerrainMat mat)
        {
            if (!Ctx.mInstance.mTerrainGlobalOption.mIsReadFile)
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

                mSerializeData.setTerrainId(mMImportData.mTerrainId);
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
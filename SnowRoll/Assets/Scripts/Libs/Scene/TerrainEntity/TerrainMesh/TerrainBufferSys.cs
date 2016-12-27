using Mono.Xml;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Text;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 
     */
    public class ScenePageItem
    {
        public string mId;
    }

    /**
     * @brief 地形缓冲系统
     */
    public class TerrainBufferSys
    {
        protected MDictionary<string, TerrainBuffer> mTerrainBufferDic;
        public TextRes mTextRes;
        public MDictionary<int, MDictionary<int, ScenePageItem>> mScenePageCfg;
        public StreamWriter mStreamWriter;

        public TerrainVisibleCheck mTerrainVisibleCheck;

        public TerrainBufferSys()
        {
            init();
        }

        public void init()
        {
            mTerrainBufferDic = new MDictionary<string, TerrainBuffer>();
            mTerrainVisibleCheck = new TerrainVisibleCheck();
        }

        public void clearBuffer()
        {
            init();
        }

        public void loadNeedRes()
        {
            mTextRes = Ctx.mInstance.mTextResMgr.getAndSyncLoadRes("TerrainData/Terrain.xml", null);
            if (mTextRes != null)
            {
                string text = mTextRes.getText("");
                SecurityParser xmlDoc = new SecurityParser();
                xmlDoc.LoadXml(text);
                SecurityElement config = xmlDoc.ToXml();
                ArrayList itemNodeList = new ArrayList();
                UtilXml.getXmlChildList(config, "Terrain", ref itemNodeList);
                string terrainId = "";

                foreach (SecurityElement itemElem in itemNodeList)
                {
                    UtilXml.getXmlAttrStr(itemElem, "id", ref terrainId);
                    mTerrainBufferDic[terrainId] = new TerrainBuffer();
                    mTerrainBufferDic[terrainId].loadNeedResByXml(terrainId, itemElem);
                }
            }
        }

        public bool getVertData(string terrainId, string key, ref MVertexDataRecord record)
        {
            if (!Ctx.mInstance.mTerrainGlobalOption.mIsReadFile)
            {
                return false;
            }
            return mTerrainBufferDic[terrainId].getVertData(key, ref record);
        }

        public void addVertData(string terrainId, string key, MVertexDataRecord vertData)
        {
            mTerrainBufferDic[terrainId].addVertData(key, vertData);
        }

        public bool getAABB(string terrainId, string key, ref MAxisAlignedBox aabb)
        {
            if (!Ctx.mInstance.mTerrainGlobalOption.mIsReadFile)
            {
                return false;
            }
            return mTerrainBufferDic[terrainId].getAABB(key, ref aabb);
        }

        public void addAABB(string terrainId, string key, ref MAxisAlignedBox aabb)
        {
            mTerrainBufferDic[terrainId].addAABB(key, ref aabb);
        }

        public bool getTerrainTileRender(string terrainId, string key, ref TerrainTileRender render)
        {
            if (!Ctx.mInstance.mTerrainGlobalOption.mIsReadFile)
            {
                return false;
            }
            return mTerrainBufferDic[terrainId].getTerrainTileRender(key, ref render);
        }

        public void addTerrainTileRender(string terrainId, string key, ref TerrainTileRender render)
        {
            if (!Ctx.mInstance.mTerrainGlobalOption.mIsReadFile)
            {
                return;
            }

            mTerrainBufferDic[terrainId].addTerrainTileRender(key, ref render);
        }

        public bool getTerrainMat(string terrainId, ref TerrainMat mat)
        {
            if (!Ctx.mInstance.mTerrainGlobalOption.mIsReadFile)
            {
                return false;
            }
            return mTerrainBufferDic[terrainId].getTerrainMat(ref mat);
        }

        public void addTerrainMat(string terrainId, ref TerrainMat mat)
        {
            mTerrainBufferDic[terrainId].getTerrainMat(ref mat);
        }

        public void setHeaderSize(string terrainId, int size)
        {
            if(!mTerrainBufferDic.ContainsKey(terrainId))
            {
                mTerrainBufferDic[terrainId] = new TerrainBuffer();
            }

            mTerrainBufferDic[terrainId].setHeaderSize(size);
        }

        public void deserialize(string terrainId)
        {
            if (!mTerrainBufferDic.ContainsKey(terrainId))
            {
                mTerrainBufferDic[terrainId] = new TerrainBuffer();
            }
            mTerrainBufferDic[terrainId].deserialize();
        }

        public void loadSceneCfg(string path)
        {
            if(mScenePageCfg == null)
            {
                mScenePageCfg = new MDictionary<int, MDictionary<int, ScenePageItem>>();
            }
            else
            {
                mScenePageCfg.Clear();
            }

            mTextRes = Ctx.mInstance.mTextResMgr.getAndSyncLoadRes(string.Format("TerrainData/{0}.xml", path), null);
            if (mTextRes != null)
            {
                string text = mTextRes.getText("");
                SecurityParser xmlDoc = new SecurityParser();
                xmlDoc.LoadXml(text);
                SecurityElement config = xmlDoc.ToXml();
                ArrayList itemNodeList = new ArrayList();
                UtilXml.getXmlChildList(config, "Page", ref itemNodeList);
                string id = "";
                int x = 0;
                int y = 0;

                foreach (SecurityElement itemElem in itemNodeList)
                {
                    UtilXml.getXmlAttrStr(itemElem, "id", ref id);
                    UtilXml.getXmlAttrInt(itemElem, "x", ref x);
                    UtilXml.getXmlAttrInt(itemElem, "y", ref y);
                    if(!mScenePageCfg.ContainsKey(y))
                    {
                        mScenePageCfg[y] = new MDictionary<int, ScenePageItem>();
                    }
                    if (!mScenePageCfg[y].ContainsKey(x))
                    {
                        mScenePageCfg[y][x] = new ScenePageItem();
                    }
                    mScenePageCfg[y][x].mId = id;
                }
            }
        }

        public string getTerrainId(int x, int y)
        {
            if(mScenePageCfg.ContainsKey(y) && mScenePageCfg[y].ContainsKey(x))
            {
                return mScenePageCfg[y][x].mId;
            }

            return "";
        }

        public void openFile()
        {
            string fileName = string.Format("{0}/{1}.txt", Application.dataPath, "aaa");
            mStreamWriter = new StreamWriter(fileName);
        }

        public void writeVertex(MVector3 vert, float height)
        {
            StringBuilder sb = null;
            sb = new StringBuilder("v ", 20);
            sb.Append(vert.x.ToString()).Append(" ").
            Append(vert.y.ToString()).Append(" ").
            Append(vert.z.ToString()).Append(" ").Append(height);
            mStreamWriter.WriteLine(sb);
        }

        public void closeFile()
        {
            mStreamWriter.Close();
        }
    }
}
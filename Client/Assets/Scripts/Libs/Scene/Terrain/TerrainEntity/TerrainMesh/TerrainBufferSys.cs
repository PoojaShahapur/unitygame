using Mono.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 地形缓冲系统
     */
    public class TerrainBufferSys
    {
        protected Dictionary<string, TerrainBuffer> mTerrainBufferDic;
        public TextRes m_textRes;

        public TerrainBufferSys()
        {
            mTerrainBufferDic = new Dictionary<string, TerrainBuffer>();
        }

        public void loadNeedRes()
        {
            m_textRes = Ctx.m_instance.m_textResMgr.getAndSyncLoadRes("XmlConfig/Terrain.xml");
            if (m_textRes != null)
            {
                string text = m_textRes.getText("");
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
            return mTerrainBufferDic[terrainId].getVertData(key, ref record);
        }

        public void addVertData(string terrainId, string key, MVertexDataRecord vertData)
        {
            mTerrainBufferDic[terrainId].addVertData(key, vertData);
        }

        public bool getAABB(string terrainId, string key, ref MAxisAlignedBox aabb)
        {
            return mTerrainBufferDic[terrainId].getAABB(key, ref aabb);
        }

        public void addAABB(string terrainId, string key, ref MAxisAlignedBox aabb)
        {
            mTerrainBufferDic[terrainId].addAABB(key, ref aabb);
        }

        public bool getTerrainTileRender(string terrainId, string key, ref TerrainTileRender render)
        {
            return mTerrainBufferDic[terrainId].getTerrainTileRender(key, ref render);
        }

        public void addTerrainTileRender(string terrainId, string key, ref TerrainTileRender render)
        {
            mTerrainBufferDic[terrainId].addTerrainTileRender(key, ref render);
        }

        public bool getTerrainMat(string terrainId, ref TerrainMat mat)
        {
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
    }
}
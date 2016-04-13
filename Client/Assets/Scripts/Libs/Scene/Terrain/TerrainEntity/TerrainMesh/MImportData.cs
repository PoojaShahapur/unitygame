﻿using Mono.Xml;
using System.Collections;
using System.Security;

namespace SDK.Lib
{
    public class LayerInstance
	{
		public float worldSize;
        public string textureName;

        public LayerInstance()
        {
            worldSize = 16;
        }
    }

    public class MImportData
    {
        public string mTerrainId;   // 地图配置文件唯一 ID
        public ushort terrainSize;
        public ushort maxBatchSize;
        public ushort minBatchSize;
        public MVector3 pos;
        public float worldSize;
        public string diffusePath;
        public string heightPath;
        public float inputScale;
        public float inputBias;
        public bool deleteInputData;
        public int detailWorldSize;
        public bool isUseSplatMap;
        public MList<LayerInstance> layerList;
        public string mAlphaTexName;
        public string mFileName;
        public TextRes m_textRes;
        public string m_terrainId;

        public long x, y;

        public MImportData()
        {
            terrainSize = Ctx.m_instance.mTerrainGlobalOption.mTerrainSize;
            maxBatchSize = Ctx.m_instance.mTerrainGlobalOption.mMaxBatchSize;
            minBatchSize = Ctx.m_instance.mTerrainGlobalOption.mMinBatchSize;
            pos = MVector3.ZERO;
            worldSize = Ctx.m_instance.mTerrainGlobalOption.mTerrainWorldSize;
            diffusePath = "Materials/Textures/Terrain/TerrainDiffuse_1.png";
            //diffusePath = "Materials/Textures/Terrain/terrain_diffuse.png";
            heightPath = "Materials/Textures/Terrain/TerrainHeight_1.png";
            //heightPath = "Materials/Textures/Terrain/terrain.png";
            inputScale = Ctx.m_instance.mTerrainGlobalOption.mInputScale;
            inputBias = Ctx.m_instance.mTerrainGlobalOption.mInputBias;
            deleteInputData = true;
            detailWorldSize = 8;
            isUseSplatMap = true;
            layerList = new MList<LayerInstance>();

            //parseXml();
        }

        public void setTerrainId(string terrainId)
        {
            mTerrainId = terrainId;
        }

        public void assignFrom(MImportData rhs)
        {
            terrainSize = rhs.terrainSize;
            maxBatchSize = rhs.maxBatchSize;
            minBatchSize = rhs.minBatchSize;
            pos = rhs.pos;
            worldSize = rhs.worldSize;
            diffusePath = rhs.diffusePath;
            heightPath = rhs.heightPath;
            inputScale = rhs.inputScale;
            inputBias = rhs.inputBias;
            deleteInputData = rhs.deleteInputData;
        }

        public int calcTotalByte()
        {
            // 顶点， UV，法向量，切向量，索引
            return maxBatchSize * maxBatchSize * sizeof(float) * 3 + maxBatchSize * maxBatchSize * sizeof(float) * 2 + maxBatchSize * maxBatchSize * sizeof(float) * 3 + maxBatchSize * maxBatchSize * sizeof(float) * 4 + (maxBatchSize - 1) * (maxBatchSize - 1) * 6 * sizeof(int);
        }

        public void parseXml()
        {
            m_textRes = Ctx.m_instance.m_textResMgr.getAndSyncLoadRes(string.Format("XmlConfig/{0}.xml", m_terrainId));
            if (m_textRes != null)
            {
                string text = m_textRes.getText("");
                SecurityParser xmlDoc = new SecurityParser();
                xmlDoc.LoadXml(text);
                SecurityElement config = xmlDoc.ToXml();
                parseXmlNode(config);
            }
        }

        public void parseXmlNode(SecurityElement terrain)
        {
            ArrayList itemNodeList = new ArrayList();
            UtilXml.getXmlChildList(terrain, "SplatName", ref itemNodeList);

            LayerInstance ins = null;
            foreach (SecurityElement itemElem in itemNodeList)
            {
                ins = new LayerInstance();
                layerList.Add(ins);
                UtilXml.getXmlAttrStr(itemElem, "name", ref ins.textureName);
                UtilXml.getXmlAttrFloat(itemElem, "worldSize", ref ins.worldSize);
            }

            itemNodeList.Clear();
            UtilXml.getXmlChildList(terrain, "AlphaName", ref itemNodeList);
            foreach (SecurityElement itemElem in itemNodeList)
            {
                UtilXml.getXmlAttrStr(itemElem, "name", ref mAlphaTexName);
            }
        }
    }
}

using Mono.Xml;
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
        public string mHeightDataPath;  // 高度数据目录
        public float inputScale;
        public float inputBias;
        public bool deleteInputData;
        public int detailWorldSize;
        public bool isUseSplatMap;
        public MList<LayerInstance> layerList;
        public string mAlphaTexName;
        public TextRes mTextRes;

        public long x, y;

        public MImportData()
        {
            terrainSize = Ctx.mInstance.mTerrainGlobalOption.mTerrainSize;
            maxBatchSize = Ctx.mInstance.mTerrainGlobalOption.mMaxBatchSize;
            minBatchSize = Ctx.mInstance.mTerrainGlobalOption.mMinBatchSize;
            pos = MVector3.ZERO;
            worldSize = Ctx.mInstance.mTerrainGlobalOption.mTerrainWorldSize;
            diffusePath = "Materials/Textures/Terrain/TerrainDiffuse_1.png";
            //diffusePath = "Materials/Textures/Terrain/terrain_diffuse.png";
            heightPath = "Materials/Textures/Terrain/TerrainHeight_1.png";
            //heightPath = "Materials/Textures/Terrain/terrain.png";
            inputScale = Ctx.mInstance.mTerrainGlobalOption.mInputScale;
            inputBias = Ctx.mInstance.mTerrainGlobalOption.mInputBias;
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
            mTextRes = Ctx.mInstance.mTextResMgr.getAndSyncLoadRes(string.Format("TerrainData/{0}.xml", mTerrainId), null);
            if (mTextRes != null)
            {
                string text = mTextRes.getText("");
                SecurityParser xmlDoc = new SecurityParser();
                xmlDoc.LoadXml(text);
                SecurityElement config = xmlDoc.ToXml();
                parseXmlNode(config);
            }
        }

        public void parseXmlNode(SecurityElement terrain)
        {
            ArrayList itemNodeList = new ArrayList();
            UtilXml.getXmlChildList(terrain, "SplatMapName", ref itemNodeList);

            // 获取 Splat 数据
            LayerInstance ins = null;
            foreach (SecurityElement itemElem in itemNodeList)
            {
                ins = new LayerInstance();
                layerList.Add(ins);
                UtilXml.getXmlAttrStr(itemElem, "name", ref ins.textureName);
                UtilXml.getXmlAttrFloat(itemElem, "worldSize", ref ins.worldSize);
            }

            // 获取 Alpha 数据
            itemNodeList.Clear();
            UtilXml.getXmlChildList(terrain, "AlphaMapName", ref itemNodeList);
            foreach (SecurityElement itemElem in itemNodeList)
            {
                UtilXml.getXmlAttrStr(itemElem, "name", ref mAlphaTexName);
            }

            // 获取高度数据
            itemNodeList.Clear();
            UtilXml.getXmlChildList(terrain, "HeightDataName", ref itemNodeList);
            if (itemNodeList.Count > 0)
            {
                UtilXml.getXmlAttrStr(itemNodeList[0] as SecurityElement, "name", ref mHeightDataPath);
            }

            // 获取世界大小
            itemNodeList.Clear();
            UtilXml.getXmlChildList(terrain, "WorldSize", ref itemNodeList);
            if (itemNodeList.Count > 0)
            {
                UtilXml.getXmlAttrUShort(itemNodeList[0] as SecurityElement, "Width", ref terrainSize);
                UtilXml.getXmlAttrFloat(itemNodeList[0] as SecurityElement, "Height", ref inputScale);
            }

            // 获取批次大小
            itemNodeList.Clear();
            UtilXml.getXmlChildList(terrain, "BatchSize", ref itemNodeList);
            if (itemNodeList.Count > 0)
            {
                UtilXml.getXmlAttrUShort(itemNodeList[0] as SecurityElement, "Max", ref maxBatchSize);
                UtilXml.getXmlAttrUShort(itemNodeList[0] as SecurityElement, "Min", ref minBatchSize);
            }

            // 获取高度图分辨率
            itemNodeList.Clear();
            UtilXml.getXmlChildList(terrain, "HeightMapResolution", ref itemNodeList);
            if (itemNodeList.Count > 0)
            {
                UtilXml.getXmlAttrUShort(itemNodeList[0] as SecurityElement, "Size", ref terrainSize);
            }

            // 获取高度贴图
            itemNodeList.Clear();
            UtilXml.getXmlChildList(terrain, "HeightMapName", ref itemNodeList);
            if (itemNodeList.Count > 0)
            {
                UtilXml.getXmlAttrStr(itemNodeList[0] as SecurityElement, "name", ref heightPath);
            }

            // 获取 Diffuse 贴图数据
            itemNodeList.Clear();
            UtilXml.getXmlChildList(terrain, "DiffuseMapName", ref itemNodeList);
            if (itemNodeList.Count > 0)
            {
                UtilXml.getXmlAttrStr(itemNodeList[0] as SecurityElement, "name", ref diffusePath);
            }
        }
    }
}

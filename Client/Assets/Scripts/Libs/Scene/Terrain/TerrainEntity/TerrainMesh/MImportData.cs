namespace SDK.Lib
{
    public class MImportData
    {
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

        public MImportData()
        {
            terrainSize = 1025;
            maxBatchSize = 65;
            minBatchSize = 33;
            pos = MVector3.ZERO;
            worldSize = 1024;
            diffusePath = "Materials/Textures/Terrain/TerrainDiffuse_1.png";
            //diffusePath = "Materials/Textures/Terrain/terrain_diffuse.png";
            heightPath = "Materials/Textures/Terrain/TerrainHeight_1.png";
            //heightPath = "Materials/Textures/Terrain/terrain.png";
            inputScale = 1023;
            inputBias = 0;
            deleteInputData = true;
            detailWorldSize = 16;
            isUseSplatMap = true;
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
    }
}

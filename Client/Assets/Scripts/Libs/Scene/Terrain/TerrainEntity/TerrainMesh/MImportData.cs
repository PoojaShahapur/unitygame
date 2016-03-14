namespace SDK.Lib
{
    public class MImportData
    {
        public ushort terrainSize;
        public ushort maxBatchSize;
        public ushort minBatchSize;
        public MVector3 pos;
        public float worldSize;
        //public Image* inputImage;
        public string heightPath;
        public float inputScale;
        public float inputBias;
        public bool deleteInputData;

        public MImportData()
        {
            terrainSize = 513;
            maxBatchSize = 65;
            minBatchSize = 33;
            pos = MVector3.ZERO;
            worldSize = 512;
            //public Image* inputImage;
            heightPath = "Materials/Texture/Terrain/TerrainHeight_1.png";
            inputScale = 255;
            inputBias = 0;
            deleteInputData = true;
        }
    }
}

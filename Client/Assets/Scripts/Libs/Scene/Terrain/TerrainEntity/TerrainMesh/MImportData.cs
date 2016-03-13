namespace SDK.Lib
{
    public struct MImportData
    {
        public int terrainSize;
        public int maxBatchSize;
        public int minBatchSize;
        public MVector3 pos;
        public float worldSize;
        //public Image* inputImage;
        public float inputScale;
        public float inputBias;
        public bool deleteInputData;
    }
}

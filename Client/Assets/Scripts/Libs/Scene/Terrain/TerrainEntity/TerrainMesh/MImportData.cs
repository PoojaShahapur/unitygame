namespace SDK.Lib
{
    public struct MImportData
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
    }
}

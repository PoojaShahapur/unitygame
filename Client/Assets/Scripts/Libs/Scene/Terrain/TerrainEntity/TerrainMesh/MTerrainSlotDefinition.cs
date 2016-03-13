namespace SDK.Lib
{
    public struct MTerrainSlotDefinition
    {
        public string filename;
        public MImportData importData;

        public void useImportData()
        {
            filename = "";
            importData = new MImportData();
            importData.deleteInputData = true;
        }
    }
}
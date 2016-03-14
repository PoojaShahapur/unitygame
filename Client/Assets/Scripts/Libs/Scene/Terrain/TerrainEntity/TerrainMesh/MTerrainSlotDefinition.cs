namespace SDK.Lib
{
    public class MTerrainSlotDefinition
    {
        public string filename;
        public MImportData importData;

        public MTerrainSlotDefinition()
        {
            filename = "Materials/Texture/Terrain/terrain.png";
        }

        public void useImportData()
        {
            importData = new MImportData();
            importData.deleteInputData = true;
        }
    }
}
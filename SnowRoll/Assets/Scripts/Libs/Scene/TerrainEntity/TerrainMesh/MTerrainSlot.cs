namespace SDK.Lib
{
    public class MTerrainSlot
    {
        public long x, y;
        public MTerrainSlotDefinition def;
        public MTerrain instance;

        public MTerrainSlot(long _x, long _y)
        {
            x = _x;
            y = _y;
            def = new MTerrainSlotDefinition();
            instance = null;
        }

        public void freeInstance()
        {
            instance = null;
        }
    }
}
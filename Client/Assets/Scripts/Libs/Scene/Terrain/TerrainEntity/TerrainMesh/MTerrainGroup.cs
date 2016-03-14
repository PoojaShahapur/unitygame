using System.Collections.Generic;

namespace SDK.Lib
{
    public class MTerrainGroup
    {
        protected int mTerrainSize;
        protected float mTerrainWorldSize;
        protected MImportData mDefaultImportData;
        protected MVector3 mOrigin;
        protected Dictionary<int, MTerrainSlot> mTerrainSlots;
        protected string mFilenamePrefix;
        protected string mFilenameExtension;

        public MTerrainGroup(ushort terrainSize, float terrainWorldSize)
        {
            mTerrainSlots = new Dictionary<int, MTerrainSlot>();
            mTerrainSize = terrainSize;
            mTerrainWorldSize = terrainWorldSize;
            mOrigin = MVector3.ZERO;
            mFilenamePrefix = "terrain";
            mFilenameExtension = "dat";

            mDefaultImportData = new MImportData();
            mDefaultImportData.terrainSize = terrainSize;
            mDefaultImportData.worldSize = terrainWorldSize;
        }

        public void defineTerrain(long x, long y)
        {
            MTerrainSlot slot = getTerrainSlot(x, y, true);
            slot.def.useImportData();

            slot.def.importData.terrainSize = (ushort)mTerrainSize;
            slot.def.importData.worldSize = mTerrainWorldSize;
        }

        public MTerrainSlot getTerrainSlot(long x, long y, bool createIfMissing)
        {
            int key = (int)packIndex(x, y);
            if (mTerrainSlots.ContainsKey(key))
                return mTerrainSlots[key];
            else if (createIfMissing)
            {
                MTerrainSlot slot = new MTerrainSlot(x, y);
                mTerrainSlots[key] = slot;
                return slot;
            }
            return new MTerrainSlot();
        }

        public MTerrainSlot getTerrainSlot(long x, long y)
        {
            int key = (int)packIndex(x, y);
            if (mTerrainSlots.ContainsKey(key))
                return mTerrainSlots[key];
            else
                return new MTerrainSlot();
        }

        public void loadTerrain(long x, long y, bool synchronous /*= false*/)
        {
            MTerrainSlot slot = getTerrainSlot(x, y, false);
            //if (slot)
            {
                loadTerrainImpl(slot, synchronous);
            }
        }

        public void loadTerrainImpl(MTerrainSlot slot, bool synchronous)
        {
            if (slot.instance == null && !string.IsNullOrEmpty(slot.def.filename))
            {
                slot.instance = new MTerrain();
                slot.instance.prepare(slot.def.importData);
            }
        }

        public uint packIndex(long x, long y)
        {
            short xs16 = (short)(x);
            short ys16 = (short)(y);

            ushort x16 = (ushort)(xs16);
            ushort y16 = (ushort)(ys16);

            uint key = 0;
            key = (uint)((x16 << 16) | y16);

            return key;
        }

        public void unpackIndex(uint key, ref long x, ref long y)
        {
            ushort y16 = (ushort)(key & 0xFFFF);
            ushort x16 = (ushort)((key >> 16) & 0xFFFF);

            x = (short)(x16);
            y = (short)(y16);
        }

        public string generateFilename(long x, long y)
        {
            string str;
            str = (mFilenamePrefix + "_" + packIndex(x, y) + "." + mFilenameExtension);
            return str;
        }

        public void showTerrain(long x, long y)
        {
            MTerrainSlot terrain = getTerrainSlot(x, y, false);
            if(terrain.instance != null)
            {
                terrain.instance.show();
            }
        }
    }
}
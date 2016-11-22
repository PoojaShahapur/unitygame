using System.Collections.Generic;

namespace SDK.Lib
{
    public struct MRayResult
    {
        public bool hit;
        public MTerrain terrain;
        public MVector3 position;

        public MRayResult(bool _hit, MTerrain _terrain, MVector3 _pos)
        {
            hit = _hit;
            terrain = _terrain;
            position = _pos;
        }
    };

    public class MTerrainGroup
    {
        protected Alignment mAlignment;
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

        public void setOrigin(MVector3 pos)
        {
            if (pos != mOrigin)
            {
                mOrigin = pos;
                foreach (KeyValuePair<int, MTerrainSlot> i in mTerrainSlots)
                {
                    MTerrainSlot slot = i.Value;
                    if (slot.instance != null)
                    {
                        slot.instance.setPosition(getTerrainSlotPosition(slot.x, slot.y));
                    }
                }
            }
        }

        public MVector3 getOrigin()
        {
            return mOrigin;
        }

        public Alignment getAlignment()
        {
            return mAlignment;
        }

        public float getTerrainWorldSize()
        {
            return mTerrainWorldSize;
        }

        public void setFilenameConvention(string prefix, string extension)
        {
            mFilenamePrefix = prefix;
            mFilenameExtension = extension;
        }

        public void setFilenamePrefix(string prefix)
        {
            mFilenamePrefix = prefix;
        }

        public void setFilenameExtension(string extension)
        {
            mFilenameExtension = extension;
        }

        public void defineTerrain(long x, long y, string terrainId)
        {
            MTerrainSlot slot = getTerrainSlot(x, y, true);
            slot.def.useImportData();
            slot.def.importData.assignFrom(mDefaultImportData);
            slot.def.importData.x = slot.x;
            slot.def.importData.y = slot.y;
            slot.def.importData.pos = new MVector3(x * mTerrainWorldSize, 0, y * mTerrainWorldSize);
            slot.def.importData.setTerrainId(terrainId);

            slot.def.importData.terrainSize = (ushort)mTerrainSize;
            slot.def.importData.worldSize = mTerrainWorldSize;
        }

        public void loadAllTerrains(bool synchronous)
        {
            foreach (KeyValuePair<int, MTerrainSlot> i in mTerrainSlots)
            {
                MTerrainSlot slot = i.Value;
                loadTerrainImpl(slot, synchronous);
            }
        }

        public void loadTerrain(long x, long y, bool synchronous /*= false*/)
        {
            MTerrainSlot slot = getTerrainSlot(x, y, false);
            if (slot != null)
            {
                loadTerrainImpl(slot, synchronous);
            }
        }

        public void loadTerrainImpl(MTerrainSlot slot, bool synchronous)
        {
            if (slot.instance == null && (!string.IsNullOrEmpty(slot.def.filename) || slot.def.importData != null))
            {
                slot.instance = new MTerrain(Ctx.mInstance.mSceneManager);
                if (!Ctx.mInstance.mTerrainGlobalOption.mIsReadFile)
                {
                    slot.instance.prepareOrig(slot.def.importData);
                }
                else
                {
                    slot.instance.prepareFile(slot.def.importData);
                }
            }
        }

        public void unloadTerrain(long x, long y)
        {
            MTerrainSlot slot = getTerrainSlot(x, y, false);
            if (slot != null)
            {
                slot.freeInstance();
            }
        }

        public void removeTerrain(long x, long y)
        {
            uint key = packIndex(x, y);
            if (mTerrainSlots.ContainsKey((int)key))
            {
                mTerrainSlots.Remove((int)key);
            }
        }

        public void removeAllTerrains()
        {
            foreach (KeyValuePair<int, MTerrainSlot> i in mTerrainSlots)
            {
                i.Value.instance.dispose();
            }
            mTerrainSlots.Clear();
        }

        MTerrainSlotDefinition getTerrainDefinition(long x, long y)
        {
            MTerrainSlot slot = getTerrainSlot(x, y);
            if (slot != null)
                return slot.def;
            else
                return null;
        }

        public MTerrain getTerrain(long x, long y)
        {
            MTerrainSlot slot = getTerrainSlot(x, y);
            if (slot != null)
                return slot.instance;
            else
                return null;
        }

        public float getHeightAtWorldPosition(float x, float y, float z, ref MTerrain ppTerrain)
        {
            return getHeightAtWorldPosition(new MVector3(x, y, z), ref ppTerrain);
        }

        public float getHeightAtWorldPosition(MVector3 pos, ref MTerrain ppTerrain)
        {
            long x = 0, y = 0;
            convertWorldPositionToTerrainSlot(pos, ref x, ref y);
            MTerrainSlot slot = getTerrainSlot(x, y);
            if (slot != null && slot.instance != null && slot.instance.isLoaded())
            {
                if (ppTerrain != null)
                    ppTerrain = slot.instance;
                return slot.instance.getHeightAtWorldPosition(ref pos);
            }
            else
            {
                if (ppTerrain != null)
                    ppTerrain = null;
                return 0;
            }
        }

        public MRayResult rayIntersects(MRay ray, float distanceLimit)
        {
            long curr_x = 0, curr_z = 0;

            convertWorldPositionToTerrainSlot(ray.getOrigin(), ref curr_x, ref curr_z);
            MTerrainSlot slot = getTerrainSlot(curr_x, curr_z);
            MRayResult result = new MRayResult(false, null, MVector3.ZERO);

            MVector3 tmp = new MVector3(0, 0, 0), localRayDir = new MVector3(0, 0, 0), centreOrigin = new MVector3(0, 0, 0), offset = new MVector3(0, 0, 0);

            convertTerrainSlotToWorldPosition(curr_x, curr_z, ref centreOrigin);
            offset = ray.getOrigin() - centreOrigin;
            localRayDir = ray.getDirection();
            switch (getAlignment())
            {
                case Alignment.ALIGN_X_Y:
                    UtilMath.swap(ref localRayDir.y, ref localRayDir.z);
                    UtilMath.swap(ref offset.y, ref offset.z);
                    break;
                case Alignment.ALIGN_Y_Z:
                    tmp.x = localRayDir.z;
                    tmp.z = localRayDir.y;
                    tmp.y = -localRayDir.x;
                    localRayDir = tmp;
                    tmp.x = offset.z;
                    tmp.z = offset.y;
                    tmp.y = -offset.x;
                    offset = tmp;
                    break;
                case Alignment.ALIGN_X_Z:
                    localRayDir.z = -localRayDir.z;
                    offset.z = -offset.z;
                    break;
            }

            offset /= mTerrainWorldSize;
            offset += 0.5f;

            MVector3 inc = new MVector3(UtilMath.Abs(localRayDir.x), UtilMath.Abs(localRayDir.y), UtilMath.Abs(localRayDir.z));
            long xdir = localRayDir.x > 0.0f ? 1 : -1;
            long zdir = localRayDir.z > 0.0f ? 1 : -1;

            if (xdir < 0)
                offset.x = 1.0f - offset.x;
            if (zdir < 0)
                offset.z = 1.0f - offset.z;

            bool keepSearching = true;
            int numGaps = 0;
            while (keepSearching)
            {
                if (UtilMath.RealEqual(inc.x, 0.0f) && UtilMath.RealEqual(inc.z, 0.0f))
                    keepSearching = false;

                while ((slot == null || slot.instance == null) && keepSearching)
                {
                    ++numGaps;
                    if (numGaps > 6)
                    {
                        keepSearching = false;
                        break;
                    }

                    MVector3 oldoffset = offset;
                    while (offset.x < 1.0f && offset.z < 1.0f)
                        offset += inc;
                    if (offset.x >= 1.0f && offset.z >= 1.0f)
                    {
                        float diffz = 1.0f - oldoffset.z;
                        float diffx = 1.0f - oldoffset.x;
                        float distz = diffz / inc.z;
                        float distx = diffx / inc.x;
                        if (distx < distz)
                        {
                            curr_x += xdir;
                            offset.x -= 1.0f;
                        }
                        else
                        {
                            curr_z += zdir;
                            offset.z -= 1.0f;
                        }
                    }
                    else if (offset.x >= 1.0f)
                    {
                        curr_x += xdir;
                        offset.x -= 1.0f;
                    }
                    else if (offset.z >= 1.0f)
                    {
                        curr_z += zdir;
                        offset.z -= 1.0f;
                    }
                    if (distanceLimit != 0)
                    {
                        MVector3 worldPos = new MVector3(0, 0, 0);

                        convertTerrainSlotToWorldPosition(curr_x, curr_z, ref worldPos);
                        if (ray.getOrigin().distance(ref worldPos) > distanceLimit)
                        {
                            keepSearching = false;
                            break;
                        }
                    }
                    slot = getTerrainSlot(curr_x, curr_z);
                }
                if (slot != null && slot.instance != null)
                {
                    numGaps = 0;
                    MKeyValuePair<bool, MVector3> raypair = slot.instance.rayIntersects(ray, false, distanceLimit);
                    if (raypair.Key)
                    {
                        keepSearching = false;
                        result.hit = true;
                        result.terrain = slot.instance;
                        result.position = raypair.Value;
                    }
                    else
                    {
                        slot = null;
                    }
                }
            }

            return result;
        }

        public void convertWorldPositionToTerrainSlot(MVector3 pos, ref long x, ref long y)
        {
            MVector3 terrainPos = new MVector3(0, 0, 0);

            MTerrain.convertWorldToTerrainAxes(mAlignment, pos - mOrigin, ref terrainPos);

            float offset = mTerrainWorldSize * 0.5f;
            terrainPos.x += offset;
            terrainPos.y += offset;

            x = (long)(UtilMath.floor(terrainPos.x / mTerrainWorldSize));

            y = (long)(UtilMath.floor(terrainPos.y / mTerrainWorldSize));
        }

        public void convertTerrainSlotToWorldPosition(long x, long y, ref MVector3 pos)
        {
            MVector3 terrainPos = new MVector3(x * mTerrainWorldSize, y * mTerrainWorldSize, 0);

            MTerrain.convertTerrainToWorldAxes(mAlignment, terrainPos, ref pos);

            pos += mOrigin;
        }

        public void connectNeighbour(MTerrainSlot slot, long offsetx, long offsety)
        {
            MTerrainSlot neighbourSlot = getTerrainSlot(slot.x + offsetx, slot.y + offsety);
            if (neighbourSlot != null && neighbourSlot.instance != null && neighbourSlot.instance.isLoaded())
            {
                slot.instance.setNeighbour(MTerrain.getNeighbourIndex(offsetx, offsety), neighbourSlot.instance,
                    slot.def.importData != null, true);
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

        public MVector3 getTerrainSlotPosition(long x, long y)
        {
            MVector3 pos = new MVector3(0, 0, 0);
            convertTerrainSlotToWorldPosition(x, y, ref pos);
            return pos;
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
            return null;
        }

        public MTerrainSlot getTerrainSlot(long x, long y)
        {
            int key = (int)packIndex(x, y);
            if (mTerrainSlots.ContainsKey(key))
                return mTerrainSlots[key];
            else
                return null;
        }

        public void update(bool synchronous)
        {
            foreach (KeyValuePair<int, MTerrainSlot> i in mTerrainSlots)
            {
                if (i.Value.instance != null)
                    i.Value.instance.update(true);
            }
        }

        public void updateGeometry()
        {
            foreach (KeyValuePair<int, MTerrainSlot> i in mTerrainSlots)
            {
                if (i.Value.instance != null)
                    i.Value.instance.updateGeometry();
            }
        }

        public void updateDerivedData(bool synchronous, byte typeMask)
        {
            foreach (KeyValuePair<int, MTerrainSlot> i in mTerrainSlots)
            {
                if (i.Value.instance != null)
                    i.Value.instance.updateDerivedData(true, 0);
            }
        }

        public void setTerrainWorldSize(float newWorldSize)
        {
            if (newWorldSize != mTerrainWorldSize)
            {
                mTerrainWorldSize = newWorldSize;
                foreach (KeyValuePair<int, MTerrainSlot> i in mTerrainSlots)
                {
                    if (i.Value.instance != null)
                    {
                        i.Value.instance.setWorldSize(newWorldSize);
                        i.Value.instance.setPosition(getTerrainSlotPosition(i.Value.x, i.Value.y));
                    }
                }
            }
        }

        public void setTerrainSize(ushort newTerrainSize)
        {
            if (newTerrainSize != mTerrainSize)
            {
                mTerrainSize = newTerrainSize;
                foreach (KeyValuePair<int, MTerrainSlot> i in mTerrainSlots)
                {
                    if (i.Value.instance != null)
                    {
                        i.Value.instance.setSize(newTerrainSize);
                    }
                }
            }
        }

        public void showTerrain(long x, long y)
        {
            MTerrainSlot terrain = getTerrainSlot(x, y, false);
            if (terrain.instance != null)
            {
                terrain.instance.showAllNode();
            }
        }

        public void cullTerrain(long x, long y, MFrustum frustum)
        {
            MTerrainSlot slot = getTerrainSlot(x, y, false);
            if (slot != null && slot.instance != null)
            {
                slot.instance.cullNode(frustum);
            }
        }

        public void updateAABB(long x, long y)
        {
            MTerrainSlot slot = getTerrainSlot(x, y, false);
            if (slot != null && slot.instance != null)
            {
                slot.instance.updateAABB();
            }
        }

        public void serializeTerrain(long x, long y)
        {
            MTerrainSlot slot = getTerrainSlot(x, y, false);
            if (slot != null && slot.instance != null)
            {
                slot.instance.serialize();
            }
        }
    }
}
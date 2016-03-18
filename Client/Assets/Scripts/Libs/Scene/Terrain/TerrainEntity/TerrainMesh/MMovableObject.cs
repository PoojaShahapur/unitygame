namespace SDK.Lib
{
    public class MMovableObject
    {
        public string mName;
        public MAxisAlignedBox mWorldAABB;

        public virtual string getName()
        {
            return mName;
        }

        public MAxisAlignedBox getWorldBoundingBox(bool derive)
        {
            return new MAxisAlignedBox(MAxisAlignedBox.Extent.EXTENT_FINITE);
        }
    }
}
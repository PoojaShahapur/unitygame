namespace SDK.Lib
{
    public class UniqueNumIdGen
    {
        protected uint mPreIdx;
        protected uint mCurId;

        public UniqueNumIdGen(uint baseUniqueId)
        {
            mCurId = 0;
        }

        public uint genNewId()
        {
            mPreIdx = mCurId;
            mCurId++;
            return mPreIdx;
        }
    }
}
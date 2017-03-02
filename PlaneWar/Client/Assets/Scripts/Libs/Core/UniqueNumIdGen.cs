namespace SDK.Lib
{
    public class UniqueNumIdGen
    {
        protected uint mPreIdx;
        protected uint mCurId;

        public UniqueNumIdGen(uint baseUniqueId)
        {
            this.mCurId = 0;
        }

        public uint genNewId()
        {
            this.mPreIdx = this.mCurId;
            this.mCurId++;
            return this.mPreIdx;
        }

        public uint getCurId()
        {
            return this.mCurId;
        }
    }
}
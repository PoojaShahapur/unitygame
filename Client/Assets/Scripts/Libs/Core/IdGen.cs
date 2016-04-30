namespace SDK.Lib
{
    public class IdGen
    {
        protected uint mCurrentId;

        public IdGen()
        {
            mCurrentId = 0;
        }

        public uint generateNewId()
        {
            return mCurrentId++;
        }
    }
}
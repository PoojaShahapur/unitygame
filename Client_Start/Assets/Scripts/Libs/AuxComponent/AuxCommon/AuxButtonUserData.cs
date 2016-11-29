namespace SDK.Lib
{
    public class AuxButtonUserData : AuxUserData
    {
        protected AuxButton mData;

        //public AuxButton getUserData()
        //{
        //    return this.getUserData<AuxButton>();
        //}

        //public AuxButton addUserData()
        //{
        //    if (mData == null)
        //    {
        //        mData = new AuxButton(this.gameObject);
        //    }

        //    return mData as AuxButton;
        //}

        public AuxButton getUserData()
        {
            return this.mData;
        }

        public AuxButton addUserData()
        {
            if (mData == null)
            {
                mData = new AuxButton(this.gameObject);
            }

            return mData as AuxButton;
        }
    }
}
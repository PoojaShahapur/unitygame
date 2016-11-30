namespace SDK.Lib
{
    /**
     * @brief SceneEntity 用户数据
     */
    public class AuxSceneEntityUserData : AuxUserData
    {
        private BeingEntity mUserData;

        public BeingEntity getUserData()
        {
            return mUserData;
        }

        public void setUserData(BeingEntity value)
        {
            mUserData = value;
        }
    }
}
namespace SDK.Lib
{
    /**
     * @brief SceneEntity 用户数据
     */
    public class AuxSceneEntityUserData : AuxUserData
    {
        private SceneEntityBase mUserData;

        public SceneEntityBase getUserData()
        {
            return mUserData;
        }

        public void setUserData(SceneEntityBase value)
        {
            mUserData = value;
        }
    }
}
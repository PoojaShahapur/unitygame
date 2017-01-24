namespace SDK.Lib
{
    public class ComputerBallRender : BeingEntityRender
    {
        public ComputerBallRender(SceneEntityBase entity_)
            : base(entity_)
        {

        }

        override public void onInit()
        {
            this.mResPath = "World/Model/PlayerOtherTest.prefab";
        }

        override protected void onSelfChanged()
        {
            base.onSelfChanged();

            AuxComputerBallUserData auxData = UtilApi.AddComponent<AuxComputerBallUserData>(this.selfGo);

            auxData.setUserData(this.mEntity);
        }
    }
}

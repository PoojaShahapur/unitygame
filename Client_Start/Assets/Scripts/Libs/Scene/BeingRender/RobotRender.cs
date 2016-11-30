namespace SDK.Lib
{
    public class RobotRender : BeingEntityRender
    {
        public RobotRender(SceneEntityBase entity_)
            : base(entity_)
        {

        }

        override public void onInit()
        {
            this.mResPath = "World/Model/FoodTest.prefab";
        }
    }
}

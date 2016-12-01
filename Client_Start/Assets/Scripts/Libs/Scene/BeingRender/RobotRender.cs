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

        override protected void onSelfChanged()
        {
            base.onSelfChanged();

            UnityEngine.GameObject sphere = UtilApi.TransFindChildByPObjAndPath(this.selfGo, "Sphere");
            AuxRobotUserData auxData = sphere.GetComponent<AuxRobotUserData>();
            if (null == auxData)
            {
                auxData = sphere.AddComponent<AuxRobotUserData>();
            }
            auxData.setUserData(this.mEntity);
        }
    }
}

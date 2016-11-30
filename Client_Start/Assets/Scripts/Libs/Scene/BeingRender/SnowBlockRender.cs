﻿namespace SDK.Lib
{
    public class SnowBlockRender : BeingEntityRender
    {
        public SnowBlockRender(SceneEntityBase entity_)
            : base(entity_)
        {

        }

        override public void onInit()
        {
            this.mResPath = "World/Model/SnowBlockTest.prefab";
        }

        override protected void onSelfChanged()
        {
            base.onSelfChanged();

            AuxSnowBlockUserData auxData = this.selfGo.AddComponent<AuxSnowBlockUserData>();
            auxData.setUserData(this.mEntity);
        }
    }
}

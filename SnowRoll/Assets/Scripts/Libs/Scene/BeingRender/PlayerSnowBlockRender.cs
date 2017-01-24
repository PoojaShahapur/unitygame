﻿namespace SDK.Lib
{
    public class PlayerSnowBlockRender : BeingEntityRender
    {
        public PlayerSnowBlockRender(SceneEntityBase entity_)
            : base(entity_)
        {

        }

        override public void onInit()
        {
            this.mResPath = "World/Model/SnowBlockTest1.prefab";
        }

        override protected void onSelfChanged()
        {
            base.onSelfChanged();

            //GameObject collide = UtilApi.TransFindChildByPObjAndPath(this.selfGo, UtilApi.COLLIDE_NAME);
            //AuxPlayerSnowBlockUserData auxData = UtilApi.AddComponent<AuxPlayerSnowBlockUserData>(collide);
            AuxPlayerSnowBlockUserData auxData = UtilApi.AddComponent<AuxPlayerSnowBlockUserData>(this.selfGo);

            auxData.setUserData(this.mEntity);

            //UtilApi.setLayer(this.selfGo, "PlayerSnowBlock");
        }

        override public void updateLocalTransform()
        {
            if (null != this.mSelfGo)
            {
                // 雪块只更新位置和旋转，不更新缩放
                if (this.mIsPosDirty)
                {
                    this.mIsPosDirty = false;
                    UtilApi.setPos(this.mSelfGo.transform, this.mEntity.getPos());
                }
                if (this.mIsRotDirty)
                {
                    this.mIsRotDirty = false;
                    UtilApi.setRot(this.mSelfGo.transform, this.mEntity.getRotate());
                }
            }
        }
    }
}

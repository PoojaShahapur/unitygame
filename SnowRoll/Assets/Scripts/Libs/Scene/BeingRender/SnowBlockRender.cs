using UnityEngine;

namespace SDK.Lib
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

            //GameObject collide = UtilApi.TransFindChildByPObjAndPath(this.selfGo, UtilApi.COLLIDE_NAME);
            //AuxSnowBlockUserData auxData = UtilApi.AddComponent<AuxSnowBlockUserData>(collide);
            AuxSnowBlockUserData auxData = UtilApi.AddComponent<AuxSnowBlockUserData>(this.selfGo);

            auxData.setUserData(this.mEntity);

            UtilApi.setLayer(this.selfGo, "SnowBlock");
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

using UnityEngine;

namespace SDK.Lib
{
    public class AuxPlayerMainUserData : AuxPlayerUserData
    {
        void OnTriggerEnter(Collider other)
        {
            AuxSceneEntityUserData aUserData = this.GetComponent<AuxSceneEntityUserData>();
            if (null != aUserData)
            {
                Player aBeingEntity = aUserData.getUserData() as Player;

                AuxSceneEntityUserData bUserData = other.gameObject.GetComponent<AuxSceneEntityUserData>();
                if (null != bUserData)
                {
                    BeingEntity bBeingEntity = bUserData.getUserData() as BeingEntity;
                    if (null != bBeingEntity)
                    {
                        if (!Ctx.mInstance.mFrameCollideMgr.isOrAddCollidedInCurFrame(aBeingEntity.getEntityUniqueId(), bBeingEntity.getEntityUniqueId()))
                        {
                            aBeingEntity.overlapTo(bBeingEntity);
                        }
                    }
                }
                else
                {
                    // 如果碰撞的是地形
                    //aBeingEntity.m_isOnGround = true;
                }
            }
        }

        void OnCollisionStay(Collision collision)
        {
            AuxSceneEntityUserData aUserData = this.GetComponent<AuxSceneEntityUserData>();
            Player aBeingEntity = aUserData.getUserData() as Player;

            AuxSceneEntityUserData bUserData = collision.gameObject.GetComponent<AuxSceneEntityUserData>();
            if (null == bUserData)
            {
                //aBeingEntity.m_isOnGround = true;
            }
        }

        void OnCollisionExit(Collision collision)
        {
            AuxSceneEntityUserData aUserData = this.GetComponent<AuxSceneEntityUserData>();
            Player aBeingEntity = aUserData.getUserData() as Player;

            AuxSceneEntityUserData bUserData = collision.gameObject.GetComponent<AuxSceneEntityUserData>();
            if (null == bUserData)   // 地形暂时没有绑定组件
            {
                //aBeingEntity.m_isOnGround = false;
            }
        }
    }
}
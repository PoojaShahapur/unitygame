using UnityEngine;

namespace SDK.Lib
{
    class AuxRobotUserData : AuxPlayerUserData
    {
        void OnTriggerEnter(Collider other)
        {
            
        }

        void OnCollisionStay(Collision collision)
        {
            AuxSceneEntityUserData aUserData = this.GetComponent<AuxSceneEntityUserData>();
            Player aBeingEntity = aUserData.getUserData() as Player;

            AuxSceneEntityUserData bUserData = collision.gameObject.GetComponent<AuxSceneEntityUserData>();
            if (null == bUserData)
            {
                aBeingEntity.m_isOnGround = true;
            }
        }

        void OnCollisionExit(Collision collision)
        {
            AuxSceneEntityUserData aUserData = this.GetComponent<AuxSceneEntityUserData>();
            Player aBeingEntity = aUserData.getUserData() as Player;

            AuxSceneEntityUserData bUserData = collision.gameObject.GetComponent<AuxSceneEntityUserData>();
            if (null == bUserData)   // 地形暂时没有绑定组件
            {
                aBeingEntity.m_isOnGround = false;
            }
        }
    }
}
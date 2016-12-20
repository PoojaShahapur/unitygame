using UnityEngine;

namespace SDK.Lib
{
    class AuxRobotUserData : AuxPlayerUserData
    {
        void OnCollisionEnter(UnityEngine.Collision collisionInfo)
        {
            
        }

        void OnCollisionStay(Collision collisionInfo)
        {
            AuxSceneEntityUserData aUserData = this.GetComponent<AuxSceneEntityUserData>();
            Player aBeingEntity = aUserData.getUserData() as Player;

            AuxSceneEntityUserData bUserData = collisionInfo.gameObject.GetComponent<AuxSceneEntityUserData>();
            if (null == bUserData)
            {
                //aBeingEntity.m_isOnGround = true;
            }
        }

        void OnCollisionExit(Collision collisionInfo)
        {
            AuxSceneEntityUserData aUserData = this.GetComponent<AuxSceneEntityUserData>();
            Player aBeingEntity = aUserData.getUserData() as Player;

            AuxSceneEntityUserData bUserData = collisionInfo.gameObject.GetComponent<AuxSceneEntityUserData>();
            if (null == bUserData)   // 地形暂时没有绑定组件
            {
                //aBeingEntity.m_isOnGround = false;
            }
        }
    }
}
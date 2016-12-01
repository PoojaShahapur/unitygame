﻿using UnityEngine;

namespace SDK.Lib
{
    public class PlayerMainRender : PlayerRender
    {
        private GameObject player1;//该子物体用于添加表现雪球旋转
        private GameObject cmr;

        public PlayerMainRender(SceneEntityBase entity_)
            : base(entity_)
        {

        }

        override public void onInit()
        {
            this.mResPath = "World/Model/PlayerTest.prefab";
        }

        override protected void onSelfChanged()
        {
            base.onSelfChanged();

            AuxPlayerMainUserData auxData = this.selfGo.GetComponent<AuxPlayerMainUserData>();
            if(null == auxData)
            {
                auxData = this.selfGo.AddComponent<AuxPlayerMainUserData>();
            }
            auxData.setUserData(this.mEntity);

            player1 = this.transform().FindChild("Player1").gameObject;
            cmr = GameObject.FindGameObjectWithTag("MainCamera").gameObject;

            Ctx.mInstance.mCamSys.setCameraActor(this.selfGo);
        }

        public void updatePlayer1Pos()
        {
            player1.transform.rotation = Quaternion.Euler(0, (Mathf.Atan2(-(mEntity as PlayerMain).vertical_move, (mEntity as PlayerMain).horizontal_move) * Mathf.Rad2Deg) + cmr.transform.rotation.eulerAngles.y + 90, 0);
        }
    }
}
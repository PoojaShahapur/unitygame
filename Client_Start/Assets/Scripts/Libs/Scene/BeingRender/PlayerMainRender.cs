using UnityEngine;

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
            this.mResPath = "World/Model/Player";
        }

        override protected void onSelfChanged()
        {
            base.onSelfChanged();

            player1 = this.transform().FindChild("Player1").gameObject;
            cmr = GameObject.FindGameObjectWithTag("MainCamera").gameObject;
        }

        public void updatePlayer1Pos()
        {
            player1.transform.rotation = Quaternion.Euler(0, (Mathf.Atan2(-(mEntity as PlayerMain).vertical_move, (mEntity as PlayerMain).horizontal_move) * Mathf.Rad2Deg) + cmr.transform.rotation.eulerAngles.y + 90, 0);
        }
    }
}
using UnityEngine;
using System.Collections;
using SDK.Common;
using Game.Msg;

namespace SDK.Lib
{
    /// <summary>
    /// 所有界面按钮的行为
    /// </summary>
    public class btn : InterActiveEntity
    {
        public bool Disable = false;
        GameObject mlight;

        // Use this for initialization
        public override void Start()
        {
            Transform t = transform.FindChild("light");
            if (t != null)
            {
                mlight = t.gameObject;
            }

            if (Disable)
            {
                return;
            }
        }

        void OnMouseEnter()
        {
            if (Disable)
            {
                return;
            }
            if (mlight != null)
            {
                mlight.SetActive(true);
            }

            if (animation != null)
            {
                animation.Play("btnanim");
            }
        }

        void OnMouseExit()
        {
            if (Disable)
            {
                return;
            }
            if (mlight != null)
            {
                mlight.SetActive(false);
            }
        }

        //向上一级发送
        public bool sendtoparent = false;
        public override void OnMouseUpAsButton()
        {
            if (Disable)
            {
                return;
            }

            //if (EntityTag.eETagShop == m_tag)
            //{
            //    // 发送消息
            //    stReqMarketObjectInfoPropertyUserCmd cmd = new stReqMarketObjectInfoPropertyUserCmd();
            //    UtilMsg.sendMsg(cmd);

            //    // 显示内容
            //    (Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("shop") as shop).show();
            //}
            //else if (EntityTag.eETagExtPack == m_tag)
            //{
            //    (Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("open") as open).show();
            //}
            //else if (EntityTag.eETaggoback == m_tag)
            //{
            //    (Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("open") as open).goback();
            //}
            //else if(EntityTag.eETagwdscbtn == m_tag)
            //{
            //    (Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("wdscjm") as wdscjm).show();
            //}
            //else if (EntityTag.eETagdzmoshibtn == m_tag)
            //{
            //    (Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("moshijm") as moshijm).dzmoshi();
            //}
            //else if(EntityTag.eETaggoldbuy == m_tag)
            //{
            //    stReqBuyMobileObjectPropertyUserCmd cmd = new stReqBuyMobileObjectPropertyUserCmd();
            //    cmd.index = 1;
            //    UtilMsg.sendMsg(cmd);
            //}
        }

        void OnClick()
        {
            Debug.Log("空按钮函数");
        }
    }
}
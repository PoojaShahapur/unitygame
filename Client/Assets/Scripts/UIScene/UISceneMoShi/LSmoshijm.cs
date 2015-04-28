using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SDK.Common;
using Game.UI;
using SDK.Lib;
using Game.Msg;

namespace Game.UI
{
    enum moshijmmethod
    {
        knewset,        // 建立卡牌模式
        dz,             // 对战模式
        lx,             // 冒险
        jj              // 竞技
    }

    /// <summary>
    /// 模式界面主控制
    /// </summary>
    public class moshijm : InterActiveEntity
    {
        moshijmmethod nowMethod;
        Text bt, tpm, nameText;
        Vector3 goodp = new Vector3(-0.6957181f, 2.428539f, 0.5254142f);
        // Use this for initialization
        public override void Start()
        {
            bt = transform.FindChild("bt").GetComponent<Text>();
            tpm = transform.FindChild("tpm").GetComponent<Text>();
            nameText = transform.FindChild("name").GetComponent<Text>();

            chooseclassname = UtilApi.getComByP<Text>(UtilApi.GoFindChildByPObjAndName("moshijm/xuanname"));
            classpic = UtilApi.TransFindChildByPObjAndPath(UtilApi.GoFindChildByPObjAndName("moshijm"), "yxkuan/classpic");
            jbtpbtn = UtilApi.GoFindChildByPObjAndName("moshijm/jbtpbtn");
            zdytpbtn = UtilApi.GoFindChildByPObjAndName("moshijm/zdytpbtn");
            //newsetbtn = UtilApi.GoFindChildByPObjAndName("wdscjm/setbtn/newSetBtn");
        }

        #region 从英雄点击发送而来
        EnPlayerCareer chooseClass;
        public void setclass(EnPlayerCareer c)
        {
            chooseClass = c;
        }

        public EnPlayerCareer getClass()
        {
            return chooseClass;
        }

        public Text chooseclassname;
        public void setClassname(string n)
        {
            //chooseclassname.text = n;
        }
        public GameObject classpic;
        public void setclasspic(Material pic)
        {
            classpic.transform.parent.gameObject.SetActive(true);
#if UNITY_5
            classpic.GetComponent<Renderer>().material = pic;
#elif UNITY_4_6
            classpic.renderer.material = pic;
#endif
        }
        #endregion

        void Onjbtp()
        {
            tpm.GetComponent<Text>().text = "基本套牌";

#if UNITY_5
            transform.GetComponent<Animation>()["jbtpshow"].speed = 1;
            transform.GetComponent<Animation>().Play("jbtpshow");
#elif UNITY_4_6
            transform.animation["jbtpshow"].speed = 1;
            transform.animation.Play("jbtpshow");
#endif
        }

        void Onzdytp()
        {
            tpm.GetComponent<Text>().text = "自定义套牌";

#if UNITY_5
            transform.GetComponent<Animation>()["jbtpshow"].speed = -1;
            transform.GetComponent<Animation>()["jbtpshow"].time = transform.GetComponent<Animation>()["jbtpshow"].length;
            transform.GetComponent<Animation>().Play("jbtpshow");
#elif UNITY_4_6
            transform.animation["jbtpshow"].speed = -1;
            transform.animation["jbtpshow"].time = transform.animation["jbtpshow"].length;
            transform.animation.Play("jbtpshow");
#endif
        }

        void nochoose()
        {
            //chooseclassname.text = "";
            classpic.transform.parent.gameObject.SetActive(false);
        }

        // 新建立一张卡牌
        Vector3 jbtppostion = new Vector3(-1.037413f, -0.004264796f, 0.002689363f);
        public GameObject jbtpbtn, zdytpbtn;
        public void newset()
        {
            transform.FindChild("jbtp").gameObject.SetActive(true);

            jbtpbtn.transform.Rotate(180, 0, 0);
            jbtpbtn.transform.Rotate(0, 0, 0);
            transform.FindChild("jbtp").localPosition = jbtppostion;
            iTween.MoveTo(gameObject, goodp, 1);
            nowMethod = moshijmmethod.knewset;
            nochoose();
            //bt.text = "选择英雄";
            //tpm.text = "";
            //nameText.text = "新建套牌";
        }

        public void dzmoshi()
        {
            transform.FindChild("jbtp").gameObject.SetActive(false);

            jbtpbtn.transform.Rotate(0, 0, 0);
            //jbtpbtn.transform.Rotate(0, 0, 0);
            transform.position = goodp;
            nowMethod = moshijmmethod.dz;
            //Camera.main.SendMessage("push");
            //(Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("mcam") as boxcam).push();
            Ctx.m_instance.m_camSys.m_boxcam.push();

            nochoose();
            //bt.text = "选择套牌";
            //tpm.text = "自定义套牌";
            //name.text = "对战模式";
            getPlayerSet();
        }

        void lxmoshi()
        {
            transform.FindChild("jbtp").gameObject.SetActive(false);

            jbtpbtn.transform.Rotate(0, 0, 0);
            //jbtpbtn.transform.Rotate(0, 0, 0);
            transform.position = goodp;
            nowMethod = moshijmmethod.lx;
            //Camera.main.SendMessage("push");
            //(Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("mcam") as boxcam).push();
            Ctx.m_instance.m_camSys.m_boxcam.push();

            nochoose();
            //bt.text = "选择套牌";
            //tpm.text = "自定义套牌";
            //nameText.text = "练习模式";
            getPlayerSet();
        }

        //List<set> sets = new List<set>();
        List<CardGroupItem> sets = null;

        public void getPlayerSet()
        {
            //sets = web.getSets();
            sets = Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardGroupListArr;
            Ctx.m_instance.m_logSys.log(string.Format("对战模式界面显示卡组列表信息，数量 {0}", sets.Count));
            //先让卡组不显示
            for (int x = 1; x < 10; x++)
            {
                Transform t = transform.FindChild(x.ToString());
                t.gameObject.SetActive(false);
            }

            //让卡组模型显示
            int p = 1;
            //foreach (set s in sets)
            foreach (CardGroupItem s in sets)
            {
                Transform t = transform.FindChild(p.ToString());
                t.gameObject.SetActive(true);
                //t.SendMessage("setinfo", s);
                (Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneMoShi) as UISceneMoShi).m_cardGroupList[p - 1].setinfo(s);
                p++;
            }
        }

        void jjmoshi()
        {
            nochoose();
            //bt.text = "选择套牌";
            //tpm.text = "自定义套牌";
            //nameText.text = "竞技模式";
        }

        public void gotoback()
        {
            switch (nowMethod)
            {
                case moshijmmethod.knewset:
                    {
                        iTween.MoveBy(gameObject, Vector3.left * 15f, 1);
                    }
                    break;
                default:
                    {
                        //Camera.main.SendMessage("back");
                        //iTween.MoveBy(gameObject, iTween.Hash(iT.MoveBy.amount, Vector3.down * 10, iT.MoveBy.time, 0.1f, iT.MoveBy.delay, 1));
                        //(Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("mcam") as boxcam).back();
                        Ctx.m_instance.m_camSys.m_boxcam.back();
                        iTween.MoveBy(gameObject, iTween.Hash(iT.MoveBy.amount, Vector3.down * 10, iT.MoveBy.time, 0.1f, iT.MoveBy.delay, 1));
                    }
                    break;

            }
        }

        //public GameObject newsetbtn;
        public void chooseok()
        {
            switch (nowMethod)
            {
                case moshijmmethod.knewset:
                    {
                        iTween.MoveBy(gameObject, Vector3.left * 15f, 1);
                        //newsetbtn.SendMessage("newcardset", chooseClass);//向newsetbtn发送
                        // 等待服务器返回消息，才能建立
                        //CardGroupItem group = new CardGroupItem();
                        //group.m_cardGroup = new t_group_list();
                        //group.m_cardGroup.index = 1;
                        //group.m_cardGroup.occupation = 1;
                        //group.m_cardGroup.name = "nihao";
                        //IUISceneWDSC uiSC = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as IUISceneWDSC;
                        //if (uiSC != null)
                        //{
                        //    uiSC.newcardset(group);
                        //}
                        stReqCreateOneCardGroupUserCmd cmd = new stReqCreateOneCardGroupUserCmd();
                        cmd.occupation = (uint)getClass();
                        UtilMsg.sendMsg(cmd);
                    }
                    break;
                case moshijmmethod.lx:
                    break;
                case moshijmmethod.dz:
                    UISceneMoShi uiMS = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneMoShi) as UISceneMoShi;
                    if (uiMS.m_curSel != null)      // 如果有选中
                    {
                        if (Ctx.m_instance.m_dataPlayer.m_dzData.m_canReqDZ)
                        {
                            Ctx.m_instance.m_dataPlayer.m_dzData.m_canReqDZ = false;
                            Ctx.m_instance.m_dataPlayer.m_dzData.setSelfHeroInfo(uiMS.m_curSel.info);

                            stReqHeroFightMatchUserCmd cmd = new stReqHeroFightMatchUserCmd();
                            cmd.index = uiMS.m_curSel.info.m_cardGroup.index;
                            UtilMsg.sendMsg(cmd);

                            //开始查找
                            uiMS.startmatch();
                        }
                    }
                    // test
                #if DEBUG_NOTNET
                    uiMS.startmatch();
                #endif
                    break;
            }
        }
    }
}
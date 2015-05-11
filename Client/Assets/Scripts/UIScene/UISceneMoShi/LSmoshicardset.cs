using UnityEngine;
using System.Collections;
using SDK.Lib;
using SDK.Common;

namespace Game.UI
{
    /// <summary>
    /// 模式里用的卡组
    /// </summary>
    public class moshicardset : InterActiveEntity
    {
        //职业图片
        //public Material zs, sm, dz, sq, lr, dly, ss, fs, ms;
        //职业logo
        //public Material lzs, lsm, ldz, lsq, llr, ldly, lss, lfs, lms;

        //set info;
        public CardGroupItem info;

        public override void Start()
        {
            // 职业图片
            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "skin/classzs", "classzs");
            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "skin/classdz", "classdz");
            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "skin/classdly", "classdly");

            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "skin/classlr", "classlr");
            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "skin/classms", "classms");
            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "skin/classsq", "classsq");

            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "skin/classsm", "classsm");
            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "skin/classfs", "classfs");
            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "skin/classss", "classss");

            //// 职业 lOGO
            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "zs", "zs");
            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "dz", "dz");
            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "dly", "dly");

            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "lr", "lr");
            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "ms", "ms");
            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "sq", "sq");

            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "sm", "sm");
            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "fs", "fs");
            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "ss", "ss");

            UtilApi.addEventHandle(gameObject, onMouseUp);
        }

        //void setinfo(set i)
        public void setinfo(CardGroupItem i)
        {
            Transform clogo = transform.FindChild("classlogo");
            Transform cpic = transform.FindChild("cardsetdaiqi").FindChild("pic");
            //UILabel namelabel = cpic.transform.parent.FindChild("name").GetComponent<UILabel>();
            //namelabel.text = i.name;

            info = i;
            //switch (info.m_cardGroup.occupation)
            //{
            //    case (uint)EnPlayerCareer.HERO_OCCUPATION_1:
            //        cpic.renderer.material = dly;
            //        clogo.renderer.material = ldly;
            //        break;

            //    case (uint)EnPlayerCareer.HERO_OCCUPATION_2:
            //        cpic.renderer.material = lr;
            //        clogo.renderer.material = llr;
            //        break;

            //    case (uint)EnPlayerCareer.HERO_OCCUPATION_3:
            //        cpic.renderer.material = fs;
            //        clogo.renderer.material = lfs;
            //        break;

            //    //case CardClass.kpaladin:
            //    //    cpic.renderer.material = sq;
            //    //    clogo.renderer.material = lsq;
            //    //    break;

            //    //case CardClass.kpriest:
            //    //    cpic.renderer.material = ms;
            //    //    clogo.renderer.material = lms;
            //    //    break;

            //    //case CardClass.krogue:
            //    //    cpic.renderer.material = dz;
            //    //    clogo.renderer.material = ldz;
            //    //    break;

            //    //case CardClass.kshama:
            //    //    cpic.renderer.material = sm;
            //    //    clogo.renderer.material = lsm;
            //    //    break;

            //    //case CardClass.kwarlock:
            //    //    cpic.renderer.material = zs;
            //    //    clogo.renderer.material = lzs;
            //    //    break;

            //    //case CardClass.kwarrior:
            //    //    cpic.renderer.material = ss;
            //    //    clogo.renderer.material = lss;
            //    //    break;
            //}

#if UNITY_5
            cpic.GetComponent<Renderer>().material = Ctx.m_instance.m_matMgr.getCardGroupMatByOccup((EnPlayerCareer)info.m_cardGroup.occupation).m_mat;
            clogo.GetComponent<Renderer>().material = Ctx.m_instance.m_matMgr.getCardGroupLOGOMatByOccup((EnPlayerCareer)info.m_cardGroup.occupation).m_mat;
#elif UNITY_4_6
            cpic.renderer.material = Ctx.m_instance.m_matMgr.getCardGroupMatByOccup((EnPlayerCareer)info.m_cardGroup.occupation).m_mat;
            clogo.renderer.material = Ctx.m_instance.m_matMgr.getCardGroupLOGOMatByOccup((EnPlayerCareer)info.m_cardGroup.occupation).m_mat;
#endif
        }

        protected void onMouseUp(GameObject go)
        {
            //Transform clogo = transform.FindChild("classlogo");
            //Transform cpic = transform.FindChild("cardsetdaiqi").FindChild("pic");
            OnMouseUpAsButton();
        }

        //public static set nowchoose;
        public static CardGroupItem nowchoose;
        public void OnMouseUpAsButton()
        {
#if UNITY_5
            Material classpic = transform.FindChild("cardsetdaiqi").FindChild("pic").GetComponent<Renderer>().material;
#elif UNITY_4_6
            Material classpic = transform.FindChild("cardsetdaiqi").FindChild("pic").renderer.material;
#endif
            //transform.root.SendMessage("setclass", info.classs);
            //transform.root.SendMessage("setclasspic", classpic);
            //transform.root.SendMessage("setClassname", info.name);

            UISceneMoShi uiMS = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneMoShi>(UISceneFormID.eUISceneMoShi);
            if (uiMS != null)
            {
                uiMS.setclass((EnPlayerCareer)info.m_cardGroup.occupation);
                uiMS.setclasspic(classpic);
                uiMS.setClassname(info.m_cardGroup.name);
                uiMS.setCardGroup(this);
            }

            nowchoose = info;
        }
    }
}
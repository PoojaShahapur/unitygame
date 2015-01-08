using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using SDK.Common;
using SDK.Lib;

namespace Game.UI
{
    /// <summary>
    /// 卡牌的结构体
    /// </summary>
    public struct card
    {
        public Material image;
        public string cardid;
        public string cnname;
        public string name;
        public string cndescription;
        public string description;
        public CardQuality quality;
        public CardSet set;
        public CardType type;
        public CardRace race;
        public CardClass classs;
        public string attack;
        public string health;
        public string cost;
        /// <summary>
        /// 有几张
        /// </summary>
        public int count;
        public int insetcount;
    }

    //职业
    public enum CardClass
    {
        kany,
        kwarrior = 1, //ZZ
        kpaladin = 2,//SQ
        khunter = 3, //LR
        krogue = 4,//SQ
        kpriest = 5,//MS
        kshama = 7,//SM
        kmage = 8,//DZ
        kwarlock = 9,//SS
        kdruid = 11,//DLY
    };

    public enum CardType
    {
        kability = 5,//法术卡
        khero = 3,//英雄卡
        kheroPower = 10,//英雄技能卡
        kminion = 4,//仆从卡
        kweapon = 7//武器卡
    };

    public enum CardRace
    {
        knone,
        kbeast = 20,//野兽
        kdemon = 15,//妖
        kdragon = 24,//龙
        kmurloc = 14,//鱼人
        kpirate = 23,//海盗
        ktotem = 21//图腾

    };

    public enum CardQuality
    {
        kfree,//免费
        kcommon,//常见
        krare = 3,//罕见
        kepic,//史诗
        klegendary//传说
    };
    public enum CardSet
    {
        kbasic = 2,//基本
        kexpert,//专家
        kreward,//奖励
        kmissions//任务
    };

    /// <summary>
    /// 我的收藏行为
    /// </summary>
    public class wdscjm : InterActiveEntity
    {
        public static wdscmMod nowMod;

        Vector3 showpostion = new Vector3(-1.532529f, 1.805149f, 0.860475f);

        public override void Start()
        {

        }

        public void editset()
        {
            //让返回变完成
            //可加入卡牌
            nowMod = wdscmMod.editset;
        }

        void endeditset()
        {
            // 先保存，然后退回
            IUISceneWDSC uiSC = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as IUISceneWDSC;
            uiSC.reqSaveCard();

            (Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_curEditCardSet.hide();

            //cardset.nowEditingSet.transform.parent.BroadcastMessage("goback");
            uiSC.newCardSet_goback();
            uiSC.cardset_goback();
            //职业标签 回来
            //transform.root.FindChild("classfilter").BroadcastMessage("gotoback");
            uiSC.classfilter_gotoback();

            //不可加入卡牌
            nowMod = wdscmMod.look;
        }

        public void back()
        {
            switch (nowMod)
            {
                case wdscmMod.editset: endeditset();
                    break;
                case wdscmMod.look:
                    //Camera.main.SendMessage("back");
                    (Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("mcam") as boxcam).back();
                    iTween.MoveBy(gameObject, iTween.Hash(iT.MoveBy.amount, Vector3.down * 10, iT.MoveBy.time, 0.1f, iT.MoveBy.delay, 1));
                    break;
            }
        }

        public void show()
        {
            transform.position = showpostion;
            //Camera.main.SendMessage("push");
            (Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("mcam") as boxcam).push();
        }
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SDK.Common;
using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief 添加卡牌组按钮处理事件
     */
    public class newCardSet : InterActiveEntity
    {
        //public Transform cardsetpre;
        //public static List<Transform> playersets = new List<Transform>();       // 卡牌 Tranforms 以及最后的按钮
        //public static List<cardset> m_taoPaiEntityList = new List<cardset>();

        public override void Start()
        {
            // 加载资源
            //load();
            //得到用户的卡组
            //List<set> playset = web.getSets();
            //foreach (set s in playset)
            //{
            //    addset(s);
            //}
            // 加入已经有的卡牌
            foreach(CardGroupItem groupItem in Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardGroupListArr)
            {
                addset(groupItem);
            }
            // 加入最后的按钮
            //(Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_playersets.Add(transform);
            //(Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_leftCardGroupList.AddChild(transform);
        }

        //选职业
        public void OnMouseUpAsButton()
        {

        }

        //拉取用户自定义时用来加入按钮的
        //void addset(set s)
        void addset(CardGroupItem s)
        {
            //Transform g = (Transform)UtilApi.Instantiate(cardsetpre, transform.position, transform.rotation);
            Transform g = (UtilApi.Instantiate(Ctx.m_instance.m_modelMgr.getCardGroupModel().getObject(), transform.position, transform.rotation) as GameObject).transform;
            g.parent = transform.parent;
            //g.SendMessage("setinfo", s);
            cardset taopai = new cardset();
            Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneWDSC>(UISceneFormID.eUISceneWDSC).m_taoPaiEntityList.Add(taopai);
            taopai.setGameObject(g.gameObject);
            taopai.setinfo(s);
            //(Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_playersets.Add(g);
            Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneWDSC>(UISceneFormID.eUISceneWDSC).m_leftCardGroupList.AddChild(g);
            transform.Translate(new Vector3(0, 0, -0.525f));
        }

        Vector3 lastpostion;
        public void hide()
        {
            lastpostion = transform.localPosition;
            transform.Translate(new Vector3(0, 0, -20f));
        }

        public void goback()
        {
            transform.localPosition = lastpostion;
        }

        // 新建卡牌组， bEnterEdit 是否立即进入编辑模式
        public void newcardset(CardGroupItem s, bool bEnterEdit = true)
        {
            //Transform g = (Transform)UtilApi.Instantiate(cardsetpre, transform.position, transform.rotation);
            //g.parent = transform.parent;
            //(Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_playersets.Insert((Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_playersets.Count - 1, g);//插入到最后一位
            //transform.Translate(new Vector3(0, 0, -0.525f));

            //Transform g = (Transform)UtilApi.Instantiate(cardsetpre, transform.position, transform.rotation);
            Transform g = (UtilApi.Instantiate(Ctx.m_instance.m_modelMgr.getCardGroupModel().getObject(), transform.position, transform.rotation) as GameObject).transform;
            //g.localPosition = Vector3.zero;
            Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneWDSC>(UISceneFormID.eUISceneWDSC).m_leftCardGroupList.AddChild(g);//插入到最后一位
            updatePos();

            //set s = new set();
            //s.id = -1;
            //s.classs = c;

            //CardGroupItem s = new CardGroupItem();
            //s.id = -1;
            //s.classs = c;

            cardset taopai = new cardset();
            Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneWDSC>(UISceneFormID.eUISceneWDSC).m_taoPaiEntityList.Add(taopai);
            taopai.setGameObject(g.gameObject);
            taopai.Createnew(s, bEnterEdit);
            //g.SendMessage("Createnew", s);
        }

        // 更新新建按钮位置
        public void updatePos()
        {
            transform.localPosition = new Vector3(0, 0, -0.525f * Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneWDSC>(UISceneFormID.eUISceneWDSC).m_leftCardGroupList.getChildCount());
        }

        //public void addOneCardSet(cardset one)
        //{
        //    (Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_playersets.Insert((Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_playersets.Count - 1, one.getGameObject().transform);//插入到最后一位
        //    transform.Translate(new Vector3(0, 0, -0.525f));

        //    (Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_taoPaiEntityList.Add(one);
        //}

        public void hideAllCard()
        {
            foreach (cardset item in Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneWDSC>(UISceneFormID.eUISceneWDSC).m_taoPaiEntityList)
            {
                item.hide();
            }
        }

        public void insEditCardGroup()
        {
            //Transform trans = (Transform)UtilApi.Instantiate(cardsetpre, transform.position, transform.rotation);
            Transform trans = (UtilApi.Instantiate(Ctx.m_instance.m_modelMgr.getCardGroupModel().getObject(), transform.position, transform.rotation) as GameObject).transform;
            trans.parent = transform.parent;

            Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneWDSC>(UISceneFormID.eUISceneWDSC).m_curEditCardSet = new cardset();
            Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneWDSC>(UISceneFormID.eUISceneWDSC).m_curEditCardSet.setGameObject(trans.gameObject);
            //(Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_curEditCardSet.Editing = true;
            Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneWDSC>(UISceneFormID.eUISceneWDSC).m_curEditCardSet.hide();
        }
    }
}
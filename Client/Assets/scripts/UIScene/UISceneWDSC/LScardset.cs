using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SDK.Common;
using Game.UI;
using Game.Msg;

namespace SDK.Lib
{
    public class cardset : InterActiveEntity
    {
        //set info;
        public CardGroupItem info;             // 当前卡牌组对应的数据信息
        //public Material zs, dz, dly, lr, ms, sq, sm, fs, ss;
        public bool Editing = false;
        //当前正在编辑的
        //public static GameObject nowEditingSet;     // 当前正在编辑的的卡牌组

        //统计信息
        Transform infokuan;
        int[] costcount = new int[7];
        Transform del;
        cardsetdelshow m_delBtn = new cardsetdelshow();
        protected List<SCCardListItem> m_cardList = new List<SCCardListItem>();

        public override void Awake()
        {
            del = transform.FindChild("del");
        }

        public override void Start()
        {
            // 加载资源
            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel] + "setcard", "setcard");
            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "classzs", "classzs");
            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "classdz", "classdz");
            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "classdly", "classdly");

            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "classlr", "classlr");
            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "classms", "classms");
            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "classsq", "classsq");

            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "classsm", "classsm");
            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "classfs", "classfs");
            //load(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial] + "classss", "classss");

            UtilApi.addEventHandle(gameObject, OnMouseClick);
            UtilApi.addHoverHandle(gameObject, OnMouseHover);

            UtilApi.addEventHandle(UtilApi.TransFindChildByPObjAndPath(gameObject, "del"), onDelBtnClk);       // 删除卡牌按钮
            m_delBtn.setGameObject(del.gameObject);
        }

        //public void load(string prefabName, string path)
        //{
        //    LoadParam param;
        //    param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
        //    param.m_prefabName = prefabName;
        //    param.m_path = path;
        //    param.m_loaded = onloaded;
        //    param.m_loadNeedCoroutine = false;
        //    param.m_resNeedCoroutine = false;
        //    Ctx.m_instance.m_resLoadMgr.loadResources(param);
        //}

        //public virtual void onloaded(IDispatchObject resEvt)            // 资源加载成功
        //{
        //    IResItem res = resEvt as IResItem;
        //    if (res.getPrefabName() == "setcard")
        //    {
        //        setcardmodel = (res.getObject(res.getPrefabName()) as GameObject).transform;
        //    }
        //    else if (res.getPrefabName() == "classzs")
        //    {
        //        zs = res.getObject(res.getPrefabName()) as Material;
        //    }
        //    else if (res.getPrefabName() == "classdz")
        //    {
        //        dz = res.getObject(res.getPrefabName()) as Material;
        //    }
        //    else if (res.getPrefabName() == "classdly")
        //    {
        //        dly = res.getObject(res.getPrefabName()) as Material;
        //    }
        //    else if (res.getPrefabName() == "classlr")
        //    {
        //        lr = res.getObject(res.getPrefabName()) as Material;
        //    }
        //    else if (res.getPrefabName() == "classms")
        //    {
        //        ms = res.getObject(res.getPrefabName()) as Material;
        //    }
        //    else if (res.getPrefabName() == "classsq")
        //    {
        //        sq = res.getObject(res.getPrefabName()) as Material;
        //    }
        //    else if (res.getPrefabName() == "classms")
        //    {
        //        ms = res.getObject(res.getPrefabName()) as Material;
        //    }
        //    else if (res.getPrefabName() == "classfs")
        //    {
        //        fs = res.getObject(res.getPrefabName()) as Material;
        //    }
        //    else if (res.getPrefabName() == "classss")
        //    {
        //        ss = res.getObject(res.getPrefabName()) as Material;
        //    }
        //}

        public void OnMouseClick(GameObject go)
        {
            OnMouseUpAsButton();
        }

        public void OnMouseUpAsButton()
        {
            (Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_curEditCardSet.startEdit(this);
        }

        public void enterEditorMode()
        {
            if (Editing)
            {
                return;
            }
            goup();

            //nowEditingSet = gameObject;

            //edit();
            //transform.root.SendMessage("editset");
            //transform.parent.BroadcastMessage("hide");
            //transform.root.FindChild("classfilter").BroadcastMessage("classfilterhide", info.classs);

            IUISceneWDSC uiSC = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as IUISceneWDSC;
            // 设置编辑模式
            uiSC.editset();

            // hide 新建卡牌按钮
            uiSC.hidenewCardSet();
            // 遍历所有的卡牌集合，进行隐藏
            //uiSC.hideAllCard();
            (Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_leftCardGroupList.hide();

            // classfilterhide
            uiSC.classfilterhide((EnPlayerCareer)info.m_cardGroup.occupation);
        }

        //public void setinfo(set s)
        public void setinfo(CardGroupItem s)
        {
            info = s;
            if (s.m_cardGroup.index == uint.MaxValue)
            {
                s.m_cardGroup.name = "新建套牌";
            }
            setname(s.m_cardGroup.name);
            //switch (s.classs)
            //{
            //    case CardClass.kdruid:
            //        setpic(dly);
            //        break;

            //    case CardClass.khunter:
            //        setpic(lr);
            //        break;

            //    case CardClass.kmage:
            //        setpic(fs);
            //        break;

            //    case CardClass.kpaladin:
            //        setpic(sq);
            //        break;

            //    case CardClass.kpriest:
            //        setpic(ms);
            //        break;

            //    case CardClass.krogue:
            //        setpic(dz);
            //        break;

            //    case CardClass.kshama:
            //        setpic(sm);
            //        break;

            //    case CardClass.kwarlock:
            //        setpic(ss);
            //        break;

            //    case CardClass.kwarrior:
            //        setpic(zs);
            //        break;
            //}
            setpic(Ctx.m_instance.m_matMgr.getCardGroupMatByOccup((EnPlayerCareer)s.m_cardGroup.occupation).m_mat);
        }

        void setclass(EnPlayerCareer c)
        {
            info.m_cardGroup.occupation = (uint)c;
        }

        //public void Createnew(set s)
        public void Createnew(CardGroupItem s, bool bEnterEdit = true)
        {
            setinfo(s);
            //OnMouseUpAsButton();      // 不在模拟点击
            if (bEnterEdit)
            {
                (Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_curEditCardSet.startEdit(this);
            }
        }

        public void startEdit(cardset cardSet)
        {
            // 卡牌组处理
            (Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_curEditCardSet.copyAndInitData(cardSet);
            (Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_curEditCardSet.enterEditorMode();
            // 当前编辑的卡牌列表处理

            (Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_leftCardList.show();
            (Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).updateLeftCardList();
        }

        public void copyAndInitData(cardset cardSet)
        {
            (Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_curEditCardSet.copyFrom(cardSet);
            (Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_curEditCardSet.setinfo((Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_curEditCardSet.info);      // 设置卡牌的显示信息
        }

        void setname(string n)
        {
            info.m_cardGroup.name = n;
            //transform.FindChild("name").FindChild("Label").GetComponent<Text>().text = n;
            name = n;
        }

        void setpic(Material m)
        {
#if UNITY_5
            transform.FindChild("pic").GetComponent<Renderer>().material = m;
#elif UNITY_4_6
            transform.FindChild("pic").renderer.material = m;
#endif
        }

        Vector3 lastpostion;
        Vector3 uppostion = new Vector3(2.313244f, 2.465511f, 2.985396f);
        // 将编辑的卡牌放到顶端
        void goup()
        {
            lastpostion = transform.localPosition;
            transform.position = uppostion;
            Editing = true;
            infokuan = transform.root.FindChild("setinfo");
        }

        public void hide()
        {
            if (Editing)
            {
                //return;
                Editing = false;
            }
            lastpostion = transform.localPosition;
            transform.Translate(new Vector3(0, 0, -20f));
        }

        public void goback()
        {
            transform.localPosition = lastpostion;
            //Editing = false;
            //if (nowEditingSet == gameObject)
            if ((Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_curEditCardSet.getGameObject() == gameObject)
            {
                cls();
            }
        }

        public void OnMouseHover(GameObject go, bool state)
        {
            if(true == state)
            {
                OnMouseEnter();
            }
            else
            {
                OnMouseExit();
            }
        }

        void OnMouseEnter()
        {
            if (Editing)
            {
                infokuan.gameObject.SetActive(true);
            }
            else
            {
                del.gameObject.SetActive(true);
            }
        }

        void Update()
        {
            /*
            if (!Editing&&!isindel&&isexit)
            {
            
                isexit = false;
            }*/
        }

        void OnMouseExit()
        {
            if (Editing)
            {
                infokuan.gameObject.SetActive(false);
            }
            else
            {
                // 确认对话框
                //del.SendMessage("hide", SendMessageOptions.DontRequireReceiver);
                Ctx.m_instance.m_coroutineMgr.StartCoroutine(m_delBtn.hide());
            }
        }

        // 显示卡牌组中的卡牌
        void edit()
        {
            if (info.m_cardList == null)
            {
                return;
            }

            //拆箱
            //string[] cs = info.cards.Split('/');
            //foreach (string s in cs)
            //{
            //    Debug.Log("将要拆箱的:" + s);
            //    if (s != "")
            //    {
            //        string[] idandnum = s.Split('*');
            //        card tc = wdscpage.getcardwithid(idandnum[0]);
            //        Debug.Log("拆箱后得到的结果:" + tc.cardid + "*" + tc.insetcount + "*" + tc.count);

            //        for (int x = 0; x < int.Parse(idandnum[1]); x++)
            //        {
            //            addcard(tc);
            //            tc.insetcount++;
            //            tc.count--;
            //        }
            //    }
            //}

            if (info.m_cardList != null && info.m_cardList.Count > 0)
            {
                foreach (uint id in info.m_cardList)
                {
                    CardItemBase tc = Ctx.m_instance.m_dataPlayer.m_dataCard.m_id2CardDic[id];
                    addcard(tc);
                }
 
                (Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_leftCardList.Reposition();
            }
        }

        //把卡清掉
        void cls()
        {
            string cardstext = "";
            foreach (Transform t in setcards)
            {
                //card c = t.GetComponent<SetCard>().info;
                CardItemBase c = new CardItemBase();
                //装箱
                //cardstext += c.cardid + "*" + c.insetcount + "/";//进行记录              
            }

            //if (cardstext != info.cards)
            //{
            //    info.cards = cardstext;
            //}
            //广播方法 setcardcls
            gameObject.BroadcastMessage("setcardcls", SendMessageOptions.DontRequireReceiver);
            setcards.Clear();
            updatacardcount("");
        }

        //删除
        void ondel()
        {
            //填充数据
            //yesnomsgbox.callbackgameobject = gameObject;
            //yesnomsgbox.callbackeventname = "delyesorno";
            (Ctx.m_instance.m_uiSceneMgr.loadAndShowForm(UISceneFormID.eUISceneComDialog) as UISceneComDialog).m_yesnomsgbox.m_cb = delyesorno;
            //显示确认取消框
            //boxcam.ynmsgbox.SendMessage("show", "你确认删除嘛?这个操作不可逆!");
            (Ctx.m_instance.m_uiSceneMgr.loadAndShowForm(UISceneFormID.eUISceneComDialog) as UISceneComDialog).m_yesnomsgbox.show("你确认删除嘛?这个操作不可逆!");
        }

        void delyesorno(bool yorn)
        {
            if (yorn)
            {
                //把别的向上移动
                //int p = (Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_playersets.IndexOf(transform);
                //Debug.Log(p);
                //for (; p < (Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_playersets.Count; p++)
                //{
                //    (Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_playersets[p].Translate(new Vector3(0, 0, 0.525f));
                //}
                //(Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_playersets.Remove(transform);
                //UtilApi.Destroy(gameObject);

                stReqDeleteOneCardGroupUserCmd cmd = new stReqDeleteOneCardGroupUserCmd();
                cmd.index = info.m_cardGroup.index;
                UtilMsg.sendMsg(cmd);
            }
            else
            {
                //不
            }
        }

        void realdel()
        {

        }

        //卡组中的卡
        List<Transform> setcards = new List<Transform>();
        //卡组中卡的模型
        //public Transform setcardmodel;
        Transform page;
        void updatecard(CardItemBase c)
        {
            if (page == null)
            {
                page = transform.root.FindChild("page");
            }
            page.SendMessage("updatecard", c);
        }

        //减去一张卡
        void subcard(CardItemBase c)
        {
            //if (c.insetcount <= 0)//删除
            //{
            //    foreach (Transform t in setcards)
            //    {
            //        if (t.name == c.cardid)
            //        {
            //            int p = setcards.IndexOf(t);
            //            setcardup(p);
            //            setcards.Remove(t);
            //            UtilApi.Destroy(t.gameObject);
            //            break;
            //        }
            //    }
            //}
            //向page更新这张卡的信息
            updatecard(c);
            updatacardcount();
        }

        Vector3 startpostion = new Vector3(-0.007081032f, -0.1669452f, -0.3663702f);
        //加一张卡
        //public void addcard(card c)
        public void addcard(CardItemBase c)
        {
            ////如果已经满了
            //if (getcardcount() == 30)
            //{
            //    //不加,
            //    return;
            //}
            //c.insetcount++;
            //c.count--;
            //if (isinset(c.cardid)) //有
            //{
            //    transform.FindChild(c.cardid).SendMessage("addone", c);
            //}
            //else
            //{
            //    //根据消费来定插入位置
            //    int p = indexofset(int.Parse(c.cost));
            //    //实例化一个
            //    Transform go;

            //    if (setcards.Count == 0)
            //    {
            //        //go = (Transform)UtilApi.Instantiate(setcardmodel, startpostion, transform.rotation);
            //        go = (Transform)UtilApi.Instantiate(Ctx.m_instance.m_modelMgr.getCardGroupModel().getObject(), startpostion, transform.rotation);
            //        go.parent = transform;
            //        go.localPosition = startpostion;
            //    }
            //    else if (p == setcards.Count)
            //    {
            //        go = (Transform)UtilApi.Instantiate(Ctx.m_instance.m_modelMgr.getCardGroupModel().getObject(), setcards[p - 1].position, setcards[p - 1].rotation);
            //        go.Translate(Vector3.back * 0.15f * 1.290421f);
            //    }
            //    else
            //    {
            //        go = (Transform)UtilApi.Instantiate(Ctx.m_instance.m_modelMgr.getCardGroupModel().getObject(), setcards[p].position, setcards[p].rotation);
            //    }

            //    go.parent = transform;

            //    go.SendMessage("setinfo", c);
            //    setcarddown(p);
            //    setcards.Insert(p, go);
            ////}
            //updatacardcount();
            ////向page更新这张卡的信息
            //updatecard(c);
            ////更新统计信息

            GameObject go = UtilApi.Instantiate(Ctx.m_instance.m_modelMgr.getGroupCardModel().getObject()) as GameObject;
            //go.transform.parent = (Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_leftCardList.getGameObject().transform;
            (Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC).m_leftCardList.AddChild(go.transform);
        }

        //查找一张卡在不在卡组中
        bool isinset(string n)
        {
            foreach (Transform t in setcards)
            {
                if (t.name == n)
                {
                    return true;
                }
            }

            return false;
        }

        Text cardcount;
        void updatacardcount(string t)
        {
            if (cardcount == null)
            {
                cardcount = transform.root.FindChild("cardcount").GetComponent<Text>();
            }
            //cardcount.text = t;
        }

        int getcardcount()
        {
            //if (cardcount == null)
            //{
            //    return null;
            //}
            //else
            //{
            //    return cardcount.text;
            //}

            return info.getCardCount();
        }

        void updatacardcount()
        {
            int count = 0;
            costcount = new int[7] { 0, 0, 0, 0, 0, 0, 0 };

            foreach (Transform t in setcards)
            {
                //card c = t.GetComponent<SetCard>().info;
                CardItemBase c = new CardItemBase();
                //count = count + c.insetcount;
                ////更新统计信息
                //int cost = int.Parse(c.cost);
                //if (cost <= 7)
                //{
                //    costcount[cost] += c.insetcount;
                //}
                //else
                //{
                //    costcount[7] += c.insetcount;
                //}
            }

            updatacardcount(count + "/30");
            //更新统计信息
            updateCostCount();
        }

        void updateCostCount()
        {
            float max = 0;
            //得到最大值
            foreach (int k in costcount)
            {
                if (k > max)
                {
                    max = k;
                }
            }

            for (int x = 0; x < 7; x++)
            {
                infokuan.FindChild(x.ToString()).GetComponent<Text>().text = costcount[x].ToString();
                //得到条形
                Transform t = infokuan.FindChild("c" + x);

                float y = costcount[x] / max;//NaN测试
                if (float.IsNaN(y))
                {
                    y = 0;
                }
                t.localScale = new Vector3(1, y, 1);
            }
        }
        int indexofset(int cost)
        {
            int ret = 0;

            foreach (Transform t in setcards)
            {
                //int cardcost = int.Parse(t.transform.FindChild("cost").GetComponent<Text>().text);

                //if (cardcost > cost)
                //{
                //    return ret;
                //}

                ret++;
            }
            return ret;
        }

        void setcarddown(int p)
        {
            for (; p < setcards.Count; p++)
            {
                setcards[p].Translate(Vector3.back * 0.15f * 1.290421f);
                iTween.MoveBy(setcards[p].gameObject, Vector3.back * 0.15f, 0.1f);
            }
        }

        void setcardup(int p)
        {
            for (; p < setcards.Count; p++)
            {
                setcards[p].Translate(Vector3.forward * 0.15f * 1.290421f);
                iTween.MoveBy(setcards[p].gameObject, Vector3.forward * 0.15f, 0.1f);
            }
        }

        public void copyFrom(cardset cards)
        {
            if (info == null)
            {
                info = new CardGroupItem();
            }
            this.info.copyFrom(cards.info);
        }

        public void onDelBtnClk(GameObject go)
        {
            ondel();
        }
    }
}
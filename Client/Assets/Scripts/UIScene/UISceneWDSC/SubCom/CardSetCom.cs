using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SDK.Common;
using Game.Msg;
using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief 一个卡组
     */
    public class CardSetCom : InterActiveEntity
    {
        public SceneWDSCData m_sceneWDSCData;

        public CardGroupItem m_cardGroupItem;             // 当前卡牌组对应的数据信息
        //统计信息
        protected Transform m_infoKuan;
        protected int[] m_costCount = new int[7];
        protected Transform m_delTrans;
        protected CardSetDelBtn m_delBtn = new CardSetDelBtn();
        protected List<SCCardListItem> m_cardList = new List<SCCardListItem>();

        public override void Awake()
        {
            
        }

        public override void Start()
        {
            m_delTrans = transform.FindChild("del");        // 右上角的删除叉号

            UtilApi.addEventHandle(gameObject, OnMouseClick);
            UtilApi.addHoverHandle(gameObject, OnMouseHover);

            UtilApi.addEventHandle(UtilApi.TransFindChildByPObjAndPath(gameObject, "del"), onDelBtnClk);       // 删除卡牌按钮
            m_delBtn.setGameObject(m_delTrans.gameObject);
        }

        public void OnMouseClick(GameObject go)
        {
            OnMouseUpAsButton();
        }

        public void OnMouseUpAsButton()
        {
            m_cardGroupItem.reqCardList();
            m_sceneWDSCData.m_wdscCardSetPnl.m_curEditCardSet.startEdit(this);
        }

        public void enterEditorMode()
        {
            moveSelf2Top();     // 移动自己到顶端
            m_sceneWDSCData.m_wdscCardSetPnl.startEditCardSetMode();        // 卡牌组面板进入编辑模式
            m_sceneWDSCData.m_pClassFilterPnl.hideClassFilterBtnExceptThis((EnPlayerCareer)m_cardGroupItem.m_cardGroup.occupation);    // 职业过滤按钮面板根据职业过滤掉不显示的职业
        }

        public void setInfo(CardGroupItem s)
        {
            m_cardGroupItem = s;
            if (s.m_cardGroup.index == uint.MaxValue)
            {
                s.m_cardGroup.name = "新建套牌";
            }
            setName(s.m_cardGroup.name);
            setPic(Ctx.m_instance.m_matMgr.getCardGroupMatByOccup((EnPlayerCareer)s.m_cardGroup.occupation).m_mat);
        }

        protected void setClass(EnPlayerCareer c)
        {
            m_cardGroupItem.m_cardGroup.occupation = (uint)c;
        }

        public void createNew(CardGroupItem s, bool bEnterEdit = true)
        {
            setInfo(s);
            if (bEnterEdit)
            {
                m_sceneWDSCData.m_wdscCardSetPnl.m_curEditCardSet.startEdit(this);
            }
        }

        public void startEdit(CardSetCom cardSet)
        {
            // 卡牌组处理
            copyAndInitData(cardSet);
            enterEditorMode();
            // 当前编辑的卡牌列表处理
            m_sceneWDSCData.m_wdscCardSetPnl.m_leftCardList.showGrid();
            m_sceneWDSCData.m_wdscCardSetPnl.updateLeftCardList();
        }

        public void copyAndInitData(CardSetCom cardSet)
        {
            m_sceneWDSCData.m_wdscCardSetPnl.m_curEditCardSet.copyFrom(cardSet);
            m_sceneWDSCData.m_wdscCardSetPnl.m_curEditCardSet.setInfo(m_sceneWDSCData.m_wdscCardSetPnl.m_curEditCardSet.m_cardGroupItem);      // 设置卡牌的显示信息
        }

        protected void setName(string n)
        {
            m_cardGroupItem.m_cardGroup.name = n;
            //transform.FindChild("name").FindChild("Label").GetComponent<Text>().text = n;
            name = n;
        }

        protected void setPic(Material m)
        {
#if UNITY_5
            transform.FindChild("pic").GetComponent<Renderer>().material = m;
#elif UNITY_4_6
            transform.FindChild("pic").renderer.material = m;
#endif
        }

        // 将编辑的卡牌放到顶端
        protected void moveSelf2Top()
        {
            transform.localPosition = m_sceneWDSCData.m_wdscCardSetPnl.m_topEditCardPosGo.transform.localPosition;
            showCardSet();
            m_infoKuan = transform.root.FindChild("setinfo");
        }

        public void hideCardSet()
        {
            UtilApi.SetActive(gameObject, false);
        }

        public void showCardSet()
        {
            UtilApi.SetActive(gameObject, true);
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

        protected void OnMouseEnter()
        {
            if (m_sceneWDSCData.m_wdscCardSetPnl.bCurEditCardSet(this))
            {
                m_infoKuan.gameObject.SetActive(true);
            }
            else
            {
                m_delTrans.gameObject.SetActive(true);
            }
        }

        protected void Update()
        {
            
        }

        protected void OnMouseExit()
        {
            if (m_sceneWDSCData.m_wdscCardSetPnl.bCurEditCardSet(this))
            {
                m_infoKuan.gameObject.SetActive(false);
            }
            else
            {
                // 确认对话框
                Ctx.m_instance.m_coroutineMgr.StartCoroutine(m_delBtn.hide());
            }
        }

        //删除
        protected void onDel()
        {
            //填充数据
            (Ctx.m_instance.m_uiSceneMgr.loadAndShowForm<UISceneComDialog>(UISceneFormID.eUISceneComDialog) as UISceneComDialog).m_yesnomsgbox.m_cb = delYesOrNo;
            //显示确认取消框
            (Ctx.m_instance.m_uiSceneMgr.loadAndShowForm<UISceneComDialog>(UISceneFormID.eUISceneComDialog) as UISceneComDialog).m_yesnomsgbox.show("你确认删除嘛?这个操作不可逆!");
        }

        protected void delYesOrNo(bool yorn)
        {
            if (yorn)
            {
                //把别的向上移动
                stReqDeleteOneCardGroupUserCmd cmd = new stReqDeleteOneCardGroupUserCmd();
                cmd.index = m_cardGroupItem.m_cardGroup.index;
                UtilMsg.sendMsg(cmd);
            }
            else
            {
                //不
            }
        }

        protected void realDel()
        {

        }

        public void copyFrom(CardSetCom cards)
        {
            if (m_cardGroupItem == null)
            {
                m_cardGroupItem = new CardGroupItem();
            }
            this.m_cardGroupItem.copyFrom(cards.m_cardGroupItem);
        }

        public void onDelBtnClk(GameObject go)
        {
            onDel();
        }
    }
}
using Game.Msg;
using SDK.Common;
using SDK.Lib;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /**
     * @brief 模式界面
     */
    public class UISceneMoShi : SceneForm, IUISceneMoShi
    {
        protected moshijm m_moshijm = new moshijm();
        protected yxclick[] m_heroList = new yxclick[9];                        // 9 个职业 hero
        public moshicardset[] m_cardGroupList = new moshicardset[9];            // 9 个卡牌组

        public moshicardset m_curSel;           // 当前选择的卡牌组
        public Text m_lblTip;

        public override void onReady()
        {
            base.onReady();

            getWidget();
            addEventHandle();
        }

        public override void onShow()
        {
            base.onShow();
            m_moshijm.dzmoshi();
        }

        public override void onHide()
        {
            base.onHide();
            m_lblTip.text = "";
        }

        protected void getWidget()
        {
            m_moshijm.setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm"));
            int idx = 0;
            while(idx < 9)
            {
                m_heroList[idx] = new yxclick();

                ++idx;
            }

            m_heroList[0].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/jbtp/战士"));
            m_heroList[1].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/jbtp/萨满祭司"));
            m_heroList[2].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/jbtp/潜行者"));

            m_heroList[3].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/jbtp/圣骑士"));
            m_heroList[4].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/jbtp/猎人"));
            m_heroList[5].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/jbtp/德鲁伊"));

            m_heroList[6].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/jbtp/术士"));
            m_heroList[7].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/jbtp/法师"));
            m_heroList[8].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/jbtp/牧师"));

            idx = 0;
            while(idx < 9)
            {
                m_cardGroupList[idx] = new moshicardset();

                ++idx;
            }

            m_cardGroupList[0].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/1"));
            m_cardGroupList[1].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/2"));
            m_cardGroupList[2].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/3"));

            m_cardGroupList[3].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/4"));
            m_cardGroupList[4].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/5"));
            m_cardGroupList[5].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/6"));

            m_cardGroupList[6].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/7"));
            m_cardGroupList[7].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/8"));
            m_cardGroupList[8].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/9"));

            m_lblTip = UtilApi.GoFindChildByPObjAndName("MatchResult/Canvas/Text").GetComponent<Text>();
            m_lblTip.text = "";
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName(SceneMoshiPath.BtnReturn), onBtnClkReturn);   // 返回
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName(SceneMoshiPath.BtnSelect), onBtnClkSelect);   // 返回
        }

        protected void onBtnClkReturn(GameObject go)
        {
            //hide();
            m_moshijm.gotoback();
            this.bVisible = false;
        }

        protected void onBtnClkSelect(GameObject go)
        {
            m_moshijm.chooseok();
            this.bVisible = false;
        }

        public void AddNewTaoPai()
        {
            m_moshijm.newset();
        }

        public void setclass(EnPlayerCareer c)
        {
            m_moshijm.setclass(c);
        }

        public void setClassname(string n)
        {
            m_moshijm.setClassname(n);
        }

        public void setclasspic(Material pic)
        {
            m_moshijm.setclasspic(pic);
        }

        public EnPlayerCareer getClass()
        {
            return m_moshijm.getClass();
        }

        public void setCardGroup(moshicardset value)
        {
            m_curSel = value;
        }

        public void updateHeroList()
        {
            m_moshijm.getPlayerSet();
        }

        public void psstRetHeroFightMatchUserCmd(stRetHeroFightMatchUserCmd cmd)
        {
            m_lblTip.text = "匹配成功了";
        }

        public void startmatch()
        {
            m_lblTip.text = "现在开始匹配了，注意了";
            // test 进入战场
            //Ctx.m_instance.m_loadDZScene();
        }
    }
}
using Game.Msg;
using SDK.Common;
using SDK.Lib;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /**
     * @brief 场景中卡牌基类
     */
    public class SceneCardEntityBase : LSBehaviour
    {
        public static Vector3 SMALLFACT = new Vector3(0.5f, 0.5f, 0.5f);    // 小牌时的缩放因子
        public static Vector3 BIGFACT = new Vector3(1.2f, 1.2f, 1.2f);      // 大牌时候的因子

        protected SceneCardItem m_sceneCardItem;
        public SceneDZData m_sceneDZData;

        protected ushort m_curIndex = 0;// 当前索引，因为可能初始卡牌的时候 m_sceneCardItem 
        protected ushort m_preIndex = 0;// 在牌中的索引，主要是手里的牌和打出去的牌，这个是客户端设置的索引，服务器的索引在 t_Card 类型里面

        protected GameObject m_chaHaoGo;
        public uint m_startCardID;

        protected NumAniSequence m_numAniSeq = new NumAniSequence();       // 攻击动画序列，这个所有的都有

        public override void Start()
        {
            base.Start();

            UtilApi.addEventHandle(gameObject, onClk);
        }

        public SceneCardItem sceneCardItem
        {
            get
            {
                return m_sceneCardItem;
            }
            set
            {
                m_sceneCardItem = value;

                if (m_sceneCardItem != null)
                {
                    updateCardDataChange();
                    updateCardDataNoChange();
                }
                else
                {
                    Ctx.m_instance.m_logSys.log("服务器卡牌数据为空");
                }
            }
        }

        public GameObject chaHaoGo
        {
            get
            {
                return m_chaHaoGo;
            }
            set
            {
                m_chaHaoGo = value;
            }
        }

        public ushort curIndex
        {
            get
            {
                if (m_sceneCardItem != null)
                {
                    return m_sceneCardItem.svrCard.pos.y;
                }
                else
                {
                    return m_curIndex;
                }
            }
            set
            {
                if (m_sceneCardItem != null)
                {
                    m_preIndex = m_sceneCardItem.svrCard.pos.y;
                    m_sceneCardItem.svrCard.pos.y = value;
                }
                else
                {
                    m_preIndex = m_curIndex;
                    m_curIndex = value;
                }
            }
        }

        public ushort preIndex
        {
            get
            {
                return m_preIndex;
            }
        }

        public void destroy()
        {
            UtilApi.Destroy(gameObject);
            m_sceneCardItem = null;
        }

        // 更新卡牌属性，这个主要更改卡牌经常改变的属性
        public virtual void updateCardDataChange()
        {
            if (m_sceneCardItem != null)
            {
                if (m_sceneCardItem.cardArea == CardArea.CARDCELLTYPE_COMMON || m_sceneCardItem.cardArea == CardArea.CARDCELLTYPE_HAND)
                {
                    Text text;
                    text = UtilApi.getComByP<Text>(gameObject, "attack/Text");       // 攻击
                    text.text = m_sceneCardItem.svrCard.damage.ToString();
                    text = UtilApi.getComByP<Text>(gameObject, "cost/Text");         // Magic
                    text.text = m_sceneCardItem.svrCard.mpcost.ToString();
                    text = UtilApi.getComByP<Text>(gameObject, "health/Text");       // HP
                    text.text = m_sceneCardItem.svrCard.hp.ToString();
                }
            }
        }

        // 这个主要是更新卡牌不经常改变的属性
        public virtual void updateCardDataNoChange()
        {
            if (m_sceneCardItem != null)
            {
                if (m_sceneCardItem.cardArea != CardArea.CARDCELLTYPE_HERO)
                {
                    UtilApi.updateCardDataNoChange(m_sceneCardItem.m_cardTableItem, gameObject);
                }
            }
        }

        // 根据表更新卡牌数据，这个主要是用于初始卡牌更新
        public void updateCardDataByTable()
        {
            TableItemBase tableBase = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, m_startCardID);
            if(tableBase != null)
            {
                TableCardItemBody cardTableData = tableBase.m_itemBody as TableCardItemBody;
                UtilApi.updateCardDataNoChange(cardTableData, gameObject);
            }
            else
            {
                Ctx.m_instance.m_logSys.log(string.Format("卡表查找失败， ID = {0}", m_startCardID));
            }
        }

        // 关闭拖放功能
        public virtual void disableDrag()
        {

        }

        // 开启拖动
        public virtual void  enableDrag()
        {

        }

        // 所有的卡牌都可以点击，包括主角、装备、技能、手里卡牌、出的卡牌
        public void onClk(GameObject go)
        {
            if (m_sceneDZData.m_gameRunState.isInState(GameRunState.INITCARD))      // 如果处于初始化卡牌阶段
            {
                string resPath = "";
                // 这个时候还没有服务器的数据 m_sceneCardItem
                int idx = 0;
                idx = m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].inSceneCardList.findCardIdx(this);
                // 显示换牌标志
                if (m_sceneDZData.m_changeCardList.IndexOf(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_startCardList[idx]) != -1)      // 如果已经选中
                {
                    m_sceneDZData.m_changeCardList.Remove(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_startCardList[idx]);
                    // 去掉叉号
                    UtilApi.Destroy(m_chaHaoGo);        // 释放资源
                }
                else  // 选中
                {
                    m_sceneDZData.m_changeCardList.Add(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_startCardList[idx]);
                    // 添加叉号
                    resPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "ChaHao.prefab");
                    ModelRes model = Ctx.m_instance.m_modelMgr.syncGet<ModelRes>(resPath) as ModelRes;
                    m_chaHaoGo = model.InstantiateObject(resPath) as GameObject;
                    UtilApi.SetParent(m_chaHaoGo.transform, gameObject.transform, false);
                }
            }
            else        // 如果在对战阶段
            {
                if (this.m_sceneCardItem != null)
                {
                    if (m_sceneDZData.m_gameOpState.bInOp(EnGameOp.eOpNormalAttack))
                    {
                        if (m_sceneDZData.m_gameOpState.canAttackOp(this, EnGameOp.eOpNormalAttack))
                        {
                            // 发送攻击指令
                            stCardAttackMagicUserCmd cmd = new stCardAttackMagicUserCmd();
                            cmd.dwAttThisID = m_sceneDZData.m_gameOpState.getOpCardID();
                            cmd.dwDefThisID = this.m_sceneCardItem.svrCard.qwThisID;
                            UtilMsg.sendMsg(cmd);

                            //m_sceneDZData.m_gameOpState.quitAttackOp(false);
                        }
                        else
                        {
                            enterAttack();
                        }
                    }
                    else if ((m_sceneDZData.m_gameOpState.bInOp(EnGameOp.eOpFaShu)))        // 法术攻击
                    {
                        if (m_sceneDZData.m_gameOpState.canAttackOp(this, EnGameOp.eOpFaShu))
                        {
                            // 必然是有目标的法术攻击
                            // 发送法术攻击消息
                            stCardMoveAndAttackMagicUserCmd cmd = new stCardMoveAndAttackMagicUserCmd();
                            cmd.dwAttThisID = m_sceneDZData.m_gameOpState.getOpCardID();
                            cmd.dwMagicType = (uint)m_sceneDZData.m_gameOpState.getOpCardFaShu();
                            cmd.dwDefThisID = this.sceneCardItem.svrCard.qwThisID;
                            //m_sceneDZData.m_gameOpState.quitAttackOp(false);
                            UtilMsg.sendMsg(cmd);
                        }
                        else
                        {
                            enterAttack();
                        }
                    }
                    else if (m_sceneDZData.m_gameOpState.bInOp(EnGameOp.eOpZhanHouAttack))      // 战吼攻击
                    {
                        if (m_sceneDZData.m_gameOpState.canAttackOp(this, EnGameOp.eOpZhanHouAttack))
                        {
                            // 发送攻击指令
                            stCardMoveAndAttackMagicUserCmd cmd = new stCardMoveAndAttackMagicUserCmd();
                            cmd.dwAttThisID = m_sceneDZData.m_gameOpState.getOpCardID();
                            cmd.dwMagicType = (uint)m_sceneDZData.m_gameOpState.getOpCardFaShu();
                            cmd.dwDefThisID = this.m_sceneCardItem.svrCard.qwThisID;
                            cmd.dst = new stObjectLocation();
                            cmd.dst.dwLocation = (uint)this.m_sceneCardItem.cardArea;
                            cmd.dst.y = this.curIndex;
                            UtilMsg.sendMsg(cmd);

                            //m_sceneDZData.m_gameOpState.quitAttackOp();
                        }
                        else
                        {
                            enterAttack();
                        }
                    }
                    else        // 默认点击处理都走这里
                    {
                        enterAttack();
                    }
                }
            }
        }

        // 进入普通攻击状态
        protected void enterAttack()
        {
            if (this.m_sceneCardItem.cardArea == CardArea.CARDCELLTYPE_COMMON)
            {
                // 只有点击自己的时候，才启动攻击
                if (m_sceneCardItem.m_playerFlag == EnDZPlayer.ePlayerSelf)
                {
                    m_sceneDZData.m_gameOpState.enterAttackOp(EnGameOp.eOpNormalAttack, this);
                }
            }
        }

        // 更新卡牌是否可以出牌
        public void updateCardOutState(bool benable)
        {
            GameObject go = UtilApi.TransFindChildByPObjAndPath(getGameObject(), "bailight");
            if (go != null)
            {
                if (benable)
                {
                    if (sceneCardItem != null)
                    {
                        //try
                        //{
                            if (sceneCardItem.svrCard.mpcost <= Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)sceneCardItem.m_playerFlag].m_heroMagicPoint.mp)
                            {
                                if (UtilApi.getComByP<MeshRenderer>(go).enabled != true)
                                {
                                    UtilApi.getComByP<MeshRenderer>(go).enabled = true;
                                }
                            }
                            else
                            {
                                if (UtilApi.getComByP<MeshRenderer>(go).enabled != false)
                                {
                                    UtilApi.getComByP<MeshRenderer>(go).enabled = false;
                                }
                            }
                        //}
                        //catch (System.Exception e)
                        //{
                        //    // 输出日志
                        //    Ctx.m_instance.m_logSys.error("updateCardGreenFrame 异常");
                        //    Ctx.m_instance.m_logSys.error(e.Message);
                        //}
                    }
                }
                else
                {
                    if(UtilApi.getComByP<MeshRenderer>(go).enabled != benable)
                    {
                        UtilApi.getComByP<MeshRenderer>(go).enabled = false;
                    }
                }
            }
        }

        // 更新卡牌是否可以被击
        public void updateCardAttackedState(bool benable)
        {
            GameObject go = UtilApi.TransFindChildByPObjAndPath(getGameObject(), "bailight");
            if (go != null)
            {
                if (UtilApi.getComByP<MeshRenderer>(go).enabled != benable)
                {
                    if (benable)
                    {
                        if (sceneCardItem != null)
                        {
                            UtilApi.getComByP<MeshRenderer>(go).enabled = true;
                        }
                    }
                    else
                    {
                        UtilApi.getComByP<MeshRenderer>(go).enabled = false;
                    }
                }
            }
        }

        // 播放攻击动画，就是移动过去砸一下
        public void playAttackAni(Vector3 destPos)
        {
            Vector3 midPt;      // 中间点
            midPt = (gameObject.transform.localPosition + destPos) / 2;
            midPt.y = 2;

            SimpleCurveAni curveAni = new SimpleCurveAni();
            m_numAniSeq.addOneNumAni(curveAni);
            curveAni.setGO(gameObject);
            curveAni.setTime(0.3f);
            curveAni.setPlotCount(3);
            curveAni.addPlotPt(0, gameObject.transform.localPosition);
            curveAni.addPlotPt(1, midPt);
            curveAni.addPlotPt(2, destPos);

            curveAni.setEaseType(iTween.EaseType.easeInExpo);

            curveAni = new SimpleCurveAni();
            m_numAniSeq.addOneNumAni(curveAni);
            curveAni.setGO(gameObject);
            curveAni.setTime(0.3f);
            curveAni.setPlotCount(3);
            curveAni.addPlotPt(0, destPos);
            curveAni.addPlotPt(1, midPt);
            curveAni.addPlotPt(2, gameObject.transform.localPosition);

            curveAni.setEaseType(iTween.EaseType.easeInExpo);

            m_numAniSeq.play();
        }

        public void playFlyNum(int num)
        {
            Ctx.m_instance.m_pFlyNumMgr.addFlyNum(num, gameObject.transform.localPosition, m_sceneDZData.m_centerGO);
        }

        // 是否是客户端先从手牌区域移动到出牌区域，然后再发动攻击的卡牌
        public bool canClientMove2OutArea()
        {
            if ((m_sceneCardItem.m_cardTableItem.m_type == (int)CardType.CARDTYPE_MAGIC && m_sceneCardItem.m_cardTableItem.m_bNeedFaShuTarget > 0) || (m_sceneCardItem.m_cardTableItem.m_zhanHou > 0 && m_sceneCardItem.m_cardTableItem.m_bNeedZhanHouTarget > 0))
            {
                return true;
            }

            return false;
        }
    }
}
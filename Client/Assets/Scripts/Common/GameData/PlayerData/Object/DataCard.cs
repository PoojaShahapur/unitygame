using Game.Msg;
using Game.UI;
using SDK.Lib;
using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 卡牌数据
     */
    public class DataCard
    {
        public bool m_canReqData = true;            // 请求所有的卡牌数据
        public bool m_canReqCardGroup = true;       // 是否可以请求卡牌组列表

        // 分职业卡牌
        public List<CardItemBase>[] m_cardListArr = new List<CardItemBase>[(int)EnPlayerCareer.ePCTotal];      // 每一个职业一个列表
        public Dictionary<uint, CardItemBase> m_id2CardDic = new Dictionary<uint, CardItemBase>();

        // 每一个卡牌组
        public List<CardGroupItem> m_cardGroupListArr = new List<CardGroupItem>();      // 每一个职业一个列表
        public Dictionary<uint, CardGroupItem> m_id2CardGroupDic = new Dictionary<uint, CardGroupItem>();
        public EventDispatch m_cardSetChangedDisp;  // 卡组改变事件分发
        
        // 场景中的卡牌
        public CardModelItem[] m_sceneCardModelAttrItemList = new CardModelItem[(int)CardType.eCARDTYPE_Total];

        public DataCard()
        {
            int idx = 0;
            while(idx < (int)EnPlayerCareer.ePCTotal)
            {
                m_cardListArr[idx] = new List<CardItemBase>();
                ++idx;
            }

            m_cardSetChangedDisp = new AddOnceEventDispatch();
        }

        public void registerCardAttr()
        {
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_ATTEND] = new CardModelItem();
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_ATTEND].m_handleModelPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "Character/CommonCard.prefab");
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_ATTEND].m_outModelPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "Character/ChangCard.prefab");
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_ATTEND].m_headerSubModel = "gaibangzhutu_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_ATTEND].m_frameSubModel = "paidi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_ATTEND].m_yaoDaiSubModel = "mingzidi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_ATTEND].m_pinZhiSubModel = "pinzhi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_ATTEND].m_raceSubModel = "menpaidi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_ATTEND].m_outHeaderSubModel = "changpaizt_zhanchang";

            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_SECRET] = new CardModelItem();
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_SECRET].m_handleModelPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "Character/CommonCard.prefab");
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_SECRET].m_outModelPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "Character/ChangCard.prefab");
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_SECRET].m_headerSubModel = "gaibangzhutu_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_SECRET].m_frameSubModel = "paidi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_SECRET].m_yaoDaiSubModel = "mingzidi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_SECRET].m_pinZhiSubModel = "pinzhi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_SECRET].m_raceSubModel = "menpaidi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_SECRET].m_outHeaderSubModel = "changpaizt_zhanchang";

            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_MAGIC] = new CardModelItem();
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_MAGIC].m_handleModelPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "Character/CommonCard.prefab");
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_MAGIC].m_outModelPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "Character/ChangCard.prefab");
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_MAGIC].m_headerSubModel = "gaibangzhutu_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_MAGIC].m_frameSubModel = "paidi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_MAGIC].m_yaoDaiSubModel = "mingzidi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_MAGIC].m_pinZhiSubModel = "pinzhi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_MAGIC].m_raceSubModel = "menpaidi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_MAGIC].m_outHeaderSubModel = "changpaizt_zhanchang";

            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_EQUIP] = new CardModelItem();
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_EQUIP].m_handleModelPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "Scene/wuqi_zhanchang.prefab");
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_EQUIP].m_headerSubModel = "wuqitu_zhanchang ";

            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_HERO] = new CardModelItem();
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_HERO].m_handleModelPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "Scene/yingxiong_zhanchang.prefab");
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_HERO].m_headerSubModel = "yingxiongtu_zhanchang";

            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_SKILL] = new CardModelItem();
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_SKILL].m_handleModelPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "Scene/jineng_zhanchang.prefab");
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_SKILL].m_headerSubModel = "Cylinder001";
        }

        public void reqAllCard()
        {
            if (m_canReqData)
            {
                m_canReqData = false;
                stReqAllCardTujianDataUserCmd cmd = new stReqAllCardTujianDataUserCmd();
                UtilMsg.sendMsg(cmd);
            }
        }

        public void reqCardGroup()
        {
            if (m_canReqCardGroup)
            {
                m_canReqCardGroup = false;
                stReqCardGroupListInfoUserCmd cmd = new stReqCardGroupListInfoUserCmd();
                UtilMsg.sendMsg(cmd);
            }
        }

        public void psstNotifyAllCardTujianInfoCmd(List<t_Tujian> list)
        {
            clearAllTuJian();

            CardItemBase item = null;
            int idx = 0;
            TableItemBase tableItem = null;
            while(idx < list.Count)
            {
                tableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, list[idx].id);
                if (tableItem != null)
                {
                    item = new CardItemBase();
                    item.m_tujian = list[idx];
                    item.m_tableItemCard = tableItem.m_itemBody as TableCardItemBody;

                    m_cardListArr[item.m_tableItemCard.m_career].Add(item);
                    m_id2CardDic[list[idx].id] = item;
                }
                else
                {
                    Ctx.m_instance.m_logSys.log("表格读取失败");
                }

                ++idx;
            }

            // 更新卡牌图鉴中的显示
            IUITuJian uiTuJian = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUITuJian) as IUITuJian;
            if(uiTuJian != null)
            {
                uiTuJian.updateMidCardModel();
            }
        }

        // 新增\数量改变,不包括删除
        public void psstNotifyOneCardTujianInfoCmd(uint id, byte num)
        {
            TableItemBase tableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, id);

            if (tableItem != null)
            {
                if (!m_id2CardDic.ContainsKey(id))
                {
                    CardItemBase item = new CardItemBase();
                    item.m_tujian = new t_Tujian();
                    item.m_tujian.id = id;
                    item.m_tujian.num = num;
                    item.m_tableItemCard = tableItem.m_itemBody as TableCardItemBody;

                    m_cardListArr[item.m_tableItemCard.m_career].Add(item);
                    m_id2CardDic[id] = item;
                }
                m_id2CardDic[id].m_tujian.num = num;
            }
            else
            {
                Ctx.m_instance.m_logSys.error("psstNotifyOneCardTujianInfoCmd 不能查找到卡牌 Item");
            }

            // 更新卡牌图鉴中的显示
            IUITuJian uiTuJian = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUITuJian) as IUITuJian;
            if (uiTuJian != null)
            {
                uiTuJian.updateMidCardModel();
            }
        }

        public void psstRetCardGroupListInfoUserCmd(List<t_group_list> info)
        {
            clearAllCardGroup();

            CardGroupItem item;
            TableItemBase tableItem;
            foreach (var itemlist in info)
            {
                tableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_JOB, itemlist.occupation);
                if(tableItem != null)
                {
                    item = new CardGroupItem();
                    item.m_cardGroup = itemlist;
                    item.m_tableJobItemBody = tableItem.m_itemBody as TableJobItemBody;
                    m_cardGroupListArr.Add(item);
                    m_id2CardGroupDic[item.m_cardGroup.index] = item;
                }
            }

            m_cardSetChangedDisp.dispatchEvent(null);
        }

        public void psstRetOneCardGroupInfoUserCmd(stRetOneCardGroupInfoUserCmd msg)
        {
            if(m_id2CardGroupDic.ContainsKey(msg.index))
            {
                m_id2CardGroupDic[msg.index].m_cardList = msg.id;
            }
        }

        public void psstRetCreateOneCardGroupUserCmd(stRetCreateOneCardGroupUserCmd msg)
        {
            TableItemBase tableItem;
            tableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_JOB, msg.occupation);
            if (tableItem != null)
            {
                CardGroupItem item = new CardGroupItem();
                item.m_cardGroup = new t_group_list();
                item.m_cardGroup.occupation = msg.occupation;
                item.m_cardGroup.index = msg.index;
                item.m_cardGroup.name = msg.name;
                item.m_tableJobItemBody = tableItem.m_itemBody as TableJobItemBody;
                m_cardGroupListArr.Add(item);
                m_id2CardGroupDic[item.m_cardGroup.index] = item;
            }

            m_cardSetChangedDisp.dispatchEvent(null);
        }

        public int psstRetDeleteOneCardGroupUserCmd(uint index)
        {
            int curIdx = 0;
            foreach (CardGroupItem item in m_cardGroupListArr)
            {
                if (item.m_cardGroup.index == index)
                {
                    break;
                }

                ++curIdx;
            }

            m_cardGroupListArr.Remove(m_id2CardGroupDic[index]);
            m_id2CardGroupDic.Remove(index);

            return curIdx;
        }

        protected void clearAllTuJian()
        {
            for(int idx = 0; idx < (int)EnPlayerCareer.ePCTotal; ++idx)
            {
                if(m_cardListArr[idx] != null)
                {
                    m_cardListArr[idx].Clear();
                }
            }

            m_id2CardDic.Clear();
        }

        protected void clearAllCardGroup()
        {
            m_cardGroupListArr.Clear();
            m_id2CardGroupDic.Clear();
        }
    }
}
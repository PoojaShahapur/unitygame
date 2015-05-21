using Game.Msg;
using Game.UI;
using SDK.Lib;
using System.Collections.Generic;

namespace SDK.Common
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

        public Dictionary<int, CardGroupAttrMatItem> m_id2CardGroupMatAttrDic = new Dictionary<int, CardGroupAttrMatItem>();        // 卡牌材质属性

        // 卡牌组
        public CardGroupModelAttrItem m_cardGroupModelAttrItem = new CardGroupModelAttrItem();
        // 卡牌组中的卡牌
        public CardGroupModelAttrItem m_groupCardModelAttrItem = new CardGroupModelAttrItem();
        // 场景中的卡牌
        public CardModelItem[] m_sceneCardModelAttrItemList = new CardModelItem[(int)CardType.eCARDTYPE_Total];
        // cost 模型
        public CardGroupModelAttrItem m_costModelAttrItem = new CardGroupModelAttrItem();
        public CardGroupModelAttrItem m_enemyCardModelAttrItem = new CardGroupModelAttrItem();
        public CardGroupModelAttrItem m_minionModelAttrItem = new CardGroupModelAttrItem();

        public DataCard()
        {
            int idx = 0;
            while(idx < (int)EnPlayerCareer.ePCTotal)
            {
                m_cardListArr[idx] = new List<CardItemBase>();
                ++idx;
            }

            // Test
            m_id2CardDic[1] = new CardItemBase();
            m_id2CardDic[2] = new CardItemBase();
            m_id2CardDic[3] = new CardItemBase();
        }

        public void registerCardAttr()
        {
            m_id2CardGroupMatAttrDic[(int)EnPlayerCareer.HERO_OCCUPATION_1] = new CardGroupAttrMatItem();
            m_id2CardGroupMatAttrDic[(int)EnPlayerCareer.HERO_OCCUPATION_1].m_cardClass = EnPlayerCareer.HERO_OCCUPATION_1;
            m_id2CardGroupMatAttrDic[(int)EnPlayerCareer.HERO_OCCUPATION_1].m_path = string.Format("{0}{1}{2}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial], "skin/", "classdly_1");
            m_id2CardGroupMatAttrDic[(int)EnPlayerCareer.HERO_OCCUPATION_1].m_logoPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial], "dly");

            m_id2CardGroupMatAttrDic[(int)EnPlayerCareer.HERO_OCCUPATION_2] = new CardGroupAttrMatItem();
            m_id2CardGroupMatAttrDic[(int)EnPlayerCareer.HERO_OCCUPATION_2].m_cardClass = EnPlayerCareer.HERO_OCCUPATION_2;
            m_id2CardGroupMatAttrDic[(int)EnPlayerCareer.HERO_OCCUPATION_2].m_path = string.Format("{0}{1}{2}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial], "skin/", "classlr_1");
            m_id2CardGroupMatAttrDic[(int)EnPlayerCareer.HERO_OCCUPATION_2].m_logoPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial], "lr");

            m_id2CardGroupMatAttrDic[(int)EnPlayerCareer.HERO_OCCUPATION_3] = new CardGroupAttrMatItem();
            m_id2CardGroupMatAttrDic[(int)EnPlayerCareer.HERO_OCCUPATION_3].m_cardClass = EnPlayerCareer.HERO_OCCUPATION_3;
            m_id2CardGroupMatAttrDic[(int)EnPlayerCareer.HERO_OCCUPATION_3].m_path = string.Format("{0}{1}{2}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial], "skin/", "classfs_1");
            m_id2CardGroupMatAttrDic[(int)EnPlayerCareer.HERO_OCCUPATION_3].m_logoPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathMaterial], "fs");

            m_cardGroupModelAttrItem.m_path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "cardset.prefab");
            m_groupCardModelAttrItem.m_path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "setcard.prefab");

            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_ATTEND] = new CardModelItem();
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_ATTEND].m_path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "CardModel.prefab");
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_ATTEND].m_headerSubModel = "gaibangzhutu_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_ATTEND].m_frameSubModel = "paidi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_ATTEND].m_yaoDaiSubModel = "mingzidi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_ATTEND].m_pinZhiSubModel = "pinzhi_kapai";

            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_SECRET] = new CardModelItem();
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_SECRET].m_path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "CardModel.prefab");
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_SECRET].m_headerSubModel = "gaibangzhutu_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_SECRET].m_frameSubModel = "paidi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_SECRET].m_yaoDaiSubModel = "mingzidi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_SECRET].m_pinZhiSubModel = "pinzhi_kapai";

            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_MAGIC] = new CardModelItem();
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_MAGIC].m_path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "CardModel.prefab");
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_MAGIC].m_headerSubModel = "gaibangzhutu_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_MAGIC].m_frameSubModel = "paidi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_MAGIC].m_yaoDaiSubModel = "mingzidi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_MAGIC].m_pinZhiSubModel = "pinzhi_kapai";

            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_EQUIP] = new CardModelItem();
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_EQUIP].m_path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "CardModel.prefab");
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_EQUIP].m_headerSubModel = "gaibangzhutu_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_EQUIP].m_frameSubModel = "paidi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_EQUIP].m_yaoDaiSubModel = "mingzidi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_EQUIP].m_pinZhiSubModel = "pinzhi_kapai";

            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_HERO] = new CardModelItem();
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_HERO].m_path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "CardModel.prefab");
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_HERO].m_headerSubModel = "gaibangzhutu_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_HERO].m_frameSubModel = "paidi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_HERO].m_yaoDaiSubModel = "mingzidi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_HERO].m_pinZhiSubModel = "pinzhi_kapai";

            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_SKILL] = new CardModelItem();
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_SKILL].m_path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "CardModel.prefab");
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_SKILL].m_headerSubModel = "gaibangzhutu_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_SKILL].m_frameSubModel = "paidi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_SKILL].m_yaoDaiSubModel = "mingzidi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_SKILL].m_pinZhiSubModel = "pinzhi_kapai";

            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_LUCK_COINS] = new CardModelItem();
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_LUCK_COINS].m_path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "CardModel.prefab");
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_LUCK_COINS].m_headerSubModel = "gaibangzhutu_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_LUCK_COINS].m_frameSubModel = "paidi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_LUCK_COINS].m_yaoDaiSubModel = "mingzidi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_LUCK_COINS].m_pinZhiSubModel = "pinzhi_kapai";

            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_NEW1] = new CardModelItem();
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_NEW1].m_path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "CardModel.prefab");
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_NEW1].m_headerSubModel = "gaibangzhutu_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_NEW1].m_frameSubModel = "paidi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_NEW1].m_yaoDaiSubModel = "mingzidi_kapai";
            m_sceneCardModelAttrItemList[(int)CardType.CARDTYPE_NEW1].m_pinZhiSubModel = "pinzhi_kapai";

            m_costModelAttrItem.m_path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "cost.prefab");
            m_enemyCardModelAttrItem.m_path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "enemycard.prefab");
            m_minionModelAttrItem.m_path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "minion.prefab");
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
        }

        public void psstRetCardGroupListInfoUserCmd(List<t_group_list> info)
        {
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
    }
}
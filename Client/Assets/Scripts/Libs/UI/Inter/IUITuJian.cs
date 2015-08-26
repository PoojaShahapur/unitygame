using System.Collections.Generic;
namespace SDK.Lib
{
    public interface IUITuJian : IUIBase
    {
        void showUI();
        // 所有卡牌
        void psstNotifyAllCardTujianInfoCmd();
        // 新增\数量改变,不包括删除, badd 指明是添加还是改变
        void psstNotifyOneCardTujianInfoCmd(uint id, byte num, bool badd);
        // 套牌列表
        void psstRetCardGroupListInfoUserCmd();
        // 一个套牌的卡牌列表，index 指明是哪个套牌的
        void psstRetOneCardGroupInfoUserCmd(uint index, List<uint> list);
        // 新添加一个套牌
        void psstRetCreateOneCardGroupUserCmd(CardGroupItem cardGroup);
        // 删除一个套牌
        void psstRetDeleteOneCardGroupUserCmd(int index);
        void newCardSet(CardGroupItem cardGroup, bool bEnterEdit = true);
        // 保存卡牌成功
        void psstRetSaveOneCardGroupUserCmd(uint index);
        void delOneCardGroup(int idx);
        void toggleCardVisible(bool bShow);
        void editCurCardSet();
        void delCardSet();
        void addCurCard2CardSet();
        void updateMidCardModel();
        void updateByCareer(int idx);
        void updateFilter(int idx);
        // 返回是否在编辑模式
        bool bInEditMode();
        // 返回编辑的套牌
        uint getEditCareerID();
    }
}
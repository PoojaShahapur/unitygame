using Game.Msg;
using Game.UI;
using SDK.Lib;
using System.Collections.Generic;

namespace SDK.Common
{
    /**
     * @brief 场景商店
     */
    public interface IUISceneWDSC : ISceneForm
    {
        void showUI();
        void psstRetCardGroupListInfoUserCmd();
        void psstRetOneCardGroupInfoUserCmd(uint index, List<uint> list);
        void psstRetCreateOneCardGroupUserCmd(CardGroupItem cardGroup);
        void psstNotifyAllCardTujianInfoCmd();
        void psstNotifyOneCardTujianInfoCmd(uint id, byte num, bool badd);
        void psstRetDeleteOneCardGroupUserCmd(uint index);
        void psstRetSaveOneCardGroupUserCmd(uint index);

        void newcardset(CardGroupItem cardGroup, bool bEnterEdit = true);
        void editset();
        void hidenewCardSet();
        void hideAllCard();
        void classfilterhide(EnPlayerCareer c);
        void onclass(EnPlayerCareer myclass);
        void classfilter_gotoback();
        void newCardSet_goback();
        void cardset_goback();
        void reqSaveCard();
    }
}
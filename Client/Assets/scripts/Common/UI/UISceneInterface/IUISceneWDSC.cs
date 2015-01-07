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
        void psstRetCreateOneCardGroupUserCmd(stRetCreateOneCardGroupUserCmd msg);
        void psstNotifyAllCardTujianInfoCmd();
        void psstNotifyOneCardTujianInfoCmd(uint id, byte num, bool badd);
        void psstRetDeleteOneCardGroupUserCmd(uint index);
        void psstRetSaveOneCardGroupUserCmd(uint index);

        void newcardset(CardClass c);
        void editset();
        void hide();
        void hideAllCard();
        void classfilterhide(CardClass c);
        void onclass(CardClass myclass);
        void classfilter_gotoback();
        void newCardSet_goback();
        void cardset_goback();
        void reqSaveCard();
    }
}
using Game.Msg;
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
    }
}
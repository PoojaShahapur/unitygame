﻿namespace SDK.Lib
{
    public interface IUIOpenPack : IUIBase
    {
        void updateData();
        void psstRetGiftBagCardsDataUserCmd(params uint[] idList);
        void updatePackNum();
    }
}
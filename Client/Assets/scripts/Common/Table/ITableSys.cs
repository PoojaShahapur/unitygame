﻿namespace SDK.Common
{
    public interface ITableSys
    {
        TableItemBase getItem(TableID tableID, uint itemID);
    }
}
--功能性的必须加载的文件

-- Tab 控件
MLoader("MyLua.Libs.UI.TabPageMgr.TabPage");
MLoader("MyLua.Libs.UI.TabPageMgr.TabPageMgr");

MLoader("MyLua.Libs.UI.AreaWidget.AreaViewItem");
MLoader("MyLua.Libs.UI.AreaWidget.AreaWidget");

--Common 
MLoader("MyLua.Libs.UI.Common.CirclePanelOpenType");
MLoader("MyLua.Libs.GameData.Object.MoneyType");

-- 根据道具 Id 显示图像
MLoader("MyLua.Libs.UI.UIMisc.AuxObjectImage");
MLoader("MyLua.Libs.UI.UIMisc.AuxSimpleAtlasImage");

-- Item 显示
MLoader("MyLua.Libs.UI.ObjectItem.ObjectViewCV");
MLoader("MyLua.Libs.UI.ObjectItem.ItemViewBase");
MLoader("MyLua.Libs.UI.ObjectItem.ObjectViewItem");
MLoader("MyLua.Libs.UI.ObjectItem.SkinViewItem");

-- 数据基类
MLoader("MyLua.Libs.GameData.Object.ObjectItemBase");
MLoader("MyLua.Libs.GameData.Object.ObjectPanelBase");

-- 道具基本数据
MLoader("MyLua.Libs.GameData.Object.DataItemType");
MLoader("MyLua.Libs.GameData.Object.ObjectType");
MLoader("MyLua.Libs.GameData.Object.ObjectPanelType");
MLoader("MyLua.Libs.GameData.Object.ObjectItem");
MLoader("MyLua.Libs.GameData.Object.ObjectPanel");

-- 商店
MLoader("MyLua.Libs.GameData.Object.ShopPanel");
MLoader("MyLua.Libs.GameData.Object.ShopTopPanel");
MLoader("MyLua.Libs.GameData.Object.ShopTopType");
MLoader("MyLua.Libs.GameData.Object.ShopDataItem");
MLoader("MyLua.Libs.UI.ObjectItem.ShopViewItem");

-- 皮肤
MLoader("MyLua.Libs.GameData.Object.SkinPanel");
MLoader("MyLua.Libs.GameData.Object.SkinItem");
MLoader("MyLua.Libs.GameData.Object.SkinPiFuItem");
MLoader("MyLua.Libs.GameData.Object.SkinBulletItem");
MLoader("MyLua.Libs.GameData.Object.SkinClientItem");
MLoader("MyLua.Libs.GameData.Object.SkinPiFuClientItem");
MLoader("MyLua.Libs.GameData.Object.SkinBulletClientItem");

--签到数据
MLoader("MyLua.Libs.GameData.Sign.MonthSignInfo");

-- 各种数据
MLoader("MyLua.Libs.GameData.GameData");
MLoader("MyLua.Libs.GameData.GoodsData");
MLoader("MyLua.Libs.GameData.SignData");
MLoader("MyLua.Libs.GameData.PackData");
MLoader("MyLua.Libs.GameData.ShopData");
MLoader("MyLua.Libs.GameData.HeroData");
MLoader("MyLua.Libs.GameData.SkinData");
MLoader("MyLua.Libs.GameData.PlayerData");
MLoader("MyLua.Libs.GameData.SocialData");
MLoader("MyLua.Libs.GameData.TeamData");

--轨迹动画
MLoader("MyLua.Libs.UI.UIMoveAni.FlyPos");
MLoader("MyLua.Libs.UI.UIMoveAni.FlyUFO");
MLoader("MyLua.Libs.UI.UIMoveAni.UFOMgr");
MLoader("MyLua.Libs.UI.UIMoveAni.FlyItem");
MLoader("MyLua.Libs.UI.UIMoveAni.FlyItemMgr");

--表格加载
MLoader("MyLua.LuaTable.skin");
MLoader("MyLua.LuaTable.object");

--语言
MLoader("MyLua.Libs.Lang.Config.zh_CN");
--新手引导
MLoader("MyLua.Libs.BeginnerGuide.GuideTypeId");
MLoader("MyLua.Libs.BeginnerGuide.BeginnerGuideSys");

--数字动画
MLoader("MyLua.Libs.AnimSys.NumNodeAnim.NumIncOrDecAnimMode");
MLoader("MyLua.Libs.AnimSys.NumNodeAnim.NumNodeAniBase");
MLoader("MyLua.Libs.AnimSys.NumNodeAnim.NumIncOrDecAnim");
MLoader("MyLua.Libs.AnimSys.NumNodeAnim.CanvasGroupAlphaAnim");
MLoader("MyLua.Libs.AnimSys.NumNodeAnim.NumNodeAnimSys");

--每日任务
MLoader("MyLua.Libs.GameData.DailyTask.DailyTaskTypeId");
MLoader("MyLua.Libs.GameData.DailyTask.ActivityRewardTypeId");
MLoader("MyLua.Libs.GameData.DailyTask.DailyTaskItemData");
MLoader("MyLua.Libs.GameData.DailyTask.ActivityRewardItemData");
MLoader("MyLua.Libs.GameData.DailyTask.DailyTaskData");
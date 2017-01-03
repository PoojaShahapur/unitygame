--region *.lua
--Date
--此文件由[BabeLua]插件自动生成
local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "GoodsData";
GlobalNS[M.clsName] = M;

function M:ctor(...)
    self.CurrentShopType = GlobalNS.UIFormId.eUIShop_SkinPanel;
    self.goodsCount = 0; --物品数量
    self.goodsitems = {}; --物品

    self.shapeCount = 0;
    self.shapeitems = {};
    self.childCount = 0;
    self.childitems = {};
end

function M:dtor()

end

function M:init()
    --外形
    self.shapeCount = GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.shape.Length;
    for i=1, self.shapeCount do
        self.shapeitems[i] = 
        {
            m_goodsID = GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.shape[i-1].mID;
            m_goodsName = GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.shape[i-1].mName;
            m_needID = GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.shape[i-1].mNeedID;
            m_needNum = GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.shape[i-1].mNeedNum;
            m_hot = GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.shape[i-1].mHot;
            m_only = GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.shape[i-1].mOnly;
        };
    end

    --鱼仔
    self.childCount = GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.child.Length;
    for i=1, self.childCount do
        self.childitems[i] = 
        {
            m_goodsID = GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.child[i-1].mID;
            m_goodsName = GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.child[i-1].mName;
            m_needID = GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.child[i-1].mNeedID;
            m_needNum = GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.child[i-1].mNeedNum;
            m_hot = GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.child[i-1].mHot;
            m_only = GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.child[i-1].mOnly;
        };
    end
end

function M:getGoodsItems(shopType, goodsType, args)
    local isOwn = false;
    if nil ~= args then --自身已有的物品从服务器消息获取
        if shopType ~= GCtx.mGoodsData.CurrentShopType then --服务器回复过迟
            return;
        else
            --处理服务器发来的个人物品数据
            self:SetGoodsItemsFromServer(goodsType, args);
            isOwn = true;
        end
    else
        self:SetGoodsItems(goodsType);
        isOwn = false;
    end

    local form = GCtx.mUiMgr:getForm(self.CurrentShopType);
    if nil ~= form and form.mIsReady then
        form:updateUIData(isOwn);
    end
end

function M:SetGoodsItems(goodsType)
    if self.CurrentShopType == GlobalNS.UIFormId.eUIShop_SkinPanel then
        if goodsType == 1 then --外形
            self.goodsCount = self.shapeCount;
            self.goodsitems = self.shapeitems;
        else --鱼仔
            self.goodsCount = self.childCount;
            self.goodsitems = self.childitems;
        end
    else
        --圣衣
    end    
end

function M:SetGoodsItemsFromServer(goodsType, args)
    if self.CurrentShopType == GlobalNS.UIFormId.eUIShop_SkinPanel then
        if goodsType == 1 then --外形
        else --鱼仔
        end
    else
        --圣衣
    end    
end

return M;


--endregion

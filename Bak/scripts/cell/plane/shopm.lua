--[[
商店管理器
跟仙侠一样,策划填好商品,商店就出来了。
但仙侠是访问Npc调用对应哪个的商店,手游是点击对应的标签页打开对应的商店

商店里,有道具解锁等级,未达到等级的,客户端暂未开放。

注意压缩传输吧,加密到不至于,玩家知道了,也没意义。

如何增加一个商店?
1. 换一个没用过的商店id
2. 策划配置该商店售卖的道具,价格
3. 客户端界面热更,客户端lua里写上新的分栏id, 客户端显示新的分栏id

c->s,请求某商店id的道具列表
s->c,返回所有道具
c->s,请求购买index的物品

--]]

-- items 代表所有该商店售卖的道具
-- pos 代表该物品在商店摆放的位置,必须保证不重复
-- baseid 代表道具表中的道具id;
-- money, ticket, plastic 分别代表购买该道具需要的 饼干(money游戏里面获得的),糖果(ticket分享获得的货币),弹珠(plastic人民币充值的)
-- [0,1000]留给皮肤分栏;[1001,2000]留给宝箱分栏...
local log = require("log"):new("plane.shopm")
local pb = require("protobuf")
local all_shops = {
    -- shopid=1,代表客户端 皮肤-幽浮 分栏下售卖的道具
    {
        shopid = 0 * 1000 + 1,
        items = {
            { pos = 1, baseid = 10006, money = 0, ticket = 999999, plastic = 0 },
            { pos = 2, baseid = 10002, money = 0, ticket = 20, plastic = 0 },
            { pos = 3, baseid = 10003, money = 0, ticket = 30, plastic = 0 },
            { pos = 4, baseid = 10004, money = 0, ticket = 50, plastic = 0 },
            { pos = 5, baseid = 10005, money = 0, ticket = 100, plastic = 0 },
            { pos = 6, baseid = 10012, money = 0, ticket = 120, plastic = 0 },
            { pos = 7, baseid = 10014, money = 0, ticket = 150, plastic = 0 },
            { pos = 8, baseid = 10007, money = 0, ticket = 300, plastic = 0 },
            { pos = 9, baseid = 10011, money = 0, ticket = 500, plastic = 0 },

        }
    },
    -- shopid=2,代表客户端 皮肤-子弹 分栏下售卖的道具
    {
        shopid = 0 * 1000 + 2,
        items = {
            { pos = 1, baseid = 20019, money = 0, ticket = 999999, plastic = 0 },
            { pos = 2, baseid = 20003, money = 0, ticket = 20, plastic = 0 },
            { pos = 3, baseid = 20004, money = 0, ticket = 20, plastic = 0 },
            { pos = 4, baseid = 20002, money = 0, ticket = 30, plastic = 0 },
            { pos = 5, baseid = 20016, money = 0, ticket = 60, plastic = 0 },
            { pos = 6, baseid = 20007, money = 0, ticket = 80, plastic = 0 },
            { pos = 7, baseid = 20011, money = 0, ticket = 100, plastic = 0 },
            { pos = 8, baseid = 20010, money = 0, ticket = 120, plastic = 0 },
            { pos = 9, baseid = 20009, money = 0, ticket = 150, plastic = 0 },
        }
    }
}

local Shopm = {
    shops = {},
}

function Shopm.load_all_shop()
    for k,v in pairs(all_shops) do
        -- 遍历加载每个商店
        local one_shop = {}
        local shopid = v.shopid
        if Shopm.shops[shopid] ~= nil then
            assert('策划商店id配重复了！！Shopm:load_all_shop failed')
            break
        end

        for _,item in pairs(v.items) do
            local itempos = item.pos
            if one_shop[itempos] ~= nil then
                assert('策划id=' .. shopid .. '配重复了pos=' .. itempos .. '！！Shopm:load_all_shop failed')
                break
            end
            one_shop[itempos] = item
        end

        Shopm.shops[shopid] = one_shop
    end
end

-- 将所有商店内的道具,打包发送给客户端
function Shopm.send_all_items_in_shop(shopid, user)
    local msg = {
        reqshopid = shopid,
        allitems = {},
    }
    if Shopm.shops[shopid] == nil then
        return
    end

    for k,v in pairs(Shopm.shops[shopid]) do
        local one_item = {
            pos = k,
            baseid = v.baseid,
            money = v.money,
            ticket = v.ticket,
            plastic = v.plastic,
        }
        table.insert(msg.allitems, one_item)
    end

    user:rpc_request("plane.ObjectPush", "NotifyAllShopItem", pb.encode("plane.AllShopItemMsg", msg))
end

-- 根据商店id和位置获取道具
function Shopm.get_item(shopid, pos)
    local shop = Shopm.shops[shopid]
    if shop == nil then
        return
    end

    if shop[pos] == nil then
        return
    end

    return shop[pos]
end

return Shopm

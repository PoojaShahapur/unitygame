local log = require('log'):new('excelm')

-- 超过这个类型,都代表皇冠段位,
-- 只有奖励的区分,但显示上都要显示皇冠+具体等级
local max_level_type = 16

local Excelm = {
    objectbm = {},
    skinbm = {
        multiskin = {},--一个skinid对应所有的皮肤id
    },
    levelbm = { },--key:段位id, value:该段位id对应的段位数据
    -- 每个段位类型的最大星 对应的段位id
    -- key:段位类型;value:最大星的段位id
    leveltype2id = {},
    maxlevelid = 0,
}

function Excelm:load_objectbm()
    self.objectbm = {}
    local object_t = c_csv.CsvTable("object_object.csv")
    local all = object_t:get_all_records()
    for k,v in pairs(all) do
        local object = {}
        object.baseid = v['id']
        object.name = v['name']
        object.type = v['type']
        object.subtype = v['subtype']
        object.time_limit = v['time_limit']
        object.activeskinid = v['activeskinid']
        object.plastic = v['plastic']
        object.ticket = v['ticket']
        object.money = v['money']
        if self.objectbm[object.baseid] ~= nil then
            assert(false, '策划吧道具id=' .. object.baseid .. '配重复了')
        end
        self.objectbm[object.baseid] = object
    end
end

function Excelm:load_skinbm()
    self.skinbm = {
        multiskin = {},--一个skinid对应所有的皮肤id
    }
    local skin_t = c_csv.CsvTable("skin_skin.csv")
    local all = skin_t:get_all_records()
    for k,v in pairs(all) do
        local one_skindata = {}
        local id = v['id']
        one_skindata.id = id
        one_skindata.skinid = tonumber(v['skinid'])
        one_skindata.money = tonumber(v['money'])
        one_skindata.tlimit = tonumber(v['time_limit'])
        if one_skindata.tlimit == nil then
            one_skindata.tlimit = 0
        end
        if self.skinbm[id] ~= nil then
            assert(false, '策划吧皮肤id=' .. id .. '配重复了')
        end
        self.skinbm[id] = one_skindata
        if self.skinbm.multiskin[one_skindata.skinid] == nil then
            --log:info('tbl=nil, load skin,id=%d,skinid=%s,money=%s,tlimit=%s'
            --    ,id, tostring(v['skinid']), tostring(one_skindata.money), tostring(one_skindata.tlimit))
            self.skinbm.multiskin[one_skindata.skinid] = {}
            table.insert(self.skinbm.multiskin[one_skindata.skinid], one_skindata)
        else
            --log:info('tbl~=nil, load skin,id=%d,skinid=%s,money=%s,tlimit=%s'
            --    ,id, tostring(v['skinid']), tostring(one_skindata.money), tostring(one_skindata.tlimit))
            table.insert(self.skinbm.multiskin[one_skindata.skinid], one_skindata)
        end
    end

    for k,v in pairs(self.skinbm.multiskin) do
        log:info('load multi,k=%d', k)
    end
end

function parse_level_range(range_str)
    local key,value = range_str:match('([^-]+)-([^-]+)')
    if key == nil or value == nil then
        assert(false, '段位表范围解析失败,' .. range_str)
    end
    tbl = {rbegin = tonumber(key), rend = tonumber(value)}
    return tbl
end

-- 个人模式,根据旧段位,以及本局排名,计算出新段位
function Excelm:get_newlevel_by_personal_rank(oldlevel, rank)
    local config = self.levelbm[oldlevel]
    if config == nil then
        return 0
    end
    local maxstar_id = self.leveltype2id[config.type]
    if maxstar_id == nil then
        return 0
    end
    -- 判断名次是在 升,保,还是降星范围
    if rank >= config.free_raise.rbegin and rank <= config.free_raise.rend then
        -- 判断是否是该类型的满星
        local newlevel = oldlevel + 1
        -- 只有处在type=16皇冠以下,升段的时候才能连跳2级
        if oldlevel == maxstar_id and config.type < max_level_type then
            newlevel = oldlevel + 2
        end
        return (newlevel <= self.maxlevelid and newlevel or self.maxlevelid)
    elseif rank >= config.free_retain.rbegin and rank <= config.free_retain.rend then
        return oldlevel
    elseif rank >= config.free_down.rbegin and rank <= config.free_down.rend then
        -- 需要降星,这里没判断小于0,策划配置保证新手段位只会升/保,而不会降段位
        local newlevel = oldlevel - 1
        -- 只有处在type=16的皇冠段位及以下,降段的时候才会连跳2级,否则只降1级
        if config.star == 0 and config.type <= max_level_type then
            newlevel = oldlevel - 2
        end
        return (newlevel >= 1 and newlevel or 1)
    else
        return 0
    end
end

-- 团队模式
function Excelm:get_newlevel_by_team_rank(oldlevel, rank)
    local config = self.levelbm[oldlevel]
    if config == nil then
        return 0
    end
    local maxstar_id = self.leveltype2id[config.type]
    if maxstar_id == nil then
        return 0
    end
    -- 判断名次是在 升,保,还是降星范围
    if rank >= config.team_raise.rbegin and rank <= config.team_raise.rend then
        -- 判断是否是该类型的满星
        local newlevel = oldlevel + 1
        -- 只有处在type=16皇冠以下,升段的时候才能连跳2级
        if oldlevel == maxstar_id and config.type < max_level_type then
            newlevel = oldlevel + 2
        end
        return (newlevel <= self.maxlevelid and newlevel or self.maxlevelid)
    elseif rank >= config.team_retain.rbegin and rank <= config.team_retain.rend then
        return oldlevel
    elseif rank >= config.team_down.rbegin and rank <= config.team_down.rend then
        -- 需要降星,这里没判断小于0,策划配置保证新手段位只会升/保,而不会降段位
        local newlevel = oldlevel - 1
        -- 只有处在type=16的皇冠段位及以下,降段的时候才会连跳2级,否则只降1级
        if config.star == 0 and config.type <= max_level_type then
            newlevel = oldlevel - 2
        end
        return (newlevel >= 1 and newlevel or 1)
    else
        return 0
    end
end

function Excelm:load_levelbm()
    self.levelbm = {}
    self.leveltype2id = {}
    self.maxlevelid = 0
    local level_t = c_csv.CsvTable("level_level.csv")
    local all = level_t:get_all_records()
    for k,v in pairs(all) do
        local leveldata = {}
        leveldata.id = v['id']
        leveldata.type = v['type']
        leveldata.star = v['star']
        if self.leveltype2id[leveldata.type] == nil then
            self.leveltype2id[leveldata.type] = leveldata.id
        else
            local olddataid = self.leveltype2id[leveldata.type]
            if leveldata.star > self.levelbm[olddataid].star then
                self.leveltype2id[leveldata.type] = leveldata.id
            end
        end
        if self.maxlevelid < leveldata.id then
            self.maxlevelid = leveldata.id
        end
        leveldata.name = v['name']
        leveldata.team_raise = parse_level_range(v['c_raise'])
        leveldata.team_retain = parse_level_range(v['c_retain'])
        leveldata.team_down = parse_level_range(v['c_down'])
        leveldata.free_raise = parse_level_range(v['f_raise'])
        leveldata.free_retain = parse_level_range(v['f_retain'])
        leveldata.free_down = parse_level_range(v['f_down'])
        if self.levelbm[leveldata.id] ~= nil then
            assert(false, '策划吧段位id=' .. id .. '配重复了')
        end
        self.levelbm[leveldata.id] = leveldata
    end
    -- 相当于单元测试
    -- 个人模式第18名,从1段升到2段
    assert(self:get_newlevel_by_personal_rank(1,18) == 2)
    assert(self:get_newlevel_by_personal_rank(1,19) == 1)
    -- 每个类型的段位最高星,在胜利后,突破到下个类型段位的1星(即连升2级)
    assert(self:get_newlevel_by_personal_rank(3,18) == 5)
    assert(self:get_newlevel_by_personal_rank(66,1) == 68)
    assert(self:get_newlevel_by_personal_rank(67,10) == 67)
    -- 0星降段的话降2级
    assert(self:get_newlevel_by_personal_rank(67,30) == 65)
    assert(self:get_newlevel_by_personal_rank(68,30) == 67)
end


return Excelm

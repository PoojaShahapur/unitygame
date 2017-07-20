local pb = require("protobuf")
local serpent = require('serpent')

local Config = {
    rankreward = {},--排行榜奖励字典,key:名次;value:奖励的糖果盒饼干数量
    team_rankreward = {},
    --炼狱模式 结算界面的奖励
    purgatory_reward = {},
    purgatory_rankreward = {},
    season_reward = {},
    season_level_reward_pbstr = '',
}

-- 普通模式结算的奖励配置
local rank_reward_config = {
    {rankstart = 1, rankend = 1, sugar = 0, cookie = 5},--第1-1名,给5个糖果,3个饼干
    {rankstart = 2, rankend = 2, sugar = 0, cookie = 3},--第2-2名
    {rankstart = 3, rankend = 3, sugar = 0, cookie = 2},
    {rankstart = 4, rankend = 4, sugar = 0, cookie = 1},
    {rankstart = 5, rankend = 5, sugar = 0, cookie = 1},
    {rankstart = 6, rankend = 10, sugar = 0, cookie = 1},
    {rankstart = 11, rankend = 25, sugar = 0, cookie = 0},
}

-- 团战模式结算的奖励配置
local team_rank_reward_config = {
    {rankstart = 1, rankend = 1, sugar = 0, cookie = 5},--第1-1名,给5个糖果,3个饼干
    {rankstart = 2, rankend = 2, sugar = 0, cookie = 3},--第2-2名
    {rankstart = 3, rankend = 3, sugar = 0, cookie = 2},
    {rankstart = 4, rankend = 4, sugar = 0, cookie = 1},
    {rankstart = 5, rankend = 5, sugar = 0, cookie = 1},
    {rankstart = 6, rankend = 10, sugar = 0, cookie = 1},
    {rankstart = 11, rankend = 25, sugar = 0, cookie = 0},
}

-- 炼狱模式每局奖励
local purgatory_reward_config = {
    {rankstart = 1, rankend = 1, sugar = 0, cookie = 10},--第1-1名,给5个糖果,3个饼干
    {rankstart = 2, rankend = 2, sugar = 0, cookie = 7},--第2-2名
    {rankstart = 3, rankend = 3, sugar = 0, cookie = 5},
    {rankstart = 4, rankend = 4, sugar = 0, cookie = 3},
    {rankstart = 5, rankend = 5, sugar = 0, cookie = 3},
    {rankstart = 6, rankend = 10, sugar = 0, cookie = 2},
    {rankstart = 11, rankend = 20, sugar = 0, cookie = 1},
    {rankstart = 21, rankend = 25, sugar = 0, cookie = 0},
}

--炼狱总排行榜奖励
local purgatory_rank_reward_config = {
    {rankstart = 1, rankend = 1, sugar = 30 },--第1-1名
    {rankstart = 2, rankend = 2, sugar = 20 },--第2-2名
    {rankstart = 3, rankend = 3, sugar = 15 },
    {rankstart = 4, rankend = 4, sugar = 10 },
    {rankstart = 5, rankend = 5, sugar = 10 },
    {rankstart = 6, rankend = 10, sugar = 5},
    {rankstart = 11, rankend = 20, sugar = 2},
    {rankstart = 21, rankend = 100, sugar = 0},
}

-- 每赛季段位奖励
-- 切记,每个reward的大括号后面都加逗号！！！
local season_level_reward_config = {
    -- 16为段位表中段位类型,代表皇冠段位
    [1] = {
        {objid = 2, num=1},
    },
    [2] = {
        {objid = 2, num=5},
    },
    [3] = {
        {objid = 2, num=10},
    },
    [4] = {
        {objid = 2, num=15},
    },
    [5] = {
        {objid = 2, num=20},
    },
    [6] = {
        {objid = 2, num=30},
    },
    [7] = {
        {objid = 2, num=40},
    },
    [8] = {
        {objid = 2, num=50},
    },
    [9] = {
        {objid = 2, num=60},
    },
    [10] = {
        {objid = 2, num=70},
    },
    [11] = {
        {objid = 2, num=90},
    },
    [12] = {
        {objid = 20019, num=1},
        {objid = 2, num=110},
    },
    [13] = {
        {objid = 20019, num=1},
        {objid = 2, num=130},
    },
    [14] = {
        {objid = 20019, num=1},
        {objid = 2, num=150},
    },
    [15] = {
        {objid = 20019, num=1},
        {objid = 2, num=180},
    },
    [16] = {
        {objid = 10006, num=1},
        {objid = 20019, num=1},
        {objid = 2, num=210},
    },
    [17] = {
        {objid = 10006, num=1},
        {objid = 20019, num=1},
        {objid = 2, num=240},
    },
    [18] = {
        {objid = 10006, num=1},
        {objid = 20019, num=1},
        {objid = 2, num=270},
    },
    [19] = {
        {objid = 10006, num=1},
        {objid = 20019, num=1},
        {objid = 2, num=300},
    },
    [20] = {
        {objid = 10006, num=1},
        {objid = 20019, num=1},
        {objid = 2, num=330},
    },
    [21] = {
        {objid = 10006, num=1},
        {objid = 20019, num=1},
        {objid = 2, num=360},
    },
    [22] = {
        {objid = 10006, num=1},
        {objid = 20019, num=1},
        {objid = 2, num=390},
    },
    [23] = {
        {objid = 10006, num=1},
        {objid = 20019, num=1},
        {objid = 2, num=420},
    },
    [24] = {
        {objid = 10006, num=1},
        {objid = 20019, num=1},
        {objid = 2, num=450},
    },
    [25] = {
        {objid = 10006, num=1},
        {objid = 20019, num=1},
        {objid = 2, num=480},
    },
    [26] = {
        {objid = 10006, num=1},
        {objid = 20019, num=1},
        {objid = 2, num=500},
    },
}

function get_min_max_id(all_records)
    local minid = math.maxinteger
    local maxid = math.mininteger
    for k,v in pairs(all_records) do
        local id = v['ID'] 
        if id < minid then
            minid = id
        end
        if id > maxid then
            maxid = id
        end
    end

    return minid, maxid
end

function Config:loadconfig()
    local common_t = c_csv.CsvTable("param_Common.csv")
    local basic_t = c_csv.CsvTable("param_SnowBallBasic.csv")
    local width = tonumber(common_t:get_record({key = 'Map_Width'})['value'])
    self.x_min = 32
    self.x_max = width - 32
    --self.x_max = tonumber(width['value'])
    local height = tonumber(common_t:get_record({key = 'Map_Height'})['value'])
    self.y_min = 32
    self.y_max = height - 32
    --需要击中2架飞机才能给自己增加1架
    self.need_shot_num = tonumber(common_t:get_record({key = 'need_shot_num'})['value'])
    --房间相关配置, hold_num代表房间可容纳最大人数;last_seconds 代表房间持续时间
    self.room = {hold_num = tonumber(common_t:get_record({key = 'room_hold_num'})['value'])
        , last_seconds = tonumber(common_t:get_record({key = 'room_last_seconds'})['value'])
        , food_num = tonumber(common_t:get_record({key = 'food_num'})['value'])
        , forbid_join_seconds = tonumber(common_t:get_record({key = 'forbid_join_seconds'})['value'])
    }
    self.purgatory_room_last_seconds = tonumber(common_t:get_record({key = 'purgatory_room_last_seconds'})['value'])
    --self.y_max = tonumber(height['value'])
    self.movespeed_k = tonumber(basic_t:get_record({key = 'MoveSpeed_k'})['value'])
    self.movespeed_b = tonumber(basic_t:get_record({key = 'MoveSpeed_b'})['value'])
    self.movespeed_a = tonumber(basic_t:get_record({key = 'MoveSpeed_a'})['value'])
    self.rank_show_num = 8 --排行榜上显示的玩家数量
    self.born_pos = {x = 70, y = 70}
    
    --子弹速度在玩家速度基础上加成的百分比
    self.bullet_speed_coef = tonumber(basic_t:get_record({key = 'bullet_speed_coef'})['value'])
    self.bullet_life_seconds = tonumber(basic_t:get_record({key = 'bullet_life_seconds'})['value'])
    -- 从被团灭到弹出复活界面的时间间隔
    self.relive_seconds = tonumber(basic_t:get_record({key = 'relive_seconds'})['value'])
    -- 无敌持续的时间
    self.god_last_seconds = tonumber(basic_t:get_record({key = 'god_last_seconds'})['value'])
    
    -- 每个玩家出生的小飞机个数
    self.player_born_plane_count = tonumber(basic_t:get_record({key = 'player_born_plane_num'})['value'])
    -- 最大可达到的飞机数量
    self.max_plane_num = tonumber(basic_t:get_record({key = 'player_max_plane_num'})['value'])
    self.ai_max_plane_num = tonumber(basic_t:get_record({key = 'ai_max_plane_num'})['value'])
    -- ai 玩家变化方向的频率,
    self.change_direction_seconds = tonumber(basic_t:get_record({key = 'ai_change_direction_seconds'})['value'])

    self.lifetime_k = tonumber(basic_t:get_record({key = 'lifetime_k'})['value'])
    self.lifetime_a = tonumber(basic_t:get_record({key = 'lifetime_a'})['value'])
    self.lifetime_b = tonumber(basic_t:get_record({key = 'lifetime_b'})['value'])
    
    
    self.shot_cd = {
        min_seconds = tonumber(basic_t:get_record({key = 'shot_minseconds'})['value'])
        , max_seconds = tonumber(basic_t:get_record({key = 'shot_maxseconds'})['value'])
        , add_coef = tonumber(basic_t:get_record({key = 'shot_add_coef'})['value'])
        , delay_seconds = tonumber(basic_t:get_record({key = 'shot_delayseconds'})['value'])
    }
    
    -- 分裂相关配置,注意 分裂cd 一定要大于 分裂持续时间,否则服务器无法正常工作
    self.split_cd = tonumber(basic_t:get_record({key = 'split_cd'})['value'])
    self.split_speed_k = tonumber(basic_t:get_record({key = 'split_speed_factor'})['value'])
    self.split_last_seconds = tonumber(basic_t:get_record({key = 'split_last_seconds'})['value'])
    
        -- 飞机模型的半径
        self.modle_radius = 0.55
        self.circle_modle_radius = 0.6
    
    -- 玩家皮肤id集合,玩家进入时候随机
    local scene_t = c_csv.CsvTable("scene_plane.csv")
    local all = scene_t:get_all_records()
    local minid, maxid = get_min_max_id(all)
    self.skinid = {min = minid
        , max = maxid
    }
    scene_t = c_csv.CsvTable("scene_bullet.csv")
    all = scene_t:get_all_records()
    local minid, maxid = get_min_max_id(all)
    self.bulletskinid = {min = minid, max = maxid}

    -- 加载结算奖励字典
    for k,v in pairs(rank_reward_config) do
        for rank = v.rankstart, v.rankend do
            self.rankreward[rank] = {sugar = v.sugar, cookie = v.cookie}
        end
    end
    for k,v in pairs(team_rank_reward_config) do
        for rank = v.rankstart, v.rankend do
            self.team_rankreward[rank] = {sugar = v.sugar, cookie = v.cookie}
        end
    end
    for k,v in pairs(purgatory_reward_config) do
        for rank = v.rankstart, v.rankend do
            self.purgatory_reward[rank] = {sugar = v.sugar, cookie = v.cookie}
        end
    end

    -- 炼狱榜>0的排名个数(0点发奖励用到)
    self.purgatory_rank_reward_num = 0
    for k,v in pairs(purgatory_rank_reward_config) do
        for rank = v.rankstart, v.rankend do
            self.purgatory_rankreward[rank] = v.sugar
            self.purgatory_rank_reward_num = self.purgatory_rank_reward_num + 1
        end
    end

    -- 每个人点击赛季奖励发的奖励都是一样的,所以在这里做个缓存,直接encode出str
    local reward = {
        rewards = {},
    }
    for k,v in pairs(season_level_reward_config) do
        local one_level_reward = {
            type = k,
            objs = {}
        }
        for _,obj in pairs(v) do
            table.insert(one_level_reward.objs, {baseid = obj.objid, num = obj.num})
        end
        table.insert(reward.rewards, one_level_reward)
    end
    self.season_reward = season_level_reward_config
    self.season_level_reward_pbstr = pb.encode("plane.SeasonRewardMsg", reward)

-- 移动中发射子弹,将子弹组的位置按玩家速度往后拉100ms
--move_shot_delay_seconds = 0.11,
end

return Config

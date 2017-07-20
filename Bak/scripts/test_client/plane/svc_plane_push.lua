-- svc_plane_push.lua
-- PlanePush服务

local M = { }

local log = require("log"):new("plane.push_svc")
local pb = require("protobuf")
local serpent = require("serpent")
local empty_msg_str = pb.encode("rpc.EmptyMsg", { })
local test_plane = require("plane.test_plane")
local inspect = require("inspect")  -- luacheck: ignore

log:debug("loading service...")

local function reply_empty(ctx)
    c_rpc.reply(ctx, empty_msg_str)
end  -- ReplyEmpty()

-- 发射子弹. Todo: check cooldown
function M.Fire(ctx, content)
    log:debug("Fire")
    local req = assert(pb.decode("plane.FireSimpleBcMsg", content))
    print("%s", serpent.block(req))
    reply_empty(ctx)
end  -- Fire()

-- 击中
function M.Hit(ctx, content)
    log:debug("Hit")
    local req = assert(pb.decode("plane.HitBcMsg", content))
    reply_empty(ctx)
end  -- Hit()

-- 吃星星加飞机
function M.Eat(ctx, content)
    log:debug("Eat")
    local req = assert(pb.decode("plane.EatBcMsg", content))
    reply_empty(ctx)
end  -- Eat()

function M.PlayerEnter(ctx, content)
    log:debug("PlayerEnter")
    local req = assert(pb.decode("plane.PlayerInfo", content))
    reply_empty(ctx)
end  -- PlayerEnter()

function M.NewFood(ctx, content)
    log:debug("NewFood")
    local req = assert(pb.decode("plane.FoodMsg", content))
    reply_empty(ctx)
end  -- NewFood()

-- 新增飞机
function M.NewPlane(ctx, content)
    log:debug("NewPlane")
    local req = assert(pb.decode("plane.PlaneBcMsg", content))
    reply_empty(ctx)
end  -- NewPlane()

-- 删除飞机
function M.RemovePlane(ctx, content)
    log:debug("RemovePlane")
    local req = assert(pb.decode("plane.PlaneBcMsg", content))
    reply_empty(ctx)
end  -- RemovePlane()

function M.PlayerExit(ctx, content)
    log:debug("PlayerExit")
    local req = assert(pb.decode("plane.MsAndId", content))
    reply_empty(ctx)
end

function M.PackPlayerMoveTo(ctx, content)
    local req = assert(pb.decode("plane.MoveToBcMsgRoom", content))
    -- log:debug("PackPlayerMoveTo， req=" .. inspect(req))
    reply_empty(ctx)
    local moves = req.moves
    for _, move in ipairs(moves) do
        -- log:debug(i  .. " : " .. inspect(move))
        if move.ms_and_id.id == test_plane.my_id then
            test_plane.on_move_to(move.move_to)
            break
        end
    end
end

function M.HitEnergy(ctx, content)
    reply_empty(ctx)
end

function M.NotifyResultData(ctx, content)
    log:debug("NotifyResultData()")
    c_plane.set_active(false)
end

function M.PlayerStopOrMove(ctx, content)
    log:debug("PlayerStopOrMove()")
end

function M.PlayerStopOrMove(ctx, content)
    log:debug("PlayerStopOrMove()")
end

function M.NotifyRankData(ctx, content)
    log:debug("NotifyRankData()")
end

function M.NotifyMyScore(ctx, content)
    log:debug("NotifyMyScore()")
end

function M.NotifyDeath(ctx, content)
    log:debug("NotifyDeath()")
end

function M.NotifyStateChanged(ctx, content)
    log:debug("XXX()")
end

function M.UpdateSpeed(ctx, content)
    log:debug("UpdateSpeed()")
end

function M.Init()
    require("rpc_request_handler").register_service("plane.PlanePush", M)
end  -- Init()

return M

LuaConfig = LuaConfig or {}

LuaConfig.DailyTaskConfig = {
	--各种配置
	["Misc"] = {
		-- 暗色星星 Id
		["DarkStar"] = 1,
		-- 亮色星星 Id
		["LightStar"] = 2
	},
	-- 活跃度奖励
	["ActivityReward"] = {
		-- 第一个活跃度奖励
		[1] = {
			-- 活跃度值
			["ActivityValue"] = 100,
			-- 奖励道具 Id
			["ObjectId"] = 10001
		},
		[2] = {
			["ActivityValue"] = 300,
			["ObjectId"] = 10001
		},
		[3] = {
			["ActivityValue"] = 500,
			["ObjectId"] = 10001
		},
		[4] = {
			["ActivityValue"] = 750,
			["ObjectId"] = 10001
		},
		[5] = {
			["ActivityValue"] = 1000,
			["ObjectId"] = 10001
		}
	},
	["DailyTask"] = {
		-- 第一个任务
		[1] = {
			-- 任务名字
			["TaskName"] = "aaa",
			-- 增加的活跃度值
			["AddActivityValue"] = 50,
			-- 任务描述
			["TaskDesc"] = "TaskDesc",
			-- 完成次数
			["FinishCount"] = 1,
			--任务图标 Id
			["TaskImage"] = 1,
			-- 任务奖励
			["TaskReward"] = {
				-- 第一个任务奖励
				[1] = {
					-- 奖励道具 Id 
					["ObjectId"] = 10001,
					-- 奖励道具
					["ObjectNum"] = 1
				},
				[2] = {
					["ObjectId"] = 10002,
					["ObjectNum"] = 2
				}
			}
		},
		[2] = {
			["TaskName"] = "aaa",
			["AddActivityValue"] = 50,
			["TaskDesc"] = "TaskDesc",
			["FinishCount"] = 1,
			["TaskImage"] = 1,
			["TaskReward"] = {
				[1] = {
					["ObjectId"] = 10001,
					["ObjectNum"] = 1
				},
				[2] = {
					["ObjectId"] = 10002,
					["ObjectNum"] = 2
				}
			}
		},
		[3] = {
			["TaskName"] = "aaa",
			["AddActivityValue"] = 50,
			["TaskDesc"] = "TaskDesc",
			["FinishCount"] = 1,
			["TaskImage"] = 1,
			["TaskReward"] = {
				[1] = {
					["ObjectId"] = 10001,
					["ObjectNum"] = 1
				},
				[2] = {
					["ObjectId"] = 10002,
					["ObjectNum"] = 2
				}
			}
		},
		[4] = {
			["TaskName"] = "aaa",
			["AddActivityValue"] = 50,
			["TaskDesc"] = "TaskDesc",
			["FinishCount"] = 1,
			["TaskImage"] = 1,
			["TaskReward"] = {
				[1] = {
					["ObjectId"] = 10001,
					["ObjectNum"] = 1
				},
				[2] = {
					["ObjectId"] = 10002,
					["ObjectNum"] = 2
				}
			}
		},
		[5] = {
			["TaskName"] = "aaa",
			["AddActivityValue"] = 50,
			["TaskDesc"] = "TaskDesc",
			["FinishCount"] = 1,
			["TaskImage"] = 1,
			["TaskReward"] = {
				[1] = {
					["ObjectId"] = 10001,
					["ObjectNum"] = 1
				},
				[2] = {
					["ObjectId"] = 10002,
					["ObjectNum"] = 2
				}
			}
		},
		[6] = {
			["TaskName"] = "aaa",
			["AddActivityValue"] = 50,
			["TaskDesc"] = "TaskDesc",
			["FinishCount"] = 1,
			["TaskImage"] = 1,
			["TaskReward"] = {
				[1] = {
					["ObjectId"] = 10001,
					["ObjectNum"] = 1
				},
				[2] = {
					["ObjectId"] = 10002,
					["ObjectNum"] = 2
				}
			}
		}
	}
}
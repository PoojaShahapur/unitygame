LuaConfig = LuaConfig or {}

LuaConfig.SignConfig = 
{
	--累积奖励
	["CumulaReward"] = {
		[1] = {
			--奖励 Id ，使用这确定领取的奖励，目前 Id 默认就是下标
			--["UniqueId"] = 1, 
			--奖励描述
			["RewardDesc"] = "签到3天", 
			--奖励内容
			["Reward"] = {
				--第一个奖励
				[1] = {
					["ObjectId"] = 10001, 
					["ObjectNum"] = 10001
				},
				--第二个奖励
				[2] = {
					["ObjectId"] = 10002, 
					["ObjectNum"] = 10003
				}
			}
		},
		[2] = {
			["RewardDesc"] = "签到5天", 
			["Reward"] = {
				[1] = {
					["ObjectId"] = 10001, 
					["ObjectNum"] = 10001
				},
				[2] = {
					["ObjectId"] = 10002, 
					["ObjectNum"] = 10003
				}
			}
		},
		[3] = {
			["RewardDesc"] = "签到7天", 
			["Reward"] = {
				[1] = {
					["ObjectId"] = 10001, 
					["ObjectNum"] = 10001
				},
				[2] = {
					["ObjectId"] = 10002, 
					["ObjectNum"] = 10003
				}
			}
		}
	},
	--每日奖励
	["DailyReward"] = {
		--一月奖励
		[1] = {
			--第一个奖励
			[1] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[2] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[3] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[4] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[5] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[6] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[7] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[8] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[9] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[10] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[11] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[12] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[13] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[14] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[15] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[16] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[17] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[18] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[19] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[20] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[21] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[22] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[23] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[24] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[25] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[26] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[27] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[28] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[29] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[30] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[31] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			}
		},
		--二月奖励
		[2] = {
			[1] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[2] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[3] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[4] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[5] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[6] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[7] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[8] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[9] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[10] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[11] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[12] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[13] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[14] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[15] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[16] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[17] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[18] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[19] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[20] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[21] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[22] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[23] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[24] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[25] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[26] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[27] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[28] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[29] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			}
		},
		--三月奖励
		[3] = {
			[1] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[2] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[3] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[4] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[5] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[6] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[7] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[8] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[9] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[10] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[11] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[12] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[13] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[14] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[15] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[16] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[17] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[18] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[19] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[20] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[21] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[22] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[23] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[24] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[25] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[26] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[27] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[28] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[29] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[30] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[31] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			}
		},
		--四月奖励
		[4] = {
			[1] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[2] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[3] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[4] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[5] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[6] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[7] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[8] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[9] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[10] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[11] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[12] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[13] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[14] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[15] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[16] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[17] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[18] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[19] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[20] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[21] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[22] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[23] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[24] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[25] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[26] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[27] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[28] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[29] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[30] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			}
		},
		--五月奖励
		[5] = {
			[1] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[2] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[3] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[4] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[5] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[6] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[7] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[8] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[9] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[10] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[11] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[12] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[13] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[14] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[15] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[16] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[17] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[18] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[19] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[20] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[21] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[22] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[23] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[24] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[25] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[26] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[27] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[28] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[29] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[30] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[31] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			}
		},
		--六月奖励
		[6] = {
			[1] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[2] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[3] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[4] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[5] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[6] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[7] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[8] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[9] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[10] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[11] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[12] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[13] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[14] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[15] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[16] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[17] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[18] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[19] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[20] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[21] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[22] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[23] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[24] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[25] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[26] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[27] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[28] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[29] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[30] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			}
		},
		--七月奖励
		[7] = {
			[1] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[2] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[3] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[4] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[5] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[6] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[7] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[8] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[9] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[10] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[11] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[12] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[13] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[14] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[15] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[16] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[17] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[18] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[19] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[20] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[21] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[22] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[23] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[24] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[25] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[26] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[27] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[28] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[29] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[30] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[31] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			}
		},
		--八月奖励
		[8] = {
			[1] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[2] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[3] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[4] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[5] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[6] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[7] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[8] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[9] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[10] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[11] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[12] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[13] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[14] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[15] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[16] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[17] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[18] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[19] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[20] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[21] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[22] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[23] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[24] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[25] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[26] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[27] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[28] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[29] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[30] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[31] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			}
		},
		--九月奖励
		[9] = {
			[1] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[2] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[3] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[4] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[5] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[6] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[7] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[8] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[9] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[10] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[11] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[12] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[13] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[14] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[15] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[16] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[17] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[18] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[19] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[20] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[21] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[22] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[23] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[24] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[25] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[26] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[27] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[28] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[29] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[30] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			}
		},
		--十月奖励
		[10] = {
			[1] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[2] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[3] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[4] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[5] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[6] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[7] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[8] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[9] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[10] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[11] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[12] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[13] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[14] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[15] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[16] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[17] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[18] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[19] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[20] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[21] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[22] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[23] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[24] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[25] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[26] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[27] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[28] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[29] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[30] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[31] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			}
		},
		--十一月奖励
		[11] = {
			[1] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[2] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[3] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[4] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[5] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[6] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[7] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[8] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[9] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[10] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[11] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[12] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[13] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[14] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[15] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[16] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[17] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[18] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[19] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[20] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[21] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[22] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[23] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[24] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[25] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[26] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[27] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[28] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[29] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[30] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			}
		},
		--十二月奖励
		[12] = {
			[1] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[2] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[3] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[4] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[5] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[6] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[7] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[8] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[9] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[10] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[11] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[12] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[13] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[14] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[15] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[16] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[17] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[18] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[19] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[20] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[21] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[22] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[23] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[24] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[25] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[26] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[27] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[28] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[29] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[30] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			},
			[31] = {
				["ObjectId"] = 10001, 
				["ObjectNum"] = 10001
			}
		}
	}
}
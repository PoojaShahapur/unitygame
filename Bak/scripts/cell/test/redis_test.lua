--- Redis测试.
-- 需要csv/redis.csv中设置开启redis.
-- 可利用 tools/redis_cluster/run.bat 开启Redis集群。
-- @module test.redis_test

local M = {}

local log = require("log"):new("test.redis_test")

--- 测试函数.
-- 可由test_client触发。
-- 即`c_test.cmd("redis_test")`
function M.test()
	log:info("test2")
	c_redis.set("test:my_key", "test value")
	c_redis.set("test:my_key\0\0", "value2")
	c_redis.get("test:my_key", function(reply_type, v)
			if 0 == reply_type then
				log:info("Got value: " .. v)
				assert(v == "test value")
			elseif 1 == reply_type then
				log:info("No key.")
			else
				log:error("Redis error: " .. v)
			end  -- if
		end)
	c_redis.command("set", "test:my_key", 12345)
	c_redis.command("get", "test:my_key", {}, function(reply)
			assert(reply.type == 1)  -- REDIS_REPLY_STRING 1
			assert(reply.str == "12345")
			assert(#reply.elements == 0)
		end)
end  -- test()

return M

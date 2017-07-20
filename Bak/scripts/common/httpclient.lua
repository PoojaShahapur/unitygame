local HttpClient = {}

local log = require("log"):new("HttpClient")
local event_handler = require("event_handler.event_handler")

local httpclient_get = c_httpclient.get
local httpclient_post = c_httpclient.post

function HttpClient:get(url, cb)
	local id = httpclient_get(url)
	event_handler.register_once("__httpclient_get_" .. tostring(id), cb)
end

function HttpClient:post(url, data, cb)
	local id = httpclient_post(url, data)
	event_handler.register_once("__httpclient_post_" .. tostring(id), cb)
end

return HttpClient

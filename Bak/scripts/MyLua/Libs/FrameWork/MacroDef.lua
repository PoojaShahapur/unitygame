-- 全局表，不能引用其它表
MacroDef = {};

MacroDef.UNIT_TEST = false;
MacroDef.UNITY_EDITOR = false;
MacroDef.OPEN_ZBS_DEBUG = false;

MacroDef.ENABLE_LOG = false;
MacroDef.ENABLE_WARN = false;
MacroDef.ENABLE_ERROR = false;
MacroDef.DEBUG_NOTNET = false;

-- tolua 老版本
MacroDef.TOLUA_20160101 = false;   -- 之前的版本

-- tolua 新版本
MacroDef.TOLUA_20170711 = false;   -- 新版本

return MacroDef;
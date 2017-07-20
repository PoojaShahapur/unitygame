--region *.lua
--Date
--此文件由[BabeLua]插件自动生成

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "SocialData";
GlobalNS[M.clsName] = M;

function M:ctor(...)
    self:setclientdata();
    self:init();
end

function M:init(args)
    self.mEveryPageNum = 6; --每页最多6个
    self.mCurOpenTag = 0;
    -- 关注列表
    self.mFocusList = GlobalNS.new(GlobalNS.MKeyIndexList);
	self.mFocusList:setIsSpeedUpFind(true);
	self.mFocusList:setIsOpKeepSort(true);

    self.mFocusTotalPage = 1; --总页数
    self.mFocusLastReqPage = 1; --最后一次请求的页数
    self.mFocusTotalNum = 0; --总个数

    -- 粉丝列表
    self.mFansList = GlobalNS.new(GlobalNS.MKeyIndexList);
	self.mFansList:setIsSpeedUpFind(true);
	self.mFansList:setIsOpKeepSort(true);
    
    self.mFansTotalPage = 1; --总页数
    self.mFansLastReqPage = 1; --最后一次请求的页数
    self.mFansTotalNum = 0; --总个数
end

--关注
function M:getFocusListCount()
	return self.mFocusList:count();
end

function M:getFocusItemByIndex(index)
	return self.mFocusList:get(index);
end

function M:updateFocusList(totalnum, reqpage, count, args)
    self.mFocusTotalNum = totalnum;
    self.mFocusTotalPage = math.floor((self.mFocusTotalNum + self.mEveryPageNum - 1) / self.mEveryPageNum);
    self.mFocusLastReqPage = reqpage;

    local key = 0;
    for i=1, count do
        key = args[i-1].uid;
        if(not self.mFocusList:ContainsKey(key)) then
            local item = args[i-1];
		    self.mFocusList:add(key, item);
	    end
    end
    
    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIFindFriend);
    if nil ~= form and form.mIsReady then
        form:updateFocusData(false);
    end
end

function M:getFocusItembyKey(key)
    local item = nil;
    if(self.mFocusList:ContainsKey(key)) then
		item = self.mFocusList:value(key);
	end
    return item;
end

--粉丝
function M:getFansListCount()
	return self.mFansList:count();
end

function M:getFansItemByIndex(index)
	return self.mFansList:get(index);
end

function M:updateFansList(totalnum, reqpage, count, args)
    self.mFansTotalNum = totalnum;
    self.mFansTotalPage = math.floor((self.mFansTotalNum + self.mEveryPageNum - 1) / self.mEveryPageNum);
    self.mFansLastReqPage = reqpage;

    local key = 0;
    for i=1, count do
        key = args[i-1].uid;
        if(not self.mFansList:ContainsKey(key)) then
            local item = args[i-1];
		    self.mFansList:add(key, item);
	    end
    end
    
    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIFindFriend);
    if nil ~= form and form.mIsReady then
        form:updateFansData(false);
    end
end

function M:getFansItembyKey(key)
    local item = nil;
    if(self.mFansList:ContainsKey(key)) then
		item = self.mFansList:value(key);
	end
    return item;
end

function M:dtor()
    self:clear();
end

function M:clear()
    self.mFocusList:clear();
    self.mFansList:clear();
end

--城市表和昵称表
function M:setclientdata()
    self.citys = {
"上海",
"北京",
"天津",
"河北",
"山西",
"内蒙古",
"辽宁",
"吉林",
"黑龙江",
"江苏",
"浙江",
"安徽",
"福建",
"江西",
"山东",
"河南",
"湖北",
"湖南",
"广东",
"广西",
"海南",
"重庆",
"四川",
"贵州",
"云南",
"西藏",
"陕西",
"甘肃",
"青海",
"宁夏",
"新疆",
"台湾",
"香港",
"澳门"
};

	self.nicknames = {
"唐公主",
"小二",
"鹰之舞V冰棍",
"MM",
"伊诺",
"DG龙帝香水泡泡",
"云中孤龙",
"神一样的辅助",
"春暖花开",
"永久隐身",
"天堂小象",
"六日",
"KILLY天蝎",
"苔丝",
"女神咩咩",
"残羽",
"宇哥玩魔道",
"猛男哥",
"雪姬",
"大白兔",
"火舞嫣冉",
"卡蒂娜",
"芭宝宝",
"Jay然不同",
"小可可",
"蔚蓝X柔儿",
"飞天奶妞",
"众神魅影",
"十字军交叉",
"决战V木头",
"魔道小猪",
"新月v奇缘",
"问题儿童",
"宁宁",
"小宝静静",
"魔鬼uu",
"貊弓",
"唐朝飞扬",
"十子军十八骑",
"佳少爷",
"吉尺明步",
"墨墨",
"柠檬龙女",
"阳光到来",
"WY英雄",
"唐朝歆雨彤",
"LAN_YT",
"桃生小鸟",
"崔斯特",
"弑天",
"天族法师",
"冰柠檬",
"sume",
"疯人院旱匪小河",
"冰之翡翠",
"安魂X婉儿",
"Goku",
"吉诺比利",
"DG龙帝魂守",
"NLT宝贝",
"天雄",
"DSS醉",
"红岸猫战",
"鬼舞帝国",
"苹果三千",
"落叶",
"灵魂契约者",
"CY点月",
"ZX暴君Bear",
"鬼厉",
"piao",
"十字军邀月",
"琦琦飞扬",
"蔷薇旱魃",
"花心的九天九夜",
"追梦打劫",
"法魔",
"银月恶魔",
"神殇V催风",
"不准删",
"猫猫鱼",
"诸葛海恩",
"中法",
"四处飘",
"斗剑者CCTV",
"Doreen肉肉",
"Doreen龙聪",
"炫舞V梅尔菲斯",
"曰天曰地曰美女",
"战天疯哥",
"瘋狂的鴨子",
"Ssd乄赳赳",
"DG龙帝梦魇",
"射小鸡",
"Max零",
"复古风格",
"斗剑者M",
"Gettin",
"溪尔",
"唯笑",
"煞神之死神",
"SPEED心缘",
"白白",
"伊丝妮丫",
"辉起一脚",
"物语",
"小邪",
"上尉",
"EVA",
"So刺青灬",
"柳欣欣",
"战O霸",
"囧嵩",
"Now呔假",
"卡卡卡",
"十二守护",
"洛珈V淹s的鱼",
"Ea霸者乄论道",
"用舌头舔死你",
"骚不骚看小腰",
"聚缘歪超",
"趴蔻",
"炫舞V灵通",
"AYU紫龙",
"戳戳",
"五眼",
"如影随形",
"Doreen未央",
"炽火轮子",
"小脸迷人",
"闪白飞帅",
"天翼龙轩",
"Doreen浅唱",
"战天丶YoYo",
"北方V死訫",
"火凤洛凌",
"北方V夜鬼",
"镇魂咒",
"今夜我会来",
"X黑仔",
"格调丨零",
"蚂蚁V流浪",
"Kumo",
"爱情的木偶",
"无影",
"嘉嘉",
"蓝凌V冷若寒冰",
"聚缘V貔貅",
"vIp暗夜魔王",
"DG光明",
"无双小鬼",
"阝灬黑蝶",
"泡妞是美德",
"未灵风",
"永恒的月亮",
"轻纱舞风",
"紫燕",
"KinGR",
"出水芙蓉",
"翼乘风",
"永恒DE战士",
"北帝小非",
"風雲",
"暗夜v儒帅风度",
"DoreenEG",
"星海骑士",
"珈蓝妖怪",
"骑士之王",
"XF蔓落",
"神仙",
"爱吃猫的鱼",
"啭身微笑",
"KOF五味子",
"咆哮的大蚂蚁",
"星辰之恋",
"奥西里斯",
"STAR小手",
"DG无悠",
"sara",
"唐朝商殇",
"蔚蓝小蜜",
"情谊灵魂",
"哈汀",
"秋天de菠菜",
"ZX凉茶",
"DG心的颜色",
"灵帝",
"幻域圣徒犹大",
"艾尔",
"炽火小迦",
"聚缘u诱惑天下",
"KOF低调",
"Rank甜心",
"卧龙心宝",
"DG花海",
"Ea霸者无名",
"AKL绝世狂龙",
"众神之魂",
"彩潋",
"神昭女侠",
"ns梅扎坏坏",
"康娜莉娅",
"YoYo",
"小浪猪",
"九州飞鸟",
"聚缘V原绍伟",
"灬義薄雲天灬",
"弑神V任远",
"虾米快跑",
"想太多",
"西里西亚Elo",
"神之手",
"荣耀残阳",
"永恒之旅",
"O左手O",
"CLORIS",
"凌空",
"天若兮的老公",
"传说败类",
"轩辕希望之都",
"日游",
"蓝尾狐",
"Alce",
"天香",
"狂兰",
"灵魂二爺",
"ZXwin",
"DG兵九",
"CA凤",
"无邪",
"抽风",
"疯人院小牙",
"二十一克平常",
"AKL绯雨狂",
"三界",
"忘记过去",
"相忘江湖",
"聚缘V**龙",
"蔚蓝柔儿",
"弑神灭魔",
"暴力茶叶",
"秋泽",
"johson",
"Ea霸者丶",
"神de彪彪",
"ns刈百",
"紫羽冰魂",
"圣殿啊猛",
"蝶舞小南瓜",
"Np不老的传说",
"羽零尛",
"风飘渺",
"唐朝尛藾潴",
"FFD仙缘",
"海峡西岸",
"妖孽海",
"安娜苏",
"ELLE",
"koeon",
"追梦芬朵朵",
"性感小野猫",
"若雨",
"翠鈺讌繎",
"缘来VS冷血",
"火焰之舞",
"天使之翼",
"我喝三鹿",
"见世面了",
"买个烧麦",
"空心核桃",
"摇滚芭比",
"丿三蚊鱼",
"邪心卡卡",
"刀问情",
"楚蝶衣",
"罗德里格斯",
"天宫De老鼠",
"熔点",
"七宗罪",
"塟礼",
"乘风",
"世界末日",
"矮子哥",
"幻域天煞",
"龙在心",
"九点射手星",
"孑少",
"无道落海",
"黑夜",
"就是来看看",
"灰灰",
"珊瑚",
"十二耳环",
"eaka",
"雪儿乖潴潴",
"就一畸形",
"银月之殇",
"不可",
"君莫忆",
"逐梦小白",
"琪琪児",
"唐朝缘字诀",
"Ea霸者冰凌决",
"独特de坏",
"KinG風儿",
"决战醉九儿",
"铁血封情",
"枫vv雪",
"雪芭",
"小花花",
"弦月c缠绵天涯",
"鬼魅天使",
"kok领袖",
"暮春晓",
"艾瑞克尔",
"鈊煩",
"魔王归来",
"奶茶",
"锋芒锋芒",
"收割者",
"创迹丶止愛",
"女圭女圭",
"艾帝雷斯",
"小不点兔子",
"雷ray",
"暗精灵",
"死亡之舞",
"弹无虚发",
"伤感女儿红",
"Oo危险人物oO",
"基纽君君",
"狼外婆",
"该隐",
"AS恋星梦",
"訫随倪动",
"风灵",
"縌彤",
"康娜莉亚",
"旋天",
"心随风最舞月光",
"纠葛",
"天蚁天堂",
"ns流枫",
"Gods众神领域",
"小乖乖",
"肥婆肆",
"飘零de枫叶",
"CK",
"天使帝国",
"残忆",
"我不是瞿峰",
"佳佳",
"唐朝冷心",
"兜兜罗祭师",
"TArrow",
"御守",
"丨翼丶猫",
"坎帕斯",
"剑太一",
"弑神vs司令",
"武夫",
"棉花棠棠",
"雨莫言",
"jessice",
"法斯特",
"炽火奶油",
"当世界充满爱",
"追梦V冬虫草",
"战天丶龙龙",
"夜神light",
"将军妃",
"蓝忧雪",
"狼魂X风情",
"甄子",
"蔚蓝狂妄",
"舞凤",
"阿白",
"残月X暗影绝杀",
"唯莪",
"鱼儿水中游",
"茹烟小妖",
"乖宝",
"沖天無雙",
"Hrose狱火",
"弑神v嘩囇烟火",
"唐朝体验号",
"斯文败类",
"追梦小睡",
"童童",
"女猎人",
"甜瞳",
"纤纤",
"摩鲤鱼",
"天道难容",
"笑苍天",
"楓舞",
"J謎亂J囬渋",
"刀三十三君君",
"幽谷百合",
"暗影刺客",
"江山旋律",
"Lisa语",
"放弃荣耀",
"折翅o天使",
"逍遥八爷",
"站住打劫",
"狂奔",
"SPEED旋律",
"九州石头",
"我的追随者",
"苏城宝贝",
"追梦排骨哥哥",
"珈蓝翔空一箭",
"箭竹酒De味道",
"龙之星",
"霸道娃娃KOK",
"经常给雷劈",
"弑神v龙舞",
"逆天V小魅力",
"神秘枫",
"丑战",
"逆天V风",
"红岸鳯皇",
"失去记忆",
"迦楼羅丶幻歌",
"Takisu",
"玉灵风",
"火凤师真",
"ONLY",
"棉花糖糖",
"反骨仔",
"紫情",
"帝国丨单人小马",
"舞動的瑰寶",
"睡衣",
"众神之王",
"Oo冬妮娅之恋oO",
"老法",
"雪精灵",
"粤之青龍",
"红岸回w回",
"蚂蚁军团男杀星",
"游戏",
"十三归来",
"Mua小粗",
"男法",
"修罗无魂",
"ooLiLi",
"优雅小舞",
"SWAT扬松",
"永毅缔造",
"瞬时",
"麥兜兜",
"偶很有型",
"FFD冷寂",
"北极星DE幻想",
"月色雨",
"rickroot",
"初体验",
"粉色的小猪",
"决战V残剑",
"冷月血祭",
"天若兮",
"你并不孤独",
"天海",
"幻舞VBirth",
"露漪",
"决战V壊亼",
"改改",
"ASura可可",
"NoNoLiLi",
"toto",
"SPEED三三",
"心随星动"
};

end

return M;
--endregion

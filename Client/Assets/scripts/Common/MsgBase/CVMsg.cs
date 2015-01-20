namespace SDK.Common
{
    public class CVMsg
    {
        public const int GAME_VERSION = 1999;
        public const int MAX_IP_LENGTH = 16;
        public const int MAX_ACCNAMESIZE = 48;
        public const int MAX_PASSWORD = 16;
        public const ushort MAX_CHARINFO = 3;
        public const int MAX_NAMESIZE = 32;
    }

    public enum ERetResult
    {
        LOGIN_RETURN_UNKNOWN,   /// 未知错误
        LOGIN_RETURN_VERSIONERROR, /// 版本错误
        LOGIN_RETURN_UUID,     /// UUID登陆方式没有实现
        LOGIN_RETURN_DB,     /// 数据库出错
        LOGIN_RETURN_PASSWORDERROR,/// 帐号密码错误
        LOGIN_RETURN_CHANGEPASSWORD,/// 修改密码成功
        LOGIN_RETURN_IDINUSE,   /// ID正在被使用中
        LOGIN_RETURN_IDINCLOSE,   /// ID被封
        LOGIN_RETURN_GATEWAYNOTAVAILABLE,/// 网关服务器未开
        LOGIN_RETURN_USERMAX,   /// 用户满
        LOGIN_RETURN_ACCOUNTEXIST, /// 账号已经存在
        LOGON_RETURN_ACCOUNTSUCCESS,/// 注册账号成功

        LOGIN_RETURN_CHARNAMEREPEAT,/// 角色名称重复
        LOGIN_RETURN_USERDATANOEXIST,/// 用户档案不存在
        LOGIN_RETURN_USERNAMEREPEAT,/// 用户名重复
        LOGIN_RETURN_TIMEOUT,   /// 连接超时
        LOGIN_RETURN_PAYFAILED,   /// 计费失败
        LOGIN_RETURN_JPEG_PASSPORT, /// 图形验证码输入错误
        LOGIN_RETURN_LOCK,         /// 帐号被锁定
        LOGIN_RETURN_WAITACTIVE, /// 帐号待激活
        LOGIN_RETURN_NEWUSER_OLDZONE      ///新账号不允许登入旧的游戏区 
    }

    public enum ChallengeGameType
    {
        CHALLENGE_GAME_RELAX_TYPE = 1,        //PVP 休闲对战
        CHALLENGE_GAME_RANKING_TYPE = 2,        //PVP 排名对战
        CHALLENGE_GAME_COMPETITIVE_TYPE = 3,        //PVP 竞技对战
        CHALLENGE_GAME_FRIEND_TYPE = 4,        //PVP 好友对战
        CHALLENGE_GAME_PRACTISE_TYPE = 5,        //PVE 普通练习
        CHALLENGE_GAME_BOSS_TYPE = 6,        //PVE BOSS模式
    };

    /**  
     * \brief   对战类型枚举
     */
    //enum ChallengeGameType
    //{
    //    CHALLENGE_GAME_RELAX_TYPE = 1,        //PVP 休闲对战
    //    CHALLENGE_GAME_RANKING_TYPE = 2,        //PVP 排名对战
    //    CHALLENGE_GAME_COMPETITIVE_TYPE = 3,        //PVP 竞技对战
    //    CHALLENGE_GAME_FRIEND_TYPE = 4,        //PVP 好友对战
    //    CHALLENGE_GAME_PRACTISE_TYPE = 5,        //PVE 普通练习
    //    CHALLENGE_GAME_BOSS_TYPE = 6,        //PVE BOSS模式
    //};

    public enum ChallengeState
    {
        CHALLENGE_STATE_NONE = 0,	//游戏刚刚创建(还未填充双方数据)
        CHALLENGE_STATE_PREPARE = 1,	//准备阶段(此时可以替换第一把的手牌)
        CHALLENGE_STATE_BATTLE = 2,	//战斗中
        CHALLENGE_STATE_END = 3,	//游戏结束
        CHALLENGE_STATE_CANCLEAR = 4,	//可以卸载状态
    }

    //enum ChallengeState
    //{
    //    CHALLENGE_STATE_NONE	= 0,	//游戏刚刚创建(还未填充双方数据)
    //    CHALLENGE_STATE_PREPARE	= 1,	//准备阶段(此时可以替换第一把的手牌)
    //    CHALLENGE_STATE_BATTLE	= 2,	//战斗中
    //    CHALLENGE_STATE_END		= 3,	//游戏结束
    //    CHALLENGE_STATE_CANCLEAR	= 4,	//可以卸载状态
    //};

    public enum CardArea
    {
        CARDCELLTYPE_NONE,
        CARDCELLTYPE_COMMON,	    //׷սӡ
        CARDCELLTYPE_HAND,	    //˖Ɔλ׃
        CARDCELLTYPE_EQUIP,	    //ϤǷӛλ׃
        CARDCELLTYPE_SKILL,	    //ܼŜӛλ׃
        CARDCELLTYPE_HERO,	    //Ӣћӛλ׃
    }

    // 这个是卡牌区域位置
    //enum CardArea
    //{
    //  CARDCELLTYPE_NONE,/// 不是格子，用于丢弃或捡到物品
    //  CARDCELLTYPE_COMMON,	    //׷սӡ
    //  CARDCELLTYPE_HAND,	    //˖Ɔλ׃
    //  CARDCELLTYPE_EQUIP,	    //ϤǷӛλ׃
    //  CARDCELLTYPE_SKILL,	    //ܼŜӛλ׃
    //  CARDCELLTYPE_HERO,	    //Ӣћӛλ׃
    //};
}
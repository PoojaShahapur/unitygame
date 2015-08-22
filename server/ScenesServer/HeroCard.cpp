#include "SceneUser.h"
#include "CardTujianManager.h"
#include "Mail.h"

/**     
 * \brief  å¤„ç†stHeroCardCmdæŒ‡ä»¤
 *
 *
 * å¤„ç†stHeroCardCmdæŒ‡ä»¤
 *      
 * \param rev: æŽ¥å—åˆ°çš„æŒ‡ä»¤å†…å®¹
 * \param cmdLen: æŽ¥å—åˆ°çš„æŒ‡ä»¤é•¿åº¦
 * \return å¤„ç†æŒ‡ä»¤æˆåŠŸè¿”å›žçœŸ,å¦åˆ™è¿”å›žfalse
 */       
bool SceneUser::doHeroCardCmd(const Cmd::stHeroCardCmd *rev,DWORD cmdLen)
{
    switch(rev->byParam)
    {
#if 0
	case Cmd::REQ_CARD_GROUP_LIST_INFO_CMD:
	    {
		GroupCardManager::getMe().notifyAllGroupListToMe(*this);
		//Mail::sendSysText("ÏµÍ³ÓÊ¼þ", this->id, "»¶Ó­ÄúÀ´µ½ÓÎÏ·!×£ÄúÍæµÃÓä¿ì!");
		return true;
	    }
	    break;
#endif
	case Cmd::REQ_ONE_CARD_GROUP_INFO_CMD:
	    {
		Cmd::stRetOneCardGroupInfoUserCmd *cmd = (Cmd::stRetOneCardGroupInfoUserCmd*)rev;
		GroupCardManager::getMe().notifyOneGroupInfoToMe(*this, cmd->index);
		return true;
	    }
	    break;
	case Cmd::REQ_CREATE_ONE_CARD_GROUP_CMD:
	    {
		Cmd::stReqCreateOneCardGroupUserCmd *cmd = (Cmd::stReqCreateOneCardGroupUserCmd*)rev;
		GroupCardManager::getMe().handleCreateOneGroup(*this, cmd->occupation);
		return true;
	    }
	    break;
	case Cmd::REQ_SAVE_ONE_CARD_GROUP_CMD:
	    {
		Cmd::stReqSaveOneCardGroupUserCmd *cmd = (Cmd::stReqSaveOneCardGroupUserCmd*)rev;
		if(cmdLen < (sizeof(Cmd::stReqSaveOneCardGroupUserCmd)+sizeof(DWORD)*cmd->count))
		{
		    Zebra::logger->debug("[Íâ¹Ò]%s(%u) stReqSaveOneCardGroupUserCmd size=%u cmdLen=%u",this->name, this->id, cmd->count, cmdLen);
		    return true;
		}
		GroupCardManager::getMe().handleSaveOneGroup(*this, cmd->id, cmd->count, cmd->index);
		return true;
	    }
	    break;
	case Cmd::REQ_DELETE_ONE_CARD_GROUP_CMD:
	    {
		Cmd::stReqDeleteOneCardGroupUserCmd *cmd = (Cmd::stReqDeleteOneCardGroupUserCmd*)rev;
		GroupCardManager::getMe().handleDeleteOneGroup(*this, cmd->index);
		return true;
	    }
	    break;
	case Cmd::RENAME_CARD_GROUP_USERCMD_PARAMETER:
	    {//¿¨×é¸ÄÃû
		Cmd::stRenameCardGroupUserCmd *cmd = (Cmd::stRenameCardGroupUserCmd *)rev;
		GroupCardManager::getMe().handleRenameOneGroup(*this, cmd->index, cmd->name);
		return true;
	    }
	    break;
#if 0
	case Cmd::REQ_ALL_CARD_TUJIAN_DATA_CMD:
	    {
		CardTujianManager::getMe().notifyAllTujianDataToMe(*this);
		return true;
	    }
	    break;
	case Cmd::REQ_ALL_HERO_INFO_CMD:
	    {
		HeroInfoManager::getMe().notifyAllHeroInfoToMe(*this);
		return true;
	    }
	    break;
#endif
	case Cmd::REQ_HERO_FIGHT_MATCH_CMD:
	    {
		Cmd::stReqHeroFightMatchUserCmd *cmd = (Cmd::stReqHeroFightMatchUserCmd *)rev;

		if(ChallengeGameManager::getMe().isFighting(this))
		    return false;

		if(GroupCardManager::getMe().canUseOneGroup(*this, cmd->index))
		{
		    Cmd::Session::t_ReqFightMatch_SceneSession send;
		    send.userID = this->id;
		    send.cardsNumber = cmd->index;	    //Ì×ÅÆ
		    send.score = 1;			    //Õ½Á¦Öµ
		    send.type = cmd->fightType;	    //¶ÔÕ½ÀàÐÍ
		    send.cancel = cmd->cancel;	    
		    return sessionClient->sendCmd(&send, sizeof(send));
		}
		else
		{
		    Zebra::logger->error("[ÇëÇó¶ÔÕ½] Íæ¼ÒÌ×ÅÆ²»´æÔÚ »òÕß Ì×ÅÆ²»Âú30ÕÅ ");
		}
		return false;
	    }
	    break;
	case Cmd::REQ_END_MY_ROUND_CMD:		    //½áÊø»ØºÏ
	    {
		ChallengeGameManager::getMe().handleEndOneRound(*this);
		return true;
	    }
	    break;
	case Cmd::REQ_GIVEUP_ONE_BATTLE_CMD:	    //ÈÏÊä
	    {
		ChallengeGameManager::getMe().handleGiveUpBattle(*this);
		return true;
	    }
	    break;
	case Cmd::REQ_FIGHT_PREPARE_OVER_CMD:
	    {
		Cmd::stReqFightPrepareOverUserCmd *cmd = (Cmd::stReqFightPrepareOverUserCmd *)rev;
		if(cmd)
		{
		    ChallengeGameManager::getMe().handleStartGame(*this, cmd->change);
		}
		return true;
	    }
	    break;
	case Cmd::REQ_ENTER_UNFINISHED_GAME_CMD:
	    {
		ChallengeGameManager::getMe().handlesecondEnter(this);
		return true;
	    }
	    break;
	case Cmd::REQ_CARD_MAGIC_USERCMD_PARA:
	    {//ÆÕÍ¨¹¥»÷
		Cmd::stCardAttackMagicUserCmd *cmd =(Cmd::stCardAttackMagicUserCmd *)rev;
		ChallengeGameManager::getMe().handleCardAttackMagic(*this, cmd);
		return true;
	    }
	    break;
	case Cmd::REQ_CARD_MOVE_AND_MAGIC_USERCMD_PARA:
	    {//³öÅÆ
		Cmd::stCardMoveAndAttackMagicUserCmd *cmd = (Cmd::stCardMoveAndAttackMagicUserCmd *)rev;
		ChallengeGameManager::getMe().handleCardMoveAndAttackMagic(*this, cmd);
		return true;
	    }
	    break;
	default:
	    break;
    }
    Zebra::logger->error("SceneUser::doHeroCardCmd\tparam:%d",rev->byParam);
    return false;
}


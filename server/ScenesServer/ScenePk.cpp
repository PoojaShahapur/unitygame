/**
  * \brief PK文件,所有PK相关的实现放在这里
  * 
  */
#include <zebra/ScenesServer.h>

#define NOTE

/**
 * \brief 构造函数
 */
SkillState::SkillState()
{
  bzero(swdValue,sizeof(swdValue));
}

/**
 * \brief 初始化技能状态
 */
void SkillState::init()
{
  // 以下这几个值是宠物能力增强相关状态,由于增强因数之一的人物魔法值在增强以后被清除所以数值不可重算故保留
  SWORD bk_uppetdamage = uppetdamage; //提升召唤兽的攻击力 
  SWORD bk_uppetdefence= uppetdefence; //提升召唤兽的防御力
  SDWORD bk_maxhp     = maxhp;       //生命值最大值变更

  WORD bk_introject_maxmdam = introject_maxmdam;
  WORD bk_introject_maxpdam  = introject_maxpdam;
  WORD bk_introject_mdam  = introject_mdam;
  WORD bk_introject_pdam  = introject_pdam;
  WORD bk_introject_mdef  = introject_mdef;
  WORD bk_introject_pdef  = introject_pdef;
  WORD bk_introject_maxhp = introject_maxhp;

  bzero(swdValue,sizeof(swdValue));

  uppetdamage = bk_uppetdamage; //提升召唤兽的攻击力
  uppetdefence= bk_uppetdefence; //提升召唤兽的防御力
  maxhp    = bk_maxhp;       //生命值最大值变更
  introject_maxmdam  = bk_introject_maxmdam;
  introject_maxpdam  = bk_introject_maxpdam;
  introject_mdam    = bk_introject_mdam;
  introject_pdam    = bk_introject_pdam;
  introject_mdef    = bk_introject_mdef;
  introject_pdef    = bk_introject_pdef;
  introject_maxhp    = bk_introject_maxhp;
}

/**
 * \brief 处理接收到的PK消息
 * 
 * \rev 接收的数据的地址
 *
 * \return 攻击成功返回TRUE,否则返回FALSE
 */
// [ranqd] 攻击入口
bool SceneUser::attackMagic(const Cmd::stAttackMagicUserCmd *rev,const DWORD cmdLen)
{
  attackTarget = NULL; // 将装备属性的附加状态投送目标设置成空

  //sky 将加成标志初始化
  Daggerflag = false;
  Throflag = false;
  Handsflag = false;
  Handflag = false;
  Flameflag = false;
  Iceflag = false;
  Sacredflag = false;
  Darkflag = false;	
  
  if (this->isQuiz)
  {
    ScenePk::attackFailToMe(rev,this);
    return true;
  }

  //sky 灵魂状态下不可以进行攻击
  if(Soulflag)
	  return true;
    
  if (this->isSitdown())
  {
    ScenePk::attackFailToMe(rev,this);
    Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"你处于打坐状态！");
    return true;
  }

  //sky 如果是隐身就先退出隐身在攻击
  if (hideme)
  {
	 skillStatusM.clearRecoveryElement(241);
  }

  // 检查可否攻击标志
  if (!attackAction)
  {
    ScenePk::attackFailToMe(rev,this);
    return true;
  }

  SceneEntryPk * def = 0;

  if (Cmd::ATTACKTYPE_U2U==rev->byAttackType)
  {
	  def = scene->getUserByTempID(rev->dwDefenceTempID);
	  if(!def)
	  {
		  Zebra::logger->error("%s 用户攻击拉一个无效目标!", this->name);
		  return false;
	  }
  }
  else if(Cmd::ATTACKTYPE_U2N==rev->byAttackType)
  {
	  def = SceneNpcManager::getMe().getNpcByTempID(rev->dwDefenceTempID);
	  if(!def)
	  {
		  Zebra::logger->error("%s 用户攻击拉一个无效目标!", this->name);
		  return false;
	  }
  }

  //TODO 检查能否攻击可能有技能造成自己无法攻击
  //骑马动作过滤
  //暂时所有马都能攻击
  if (horse.mount()&& !horse.canFight())
  {
    ScenePk::attackFailToMe(rev,this);
    Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"对不起,您的马不能战斗");
    return true;
  }

  if (this->assault)
  {
    this->skillStatusM.clearRecoveryElement(121);
    this->reSendMyMapData();
  }
  else if (!ScenePk::checkAttackSpeed(this,rev)) 
  {
    ScenePk::attackFailToMe(rev,this,true,true);
    return true;
  }


  //攻击类装备耐久度消耗
  this->packs.equip.costAttackDur(this);

  this->skillValue.init();
  this->skillStatusM.processPassiveness();// 处理我的被动状态影响
  this->pkValue.damagebonus=0;

  if (Cmd::ATTACKTYPE_U2U==rev->byAttackType)
    def = scene->getUserByTempID(rev->dwDefenceTempID);
  else if (Cmd::ATTACKTYPE_U2N==rev->byAttackType)
    def = SceneNpcManager::getMe().getNpcByTempID(rev->dwDefenceTempID);
  if (def)
    setPetsChaseTarget(def);


  /*SceneEntryPk * def = 0;
  if (Cmd::ATTACKTYPE_U2U==rev->byAttackType)
    def = scene->getUserByTempID(rev->dwDefenceTempID);
  else if (Cmd::ATTACKTYPE_U2N==rev->byAttackType)
    def = SceneNpcManager::getMe().getNpcByTempID(rev->dwDefenceTempID);
  else if (Cmd::ATTACKTYPE_U2N==rev->byAttackType)
  {
    def = SceneNpcManager::getMe().getNpcByTempID(rev->dwDefenceTempID);
    if ((def)&&((((SceneNpc*)def)->aif&AIF_ATK_REDNAME)||(((SceneNpc*)def)->npc->kind==NPC_TYPE_GUARD)))
      if (pkState.addProtect(this))
      {
        Cmd::stAddUserMapScreenUserCmd send;
        full_t_MapUserData(send.data);
        scene->sendCmdToNine(getPosI(),&send,sizeof(send),false);
        Channel::sendSys(this,Cmd::INFO_TYPE_GAME,"你攻击了 %s,两分钟内所有玩家可以对你正当攻击",def->name);
      }
  }
  if (def)
  {
    setCurTarget(def);
    if (!def->isFighting())
      def->setCurTarget(this);
  }
  */

  switch (rev->wdMagicType)
  {
  case SERVER_SKILL_ATTACK_NORMAL:		//单手武器普通攻击
  case SERVER_SKILL_DAGGER_ATTACK_NORMAL: //双持武器(匕首)普通攻击
  case SERVER_SKILL_DART_ATTACK_NORMAL:	//飞镖武器普通攻击
  case SERVER_SKILL_HANDS_ATTACK_NORMAL:	//双手武器普通攻击
	  {
		  switch (rev->byAttackType)
		  {
		  case Cmd::ATTACKTYPE_U2U:
			  {
				  //防止死锁
				  if (this->tempid == rev->dwDefenceTempID)
				  {
					  ScenePk::attackUserCmdToNine(rev,this);
					  return true;
				  }

				  SceneUser *pDef = this->scene->getUserByTempID(rev->dwDefenceTempID);

				  if (pDef && pDef->isPkZone(this)&&this->isPkZone(pDef))// 新加&&this->isPkZone(pDef)
				  {
					  pDef->isPhysics = true;
					  if (rev->wdMagicType==SERVER_SKILL_DART_ATTACK_NORMAL)
					  {
						  //sky 如果是投掷类武器普通攻击
						  Throflag = true;

						  if (this->skillValue.introject_mdam==0) //如果是非合体状态
						  {
							  if (!this->reduce(BOW_ARROW_ITEM_TYPE,1))
							  {
								  ScenePk::attackFailToMe(rev,this);
								  return true;
							  }
						  }
					  }

					  bool bATT = false;
					  switch(rev->wdMagicType)
					  {
					  case SERVER_SKILL_ATTACK_NORMAL:
						  Handflag = true;
						  bATT = true;
						  break;
					  case SERVER_SKILL_DAGGER_ATTACK_NORMAL:
						  Daggerflag = true;
						  bATT = true;
						  break;
					  case SERVER_SKILL_HANDS_ATTACK_NORMAL:
						  Handsflag = true;
						  bATT = true;
						  break;
					  }

					  if (this->isEnemy(pDef,true)&& // 判断敌人友和检查PK模式是一个方法,所以只要判断一次就OK了
						  this->checkMagicFlyRoute(pDef,bATT == true?AttackNear:AttackFly))
					  {
						  if (!ScenePk::attackUserCmdToNine(rev,this))
						  {
							  return true;
						  }

						  if (!pDef->preAttackMe(this,rev,true))
						  {
							  //      ScenePk::attackFailToMe(rev,this,false);
							  return true;
						  }

						  bool physicsatt = true;

						  if (!pDef->AttackMe(this,rev,physicsatt))
						  {
							  ScenePk::attackFailToMe(rev,this);
							  return true;
						  }
						  appendAttack(rev);
						  return true;
					  }
					  else
					  {
						  ScenePk::attackFailToMe(rev,this);
						  return true;
					  }
				  }
				  else
				  {
					  //为元神新加处理
					  SceneNpc *NpcDef = SceneNpcManager::getMe().getNpcByTempID(rev->dwDefenceTempID);
					  if( NpcDef )
					  {
						  if( NpcDef->npc->kind == NPC_TYPE_GHOST )
						  {
							  NpcDef->isPhysics = true;
							  if ((NpcDef->getPetType()!=Cmd::PET_TYPE_NOTPET)
								  && NpcDef->getMaster())
							  {
								  if (NpcDef->scene!=NpcDef->getMaster()->scene
									  ||!scene->zPosShortRange(NpcDef->getPos(),NpcDef->getMaster()->getPos(),20))
								  {
								  }
								  /*else if (!(NpcDef->isPkZone(this) && this->isPkZone(pDef)))
								  {
									  ScenePk::attackUserCmdToNine(rev,this);
									  ScenePk::attackFailToMe(rev,this);
									  return true;
								  }*/
							  }

							  if (!isEnemy(NpcDef))
							  {
								  ScenePk::attackFailToMe(rev,this);
								  return true;
							  }

							  bool bATT = false;
							  switch(rev->wdMagicType)
							  {
							  case SERVER_SKILL_ATTACK_NORMAL:
								  Handflag = true;
								  bATT = true;
								  break;
							  case SERVER_SKILL_DAGGER_ATTACK_NORMAL:
								  Daggerflag = true;
								  bATT = true;
								  break;
							  case SERVER_SKILL_HANDS_ATTACK_NORMAL:
								  Handsflag = true;
								  bATT = true;
								  break;
							  case SERVER_SKILL_DART_ATTACK_NORMAL:
								  Throflag = true;
								  bATT =true;
								  break;
							  }

							  if (this->checkMagicFlyRoute(NpcDef,bATT == true?AttackNear:AttackFly))
							  {
								  if (!NpcDef->preAttackMe(this,rev,true))
								  {
									  return true;
								  }

								  if (!ScenePk::attackUserCmdToNine(rev,this))
								  {
									  return true;
								  }

								  bool physicsatt = true;

								  if (!NpcDef->AttackMe(this,rev,physicsatt))
								  {
									  ScenePk::attackFailToMe(rev,this);
									  return true;
								  }
								  appendAttack(rev);
								  return true;
							  }
							  else
							  {
								  ScenePk::attackUserCmdToNine(rev,this);
								  ScenePk::attackFailToMe(rev,this);
								  return true;
							  }
						  }
						  else
						  {
							  ScenePk::attackFailToMe(rev,this);
						  }
						  break;
					  }
					  else
					  {
						  ScenePk::attackUserCmdToNine(rev,this);
						  ScenePk::attackFailToMe(rev,this);
						  //Zebra::logger->debug("错误的dwDefenceTempID.(%ld,%ld)",rev->dwUserTempID,rev->dwDefenceTempID);
					  }
				  }
		  }  
		  break;

		  case Cmd::ATTACKTYPE_U2N:
			  {
				  SceneNpc *pDef = SceneNpcManager::getMe().getNpcByTempID(rev->dwDefenceTempID);
				  if (pDef)
				  {
					  pDef->isPhysics = true;
					  if ((pDef->getPetType()!=Cmd::PET_TYPE_NOTPET)
						  && pDef->getMaster())
					  {
						  if (pDef->scene!=pDef->getMaster()->scene
							  ||!scene->zPosShortRange(pDef->getPos(),pDef->getMaster()->getPos(),20))
						  {
						  }
						  else if (!(pDef->isPkZone(this) && this->isPkZone(pDef)))
						  {
							  ScenePk::attackUserCmdToNine(rev,this);
							  ScenePk::attackFailToMe(rev,this);
							  return true;
						  }
					  }

					  if (!isEnemy(pDef))
					  {
						  ScenePk::attackFailToMe(rev,this);
						  return true;
					  }

					  if (rev->wdMagicType==SERVER_SKILL_DART_ATTACK_NORMAL)
					  {
						  //sky 如果是投掷类武器普通攻击
						  Throflag = true;

						  if (this->skillValue.introject_mdam==0) //如果是非合体状态
						  {
							  if (!this->reduce(BOW_ARROW_ITEM_TYPE,1))
							  {
								  ScenePk::attackFailToMe(rev,this);
								  return true;
							  }
						  }
					  }

					  bool bATT = false;
					  switch(rev->wdMagicType)
					  {
					  case SERVER_SKILL_ATTACK_NORMAL:
						  Handflag = true;
						  bATT = true;
						  break;
					  case SERVER_SKILL_DAGGER_ATTACK_NORMAL:
						  Daggerflag = true;
						  bATT = true;
						  break;
					  case SERVER_SKILL_HANDS_ATTACK_NORMAL:
						  Handsflag = true;
						  bATT = true;
						  break;
					  }

					  if (this->checkMagicFlyRoute(pDef,bATT == true?AttackNear:AttackFly))
					  {
						  if (!pDef->preAttackMe(this,rev,true))
						  {
							  //  ScenePk::attackFailToMe(rev,this,false);
							  return true;
						  }

						  if (!ScenePk::attackUserCmdToNine(rev,this))
						  {
							  return true;
						  }

						  bool physicsatt = true;

						  if (!pDef->AttackMe(this,rev,physicsatt))
						  {
							  ScenePk::attackFailToMe(rev,this);
							  return true;
						  }
						  appendAttack(rev);

						  //sky 设置自己为PVE状态
						  if(IsPveOrPvp() != USE_FIGHT_PVP && IsPveOrPvp() != USE_FIGHT_PVE)	//sky 先检测自己不是处在pvp状态
						  {
							  SetPveOrPvp(USE_FIGHT_PVE);		//sky 设置自己的战斗状态为pve模式
							  Channel::sendSys(this,Cmd::INFO_TYPE_GAME,"你已进入pve模式");
						  }
						  //sky 同时刷新战斗时间
						  SetPkTime();

						  return true;
					  }
					  else
					  {
						  ScenePk::attackUserCmdToNine(rev,this);
						  ScenePk::attackFailToMe(rev,this);
						  return true;
					  }
				  }
				  else
				  {
					  ScenePk::attackFailToMe(rev,this);
					  //Zebra::logger->debug("错误的dwDefenceTempID.(AID=%ld,DID=%ld)",rev->dwUserTempID,rev->dwDefenceTempID);
				  }
			  }
			  break;

		  case Cmd::ATTACKTYPE_U2B:
			  {
				  if (!ScenePk::attackUserCmdToNine(rev,this))
				  {
					  return true;
				  }
				  //return true;
			  }
			  break;

		  case Cmd::ATTACKTYPE_U2P:
			  {
				  ScenePk::attackUserCmdToNine(rev,this);
			  }
			  break;

		  default:
			  //return true;
			  break;
		}
	  }
	  break;
	default:
		{
			zSkill *s = this->usm.findSkill(rev->wdMagicType);
			//--[新的技能操作方法]---------------------------
			if (s)
			{
				if (skillAction)
				{
					//sky 魔法系魔法加成检测
					if(s->base->preskill3 & SPEC_MAGIC)
					{
						if ((3 == s->base->kind) && (4 == s->base->subkind))
							Flameflag = true;

						if ((3 == s->base->kind) && (5 == s->base->subkind))
							Iceflag = true;

						if ((4 == s->base->kind) && (6 == s->base->subkind))
							Sacredflag = true;

						if ((4 == s->base->kind) && (7 == s->base->subkind))
							Darkflag = true;

					}

					//sky 武器系列加成检测
					if(s->base->preskill3 & SPEC_PHYSICS)
					{
						switch(this->getWeaponType())
						{
						case ItemType_Sword:		// sky 105代表左手剑
						case ItemType_Blade:		// sky 104代表单手刀
							Handflag = true;
							break;
						case ItemType_Crossbow:		// sky 109代表法杖类武器
						case ItemType_Axe:			// sky 106代表双手类武器
							Handsflag = true;
							break;
						case ItemType_Hammer:		// sky 107代表匕首类武器
							Daggerflag = true;
							break;
						case ItemType_Staff:
							Throflag = true;
							break;		
						}
					}
		
					if (!s->action(rev,cmdLen))
					{
						ScenePk::attackFailToMe(rev,this);
						return true;
					}

					//pkState.speedOutM(0);
					appendAttack(rev);
					return true;
				}
				else
				{
					ScenePk::attackFailToMe(rev,this);
					return true;
				}
			}
			else
			{
				ScenePk::attackFailToMe(rev,this);
				Zebra::logger->debug("(%s,%ld)使用自己没有练的技能(%ld)",this->name,this->id,rev->wdMagicType);
				return true;
			}
		}
  }
  return false;
}


void SceneUser::appendAttack(const Cmd::stAttackMagicUserCmd *rev)
{
  Cmd::stAttackMagicUserCmd cmd;

  if (attackTarget)
  {
    memcpy(&cmd,rev,sizeof(cmd),sizeof(cmd));
    
    switch (attackTarget->getType())
    {
      case zSceneEntry::SceneEntry_Player:
        {
          cmd.byAttackType = Cmd::ATTACKTYPE_U2U;
        }
        break;
      case zSceneEntry::SceneEntry_NPC:
        {
          cmd.byAttackType = Cmd::ATTACKTYPE_U2N;
        }
        break;
      default:
        {
          cmd.byAttackType = Cmd::ATTACKTYPE_U2U;
        }
        break;
    }
    

    zSkill *s = NULL;

    //s = zSkill::createTempSkill(this,381,1);
       if (selectByPercent(this->getPoison()))
     {
      s = zSkill::createTempSkill(this,388,1);
      cmd.wdMagicType = 388;
     }
       else if (selectByPercent(this->getLull()))
     {
       s = zSkill::createTempSkill(this,382,1);
       cmd.wdMagicType = 382;
     }
       else if (selectByPercent(this->getReel()))
     {
       s = zSkill::createTempSkill(this,386,1);
       cmd.wdMagicType = 386;
     }
       else if (selectByPercent(this->getChaos()))
     {
       s = zSkill::createTempSkill(this,390,1);
       cmd.wdMagicType = 390;
     }
       else if (selectByPercent(this->getCold()))
     {
       s = zSkill::createTempSkill(this,384,1);
       cmd.wdMagicType = 384;
     }
       else if (selectByPercent(this->getPetrify()))
     {
       s = zSkill::createTempSkill(this,389,1);
       cmd.wdMagicType = 389;
     }
       else if (selectByPercent(this->getBlind()))
     {
       s = zSkill::createTempSkill(this,391,1);
       cmd.wdMagicType = 391;
     }
       else if (selectByPercent(this->getStable()))
     {
       s = zSkill::createTempSkill(this,392,1);
       cmd.wdMagicType = 392;
     }
       else if (selectByPercent(this->getSlow()))
     {
       s = zSkill::createTempSkill(this,387,1);
       cmd.wdMagicType = 387;
     }

    if (s)
    {
      s->action(&cmd,sizeof(cmd));
      SAFE_DELETE(s);
    }
    if (this->skillValue.magicattack >0)
    {
      s = zSkill::createTempSkill(this,this->skillValue.magicattack,1);
      if (s)
      {
        s->action(&cmd,sizeof(cmd));
        SAFE_DELETE(s);
      }
    }
    if (this->skillValue.unitarybattle >0)
    {
      s = zSkill::createTempSkill(this,this->skillValue.unitarybattle,1);
      if (s)
      {
        s->action(&cmd,sizeof(cmd));
        SAFE_DELETE(s);
      }
    }
  }
}


void SceneUser::sendSkill(WORD wdSkillID,BYTE level,DWORD target,BYTE attackType,BYTE action)
{
  Cmd::stAttackMagicUserCmd cmd;

  if (0 == target)
  {
    cmd.dwDefenceTempID = this->tempid;
    cmd.byAttackType = Cmd::ATTACKTYPE_U2U;
    cmd.byAction = Cmd::Ani_Null;
  }
  else
  {
    cmd.dwDefenceTempID = target;
    cmd.byAttackType = attackType;
    cmd.byAction = action;
  }

  cmd.dwUserTempID = this->tempid;
  cmd.wdMagicType = wdSkillID;
  cmd.byDirect = this->getDir();
    
  zSkill *s = NULL;

  s = zSkill::createTempSkill(this,wdSkillID,level);
  if (s)
  {
    s->action(&cmd,sizeof(cmd));
    SAFE_DELETE(s);
  }
}

/**
  * \brief 设置死亡状态
  *
  *  把该玩家状态置为USTATE_DEATH,
  *  并把该玩家死亡的消息,通知给九屏玩家
  *  死亡后,如果玩家在马上,让玩家自动下马
  *
  * \return 无返回值
  */
void SceneUser::setDeathState()
{  
  if (angelMode) return;

  if (this->bombskillId>0)
    deathWaitTime = 2; //不等待立即复活2是特别那么写的
  else
    deathWaitTime = 300; //死亡等待5分钟,自动复活

  //取消交易状态
  if (tradeorder.hasBegin()) {
    tradeorder.cancel();
  }

  //取消摆摊
  privatestore.step(PrivateStore::NONE,this);
  
  //护镖则失败
  //if (guard) guard->reset();

//  //有马则下马
//  if (horse.mount())
//  {
//    //Cmd::stRideMapScreenUserCmd *rev = 0;//这个消息在ride方法里面没有用到
//    //ride(rev);
//    horse.mount(false,false);
//  }

  // 清除所有宠物
  this->killAllPets();
  if (guard)//死亡后镖车不移动
    guard->masterIsAlive = false;

  //清除阻挡
  this->scene->clearBlock(this->getPos());

  OnDie event(1);
  EventTable::instance().execute(*this,event);
  execute_script_event(this,"die");

  SceneUser *pDef = this;
  //清除被砍死的人的自卫列表
  pDef->pkState.clearProtect();
  pDef->charbase.goodness &= (~0x00FF0000);  

  Cmd::stMainUserDeathReliveUserCmd death;
  death.dwUserTempID = pDef->tempid;
  death.deathType = pDef->lastKiller?1:0;
#ifdef _DEBUG
  Zebra::logger->debug("%s 被恶意PK杀死",name);
#endif

  pDef->scene->sendCmdToNine(pDef->getPosI(),&death,sizeof(death),pDef->dupIndex);
  /*
  // \brief
  // 临时补血用
  pDef->charbase.hp = pDef->charstate.maxhp;
  pDef->charbase.mp = pDef->charstate.maxmp;
  pDef->charbase.sp = pDef->charstate.maxsp;
  // */
  pDef->charbase.hp=0;
  pDef->setState(SceneUser::SceneEntry_Death);
  pDef->showCurrentEffect(Cmd::USTATE_DEATH,true);
  
  this->skillStatusM.processDeath();
  
  if (npcdareflag)
  {
    Zebra::logger->info("[家族争夺NPC]家族[%u]中的角色%s战死",this->charbase.septid,this->name);
  }
  if (this->skillValue.nowrelive>0)
  {
    this->relive(Cmd::ReliveSkill,0,this->skillValue.nowrelive);
    this->skillValue.nowrelive=0;
  }

  if (this->skillValue.relive>0 && selectByPercent((int)(this->skillValue.relive)))
  {
    this->relive(Cmd::ReliveSkill,0,90);
    this->skillValue.relive=0;
  }
#ifdef _TEST_DATA_LOG
  writeCharTest(Cmd::Record::DEATH_WRITEBACK);
#endif // _TEST_DATA_LOG测试数据
}

/**
  * \brief 
  *
  *
  */
/*void SceneUser::Death()
{
  //清除阻挡状态
  this->scene->clearBlock(this->getPos());
}*/

/**
 * \brief Pk模式切换
 *
 * \rev 收到客户端发来的切换指令
 *
 * \return 切换是否成功
 */
bool SceneUser::switchPKMode(const Cmd::stPKModeUserCmd *rev)
{
  if (!rev)
  {
    return false;
  }
  using namespace Cmd;
  if (rev->byPKMode == 255)
  {
    this->pkMode = (this->pkMode + 1) % PKMODE_MAX;
  }
  else
  {
    this->pkMode = rev->byPKMode%PKMODE_MAX;
  }
  switch(this->pkMode)
  {
    case PKMODE_NORMAL:
      {
        Channel::sendSys(this,Cmd::INFO_TYPE_STATE,"PK切换模式为:和平模式.");
      }
      break;
    case PKMODE_ENTIRE:
      {
        Channel::sendSys(this,Cmd::INFO_TYPE_STATE,"PK切换模式为:全体模式.");
      }
      break;
    case PKMODE_TEAM:
      {
        Channel::sendSys(this,Cmd::INFO_TYPE_STATE,"PK切换模式为:组队模式.");
      }
      break;
    case PKMODE_TONG:
      {
        Channel::sendSys(this,Cmd::INFO_TYPE_STATE,"PK切换模式为:帮会模式.");
      }
      break;
    case PKMODE_SEPT:
      {
        Channel::sendSys(this,Cmd::INFO_TYPE_STATE,"PK切换模式为:家族模式.");
      }
      break;
    /*case PKMODE_SCHOOL:
      {
        Channel::sendSys(this,Cmd::INFO_TYPE_STATE,"PK切换模式为:师门模式.");
      }
      break;*/
    case PKMODE_COUNTRY:
      {
        Channel::sendSys(this,Cmd::INFO_TYPE_STATE,"PK切换模式为:国家模式.");
      }
      break;
    case PKMODE_GOODNESS:
      {
        Channel::sendSys(this,Cmd::INFO_TYPE_STATE,"PK切换模式为:善恶模式.");
      }
      break;
    case PKMODE_ALLY:
      {
        Channel::sendSys(this,Cmd::INFO_TYPE_STATE,"PK切换模式为:盟国模式.");
      }
      break;
    /*case PKMODE_CHALLENGE:
      {
        Channel::sendSys(this,Cmd::INFO_TYPE_STATE,"PK切换模式为:挑战模式.");
      }
      break;*/
    default:
      break;
  }

  {
    stPKModeUserCmd ret;
    ret.byPKMode = this->pkMode;
    //Zebra::logger->debug("switchPkMode=%u",this->pkMode);
    this->sendCmdToMe(&ret,sizeof(stPKModeUserCmd));

    return true;
  }
  return false;
}

/**
 * \brief 重新计算善恶度
 *
 * \return 始终返回TRUE
 */
bool SceneUser::checkGoodness()
{

  zRTime ctv;
  if ((charbase.goodness & 0x0000FFFF) != Cmd::GOODNESS_2_1)
  {
    if (ctv > pkState.tGood)
    {
      if ((charbase.goodness & 0x0000FFFF) >= (Cmd::GOODNESS_0 & 0x0000FFFF))
      {
        if ((charbase.goodness & 0x0000FFFF ) == 0x0000FFFF)
        {
          charbase.goodness &= 0xFFFF0000;
        }
        else
        {
          charbase.goodness ++;
        }
      }
      else
      {
        int num = 1;

        for (int i=0; i<num; i++)
        {
          charbase.goodness --;
          switch(charbase.goodness)
          {
            case Cmd::GOODNESS_2_1:
            case Cmd::GOODNESS_2_2:
            case Cmd::GOODNESS_3:
            case Cmd::GOODNESS_4:
            case Cmd::GOODNESS_5:
            case Cmd::GOODNESS_6:
              {
                //通知客户端属性变化
                // mark
                //Cmd::stAddUserMapScreenUserCmd send;
                //this->full_t_MapUserData(send.data);
                //this->scene->sendCmdToNine(getPosI(),&send,sizeof(send),false);
                //Zebra::logger->debug("%s(%d)pk状态改变(%d-->%d)",this->name,this->id,temp,this->charbase.goodness);
                //this->reSendMyMapData();
                this->sendGoodnessToNine();
              }
              break;
            default:
              break;
          }
          if (0==(charbase.goodness & 0x0000FFFF))
          {
            //clearUState(Cmd::USTATE_XIXINGEMIA);
            break;
          }
        }
      }

      //ScenePk::sendChangedUserData(this);
      //Zebra::logger->debug("当前善恶度(%s(%ld),%X)",this->name,this->id,charbase.goodness);
      pkState.tGood = ctv;
      pkState.tGood.addDelay(ScenePkState::goodnessPeriod);
    }
  }
  if (charbase.goodness & 0x00FF0000)
  {
    if (ctv > pkState.tProtect)
    {
      if (pkState.cancelProtect(this))
      {
        //通知客户端属性变化
        // mark
        //Cmd::stAddUserMapScreenUserCmd send;
        //this->full_t_MapUserData(send.data);
        //this->scene->sendCmdToNine(getPosI(),&send,sizeof(send),false);
        //this->reSendMyMapData();
        this->sendGoodnessToNine();
        
        //Zebra::logger->debug("取消自卫状态(%s(%ld),%X)",this->name,this->id,this->charbase.goodness);
      }
      pkState.tProtect = ctv;
      pkState.tProtect.addDelay(pkState.protectPeriod);
    }
  }
  return true;
}

/**
 * \brief 检查自己是不是在对对方处于自卫状态
 *
 * \param pThis: 对方玩家
 * \param defid: 对方玩家ID
 *
 */
bool ScenePkState::deathUserProtect(SceneUser *pThis,DWORD defid)
{
  protect_time=0;
  pThis->charbase.goodness &= (~Cmd::GOODNESS_DEF);  
  pThis->charbase.goodness &= (~Cmd::GOODNESS_ATT);  
  return true;
}

/**
 * \brief 重新计算自卫时间,如果为零,将取消自卫
 *
 * \param pThis:对方玩家
 *
 * \return 如果自卫时间并未结束,返回FALSE,否则返回TRUE
 */
bool ScenePkState::cancelProtect(SceneUser *pThis,DWORD time)
{
  if (time)
  {
    protect_time=0;
  }
  else if (protect_time)
  {
    protect_time --;
  }
  //Zebra::logger->debug("%s(%d)灰名时间:%d",pThis->name,pThis->id,protect_time);
  if (!protect_time)
  {
    pThis->charbase.goodness &= ~0x00020000;
    pThis->charbase.goodness &= ~0x00010000;
    return true;
  }
  return false;
}

/**
 * \brief 用户死亡处理
 *
 * \pDef 需要宣告死亡的用户
 *
 * \return 处理是否成功
 */
bool ScenePk::attackDeathUser(SceneUser *pAtt,SceneUser *pDef)
{
  //保护状态不计算pk值
  bool guard = pDef->issetUState(Cmd::USTATE_GUARD);
  //应该加锁,需要对pDef的数据进行修改
  // 社会关系交战状态不计算善恶度（歹徒,恶魔等）
  bool dare = pAtt->isWar(pDef);
  // 护宝状态不计算pk值
  bool gem = false;
  // 捕头状态不计算pk值
  bool catcher = false;

  if ((pDef->issetUState(Cmd::USTATE_TOGETHER_WITH_TIGER)
        || pDef->issetUState(Cmd::USTATE_TOGETHER_WITH_DRAGON))
      && pAtt->charbase.country == pDef->charbase.country)
  {
    gem = true;
  }

  if (pAtt->isCatcherState() && pAtt->charbase.country == pDef->charbase.country)
  {
    catcher = true;
  }

  if (pDef->getState() == SceneUser::SceneEntry_Death)
  {
    return true;
  }
#define attGood ((SDWORD)(pAtt->charbase.goodness & 0x0000FFFF))
#define defGood ((SDWORD)(pDef->charbase.goodness & 0x0000FFFF))
#define attSub  ((SDWORD)(pAtt->charbase.goodness & 0xFFFF0000))
#define defSub  ((SDWORD)(pDef->charbase.goodness & 0xFFFF0000))
#define bitMask  0x00FF0000

  //检查是否是自卫砍死人
  /*
     bool protection = false;
     if (attSub & Cmd::GOODNESS_DEF)
     {
     protection = pAtt->pkState.deathUserProtect(pAtt,pDef->id);
     }

  //如果不是自卫
  if (!protection)
  // */

  if (!pAtt->scene->isNoRedScene())
  {
    DWORD protect_state=pAtt->charbase.goodness & 0x00FF0000;
    if (!(defSub & Cmd::GOODNESS_ATT) && !guard  &&  !dare && !gem && !catcher
        && (pAtt->charbase.country == pDef->charbase.country || pAtt->charbase.country == PUBLIC_COUNTRY)
        && (!pDef->scene->checkZoneType(pDef->getPos(),ZoneTypeDef::ZONE_SPORTS) 
          &&!pAtt->scene->checkZoneType(pAtt->getPos(),ZoneTypeDef::ZONE_SPORTS))
        && (pAtt != pDef))
    {
      //pAtt->pkState.cancelProtect(pAtt,0);

      switch(pAtt->getGoodnessState())
      {
        case Cmd::GOODNESS_2_1: // 普通
          {
            if (pDef->getGoodnessState() <= 60)
            {
              DWORD temp=pAtt->charbase.goodness;
              pAtt->charbase.goodness = (DWORD)Cmd::GOODNESS_3 + pDef->getPkAddition();
              Zebra::logger->debug("%s(%d)pk状态改变(%d-->%d)",pAtt->name,pAtt->id,temp,pAtt->charbase.goodness);
            }
            else//杀红名
            {
              pDef->lastKiller = 0;

              if ((attSub & Cmd::GOODNESS_2_2) == 0)
              {
                pAtt->charbase.goodness = (DWORD)Cmd::GOODNESS_2_2;
                zRTime ctv;
                pAtt->pkState.tGoodNormal = ctv;
              }
              else
              {
                zRTime ctv;
                ctv.addDelay(-3600 * 1000);
                if (ctv <= pAtt->pkState.tGoodNormal)
                {
                  DWORD temp=pAtt->charbase.goodness;
                  pAtt->charbase.goodness = (pAtt->charbase.goodness & 0x00FF0000) | (DWORD)Cmd::GOODNESS_1;
                  Zebra::logger->debug("%s(%d)pk状态改变(%d-->%d)",pAtt->name,pAtt->id,temp,pAtt->charbase.goodness);
                  //pAtt->charstate.lucky ++;
                }
                else
                {
                  pAtt->pkState.tGoodNormal = ctv;
                }
              }
            }
          }
          break;

        case Cmd::GOODNESS_3://歹徒
          {
            DWORD temp=pAtt->charbase.goodness;
            pAtt->charbase.goodness = (DWORD)Cmd::GOODNESS_4 + pDef->getPkAddition();
            Zebra::logger->debug("%s(%d)pk状态改变(%d-->%d)",pAtt->name,pAtt->id,temp,pAtt->charbase.goodness);
          }
          break;

        case Cmd::GOODNESS_4://恶徒
          {
            DWORD temp=pAtt->charbase.goodness;

            //红名杀绿名PK值增加300
            if ((short)(pDef->charbase.goodness & 0x0000FFFF) < 0)
            {
              pAtt->charbase.goodness += 300;
            }else{
              pAtt->charbase.goodness = (DWORD)Cmd::GOODNESS_5 + pDef->getPkAddition();
            }
            Zebra::logger->debug("%s(%d)pk状态改变(%d-->%d)",pAtt->name,pAtt->id,temp,pAtt->charbase.goodness);
            //pAtt->charstate.lucky --;
          }
          break;

        case Cmd::GOODNESS_5://恶魔
          {
            DWORD temp=pAtt->charbase.goodness;

            if ((short)(pDef->charbase.goodness & 0x0000FFFF) < 0)
            {
              pAtt->charbase.goodness += 300;
            }else{
              pAtt->charbase.goodness = (DWORD)Cmd::GOODNESS_6 + pDef->getPkAddition();
            }
            Zebra::logger->debug("%s(%d)pk状态改变(%d-->%d)",pAtt->name,pAtt->id,temp,pAtt->charbase.goodness);
            //pAtt->charstate.lucky --;
          }
          break;
        case Cmd::GOODNESS_6://恶魔
          {
            DWORD temp=pAtt->charbase.goodness;

            if ((short)(pDef->charbase.goodness & 0x0000FFFF) < 0)
            {
              pAtt->charbase.goodness += 300;
            }else{
              pAtt->charbase.goodness += 30;
            }

            if (pAtt->charbase.goodness > MAX_GOODNESS)
            {
              pAtt->charbase.goodness = MAX_GOODNESS;
            }
            Zebra::logger->debug("%s(%d)pk状态改变(%d-->%d)",pAtt->name,pAtt->id,temp,pAtt->charbase.goodness);
            //pAtt->charstate.lucky --;
          }
          break;

        case Cmd::GOODNESS_1://侠士
          {
            if (pDef->getGoodnessState() > (short)Cmd::GOODNESS_3 && pDef->getGoodnessState() < (short)MAX_GOODNESS)
            {
              pAtt->charbase.goodness = (pAtt->charbase.goodness & 0x00FF0000) | (DWORD)Cmd::GOODNESS_0;
              //pAtt->charstate.lucky ++;
            }
            else
            {
              DWORD temp=pAtt->charbase.goodness;
              pAtt->charbase.goodness = (DWORD)Cmd::GOODNESS_3 + pDef->getPkAddition();
              Zebra::logger->debug("%s(%d)pk状态改变(%d-->%d)",pAtt->name,pAtt->id,temp,pAtt->charbase.goodness);
            }
          }
          break;

        case Cmd::GOODNESS_0://英雄
          {
            if (pDef->getGoodnessState() <= (short)Cmd::GOODNESS_3)
            {
              Zebra::logger->debug("%s(%d)pk状态改变(%d-->%d)",pAtt->name,pAtt->id,pAtt->charbase.goodness,(DWORD)Cmd::GOODNESS_3 + pDef->getPkAddition());
              pAtt->charbase.goodness = (DWORD)Cmd::GOODNESS_3 + pDef->getPkAddition();
            }
          }
          break;
        default:
          break;
      }
      if (!pDef->isRedNamed(false) && !pAtt->mask.is_masking())
          pDef->lastKiller = pAtt->tempid;
    }

    if (pAtt->mask.is_masking() ) {
      Channel::sendSys(pDef,Cmd::INFO_TYPE_FAIL,"你被蒙面人杀死了." );  
    }else {
      Channel::sendSys(pDef,Cmd::INFO_TYPE_FAIL,"你被%s杀死了.",pAtt->name);

      if (!guard && !dare && !gem && !catcher &&
          !pDef->scene->checkZoneType(pDef->getPos(),ZoneTypeDef::ZONE_SPORTS)&&
          !pAtt->scene->checkZoneType(pAtt->getPos(),ZoneTypeDef::ZONE_SPORTS))
      {
        if (pDef->mask.is_masking())
          Channel::sendSys(pAtt,Cmd::INFO_TYPE_STATE,"你杀死了 蒙面人,你成为%s",pAtt->getGoodnessName());
        else if (pAtt->charbase.country == pDef->charbase.country )
          Channel::sendSys(pAtt,Cmd::INFO_TYPE_STATE,"你杀死了%s,你成为%s",pDef->name,pAtt->getGoodnessName());
        else
          Channel::sendSys(pAtt,Cmd::INFO_TYPE_STATE,"你杀死了%s",pDef->name);
        //Channel::sendNine(pAtt,"%s杀死了%s,成为%s",pAtt->name,pDef->name,pAtt->getGoodnessName());
      }
      else if (dare)
      {
        if (pDef->mask.is_masking())
          Channel::sendSys(pAtt,Cmd::INFO_TYPE_STATE,"你在对战中杀死了 蒙面人");
        else
          Channel::sendSys(pAtt,Cmd::INFO_TYPE_STATE,"你在对战中杀死了%s",pDef->name);
        //Channel::sendNine(pAtt,"%s在对战中杀死了%s",pAtt->name,pDef->name);

      }
    }

    // mark
    //Cmd::stAddUserMapScreenUserCmd send;
    //pAtt->full_t_MapUserData(send.data);
    //pAtt->scene->sendCmdToNine(pAtt->getPosI(),&send,sizeof(send),false);
    //进行国家功勋值的计算
    if (pAtt->charbase.country!=pDef->charbase.country && pAtt->charbase.country!=PUBLIC_COUNTRY)
    {  
      int level_diff = (int)pAtt->charbase.level - (int)pDef->charbase.level;
      if (level_diff <= 0)
      {
        pAtt->charbase.exploit+=(2*exploit_arg);
        Channel::sendSys(pAtt,Cmd::INFO_TYPE_GAME,"您获得了 %d 点功勋值",2);

      }
      else if (level_diff<20)
      {
        pAtt->charbase.exploit+=(1*exploit_arg);
        Channel::sendSys(pAtt,Cmd::INFO_TYPE_GAME,"您获得了 %d 点功勋值",1);
      }
      //在国战或本国杀外国人,即使被杀的人比自己低20级以上功勋也值不减少
      if (pAtt->scene->getCountryID() == pAtt->charbase.country ||
          pAtt->isWarRecord(Cmd::COUNTRY_FORMAL_DARE,pDef->charbase.country))
      {
      }else{
        //被杀者等级<自己等级-60 则扣除5点功勋值
        if (level_diff>60)
        {
          if (pAtt->charbase.exploit > (5*exploit_arg) )
          {
            pAtt->charbase.exploit -= (5*exploit_arg);
          }
          else
          {
            pAtt->charbase.exploit = 0;
          }

          Channel::sendSys(pAtt,Cmd::INFO_TYPE_GAME,"您失去了 %d 点功勋值",5);
        }

        if (level_diff <= 60 && level_diff>40)
        {
          if (pAtt->charbase.exploit > (2*exploit_arg) )
          {
            pAtt->charbase.exploit -= (2*exploit_arg);
          }
          else
          {
            pAtt->charbase.exploit = 0;
          }
  
          Channel::sendSys(pAtt,Cmd::INFO_TYPE_GAME,"您失去了 %d 点功勋值",2);
        }

        if (level_diff <= 40 && level_diff>=20)
        {
          if (pAtt->charbase.exploit > (1*exploit_arg) )
          {
            pAtt->charbase.exploit -= (1*exploit_arg);
          }
          else
          {
            pAtt->charbase.exploit = 0;
          }
  
          Channel::sendSys(pAtt,Cmd::INFO_TYPE_GAME,"您失去了 %d 点功勋值",1);
        }  
      }

      if (level_diff < 0 && level_diff >= -40)
      {
        if (pDef->charbase.exploit > (1*exploit_arg) )
        {
          pDef->charbase.exploit = pDef->charbase.exploit - (1*exploit_arg);
        }
        else
        {
          pDef->charbase.exploit = 0;
        }
        Channel::sendSys(pDef,Cmd::INFO_TYPE_GAME,"您失去了 %d 点功勋值",1);
      }
      if (level_diff < -40)
      {
        if (pDef->charbase.exploit > (2*exploit_arg) )
        {
          pDef->charbase.exploit = pDef->charbase.exploit - (2*exploit_arg);
        }
        else
        {
          pDef->charbase.exploit = 0;
        }
        Channel::sendSys(pDef,Cmd::INFO_TYPE_GAME,"您失去了 %d 点功勋值",2);
      } 
      
      // 回写档案
      //pAtt->save(Cmd::Record::OPERATION_WRITEBACK);
      //pDef->save(Cmd::Record::OPERATION_WRITEBACK);
    }

    //保证灰名状态存在
    if (pAtt->charbase.goodness <= Cmd::GOODNESS_3)
    {
      pAtt->charbase.goodness |= protect_state;
    }
    else
    {
      pAtt->pkState.clearProtect();
    }
    if (!pDef->scene->checkZoneType(pDef->getPos(),ZoneTypeDef::ZONE_SPORTS) 
        && !pAtt->scene->checkZoneType(pAtt->getPos(),ZoneTypeDef::ZONE_SPORTS) 
        && !((pDef->getGoodnessState() == Cmd::GOODNESS_2_1 || pDef->isPkAddition()) && dare) )
    {
      pDef->lostObject(pAtt);
    }          
  }

  if (pAtt->getGoodnessState()>(short)Cmd::GOODNESS_3)
  {
    pAtt->charbase.pkaddition=0;
    if (pAtt->issetUState(Cmd::USTATE_PK))
    {
      pAtt->clearUState(Cmd::USTATE_PK);
      pAtt->sendtoSelectedPkAdditionState();
    }
  }
  pAtt->reSendMyMapData();

  if (dare)
  {// 向会话服务器发送对战计分命令
    Cmd::Session::t_darePk_SceneSession send;
    send.attID = pAtt->id;
    send.defID = pDef->id;
    sessionClient->sendCmd(&send,sizeof(send));
  }

  if (gem)
  {// 向会话服务器发送护宝重置命令
    bool dragon = pDef->issetUState(Cmd::USTATE_TOGETHER_WITH_DRAGON);
    bool tiger = pDef->issetUState(Cmd::USTATE_TOGETHER_WITH_TIGER);
    Cmd::Session::t_ChangeGemState_SceneSession change_send;

    change_send.fromUserID = pDef->id;
    change_send.toUserID = pAtt->id;

    if (tiger && dragon)
    {
      int choice_state = randBetween(1,2);
      change_send.dwState = choice_state;
    }
    else if (dragon)
    {
      change_send.dwState = 1;
    }
    else if (tiger)
    {
      change_send.dwState = 2;
    }

    sessionClient->sendCmd(&change_send,sizeof(change_send));
  }

  if (pDef->miniGame)
  {
    Dice * temp = pDef->miniGame;
    pDef->miniGame->endGame();

    delete temp;
    pDef->miniGame = 0;
  }

  pAtt->leaveBattle();
  pDef->leaveBattle();
  pDef->clearDefTarget();
  pDef->setDeathState();
  /// 死亡时清除药品作用
  pDef->leechdom.clear();

  if (attGood>=1500 && attGood<=(SDWORD)MAX_GOODNESS && pAtt->scene->getRealMapID()!=189)
  {
    Scene * s=SceneManager::getInstance().getSceneByName("中立区·牢狱");
    if (s)
    {
      bool suc = pAtt->changeMap(s,zPos(80,70));
      if (!suc)
        Zebra::logger->error("%s PK值 %u,送往监狱失败,目的 %s (%d,%d)",pAtt->name,pAtt->charbase.goodness,s->name,100,100);
      else
        Zebra::logger->error("%s PK值 %u,送往监狱",pAtt->name,pAtt->charbase.goodness);
    }
    else
    {
      //if (pAtt->guard && pAtt->guard->canMove()) pAtt->saveGuard = true;//使镖车跟随指令使用者
      //if (pAtt->adoptList.size()) pAtt->saveAdopt = true;
      Cmd::Session::t_changeScene_SceneSession cmd;
      cmd.id = pAtt->id;
      cmd.temp_id = pAtt->tempid;
      cmd.x = 80;
      cmd.y = 70;
      cmd.map_id = 0;
      cmd.map_file[0] = '\0';
      strncpy((char *)cmd.map_name,"中立区·牢狱",MAX_NAMESIZE);
      sessionClient->sendCmd(&cmd,sizeof(cmd));

    }

    Channel::sendCountryInfo(pAtt->charbase.country,Cmd::INFO_TYPE_EXP,"%s 恶名昭著,现已捉拿归案。",pAtt->name);
  }

#ifdef _DEBUG
  Zebra::logger->debug("%s 被 %s 杀死,凶手tempid",pDef->name,pAtt->name,pDef->lastKiller);
#endif
  return true;
}

/**
 * \brief NPC死亡处理
 * \pAtt 攻击者
 * \pDef 需要宣告死亡的NPC
 *
 * \return 处理是否成功
 */
bool ScenePk::attackDeathNpc(SceneEntryPk *pAtt,SceneNpc *pDef)
{
  if (pAtt)
  {
    if (zSceneEntry::SceneEntry_NPC==pAtt->getType())
    {
      if (!((SceneNpc *)pAtt)->chaseSecondTarget() && !((SceneNpc *)pAtt)->chaseItsMaster())
        pAtt->leaveBattle();
    }
  }

  pDef->on_death(pAtt);
  pDef->death(SceneTimeTick::currentTime);

  //向王城、边境、东郊、南郊通知
  if (pDef->id==COUNTRY_SEC_GEN)
  {
    SceneEntryPk * am = pAtt->getTopMaster();

    if (am->getType()==zSceneEntry::SceneEntry_Player
        && pDef->scene->getCountryID()!=((SceneUser *)am)->charbase.country)
    {
      char buf[MAX_CHATINFO];
      bzero(buf,sizeof(buf));
      _snprintf(buf,MAX_CHATINFO-1,"%s(%s) 在 %s(%u,%u) 杀死 %s"
          ,am->name,SceneManager::getInstance().getCountryNameByCountryID(((SceneUser *)am)->charbase.country)
          ,pDef->scene->name,pDef->getPos().x,pDef->getPos().y,pDef->name);

      DWORD high = (pDef->scene->getCountryID()<<16);
      Channel::sendMapInfo(high+WANGCHENG_MAP_ID,Cmd::INFO_TYPE_EXP,buf);
      Channel::sendMapInfo(high+DONGJIAO_MAP_ID,Cmd::INFO_TYPE_EXP,buf);
      Channel::sendMapInfo(high+NANJIAO_MAP_ID,Cmd::INFO_TYPE_EXP,buf);
      Channel::sendMapInfo(high+BIANJING_MAP_ID,Cmd::INFO_TYPE_EXP,buf);
    }
  }

  Cmd::stNpcDeathUserCmd death;
  death.dwNpcTempID = pDef->tempid;
  pDef->scene->sendCmdToNine(pDef->getPosI(),&death,sizeof(death),pDef->dupIndex);

  pDef->setUState(Cmd::USTATE_DEATH);
  Cmd::stRTMagicPosUserCmd ret;
  pDef->full_stRTMagicPosUserCmd(ret);
  pDef->scene->sendCmdToNine(pDef->getPosI(),&ret,sizeof(ret),pDef->dupIndex);

  SceneEntryPk * m = pDef->getTopMaster();
  if (m && zSceneEntry::SceneEntry_NPC==m->getType())
  {
    if (pAtt) pAtt->reduceGoodness(pDef);
    if (Cmd::PET_TYPE_NOTPET==pDef->getPetType()) pDef->distributeExp();
  }

  pDef->leaveBattle(true);
  pDef->sendMeToNine();
  //pDef->setStateToNine(Cmd::USTATE_DEATH);
  return true;
}

/* 
 * 发送新手体验卡的黄金装备
 *
 */
void SceneUser::sendGiftEquip(WORD level)
{
}

/*
 * 奖励宠物的修炼时间
 * petPoint以秒为单位记录
 */
void SceneUser::givePetPoint()
{
  if (Zebra::global["pet_point"]!="true") return;
  DWORD hour = 0;
  switch (charbase.level)
  {
    case 30:
    case 40:
    case 50:
    case 60:
    case 70:
    case 80:
    case 90:
    case 100:
    case 110:
    case 120:
      hour = 10+(charbase.level/10-3)*5;
      //新手卡用户30级多送10小时
      if ((charbase.accPriv&ACCPRIV_NEWBIE_EQUIP_AT_5_15) && charbase.level==30)
        hour += 10;
      //80级以上每级多送5小时
      if (charbase.level>=80)
          hour += ((charbase.level/10-7)*5);
      charbase.petPoint += hour*3600;

      char text[MAX_CHATINFO];
      bzero(text,sizeof(text));
      _snprintf(text,sizeof(text),"亲爱的玩家:\n\t恭喜你已经达到%u级,系统将为您当前的替身宝贝充值%u小时修炼时间以减轻你您练级的强度。如果您现在没有替身宝贝则于购买后第一时间给予\n\t\t\t\t\t\t英雄无双运营团队",charbase.level,hour);
      sendMail("英雄无双运营",0,name,id,Cmd::Session::MAIL_TYPE_SYS,0,0,text);

      
	  if(cartoonList.empty())
		  return;


      if (cartoonList.begin()->second.state==Cmd::CARTOON_STATE_PUTAWAY && !cartoon)
      {
        //升级奖励的宠物修炼时间,一次性加到宠物身上
        cartoonList.begin()->second.time += charbase.petPoint;

        Cmd::Session::t_chargeCartoon_SceneSession send;
        send.masterID = id;
        send.cartoonID = cartoonList.begin()->first;
        send.time = charbase.petPoint;
        sessionClient->sendCmd(&send,sizeof(send));

        Cmd::stAddCartoonCmd ac;
        ac.isMine = true;
        ac.cartoonID = cartoonList.begin()->first;
        ac.data = cartoonList.begin()->second;
        sendCmdToMe(&ac,sizeof(ac));

        charbase.petPoint = 0;

        Zebra::logger->info("[宠物]%s %u级,其宠物%s(%u)获得 %u 秒修炼时间",name,charbase.level,cartoonList.begin()->second.name,cartoonList.begin()->first,hour*3600);
      }
      break;
    default:
      return;
  }
}

/**
 * \brief 玩家等级,升级函数
 *
 * 如果升级成功就通知客户端
 * 
 * \param num: 升级后剩余经验值
 *
 * \return 升级是否成功
 */
bool SceneUser::upgrade(DWORD num)
{
  int old_level = charbase.level;
  if (num > 0)
  {
    while(num--)
    {
      if (charbase.level < max_level)
      {
        charbase.level ++;
        OnOther event(3);
        EventTable::instance().execute(*this,event);
        charbase.skillpoint ++;
        charbase.points += (charbase.level-1)/20 +3;
      }
      else
      {
        break;
      }
      setupCharBase();

      sendGiftEquip(charbase.level);//送各种道具卡送的装备
      givePetPoint();//送宠物修炼时间
      if (30==charbase.level && (atoi(Zebra::global["service_flag"].c_str())&Cmd::Session::SERVICE_HORSE))//30级送枣红马
      {
          horse.horse(3000);
          Channel::sendSys(this,Cmd::INFO_TYPE_GAME,"你获得了系统赠送的一匹枣红马");
      }
    }
  }
  else
  {
    while(charbase.exp >= charstate.nextexp)
    {
      if (charbase.level < max_level)
      {
        charbase.level ++;
        OnOther event(3);
        EventTable::instance().execute(*this,event);
        charbase.skillpoint ++;
        charbase.points += (charbase.level-1)/20 +3;
      }
      else
      {
        break;
      }
      charbase.exp -= charstate.nextexp;
      if (charbase.level == max_level) charbase.exp = 0;
      setupCharBase();

      sendGiftEquip(charbase.level);//送各种道具卡送的装备
      givePetPoint();//送宠物修炼时间
      if (30==charbase.level && (atoi(Zebra::global["service_flag"].c_str())&Cmd::Session::SERVICE_HORSE))//30级送枣红马
      {
          horse.horse(3000);
          Channel::sendSys(this,Cmd::INFO_TYPE_GAME,"你获得了系统赠送的一匹枣红马");
      }
    }
  }

  if (charbase.level == max_level) Channel::sendSys(this,Cmd::INFO_TYPE_EXP,"恭喜您已经达到顶级%u级",max_level);

  if (old_level < 20 && this->charbase.level >= 20)
  {
    Channel::sendSys(this,Cmd::INFO_TYPE_GAME,"您现在可以购买一个新的仓库了");    
  }
  if (old_level < 40  && this->charbase.level >= 40)
  {
    Channel::sendSys(this,Cmd::INFO_TYPE_GAME,"您现在可以购买一个新的仓库了");    
  }
  if (charbase.level>20 && this->charbase.septid)
  {
    Cmd::Session::t_OpRepute_SceneSession send;
    send.dwSeptID = this->charbase.septid;
    send.dwRepute = 1;
    sessionClient->sendCmd(&send,sizeof(send));
  }
  
    
  charbase.hp = charstate.maxhp;
  charbase.sp = charstate.maxsp;
  charbase.mp = charstate.maxmp;
  ScenePk::sendChangedUserData(this);
  Cmd::stLevelUpUserCmd stl;
  stl.dwUserTempID = this->tempid;
  this->scene->sendCmdToNine(this->getPosI(),&stl,sizeof(stl),this->dupIndex);
  Zebra::logger->info("用户(%s(%ld))升到%ld级",this->name,this->id,charbase.level);
  Cmd::Session::t_levelupNotify_SceneSession send;
  send.dwUserID = charbase.id;
  send.level = charbase.level;
  send.qwExp = charbase.exp;
  sessionClient->sendCmd(&send,sizeof(send));

  for (cartoon_it it=cartoonList.begin(); it!=cartoonList.end(); it++)
    it->second.masterLevel = charbase.level;
  if (cartoon)
    cartoon->setCartoonData(cartoonList[cartoon->getCartoonID()]);

#ifdef _TEST_DATA_LOG
  writeCharTest(Cmd::Record::LEVELUP_WRITEBACK);
#endif // _TEST_DATA_LOG测试数据
  //ScenePk::attackRTExp(this,charbase.exp);
  return true;
}

/**
 * \brief 通知用户属性发生变化
 *
 * \pUser 属性发生变化的用户
 * \return 属性修改是否成功 
 */
bool  ScenePk::sendChangedUserData(SceneUser *pUser)
{
  Cmd::stMainUserDataUserCmd toDef;
  pUser->full_t_MainUserData(toDef.data);
  pUser->sendCmdToMe(&toDef,sizeof(toDef));

  pUser->sendMeToNine();

  pUser->sendInitHPAndMp();

  //通知队友属性变化
  TeamManager * team = SceneManager::getInstance().GetMapTeam(pUser->TeamThisID);

  if(team)
	  team->sendtoTeamCharData(pUser);

  return true;
}

/**
 * \brief 用户攻击指令失败时返回
 * 
 *
 * \rev 攻击失败的指令
 * \pAtt 发出攻击者
 * \isTargetU 目标攻击是用户还是npc
 * \return 始终返回true
 */
bool ScenePk::attackFailToMe(const Cmd::stAttackMagicUserCmd *rev,SceneEntryPk * pAtt,bool failed,bool me)
{
#ifdef _DEBUG
  //Zebra::logger->debug("%s 攻击失败",pAtt->name);
#endif

  if (rev &&(rev->wdMagicType == 351 || rev->wdMagicType == 352)) //人物反弹技能状态不返回失败
  {
    return true;
  }

  Cmd::stRTMagicUserCmd ret;
  ret.dwUserTempID = rev->dwDefenceTempID;
  ret.dwSrcTempID = pAtt->tempid;

  if (rev->byAttackType == Cmd::ATTACKTYPE_U2U || 
    rev->byAttackType == Cmd::ATTACKTYPE_N2U)
  {
    ret.byTarget = Cmd::MAPDATATYPE_USER;
  }
  else
  {
	  ret.byTarget = Cmd::MAPDATATYPE_NPC;
  }

  if (pAtt->getType() == zSceneEntry::SceneEntry_Player )
	  ret.bySrc = Cmd::MAPDATATYPE_USER;
  else
  {
	  if( ((SceneNpc *)pAtt)->npc->kind == NPC_TYPE_GHOST )
		  ret.bySrc = Cmd::MAPDATATYPE_USER;
	  else
		  ret.bySrc = Cmd::MAPDATATYPE_NPC;
  }
  ret.byLuck = 0;
  ret.byDirect = pAtt->getDir();
  ret.dwHP = 0;
  ret.sdwHP = 0;

  if (failed)
  {
    ret.byRetcode = Cmd::RTMAGIC_FAILURE;
  }
  else
  {
    ret.byRetcode = Cmd::RTMAGIC_DUCK;
  }

  Cmd::stAttackMagicUserCmd attCmd;
  bcopy(rev,&attCmd,sizeof(attCmd),sizeof(attCmd));
  attCmd.dwUserTempID = pAtt->tempid;

  SceneEntryPk *pEntry = pAtt->getTopMaster();
  if (pEntry&&pEntry->getType() == zSceneEntry::SceneEntry_Player)
  {
    SceneUser *pUser = (SceneUser *)pEntry;
    if (pUser)
    {
      pUser->sendCmdToMe(&ret,sizeof(ret));
      //if (me)
      //  pUser->sendCmdToMe(&attCmd,sizeof(attCmd));
      //else
      //  pAtt->scene->sendCmdToNine(pAtt->getPosI(),&attCmd,sizeof(attCmd),false);

    }
  }
  else
  {
    //临时修改等待策划定案
    pAtt->scene->sendCmdToNine(pAtt->getPosI(),&attCmd,sizeof(attCmd),pAtt->dupIndex);

  }

  return true;
}

/**
 * \brief 通知经验值发生改变
 *
 * \pUser 用户
 * \return 无
 */
/*
void ScenePk::attackRTExp(SceneUser *pUser,DWORD exp,DWORD dwTempID,BYTE byType)
{
  Cmd::stObtainExpUserCmd ret;
  ret.dwTempID = dwTempID;        ** 经验值来源临时编号 *
  ret.byType = byType;        ** 经验值来源 enumMapDataType *
  //  ret.dwUserTempID = pUser->tempid;
  ret.dwExp = exp;
  if (pUser->charbase.exp >= pUser->charstate.nextexp)
  {
    ret.dwUserExp = pUser->charbase.exp-pUser->charstate.nextexp;
  }
  else
  {
    ret.dwUserExp = pUser->charbase.exp;
  }
#ifdef _DEBUG
  Zebra::logger->info("[发送经验增加通知]获得经验：%u 用户当前经验：%u",ret.dwExp,ret.dwUserExp);
#endif
  pUser->sendCmdToMe(&ret,sizeof(ret));
}
*/
/**
 * \brief 更改用户HP和MP
 *
 * 通知用户及team,HP和MP的改变
 *
 * \pUser 用户
 * \wdHP 用户当前HP
 * wdMP 用户当前MP
 *
 * \return 是否成功
 */
void ScenePk::attackRTHpAndMp(SceneUser *pUser)
{
  Cmd::stSetHPAndMPDataUserCmd ret;
  ret.dwHP = pUser->charbase.hp;
  ret.dwMP = pUser->charbase.mp;
  //ret.dwSP = pUser->charbase.sp;
  pUser->sendCmdToMe(&ret,sizeof(ret));

  TeamManager * team = SceneManager::getInstance().GetMapTeam(pUser->TeamThisID);
  if(team)
	  team->sendtoTeamCharData(pUser);

  pUser->sendtoSelectedHpAndMp();
}

/**
 * \brief 攻击动作返回
 *
 * \rev 收到的攻击动作
 * \pAtt 攻击方
 * \pDef 被攻击方
 * \wdHP 本次攻击掉血值
 * \byLuck 本次攻击的幸运值
 *
 * \return 广播是否成功
 */
bool ScenePk::attackRTCmdToNine(const Cmd::stAttackMagicUserCmd *rev,SceneEntryPk *pAtt,SceneEntryPk *pDef,const SDWORD sdwHP,BYTE byLuck)
{
  Cmd::stRTMagicUserCmd ret;
  Cmd::stRTOtherMagicUserCmd ret1;
  ret.dwUserTempID = pDef->tempid;
  ret.dwSrcTempID = pAtt->tempid;
  SceneEntryPk *pAttMaster=NULL;
  SceneEntryPk *pDefMaster=NULL;

  switch(pAtt->getType())
  {
  case zSceneEntry::SceneEntry_Player:  /** 玩家角色*/
	  ret.bySrc = Cmd::MAPDATATYPE_USER;
	  break;
  case zSceneEntry::SceneEntry_NPC:    /** NPC*/
	  {
		  if( ((SceneNpc *)pAtt)->npc->kind == NPC_TYPE_GHOST )
		  {
			  pAttMaster=pAtt->getTopMaster(); 
			  ret.bySrc = Cmd::MAPDATATYPE_USER;
		  }
		  else
		  {
			  pAttMaster=pAtt->getTopMaster(); 
			  ret.bySrc = Cmd::MAPDATATYPE_NPC;
		  }
	  }
      break;
      //    case zSceneEntry::SceneEntry_Build:    /**< 建筑*/
      //      break;
      //    case zSceneEntry::SceneEntry_Object:  /**< 地上物品*/
      //      break;
    default:
      ret.bySrc = Cmd::MAPDATATYPE_NPC;
      break;
  }

  switch(pDef->getType())
  {
    case zSceneEntry::SceneEntry_Player:  /** 玩家角色*/
      ret.byTarget = Cmd::MAPDATATYPE_USER;
      ret.dwHP = ((SceneUser *)pDef)->charbase.hp;
      break;
    case zSceneEntry::SceneEntry_NPC:    /** NPC*/
      {
		  if( ((SceneNpc *)pDef)->npc->kind == NPC_TYPE_GHOST )
		  {
			  pAttMaster=pAtt->getTopMaster();
			  ret.bySrc = Cmd::MAPDATATYPE_USER;
			  ret.dwHP = ((SceneNpc *)pDef)->hp;
		  }
		  else
		  {
			  pAttMaster=pAtt->getTopMaster(); 
			  ret.byTarget = Cmd::MAPDATATYPE_NPC;
			  ret.dwHP = ((SceneNpc *)pDef)->hp;
		  }
      }
      break;
      //    case zSceneEntry::SceneEntry_Build:    /**< 建筑*/
      //      break;
      //    case zSceneEntry::SceneEntry_Object:  /**< 地上物品*/
      //      break;
    default:
      ret.byTarget = Cmd::MAPDATATYPE_NPC;
      break;
  }

  if (pDef->isPhysics)
  {
    if (pAtt->isPhysicBang)
    {
      ret.byLuck = 1;
    }
    else if (pAtt->isHPhysicBang)
    {
      ret.byLuck = 2;
    }
    else
    {
      ret.byLuck = 0;
    }
  }
  else
  {
    if (pAtt->isMagicBang)
    {
      ret.byLuck = 1;
    }
    else if (pAtt->isHMagicBang)
    {
      ret.byLuck = 2;
    }
    else
    {
      ret.byLuck = 0;
    }
  }
#ifdef _DEBUG
  if (pAtt->getType() == zSceneEntry::SceneEntry_Player) Channel::sendSys((SceneUser *)pAtt,Cmd::INFO_TYPE_GAME,"广播九屏：%s攻击%s 攻击方式[%s] 魔法爆击[%s] 物理爆击[%s] ret.byLuck[%u]",pAtt->name,pDef->name,pDef->isPhysics?"物理":"法术",pAtt->isMagicBang?"爆":"否",pAtt->isPhysicBang?"爆":"否",ret.byLuck);
#endif
  pAtt->isPhysicBang=false;
  pAtt->isMagicBang=false;
  pAtt->isHPhysicBang=false;
  pAtt->isHMagicBang=false;
  ret.byDirect = rev?rev->byDirect:pAtt->getDir();
  ret.sdwHP = sdwHP;
  if (ret.sdwHP >= 0)
  {
    ret.byRetcode = Cmd::RTMAGIC_SUCCESS;
  }
  else
  {
    ret.byRetcode = Cmd::RTMAGIC_FAILURE;
  }


  /*
  //跟踪并攻击用户
  if (pDef->getType()==zSceneEntry::SceneEntry_NPC)
  {
    ((SceneNpc *)pDef)->forceChaseUser(pAtt);
#ifdef _DEBUG
    if (pAtt->getType() == zSceneEntry::SceneEntry_Player) Channel::sendSys((SceneUser *)pAtt,Cmd::INFO_TYPE_GAME,"广播九屏：%s攻击%s 伤害点数%u",pAtt->name,pDef->name,ret.wdHP);
#endif
  }
  */

  if (pAtt->getType() == zSceneEntry::SceneEntry_Player)
  {
    ((SceneUser*)pAtt)->sendCmdToMe(&ret,sizeof(ret));
  }
  else if (pAttMaster && pAttMaster->getType() == zSceneEntry::SceneEntry_Player)
  {
    ((SceneUser*)pAttMaster)->sendCmdToMe(&ret,sizeof(ret));
  }
  if (pDef->getType() == zSceneEntry::SceneEntry_Player)
  {
    ((SceneUser*)pDef)->sendCmdToMe(&ret,sizeof(ret));
  }
  else if (pDefMaster && pDefMaster->getType() == zSceneEntry::SceneEntry_Player)
  {
    ((SceneUser*)pDefMaster)->sendCmdToMe(&ret,sizeof(ret));
  }
  ret1.byTarget=ret.byTarget;        /**< 目标类型：enumMapDataType */
  ret1.dwUserTempID=ret.dwUserTempID;      /**< 目标临时编号 */
  ret1.bySrc=ret.bySrc;          /**< 攻击者类型：enumMapDataType */
  ret1.dwSrcTempID=ret.dwSrcTempID;      /**< 攻击者临时编号 */
  pDef->sendCmdToSelected(&ret1,sizeof(ret1));
  return pAtt->scene->sendCmdToNine(pAtt->getPosI(),&ret1,sizeof(ret1),pAtt->dupIndex);
}

/**
 * \brief 检查更新自卫状态
 *
 * \pAtt 攻击方
 *
 * \pDef 被攻击方
 */
bool ScenePkState::hasProtected()
{
  return protect_time?true:false;
}

/**
 * \brief 添加一个玩家进入自己的自卫列表
 *
 * \param pThis:攻击自己的玩家
 * \param attid:攻击自己的玩家ID
 */
bool ScenePkState::addProtect(SceneUser * pThis,DWORD time)
{
  bool bret = false;
  if (!(pThis->charbase.goodness & Cmd::GOODNESS_ATT))
  {
    bret=true;
  }
  else if (!protect_time)
  {
    bret=true;
  }
  pThis->charbase.goodness |= Cmd::GOODNESS_ATT;  
  if (time)
  {
    protect_time = time;
  }
  else
  {
    protect_time = 120000/ScenePkState::protectPeriod;
  }
  return bret;
}

/**
 * \brief 开始攻击玩家时,检查是否进入自卫状态。
 *
 * 被SceneEntryPk::AttackMe所调用,如果还未加入到自卫列表中,则加入
 * 
 * \param  psAtt: 攻击者
 * \param  psDef: 被攻击者
 */
void ScenePk::checkProtect(SceneEntryPk *psAtt,SceneEntryPk *psDef)
{
  if ((!psAtt)||(!psDef)) return;

  if (zSceneEntry::SceneEntry_NPC==psAtt->getType())
  {
    if (psAtt->getTopMaster())
      checkProtect(psAtt->getMaster(),psDef);
    return;
  }
  if (zSceneEntry::SceneEntry_NPC==psDef->getType())
  {
    if (psDef->getTopMaster())
      checkProtect(psAtt,psDef->getMaster());
    return;
  }

  SceneUser *pAtt =(SceneUser *)psAtt;
  SceneUser *pDef =(SceneUser *)psDef;

  if (pAtt->scene && pAtt->scene->isNoRedScene()) return;
  if (pAtt->isWar(pDef)) return;

  if (pAtt->isCatcherState()) return;

  //如果是好人伤害好人
  SWORD swGoodAtt = (SWORD)(pAtt->charbase.goodness & 0x0000FFFF);
  SWORD swGoodDef = (SWORD)(pDef->charbase.goodness & 0x0000FFFF);
  if (!pAtt->issetUState(Cmd::USTATE_GUARD) && 
      (!psDef->scene->checkZoneType(pDef->getPos(),ZoneTypeDef::ZONE_SPORTS)  
      && !psAtt->scene->checkZoneType(psAtt->getPos(),ZoneTypeDef::ZONE_SPORTS)) 
      && pAtt->charbase.country == pDef->charbase.country)
  {
    if (swGoodAtt <= 60 && swGoodDef <= 60)
    {
      if (!pDef->pkState.hasProtected())
      {
        if (pAtt->pkState.addProtect(pAtt))
        {
          if (pAtt->mask.is_masking())
            Channel::sendSys(pDef,Cmd::INFO_TYPE_GAME,"蒙面人 攻击了你,两分钟内你可以对他进行正当防卫");    
          else
            Channel::sendSys(pDef,Cmd::INFO_TYPE_GAME,"%s攻击了你,两分钟内你可以对他进行正当防卫",pAtt->name);    
          if (pDef->mask.is_masking())
          {
            Channel::sendSys(pAtt,Cmd::INFO_TYPE_GAME,"你攻击了 蒙面人,两分钟内 蒙面人 可以对你进行正当防卫");
            if (pDef->charbase.pkaddition>1800)
            {
              Channel::sendSys(pAtt,Cmd::INFO_TYPE_GAME,"对方处于PK保护状态！");    
            }
          }
          else
          {
            Channel::sendSys(pAtt,Cmd::INFO_TYPE_GAME,"你攻击了 %s,两分钟内 %s 可以对你进行正当防卫",pDef->name,pDef->name);    
            if (pDef->charbase.pkaddition>1800)
            {
              Channel::sendSys(pAtt,Cmd::INFO_TYPE_GAME,"对方处于PK保护状态！");    
            }
          }
          if (pAtt->charbase.pkaddition>1800)
          {
            Channel::sendSys(pAtt,Cmd::INFO_TYPE_SYS,"你的PK保护状态解除！");
          }
          pAtt->charbase.pkaddition=0;
          if (pAtt->issetUState(Cmd::USTATE_PK))
          {
            pAtt->clearUState(Cmd::USTATE_PK);
            pAtt->sendtoSelectedPkAdditionState();
            pAtt->clearStateToNine(Cmd::USTATE_PK);
          }
          pAtt->sendGoodnessToNine();
        }
      }
    }
  }
  /*
   * \brief
   * 通知被攻击人属性变化
   */
  //ScenePk::sendChangedUserData(pDef);
}

/**
 * \brief 广播收到的攻击指令
 *
 * 广播攻击指令给九屏内的玩家
 *
 * \rev,收到的指令
 * \pAtt 攻击方
 *
 * \return 广播是否成功
 */
bool ScenePk::attackUserCmdToNine(const Cmd::stAttackMagicUserCmd *rev,SceneEntryPk *pAtt)
{
  Cmd::stAttackMagicUserCmd attCmd;
  bcopy(rev,&attCmd,sizeof(attCmd),sizeof(attCmd));
  attCmd.dwUserTempID = pAtt->tempid;


  return pAtt->scene->sendCmdToNine(pAtt->getPosI(),&attCmd,sizeof(attCmd),pAtt->dupIndex);
}

/*
 * \brief 检测物理攻击速度是否过快
 * 
 * \pAtt 用户
 * \speed 当前角色速度
 * \return 是否过快
 */
bool ScenePkState::speedOutP(WORD speed,DWORD dwTime)
{
  if (dwTime - lastPTime < speed)
  {
    if (speed==0) speed=1;
    if ((float)(dwTime-lastPTime)/(float)speed < 0.94f)
      return false;
  }
  lastPTime = dwTime;
  return true;
}

/*
 * \brief 检测魔法攻击速度是否过快
 * \speed 当前角色速度
 * \return 是否过快
 */
bool ScenePkState::speedOutM(WORD speed,DWORD dwTime)
{
  if (dwTime - lastMTime < speed)
  {
    if (speed==0) speed=1;
    if ((float)(dwTime - lastMTime)/(float)speed < 0.94f)
      return false;
  }
  lastMTime = dwTime;
  return true;
}

/*
 * \brief 检测攻击速度是否过快
 * 
 * \pAtt 用户
 * \rev 接收到的指令
 * \return 是否过快
 */
bool ScenePk::checkAttackSpeed(SceneUser *pAtt,const Cmd::stAttackMagicUserCmd *rev)
{
#ifdef _DEBUG

#endif

  switch(rev->wdMagicType)
  {
	case SERVER_SKILL_ATTACK_NORMAL:		//单手武器普通攻击
	case SERVER_SKILL_DAGGER_ATTACK_NORMAL: //双持武器(匕首)普通攻击
	case SERVER_SKILL_HANDS_ATTACK_NORMAL:	//双手武器普通攻击
    {
      if (pAtt->pkState.speedOutP((WORD)(pAtt->charstate.attackspeed/640.0f*1320),rev->dwTimestamp))
      {
        return true;
      }
      else
      {
        //Zebra::logger->info("攻击速度过快(%s(%ld),%ld)",pAtt->name,pAtt->id,rev->byAction);
        return false;
      }
    }
	case SERVER_SKILL_DART_ATTACK_NORMAL:	//飞镖武器普通攻击:
    {
      if (pAtt->pkState.speedOutP((WORD)(pAtt->charstate.attackspeed/640.0f*1320),rev->dwTimestamp))
      {
        return true;
      }
      else
      {
        //Zebra::logger->info("攻击速度过快(%s(%ld),%ld)",pAtt->name,pAtt->id,rev->byAction);
        return false;
      }
    }
    default:
    {
      WORD value = 1320;
      switch(pAtt->charbase.useJob)
      {
	  case JOB_FIGHTER:		//战士
	  case JOB_THIEVES:		//盗贼
		  value = 1320;
		  break;
	  case JOB_MASTER:		//法师
	  case JOB_PASTOR:		//牧师
          value = 1560;
          break;
	  default:
		  value = 1880;
		  break;
      }

      if (pAtt->pkState.speedOutM((WORD)(pAtt->charstate.attackspeed/640.0f*value),rev->dwTimestamp))
      {
      //除物理攻击之外的所有攻击视作法术攻击
        zSkill *s = pAtt->usm.findSkill(rev->wdMagicType);
        if (s)
        {
			//sky 判断该技能是否能在战斗中使用
			if(s->base->useBook == 1)
			{
				//sky 不能的话判断下用户是否处于战斗状态
				if(pAtt->IsPveOrPvp() == USE_FIGHT_PVE || pAtt->IsPveOrPvp() == USE_FIGHT_PVP)
				{
					pAtt->sendMessageToMe("该技能不可以在战斗中使用!");
					return false;
				}
			}

			if (!s->canUse())
			{
#ifdef _DEBUG
				zRTime ctv;
				Channel::sendSys(pAtt,Cmd::INFO_TYPE_GAME,"技能攻击速度过快或处于冷却期间",s->actionbase->dtime,ctv.msecs() - s->lastUseTime);
#endif
				//Zebra::logger->info("技能攻击速度过快或处于冷却期间(%s(%ld),%ld)",pAtt->name,pAtt->id,rev->byAction);
				return false;
			}
			pAtt->lastUseSkill = rev->wdMagicType;
			switch(rev->wdMagicType)
			{
			case 219://吞噬骷髅战士
			case 224://吞噬幽魂
			case 229://吞噬骸骨守卫
			case 235://吞噬六道鬼王
			case 239://合体神兵
			case 244://合体天将
			case 249://合体天仙
			case 255://合体三界天王
			case 259://融合石灵
			case 264://融合风灵
			case 269://融合铁灵
			case 275://融合焰魔君主
				{
					zSkill *ss = pAtt->usm.findSkill(219);
					if (ss) ss->canUse();
					ss = pAtt->usm.findSkill(224);
					if (ss) ss->canUse();
					ss = pAtt->usm.findSkill(229);
					if (ss) ss->canUse();
					ss = pAtt->usm.findSkill(235);
					if (ss) ss->canUse();
					ss = pAtt->usm.findSkill(239);
					if (ss) ss->canUse();
					ss = pAtt->usm.findSkill(244);
					if (ss) ss->canUse();
					ss = pAtt->usm.findSkill(249);
					if (ss) ss->canUse();
					ss = pAtt->usm.findSkill(255);
					if (ss) ss->canUse();
					ss = pAtt->usm.findSkill(259);
					if (ss) ss->canUse();
					ss = pAtt->usm.findSkill(264);
					if (ss) ss->canUse();
					ss = pAtt->usm.findSkill(269);
					if (ss) ss->canUse();
					ss = pAtt->usm.findSkill(275);
					if (ss) ss->canUse();
				}
				break;
			case 120://火焰陷阱   120
			case 128://毒蔓陷阱   128
			case 123://毒云陷阱   123
			case 125://冰封陷阱   125
			case 130://爆炎陷阱   130
			case 133://诛仙陷阱   133
			case 137://屠魔陷阱   137
			case 138://死神陷阱   138
				{
					zSkill *ss = pAtt->usm.findSkill(120);
					if (ss) ss->canUse();
					ss = pAtt->usm.findSkill(128);
					if (ss) ss->canUse();
					ss = pAtt->usm.findSkill(123);
					if (ss) ss->canUse();
					ss = pAtt->usm.findSkill(125);
					if (ss) ss->canUse();
					ss = pAtt->usm.findSkill(130);
					if (ss) ss->canUse();
					ss = pAtt->usm.findSkill(133);
					if (ss) ss->canUse();
					ss = pAtt->usm.findSkill(137);
					if (ss) ss->canUse();
					ss = pAtt->usm.findSkill(138);
					if (ss) ss->canUse();
				}
				break;
			default:
				break;
			}
			return true;
		}
		else
		{
			Zebra::logger->info("外挂:试图使用没有学习的技能(%s(%ld),%ld) 技能id = %ld",pAtt->name,pAtt->id,rev->byAction,rev->wdMagicType);
			return false;
		}
	  }
	  else
	  {
		  //Zebra::logger->info("攻击速度过快(%s(%ld),%ld)",pAtt->name,pAtt->id,rev->byAction);
		  return false;
	  }
	  //      }
	  //      else
	  //      {
	  //        if (pAtt->pkState.speedOutM((WORD)((pAtt->charstate.attackspeed/640.0f*1000)*(pAtt->skillValue.mgspeed==0?1:(pAtt->skillValue.mgspeed/100.0f)))))
	  //        {
	  //          zSkill *s = pAtt->usm.findSkill(rev->wdMagicType);
	  //          if (s)
	  //          {
	  //            if (!s->canUse())
	  //            {
	  //#ifdef _DEBUG
	  //              zRTime ctv;
	  //              Channel::sendSys(pAtt,Cmd::INFO_TYPE_GAME,"技能攻击速度过快或处于冷却期间",s->actionbase->dtime,ctv.msecs() - s->lastUseTime);
	  //#endif
	  //              Zebra::logger->info("技能攻击速度过快或处于冷却期间(%s(%ld),%ld)",pAtt->name,pAtt->id,rev->byAction);
	  //              return false;
	  //            }
	  //            pAtt->lastUseSkill = rev->wdMagicType;
	  //            return true;
	  //          }
	  //          else
	  //          {
	  //            Zebra::logger->info("外挂:试图使用没有学习的技能(%s(%ld),%ld) 技能id = %ld",pAtt->name,pAtt->id,rev->byAction,rev->wdMagicType);
	  //            return false;
	  //          }
	  //        }
	  //        else
	  //        {
	  //#ifdef _DEBUG
	  //            Channel::sendSys(pAtt,Cmd::INFO_TYPE_GAME,"没有过技能切换间隔时间,施放失败");
	  //#endif
	  //          ScenePk::attackFailToMe(rev,pAtt);
	  //          Zebra::logger->info("攻击速度过快(%s(%ld),%ld)",pAtt->name,pAtt->id,rev->byAction);
	  //          return false;
	  //        }
	  //      }
	}
  }

  return true;
}


#define checkholyp(percent)\
pAtt->isPhysicBang = false;\
pAtt->isHPhysicBang = false;\
if (selectByPercent(pAtt->charstate.bang))\
{\
  pAtt->pkValue.pdamage = (WORD)(pAtt->pkValue.pdamage * 1.5f);\
  pAtt->isPhysicBang = true;\
}\
if (selectByPercent(pAtt->packs.equip.getEquips().get_holy()))\
{\
  pAtt->pkValue.pdamage = (WORD)(pAtt->pkValue.pdamage * 1.5f);\
  pAtt->isHPhysicBang = true;\
}

#define checkholym(percent)\
pAtt->isMagicBang = false;\
pAtt->isHMagicBang = false;\
if (selectByPercent(pAtt->charstate.bang))\
{\
  pAtt->pkValue.mdamage = (WORD)(pAtt->pkValue.mdamage * 1.5f);\
  pAtt->isMagicBang = true;\
}\
if (selectByPercent(pAtt->packs.equip.getEquips().get_holy()))\
{\
  pAtt->pkValue.mdamage = (WORD)(pAtt->pkValue.mdamage * 1.5f);\
  pAtt->isHMagicBang = true;\
}



/**
 * \brief 计算玩家对玩家的物理伤害值 
 *
 * 在SceneUser:preAttackMe中被调用
 *
 * \param rev: 攻击命令
 * \param pAtt: 攻击者
 * \param pDef: 防御者
 *
 */
void ScenePk::calpdamU2U(const Cmd::stAttackMagicUserCmd *rev,SceneUser *pAtt,SceneUser *pDef)
{
  float percent=0.0f;
  //五行处理
  switch(pAtt->IsOppose(pDef->charstate.defencefive))
  {
    //甲克已
    case  1:
      {
        percent=randBetween(0,100)/100.0f;
        if (pAtt->maxattack) percent = 1.0f;
        if (pAtt->attacklow) percent = 0.0f;
        if (rev&&(rev->wdMagicType == SERVER_SKILL_DART_ATTACK_NORMAL)&&pDef->huntermark) percent=1.0f;
        /// 计算攻击者的物理攻击力
        pAtt->pkValue.pdamage = (DWORD)(pAtt->pkpreValue.fivedam+(pAtt->pkpreValue.fivemaxdam-pAtt->pkpreValue.fivedam)*percent);
        checkholyp(percent);
        //pAtt->pkValue.pdamage= (DWORD)(pAtt->pkValue.pdamage*(1.0f+pAtt->packs.equip.getEquips().getAttFivePoint()/100.0f));

        pDef->pkValue.pdefence = pDef->pkpreValue.nofivedef;
      }
      break;
    case 2:
      {
        percent=randBetween(0,100)/100.0f;
        if (pAtt->maxattack) percent = 1.0f;
        if (pAtt->attacklow) percent = 0.0f;
        if (rev&&(rev->wdMagicType == SERVER_SKILL_DART_ATTACK_NORMAL)&&pDef->huntermark) percent=1.0f;
        /// 计算攻击者的物理攻击力
        pAtt->pkValue.pdamage = (DWORD)(pAtt->pkpreValue.nofivedam+(pAtt->pkpreValue.nofivemaxdam-pAtt->pkpreValue.nofivedam)*percent);

        checkholyp(percent);

        /// 计算防御者的物理防御力受五行点数的影响
        pDef->pkValue.pdefence = pDef->pkpreValue.fivedef;
        //pDef->pkValue.pdefence = (DWORD)(pDef->pkValue.pdefence*(1.0f+pDef->packs.equip.getEquips().getDefFivePoint()/100.0f));
      }
      break;
    case 0:
      {
        percent=randBetween(0,100)/100.0f;
        if (pAtt->maxattack) percent = 1.0f;
        if (pAtt->attacklow) percent = 0.0f;
        if (rev&&(rev->wdMagicType == SERVER_SKILL_DART_ATTACK_NORMAL)&&pDef->huntermark) percent=1.0f;
        /// 计算攻击者的物理攻击力
        pAtt->pkValue.pdamage = (DWORD)(pAtt->pkpreValue.nofivedam+(pAtt->pkpreValue.nofivemaxdam-pAtt->pkpreValue.nofivedam)*percent);

        checkholyp(percent);

        /// 计算防御者的物理防御力受五行点数的影响
        pDef->pkValue.pdefence = pDef->pkpreValue.nofivedef;
      }
      break;
    default:
      break;

  }

  if (selectByPercent(pAtt->packs.equip.getEquips().get_ignoredef())) pDef->pkValue.pdefence = 0;
}

/**
 * \brief 计算玩家对玩家的魔法伤害值
 *
 * 在SceneUser::preAttackMe中被调用
 *
 * \param rev: 攻击命令
 * \param pAtt: 攻击者
 * \param pDef: 防御者
 *
 */
void ScenePk::calmdamU2U(const Cmd::stAttackMagicUserCmd *rev,SceneUser *pAtt,SceneUser *pDef)
{
  float percent=0.0f;
  //五行处理
  switch(pAtt->IsOppose(pDef->charstate.defencefive))
  {
    //甲克已
    case  1: // pAtt克对方
      {
        percent=randBetween(0,100)/100.0f;
        if (pAtt->maxattack) percent = 1.0f;
        if (pAtt->attacklow) percent = 0.0f;
        if (rev&&(rev->wdMagicType == SERVER_SKILL_DART_ATTACK_NORMAL)&&pDef->huntermark) percent=1.0f;
        /// 计算攻击者的魔法攻击力
        pAtt->pkValue.mdamage = (DWORD)(pAtt->pkpreValue.fivemdam+(pAtt->pkpreValue.fivemaxmdam-pAtt->pkpreValue.fivemdam)*percent);
        checkholym(percent);
        //pAtt->pkValue.mdamage= (DWORD)(pAtt->pkValue.mdamage*(1.0f+pAtt->packs.equip.getEquips().getAttFivePoint()/100.0f));

        /// 计算防御者的魔法攻击力受五行点数的影响
        pDef->pkValue.mdefence = pDef->pkpreValue.nofivemdef;
      }
      break;
    case 2: // 对方克pAtt
      {
        percent=randBetween(0,100)/100.0f;
        if (pAtt->maxattack) percent = 1.0f;
        if (pAtt->attacklow) percent = 0.0f;
        if (rev&&(rev->wdMagicType == SERVER_SKILL_DART_ATTACK_NORMAL)&&pDef->huntermark) percent=1.0f;
        /// 计算攻击者的魔法攻击力
        pAtt->pkValue.mdamage = (DWORD)(pAtt->pkpreValue.nofivemdam+(pAtt->pkpreValue.nofivemaxmdam-pAtt->pkpreValue.nofivemdam)*percent);

        checkholym(percent);

        /// 计算防御者的魔法防御力受五行点数的影响
        pDef->pkValue.mdefence = pDef->pkpreValue.fivemdef;
        //pDef->pkValue.mdefence = (DWORD)(pDef->pkValue.mdefence*(1.0f+pDef->packs.equip.getEquips().getDefFivePoint()/100.0f));
      }
      break;
    case 0: // 无相克关系
      {
        percent=randBetween(0,100)/100.0f;
        if (pAtt->maxattack) percent = 1.0f;
        if (pAtt->attacklow) percent = 0.0f;
        if (rev&&(rev->wdMagicType == SERVER_SKILL_DART_ATTACK_NORMAL)&&pDef->huntermark) percent=1.0f;
        /// 计算攻击者的魔法攻击力
        pAtt->pkValue.mdamage = (DWORD)(pAtt->pkpreValue.nofivemdam+(pAtt->pkpreValue.nofivemaxmdam-pAtt->pkpreValue.nofivemdam)*percent);

        checkholym(percent);

        /// 计算防御者的魔法防御力受五行点数的影响
        pDef->pkValue.mdefence = pDef->pkpreValue.nofivemdef;
      }
      break;
    default:
      break;

  }
  if (selectByPercent(pAtt->packs.equip.getEquips().get_ignoredef())) pDef->pkValue.mdefence = 0;
}

/**
 * \brief 计算玩家对NPC的物理伤害值 
 *
 * 在SceneUser:preAttackMe中被调用
 *
 * \param rev: 攻击命令
 * \param pAtt: 攻击者
 * \param pDef: 防御者
 *
 */
void ScenePk::calpdamU2N(const Cmd::stAttackMagicUserCmd *rev,SceneUser *pAtt,SceneNpc *pDef)
{
  float percent=0.0f;
  //五行处理
  switch(pAtt->IsOppose(pDef->npc->five))
  {
	  //甲克已
  case  1:
	  {
		  percent=randBetween(0,100)/100.0f;
		  if (pAtt->maxattack) percent = 1.0f;
		  if (pAtt->attacklow) percent = 0.0f;
		  if (rev&&(rev->wdMagicType == SERVER_SKILL_DART_ATTACK_NORMAL)&&pDef->huntermark) percent=1.0f;
		  /// 计算攻击者的物理攻击力
		  pAtt->pkValue.pdamage = (DWORD)(pAtt->pkpreValue.fivedam+(pAtt->pkpreValue.fivemaxdam-pAtt->pkpreValue.fivedam)*percent);
#ifdef _DEBUG
		  Zebra::logger->info("1-----------------------------------------------------------");
		  Zebra::logger->info("pkpreValue.fivedam=%u,pkpreValue.fivemaxdam=%u",pAtt->pkpreValue.fivedam,pAtt->pkpreValue.fivemaxdam);
		  Zebra::logger->info("预处理pkValue.pdamage=[%u] percent=[%f]",pAtt->pkValue.pdamage,percent);
#endif
		  checkholyp(percent);
#ifdef _DEBUG
		  Zebra::logger->info("爆击处理预处理pkValue.pdamage=[%u] 物理爆[%s] 魔法爆[%s]",pAtt->pkValue.pdamage,pAtt->isPhysicBang?"爆":"否",pAtt->isMagicBang?"爆":"否");
#endif
		  //pAtt->pkValue.pdamage= (DWORD)(pAtt->pkValue.pdamage*(1.0f+pAtt->packs.equip.getEquips().getAttFivePoint()/100.0f));

		  /// 计算防御者的物理防御力
		  if( pDef->npc->kind == NPC_TYPE_GHOST )   //元神特殊处理直接计算主人的值sky
		  {
			  pDef->pkValue.pdefence = ((ScenePet *)pDef)->petData.def;
		  }
		  else
		  {
			  pDef->pkValue.pdefence= randBetween(pDef->getMinPDefence(),pDef->getMaxPDefence());
		  }
		  if (pDef->pkValue.pdefence > (DWORD)pDef->skillValue.dpdef)
			  pDef->pkValue.pdefence -= (DWORD)pDef->skillValue.dpdef;
		  else
			  pDef->pkValue.pdefence = 0;
	  }
	  break;
    case 2:
      {
        percent=randBetween(0,100)/100.0f;
        if (pAtt->maxattack) percent = 1.0f;
        if (pAtt->attacklow) percent = 0.0f;
        if (rev&&(rev->wdMagicType == SERVER_SKILL_DART_ATTACK_NORMAL)&&pDef->huntermark) percent=1.0f;
        /// 计算攻击者的物理攻击力
        pAtt->pkValue.pdamage = (DWORD)(pAtt->pkpreValue.nofivedam+(pAtt->pkpreValue.nofivemaxdam-pAtt->pkpreValue.nofivedam)*percent);
#ifdef _DEBUG
              Zebra::logger->info("1-----------------------------------------------------------");
          Zebra::logger->info("pkpreValue.nofivedam=%u,pkpreValue.nofivemaxdam=%u",pAtt->pkpreValue.nofivedam,pAtt->pkpreValue.nofivemaxdam);
              Zebra::logger->info("预处理pkValue.pdamage=[%u] percent=[%f]",pAtt->pkValue.pdamage,percent);
#endif
        checkholyp(percent);
#ifdef _DEBUG
              Zebra::logger->info("爆击处理预处理pkValue.pdamage=[%u] 物理爆[%s] 魔法爆[%s]",pAtt->pkValue.pdamage,pAtt->isPhysicBang?"爆":"否",pAtt->isMagicBang?"爆":"否");
#endif


			  /// 计算防御者的物理防御力
			  if( pDef->npc->kind == NPC_TYPE_GHOST )		//元神特殊处理直接计算主人的值sky
			  {
				  pDef->pkValue.pdefence = ((ScenePet *)pDef)->petData.def;
			  }
			  else
			  {
				  pDef->pkValue.pdefence = randBetween(pDef->getMinPDefence(),pDef->getMaxPDefence());
			  }

			  pDef->pkValue.pdefence = (DWORD)(pDef->pkValue.pdefence*(1.0f +pDef->npc->fivepoint/100.0f));
			  if (pDef->pkValue.pdefence > (DWORD)pDef->skillValue.dpdef)
				  pDef->pkValue.pdefence -=(DWORD) pDef->skillValue.dpdef;
			  else
				  pDef->pkValue.pdefence = 0;
	  }
	  break;
    case 0:
      {
        percent=randBetween(0,100)/100.0f;
        if (pAtt->maxattack) percent = 1.0f;
        if (pAtt->attacklow) percent = 0.0f;
        if (rev&&(rev->wdMagicType == SERVER_SKILL_DART_ATTACK_NORMAL)&&pDef->huntermark) percent=1.0f;
        /// 计算攻击者的物理攻击力
        pAtt->pkValue.pdamage = (DWORD)((float)pAtt->pkpreValue.nofivedam+((float)(pAtt->pkpreValue.nofivemaxdam-pAtt->pkpreValue.nofivedam))*percent);

#ifdef _DEBUG
              Zebra::logger->info("1-----------------------------------------------------------");
          Zebra::logger->info("pkpreValue.nofivedam=%u,pkpreValue.nofivemaxdam=%u",pAtt->pkpreValue.nofivedam,pAtt->pkpreValue.nofivemaxdam);
              Zebra::logger->info("预处理pkValue.pdamage=[%u] percent=[%f]",pAtt->pkValue.pdamage,percent);
#endif

        checkholyp(percent);
#ifdef _DEBUG
              Zebra::logger->info("爆击处理预处理pkValue.pdamage=[%u] 物理爆[%s] 魔法爆[%s]",pAtt->pkValue.pdamage,pAtt->isPhysicBang?"爆":"否",pAtt->isMagicBang?"爆":"否");
#endif

        /// 计算防御者的物理防御力
			  if( pDef->npc->kind == NPC_TYPE_GHOST )   //元神特殊处理直接计算主人的值sky
			  {
				  pDef->pkValue.pdefence = ((ScenePet *)pDef)->petData.def;
			  }
			  else
			  {
				  pDef->pkValue.pdefence= 
					  randBetween(pDef->getMinPDefence(),pDef->getMaxPDefence());
			  }
			  if (pDef->pkValue.pdefence > (DWORD)pDef->skillValue.dpdef)
				  pDef->pkValue.pdefence -= (DWORD)pDef->skillValue.dpdef;
			  else
				  pDef->pkValue.pdefence = 0;
      }
      break;
    default:
      break;

  }
  if (selectByPercent(pAtt->packs.equip.getEquips().get_ignoredef())) pDef->pkValue.pdefence = 0;
}

/**
 * \brief 计算玩家对NPC的魔法伤害值 
 *
 * 在SceneUser:preAttackMe中被调用
 *
 * \param rev: 攻击命令
 * \param pAtt: 攻击者
 * \param pDef: 防御者
 *
 */
void ScenePk::calmdamU2N(const Cmd::stAttackMagicUserCmd *rev,SceneUser *pAtt,SceneNpc *pDef)
{
  float percent=0.0f;
  //五行处理
  switch(pAtt->IsOppose(pDef->npc->five))
  {
    //甲克已
    case  1:
      {
        percent=randBetween(0,100)/100.0f;
        if (pAtt->maxattack) percent = 1.0f;
        if (pAtt->attacklow) percent = 0.0f;
        if (rev&&(rev->wdMagicType == SERVER_SKILL_DART_ATTACK_NORMAL)&&pDef->huntermark) percent=1.0f;
        /// 计算攻击者的魔法攻击力
        pAtt->pkValue.mdamage = (DWORD)(pAtt->pkpreValue.fivemdam+(pAtt->pkpreValue.fivemaxmdam-pAtt->pkpreValue.fivemdam)*percent);
#ifdef _DEBUG
              Zebra::logger->info("1-----------------------------------------------------------");
              Zebra::logger->info("预处理pkValue.mdamage=[%u]",pAtt->pkValue.mdamage);
#endif
        checkholym(percent);
#ifdef _DEBUG
              Zebra::logger->info("爆击处理预处理pkValue.mdamage=[%u] 物理爆[%s] 魔法爆[%s]",pAtt->pkValue.mdamage,pAtt->isPhysicBang?"爆":"否",pAtt->isMagicBang?"爆":"否");
#endif
        //pAtt->pkValue.mdamage= (DWORD)(pAtt->pkValue.mdamage*(1+pAtt->packs.equip.getEquips().getAttFivePoint()/100.0f));

        /// 计算防御者的魔法防御力
			  if( pDef->npc->kind == NPC_TYPE_GHOST ) //元神特殊处理直接计算主人的值sky
			  {
				  pDef->pkValue.mdefence = ((ScenePet *)pDef)->petData.mdef;
			  }
			  else
			  {
				  pDef->pkValue.mdefence= 
					  randBetween(pDef->getMinMDefence(),pDef->getMaxMDefence());
			  }
      }
      break;
    case 2:
      {
        percent=randBetween(0,100)/100.0f;
        if (pAtt->maxattack) percent = 1.0f;
        if (pAtt->attacklow) percent = 0.0f;
        if (rev&&(rev->wdMagicType == SERVER_SKILL_DART_ATTACK_NORMAL)&&pDef->huntermark) percent=1.0f;
        /// 计算攻击者的魔法攻击力
        pAtt->pkValue.mdamage = (DWORD)(pAtt->pkpreValue.nofivemdam+(pAtt->pkpreValue.nofivemaxmdam-pAtt->pkpreValue.nofivemdam)*percent);

#ifdef _DEBUG
              Zebra::logger->info("2------------------------------------------------[%u]-----------",pDef->npc->five);
              Zebra::logger->info("预处理pkValue.mdamage=[%u]",pAtt->pkValue.mdamage);
#endif
        checkholym(percent);
#ifdef _DEBUG
              Zebra::logger->info("爆击处理预处理pkValue.mdamage=[%u] 物理爆[%s] 魔法爆[%s]",pAtt->pkValue.mdamage,pAtt->isPhysicBang?"爆":"否",pAtt->isMagicBang?"爆":"否");
#endif

			  /// 计算防御者的魔法防御力
			  if( pDef->npc->kind == NPC_TYPE_GHOST )  //元神特殊处理直接计算主人的值sky
			  {
				  pDef->pkValue.mdefence = ((ScenePet *)pDef)->petData.mdef;
			  }
			  else
			  {
				  pDef->pkValue.mdefence = randBetween(pDef->getMinMDefence(),pDef->getMaxMDefence());
			  }

			  pDef->pkValue.mdefence = (DWORD)(pDef->pkValue.mdefence*(1.0f +pDef->npc->fivepoint/100.0f));
      }
      break;
    case 0:
      {
        percent=randBetween(0,100)/100.0f;
        if (pAtt->maxattack) percent = 1.0f;
        if (pAtt->attacklow) percent = 0.0f;
        if (rev&&(rev->wdMagicType == SERVER_SKILL_DART_ATTACK_NORMAL)&&pDef->huntermark) percent=1.0f;
        /// 计算攻击者的魔法攻击力
        pAtt->pkValue.mdamage = (DWORD)(pAtt->pkpreValue.nofivemdam+(pAtt->pkpreValue.nofivemaxmdam-pAtt->pkpreValue.nofivemdam)*percent);

#ifdef _DEBUG
              Zebra::logger->info("0-----------------------------------------------------------");
              Zebra::logger->info("预处理pkValue.mdamage=[%u]",pAtt->pkValue.mdamage);
#endif
        checkholym(percent);
#ifdef _DEBUG
              Zebra::logger->info("爆击处理预处理pkValue.mdamage=[%u] 物理爆[%s] 魔法爆[%s]",pAtt->pkValue.mdamage,pAtt->isPhysicBang?"爆":"否",pAtt->isMagicBang?"爆":"否");
#endif

        /// 计算防御者的魔法防御力
			  if( pDef->npc->kind == NPC_TYPE_GHOST )  //元神特殊处理直接计算主人的值sky
			  {
				  pDef->pkValue.mdefence = ((ScenePet *)pDef)->petData.mdef;
			  }
			  else
			  {
				  pDef->pkValue.mdefence= 
					  randBetween(pDef->getMinMDefence(),pDef->getMaxMDefence());
			  }
      }
      break;
    default:
      break;

  }
  if (selectByPercent(pAtt->packs.equip.getEquips().get_ignoredef())) pDef->pkValue.mdefence = 0;
}

/**
 * \brief 计算NPC对玩家的物理伤害值 
 *
 * 在SceneUser:preAttackMe中被调用
 *
 * \param rev: 攻击命令
 * \param pAtt: 攻击者
 * \param pDef: 防御者
 *
 */
void ScenePk::calpdamN2U(const Cmd::stAttackMagicUserCmd *rev,SceneNpc *pAtt,SceneUser *pDef)
{
  float percent=0.0f;
  //五行处理,与人与人的处理相反
  switch(pAtt->IsOppose(pDef->charstate.defencefive))
  {
    //NPC克人
    case  2:
      {
        percent=randBetween(0,100)/100.0f;
        if (pAtt->maxattack) percent = 1.0f;
        if (pAtt->attacklow) percent = 0.0f;
        if (rev&&(rev->wdMagicType == SERVER_SKILL_DART_ATTACK_NORMAL)&&pDef->huntermark) percent=1.0f;

        /// 计算攻击者的物理攻击力
		if( pAtt->npc->kind == NPC_TYPE_GHOST )
		{
			pAtt->pkValue.pdamage = 
				randBetween(((ScenePet *)pAtt)->petData.atk, ((ScenePet *)pAtt)->petData.maxatk );
		}
		else
		{
			pAtt->pkValue.pdamage = (DWORD)(pAtt->getMinPDamage()+(pAtt->getMaxPDamage()-pAtt->getMinPDamage())*percent);
		}
		pAtt->pkValue.pdamage = (DWORD)(pAtt->pkValue.pdamage*(1.0f +pAtt->npc->fivepoint/100.0f));

        /// 计算防御者的物理防御击力受五行点数的影响
        pDef->pkValue.pdefence = pDef->pkpreValue.nofivedef;
      }
      break;
    case 1:
      {
        percent=randBetween(0,100)/100.0f;
        if (pAtt->maxattack) percent = 1.0f;
        if (pAtt->attacklow) percent = 0.0f;
        if (rev&&(rev->wdMagicType == SERVER_SKILL_DART_ATTACK_NORMAL)&&pDef->huntermark) percent=1.0f;

        /// 计算攻击者的物理攻击力
		if( pAtt->npc->kind == NPC_TYPE_GHOST )
		{
			pAtt->pkValue.pdamage = 
				randBetween(((ScenePet *)pAtt)->petData.atk, ((ScenePet *)pAtt)->petData.maxatk );
		}
		else
		{
			pAtt->pkValue.pdamage = (DWORD)(pAtt->getMinPDamage()+(pAtt->getMaxPDamage()-pAtt->getMinPDamage())*percent);
		}

        /// 计算防御者的物理防御力受五行点数的影响
        pDef->pkValue.pdefence = pDef->pkpreValue.fivedef;
        //pDef->pkValue.pdefence = (DWORD)(pDef->pkValue.pdefence*(1+pDef->packs.equip.getEquips().getDefFivePoint()/100.0f));
      }
      break;
    case 0:
      {
        percent=randBetween(0,100)/100.0f;
        if (pAtt->maxattack) percent = 1.0f;
        if (pAtt->attacklow) percent = 0.0f;
        if (rev&&(rev->wdMagicType == SERVER_SKILL_DART_ATTACK_NORMAL)&&pDef->huntermark) percent=1.0f;

        /// 计算攻击者的物理攻击力
		if( pAtt->npc->kind == NPC_TYPE_GHOST )
		{
			pAtt->pkValue.pdamage = 
				randBetween(((ScenePet *)pAtt)->petData.atk, ((ScenePet *)pAtt)->petData.maxatk );
		}
		else
		{
			pAtt->pkValue.pdamage = (DWORD)(pAtt->getMinPDamage()+(pAtt->getMaxPDamage()-pAtt->getMinPDamage())*percent);
		}

        /// 计算防御者的物理防御力受五行点数的影响
        pDef->pkValue.pdefence = pDef->pkpreValue.nofivedef;
      }
      break;
    default:
      break;

  }
}


/**
 * \brief 计算NPC对玩家的魔法伤害值 
 *
 * 在SceneUser:preAttackMe中被调用
 *
 * \param rev 命令指针
 * \param pAtt 攻击者
 * \param pDef 防御者
 *
 */
void ScenePk::calmdamN2U(const Cmd::stAttackMagicUserCmd *rev,SceneNpc *pAtt,SceneUser *pDef)
{
  float percent=0.0f;
  //五行处理,与人与人的处理相反
  switch(pAtt->IsOppose(pDef->charstate.defencefive))
  {
    //NPC克人
    case  2:
      {
        percent=randBetween(0,100)/100.0f;
        if (pAtt->maxattack) percent = 1.0f;
        if (pAtt->attacklow) percent = 0.0f;
        if (rev&&(rev->wdMagicType == SERVER_SKILL_DART_ATTACK_NORMAL)&&pDef->huntermark) percent=1.0f;

        /// 计算攻击者的法术攻击力
		if( pAtt->npc->kind == NPC_TYPE_GHOST )
		{
			pAtt->pkValue.mdamage = 
				randBetween(((ScenePet *)pAtt)->petData.matk, ((ScenePet *)pAtt)->petData.maxmatk );
		}
		else
		{
			pAtt->pkValue.mdamage = (DWORD)(pAtt->getMinMDamage()+(pAtt->getMaxMDamage()-pAtt->getMinMDamage())*percent);
		}
        pAtt->pkValue.mdamage = (DWORD)(pAtt->pkValue.mdamage*(1.0f +pAtt->npc->fivepoint/100.0f));

        /// 计算五行对法术防御力的影响
        pDef->pkValue.mdefence = pDef->pkpreValue.nofivemdef;
      }
      break;
    case 1:
      {
        percent=randBetween(0,100)/100.0f;
        if (pAtt->maxattack) percent = 1.0f;
        if (pAtt->attacklow) percent = 0.0f;
        if (rev&&(rev->wdMagicType == SERVER_SKILL_DART_ATTACK_NORMAL)&&pDef->huntermark) percent=1.0f;

        /// 计算攻击者的法术攻击力
		if( pAtt->npc->kind == NPC_TYPE_GHOST )
		{
			pAtt->pkValue.mdamage = 
				randBetween(((ScenePet *)pAtt)->petData.matk, ((ScenePet *)pAtt)->petData.maxmatk );
		}
		else
		{
			pAtt->pkValue.mdamage = (DWORD)(pAtt->getMinMDamage()+(pAtt->getMaxMDamage()-pAtt->getMinMDamage())*percent);
		}

        /// 计算五行对法术防御力的影响
        pDef->pkValue.mdefence = pDef->pkpreValue.fivemdef;
        //pDef->pkValue.mdefence = (DWORD)(pDef->pkValue.mdefence*(1+pDef->packs.equip.getEquips().getDefFivePoint()/100.0f));
      }
      break;
    case 0:
      {
        percent=randBetween(0,100)/100.0f;
        if (pAtt->maxattack) percent = 1.0f;
        if (pAtt->attacklow) percent = 0.0f;
        if (rev&&(rev->wdMagicType == SERVER_SKILL_DART_ATTACK_NORMAL)&&pDef->huntermark) percent=1.0f;

        /// 计算攻击者的法术攻击力
		if( pAtt->npc->kind == NPC_TYPE_GHOST )
		{
			pAtt->pkValue.mdamage = 
				randBetween(((ScenePet *)pAtt)->petData.matk, ((ScenePet *)pAtt)->petData.maxmatk );
		}
		else
		{
			pAtt->pkValue.mdamage = (DWORD)(pAtt->getMinMDamage()+(pAtt->getMaxMDamage()-pAtt->getMinMDamage())*percent);
		}

        /// 计算五行对法术防御力的影响
        pDef->pkValue.mdefence = pDef->pkpreValue.nofivemdef;
      }
      break;
    default:
      break;

  }
}


/**
 * \brief 计算NPC对NPC的魔法伤害值 
 *
 * 在SceneUser:preAttackMe中被调用
 *
 * \param rev 命令指针
 * \param pAtt 攻击者
 * \param pDef 防御者
 *
 */
void ScenePk::calmdamN2N(const Cmd::stAttackMagicUserCmd *rev,SceneNpc *pAtt,SceneNpc *pDef)
{
  //TODO N2N的五行相克计算（法术）
  float percent=0.0f;
  //五行处理,与人与人的处理相反
  switch(pAtt->IsOppose(pDef->npc->five))
  {
    //NPC克人
    case  2:
      {
        percent=randBetween(0,100)/100.0f;
        if (pAtt->maxattack) percent = 1.0f;
        if (pAtt->attacklow) percent = 0.0f;
        if (rev&&(rev->wdMagicType == SERVER_SKILL_DART_ATTACK_NORMAL)&&pDef->huntermark) percent=1.0f;

        /// 计算攻击者的法术攻击力
		if( pAtt->npc->kind == NPC_TYPE_GHOST )
		{
			pAtt->pkValue.mdamage = 
				randBetween(((ScenePet *)pAtt)->petData.matk, ((ScenePet *)pAtt)->petData.maxmatk );
		}
		else
		{
			pAtt->pkValue.mdamage = (DWORD)(pAtt->getMinMDamage()+(pAtt->getMaxMDamage()-pAtt->getMinMDamage())*percent);
		}
        pAtt->pkValue.mdamage = (DWORD)(pAtt->pkValue.mdamage*(1.0f +pAtt->npc->fivepoint/100.0f));


        /// 计算防御者的魔法防御力
        pDef->pkValue.mdefence= 
          randBetween(pDef->getMinMDefence(),pDef->getMaxMDefence());
        if (pDef->pkValue.pdefence > (DWORD)pDef->skillValue.dpdef)
          pDef->pkValue.pdefence -= (DWORD)pDef->skillValue.dpdef;
        else
          pDef->pkValue.pdefence = 0;
      }
      break;
    case 1:
      {
        percent=randBetween(0,100)/100.0f;
        if (pAtt->maxattack) percent = 1.0f;
        if (pAtt->attacklow) percent = 0.0f;
        if (rev&&(rev->wdMagicType == SERVER_SKILL_DART_ATTACK_NORMAL)&&pDef->huntermark) percent=1.0f;

        /// 计算攻击者的法术攻击力
		if( pAtt->npc->kind == NPC_TYPE_GHOST )
		{
			pAtt->pkValue.mdamage = 
				randBetween(((ScenePet *)pAtt)->petData.matk, ((ScenePet *)pAtt)->petData.maxmatk );
		}
		else
		{
			pAtt->pkValue.mdamage = (DWORD)(pAtt->getMinMDamage()+(pAtt->getMaxMDamage()-pAtt->getMinMDamage())*percent);
		}

        /// 计算防御者的魔法防御力
        pDef->pkValue.mdefence= 
          randBetween(pDef->getMinMDefence(),pDef->getMaxMDefence());
        pDef->pkValue.mdefence = (DWORD)(pDef->pkValue.mdefence*(1.0f +pDef->npc->fivepoint/100.0f));
        if (pDef->pkValue.pdefence > (DWORD)pDef->skillValue.dpdef)
          pDef->pkValue.pdefence -= (DWORD)pDef->skillValue.dpdef;
        else
          pDef->pkValue.pdefence = 0;
      }
      break;
    case 0:
      {
        percent=randBetween(0,100)/100.0f;
        if (pAtt->maxattack) percent = 1.0f;
        if (pAtt->attacklow) percent = 0.0f;
        if (rev&&(rev->wdMagicType == SERVER_SKILL_DART_ATTACK_NORMAL)&&pDef->huntermark) percent=1.0f;

        /// 计算攻击者的法术攻击力
		if( pAtt->npc->kind == NPC_TYPE_GHOST )
		{
			pAtt->pkValue.mdamage = 
				randBetween(((ScenePet *)pAtt)->petData.matk, ((ScenePet *)pAtt)->petData.maxmatk );
		}
		else
		{
			pAtt->pkValue.mdamage = (DWORD)(pAtt->getMinMDamage()+(pAtt->getMaxMDamage()-pAtt->getMinMDamage())*percent);
		}

        /// 计算防御者的魔法防御力
        pDef->pkValue.mdefence= 
          randBetween(pDef->getMinMDefence(),pDef->getMaxMDefence());
        if (pDef->pkValue.pdefence > (DWORD)pDef->skillValue.dpdef)
          pDef->pkValue.pdefence -= (DWORD)pDef->skillValue.dpdef;
        else
          pDef->pkValue.pdefence = 0;
      }
      break;
    default:
      break;

  }
}

/**
 * \brief 计算NPC对NPC的物理伤害值 
 *
 * 在SceneUser:preAttackMe中被调用
 *
 * \param rev  攻击命令
 * \param pAtt 攻击者
 * \param pDef 防御者
 *
 */

void ScenePk::calpdamN2N(const Cmd::stAttackMagicUserCmd *rev,SceneNpc *pAtt,SceneNpc *pDef)
{
  //TODO N2N的五行相克计算（物理）
  float percent=0.0f;
  //五行处理,与人与人的处理相反
  switch(pAtt->IsOppose(pDef->npc->five))
  {
    //NPC克人
    case  2:
      {
        percent=randBetween(0,100)/100.0f;
        if (pAtt->maxattack) percent = 1.0f;
        if (pAtt->attacklow) percent = 0.0f;
        if (rev&&(rev->wdMagicType == SERVER_SKILL_DART_ATTACK_NORMAL)&&pDef->huntermark) percent=1.0f;

        /// 计算攻击者的物理攻击力
		if( pAtt->npc->kind == NPC_TYPE_GHOST )
		{
			pAtt->pkValue.pdamage = 
				randBetween(((ScenePet *)pAtt)->petData.atk, ((ScenePet *)pAtt)->petData.maxatk );
		}
		else
		{
			pAtt->pkValue.pdamage = (DWORD)(pAtt->getMinPDamage()+(pAtt->getMaxPDamage()-pAtt->getMinPDamage())*percent);
		}
        pAtt->pkValue.pdamage = (DWORD)(pAtt->pkValue.pdamage*(1.0f +pAtt->npc->fivepoint/100.0f));

        /// 计算防御者的物理防御力
        pDef->pkValue.pdefence= 
          randBetween(pDef->getMinPDefence(),pDef->getMaxPDefence());
      }
      break;
    case 1:
      {
        percent=randBetween(0,100)/100.0f;
        if (pAtt->maxattack) percent = 1.0f;
        if (pAtt->attacklow) percent = 0.0f;
        if (rev&&(rev->wdMagicType == SERVER_SKILL_DART_ATTACK_NORMAL)&&pDef->huntermark) percent=1.0f;

        /// 计算攻击者的物理攻击力
		if( pAtt->npc->kind == NPC_TYPE_GHOST )
		{
			pAtt->pkValue.pdamage = 
				randBetween(((ScenePet *)pAtt)->petData.atk, ((ScenePet *)pAtt)->petData.maxatk );
		}
		else
		{
			pAtt->pkValue.pdamage = (DWORD)(pAtt->getMinPDamage()+(pAtt->getMaxPDamage()-pAtt->getMinPDamage())*percent);
		}

        /// 计算防御者的物理防御力
        pDef->pkValue.pdefence= 
          randBetween(pDef->getMinPDefence(),pDef->getMaxPDefence());
        pDef->pkValue.pdefence = (DWORD)(pDef->pkValue.pdefence*(1.0f +pDef->npc->fivepoint/100.0f));
      }
      break;
    case 0:
      {
        percent=randBetween(0,100)/100.0f;
        if (pAtt->maxattack) percent = 1.0f;
        if (pAtt->attacklow) percent = 0.0f;
        if (rev&&(rev->wdMagicType == SERVER_SKILL_DART_ATTACK_NORMAL)&&pDef->huntermark) percent=1.0f;

        /// 计算攻击者的物理攻击力
		if( pAtt->npc->kind == NPC_TYPE_GHOST )
		{
			pAtt->pkValue.pdamage = 
				randBetween(((ScenePet *)pAtt)->petData.atk, ((ScenePet *)pAtt)->petData.maxatk );
		}
		else
		{
			pAtt->pkValue.pdamage = (DWORD)(pAtt->getMinPDamage()+(pAtt->getMaxPDamage()-pAtt->getMinPDamage())*percent);
		}

        /// 计算防御者的物理防御力
        pDef->pkValue.pdefence= 
          randBetween(pDef->getMinPDefence(),pDef->getMaxPDefence());
      }
      break;
    default:
      break;

  }
}

void SceneUser::reduceGoodness(SceneNpc *pNpc)
{
  if (pNpc->npc->level < charbase.level || getGoodnessState() <= 0)
  {
    return;
  }
  if (++killedNpcNum >= 5)
  {
    killedNpcNum = 0;
    charbase.goodness --;
    switch(charbase.goodness)
    {
      case Cmd::GOODNESS_0:
      case Cmd::GOODNESS_1:
      case Cmd::GOODNESS_2_1:
      case Cmd::GOODNESS_2_2:
      case Cmd::GOODNESS_3:
      case Cmd::GOODNESS_4:
      case Cmd::GOODNESS_5:
      case Cmd::GOODNESS_6:
        {
          // 通知客户端属性变化
          // mark
          //Cmd::stAddUserMapScreenUserCmd send;
          //this->full_t_MapUserData(send.data);
          //this->scene->sendCmdToNine(getPosI(),&send,sizeof(send),false);
          this->reSendMyMapData();
        }
        break;
      default:
        break;

    }
  }
}

/**
 * \brief 死亡动画播放完毕处理
 * 
 * \rev 接收的数据的地址
 * \isOrigin 是否在死亡地点重生
 *
 * \return 攻击是否成功
 */
bool SceneUser::reliveReady(const Cmd::stOKReliveUserCmd *rev,bool isOrigin)
{
  SceneUser *pDef = this;
  bool mapchange = false;
  bool finddare = false;
  bool changeService = false;

  //先通知九屏把自己删掉。
  Cmd::stRemoveUserMapScreenUserCmd send;
  send.dwUserTempID = tempid;
  scene->sendCmdToNineExceptMe(this,getPosI(),&send,sizeof(send));

  if (!isOrigin)
  {
    //准备重生数据
    zPos newPos = this->getPos();
    //在重生区随机查找坐标
    if (this->isSpecWar(Cmd::COUNTRY_FORMAL_DARE) 
     && this->scene->getRealMapID() == 139)
    {
      Scene* pToScene = NULL;
      
      if (this->charbase.country == this->scene->getCountryID())
      {// 防守方
        pToScene = SceneManager::getInstance().getSceneByID(
          SceneManager::getInstance().buildMapID(this->scene->getCountryID(),
            this->scene->getCountryDefBackToMapID()));
      }
      else if (this->scene->getCountryDareBackToMapID())
      {
        pToScene = SceneManager::getInstance().getSceneByID(
          SceneManager::getInstance().buildMapID(this->scene->getCountryID(),
            this->scene->getCountryDareBackToMapID()));
      }

      if (pToScene && pToScene->randzPosByZoneType(ZoneTypeDef::ZONE_PRIVATE_RELIVE,newPos,newPos))
      {
        Zebra::logger->info("查找重生点成功：%s,%u,%u",pToScene->name,newPos.x,newPos.y);
        mapchange = false;
      }

      if (!mapchange)
      {
        zPos findedPos;
        pToScene->findPosForUser(newPos,findedPos);
        this->changeMap(pToScene,findedPos);
        //pToScene->refresh(this,founded ? findedPos : newPos);
        //pDef->scene->setBlock(pDef->getPos());
        finddare=true;
      }
    }
    
    if (!finddare)
    {
      if (this->charbase.country == this->scene->getCountryID())
      {
        if (this->scene->randzPosByZoneType(ZoneTypeDef::ZONE_RELIVE,newPos,newPos))
        {
          Zebra::logger->info("查找重生点成功：%s,%u,%u",this->scene->name,newPos.x,newPos.y);
          mapchange = false;
        }
        else
        {
          int retcode =this->scene->changeMap(this);
          mapchange = (retcode!=0);
          changeService = (2==retcode);
          Zebra::logger->info("切换地图重生%s,%u,%u",this->scene->name,newPos.x,newPos.y);
        }
      }
      else
      {
        int retcode = this->scene->changeMap(this);
        mapchange = (retcode!=0);
        changeService = (2==retcode);
        Zebra::logger->info("切换国外地图重生%s,%u,%u",this->scene->name,newPos.x,newPos.y);
      }
      if (!mapchange)
      {
        zPos findedPos;
        bool founded = this->scene->findPosForUser(newPos,findedPos);
        this->removeNineEntry(getPosI());
        this->scene->refresh(this,founded ? findedPos : newPos);
        pDef->scene->setBlock(pDef->getPos());
      }
    }
  }

  if (changeService) return true;
//  skillStatusM.clearActiveSkillStatus();
  this->setupCharBase();

  Cmd::stMainUserReliveReliveUserCmd ret;
  ret.dwUserTempID = this->tempid;
  ret.x = this->getPos().x;
  ret.y = this->getPos().y;

  this->setState(SceneUser::SceneEntry_Normal);

  this->sendCmdToMe(&ret,sizeof(ret));

  if (!mapchange)
  {
    //得到九屏东西发给自己
    sendNineToMe();
    //通知九屏添加用户
    //ScenePk::sendChangedUserData 会发送sendMeToNine
    //sendMeToNine();
  }
  else
  {
    sendNineToMe();
    //ScenePk::sendChangedUserData 会发送sendMeToNine
    //sendMeToNine();
  }

  /*
   * \brief
   * 通知被攻击人属性
   */
  ScenePk::sendChangedUserData(this);

  //Zebra::logger->debug("复活状态(%s(%ld),%X) ",this->name,this->id,this->charbase.goodness);

  return true;
}

/**
  * \brief 复活处理
  *
  * 三种复活情况的处理：回城复活,金钱复活,技能复活
  *
  * \param relive_type: 复活类型,可选值为:ReliveHome,ReliveMoney,ReliveSkill,
  * \param delaytime: 当复活类型为技能复活时,为技能复活所消耗的时间。
  * \param data_percent: HP,SP,MP损耗百分比
  *
  * \return 复活处理完成,返回TRUE,因复返类型不符等原因而导致不能进行处理时,返回FALSE
  */
bool SceneUser::relive(const int relive_type,const int delaytime,const int data_percent)
{
	int percent = data_percent;
	//using namespace Cmd;
	bool bret = false;
	bool isOrigin = false;
	DWORD punishTime = 0;

	deathWaitTime = 0;

	//有马则下马
	if (horse.mount())
	{
		horse.mount(false,false);
	}

	if (this->charbase.reliveWeakTime >0)
	{
		this->charbase.reliveWeakTime = 0;
		this->setupCharBase();    

		Cmd::stMainUserDataUserCmd  userinfo;
		full_t_MainUserData(userinfo.data);
		sendCmdToMe(&userinfo,sizeof(userinfo));
	}

	switch(relive_type)
	{
	case Cmd::ReliveHome:
		{
			bret = true;
			isOrigin = false;
			percent = 75; //100
			npcdareflag = false;
		}
		break;
	case Cmd::ReliveMoney:
		{
			npcdareflag = false;
			DWORD cost = (int)((0.5 * charbase.level * charbase.level) + 0.5);

			//角色等级<=40级时,扣除金额=公式计算金额*0.4 取整
			if (charbase.level <= 40)
				cost = (int)(cost * 0.4);

			if (packs.checkMoney(cost) && packs.removeMoney(cost,"复活")) {
				bret = true;
				isOrigin = true;
				percent = 100;
				Channel::sendMoney(this,Cmd::INFO_TYPE_GAME,cost,"花掉了原地复活费用");         
			}
			else
			{
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"您没有足够的金钱复活.");
				//没钱回城重生
				bret = false;
				//isOrigin = false;
				//percent = 75;
			}
		}
		break;
	//sky 战场复活 直接原地复活
	case Cmd::ReliveSkill:
		{
			bret = true;
			isOrigin = true;
			deathWaitTime = delaytime;
		}
		break;
	case Cmd::ReliveBattle:
		{
			bret = true;
			isOrigin = true;
			percent = 100;
		}
		break;
	case Cmd::Relive_1_min:
		{
			percent = 100;
			punishTime = 1;
		}
		break;
	case Cmd::Relive_5_min:
		{
			percent = 100;
			punishTime = 5;
		}
		break;
	case Cmd::Relive_10_min:
		{
			//bret = true;
			percent = 100;
			punishTime = 10;
		}
		break;
	default:
		{
			bret = true;
			isOrigin = false;
			percent = 100;
			npcdareflag = false;
		}
		break;
	}
	if (npcdareflag)
	{
		Zebra::logger->info("[家族争夺NPC]家族[%u]中的角色%s被技能复活重新获得对战资格",this->charbase.septid,this->name);
	}

	if (punishTime && lastKiller)
	{
		SceneUser * pAtt = SceneUserManager::getMe().getUserByTempID(lastKiller);
		if (pAtt && pAtt->scene->getRealMapID()!=189 && pAtt->scene->getRealMapID()!=203)
		{
			DWORD cost = (int)(((0.5 * charbase.level * charbase.level) + 0.5) * punishTime);
			if ((packs.checkMoney(cost) && packs.removeMoney(cost,"报案")))
			{
				Channel::sendMoney(this,Cmd::INFO_TYPE_GAME,cost,"花掉了报案费用");

				pAtt->charbase.punishTime = punishTime;
				Scene * s=SceneManager::getInstance().getSceneByName("中立区·监牢");
				if (s)
				{
					bool suc = pAtt->changeMap(s,zPos(80,70));
					if (!suc)
						Zebra::logger->error("%s 杀死 %s,送往监牢失败,目的 %s (%d,%d) 时间 %u 分钟",pAtt->name,name,s->name,80,70,punishTime);
					else
						Zebra::logger->error("%s 杀死 %s,送往监牢,时间 %u 分钟",pAtt->name,name,punishTime);
				}
				else
				{
					//if (pAtt->guard && pAtt->guard->canMove()) pAtt->saveGuard = true;//使镖车跟随指令使用者
					//if (pAtt->adoptList.size()) pAtt->saveAdopt = true;
					Cmd::Session::t_changeScene_SceneSession cmd;
					cmd.id = pAtt->id;
					cmd.temp_id = pAtt->tempid;
					cmd.x = 80;
					cmd.y = 70;
					cmd.map_id = 0;
					cmd.map_file[0] = '\0';
					strncpy((char *)cmd.map_name,"中立区·监牢",MAX_NAMESIZE);
					sessionClient->sendCmd(&cmd,sizeof(cmd));
				}

				Channel::sendCountryInfo(charbase.country,Cmd::INFO_TYPE_EXP,"%s 恶意杀死 %s,现已捉拿归案,入狱 %u 分钟",pAtt->name,name,punishTime);
			}
			else
				Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"您没有足够的金钱报案");
		}
		else
			Channel::sendSys(this,Cmd::INFO_TYPE_FAIL,"凶手已经不知去向...");

		if (lastKiller && !bret)
		{
			lastKiller = 0;

			Cmd::stMainUserDeathReliveUserCmd death;
			death.dwUserTempID = tempid;
			death.deathType = lastKiller?1:0;

			sendCmdToMe(&death,sizeof(death));
		}
	}

	if (bret)
	{
		charbase.hp = (WORD)(charstate.maxhp*percent/100.0f);
		charbase.mp = (WORD)(charstate.maxmp*percent/100.0f);
		charbase.sp = (WORD)(charstate.maxsp*percent/100.0f);
		showCurrentEffect(Cmd::USTATE_DEATH,false);

		if (relive_type == Cmd::ReliveMoney)
		{
			this->charbase.reliveWeakTime = (SceneTimeTick::currentTime.sec() + charbase.level * 3)%10000;
			// 调用预处理方法,进行重算
			this->setupCharBase();

			Cmd::stMainUserDataUserCmd  userinfo;
			full_t_MainUserData(userinfo.data);
			sendCmdToMe(&userinfo,sizeof(userinfo));

			showCurrentEffect(Cmd::USTATE_RELIVEWEAK,true);
			this->sendtoSelectedReliveWeakState();
			this->sendtoSelectedTrainState();
			//this->save(Cmd::Record::OPERATION_WRITEBACK);
		}

		if (deathWaitTime == 0)
		{
			reliveReady(NULL,isOrigin);
		}
		if (relive_type == Cmd::ReliveMoney)
		{
			if (guard) guard->masterIsAlive = true;
		}
	}

	lastKiller = 0;//清除被杀记录
#ifdef _DEBUG
	Zebra::logger->debug("%s 清除被杀记录",name);
#endif
	return bret;
}

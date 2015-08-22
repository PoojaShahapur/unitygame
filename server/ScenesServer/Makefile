#CXX = /usr/bin/g++
CXXFLAGS = $(ZEBRA_COMPILE_OPT) -I../base -Wall -D_GNU_SOURCE -D_REENRANT `mysql_config --cflags` `xml2-config --cflags` `freetype-config --cflags`
CXXLFLAGS = $(ZEBRA_COMPILE_OPT) -Wall -L../base -lzebra -lencdec -llzma -L../luaScript -llua -lluabind -lpthread -lrt -lz `mysql_config --libs_r` `xml2-config --libs` -llog4cxx
LIB_ZEBRA = ../base/libzebra.a

OBJS = \
       ScenesServer.o \
       SceneTask.o \
       SceneTaskManager.o \
       WayPoint.o \
       LoadMap.o \
       Scene.o \
       zSceneEntryIndex.o \
       SceneManager.o \
       SceneEntryPk.o \
       SceneUserManager.o \
       SceneNpc.o \
       SceneNpcManager.o \
       SceneNpcAI.o \
       SessionClient.o \
       RecordClient.o \
       MiniClient.o \
       TimeTick.o \
       SceneUser.o \
       Chat.o \
       CounterManager.o \
       CardTujianManager.o \
       SceneRelation.o	\
       Quest.o \
       QuestAction.o \
       QuestCondition.o \
       QuestEvent.o \
       QuestTable.o \
       script_func.o \
       zObject.o \
       ObjectManager.o \
       Property.o \
       HeroCard.o \
       NpcTrade.o \
       Trade.o \
       Recycle.o \
       ChallengeGameManager.o \
       ChallengeGame.o \
       ChallengePlayer.o \
       HeroInfoManager.o \
       MarketSystemManager.o \
       CardManager.o \
       zCard.o \
       GiftBagManager.o \
       GroupCardManager.o \
       CardPk.o \
       SkillStatusManager.o \
       SceneMsgCmdHandle.o \
       SceneBattle.o \
       Mail.o \
       CardEffectCfg.o \
       SummonCardCfg.o \
       DynamicSceneManager.o \



SRCS = $(OBJS:%.o=%.cpp)
DEPS = $(OBJS:%.o=.%.d)

ALL_TARGETS = ScenesServer

all: $(ALL_TARGETS)
include $(DEPS)
$(DEPS): .%.d: %.cpp
	@$(CXX) -M $(CXXFLAGS) $< > $@.$$$$; sed 's,\($*\)\.o[ :]*,\1.o $@ : ,g' < $@.$$$$ > $@; rm -f $@.$$$$
	
$(OBJS): %.o: %.cpp
	$(CXX) -c $(CXXFLAGS) $< -o $@
	
$(ALL_TARGETS): $(OBJS) $(LIB_ZEBRA)
	$(CXX) $(OBJS) -o $@ $(CXXLFLAGS)
	
clean:
	@rm -rf *.o $(ALL_TARGETS) core *~

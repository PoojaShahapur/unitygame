#include <boost/lexical_cast.hpp>

#include "purgatory_room.h"
#include "timer_queue/timer_queue_root.h"
#include "csv/csv.h"  // for CsvCfg::Init()
#include "aiplayer.h"
#include "formation.h"

const uint32_t AISearchRange[] = {0, 400,1600, (uint32_t)-1};
const uint32_t stageShotCD[] = {0, 3, 2, 2};

PurgatoryRoom::PurgatoryRoom(const uint32_t id)
    :CommonRoom(id)
     , m_stage2StartSeconds(0)
     , m_stage3StartSeconds(0)
{
    init();
    m_roomMode = RoomMode_Purgatory;
}

void PurgatoryRoom::init()
{
    CommonRoom::init();

    auto &tbl = CsvCfg::GetTable("param_Common.csv");

    uint32_t roomLastSeconds = boost::lexical_cast<uint32_t>(tbl.GetRecord("key", "purgatory_room_last_seconds")
            .GetString("value"));
    m_roomEndMillSeconds = roomLastSeconds * 1000 + m_roomCreateMillSeconds;

    m_stage2StartSeconds = boost::lexical_cast<uint32_t>(tbl.GetRecord("key", "purgatory_stage_2_start_seconds")
            .GetString("value"));
    m_pTimerQueue->InsertSingleFromNow((roomLastSeconds-m_stage2StartSeconds) * 1000
            , [this]()
            {
                setAIStage(2);
            });

    m_stage3StartSeconds = boost::lexical_cast<uint32_t>(tbl.GetRecord("key", "purgatory_stage_3_start_seconds")
            .GetString("value"));
    m_pTimerQueue->InsertSingleFromNow((roomLastSeconds-m_stage3StartSeconds) * 1000
            , [this]()
            {
                setAIStage(3);
            });

    setAIStage(1);
}

void PurgatoryRoom::setAIStage(uint32_t stage)
{
    for (const auto &playerIt : m_players)
    {
        if (playerIt.second->isAI())
        {
            AIPlayer* pAIPlayer = (AIPlayer*)playerIt.second.get();
            pAIPlayer->onStageChanged(AISearchRange[stage], stageShotCD[stage]);
        }
    }
}

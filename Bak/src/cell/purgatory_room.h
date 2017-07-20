#pragma once
#include "common_room.h"

class PurgatoryRoom : public CommonRoom{
    public:
        PurgatoryRoom(const uint32_t id);
        ~PurgatoryRoom(){};

        void init();

        // 设置AI阶段
        void setAIStage(uint32_t stage);

    private:
        //炼狱模式一个房间只有1个AI
        CommonPlayerSharedPtr m_pOnlyHumanPlayer;
        // 第二阶段开始秒数
        uint32_t m_stage2StartSeconds;
        uint32_t m_stage3StartSeconds;
};

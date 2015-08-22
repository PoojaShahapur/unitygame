
#ifndef _stNullUserCmd_h
#define _stNullUserCmd_h

#include "zType.h"
#pragma pack(1)

namespace Cmd
{
//////////////////////////////////////////////////////////////
// 空指令定义开始
//////////////////////////////////////////////////////////////
const BYTE NULL_USERCMD_PARA = 0;
struct stNullUserCmd{
  stNullUserCmd()
  {
    dwTimestamp=0;
  }
  union{
    struct {
      BYTE  byCmd;
      BYTE  byParam;
    };
    struct {
      BYTE  byCmdType;
      BYTE  byParameterType;
    };
  };
  DWORD  dwTimestamp;
};
}
#pragma pack()

#endif
//////////////////////////////////////////////////////////////
// 空指令定义结束
//////////////////////////////////////////////////////////////



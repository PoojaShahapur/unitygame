#ifndef _zScene_h_
#define _zScene_h_

#include "zEntry.h"
/**
 * * \brief ¿¿¿¿¿¿¿
 * */
enum enumSceneRunningState{

	SCENE_RUNNINGSTATE_NORMAL,//¿¿¿¿
	    SCENE_RUNNINGSTATE_UNLOAD,//¿¿¿¿
		SCENE_RUNNINGSTATE_REMOVE,//¿¿¿¿
};

/**
* \brief ³¡¾°»ù±¾ÐÅÏ¢¶¨Òå
*/
struct zScene:public zEntry
{
private:
	DWORD running_state;
public:
	zScene():running_state(SCENE_RUNNINGSTATE_NORMAL){}
	DWORD getRunningState() const
	{
		return running_state;
	}
	DWORD setRunningState(DWORD set)
	{
		running_state = set;
		return running_state;
	}
};
#endif


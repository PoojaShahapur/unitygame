#ifndef __IDRAGDROPSYS_H
#define __IDRAGDROPSYS_H

#include "AIEditor.h"

class QGraphicsItem;

class IDragDropSys
{
public:
	virtual void startDrag() = 0;
	virtual void stopDrag() = 0;
	virtual void drog() = 0;
};

#endif // __IDRAGDROPSYS_H
#ifndef __DRAGDROPSYS_H
#define __DRAGDROPSYS_H

#include "AIEditor.h"

class QGraphicsItem;

class DragDropSys
{
protected:
	bool m_startDrag;
	QGraphicsItem* m_dragItem;

public:
	DragDropSys();
	~DragDropSys();

	void startDrag();
	void stopDrag();
	void drog();
};

#endif // DRAGDROPSYS_H
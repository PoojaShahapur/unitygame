#ifndef __DRAGDROPSYS_H
#define __DRAGDROPSYS_H

#include "AIEditor.h"
#include "IDragDropSys.h"

class QGraphicsItem;
class BuildFactory;

class DragDropSys : public IDragDropSys
{
protected:
	bool m_startDrag;
	QGraphicsItem* m_pDragItem;

public:
	DragDropSys();
	~DragDropSys();

	void startDrag();
	void stopDrag();
	void drog();
};

#endif // DRAGDROPSYS_H
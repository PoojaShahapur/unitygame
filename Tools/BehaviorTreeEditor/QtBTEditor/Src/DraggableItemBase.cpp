#include "DraggableItemBase.h"
#include <QDrag>
#include <QMimeData>
#include "DraggableFrame.h"

DraggableItemBase::DraggableItemBase()
{
	m_draggableFrame = new DraggableFrame();
}

void DraggableItemBase::dragMoveEvent(QGraphicsSceneDragDropEvent* evt)
{
	
}

QRectF DraggableItemBase::boundingRect() const
{
	return QRectF(0, 0, 10, 10);
}

void DraggableItemBase::paint(QPainter *painter, const QStyleOptionGraphicsItem *option, QWidget *widget)
{
	
}
#include "DraggableItemBase.h"
#include <QDrag>
#include <QMimeData>
#include "DraggableItemWidget.h"

#include <QtCore>
#include <QtWidgets>

DraggableItemBase::DraggableItemBase()
{
	m_draggableItemWidget = new DraggableItemWidget();
	QGraphicsProxyWidget* pProxyWidget = new QGraphicsProxyWidget(this);
	pProxyWidget->setWidget(m_draggableItemWidget);
}

void DraggableItemBase::dragMoveEvent(QGraphicsSceneDragDropEvent* evt)
{
	
}

QRectF DraggableItemBase::boundingRect() const
{
	return QRectF(0, 0, 210, 200);
}

void DraggableItemBase::paint(QPainter *painter, const QStyleOptionGraphicsItem *option, QWidget *widget)
{
	painter->setPen(QPen(QColor(255, 0, 0), 3.0));
	painter->drawRect(QRectF(0, 0, 210, 200));
}
#ifndef DRAGGABLEITEMBASE_H
#define DRAGGABLEITEMBASE_H

#include <QGraphicsItem>
#include <QGraphicsSceneMouseEvent>

class DraggableItemWidget;
class QWidget;

class DraggableItemBase : public QGraphicsObject
{
protected:
	DraggableItemWidget* m_draggableItemWidget;

public:
	DraggableItemBase();

protected:
	virtual void dragMoveEvent(QGraphicsSceneDragDropEvent* evt);
	QRectF boundingRect() const Q_DECL_OVERRIDE;
	void paint(QPainter *painter, const QStyleOptionGraphicsItem *option, QWidget *widget) Q_DECL_OVERRIDE;
};

#endif // DRAGGABLEITEMBASE_H
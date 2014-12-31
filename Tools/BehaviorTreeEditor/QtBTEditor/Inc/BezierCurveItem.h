#ifndef BEZIERCURVEITEM_H
#define BEZIERCURVEITEM_H

#include <QGraphicsItem>
#include <QGraphicsSceneMouseEvent>

class DraggableItemWidget;
class QWidget;

class BezierCurveItem : public QGraphicsObject
{
public:
	BezierCurveItem();

protected:
	QRectF boundingRect() const Q_DECL_OVERRIDE;
	void paint(QPainter *painter, const QStyleOptionGraphicsItem *option, QWidget *widget) Q_DECL_OVERRIDE;
};

#endif // BEZIERCURVEITEM_H
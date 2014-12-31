#ifndef BEZIERCURVEITEM_H
#define BEZIERCURVEITEM_H

#include <QGraphicsItem>
#include <QGraphicsSceneMouseEvent>
#include <QPoint>

class DraggableItemWidget;
class QWidget;

class BezierCurveItem : public QGraphicsObject
{
protected:
	int m_offset;		// �м��ƫ��
	QPoint m_startPos;
	QPoint m_endPos;
	QPoint m_midPos;
	QPoint m_midStartPos;
	QPoint m_midEndPos;

public:
	BezierCurveItem();

protected:
	QRectF boundingRect() const Q_DECL_OVERRIDE;
	void paint(QPainter *painter, const QStyleOptionGraphicsItem *option, QWidget *widget) Q_DECL_OVERRIDE;
};

#endif // BEZIERCURVEITEM_H
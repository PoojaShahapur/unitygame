#ifndef __BEZIERCURVEITEM_H
#define __BEZIERCURVEITEM_H

#include <QGraphicsItem>
#include <QGraphicsSceneMouseEvent>
#include <QPoint>

class DraggableItemWidget;
class QWidget;

class BezierCurveItem : public QGraphicsObject
{
protected:
	int m_offset;		// ÖÐ¼äµãÆ«ÒÆ
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

protected:
	void updateCtrlPos();
};

#endif // BEZIERCURVEITEM_H
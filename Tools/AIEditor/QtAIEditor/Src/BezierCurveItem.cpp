#include "BezierCurveItem.h"

#include <QtCore>
#include <QtWidgets>

BezierCurveItem::BezierCurveItem()
{
	m_startPos.setX(300);
	m_startPos.setY(50);

	m_endPos.setX(100);
	m_endPos.setY(200);
}

QRectF BezierCurveItem::boundingRect() const
{
	return QRectF(0, 0, 400, 400);
}

void BezierCurveItem::paint(QPainter *painter, const QStyleOptionGraphicsItem *option, QWidget *widget)
{
	painter->setRenderHint(QPainter::Antialiasing, true);
	QPen pen;
	pen.setColor(Qt::red);
	pen.setWidth(2);
	pen.setStyle(Qt::DashDotLine);
	painter->setPen(pen);
	
	//define startpos endpos
	QPoint start_pos(300, 50);
	QPoint end_pos(100, 200);
	
	//drawPath;
	QPainterPath path(start_pos);
	//QPoint c1((start_pos + end_pos).x() / 2, start_pos.y());
	//QPoint c2((start_pos + end_pos).x() / 2, end_pos.y());
	//QPoint c1(start_pos.x(), (start_pos + end_pos).y() / 2);
	//QPoint c2(end_pos.x(), (start_pos + end_pos).y() / 2);
	
	//path.cubicTo(c1, c2, end_pos);

	QPoint midPt((start_pos + end_pos).x() / 2, (start_pos + end_pos).y() / 2);

	QPoint c1(start_pos.x() + 200, (start_pos + midPt).y() / 2);
	QPoint c2(midPt.x(), midPt.y());
	QPoint c3(end_pos.x() - 200, (midPt + end_pos).y() / 2);
	path.quadTo(c1, c2);

	painter->drawPath(path);

	QPainterPath bPath(c2);
	bPath.quadTo(c3, end_pos);
	painter->drawPath(bPath);

	//
	QPen penshaper;
	penshaper.setStyle(Qt::SolidLine);
	penshaper.setColor(Qt::darkRed);
	painter->setPen(penshaper);
	
	//#define the rect size;
	QSize rect_size(60, 60);
	QRect rect_shape1(start_pos - QPoint(rect_size.height() / 2, rect_size.width() / 2), rect_size);
	QRect rect_shape2(end_pos - QPoint(rect_size.height() / 2, rect_size.width() / 2), rect_size);
	
	QBrush brush(Qt::darkGray, Qt::CrossPattern);
	painter->fillRect(rect_shape1, brush);
	painter->fillRect(rect_shape2, brush);
	painter->drawRoundedRect(rect_shape1, 10, 10);
	painter->drawRoundedRect(rect_shape2, 10, 10);
	
	QPen dotpen;
	dotpen.setWidth(8);
	dotpen.setColor(Qt::red);
	
	brush.setStyle(Qt::SolidPattern);
	painter->setPen(dotpen);
	painter->drawPoint(start_pos);
	painter->drawPoint(end_pos);
}
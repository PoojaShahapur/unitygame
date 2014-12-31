#include "BezierCurveItem.h"

#include <QtCore>
#include <QtWidgets>

BezierCurveItem::BezierCurveItem()
{
	m_offset = 200;
	m_startPos.setX(300);
	m_startPos.setY(50);

	m_endPos.setX(100);
	m_endPos.setY(200);

	updateCtrlPos();
}

void BezierCurveItem::updateCtrlPos()
{
	m_midPos.setX((m_startPos + m_endPos).x() / 2);
	m_midPos.setY((m_startPos + m_endPos).y() / 2);

	m_midStartPos.setX(m_startPos.x() + m_offset);
	m_midStartPos.setY((m_startPos + m_midPos).y() / 2);

	m_midEndPos.setX(m_endPos.x() - m_offset);
	m_midEndPos.setY((m_midPos + m_endPos).y() / 2);
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
	
	//drawPath;
	QPainterPath aPath(m_startPos);
	aPath.quadTo(m_midStartPos, m_midPos);
	painter->drawPath(aPath);

	QPainterPath bPath(m_midPos);
	bPath.quadTo(m_midEndPos, m_endPos);
	painter->drawPath(bPath);

	QPen penshaper;
	penshaper.setStyle(Qt::SolidLine);
	penshaper.setColor(Qt::darkRed);
	painter->setPen(penshaper);
	
	//#define the rect size;
	QSize rect_size(60, 60);
	QRect rect_shape1(m_startPos - QPoint(rect_size.height() / 2, rect_size.width() / 2), rect_size);
	QRect rect_shape2(m_endPos - QPoint(rect_size.height() / 2, rect_size.width() / 2), rect_size);
	
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
	painter->drawPoint(m_startPos);
	painter->drawPoint(m_endPos);
}
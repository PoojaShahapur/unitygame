#include "GraphicsView.h"

GraphicsView::GraphicsView(QWidget *parent)
	: QGraphicsView(parent)
{
}

//void GraphicsView::paintEvent(QPaintEvent *e)
//{
//	QPainter painter(this);
//	painter.setRenderHint(QPainter::Antialiasing, true);
//	QPen pen;
//	pen.setColor(Qt::red);
//	pen.setWidth(2);
//	pen.setStyle(Qt::DashDotLine);
//	painter.setPen(pen);
//
//	//define startpos endpos
//	QPoint start_pos(300, 50);
//	QPoint end_pos(100, 200);
//
//	//drawPath;
//	QPainterPath path(start_pos);
//	QPoint c1((start_pos + end_pos).x() / 2, start_pos.y());
//	QPoint c2((start_pos + end_pos).x() / 2, end_pos.y());
//
//	path.cubicTo(c1, c2, end_pos);
//	painter.drawPath(path);
//	//
//	QPen penshaper;
//	penshaper.setStyle(Qt::SolidLine);
//	penshaper.setColor(Qt::darkRed);
//	painter.setPen(penshaper);
//
//	//#define the rect size;
//	QSize rect_size(60, 60);
//	QRect rect_shape1(start_pos - QPoint(rect_size.height() / 2, rect_size.width() / 2), rect_size);
//	QRect rect_shape2(end_pos - QPoint(rect_size.height() / 2, rect_size.width() / 2), rect_size);
//
//	QBrush brush(Qt::darkGray, Qt::CrossPattern);
//	painter.fillRect(rect_shape1, brush);
//	painter.fillRect(rect_shape2, brush);
//	painter.drawRoundedRect(rect_shape1, 10, 10);
//	painter.drawRoundedRect(rect_shape2, 10, 10);
//
//	QPen dotpen;
//	dotpen.setWidth(8);
//	dotpen.setColor(Qt::red);
//
//	brush.setStyle(Qt::SolidPattern);
//	painter.setPen(dotpen);
//	painter.drawPoint(start_pos);
//	painter.drawPoint(end_pos);
//}

void GraphicsView::mouseMoveEvent(QMouseEvent * e)//鼠标移动事件响应
{
	QPoint coursePoint;
	coursePoint = e->pos();//获取当前光标的位置
	QPointF convPt = this->mapToScene(coursePoint);
}

void GraphicsView::mousePressEvent(QMouseEvent * e)//鼠标单击事件响应
{

}

void GraphicsView::mouseReleaseEvent(QMouseEvent * e)//鼠标松开事件响应
{

}
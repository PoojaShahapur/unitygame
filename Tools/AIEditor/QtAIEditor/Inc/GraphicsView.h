#ifndef __GRAPHICSVIEW_H
#define __GRAPHICSVIEW_H

#include <QGraphicsView>
#include <QtGui/QtGui>
#include <QtCore/QtCore>

class GraphicsView : public QGraphicsView
{
    Q_OBJECT
public:
	explicit GraphicsView(QWidget *parent = 0);

protected:
	//void paintEvent(QPaintEvent *e);
	// 添加了事件就不能自动移动了
	//void mouseMoveEvent(QMouseEvent * e);//鼠标移动事件响应
	//void mousePressEvent(QMouseEvent * e);//鼠标单击事件响应
	//void mouseReleaseEvent(QMouseEvent * e);//鼠标松开事件响应
};

#endif // GRAPHICSVIEW_H
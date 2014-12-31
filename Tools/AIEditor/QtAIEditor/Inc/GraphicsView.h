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
};

#endif // GRAPHICSVIEW_H
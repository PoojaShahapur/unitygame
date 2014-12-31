#ifndef __DRAGWIDGET_H
#define __DRAGWIDGET_H

#include <QWidget>
#include <QGraphicsItem>

class DragWidget : public QWidget
{
    Q_OBJECT

public:
	DragWidget(QWidget *parent = 0);
    void makeDraggable(QGraphicsItem *);
	~DragWidget();
};

#endif // __DRAGWIDGET_H
#ifndef WIDGET_H
#define WIDGET_H

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

#endif // WIDGET_H
#include "draggablepiece.h"
#include <QDrag>
#include <QMimeData>

DraggablePiece::DraggablePiece()
{
}

void DraggablePiece::mousePressEvent(QGraphicsSceneMouseEvent *event)
{
    QMimeData *data = new QMimeData;
	QVariant variant = QVariant::fromValue((int)Qt::green);
	data->setColorData(variant);

	QDrag *drag = new QDrag((QObject*)event->widget());
    drag->setMimeData(data);
    drag->start();
}
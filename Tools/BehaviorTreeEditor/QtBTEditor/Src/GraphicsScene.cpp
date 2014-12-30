#include "GraphicsScene.h"
#include <QDebug>

GraphicsScene::GraphicsScene(QObject *parent)
    : QGraphicsScene(parent)
{
}


void GraphicsScene::mouseMoveEvent(QGraphicsSceneMouseEvent * m)
{
//    qDebug() << //Q_FUNC_INFO <<
//                m->scenePos();
//    m->ignore();
    QGraphicsScene::mouseMoveEvent(m);
}

void GraphicsScene::mousePressEvent(QGraphicsSceneMouseEvent *evt)
{
	QGraphicsScene::mousePressEvent(evt);
}

void GraphicsScene::mouseReleaseEvent(QGraphicsSceneMouseEvent *evt)
{
	QGraphicsScene::mouseReleaseEvent(evt);
}
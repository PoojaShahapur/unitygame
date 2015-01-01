#include "BuildFactory.h"
#include "BezierCurveItem.h"

QGraphicsObject* BuildFactory::buildBezierCurveItem()
{
	return new BezierCurveItem();
}
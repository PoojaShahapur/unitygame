#ifndef __IBUILDFACTORY_H
#define __IBUILDFACTORY_H

class QGraphicsObject;

class IBuildFactory
{
public:
	virtual QGraphicsObject* buildBezierCurveItem() = 0;
};

#endif // __IBUILDFACTORY_H
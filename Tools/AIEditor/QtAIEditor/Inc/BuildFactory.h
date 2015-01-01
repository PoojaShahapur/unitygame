#ifndef __BUILDFACTORY_H
#define __BUILDFACTORY_H

#include "IBuildFactory.h"

class QGraphicsObject;

class BuildFactory : public IBuildFactory
{
public:
	QGraphicsObject* buildBezierCurveItem();
};

#endif // __BUILDFACTORY_H
#include "DragDropSys.h"
#include "BuildFactory.h"
#include "QtAIEditorSys.h"

DragDropSys::DragDropSys()
{
	m_startDrag = false;
}

DragDropSys::~DragDropSys()
{

}

void DragDropSys::startDrag()
{
	m_startDrag = true;

	if (m_pDragItem == nullptr)
	{
		m_pDragItem = QtAIEditorSys::getSingletonPtr()->m_pBuildFactory->buildBezierCurveItem();
	}
}

void DragDropSys::stopDrag()
{
	m_startDrag = false;
	m_pDragItem = nullptr;
}

void DragDropSys::drog()
{
	m_startDrag = false;
	m_pDragItem = nullptr;
}
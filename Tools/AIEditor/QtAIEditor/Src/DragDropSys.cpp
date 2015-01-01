#include "DragDropSys.h"

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

	if (m_dragItem == nullptr)
	{
		//m_dragItem = new 
	}
}

void DragDropSys::stopDrag()
{
	m_startDrag = false;
}

void DragDropSys::drog()
{
	m_startDrag = false;
}
#include "QtAIEditorSys.h"
#include "DragDropSys.h"
#include "BuildFactory.h"

QtAIEditorSys* QtAIEditorSys::m_sSingleton = 0;

QtAIEditorSys::QtAIEditorSys()
{
	m_pDragDropSys = new DragDropSys();
	m_pBuildFactory = new BuildFactory();
}

QtAIEditorSys* QtAIEditorSys::getSingletonPtr()
{
	if (m_sSingleton == nullptr)
	{
		m_sSingleton = new QtAIEditorSys();
	}
	return m_sSingleton;
}

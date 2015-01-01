#include "QtAIEditorSys.h"

QtAIEditorSys* QtAIEditorSys::m_sSingleton = 0;

QtAIEditorSys* QtAIEditorSys::getSingletonPtr()
{
	if (m_sSingleton == nullptr)
	{
		m_sSingleton = new QtAIEditorSys();
	}
	return m_sSingleton;
}

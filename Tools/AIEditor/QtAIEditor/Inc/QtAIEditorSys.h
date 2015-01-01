#ifndef __QTAIEDITORSYS_H_
#define __QTAIEDITORSYS_H_

#include "AIEditor.h"
#include "DragDropSys.h"

class QtAIEditorSys : public AIEditorSys
{
public:
	DragDropSys m_dragDropSys;				// 拖放数据

	// 这里写是为了解决链接错误
protected:
	static QtAIEditorSys* m_sSingleton;

public:
	static QtAIEditorSys* getSingletonPtr();
};

#endif		// __QTAIEDITORSYS_H_
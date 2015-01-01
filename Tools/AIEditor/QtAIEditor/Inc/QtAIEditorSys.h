#ifndef __QTAIEDITORSYS_H_
#define __QTAIEDITORSYS_H_

#include "AIEditor.h"

#include "IDragDropSys.h"
#include "IBuildFactory.h"

class QtAIEditorSys : public AIEditorSys
{
public:
	IDragDropSys* m_pDragDropSys;				// 拖放数据
	IBuildFactory* m_pBuildFactory;				// 工厂生成

public:
	QtAIEditorSys();

	// 这里写是为了解决链接错误
protected:
	static QtAIEditorSys* m_sSingleton;

public:
	static QtAIEditorSys* getSingletonPtr();
};

#endif		// __QTAIEDITORSYS_H_
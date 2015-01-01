#ifndef __QTAIEDITORSYS_H_
#define __QTAIEDITORSYS_H_

#include "AIEditor.h"
#include "DragDropSys.h"

class QtAIEditorSys : public AIEditorSys
{
public:
	DragDropSys m_dragDropSys;				// �Ϸ�����

	// ����д��Ϊ�˽�����Ӵ���
protected:
	static QtAIEditorSys* m_sSingleton;

public:
	static QtAIEditorSys* getSingletonPtr();
};

#endif		// __QTAIEDITORSYS_H_
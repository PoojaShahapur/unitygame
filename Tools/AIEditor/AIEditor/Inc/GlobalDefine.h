#ifndef __GlobalDefine_H_
#define __GlobalDefine_H_

//#define NO_NAMESPACE

#ifndef NO_NAMESPACE_BTEDITOR
	#define NSMESPACE_NAME_BTEDITOR BTEditor
	#define BEGIN_NAMESPACE_BTEDITOR namespace BTEditor {
	#define END_NAMESPACE_BTEDITOR }
	#define USING_NAMESPACE_BTEDITOR using namespace BTEditor;
#else
	#define NSMESPACE_NAME_BTEDITOR
	#define BEGIN_NAMESPACE_BTEDITOR
	#define END_NAMESPACE_BTEDITOR
	#define USING_NAMESPACE_BTEDITOR
#endif

#endif
#ifndef __GlobalDefine_H_
#define __GlobalDefine_H_

//#define NO_NAMESPACE

#ifndef NO_NAMESPACE_AIEDITOR
	#define NSMESPACE_NAME_AIEDITOR AIEditor
	#define BEGIN_NAMESPACE_AIEDITOR namespace AIEditor {
	#define END_NAMESPACE_AIEDITOR }
	#define USING_NAMESPACE_AIEDITOR using namespace AIEditor;
#else
	#define NSMESPACE_NAME_AIEDITOR
	#define BEGIN_NAMESPACE_AIEDITOR
	#define END_NAMESPACE_AIEDITOR
	#define USING_NAMESPACE_AIEDITOR
#endif

#endif
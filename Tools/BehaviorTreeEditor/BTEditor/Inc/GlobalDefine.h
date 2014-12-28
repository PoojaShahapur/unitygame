#ifndef __GlobalDefine_H_
#define __GlobalDefine_H_

#define NO_NAMESPACE

#ifndef NO_NAMESPACE
	#define NSMESPACE_NAME BTEditor
	#define BEGIN_NAMESPACE namespace BTEditor {
	#define END_NAMESPACE }
	#define USING_NAMESPACE using BTEditor;
#else
	#define NSMESPACE_NAME
	#define BEGIN_NAMESPACE
	#define END_NAMESPACE
	#define USING_NAMESPACE
#endif

#endif
//#define _CRTDBG_MAP_ALLOC
//#include <stdlib.h>
//#include <crtdbg.h>

#include <QtWidgets/QApplication>
//#include <corelib/codecs/qtextcodec.h>
#include <QtCore/QtCore>
#include "maindialog.hxx"

// 设置执行编码
#if defined(_MSC_VER) && (_MSC_VER >= 1600)
	#pragma execution_character_set("utf-8")
#endif

//#define _CRTDBG_MAP_ALLOC
//#include <stdlib.h>
//#include <crtdbg.h>

//#ifdef _DEBUG
//	#include <crtdbg.h>
//#endif

#include "MemLeakCheck.hxx"

//定义函数：  
void EnableMemLeakCheck()
{
	//_CrtSetDbgFlag(_CRTDBG_REPORT_FLAG | _CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);
	_CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);
}

//#ifdef _DEBUG
//#define new new(_NORMAL_BLOCK, __FILE__, __LINE__)
//#endif

int main(int argc, char *argv[])
{
	#ifdef _DEBUG
	// 内存检测
	EnableMemLeakCheck();
	_CrtSetBreakAlloc(78314);
	#endif
	//QTextCodec *gbk = QTextCodec::codecForName("gb18030");
	//QTextCodec *gbk = QTextCodec::codecForName("GB2312");
	//QTextCodec *gbk = QTextCodec::codecForName("system");
	//QTextCodec *gbk = QTextCodec::codecForName("GBK");
	//QTextCodec *utf8 = QTextCodec::codecForName("UTF-8");
    //QTextCodec::setCodecForTr(gbk);
    //QTextCodec::setCodecForLocale(gbk);
	//QTextCodec::setCodecForLocale(utf8);
    //QTextCodec::setCodecForCStrings(gbk);
	//QTextCodec::setCodecForLocale(QTextCodec::codecForName("UTF8"));

	#if QT_VERSION < QT_VERSION_CHECK(5,0,0)   
	#if defined(_MSC_VER) && (_MSC_VER < 1600)	//VS版本低于VS2010    
		QTextCodec::setCodecForTr(QTextCodec::codecForName("GBK"));
	#else    
		QTextCodec::setCodecForTr(QTextCodec::codecForName("UTF-8"));
	#endif    
	#else
		// 设置本地 Qt 认为的编码为 UTF-8，但是 VS 中文本地仍然是 GB2312 编码
		QTextCodec::setCodecForLocale(QTextCodec::codecForName("UTF-8"));	// 执行编码需要 UTF-8，不要再设置别的编码
	#endif

    QApplication app(argc, argv);
    MainDialog window;
    window.show();
    return app.exec();
}
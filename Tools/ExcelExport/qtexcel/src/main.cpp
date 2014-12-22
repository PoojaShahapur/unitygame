﻿#include <QtWidgets/QApplication>
//#include <corelib/codecs/qtextcodec.h>
#include <QtCore/QtCore>
#include "maindialog.hxx"

// 设置执行编码
#if defined(_MSC_VER) && (_MSC_VER >= 1600)
	#pragma execution_character_set("utf-8")
#endif

int main(int argc, char *argv[])
{
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
		QTextCodec::setCodecForLocale(QTextCodec::codecForName("UTF-8"));	// 执行编码需要 UTF-8，不要再设置别的编码
	#endif

    QApplication app(argc, argv);
    MainDialog window;
    window.show();
    return app.exec();
}
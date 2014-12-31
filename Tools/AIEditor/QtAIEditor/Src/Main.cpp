#include <QtWidgets/QApplication>
#include <QTranslator>

#include "MainWindow.h"

#if _MSC_VER >= 1600
	#pragma execution_character_set("utf-8")
#endif

int main(int argc, char *argv[])
{
	#if QT_VERSION < QT_VERSION_CHECK(5,0,0)  
		#if defined(_MSC_VER) && (_MSC_VER < 1600)  
			QTextCodec::setCodecForTr(QTextCodec::codecForName("GB18030-0"));
		#else  
			QTextCodec::setCodecForTr(QTextCodec::codecForName("UTF-8"));
		#endif  
	#endif  

	QApplication* pApp = new QApplication(argc, argv);
	QTranslator translator(0);
	translator.load("RunPath/languages/qt_zh.qm");
	pApp->installTranslator(&translator);
	MainWindow* pMainWindow = new MainWindow();
	pMainWindow->show();

	return pApp->exec();
}
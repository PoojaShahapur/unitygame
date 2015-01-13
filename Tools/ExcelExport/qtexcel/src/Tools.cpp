#include "Tools.hxx"
#include <QtGui/QtGui>
#include <QtWidgets>
#include <direct.h>		// getcwd

template<> Tools* Singleton<Tools>::msSingleton = 0;

Tools::Tools()
{
	m_parent = NULL;
	m_running = false;
	//m_outTextEdit = NULL;
	m_bytes = new char[4096];
}

Tools::~Tools()
{
	m_infoListMutex.lock();
	m_infoInList.clear();
	m_infoOutList.clear();
	m_infoListMutex.unlock();

	m_msgListMutex.lock();
	m_msgInList.clear();
	m_msgOutList.clear();
	m_msgListMutex.unlock();
}

QString Tools::openFileDialog(QString dir, QString filter)
{
    QFileDialog::Options options;
    options |= QFileDialog::DontUseNativeDialog;
    QString selectedFilter;
	QString fileName;
    //fileName = QFileDialog::getOpenFileName(NULL,
	//							QObject::tr("QFileDialog::getOpenFileName()"),
    //                            dir,
	//							QObject::tr("All Files (*);;Text Files (*.txt)"),
    //                            &selectedFilter,
    //                            options);

	fileName = QFileDialog::getOpenFileName(NULL,
								QObject::tr("Open Files"),
                                dir,
								filter,
                                &selectedFilter,
                                options);
    return fileName;
}

QString Tools::openDirectoryDialog(QString path)
{
	QString directory;
	//directory = QFileDialog::getExistingDirectory(NULL, QObject::tr("Find Files"), QDir::currentPath());

	directory = QFileDialog::getExistingDirectory(NULL, QObject::tr("Open Directory"), path);

    return directory;
}

std::string Tools::GetFileNameExt(const char* pszFileName)
{
	const char* p = strrchr(pszFileName, '.');
	if(p && (*p)) return (p + 1);
	return "";
}

// 这个就是日志信息，不在 UI 显示信息
void Tools::informationMessage(QString msg, QString caption, QWidget *parent)
{
    //QMessageBox::StandardButton reply;
	//reply = QMessageBox::information(parent, QObject::tr("QMessageBox::information()"), msg);

	//if(parent != NULL)
	//{
	//	QMessageBox::information(parent, caption, msg);
	//}
	//else
	//{
	//	QMessageBox::information(m_parent, caption, msg);
	//}

	LogMsg(msg);
}

// 这个就是日志信息，不在 UI 显示信息
void Tools::informationMessageUI(QString msg, QString caption, QWidget *parent)
{
	if(parent != NULL)
	{
		QMessageBox::information(parent, caption, msg);
	}
	else
	{
		QMessageBox::information(m_parent, caption, msg);
	}
}

void Tools::setParent(QWidget *parent)
{
	m_parent = parent;
}

bool Tools::isRunning()
{
	return m_running;
}

void Tools::setRunning(bool run)
{
	m_running = run;
}

// 输出日志    
void Tools::Log(QString desc)
{
	m_infoListMutex.lock();
	// 清除之前的内容  
	//m_outTextEdit->clear();
	//m_outTextEdit->appendPlainText(desc);
	//m_outTextEdit->setPlainText(desc);
	//m_outTextEdit->setPlainText("adsfasdfa"); 不允许在不同的线程里面发送信号      
	m_infoInList.push_back(desc);
	m_infoListMutex.unlock();
}

QVector<QString>& Tools::getLog()
{
	m_infoListMutex.lock();
	m_infoOutList.clear();
	QVector<QString>::Iterator begin = m_infoInList.begin();
	while(begin != m_infoInList.end())
	{
		m_infoOutList.push_back(*begin);
		++begin;
	}
	m_infoInList.clear();
	m_infoListMutex.unlock();

	return m_infoOutList;
}

// 输出日志    
void Tools::LogMsg(QString desc)
{
	m_msgListMutex.lock();
	// 清除之前的内容  
	//m_outTextEdit->clear();
	//m_outTextEdit->appendPlainText(desc);
	//m_outTextEdit->setPlainText(desc);
	//m_outTextEdit->setPlainText("adsfasdfa"); 不允许在不同的线程里面发送信号      
	m_msgInList.push_back(desc);
	m_msgListMutex.unlock();
}

QVector<QString>& Tools::getLogMsg()
{
	m_msgListMutex.lock();
	m_msgOutList.clear();
	QVector<QString>::Iterator begin = m_msgInList.begin();
	while (begin != m_msgInList.end())
	{
		m_msgOutList.push_back(*begin);
		++begin;
	}
	m_msgInList.clear();
	m_msgListMutex.unlock();

	return m_msgOutList;
}

//void Tools::setTextEdit(QTextEdit* textEdit)
//{
//	m_outTextEdit = textEdit;
//}

// 全局函数，使用 windows 编码解码
//GBK编码转换到UTF8编码
int Tools::GBKToUTF8(char * lpGBKStr, char * lpUTF8Str, int nUTF8StrLen)
{
	wchar_t * lpUnicodeStr = NULL;
	int nRetLen = 0;

	if (!lpGBKStr)  //如果GBK字符串为NULL则出错退出
		return 0;

	nRetLen = ::MultiByteToWideChar(CP_ACP, 0, (char *)lpGBKStr, -1, NULL, NULL);  //获取转换到Unicode编码后所需要的字符空间长度
	lpUnicodeStr = new WCHAR[nRetLen + 1];  //为Unicode字符串空间
	nRetLen = ::MultiByteToWideChar(CP_ACP, 0, (char *)lpGBKStr, -1, lpUnicodeStr, nRetLen);  //转换到Unicode编码
	if (!nRetLen)  //转换失败则出错退出
		return 0;

	nRetLen = ::WideCharToMultiByte(CP_UTF8, 0, lpUnicodeStr, -1, NULL, 0, NULL, NULL);  //获取转换到UTF8编码后所需要的字符空间长度

	if (!lpUTF8Str)  //输出缓冲区为空则返回转换后需要的空间大小
	{
		if (lpUnicodeStr)
			delete[]lpUnicodeStr;
		return nRetLen;
	}

	if (nUTF8StrLen < nRetLen)  //如果输出缓冲区长度不够则退出
	{
		if (lpUnicodeStr)
			delete[]lpUnicodeStr;
		return 0;
	}

	nRetLen = ::WideCharToMultiByte(CP_UTF8, 0, lpUnicodeStr, -1, (char *)lpUTF8Str, nUTF8StrLen, NULL, NULL);  //转换到UTF8编码

	if (lpUnicodeStr)
		delete[]lpUnicodeStr;

	return nRetLen;
}

// UTF8编码转换到GBK编码
int Tools::UTF8ToGBK(char * lpUTF8Str, char * lpGBKStr, int nGBKStrLen)
{
	wchar_t * lpUnicodeStr = NULL;
	int nRetLen = 0;

	if (!lpUTF8Str)  //如果UTF8字符串为NULL则出错退出
		return 0;

	nRetLen = ::MultiByteToWideChar(CP_UTF8, 0, lpUTF8Str, -1, NULL, NULL);  //获取转换到Unicode编码后所需要的字符空间长度
	lpUnicodeStr = new WCHAR[nRetLen + 1];  //为Unicode字符串空间
	nRetLen = ::MultiByteToWideChar(CP_UTF8, 0, lpUTF8Str, -1, lpUnicodeStr, nRetLen);  //转换到Unicode编码
	if (!nRetLen)  //转换失败则出错退出
		return 0;

	nRetLen = ::WideCharToMultiByte(CP_ACP, 0, lpUnicodeStr, -1, NULL, NULL, NULL, NULL);  //获取转换到GBK编码后所需要的字符空间长度

	if (!lpGBKStr)  //输出缓冲区为空则返回转换后需要的空间大小
	{
		if (lpUnicodeStr)
			delete[]lpUnicodeStr;
		return nRetLen;
	}

	if (nGBKStrLen < nRetLen)  //如果输出缓冲区长度不够则退出
	{
		if (lpUnicodeStr)
			delete[]lpUnicodeStr;
		return 0;
	}

	nRetLen = ::WideCharToMultiByte(CP_ACP, 0, lpUnicodeStr, -1, lpGBKStr, nRetLen, NULL, NULL);  //转换到GBK编码

	if (lpUnicodeStr)
		delete[]lpUnicodeStr;

	return nRetLen;
}

std::string Tools::UTF8ToGBKStr(char * lpUTF8Str)
{
	memset(m_bytes, 0, sizeof(m_bytes));
	UTF8ToGBK(lpUTF8Str, m_bytes, 4096);
	std::string ret = m_bytes;

	return ret;
}

//#include <QTextCodec>

//inline QString GBK2UTF8(const QString &inStr)
//{
//	QTextCodec *gbk = QTextCodec::codecForName("GB18030");
//	QTextCodec *utf8 = QTextCodec::codecForName("UTF-8");

//	QString g2u = gbk->toUnicode(gbk->fromUnicode(inStr));              // gbk  convert utf8  
//	return g2u;
//}

// 现在运行时是 UNICODE 编码
QString Tools::UNICODEStr2GBKStr(const QString &inStr)
{
	QTextCodec *gbk = QTextCodec::codecForName("GB18030");
	//QTextCodec *gbk = QTextCodec::codecForName("GBK");
	//QTextCodec *utf8 = QTextCodec::codecForName("UTF-8");

	//QString utf2gbk = gbk->toUnicode(inStr.toLocal8Bit());
	QByteArray byte = gbk->fromUnicode(inStr);	// 转换成 GBK 字节
	char * pGbkChar = byte.data();
	QString utf2gbk = byte;						// 转换成 QString
	return utf2gbk;
}

bool Tools::UNICODEStr2GBKChar(const QString &inStr, char* ret, int retlen)
{
	QTextCodec *gbk = QTextCodec::codecForName("GB18030");
	QByteArray gbkby = gbk->fromUnicode(inStr);
	char* gbkch = gbkby.data();
	//int len = strlen(gbkch);
	//if (len < retlen)
	//{
		strcpy(ret, gbkch);
	//}
	//else
	//{
	//	strncpy(ret, gbkch, retlen - 1);
	//}
	return true;
}

//inline std::string gbk2utf8(const QString &inStr)
//{
//	return GBK2UTF8(inStr).toStdString();
//}

//inline QString utf82gbk(const std::string &inStr)
//{
//	QString str = QString::fromStdString(inStr);

//	return UTF82GBK(str);
//}

QString Tools::GBKChar2UNICODEStr(const char* inChar)
{
	QTextCodec *gbk = QTextCodec::codecForName("GB18030");

	QString g2u = gbk->toUnicode(inChar);              // gbk  convert utf8  
	return g2u;
}

void Tools::convToAbsPath(std::string& srcPath)
{
	if (-1 == srcPath.find(':'))		// 如果没有检查到分隔符，就是相对目录
	{
		char buf[256];
		_getcwd(buf, sizeof(buf));
		srcPath = std::string(buf) + '/' + srcPath;
	}
}
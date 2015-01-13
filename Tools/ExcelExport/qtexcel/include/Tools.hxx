#ifndef _TOOLS_H
#define _TOOLS_H

#include "Singleton.hxx"
#include <QtCore/QtCore>
#include <cstring>
#include <string>	// include string
#include <QtWidgets/QPlainTextEdit>
#include <QtCore/QVector>

#include <stdio.h>
#include <windows.h>
#include "Platform.hxx"

class QWidget;

BEGIN_NAMESPACE

class Tools : public Singleton<Tools>
{
private:
	QWidget *m_parent;			// 提示对话框的父窗口 
	//QTextEdit* m_outTextEdit;	// 日志窗口    
	bool	m_running;			// 子线程是否在运行     
	QVector<QString> m_infoInList;		// 所有的信息都存放在这里
	QVector<QString> m_infoOutList;		// 所有的信息都存放在这里
	QMutex	m_infoListMutex;	// 信息类表同步

	QVector<QString> m_msgInList;		// 警告对话框信息
	QVector<QString> m_msgOutList;		// 警告对话框信息
	QMutex	m_msgListMutex;		// 警告对话框

public:
	Tools(); 
	~Tools();
	QString openFileDialog(QString dir, QString filter);
	std::string  GetFileNameExt(const char* pszFileName);
	QString openDirectoryDialog(QString path);
	void informationMessage(QString msg, QString caption = QObject::tr("QMessageBox::information()"), QWidget *parent = NULL);
	void informationMessageUI(QString msg, QString caption = QObject::tr("QMessageBox::information()"), QWidget *parent = NULL);

	void setParent(QWidget *parent);
	bool isRunning();
	void setRunning(bool run);
	//void setTextEdit(QTextEdit* textEdit);
	void Log(QString desc);
	QVector<QString>& getLog();

	void LogMsg(QString desc);
	QVector<QString>& getLogMsg();

	int GBKToUTF8(unsigned char * lpGBKStr, unsigned char * lpUTF8Str, int nUTF8StrLen);
	int UTF8ToGBK(unsigned char * lpUTF8Str, unsigned char * lpGBKStr, int nGBKStrLen);

	//QString GBK2UTF8(const QString &inStr);
	QString UNICODEStr2GBKStr(const QString &inStr);
	bool UNICODEStr2GBKChar(const QString &inStr, char* ret, int retlen);
	//std::string gbk2utf8(const QString &inStr);
	//QString utf82gbk(const std::string &inStr);
	QString GBKChar2UNICODEStr(const char* inChar);
	void convToAbsPath(std::string& srcPath);		// 将目录转换成绝对目录
};

END_NAMESPACE

#endif	// TOOL_H
﻿#ifndef _CAPPDATA_H
#define _CAPPDATA_H

#include "Singleton.hxx"
#include "CTask.hxx"
#include <string>

class QThread;
class ExcelExport;
class QComboBox;

#include "Platform.hxx"
BEGINNAMESPACE(NSExcelExport)

/**
 *@brief appdata
 */
class CAppData : public Singleton<CAppData>
{
protected:
	CTask m_task;			// main thread task, but only one thread
	ExcelExport* m_excelExport;
	QThread* m_pthread;

	string m_outPath;
	string m_xmlFile;
	string m_xmlSolution;

public:
	CAppData();
	~CAppData();

	CTask& getTask();
	void initData();
	ExcelExport* getExcelTbl();
	void startMultiPack();
	void startSinglePack();

	void setXml(string outpath, string xmlpath, string xmlsolution);
	bool isSetSolution();
	void initThread(QThread* pthread);
	void startThread();

	void initCombo(QComboBox *comboBoxSolution);
};

ENDNAMESPACE(NSExcelExport)

#endif
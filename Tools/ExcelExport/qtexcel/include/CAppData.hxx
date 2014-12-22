#ifndef CAPPDATA_H
#define CAPPDATA_H

#include "Singleton.hxx"
#include "CTask.hxx"
#include <string>

class QThread;
class ExcelExport;
class QComboBox;

/**
 *@brief appdata
 */
class CAppData : public Singleton<CAppData>
{
protected:
	CTask m_task;			// main thread task, but only one thread
	ExcelExport* m_excelTbl;
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

#endif
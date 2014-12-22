#ifndef _WORKTHREAD_H
#define _WORKTHREAD_H

#include <QtCore/QtCore>

//class ExcelTbl;

#include "Platform.hxx"
BEGINNAMESPACE(NSExcelExport)

class WorkThread : public QThread
{
private:
	//ExcelTbl* m_excelTbl;

public:
	//void setParam(ExcelTbl* para);
	virtual void run();
};

ENDNAMESPACE(NSExcelExport)

#endif	// WORKTHREAD_H  
#ifndef _WORKTHREAD_H
#define _WORKTHREAD_H

#include <QtCore/QtCore>

//class ExcelTbl;

class WorkThread : public QThread
{
private:
	//ExcelTbl* m_excelTbl;

public:
	//void setParam(ExcelTbl* para);
	virtual void run();
};

#endif	// WORKTHREAD_H  
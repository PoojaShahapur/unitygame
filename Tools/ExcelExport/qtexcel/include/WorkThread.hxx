#ifndef WORKTHREAD_H
#define WORKTHREAD_H

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
#include "WorkThread.hxx"
#include "ExcelTbl.hxx"
#include "Tools.hxx"
#include "CAppData.hxx"

void WorkThread::run()
{
	Tools::getSingletonPtr()->setRunning(true);
	if(CAppData::getSingletonPtr()->isSetSolution())
	{
		CAppData::getSingletonPtr()->startMultiPack();
	}
	else
	{
		//m_excelTbl->convExcel2Tbl();
		CAppData::getSingletonPtr()->startSinglePack();
	}
	Tools::getSingletonPtr()->setRunning(false);
}

//void WorkThread::setParam(ExcelTbl* para)
//{
//	m_excelTbl = para;
//}
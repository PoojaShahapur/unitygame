﻿#include "CAppData.hxx"
#include "ExcelExport.hxx"
#include "CTask.hxx"
#include <cstring>
#include <QtGui/QtGui>
#include <QtWidgets>
#include "Tools.hxx"

template<> CAppData* Singleton<CAppData>::msSingleton = 0;

CAppData::CAppData()
{

}

CAppData::~CAppData()
{

}

CTask& CAppData::getTask()
{
	return m_task;
}

void CAppData::initData()
{
	m_excelExport = new ExcelExport();
	m_task.readXML();
}

ExcelExport* CAppData::getExcelTbl()
{
	return m_excelExport;
}

// start Multi
void CAppData::startMultiPack()
{
	/*
	std::vector<CSolution*>::iterator ite;
	std::vector<CPackage*>::iterator itePack;
	for (ite = m_task.getSolutionLst().begin(); ite != m_task.getSolutionLst().end(); ++ite)
	{
		if(0 == strcmp((*ite)->getName().c_str(), m_xmlSolution.c_str()))
		{
			break;
		}
	}

	if(ite !=  m_task.getSolutionLst().end())
	{
		// package table
		for (itePack = (*ite)->getPackLst().begin(); itePack != (*ite)->getPackLst().end(); ++itePack)
		{
			m_excelExport->setXmlPath((*itePack)->getXml().c_str());
			m_excelExport->setOutputPath((*itePack)->getOutput().c_str());
			m_excelExport->exportExcel();
		}

		// execute external program
		if ((*ite)->getCmd().length())
		{
			QProcess::startDetached((*ite)->getCmd().c_str(), QStringList());
		}
	}
	*/

	std::vector<Table*>& tableList = CAppData::getSingletonPtr()->getTask().getTableList();

	std::vector<Table*>::iterator tableIteVecBegin = tableList.begin();
	std::vector<Table*>::iterator tableIteVecEnd = tableList.end();
	for (; tableIteVecBegin != tableIteVecEnd; ++tableIteVecBegin)
	{
		if ((*tableIteVecBegin)->m_bExportTable)
		{
			m_excelExport->exportExcelByTable((*tableIteVecBegin));
		}
	}
}

// start single
void CAppData::startSinglePack()
{
	m_excelExport->exportExcel();
}

void CAppData::setXml(string outpath, string xmlpath)
{
	m_outPath = outpath;
	m_xmlFile = xmlpath;

	m_excelExport->setXmlPath(m_xmlFile.c_str());
	m_excelExport->setOutputPath(m_outPath.c_str());
}

bool CAppData::isSetSolution()
{
	return (0 == m_outPath.length() && 0 == m_xmlFile.length());
}

void CAppData::initThread(QThread* pthread)
{
	m_pthread = pthread;
}

void CAppData::startThread()
{
	m_pthread->start();
}

void CAppData::initCombo(QComboBox *comboBoxSolution)
{
	QString tmp;
	std::vector<CSolution*>::iterator ite;
	for (ite = m_task.getSolutionLst().begin(); ite != m_task.getSolutionLst().end(); ++ite)
	{
		//comboBoxSolution->addItem(QString::fromLocal8Bit((ite->getName().c_str())));
		tmp = Tools::getSingletonPtr()->GBKChar2UNICODEStr((*ite)->getName().c_str());
		comboBoxSolution->addItem(tmp);
	}
}
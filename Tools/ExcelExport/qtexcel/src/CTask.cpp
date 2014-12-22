﻿#include "CTask.hxx"

CPackage::CPackage()
{

}

CPackage::~CPackage()
{

}

string CPackage::getXml()
{
	return m_xml;
}

string CPackage::getOutput()
{
	return m_output;
}

void CPackage::setXml(string xml)
{
	m_xml = xml;
}

void CPackage::setOutput(string output)
{
	m_output = output;
}

void CPackage::initByXml(TiXmlElement* elem)
{
	m_xml = elem->Attribute("xml");
	m_output = elem->Attribute("output");
}

void CPackage::destroy()
{

}



CSolution::CSolution()
{

}

CSolution::~CSolution()
{

}

string CSolution::getName()
{
	return m_name;
}

string CSolution::getCmd()
{
	return m_cmd;
}

void CSolution::setName(string name)
{
	m_name = name;
}

void CSolution::setCmd(string cmd)
{
	m_cmd = cmd;
}

vector<CPackage*>& CSolution::getPackLst()
{
	return m_lstPack;
}

void CSolution::initByXml(TiXmlElement* elem)
{
	TiXmlElement* packageXml = NULL;
	CPackage* ppackage;

	packageXml = elem->FirstChildElement("package");
	m_name = elem->Attribute("name");
	m_cmd = elem->Attribute("cmd");

	while(packageXml)
	{
		ppackage = new CPackage();
		m_lstPack.push_back(ppackage);
		ppackage->initByXml(packageXml);
		packageXml = packageXml->NextSiblingElement("package");
	}
}

void CSolution::destroy()
{

}


CTask::CTask()
{

}

CTask::~CTask()
{

}

vector<CSolution>& CTask::getSolutionLst()
{
	return m_lstSolution;
}

void CTask::destroy()
{

}

void CTask::readXML()
{
	TiXmlDocument doc;
	TiXmlElement* configXml = NULL;
	TiXmlElement* solutionXml = NULL;

	CSolution soluton;
	
	if(!doc.LoadFile("config.xml"))
	{
		throw "config.xml 加载失败!";
	}

	configXml = doc.FirstChildElement("config");
	if(!configXml)
	{
		throw "xml文件没有config标签";
	}

	solutionXml = configXml->FirstChildElement("solution");
	if(!solutionXml)
	{
		throw "xml文件没有solution标签";
	}

	while(solutionXml)
	{
		soluton.initByXml(solutionXml);
		m_lstSolution.push_back(soluton);
		solutionXml = solutionXml->NextSiblingElement("solution");
	}
}
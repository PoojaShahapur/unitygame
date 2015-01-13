#include "CTask.hxx"
#include "Tools.hxx"

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

void CPackage::initByXml(tinyxml2::XMLElement* elem)
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

void CSolution::initByXml(tinyxml2::XMLElement* elem)
{
	tinyxml2::XMLElement* packageXml = NULL;
	CPackage* ppackage;

	packageXml = elem->FirstChildElement("package");
	m_name = elem->Attribute("name");
	m_cmd = elem->Attribute("cmd");
	m_xmlRootPath = elem->Attribute("xmlrootpath");
	m_defaultOutput = elem->Attribute("defaultoutput");

	// 转换目录到绝对目录
	Tools::getSingletonPtr()->convToAbsPath(m_xmlRootPath);
	Tools::getSingletonPtr()->convToAbsPath(m_defaultOutput);

	while(packageXml)
	{
		ppackage = new CPackage();
		m_lstPack.push_back(ppackage);
		ppackage->initByXml(packageXml);

		// 如果没有配置输出
		//if (ppackage->getOutput() == nullptr || ppackage->getOutput().length == 0)
		if (ppackage->getOutput().length() == 0)
		{
			ppackage->setOutput(m_defaultOutput);
		}

		if (ppackage->getXml().find(':') == -1)	// 如果是相对目录
		{
			ppackage->setXml(m_xmlRootPath + "/" + ppackage->getXml());
		}

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

vector<CSolution*>& CTask::getSolutionLst()
{
	return m_lstSolution;
}

void CTask::destroy()
{

}

void CTask::readXML()
{
	tinyxml2::XMLDocument doc;
	tinyxml2::XMLElement* configXml = NULL;
	tinyxml2::XMLElement* solutionXml = NULL;

	CSolution* soluton;
	
	if (doc.LoadFile("config.xml") != tinyxml2::XML_SUCCESS)
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
		soluton = new CSolution();
		m_lstSolution.push_back(soluton);
		soluton->initByXml(solutionXml);
		solutionXml = solutionXml->NextSiblingElement("solution");
	}
}
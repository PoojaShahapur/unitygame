#include "CTask.hxx"
#include "Tools.hxx"
#include "tinyxml2.h"
#include <direct.h>		// chdir
#include <string.h>
#include <stdio.h>

XmlField::XmlField()
{
	m_fieldSize = -1;
	m_fieldBase = 10;	// 进制是什么
	m_defaultValue = "10";
}

Table::Table()
{
	m_bExportTable = false;
	m_bRecStructDef = true;
}

void Table::parseXML(tinyxml2::XMLElement* pXmlEmtFields)
{
	XmlField* fieldItem;
	tinyxml2::XMLElement* field = pXmlEmtFields->FirstChildElement("field");
	while (field)
	{
		fieldItem = new XmlField();
		m_fieldsList.push_back(fieldItem);

		fieldItem->m_fieldName = Tools::copyPChar2Str(field->Attribute("name"));
		fieldItem->m_fieldType = Tools::copyPChar2Str(field->Attribute("type"));

		// 如果 field 是 string 类型，size 配置长度包括结尾符 0 
		if (field->QueryIntAttribute("size", &fieldItem->m_fieldSize) != tinyxml2::XML_SUCCESS)
		{
			fieldItem->m_fieldSize = -1;
		}
		if (field->QueryIntAttribute("base", &fieldItem->m_fieldBase) != tinyxml2::XML_SUCCESS)
		{
			fieldItem->m_fieldBase = 10;
		}

		fieldItem->m_defaultValue = Tools::copyPChar2Str(field->Attribute("default"));
		// 默认的类型 
		if (0 == fieldItem->m_fieldType.length())
		{
			fieldItem->m_fieldType = "int";
		}
		field = field->NextSiblingElement("field");
	}
}

bool Table::buildTableDefine()
{
	m_strStructDef = "struct  ";
	m_strStructDef += m_lpszTableName;
	m_strStructDef += "{\r\n";

	int iFieldNum = 0;
	int iFieldIndex = 0;

	XmlField* field;
	char szMsg[256];

	while (iFieldIndex < m_fieldsList.size())
	{
		field = m_fieldsList[iFieldIndex];
		const char* fieldName = field->m_fieldName.c_str();
		const char* fieldType = field->m_fieldType.c_str();

		int fieldSize = -1;
		int fieldBase = 10;	// 进制是什么 
		const char* defaultValue = "10";

		fieldSize = field->m_fieldSize;
		fieldBase = field->m_fieldBase;

		defaultValue = field->m_defaultValue.c_str();
		// 默认的类型 
		if (fieldType == NULL)
		{
			fieldType = "int";
		}

		if (fieldName && fieldType)
		{
			if (stricmp(fieldType, "string") == 0)
			{
				memset(szMsg, 0, sizeof(szMsg));
				sprintf(szMsg, "\tchar\tstrField%d[%d];\t\t// %s\r\n", iFieldNum++, fieldSize, fieldName);
				m_strStructDef += szMsg;
			}
			else if (stricmp(fieldType, "int") == 0)
			{
				if (fieldSize == -1)
				{
					fieldSize = 4;
				}

				memset(szMsg, 0, sizeof(szMsg));

				switch (fieldSize)
				{
					case 1:
					{
						sprintf(szMsg, "\tBYTE\tbyField%d;\t\t// %s\r\n", iFieldNum++, fieldName);
					}
					break;
					case 2:
					{
						sprintf(szMsg, "\tWORD\twdField%d;\t\t// %s\r\n", iFieldNum++, fieldName);
					}
					break;
					case 4:
					{
						sprintf(szMsg, "\tDWORD\tdwField%d;\t\t// %s\r\n", iFieldNum++, fieldName);
					}
					break;
					case 8:
					{
						sprintf(szMsg, "\tQWORD\tqwField%d;\t\t// %s\r\n", iFieldNum++, fieldName);
					}
					break;
				}

				m_strStructDef += szMsg;
			}
			else if (stricmp(fieldType, "float") == 0)
			{
				if (fieldSize == -1)
				{
					fieldSize = 4;
				}

				memset(szMsg, 0, sizeof(szMsg));

				switch (fieldSize)
				{
				case 4:
				{
					sprintf(szMsg, "\tfloat\tfField%d;\t\t// %s\r\n", iFieldNum++, fieldName);
				}
				break;
				case 8:
				{
					sprintf(szMsg, "\tdouble\tdField%d;\t\t// %s\r\n", iFieldNum++, fieldName);
				}
				break;
				}

				m_strStructDef += szMsg;
			}
		}
		iFieldIndex++;
	}
	// 一次之后就不在输出结构了
	m_bRecStructDef = false;

	m_strStructDef += "};";

	return true;
}

// 是否是导出客户端表
bool Table::isExportClientTable()
{
	if (-1 != m_lpszTableName.find("client"))
	{
		return true;
	}

	return false;
}

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

bool CPackage::loadTableXml(std::vector<Table*>& tablesList)
{
	Table* tableItem;

	std::string::size_type iTmp;
	iTmp = m_xml.find_last_of('\\');
	if (iTmp == -1)
	{
		iTmp = m_xml.find_last_of('/');
	}

	try
	{
		tinyxml2::XMLDocument doc;
		tinyxml2::XMLElement* config = NULL;
		tinyxml2::XMLElement* table = NULL;

		if (doc.LoadFile(m_xml.c_str()) != tinyxml2::XML_SUCCESS)
		{
			throw "xml加载失败!";
		}
		config = doc.FirstChildElement("config");
		if (!config)
		{
			throw "xml文件没有config标签";
		}

		table = config->FirstChildElement("table");
		if (!table)
		{
			throw "xml文件没有table标签";
		}

		while (table)
		{
			tableItem = new Table();
			tablesList.push_back(tableItem);

			tableItem->m_strExcelDir = m_xml.substr(0, iTmp);
			if (_chdir(tableItem->m_strExcelDir.c_str()) == -1)			// 检查当前目录是否存在
			{
				QString msg = "当前目录设置正确";
				Tools::getSingletonPtr()->informationMessage(NULL, msg);
			}

			if (m_output.empty())
			{
				m_output = tableItem->m_strExcelDir;
			}

			tinyxml2::XMLElement* field;

			tableItem->m_lpszTableName = Tools::copyPChar2Str(table->Attribute("name"));
			tableItem->m_lpszExcelFile = Tools::copyPChar2Str(table->Attribute("ExcelFile"));
			tableItem->m_lpszDB = Tools::copyPChar2Str(table->Attribute("db"));
			tableItem->m_lpszDBTableName = Tools::copyPChar2Str(table->Attribute("tablename"));	// 表单的名字

			// 表中配置的 ID 范围
			tableItem->m_lpId = Tools::copyPChar2Str(table->Attribute("idrange"));
			if (tableItem->m_lpId.c_str())
			{
				tableItem->m_tableAttr.parseInRange(tableItem->m_lpId);
			}

			tableItem->m_lpszOutputFile = m_output + "/" + tableItem->m_lpszTableName;
			tableItem->m_strOutput += "//---------------------\r\n";
			tableItem->m_strOutput += "//";

			tableItem->m_strOutput += tableItem->m_lpszTableName;
			tableItem->m_strOutput += "\r\n";
			tableItem->m_strOutput += "//---------------------\r\n";
			tableItem->m_strStructDef = "";
			tableItem->m_strExcelDirAndName = tableItem->m_strExcelDir + "/" + tableItem->m_lpszExcelFile;
			if (stricmp("xls", Tools::getSingletonPtr()->GetFileNameExt(tableItem->m_lpszExcelFile.c_str()).c_str()) == 0)
			{
				tableItem->m_enExcelType = eXLS;
			}
			else if (stricmp("xlsx", Tools::getSingletonPtr()->GetFileNameExt(tableItem->m_lpszExcelFile.c_str()).c_str()) == 0)
			{
				tableItem->m_enExcelType = eXLSX;
			}
			else
			{
				QString tmpmsg = QStringLiteral("不能读取这个文件格式的表格, 文件 ");
				tmpmsg += tableItem->m_lpszExcelFile.c_str();
				Tools::getSingletonPtr()->informationMessage(tmpmsg);
			}

			field = table->FirstChildElement("fields");
			tableItem->parseXML(field);			// 读取表的属性
			tableItem->buildTableDefine();		// 生成表的定义

			tableItem->m_strOutput += tableItem->m_strStructDef.c_str();
			tableItem->m_strOutput += "\r\n";
			table = table->NextSiblingElement("table");
		}
	}
	catch (const char* p)
	{
		Tools::getSingletonPtr()->informationMessage(Tools::getSingletonPtr()->LocalChar2UNICODEStr(p));
		return false;
	}
	catch (...)
	{
		Tools::getSingletonPtr()->informationMessage(QStringLiteral("意外异常"));
		return false;
	}

	return true;
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

void CSolution::loadTableXml(std::vector<Table*>& tablesList)
{
	std::vector<CPackage*>::iterator packIteVecBegin;
	std::vector<CPackage*>::iterator packIteVecEnd;

	packIteVecBegin = m_lstPack.begin();
	packIteVecEnd = m_lstPack.end();

	for (; packIteVecBegin != packIteVecEnd; ++packIteVecBegin)
	{
		(*packIteVecBegin)->loadTableXml(tablesList);
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

CSolution* CTask::getSolution()
{
	return m_pSolution;
}

void CTask::destroy()
{

}

void CTask::readXML()
{
	tinyxml2::XMLDocument doc;
	tinyxml2::XMLElement* configXml = NULL;
	tinyxml2::XMLElement* solutionXml = NULL;
	
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

	if(solutionXml)
	{
		m_pSolution = new CSolution();
		//m_lstSolution.push_back(soluton);
		m_pSolution->initByXml(solutionXml);
		m_pSolution->loadTableXml(m_tablesList);
		//solutionXml = solutionXml->NextSiblingElement("solution");
	}
}

std::vector<Table*>& CTask::getTableList()
{
	return m_tablesList;
}
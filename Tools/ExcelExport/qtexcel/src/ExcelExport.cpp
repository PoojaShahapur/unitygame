#include "ExcelExport.hxx"
//#include <afx.h>	//CString的头文件 
#include "Tools.hxx"
//#include <QtGui/QtGui>
#include "Platform.hxx"
#include "DataItem.hxx"

#import "C:\Program Files\Common Files\System\ado\msado15.dll" \
	no_namespace rename("EOF","adoEOF") \
	rename("BOF","adoBOF")

ExcelExport::ExcelExport()
{

}

ExcelExport::~ExcelExport()
{

}

void ExcelExport::setXmlPath(QString file)
{
	mutex.lock();
	m_xmlPath = file;
	mutex.unlock();
}

void ExcelExport::setOutputPath(QString path)
{
	mutex.lock();
	m_tblPath = path;
	mutex.unlock();
}

QString ExcelExport::UTF82GBK(const QString &inStr)
{
	QTextCodec *gbk = QTextCodec::codecForName("GB18030");
	QTextCodec *utf8 = QTextCodec::codecForName("UTF-8");

	QString utf2gbk = gbk->toUnicode(inStr.toLocal8Bit());
	return utf2gbk;
}

bool ExcelExport::exportExcel()
{
	int iTmp = m_xmlPath.lastIndexOf('\\');
	if(iTmp == -1)
	{
		iTmp = m_xmlPath.lastIndexOf('/');
	}

	char filename[256];
	char outfilename[256];

	try
	{
		TiXmlDocument doc;
		TiXmlElement* config = NULL;
		TiXmlElement* table = NULL;

		memset(filename, 0, sizeof(filename));
		Tools::getSingletonPtr()->UNICODEStr2GBKChar(m_xmlPath, filename, sizeof(filename));
		if (!doc.LoadFile(filename))
		{
			throw "xml加载失败!";
		}
		config = doc.FirstChildElement("config");
		if(!config)
		{
			throw "xml文件没有config标签";
		}

		table = config->FirstChildElement("table");
		if(!table)
		{
			throw "xml文件没有table标签";
		}

		QString strExcelDir = m_xmlPath.left(iTmp);
		if(!QDir::setCurrent(strExcelDir))
		{
			QString msg = "当前目录设置正确";
			Tools::getSingletonPtr()->informationMessage(NULL, msg);
		}
		else
		{
			QString curPath = QDir::currentPath();
		}

		QString strExcelFile;
		QString strOutputFile;

		if(m_tblPath.isEmpty())
		{
			m_tblPath = strExcelDir;
		}

		m_strOutput = "";
		std::string strStructDef;
		while(table)
		{
			TiXmlElement* field = table->FirstChildElement("fields");
			const char* tableName = table->Attribute("name");
			const char* ExcelFile = table->Attribute("ExcelFile");
			const char* lpszsheetname = table->Attribute("sheetname");	// 表单的名字
			const char* lpszDB = table->Attribute("db");
			const char* lpszTable = table->Attribute("table");

			// 表中配置的 ID 范围
			const char* lpId = table->Attribute("idrange");
			if(lpId)
			{
				m_tableAttr.parseInRange(lpId);
			}

			char szMsg[256];
			memset(filename, 0, sizeof(filename));
			Tools::getSingletonPtr()->UNICODEStr2GBKChar(m_tblPath, filename, sizeof(filename));
			sprintf(szMsg, "%s\\%s.tbl", filename, tableName);
			strOutputFile = Tools::getSingletonPtr()->GBKChar2UNICODEStr(szMsg);
			m_strOutput += "//---------------------\r\n";
			m_strOutput += "//";

			m_strOutput += Tools::getSingletonPtr()->GBKChar2UNICODEStr(tableName);
			m_strOutput += "\r\n";
			m_strOutput += "//---------------------\r\n";
			strStructDef = "";

			strExcelFile = Tools::getSingletonPtr()->GBKChar2UNICODEStr(ExcelFile);
			if (stricmp("xls", Tools::getSingletonPtr()->GetFileNameExt(ExcelFile).c_str()) == 0)
			{
				memset(filename, 0, sizeof(filename));
				Tools::getSingletonPtr()->UNICODEStr2GBKChar(strExcelDir + QStringLiteral("/") + strExcelFile, filename, sizeof(filename));

				memset(outfilename, 0, sizeof(outfilename));
				Tools::getSingletonPtr()->UNICODEStr2GBKChar(strOutputFile, outfilename, sizeof(outfilename));

				exportExcel2PropertyVec(field, filename, lpszDB, lpszTable, outfilename, tableName, lpszsheetname, strStructDef, "Provider=Microsoft.Jet.OLEDB.4.0;", "Extended Properties=\'Excel 8.0;HDR=Yes;IMEX=1\';");
			}
			else if (stricmp("xlsx", Tools::getSingletonPtr()->GetFileNameExt(ExcelFile).c_str()) == 0)
			{
				memset(filename, 0, sizeof(filename));
				Tools::getSingletonPtr()->UNICODEStr2GBKChar(strExcelDir + QStringLiteral("/") + strExcelFile, filename, sizeof(filename));

				memset(outfilename, 0, sizeof(outfilename));
				Tools::getSingletonPtr()->UNICODEStr2GBKChar(strOutputFile, outfilename, sizeof(outfilename));

				exportExcel2PropertyVec(field, filename, lpszDB, lpszTable, outfilename, tableName, lpszsheetname, strStructDef, "Provider=Microsoft.ACE.OLEDB.12.0;", "Extended Properties=\'Excel 12.0 Xml;HDR=YES;IMEX=1\';");
			}
			else
			{
				QString tmpmsg = QStringLiteral("不能读取这个文件格式的表格, 文件 ");
				tmpmsg += strExcelFile;
				Tools::getSingletonPtr()->informationMessage(tmpmsg);
			}

			m_strOutput += strStructDef.c_str();
			m_strOutput += "\r\n";
			table = table->NextSiblingElement("table");
		}
	}
	catch (const char* p)
	{
		Tools::getSingletonPtr()->informationMessage(QString::fromLocal8Bit(p));
		return false;
	}
	catch(...)
	{
		Tools::getSingletonPtr()->informationMessage(QStringLiteral("意外异常"));
		return false;
	}

	return true;
}

/**
* com 接口
*/
bool ExcelExport::exportExcel2PropertyVec(
	TiXmlElement* pXmlEmtFields,	//第一个字段
	const char* lpszExcelFile,		//Excel文件的全路径（包括文件名称）
	const char* lpszDB,
	const char* lpszTable,
	const char* lpszOutputFile,		//tbl文件的全路径（包括文件名称）
	const char* lpszTableName,		//tbl文件名称本身
	const char* lpszsheetname,		//excel 中表单的名字   
	std::string& strStructDef,		// 最终结构体定义
	const char* provider,		// 数据引擎提供者  
	const char* extendedProperties		// 扩展属性    
	)
{
	// 前置检查    
	if (!lpszsheetname)
	{
		Tools::getSingletonPtr()->informationMessage(QStringLiteral("配置表中 sheetname 这个属性为空"));
		return false;
	}

	// 打开数据库
	char szMsg[512];
	std::string strCurField;
	// 添加一个指向Connection对象的指针
	_ConnectionPtr m_pConnection;
	// 添加一个指向Recordset 对象的指针
	_RecordsetPtr m_pRecordset;
	try
	{
		// com 接口初始化    
		// 第一个表的名字     
		_bstr_t _table_name;
		// 打开数据库的字符串 
		_bstr_t _strConnect;
		// 打开表的字符串   
		_bstr_t _bstrSQL;
		// 总的记录行数    
		int count = 0;	// 数据表中总的行数 

		::CoInitialize(NULL);
		_table_name = lpszsheetname;

		// 打开数据库    
		try
		{
			m_pConnection.CreateInstance("ADODB.Connection");
			_strConnect += provider;
			_strConnect += "Data Source=";
			_strConnect += lpszExcelFile;
			_strConnect += ";";
			_strConnect += extendedProperties;

			m_pConnection->Open(_strConnect, "", "", adModeUnknown);
		}
		catch (_com_error e)		//捕捉异常		
		{
			::CoUninitialize();
			Tools::getSingletonPtr()->informationMessage(QStringLiteral("打开数据库发生异常"));
			return false;
		}

		// 打开第一个表   
		try
		{
			m_pRecordset.CreateInstance(_uuidof(Recordset));

			_bstrSQL += "SELECT * FROM ";
			_bstrSQL += "[";
			_bstrSQL += _table_name;
			_bstrSQL += "$]";

			// 保证 GetRecordCount 返回正确的结果  
			m_pRecordset->CursorLocation = adUseClient;
			m_pRecordset->Open(_bstrSQL, m_pConnection.GetInterfacePtr(), adOpenDynamic, adLockOptimistic, adCmdText);

			// 获取记录集的数量 
			count = m_pRecordset->GetRecordCount();
		}
		catch (_com_error e)
		{
			m_pConnection->Close();
			::CoUninitialize();
			Tools::getSingletonPtr()->informationMessage(QStringLiteral("打开数据库的第一个表发生异常"));

			return false;
		}

		// 操作数据库
		//判读是否是客户端表
		bool m_isClient = false;
		if (strstr(lpszTableName, "client"))
		{
			m_isClient = true;
		}

		FILE* file;
		file = fopen(lpszOutputFile, "wb");
		fwrite(&count, sizeof(count), 1, file);

		// 这个作为一个中间值     
		std::string strTmp = "";

		std::vector<char> strValue;
		strStructDef = "struct  ";
		strStructDef += lpszTableName;
		strStructDef += "{\r\n";

		int iFieldNum = 0;
		bool bRecStructDef = true;

		// (1) 排序的向量，将所有的内容输入到要排序的向量列表中去        
		std::vector<DataItem*> _rowList;	// 行数据列表     
		DataItem* _rowData = NULL;			// 一行的数据   
		unsigned long int _id = 0;			// 一行唯一 ID 
		char* _strId = new char[64];		// 唯一 id 字符串   
		strcpy(_strId, "编号");				// 默认值

		int iRecord = -1;	// 在读取记录过程中，表示当前记录是第几行，zero-based
		int iCount = 0;		// 已经读取的数量   
		for (; m_pRecordset->adoEOF != -1; m_pRecordset->MoveNext())
		{
			// 申请一行的空间     
			iRecord++;
			_rowData = new DataItem();
			_rowList.push_back(_rowData);

			TiXmlElement* field = pXmlEmtFields->FirstChildElement("field");
			// id 段判断，第一个字段一定是 id 才行，否则会出现错误
			if (field)
			{
				const char* pid = field->Attribute("name");
				if (pid)
				{
					_variant_t idfield = m_pRecordset->GetCollect((_variant_t)pid);
					if (!m_tableAttr.bIdInRange(idfield.dblVal))
					{
						count--;
						continue;
					}

					strcpy(_strId, pid);	// 第一个字段是 id 段的名字
				}
			}
			int iFieldIndex = 0;
			while (field)
			{
				const char* fieldName = field->Attribute("name");
				const char* fieldType = field->Attribute("type");

				int fieldSize;
				int fieldBase = 10;	// 进制是什么 
				const char* defaultValue = "10";

				if (field->QueryIntAttribute("size", &fieldSize) != TIXML_SUCCESS)
				{
					fieldSize = 1;
				}
				if (field->QueryIntAttribute("base", &fieldBase) != TIXML_SUCCESS)
				{
					fieldBase = 10;
				}

				defaultValue = field->Attribute("default");
				// 默认的类型 
				if (fieldType == NULL)
				{
					fieldType = "int";
				}

				std::string strFieldDef;

				if (fieldName && fieldType)
				{
					// 读取字段值到 string 中
					strCurField = fieldName;
					_variant_t fieldValue = m_pRecordset->GetCollect((_variant_t)fieldName);
					if (fieldValue.vt == VT_BSTR)
					{
						strTmp = (TCHAR*)(_bstr_t)fieldValue.bstrVal;
					}
					else if (fieldValue.vt == VT_NULL || fieldValue.vt == VT_EMPTY)
					{
						strTmp = "";
						if (iFieldIndex == 0)
						{
							// 如果第一个值是空的就不处理这行，同时将数量减少，反正最后要写到头文件
							memset(szMsg, 0, sizeof(szMsg));
							sprintf(szMsg, "警告:第%d行的第一列(编号)没有值，这行就不做到tbl中!\r\n", iRecord);
							strStructDef += szMsg;
							count--;
							break;
						}
					}
					else if (fieldValue.vt == VT_I4)
					{
						memset(szMsg, 0, sizeof(szMsg));
						sprintf(szMsg, _T("%d"), fieldValue.lVal);
						strTmp = szMsg;
					}
					else if (fieldValue.vt == VT_R8)
					{
						memset(szMsg, 0, sizeof(szMsg));
						sprintf(szMsg, _T("%f"), fieldValue.dblVal);
						strTmp = szMsg;
					}
					else
					{
						strTmp = (TCHAR*)(_bstr_t)fieldValue.bstrVal;
						Tools::getSingletonPtr()->informationMessage(QStringLiteral("未能转换的字段"));
					}

					// 保存数据到 Property vector 
					if (strTmp == "" && defaultValue)
					{
						strTmp = defaultValue;
					}

					if (strTmp.length() > 1)
					{
						if (strTmp[0] == '\"')
						{
							// 去掉引号
							strTmp.erase(0, 1);
						}
						if (strTmp[strTmp.length() - 1] == '\"')
						{
							strTmp.erase(strTmp.length() - 1, 1);
						}
					}
					if (stricmp(fieldType, "string") == 0)
					{
						WCHAR wBuf[2048] = { 0 };
						char bytes[4096] = { 0 };
						int len = 0;

						// 如果字符串是  "" 空就不转换了 
						if (!strTmp.empty())
						{
							const char* strSor = (const char*)strTmp.c_str();
							len = MultiByteToWideChar(CP_ACP, MB_PRECOMPOSED, strSor, strTmp.length(), wBuf, 2048);
							if (len == 0)
							{
								throw "从ANSI转换到UNICODE失败";
							}
							len = WideCharToMultiByte(CP_UTF8, 0, wBuf, len, bytes, 4096, NULL, NULL);
							if (len == 0)
							{
								throw "从UNICODE转换到UTF-8失败";
							}
						}

						if (fieldSize == -1)
						{
							fieldSize = 16;
						}

						strValue.resize(fieldSize, 0);

						if (len + 1 > fieldSize)
						{
							memset(szMsg, 0, sizeof(szMsg));
							sprintf(szMsg, "警告:字段超出定义大小，大小 %u 字段，字段名: %s!\r\n", strTmp.length(), strCurField);
							strStructDef += szMsg;
							len = fieldSize - 1;
						}

						if (m_isClient == true)
						{
							unsigned short stLen = (unsigned short)len;
							_rowData->getByteBuffer().writeUnsignedInt16(stLen);
							_rowData->getByteBuffer().writeMultiByte(bytes, stLen);
						}
						else
						{
							memcpy(&strValue[0], bytes, len);
							strValue[len] = 0;

							_rowData->getByteBuffer().writeMultiByte(&strValue[0], fieldSize);
						}

						if (bRecStructDef)
						{
							memset(szMsg, 0, sizeof(szMsg));
							sprintf(szMsg, "\tchar\tstrField%d[%d];\t\t// %s\r\n", iFieldNum++, fieldSize, fieldName);
							strStructDef += szMsg;
						}
					}
					else if (stricmp(fieldType, "int") == 0)
					{
						if (fieldSize == -1)
						{
							fieldSize = 4;
						}

						__int64 nValue;
						if (strTmp == "")
						{
							nValue = 0;
						}
						else
						{
							nValue = _strtoi64(strTmp.c_str(), NULL, fieldBase);
						}

						memset(szMsg, 0, sizeof(szMsg));

						switch (fieldSize)
						{
						case 1:
						{
							char value = nValue;
							_rowData->getByteBuffer().writeInt8(value);

							if (bRecStructDef)
							{
								sprintf(szMsg, "\tBYTE\tbyField%d;\t\t// %s\r\n", iFieldNum++, fieldName);
							}
						}
						break;
						case 2:
						{
							short value = nValue;
							_rowData->getByteBuffer().writeInt16(value);

							if (bRecStructDef)
							{
								sprintf(szMsg, "\tWORD\twdField%d;\t\t// %s\r\n", iFieldNum++, fieldName);
							}
						}
						break;
						case 4:
						{
							long value = nValue;
							_rowData->getByteBuffer().writeInt32(value);

							if (bRecStructDef)
							{
								sprintf(szMsg, "\tDWORD\tdwField%d;\t\t// %s\r\n", iFieldNum++, fieldName);
							}
							// TODO: 如果是 id 字段  
							if (strncmp(fieldName, _strId, strlen(_strId)) == 0)
							{
								_id = value;
							}
						}
						break;
						case 8:
						{
							__int64 value = nValue;
							_rowData->getByteBuffer().writeInt64(value);

							if (bRecStructDef)
							{
								sprintf(szMsg, "\tQWORD\tqwField%d;\t\t// %s\r\n", iFieldNum++, fieldName);
							}
						}
						break;
						}

						if (bRecStructDef) strStructDef += szMsg;
					}
					else if (stricmp(fieldType, "float") == 0)
					{
						if (fieldSize == -1)
						{
							fieldSize = 4;
						}

						double dValue = strtod(strTmp.c_str(), NULL);
						memset(szMsg, 0, sizeof(szMsg));

						switch (fieldSize)
						{
						case 4:
						{
							float fValue = dValue;
							_rowData->getByteBuffer().writeFloat(fValue);

							if (bRecStructDef)
							{
								sprintf(szMsg, "\tfloat\tfField%d;\t\t// %s\r\n", iFieldNum++, fieldName);
							}
						}
						break;
						case 8:
						{
							double fValue = dValue;
							_rowData->getByteBuffer().writeDouble(fValue);

							if (bRecStructDef)
							{
								sprintf(szMsg, "\tdouble\tdField%d;\t\t// %s\r\n", iFieldNum++, fieldName);
							}
						}
						break;
						}

						if (bRecStructDef) strStructDef += szMsg;
					}

					// (4) 处理 id 字段   
					if (strncmp(fieldName, _strId, strlen(_strId)) == 0)
					{
						_rowData->setID(_id);
					}
				}
				field = field->NextSiblingElement("field");
				iFieldIndex++;
			}
			// 一次之后就不在输出结构了
			bRecStructDef = false;
			iCount++;
		}

		// (2) 排序向量列表   
		lessCmp m_cmpFunc;
		std::sort(_rowList.begin(), _rowList.end(), m_cmpFunc);

		// (3) 将排序的向量列表写到文件中去    
		std::vector<DataItem*>::iterator iteVecDataItem;
		std::vector<DataItem*>::iterator iteVecEndDataItem;
		iteVecDataItem = _rowList.begin();
		iteVecEndDataItem = _rowList.end();
		for (; iteVecDataItem != iteVecEndDataItem; ++iteVecDataItem)
		{
			(*iteVecDataItem)->writeFile(file);
			delete (*iteVecDataItem);
		}
		_rowList.clear();

		// 关闭打开句柄    
		// TODO: 关闭打开的内容 
		m_pRecordset->Close();
		m_pConnection->Close();
		::CoUninitialize();

		strStructDef += "};";
		fseek(file, 0, SEEK_SET);
		fwrite(&count, sizeof(count), 1, file);
		fclose(file);

		memset(szMsg, 0, sizeof(szMsg));
		sprintf(szMsg, "//导出 %s 成功, 共 %u 条记录\r\n", lpszTableName, count);
		strStructDef += szMsg;
		// 打表成功 
		Tools::getSingletonPtr()->Log(Tools::getSingletonPtr()->GBKChar2UNICODEStr(strStructDef.c_str()));

		return true;
	}
	catch (_com_error e)
	{
		m_pRecordset->Close();
		m_pConnection->Close();
		::CoUninitialize();

		LPCSTR szError = e.Description();
		if (!strCurField.empty())
		{
			memset(szMsg, 0, sizeof(szMsg));
			sprintf(szMsg, "%s,字段: %s", szError, strCurField);
			Tools::getSingletonPtr()->informationMessage(QString::fromLocal8Bit(szMsg));
		}
		else
		{
			Tools::getSingletonPtr()->informationMessage(QString::fromLocal8Bit(szError));
		}
		return false;
	}
	catch (std::bad_alloc& error)	// 分配内存失败   
	{
		m_pRecordset->Close();
		m_pConnection->Close();
		::CoUninitialize();
		Tools::getSingletonPtr()->informationMessage(QString::fromLocal8Bit(error.what()));
	}
}

void ExcelExport::exportPropertyVec2File(std::vector<DataItem*>& rowList)
{

}
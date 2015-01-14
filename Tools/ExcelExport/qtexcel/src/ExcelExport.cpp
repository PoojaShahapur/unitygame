#include "ExcelExport.hxx"
//#include <afx.h>	//CString的头文件 
#include "Tools.hxx"
//#include <QtGui/QtGui>
#include "Platform.hxx"
#include "DataItem.hxx"
#include <direct.h>		// chdir
#include "CTask.hxx"
#include "ADOWrap.hxx"

ExcelExport::ExcelExport()
{
	m_wBuf = new WCHAR[2048];
	m_bytes = new char[4096];
}

ExcelExport::~ExcelExport()
{
	delete []m_wBuf;
	delete []m_bytes;
}

void ExcelExport::setXmlPath(std::string file)
{
	mutex.lock();
	m_xmlPath = file;
	mutex.unlock();
}

void ExcelExport::setOutputPath(std::string path)
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
	CPackage* packItem = new CPackage();
	packItem->setXml(m_xmlPath);
	packItem->setOutput(m_tblPath);

	std::vector<Table*> tablesList;
	packItem->loadTableXml(tablesList);

	std::vector<Table*>::iterator tableBeginIte;
	std::vector<Table*>::iterator tableEndIte;
	tableBeginIte = tablesList.begin();
	tableEndIte = tablesList.end();

	for (; tableBeginIte != tableEndIte; ++tableBeginIte)
	{
		(*tableBeginIte)->buildTableDefine();		// 生成表的定义
		exportExcelInternal(*tableBeginIte);		// 导出表
	}

	return true;
}

/**
* com 接口
*/
bool ExcelExport::exportExcelInternal(Table* tableItem)
{
	// 前置检查    
	if (0 == tableItem->m_lpszDBTableName.length())
	{
		Tools::getSingletonPtr()->informationMessage(QStringLiteral("配置表中 tablename 这个属性为空"));
		return false;
	}
	if (tableItem->m_fieldsList.size() == 0)
	{
		Tools::getSingletonPtr()->informationMessage(QStringLiteral("配置表中需要导出的字段为空，没有需要导出的"));
		return false;
	}

	// 打开数据库
	char szMsg[512];
	ADOWrap adoWrap;

	// 这个作为一个中间值     
	std::string strTmp = "";

	int iFieldNum = 0;				// 相当于列号
	int iRecord = -1;				// 在读取记录过程中，表示当前记录是第几行，zero-based，相当于行号
	string warnOrErrorDesc;			// 一些警告或者错误

	// (1) 排序的向量，将所有的内容输入到要排序的向量列表中去        
	std::vector<DataItem*> _rowList;	// 行数据列表     
	DataItem* _rowData = NULL;			// 一行的数据   
	unsigned long int _id = 0;			// 一行唯一 ID 
	std::string _strId;					// 唯一 id 字符串   
	_strId = "编号";						// 默认值
	int iFieldIndex = 0;				// 当前导出的字段索引

	const char* fieldName;
	const char* fieldType;
	int fieldSize = -1;
	int fieldBase = 10;	// 十进制、十六进制
	const char* defaultValue = "10";

	try
	{
		// 操作数据库
		adoWrap.opemDB(tableItem);

		XmlField* field = tableItem->m_fieldsList[0];
		_strId = field->m_fieldName;			// 第一个字段必然是 id

		while (!adoWrap.isAdoEOF())		// 如果没有结束
		{
			// 申请一行的空间     
			iRecord++;		// 记录当前访问的表中的行数
			_rowData = new DataItem();
			_rowList.push_back(_rowData);

			// id 段判断，第一个字段一定是 id 才行，否则会出现错误
			if (!tableItem->m_tableAttr.bIdInRange(adoWrap.getCollectUInt(_strId)))
			{
				adoWrap.m_count--;
				continue;
			}
			
			while (iFieldIndex < tableItem->m_fieldsList.size())
			{
				field = tableItem->m_fieldsList[iFieldIndex];
				fieldName = field->m_fieldName.c_str();
				fieldType = field->m_fieldType.c_str();

				// 如果 field 是 string 类型，size 配置长度包括结尾符 0 
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
					// 读取 Excel 字段值到 string 中， Excel 中只读取字符串，4个字节数字，8个字节数字
					_variant_t fieldValue = adoWrap.getCollect(fieldName);
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
							warnOrErrorDesc += szMsg;
							adoWrap.m_count--;
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

					// 转化 Excel 中的数据到配置表中的数据
					if (strTmp.length() > 1)
					{
						if (strTmp[0] == '\"')
						{
							strTmp.erase(0, 1);		// 去掉引号
						}
						if (strTmp[strTmp.length() - 1] == '\"')
						{
							strTmp.erase(strTmp.length() - 1, 1);
						}
					}
					if (stricmp(fieldType, "string") == 0)
					{
						int len = 0;

						// 如果字符串是  "" 空就不转换了 
						if (!strTmp.empty())
						{
							const char* strSor = (const char*)strTmp.c_str();
							len = MultiByteToWideChar(CP_ACP, MB_PRECOMPOSED, strSor, (int)strTmp.length(), m_wBuf, 2048);
							if (len == 0)
							{
								throw "从ANSI转换到UNICODE失败";
							}
							len = WideCharToMultiByte(CP_UTF8, 0, m_wBuf, len, m_bytes, 4096, NULL, NULL);
							if (len == 0)
							{
								throw "从UNICODE转换到UTF-8失败";
							}
						}

						if (fieldSize == -1)
						{
							fieldSize = 16;
						}

						// 判断长度
						if (len + 1 > fieldSize)		// 最后一个字节填充 '\0'
						{
							memset(szMsg, 0, sizeof(szMsg));
							sprintf(szMsg, "警告:字段超出定义大小，大小 %u 字段，字段名: %s!\r\n", strTmp.length(), fieldName);
							warnOrErrorDesc += szMsg;
							len = fieldSize - 1;		// 只能放 fieldSize - 1 个，最后一个写入 '\0'，长度不包括最后一个 '\0'
						}
						m_bytes[len] = 0;		// 设置结尾符号， m_bytes 这个缓冲不清零的

						// bytes 可能很长，但是传入后，构造的时候使用 '\0' 截断
						addPropertyStr(_rowData, m_bytes, fieldSize);
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
							addProperty(_rowData, value);
						}
						break;
						case 2:
						{
							short value = nValue;
							addProperty(_rowData, value);
						}
						break;
						case 4:
						{
							int value = nValue;
							// TODO: 如果是 id 字段  
							if (strncmp(fieldName, _strId.c_str(), strlen(_strId.c_str())) == 0)
							{
								_id = value;
								addProperty(_rowData, value, true);
							}
							else
							{
								addProperty(_rowData, value);
							}
						}
						break;
						case 8:
						{
							__int64 value = nValue;
							addProperty(_rowData, value);
						}
						break;
						}
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
							float fValue = strtof(strTmp.c_str(), NULL);;
							addProperty(_rowData, fValue);
						}
						break;
						case 8:
						{
							double dValue = strtod(strTmp.c_str(), NULL);;
							addProperty(_rowData, dValue);
						}
						break;
						}
					}

					// (4) 处理 id 字段
					if (strncmp(fieldName, _strId.c_str(), strlen(_strId.c_str())) == 0)
					{
						_rowData->setID(_id);
					}
				}
				iFieldIndex++;
			}

			adoWrap.moveNext();			// 移动到下一条数据
		}

		// 导出 Excel 到文件
		exportPropertyVec2File(tableItem->m_lpszOutputFile.c_str(), _rowList, tableItem->isExportClientTable());

		memset(szMsg, 0, sizeof(szMsg));
		sprintf(szMsg, "//导出 %s 成功, 共 %u 条记录\r\n", tableItem->m_lpszTableName, adoWrap.m_count);
		warnOrErrorDesc += szMsg;
		// 打表成功 
		Tools::getSingletonPtr()->Log(Tools::getSingletonPtr()->GBKChar2UNICODEStr(warnOrErrorDesc.c_str()));

		return true;
	}
	catch (_com_error e)
	{
		LPCSTR szError = e.Description();
		if (fieldName != nullptr)
		{
			memset(szMsg, 0, sizeof(szMsg));
			sprintf(szMsg, "%s,字段: %s", szError, fieldName);
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
		Tools::getSingletonPtr()->informationMessage(QString::fromLocal8Bit(error.what()));
	}
}

void ExcelExport::exportPropertyVec2File(const char* lpszOutputFile, std::vector<DataItem*>& _rowList, bool isClient)
{
	size_t count = 0;	// 数据表中总的行数 
	count = _rowList.size();
	ByteBuffer byteBuffer;
	// (1) 写入总的行数
	byteBuffer.writeUnsignedInt32((uint32)count);

	// (2) 排序向量列表   
	lessCmp m_cmpFunc;
	std::sort(_rowList.begin(), _rowList.end(), m_cmpFunc);

	// (3) 将排序的向量列表写到文件中去    
	if (isClient)
	{
		exportPropertyVec2FileClient(_rowList, byteBuffer);
	}
	else
	{
		exportPropertyVec2FileServer(_rowList, byteBuffer);
	}

	_rowList.clear();

	FILE* file;
	file = fopen(lpszOutputFile, "wb");
	//fwrite(&count, sizeof(count), 1, file);
	byteBuffer.writeFile(file);

	fclose(file);
}

/**
*@brief |4个字节总共项数量|第一个数据|第二个数据|
*/
void ExcelExport::exportPropertyVec2FileServer(std::vector<DataItem*>& _rowList, ByteBuffer& byteBuffer)
{
	// 直接写入文件
	std::vector<DataItem*>::iterator iteVecDataItem;
	std::vector<DataItem*>::iterator iteVecEndDataItem;
	iteVecDataItem = _rowList.begin();
	iteVecEndDataItem = _rowList.end();
	for (; iteVecDataItem != iteVecEndDataItem; ++iteVecDataItem)
	{
		(*iteVecDataItem)->writeFileServer(byteBuffer);
		delete (*iteVecDataItem);
	}
}

void ExcelExport::exportPropertyVec2FileClient(std::vector<DataItem*>& _rowList, ByteBuffer& byteBuffer)
{
	exportPropertyVec2FileMobile(_rowList, byteBuffer);
}

/**
 *@brief |4个字节总共项数量|第一个数据项ID|第一个数据项距离文件头部偏移|第二个数据项ID|第二个数据距离文件头部偏移| ... |第一个数据(这个数据项是没有 ID 内容的， ID 已经在头字段写进去了)|第二个数据|
 */
void ExcelExport::exportPropertyVec2FileMobile(std::vector<DataItem*>& _rowList, ByteBuffer& byteBuffer)
{
	size_t dataOffset = 4;		// 偏移 4 个头字节
	size_t count = 0;	// 数据表中总的行数 
	unsigned int m_id;
	count = _rowList.size();

	dataOffset += 2 * count * sizeof(int);		// 每一个占用 4 个字节

	// 移动优化
	// (1) 写入每一个项在文件中或者字节数组中的偏移量，就是之前的字节数
	std::vector<DataItem*>::iterator iteVecDataItem;
	std::vector<DataItem*>::iterator iteVecEndDataItem;
	iteVecDataItem = _rowList.begin();
	iteVecEndDataItem = _rowList.end();
	for (; iteVecDataItem != iteVecEndDataItem; ++iteVecDataItem)
	{
		// 全部写到 ByteBuffer 
		(*iteVecDataItem)->writeByteBuffer(true);
		m_id = (*iteVecDataItem)->getID();
		//fwrite(&m_id, sizeof(int), 1, file);		// 写入 ID ，注意是四个 sizeof(dataOffset) == 4
		//fwrite(&dataOffset, sizeof(int), 1, file);		// 写入这一项内容在文件中的偏移，注意是四个 sizeof(dataOffset) == 4
		byteBuffer.writeUnsignedInt32(m_id);
		byteBuffer.writeUnsignedInt32((uint32)dataOffset);

		dataOffset += (*iteVecDataItem)->getByteBuffer().size();
	}

	// (2) 写入每一项的数据
	iteVecDataItem = _rowList.begin();
	iteVecEndDataItem = _rowList.end();
	for (; iteVecDataItem != iteVecEndDataItem; ++iteVecDataItem)
	{
		(*iteVecDataItem)->writeFileMobile(byteBuffer);
		delete (*iteVecDataItem);
	}
}

void ExcelExport::deleteFieldsList(std::vector<XmlField*>& fieldsList)
{
	std::vector<XmlField*>::iterator fieldIteVecBegin = fieldsList.begin();
	std::vector<XmlField*>::iterator fieldIteVecEnd = fieldsList.end();
	for (; fieldIteVecBegin != fieldIteVecEnd; ++fieldIteVecBegin)
	{
		delete (*fieldIteVecBegin);
	}
}
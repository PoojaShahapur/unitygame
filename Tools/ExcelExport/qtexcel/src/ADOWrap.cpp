#include "ADOWrap.hxx"
#include "CTask.hxx"
#include "Tools.hxx"

ADOWrap::ADOWrap()
{
	m_count = 0;
}

ADOWrap::~ADOWrap()
{
	closeDB();
}

bool ADOWrap::opemDB(Table* tableItem)
{
	const char* provider;					// 数据引擎提供者
	const char* extendedProperties;			// 扩展属性

	if (eXLS == tableItem->m_enExcelType)
	{
		provider = XLS_ENGINE_provider;
		extendedProperties = XLS_ENGINE_extendedProperties;
	}
	else
	{
		provider = XLSX_ENGINE_provider;
		extendedProperties = XLSX_ENGINE_extendedProperties;
	}

	// com 接口初始化    
	// 第一个表的名字     
	_bstr_t _table_name;
	// 打开数据库的字符串 
	_bstr_t _strConnect;
	// 打开表的字符串   
	_bstr_t _bstrSQL;

	::CoInitialize(NULL);
	_table_name = tableItem->m_lpszDBTableName.c_str();

	// 打开数据库    
	try
	{
		m_pConnection.CreateInstance("ADODB.Connection");
		_strConnect += provider;
		_strConnect += "Data Source=";
		_strConnect += tableItem->m_strExcelDirAndName.c_str();
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
		m_count = m_pRecordset->GetRecordCount();
	}
	catch (_com_error e)
	{
		m_pConnection->Close();
		::CoUninitialize();
		Tools::getSingletonPtr()->informationMessage(QStringLiteral("打开数据库的第一个表发生异常"));

		return false;
	}

	return true;
}

bool ADOWrap::isAdoEOF()
{
	return (m_pRecordset->adoEOF == -1);
}

void ADOWrap::moveNext()
{
	m_pRecordset->MoveNext();
}

unsigned int ADOWrap::getCollectUInt(std::string fieldName)
{
	return m_pRecordset->GetCollect((_variant_t)(fieldName.c_str())).uintVal;
}

_variant_t ADOWrap::getCollect(const char* fieldName)
{
	return m_pRecordset->GetCollect((_variant_t)fieldName);
}

void ADOWrap::closeDB()
{
	m_pRecordset->Close();
	m_pConnection->Close();
	::CoUninitialize();
}
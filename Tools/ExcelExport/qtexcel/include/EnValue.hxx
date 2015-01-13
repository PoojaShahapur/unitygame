#ifndef _ENVALUE_H
#define _ENVALUE_H

#include "Platform.hxx"
BEGIN_NAMESPACE

enum EnExcelType
{
	eXLS,
	eXLSX
};

#define XLS_ENGINE "Provider=Microsoft.Jet.OLEDB.4.0;", "Extended Properties=\'Excel 8.0;HDR=Yes;IMEX=1\';"
#define XLSX_ENGINE "Provider=Microsoft.ACE.OLEDB.12.0;", "Extended Properties=\'Excel 12.0 Xml;HDR=YES;IMEX=1\';"

END_NAMESPACE

#endif	// _ENVALUE_H
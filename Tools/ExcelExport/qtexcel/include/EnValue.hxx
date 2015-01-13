#ifndef _ENVALUE_H
#define _ENVALUE_H

#include "Platform.hxx"
BEGIN_NAMESPACE

enum EnExcelType
{
	eXLS,
	eXLSX
};

#define XLS_ENGINE_provider "Provider=Microsoft.Jet.OLEDB.4.0;"
#define XLS_ENGINE_extendedProperties "Extended Properties=\'Excel 8.0;HDR=Yes;IMEX=1\';"
#define XLSX_ENGINE_provider "Provider=Microsoft.ACE.OLEDB.12.0;"
#define XLSX_ENGINE_extendedProperties "Extended Properties=\'Excel 12.0 Xml;HDR=YES;IMEX=1\';"

END_NAMESPACE

#endif	// _ENVALUE_H
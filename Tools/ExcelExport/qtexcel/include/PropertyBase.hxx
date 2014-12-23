#ifndef _PROPERTYBASE_H
#define _PROPERTYBASE_H

#include "ByteBuffer.hxx"
#include "Platform.hxx"

BEGINNAMESPACE(NSExcelExport)

class PropertyBase
{
protected:
	// �����ͨ�����л�
	virtual void srz2BU(ByteBuffer& byteBuffer) = 0;

public:
	// ���л�������
	virtual void srz2BUServer(ByteBuffer& byteBuffer) = 0;
	// ���л�����ͻ���
	virtual void srz2BUDesktop(ByteBuffer& byteBuffer) = 0;
	// ���л� web
	virtual void srz2BUWeb(ByteBuffer& byteBuffer) = 0;
	// ���л��ƶ��豸
	virtual void srz2BUMobile(ByteBuffer& byteBuffer) = 0;
};

ENDNAMESPACE(NSExcelExport)

#endif	// _PROPERTYBASE_H  
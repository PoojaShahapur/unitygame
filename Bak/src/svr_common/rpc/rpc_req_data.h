#ifndef SVR_COMMON_RPC_RPC_REQ_DATA_H
#define SVR_COMMON_RPC_RPC_REQ_DATA_H

#include <string>
#include <functional>

#include "rpc_data.h"  // for CRpcData

// 异步Rpc数据
class CRpcReqData : public CRpcData
{
public:
	CRpcReqData(const std::string& sService, const std::string& sMethod,
		const std::string& sMessage) :
		CRpcData(sMessage),
		m_sService(sService),
		m_sMethod(sMethod)
	{};
	CRpcReqData(const std::string& sService, const std::string& sMethod,
		const std::string& sMessage, const Callback& cb) :
		CRpcData(sMessage),
		m_sService(sService),
		m_sMethod(sMethod),
		m_callback(cb)
	{};

	virtual ~CRpcReqData() {}

public:
	const std::string& GetService() const { return m_sService; }
	const std::string& GetMethod() const { return m_sMethod; }

	void SetCallback(const Callback& callback) { m_callback = callback; }

public:
	void SerializeToString(uint32_t uReqId,
		std::string& sOutput) const override;
	Callback GetCallback() const override { return m_callback; }

private:
	std::string m_sService;
	std::string m_sMethod;
	Callback m_callback;
};

#endif  // SVR_COMMON_RPC_RPC_REQ_DATA_H

#ifndef CELL_CLUSTER_DETAIL_FUNCTION_SVR_SELECTOR_H
#define CELL_CLUSTER_DETAIL_FUNCTION_SVR_SELECTOR_H

#include <map>
#include <set>
#include <string>

// 功能服选择器。服务器ID散列，功能名散列，取散列值相近的服务器。
// 整个集群功能服的选择是一致的。
// Todo: 排除临时关启的服务器。
class CFunctionSvrSelector
{
public:
	CFunctionSvrSelector();
	virtual ~CFunctionSvrSelector();

public:
	// 功能服选择。
	uint16_t GetSvrId(const std::string& sFunction) const;
	uint16_t GetRandSvrId() const;

	void AddSvr(uint16_t uSvrId);
	void EraseSvr(uint16_t uSvrId);

private:
	uint16_t GetSvrId(size_t uHash) const;

private:
	// 同一散列值可能有多个服，只取第1个。
	using SvrIdSet = std::set<uint16_t>;
	using Hash2SvrId = std::map<size_t, SvrIdSet>;
	Hash2SvrId m_hash2SvrId;
};  // CFunctionSvrSelector

#endif  // CELL_CLUSTER_DETAIL_FUNCTION_SVR_SELECTOR_H

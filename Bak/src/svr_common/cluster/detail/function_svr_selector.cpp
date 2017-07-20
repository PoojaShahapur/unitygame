#include "function_svr_selector.h"

#include "log.h"

#include <cassert>

const char LOG_NAME[] = "CFunctionSvrSelector";

namespace {
// 因为gcc的hash<>实现仅用于stl容器，不能平均分布，所以从VS2015复制部分hash实现。

// FUNCTION _Hash_seq from VS2015
inline size_t _Hash_seq(const unsigned char *_First, size_t _Count)
{	// FNV-1a hash function for bytes in [_First, _First + _Count)
#if !defined(_WIN32) || defined(_WIN64)  // Linux 总是64位
	static_assert(sizeof(size_t) == 8, "This code is for 64-bit size_t.");
	const size_t _FNV_offset_basis = 14695981039346656037ULL;
	const size_t _FNV_prime = 1099511628211ULL;
#else  // Win32
	static_assert(sizeof(size_t) == 4, "This code is for 32-bit size_t.");
	const size_t _FNV_offset_basis = 2166136261U;
	const size_t _FNV_prime = 16777619U;
 #endif /* defined(_WIN64) */

	size_t _Val = _FNV_offset_basis;
	for (size_t _Next = 0; _Next < _Count; ++_Next)
		{	// fold in another byte
		_Val ^= (size_t)_First[_Next];
		_Val *= _FNV_prime;
		}
	return (_Val);
}

size_t HashUint16(uint16_t n)
{
	// 因为 FNV-1a hash 算法对于相近的值结果也相近，所以扩大后再hash.
	uint16_t a[4] = {n,n,n,n};
	static_assert(sizeof(a) == 8, "Should be the whold array.");
	return _Hash_seq((const unsigned char *)&a, sizeof(a));
};

// hash functor for string
size_t HashString(const std::string& s)
{
	// 因为 FNV-1a hash 算法对于较短的或仅尾部几个字符不同的串，结果相近，所以先扩充。
	std::string sLong = s + "rstuvwxyz";
	return _Hash_seq((const unsigned char *)sLong.data(), sLong.size());
}

}  // namespace

CFunctionSvrSelector::CFunctionSvrSelector()
{
}

CFunctionSvrSelector::~CFunctionSvrSelector()
{
}

uint16_t CFunctionSvrSelector::GetSvrId(const std::string& sFunction) const
{
	assert(!m_hash2SvrId.empty());  // 总是存在自身
	size_t uFunHash = HashString(sFunction);
	return GetSvrId(uFunHash);
}

uint16_t CFunctionSvrSelector::GetSvrId(size_t uHash) const
{
	assert(!m_hash2SvrId.empty());  // 总是存在自身
	auto itr = m_hash2SvrId.lower_bound(uHash);
	if (itr == m_hash2SvrId.end())
		itr = m_hash2SvrId.begin();
	const SvrIdSet& ids = (*itr).second;
	assert(!ids.empty());  // 空了会删除
	return *(ids.begin());
}

uint16_t CFunctionSvrSelector::GetRandSvrId() const
{
	size_t uHash = HashUint16(rand());
	return GetSvrId(uHash);
}

void CFunctionSvrSelector::AddSvr(uint16_t uSvrId)
{
	LOG_DEBUG("AddSvr: " << uSvrId);
	size_t uHash = HashUint16(uSvrId);
	m_hash2SvrId[uHash].insert(uSvrId);
}

void CFunctionSvrSelector::EraseSvr(uint16_t uSvrId)
{
	LOG_DEBUG("EraseSvr: " << uSvrId);
	size_t uHash = HashUint16(uSvrId);
	auto itr = m_hash2SvrId.find(uHash);
	if (itr == m_hash2SvrId.end())
		return;

	SvrIdSet& rIds = (*itr).second;
	rIds.erase(uSvrId);
	if (rIds.empty())
		m_hash2SvrId.erase(itr);
}


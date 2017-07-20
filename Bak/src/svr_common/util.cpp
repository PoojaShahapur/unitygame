#include "util.h"

#include "app.h"  // for CApp
#include "asio/asio_session_in.h"  // for CAsioSessionIn
#include "asio/asio_session_in_mgr.h"  // for CAsioSessionInMgr
#include "cluster/cluster.h"  // for CCluster
#include "log.h"

#include <chrono>
#include <stdio.h>  // for snprint()
#include <stdlib.h>  // for setenv()

#include <openssl/rsa.h>
#include <openssl/pem.h>  
#include <openssl/err.h> 
#include <openssl/md5.h>
#include <boost/archive/iterators/binary_from_base64.hpp>  
#include <boost/archive/iterators/transform_width.hpp> 

#include <iostream>  
#include <string>  
#include <sstream> 
#ifdef _WIN32
    #include <ctime>
#endif

namespace {

CAsioServer4C& GetSvr4C()
{
	return CApp::get_const_instance().GetSvr4C();
}

CAsioServer4S& GetSvr4S()
{
	return CApp::get_const_instance().GetSvr4S();
}

}  // namespace

namespace Util {

// 不能使用GetMySvrId(), 只能传参，因为World还没初始化。
bool SetServerIdEnv(uint16_t uServerId)
{
	const char LOG_NAME[] = "SetServerIdEnv";
	static char buf[128] = {0};  // putenv need a buffer
	int nLen = snprintf(buf, sizeof(buf), "SERVER_ID=%u", uServerId);
	if (nLen < 0)
	{
		LOG_ERROR(Fmt("snprintf() failed. (%1%)%2%") % errno % strerror(errno));
		return false;
	}

	int nErr = putenv(buf);
	if (0 == nErr) return true;
	LOG_ERROR(Fmt("putenv() failed. (%1%)%2%") % errno % strerror(errno));
	return false;
}

lua_State* GetLuaState()
{
	return CApp::get_mutable_instance().GetLuaState();
}

namespace detail {
uint16_t GetMySvrId()
{
	return CApp::get_const_instance().GetMySvrId();
}
}  // namespace detail

uint16_t GetRandSvrId()
{
	return CCluster::get_const_instance().GetRandSvrId();
}

CRedis& GetRedis()
{
	return CApp::get_mutable_instance().GetRedis();
}

// 从首次调用开始计时的毫秒值
uint64_t GetMs()
{
	using namespace std::chrono;
	static auto s_start = steady_clock::now();
	auto now = steady_clock::now();
	auto duration = duration_cast<milliseconds>(now - s_start);
	return duration.count();
}

// 获取系统当前的毫秒数
uint64_t getSystemMs()
{
#ifdef _WIN32
    return time(nullptr) * 1000L;
#else
    struct timespec ts;
    clock_gettime(CLOCK_REALTIME, &ts);
    return (ts.tv_sec * 1000 + ts.tv_nsec / 1000000);
#endif
}

// 生成道具的唯一id
uint64_t genObjectID()
{
    // 上次生成唯一id的时间戳
    static uint64_t lastGenTime = getSystemMs();
    uint64_t now = getSystemMs();
    static uint64_t curMsindex = 0;
    uint16_t svrid = GetMySvrId();
    if (lastGenTime != now)
    {
        lastGenTime = now;
        curMsindex = 0;
    }

    uint64_t uniqueid = (now << 25 | (uint32_t)svrid << 9 | curMsindex++);
    const char LOG_NAME[] = "genobj";
	LOG_WARN(Fmt("generate uid=%lu") % uniqueid);
    return uniqueid;
}

std::string Base64Decode(const std::string & inPut)
{
	using boost::archive::iterators::transform_width;
	using boost::archive::iterators::binary_from_base64;
	typedef transform_width<binary_from_base64<std::string::const_iterator>, 8, 6> Base64DecodeIter;

	std::stringstream outPut;
	try
	{
		copy(Base64DecodeIter(inPut.begin()),
			Base64DecodeIter(inPut.end()),
			std::ostream_iterator<char>(outPut));
	}
	catch (...)
	{
		return "";
	}

	std::string result = outPut.str();
	size_t resultSize = result.size();
	if (resultSize > 0)
	{ 
		if (inPut.back() == '=') {
			size_t blankNum = 0;
			if (inPut[inPut.size() - 2] == '=')
				blankNum = 2;
			else
				blankNum = 1;
			result.resize(resultSize - blankNum);
		}
	}

	return result;
}

bool VerifyGiantLogin(const std::string& entites_str, const std::string& sign)
{
	const char* giant_public_key = 
		"-----BEGIN PUBLIC KEY-----\r\n"
		"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAvJvcZRBqyT0tZyyQlPQA\r\n"
		"/50o6+npHr51ZeDmq5lb8m2TMb844S1Tu6IUmg8L7vdjrb1nFT10SZtXHli8LNVG\r\n"
		"D4Qc/BEkRGR+NVFzh8IqN6BMqmXclesoCQpxuDNicw9w9W8TMfYruk3ELb8QqjoK\r\n"
		"H3Iq8Z6TGtfmM7K27WIw5OiPQepe0msX4klIjtW8JHGKDVkkLfUK66bxrFwOqO4V\r\n"
		"ebdcalR1m/Dt1FFa4ZBoS/b7MVPyKF2NHZVPU9mKXXnj8WzrKJ0bq0YoUh3MLoK8\r\n"
		"ZRZrUJYhAiy2EYZX0DUoTxXpAS77UESoDdzbjSlvm52V+RYVClOl59recTiDztKK\r\n"
		"VwIDAQAB\r\n"
		"-----END PUBLIC KEY-----\r\n";

	bool result = false;
	BIO *p_key_bio = BIO_new_mem_buf((void *)giant_public_key, -1);
	RSA *p_rsa = PEM_read_bio_RSA_PUBKEY(p_key_bio, NULL, NULL, NULL);

	if (p_rsa != NULL) {
		const char *cstr = entites_str.c_str();
		unsigned char hash[SHA_DIGEST_LENGTH] = { 0 };
		SHA1((unsigned char *)cstr, strlen(cstr), hash);
		std::string sign_cstr = Base64Decode(sign);
		unsigned int sign_len = sign_cstr.size();
		int r = RSA_verify(NID_sha1, hash, SHA_DIGEST_LENGTH, (const unsigned char*)sign_cstr.c_str(), sign_len, p_rsa);

		if (r > 0) {
			result = true;
		}
	}
	else {

		const char LOG_NAME[] = "VerifyGiantLogin";
		char errBuf[1024] = { 0 };
		ERR_load_crypto_strings();
		ERR_error_string_n(ERR_get_error(), errBuf, sizeof(errBuf));
		LOG_ERROR(Fmt("load public key failed: [%1]") % errBuf);
	}

	RSA_free(p_rsa);
	BIO_free(p_key_bio);
	return result;
}

std::string md5(const std::string& data)
{
	unsigned char md[16] = { 0 };
	MD5((unsigned char*)data.c_str(), data.size(), md);

	std::stringstream outstream;
	for (int i = 0; i < 16; i++)
		outstream << std::hex << std::setw(2) << std::setfill('0') << (int)md[i];
	return outstream.str();
}

uint16_t GetFunctionSvrId(const std::string& sFunction)
{
	return CCluster::get_const_instance().GetSvrId(sFunction);
}

bool IsValidSvrId(uint16_t uSvrId)
{
	return CCluster::get_const_instance().IsValidSvrId(uSvrId);
}

std::string GenerateRandToken(bool isBinary)
{
	unsigned char szRandBuff[32] = "GenerateRandToken_Test";
	*((uint64_t*)&szRandBuff[22]) = GetMs();
	*((uint16_t*)&szRandBuff[30]) = (uint16_t)rand();

	unsigned char md[16] = { 0 };
	MD5(szRandBuff, sizeof(szRandBuff), md);
	
	if (isBinary)
	{
		return std::string((char*)md, sizeof(md));
	}
	else
	{
		std::stringstream outstream;
		for (int i = 0; i < 16; i++)
			outstream << std::hex << std::setw(2) << std::setfill('0') << (int)md[i];
		return outstream.str();
	}
}

void Encrypt_XOR(char* szData, size_t uSize, const char* szKey, size_t keySize)
{
	for (int i = 0; i < uSize; i++)
	{
		szData[i] ^= szKey[i%keySize];
	}
}

void DisconnectGameClient(uint64_t uCltId)
{
	CAsioSessionIn* pSession = CAsioSessionInMgr::get_const_instance()
		.GetSessionIn(uCltId);
	if (!pSession) return;
	// pSession可能是服务器会话，不能断
	if (pSession->IsGameClient())
		pSession->Disconnect();
}

}  // namespace Util



#ifndef _zSocket_h_
#define _zSocket_h_

#include <errno.h>
#include <unistd.h>
#include <cstdlib>
#include <arpa/inet.h>
#include <sys/types.h>
#include <string.h>
#include <netinet/tcp.h>
#include <fcntl.h>
#include <pthread.h>
#include <sys/socket.h>
#include <zlib.h>
#include <net/if.h>
#include <sys/ioctl.h>
#include <assert.h>
#include <iostream>
#include <queue>
#include <ext/pool_allocator.h>
#include <ext/mt_allocator.h>
#include <sys/poll.h>
#include <sys/epoll.h>
#include <vector>

#include "zNoncopyable.h"
#include "zMutex.h"
#include "zNullCmd.h"
#include "Zebra.h"
#include "zTime.h"
#include "EncDec/EncDec.h"
#include "lzma920/MLzma.h"

const DWORD trunkSize = 64 * 1024;
#define unzip_size(zip_size) ((zip_size) * 120 / 100 + 12)
const DWORD PACKET_ZIP_BUFFER  =  unzip_size(trunkSize - 1) + sizeof(DWORD) + 8;  /**< 压缩需要的缓冲 */
#define CHECK_CURR_PTR(size) if(((unsigned int)-1-_currPtr) <= (size)) \
							       Zebra::logger->fatal("靠%s, %u ,%u ,%u",__PRETTY_FUNCTION__, _currPtr, (size), _maxSize) \


/**
* 字节缓冲，用于套接口接收和发送数据的缓冲
* \param _type 缓冲区数据类型
*/
template <typename _type>
class ByteBuffer
{

public:
	
	/**
	* 构造函数
	*/
	ByteBuffer();

	/**
	* 向缓冲填入数据
	* \param buf 待填入缓冲的数据
	* \param size 待填入缓冲数据的长度
	*/
	inline void put(const BYTE *buf,const DWORD size)
	{
		//首先确认缓冲内存是否足够
		wr_reserve(size);

		bcopy(buf,&_buffer[_currPtr],size);
		CHECK_CURR_PTR(size);
		_currPtr += size;
	}

	/**
	* 得到当前可写bf的未知
	* 保证在调用此函数写入数据之前需要调用wr_reserve(size)来预留缓冲区大小
	* \return 可写入缓冲开始地址
	*/
	inline BYTE *wr_buf()
	{
		return &_buffer[_currPtr];
	}

	/**
	* 返回缓冲中有效数据的开始地址
	* \return 有效数据地址
	*/
	inline BYTE *rd_buf()
	{
		return &_buffer[_offPtr];
	}

	/**
	* 判断缓冲中时候有有效数据
	* \return 返回缓冲中是否有有效数据
	*/
	inline bool rd_ready()
	{
		bool ret = _currPtr > _offPtr;
		return ret;
	}

	/**
	* 得到缓冲中有效数据的大小
	* \return 返回缓冲中有效数据大小
	*/
	inline DWORD rd_size()
	{
		DWORD ret = _currPtr - _offPtr;
		return ret;
	}

	/**
	* 当缓冲的有效数据被使用以后，需要对缓冲进行整理
	* \param size 最后一次使用的有效数据长度
	*/
	inline void rd_flip(DWORD size)
	{	
		_offPtr += size;
		if (_currPtr > _offPtr)
		{
			DWORD tmp = _currPtr - _offPtr;
			if (_offPtr >= tmp)
			{
				memmove(&_buffer[0],&_buffer[_offPtr],tmp);
				_offPtr = 0;
				_currPtr = tmp;
			}
		}
		else
		{
			_offPtr = 0;
			_currPtr = 0;
		}
	}

	/**
	* 得到缓冲可写入数据的大小
	* \return 可写入数据的大小
	*/
	inline DWORD wr_size()
	{
		DWORD ret = _maxSize - _currPtr;
		return ret;
	}

	/**
	* 实际向缓冲写入了数据，需要对缓冲进行整理
	* \param size 实际写入的数据
	*/
	inline void wr_flip(const DWORD size)
	{
	    CHECK_CURR_PTR(size);
		_currPtr += size;
	}

	/**
	* 重值缓冲中的数据，清空无用的垃圾数据
	*/
	inline void reset()
	{
		_offPtr = 0;
		_currPtr = 0;
	}

	/**
	* 返回缓冲最大大小
	* \return 缓冲最大大小
	*/
	inline DWORD maxSize() const
	{
		return _maxSize;
	}
	
	inline DWORD offPtr() const 
	{
	    return _offPtr;
	}

	inline DWORD currPtr() const 
	{
	    return _currPtr;
	}
	/**
	* 对缓冲的内存进行重新整理，向缓冲写数据，如果缓冲大小不足，重新调整缓冲大小，
	* 大小调整原则按照trunkSize的整数倍进行增加
	* \param size 向缓冲写入了多少数据
	*/
	inline void wr_reserve(const DWORD size);

private:

	DWORD _maxSize;
	DWORD _offPtr;
	DWORD _currPtr;
	_type _buffer;

};


/**
* 动态内存的缓冲区，可以动态扩展缓冲区大小
*/
typedef ByteBuffer<std::vector<unsigned char> > t_BufferCmdQueue;

/**
* 模板偏特化
* 对缓冲的内存进行重新整理，向缓冲写数据，如果缓冲大小不足，重新调整缓冲大小，
* 大小调整原则按照trunkSize的整数倍进行增加
* \param size 向缓冲写入了多少数据
*/
template <>
inline void t_BufferCmdQueue::wr_reserve(const DWORD size)
{
    if(_maxSize < _currPtr)
    {
	Zebra::logger->trace("[靠靠]%s,%u,%u,%u",__PRETTY_FUNCTION__,__LINE__,_currPtr,_maxSize);
    }
    if (wr_size() < (size+8))
    {
#define trunkCount(size) (((size) + trunkSize - 1) / trunkSize)
	unsigned int midd = (trunkSize * trunkCount(size+8));
	if(((unsigned int)-1-_maxSize) <= midd)
	    Zebra::logger->fatal("[靠靠]t_BufferCmdQueue::wr_reserve");
	_maxSize += midd;
	_buffer.resize(_maxSize);
    }
}


/**
* 静态大小的缓冲区，以栈空间数组的方式来分配内存，用于一些临时变量的获取
*/
typedef ByteBuffer<BYTE [PACKET_ZIP_BUFFER]> t_StackCmdQueue;

/**
* 模板偏特化
* 对缓冲的内存进行重新整理，向缓冲写数据，如果缓冲大小不足，重新调整缓冲大小，
* 大小调整原则按照trunkSize的整数倍进行增加
* \param size 向缓冲写入了多少数据
*/
template <>
inline void t_StackCmdQueue::wr_reserve(const DWORD size)
{
	/*
	if (wr_size() < size)
	{
	//不能动态扩展内存
	assert(false);
	}
	// */
}


/**
* \brief 变长指令的封装，固定大小的缓冲空间
* 在栈空间分配缓冲内存
* \param cmd_type 指令类型
* \param size 缓冲大小
*/
template <typename cmd_type,DWORD size = 64 * 1024>
class CmdBuffer_wrapper
{

public:

	typedef cmd_type type;
	DWORD cmd_size;
	DWORD max_size;
	type *cnt;

	CmdBuffer_wrapper() : cmd_size(sizeof(type)),max_size(size)// : cnt(NULL)
	{
		cnt = (type *)buffer;
		constructInPlace(cnt);
	}

private:

	BYTE buffer[size];

};

/**
* \brief 封装套接口底层函数，提供一个比较通用的接口
*/
class zSocket : private zNoncopyable
{

public:
#if 0
	bool				m_bUseIocp;     // [ranqd]  是否使用IOCP收发数据   

	DWORD               m_SendSize;   // [ranqd] 记录希望发送数据总长度
	DWORD               m_LastSend;   // [ranqd] 记录单次请求发送数据长度
	DWORD               m_LastSended; // [ranqd] 已发送所请求数据长度
#endif

	static const int T_RD_MSEC          =  2100;          /**< 读取超时的毫秒数 */
	static const int T_WR_MSEC          =  5100;          /**< 发送超时的毫秒数 */

	static const DWORD PH_LEN       =  sizeof(DWORD);  /**< 数据包包头大小 */
	static const DWORD PACKET_ZIP_MIN  =  32;            /**< 数据包压缩最小大小 */

	static const DWORD PACKET_ZIP    =  0x40000000;        /**< 数据包压缩标志 */
	static const DWORD INCOMPLETE_READ  =  0x00000001;        /**< 上次对套接口进行读取操作没有读取完全的标志 */
	static const DWORD INCOMPLETE_WRITE  =  0x00000002;        /**< 上次对套接口进行写入操作煤油写入完毕的标志 */

	static const DWORD PACKET_MASK      =  trunkSize - 1;  /**< 最大数据包长度掩码 */
	static const DWORD MAX_DATABUFFERSIZE  =  PACKET_MASK;            /**< 数据包最大长度，包括包头4字节 */
	static const DWORD MAX_DATASIZE      =  (MAX_DATABUFFERSIZE - PH_LEN);    /**< 数据包最大长度 */
	static const DWORD MAX_USERDATASIZE    =  (MAX_DATASIZE - 128);        /**< 用户数据包最大长度 */

	static const char *getIPByIfName(const char *ifName);

	zSocket(const int	sock,const struct sockaddr_in *addr = NULL,const bool compress = false);
	~zSocket();

	int recvToCmd(void *pstrCmd,const int nCmdLen,const bool wait);
	bool sendCmd(const void *pstrCmd,const int nCmdLen,const bool buffer = false);
	bool sendCmdNoPack(const void *pstrCmd,const int nCmdLen,const bool buffer = false);
	//int  Send(const int	sock, const void* pBuffer, const int nLen,int flags);
	bool sync();
	void force_sync();

	int checkIOForRead();
	int checkIOForWrite();
	int recvToBuf_NoPoll();
	int recvToCmd_NoPoll(void *pstrCmd,const int nCmdLen);

	/**
	* \brief 获取套接口对方的地址
	* \return IP地址
	*/
	inline const char *getIP() const { return inet_ntoa(addr.sin_addr); }
	inline const DWORD getAddr() const { return addr.sin_addr.s_addr; }

	/**
	* \brief 获取套接口对方端口
	* \return 端口
	*/
	inline const WORD getPort() const { return ntohs(addr.sin_port); }

	/**
	* \brief 获取套接口本地的地址
	* \return IP地址
	*/
	inline const char *getLocalIP() const { return inet_ntoa(local_addr.sin_addr); }

	/**
	* \brief 获取套接口本地端口
	* \return 端口
	*/
	inline const WORD getLocalPort() const { return ntohs(local_addr.sin_port); }

	/**
	* \brief 设置读取超时
	* \param msec 超时，单位毫秒 
	* \return 
	*/
	inline void setReadTimeout(const int msec) { rd_msec = msec; }

	/**
	* \brief 设置写入超时
	* \param msec 超时，单位毫秒 
	* \return 
	*/
	inline void setWriteTimeout(const int msec) { wr_msec = msec; }

	inline void addEpoll(int kdpfd, __uint32_t events, void *ptr)
	{
		struct epoll_event ev;
		ev.events = events;
		ev.data.ptr = ptr;
		if(-1 == epoll_ctl(kdpfd, EPOLL_CTL_ADD, sock, &ev))
		{
			char _buf[100];
			bzero(_buf, sizeof(_buf));
			strerror_r(errno, _buf, sizeof(_buf));
		}
	}
	
	inline void delEpoll(int kdpfd, __uint32_t events)
	{
		struct epoll_event ev;
		ev.events = events;
		ev.data.ptr = NULL;
		if(-1 == epoll_ctl(kdpfd, EPOLL_CTL_DEL, sock, &ev))
		{
			char _buf[100];
			bzero(_buf, sizeof(_buf));
			strerror_r(errno, _buf, sizeof(_buf));
		}
	}
	/**
	* \brief 填充pollfd结构
	* \param pfd 待填充的结构
	* \param events 等待的事件参数
	*/
	inline void fillPollFD(struct pollfd &pfd,short events)
	{
		pfd.fd = sock;
		pfd.events = events;
		pfd.revents = 0;
	}
	

	inline void setEncMethod(CEncrypt::encMethod m) { enc.setEncMethod(m); }
	inline void set_key_rc5(const BYTE *data,int nLen,int rounds) { enc.set_key_rc5(data,nLen,rounds); }
	inline void set_key_des(const_DES_cblock *key) { enc.set_key_des(key); }
	inline void setEncMask(const DWORD m){enc.setEncMask(m);}
	inline void setDecMask(const DWORD m){enc.setDecMask(m);}
	inline DWORD snd_queue_size() { return _snd_queue.rd_size() + _enc_queue.rd_size(); }

	inline DWORD getBufferSize() const {return _rcv_queue.maxSize() + _snd_queue.maxSize();}

	
	
private:
	int sock;                  /**< 套接口 */
	struct sockaddr_in addr;          /**< 套接口地址 */
	struct sockaddr_in local_addr;        /**< 套接口地址 */
	int rd_msec;                /**< 读取超时，毫秒 */
	int wr_msec;                /**< 写入超时，毫秒 */

			
	t_BufferCmdQueue _rcv_queue;        /**< 接收缓冲指令队列 */
	DWORD _rcv_raw_size;          /**< 接收缓冲解密数据大小 */
	t_BufferCmdQueue _snd_queue;        /**< 加密缓冲指令队列 */
	t_BufferCmdQueue _enc_queue;        /**< 加密缓冲指令队列 */
	DWORD _current_cmd;
	zMutex mutex;                /**< 锁 */

	zTime last_check_time;            /**< 最后一次检测时间 */

	DWORD bitmask;            /**< 标志掩码 */
	CEncrypt enc;                /**< 加密方式 */

	inline void set_flag(DWORD _f) { bitmask |= _f; }
	inline bool isset_flag(DWORD _f) const { return bitmask & _f; }
	inline void clear_flag(DWORD _f) { bitmask &= ~_f; }
	inline bool need_enc() const { return CEncrypt::ENCDEC_NONE!=enc.getEncMethod(); }
	/**
	* \brief 返回数据包包头最小长度
	* \return 最小长度
	*/
	inline DWORD packetMinSize() const { return PH_LEN; }

	/**
	* \brief 返回整个数据包的长度
	* \param in 数据包
	* \return 返回整个数据包的长度
	*/
	inline DWORD packetSize(const BYTE *in) const { return PH_LEN + ((*((DWORD *)in)) & PACKET_MASK); }

	inline int sendRawData(const void *pBuffer,const int nSize);
	inline int sendRawData_NoPoll(const void *pBuffer,const int nSize);
	inline bool setNonblock();
	inline int waitForRead();
	inline int waitForWrite();
	inline int recvToBuf();

	inline DWORD packetUnpack(BYTE *in,const DWORD nPacketLen,BYTE *out);
	template<typename buffer_type>
	inline DWORD packetAppend(const void *pData,const DWORD nLen,buffer_type &cmd_queue);
	template<typename buffer_type>
	inline DWORD packetAppendNoEnc(const void *pData,const DWORD nLen,buffer_type &cmd_queue);
	template<typename buffer_type>
	inline DWORD packetPackEnc(buffer_type &cmd_queue,const DWORD current_cmd,DWORD offset = 0);
public:
	template<typename buffer_type>
	static inline DWORD packetPackZip(const void *pData,const DWORD nLen,buffer_type &cmd_queue,const bool _compress = true);
	
	inline bool sendRawDataIM(const void *pBuffer,const int nSize);

	DWORD debug_currPtr;
	DWORD debug_offPtr;
	DWORD debug_max_size;
	DWORD debug_wait_size;

};


/**
* \brief 对数据进行组织,需要时压缩,不加密
* \param pData 待组织的数据，输入
* \param nLen 待拆包的数据长度，输入
* \param cmd_queue 输出，存放数据
* \return 封包后的大小
*/
template<typename buffer_type>
inline DWORD zSocket::packetAppendNoEnc(const void *pData,const DWORD nLen,buffer_type &cmd_queue)
{
    //Zebra::logger->debug("输入长度1： %d", nLen);
    int nSize = packetPackZip(pData,nLen,cmd_queue,PACKET_ZIP == (bitmask & PACKET_ZIP));
    //Zebra::logger->debug("封包长度1： %d", nSize);	

    return nSize;

}

/**
* \brief 对数据进行组织,需要时压缩和加密
* \param pData 待组织的数据，输入
* \param nLen 待拆包的数据长度，输入
* \param cmd_queue 输出，存放数据
* \return 封包后的大小
*/
template<typename buffer_type>
inline DWORD zSocket::packetAppend(const void *pData,const DWORD nLen,buffer_type &cmd_queue)
{
	DWORD nSize = packetPackZip( pData,nLen,cmd_queue,PACKET_ZIP == (bitmask & PACKET_ZIP));
	if (need_enc())
		nSize = packetPackEnc(cmd_queue, cmd_queue.rd_size());
	return nSize;
}

/**
* \brief         对数据进行加密
* \param cmd_queue    待加密的数据，输入输出
* \param current_cmd  最后一个指令长度
* \param offset    待加密数据的偏移
* \return         返回加密以后真实数据的大小
*/
template<typename buffer_type>
inline DWORD zSocket::packetPackEnc(buffer_type &cmd_queue,const DWORD current_cmd,DWORD offset)
{
	DWORD mod = (cmd_queue.rd_size() - offset) % 8;
	if (0!=mod)
	{
		mod = 8 - mod;
		// [ranqd] 这样似乎更合理
		//(*(DWORD *)(&(cmd_queue.rd_buf()[cmd_queue.rd_size() - current_cmd - PACKLASTSIZE]))) += mod;
		(*(DWORD *)(&(cmd_queue.rd_buf()[cmd_queue.rd_size() - current_cmd]))) += mod;
		cmd_queue.wr_flip(mod);
	}

	//加密动作
	enc.encdec(&cmd_queue.rd_buf()[offset],cmd_queue.rd_size() - offset,true);

	return cmd_queue.rd_size();
}


/**
* \brief       对数据进行压缩,由上层判断是否需要加密,这里只负责加密不作判断
* \param pData   待压缩的数据，输入
* \param nLen     待压缩的数据长度，输入
* \param pBuffer   输出，存放压缩以后的数据
* \param _compress  当数据包过大时候是否压缩
* \return       返回加密以后真实数据的大小
*/
template<typename buffer_type>
inline DWORD zSocket::packetPackZip(const void *pData,const DWORD nLen,buffer_type &cmd_queue,const bool _compress)
{
    /*if (nLen > MAX_DATASIZE)
      {
      Cmd::t_NullCmd *cmd = (Cmd::t_NullCmd *)pData;
      Zebra::logger->warn("zSocket::packetPackZip: 发送的数据包过大(cmd = %u,para = %u",cmd->cmd,cmd->para);
      }*/
    DWORD nSize = nLen > MAX_DATASIZE ? MAX_DATASIZE : nLen;//nLen & PACKET_MASK;
    DWORD nMask = 0;//nLen & (~PACKET_MASK);
    if (nSize > PACKET_ZIP_MIN /*数据包过大*/ 
	    && _compress /*带压缩标记，数据包需要压缩*/
	    /*&& !(nMask & PACKET_ZIP)*/ /*数据包过大可能已经是压缩过的*/ )
    {
#ifndef _LZMA
	uLong nZipLen = unzip_size(nSize);
	cmd_queue.wr_reserve(nZipLen + PH_LEN);
	int retcode = compress(&(cmd_queue.wr_buf()[PH_LEN]),&nZipLen,(const Bytef *)pData,nSize);
	switch(retcode)
	{
	    case Z_OK:
		break;
	    case Z_MEM_ERROR:
		Zebra::logger->fatal("zSocket::packetPackZip Z_MEM_ERROR.");
		break;
	    case Z_BUF_ERROR:
		Zebra::logger->fatal("zSocket::packetPackZip Z_BUF_ERROR.");
		break;
	}
#else
	//DWORD nZipLen = unzip_size(nSize);
	DWORD nZipLen = nSize * 1.1 + 1026 * 16 + LZMA_HEADER_LEN + PH_LEN;
	cmd_queue.wr_reserve(nZipLen);
	bool ret = MLzma::LzmaStrCompress((char *)pData, nSize, &(cmd_queue.wr_buf()[PH_LEN]), &nZipLen);
	if(!ret)
	{
	    Zebra::logger->fatal("zSocket::packetPackZip MLzma::LzmaStrCompress ERROR!");
	}
#endif
	nSize = nZipLen;
	nMask |= PACKET_ZIP;
    }
    else
    {
	cmd_queue.wr_reserve(nSize + PH_LEN);
	bcopy(pData,&(cmd_queue.wr_buf()[PH_LEN]),nSize);
    }
    //Zebra::logger->debug("数据压缩: 压缩前大小:%u 压缩后大小:%u",nLen, nSize);
    (*(DWORD *)(cmd_queue.wr_buf())) = (nSize | nMask);

    cmd_queue.wr_flip(nSize + PH_LEN);

    return nSize + PH_LEN;
}
class zProcessor
{
	public:
		virtual bool msgParse(const Cmd::t_NullCmd *, const unsigned int)
		{
		    return false;
		}
		virtual ~zProcessor()  {};
};

#endif

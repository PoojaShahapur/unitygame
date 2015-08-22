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
const DWORD PACKET_ZIP_BUFFER  =  unzip_size(trunkSize - 1) + sizeof(DWORD) + 8;  /**< ѹ����Ҫ�Ļ��� */
#define CHECK_CURR_PTR(size) if(((unsigned int)-1-_currPtr) <= (size)) \
							       Zebra::logger->fatal("��%s, %u ,%u ,%u",__PRETTY_FUNCTION__, _currPtr, (size), _maxSize) \


/**
* �ֽڻ��壬�����׽ӿڽ��պͷ������ݵĻ���
* \param _type ��������������
*/
template <typename _type>
class ByteBuffer
{

public:
	
	/**
	* ���캯��
	*/
	ByteBuffer();

	/**
	* �򻺳���������
	* \param buf �����뻺�������
	* \param size �����뻺�����ݵĳ���
	*/
	inline void put(const BYTE *buf,const DWORD size)
	{
		//����ȷ�ϻ����ڴ��Ƿ��㹻
		wr_reserve(size);

		bcopy(buf,&_buffer[_currPtr],size);
		CHECK_CURR_PTR(size);
		_currPtr += size;
	}

	/**
	* �õ���ǰ��дbf��δ֪
	* ��֤�ڵ��ô˺���д������֮ǰ��Ҫ����wr_reserve(size)��Ԥ����������С
	* \return ��д�뻺�忪ʼ��ַ
	*/
	inline BYTE *wr_buf()
	{
		return &_buffer[_currPtr];
	}

	/**
	* ���ػ�������Ч���ݵĿ�ʼ��ַ
	* \return ��Ч���ݵ�ַ
	*/
	inline BYTE *rd_buf()
	{
		return &_buffer[_offPtr];
	}

	/**
	* �жϻ�����ʱ������Ч����
	* \return ���ػ������Ƿ�����Ч����
	*/
	inline bool rd_ready()
	{
		bool ret = _currPtr > _offPtr;
		return ret;
	}

	/**
	* �õ���������Ч���ݵĴ�С
	* \return ���ػ�������Ч���ݴ�С
	*/
	inline DWORD rd_size()
	{
		DWORD ret = _currPtr - _offPtr;
		return ret;
	}

	/**
	* ���������Ч���ݱ�ʹ���Ժ���Ҫ�Ի����������
	* \param size ���һ��ʹ�õ���Ч���ݳ���
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
	* �õ������д�����ݵĴ�С
	* \return ��д�����ݵĴ�С
	*/
	inline DWORD wr_size()
	{
		DWORD ret = _maxSize - _currPtr;
		return ret;
	}

	/**
	* ʵ���򻺳�д�������ݣ���Ҫ�Ի����������
	* \param size ʵ��д�������
	*/
	inline void wr_flip(const DWORD size)
	{
	    CHECK_CURR_PTR(size);
		_currPtr += size;
	}

	/**
	* ��ֵ�����е����ݣ�������õ���������
	*/
	inline void reset()
	{
		_offPtr = 0;
		_currPtr = 0;
	}

	/**
	* ���ػ�������С
	* \return ��������С
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
	* �Ի�����ڴ�������������򻺳�д���ݣ���������С���㣬���µ��������С��
	* ��С����ԭ����trunkSize����������������
	* \param size �򻺳�д���˶�������
	*/
	inline void wr_reserve(const DWORD size);

private:

	DWORD _maxSize;
	DWORD _offPtr;
	DWORD _currPtr;
	_type _buffer;

};


/**
* ��̬�ڴ�Ļ����������Զ�̬��չ��������С
*/
typedef ByteBuffer<std::vector<unsigned char> > t_BufferCmdQueue;

/**
* ģ��ƫ�ػ�
* �Ի�����ڴ�������������򻺳�д���ݣ���������С���㣬���µ��������С��
* ��С����ԭ����trunkSize����������������
* \param size �򻺳�д���˶�������
*/
template <>
inline void t_BufferCmdQueue::wr_reserve(const DWORD size)
{
    if(_maxSize < _currPtr)
    {
	Zebra::logger->trace("[����]%s,%u,%u,%u",__PRETTY_FUNCTION__,__LINE__,_currPtr,_maxSize);
    }
    if (wr_size() < (size+8))
    {
#define trunkCount(size) (((size) + trunkSize - 1) / trunkSize)
	unsigned int midd = (trunkSize * trunkCount(size+8));
	if(((unsigned int)-1-_maxSize) <= midd)
	    Zebra::logger->fatal("[����]t_BufferCmdQueue::wr_reserve");
	_maxSize += midd;
	_buffer.resize(_maxSize);
    }
}


/**
* ��̬��С�Ļ���������ջ�ռ�����ķ�ʽ�������ڴ棬����һЩ��ʱ�����Ļ�ȡ
*/
typedef ByteBuffer<BYTE [PACKET_ZIP_BUFFER]> t_StackCmdQueue;

/**
* ģ��ƫ�ػ�
* �Ի�����ڴ�������������򻺳�д���ݣ���������С���㣬���µ��������С��
* ��С����ԭ����trunkSize����������������
* \param size �򻺳�д���˶�������
*/
template <>
inline void t_StackCmdQueue::wr_reserve(const DWORD size)
{
	/*
	if (wr_size() < size)
	{
	//���ܶ�̬��չ�ڴ�
	assert(false);
	}
	// */
}


/**
* \brief �䳤ָ��ķ�װ���̶���С�Ļ���ռ�
* ��ջ�ռ���仺���ڴ�
* \param cmd_type ָ������
* \param size �����С
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
* \brief ��װ�׽ӿڵײ㺯�����ṩһ���Ƚ�ͨ�õĽӿ�
*/
class zSocket : private zNoncopyable
{

public:
#if 0
	bool				m_bUseIocp;     // [ranqd]  �Ƿ�ʹ��IOCP�շ�����   

	DWORD               m_SendSize;   // [ranqd] ��¼ϣ�����������ܳ���
	DWORD               m_LastSend;   // [ranqd] ��¼�������������ݳ���
	DWORD               m_LastSended; // [ranqd] �ѷ������������ݳ���
#endif

	static const int T_RD_MSEC          =  2100;          /**< ��ȡ��ʱ�ĺ����� */
	static const int T_WR_MSEC          =  5100;          /**< ���ͳ�ʱ�ĺ����� */

	static const DWORD PH_LEN       =  sizeof(DWORD);  /**< ���ݰ���ͷ��С */
	static const DWORD PACKET_ZIP_MIN  =  32;            /**< ���ݰ�ѹ����С��С */

	static const DWORD PACKET_ZIP    =  0x40000000;        /**< ���ݰ�ѹ����־ */
	static const DWORD INCOMPLETE_READ  =  0x00000001;        /**< �ϴζ��׽ӿڽ��ж�ȡ����û�ж�ȡ��ȫ�ı�־ */
	static const DWORD INCOMPLETE_WRITE  =  0x00000002;        /**< �ϴζ��׽ӿڽ���д�����ú��д����ϵı�־ */

	static const DWORD PACKET_MASK      =  trunkSize - 1;  /**< ������ݰ��������� */
	static const DWORD MAX_DATABUFFERSIZE  =  PACKET_MASK;            /**< ���ݰ���󳤶ȣ�������ͷ4�ֽ� */
	static const DWORD MAX_DATASIZE      =  (MAX_DATABUFFERSIZE - PH_LEN);    /**< ���ݰ���󳤶� */
	static const DWORD MAX_USERDATASIZE    =  (MAX_DATASIZE - 128);        /**< �û����ݰ���󳤶� */

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
	* \brief ��ȡ�׽ӿڶԷ��ĵ�ַ
	* \return IP��ַ
	*/
	inline const char *getIP() const { return inet_ntoa(addr.sin_addr); }
	inline const DWORD getAddr() const { return addr.sin_addr.s_addr; }

	/**
	* \brief ��ȡ�׽ӿڶԷ��˿�
	* \return �˿�
	*/
	inline const WORD getPort() const { return ntohs(addr.sin_port); }

	/**
	* \brief ��ȡ�׽ӿڱ��صĵ�ַ
	* \return IP��ַ
	*/
	inline const char *getLocalIP() const { return inet_ntoa(local_addr.sin_addr); }

	/**
	* \brief ��ȡ�׽ӿڱ��ض˿�
	* \return �˿�
	*/
	inline const WORD getLocalPort() const { return ntohs(local_addr.sin_port); }

	/**
	* \brief ���ö�ȡ��ʱ
	* \param msec ��ʱ����λ���� 
	* \return 
	*/
	inline void setReadTimeout(const int msec) { rd_msec = msec; }

	/**
	* \brief ����д�볬ʱ
	* \param msec ��ʱ����λ���� 
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
	* \brief ���pollfd�ṹ
	* \param pfd �����Ľṹ
	* \param events �ȴ����¼�����
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
	int sock;                  /**< �׽ӿ� */
	struct sockaddr_in addr;          /**< �׽ӿڵ�ַ */
	struct sockaddr_in local_addr;        /**< �׽ӿڵ�ַ */
	int rd_msec;                /**< ��ȡ��ʱ������ */
	int wr_msec;                /**< д�볬ʱ������ */

			
	t_BufferCmdQueue _rcv_queue;        /**< ���ջ���ָ����� */
	DWORD _rcv_raw_size;          /**< ���ջ���������ݴ�С */
	t_BufferCmdQueue _snd_queue;        /**< ���ܻ���ָ����� */
	t_BufferCmdQueue _enc_queue;        /**< ���ܻ���ָ����� */
	DWORD _current_cmd;
	zMutex mutex;                /**< �� */

	zTime last_check_time;            /**< ���һ�μ��ʱ�� */

	DWORD bitmask;            /**< ��־���� */
	CEncrypt enc;                /**< ���ܷ�ʽ */

	inline void set_flag(DWORD _f) { bitmask |= _f; }
	inline bool isset_flag(DWORD _f) const { return bitmask & _f; }
	inline void clear_flag(DWORD _f) { bitmask &= ~_f; }
	inline bool need_enc() const { return CEncrypt::ENCDEC_NONE!=enc.getEncMethod(); }
	/**
	* \brief �������ݰ���ͷ��С����
	* \return ��С����
	*/
	inline DWORD packetMinSize() const { return PH_LEN; }

	/**
	* \brief �����������ݰ��ĳ���
	* \param in ���ݰ�
	* \return �����������ݰ��ĳ���
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
* \brief �����ݽ�����֯,��Ҫʱѹ��,������
* \param pData ����֯�����ݣ�����
* \param nLen ����������ݳ��ȣ�����
* \param cmd_queue ������������
* \return �����Ĵ�С
*/
template<typename buffer_type>
inline DWORD zSocket::packetAppendNoEnc(const void *pData,const DWORD nLen,buffer_type &cmd_queue)
{
    //Zebra::logger->debug("���볤��1�� %d", nLen);
    int nSize = packetPackZip(pData,nLen,cmd_queue,PACKET_ZIP == (bitmask & PACKET_ZIP));
    //Zebra::logger->debug("�������1�� %d", nSize);	

    return nSize;

}

/**
* \brief �����ݽ�����֯,��Ҫʱѹ���ͼ���
* \param pData ����֯�����ݣ�����
* \param nLen ����������ݳ��ȣ�����
* \param cmd_queue ������������
* \return �����Ĵ�С
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
* \brief         �����ݽ��м���
* \param cmd_queue    �����ܵ����ݣ��������
* \param current_cmd  ���һ��ָ���
* \param offset    ���������ݵ�ƫ��
* \return         ���ؼ����Ժ���ʵ���ݵĴ�С
*/
template<typename buffer_type>
inline DWORD zSocket::packetPackEnc(buffer_type &cmd_queue,const DWORD current_cmd,DWORD offset)
{
	DWORD mod = (cmd_queue.rd_size() - offset) % 8;
	if (0!=mod)
	{
		mod = 8 - mod;
		// [ranqd] �����ƺ�������
		//(*(DWORD *)(&(cmd_queue.rd_buf()[cmd_queue.rd_size() - current_cmd - PACKLASTSIZE]))) += mod;
		(*(DWORD *)(&(cmd_queue.rd_buf()[cmd_queue.rd_size() - current_cmd]))) += mod;
		cmd_queue.wr_flip(mod);
	}

	//���ܶ���
	enc.encdec(&cmd_queue.rd_buf()[offset],cmd_queue.rd_size() - offset,true);

	return cmd_queue.rd_size();
}


/**
* \brief       �����ݽ���ѹ��,���ϲ��ж��Ƿ���Ҫ����,����ֻ������ܲ����ж�
* \param pData   ��ѹ�������ݣ�����
* \param nLen     ��ѹ�������ݳ��ȣ�����
* \param pBuffer   ��������ѹ���Ժ������
* \param _compress  �����ݰ�����ʱ���Ƿ�ѹ��
* \return       ���ؼ����Ժ���ʵ���ݵĴ�С
*/
template<typename buffer_type>
inline DWORD zSocket::packetPackZip(const void *pData,const DWORD nLen,buffer_type &cmd_queue,const bool _compress)
{
    /*if (nLen > MAX_DATASIZE)
      {
      Cmd::t_NullCmd *cmd = (Cmd::t_NullCmd *)pData;
      Zebra::logger->warn("zSocket::packetPackZip: ���͵����ݰ�����(cmd = %u,para = %u",cmd->cmd,cmd->para);
      }*/
    DWORD nSize = nLen > MAX_DATASIZE ? MAX_DATASIZE : nLen;//nLen & PACKET_MASK;
    DWORD nMask = 0;//nLen & (~PACKET_MASK);
    if (nSize > PACKET_ZIP_MIN /*���ݰ�����*/ 
	    && _compress /*��ѹ����ǣ����ݰ���Ҫѹ��*/
	    /*&& !(nMask & PACKET_ZIP)*/ /*���ݰ���������Ѿ���ѹ������*/ )
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
    //Zebra::logger->debug("����ѹ��: ѹ��ǰ��С:%u ѹ�����С:%u",nLen, nSize);
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

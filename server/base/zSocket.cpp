/**
 * \brief ʵ����zSocket
 *
 * 
 */
#include "zSocket.h"

const DWORD zSocket::MAX_DATASIZE;
const DWORD zSocket::MAX_USERDATASIZE;

/**
 * ���캯��
 * ģ��ƫ�ػ�
 * ��̬�����ڴ�Ļ�����,��С������ʱ��չ
 */
    template <>
t_BufferCmdQueue::ByteBuffer()
    : _maxSize(trunkSize),_offPtr(0),_currPtr(0),_buffer(_maxSize) { }

    /**
     * ���캯��
     * ģ��ƫ�ػ�
     * ��̬����Ļ�����,��С������ʱ�ı�
     */
    template <>
t_StackCmdQueue::ByteBuffer()
    : _maxSize(PACKET_ZIP_BUFFER),_offPtr(0),_currPtr(0) { }

    /**
     * \brief ���캯��,��ʼ������
     * \param sock �׽ӿ�
     * \param addr ��ַ
     * \param compress �ײ����ݴ����Ƿ�֧��ѹ��
     */

    zSocket::zSocket(
	    const int sock,
	    const struct sockaddr_in *addr,
	    const bool compress)
{
    //	Zebra::logger->debug("�½����� %0.8X", (DWORD)pTask );

    assert(-1 != sock);
    this->sock = sock;
    bzero(&this->addr,sizeof(struct sockaddr_in));
    if (NULL == addr) 
    {
	socklen_t len = sizeof(struct sockaddr);
	getpeername(this->sock,(struct sockaddr *)&this->addr,&len);
    }
    else 
    {
	bcopy(addr,&this->addr,sizeof(struct sockaddr_in));
    }
    bzero(&this->local_addr,sizeof(struct sockaddr_in));
    {
	socklen_t len = sizeof(struct sockaddr_in);
	getsockname(this->sock,(struct sockaddr *)&this->local_addr,&len);
    }

    fcntl(this->sock, F_SETFD, (fcntl(this->sock, F_GETFD, 0)|FD_CLOEXEC));
    setNonblock();

    rd_msec = T_RD_MSEC;
    wr_msec = T_WR_MSEC;
    _rcv_raw_size = 0;
    _current_cmd = 0; 

    set_flag(INCOMPLETE_READ | INCOMPLETE_WRITE);
    if (compress)
	bitmask |= PACKET_ZIP;
    debug_offPtr=debug_currPtr=debug_max_size=debug_wait_size=0;
}


/**
 * \brief ��������
 */
zSocket::~zSocket()
{
    //	Zebra::logger->debug("�ر��׽ӿ�����");

    ::shutdown(sock, SHUT_RDWR);
    TEMP_FAILURE_RETRY(::close(sock));
    sock = -1;
}


#define success_unpack() \
    do { \
	if (_rcv_raw_size >= packetMinSize()/* && _rcv_queue.rd_size() >= packetMinSize()*/) \
	{ \
	    DWORD nRecordLen = packetSize(_rcv_queue.rd_buf()); \
	    if (_rcv_raw_size >= nRecordLen/* && _rcv_queue.rd_size() >= nRecordLen*/) \
	    { \
		int retval = packetUnpack(_rcv_queue.rd_buf(),nRecordLen,(BYTE*)pstrCmd); \
		_rcv_queue.rd_flip(nRecordLen); \
		_rcv_raw_size -= nRecordLen; \
		return retval; \
	    } \
	} \
    } while(0)

/**
 * \brief ����ָ�������
 * \param pstrCmd ָ�����
 * \param nCmdLen ָ������Ĵ�С
 * \param wait ���׽ӿ�����û��׼���õ�ʱ��,�Ƿ���Ҫ�ȴ�
 * \return ʵ�ʽ��յ�ָ���С
 *       ����-1,��ʾ���մ���
 *       ����0,��ʾ���ճ�ʱ
 *       ��������,��ʾʵ�ʽ��յ��ֽ���
 */
int zSocket::recvToCmd(void *pstrCmd,const int nCmdLen,const bool wait)
{
    success_unpack();
    do
    {
	int retval = recvToBuf();
	if(-1 == retval || ( 0 == retval && !wait))
	    return retval;
	success_unpack();
    }while(true);
    return 0;
}

/**
 * \brief ���׽ӿڷ���ԭʼ����,û�д��������,һ�㷢�����ݵ�ʱ����Ҫ�������İ�ͷ
 * \param pBuffer �����͵�ԭʼ����
 * \param nSize �����͵�ԭʼ���ݴ�С
 * \return ʵ�ʷ��͵��ֽ���
 *       ����-1,��ʾ���ʹ���
 *       ����0,��ʾ���ͳ�ʱ
 *       ��������,��ʾʵ�ʷ��͵��ֽ���
 */
int zSocket::sendRawData(const void *pBuffer,const int nSize)
{
    int retcode = 0;
    if(isset_flag(INCOMPLETE_WRITE))
    {
	clear_flag(INCOMPLETE_WRITE);
	goto do_select;
    }
    retcode = TEMP_FAILURE_RETRY(::send(sock, pBuffer, nSize, MSG_NOSIGNAL));
    if(-1 == retcode && (errno == EAGAIN || errno == EWOULDBLOCK))
    {
do_select:
	retcode = waitForWrite();
	if(1 == retcode)
	    retcode = TEMP_FAILURE_RETRY(::send(sock, pBuffer, nSize, MSG_NOSIGNAL));
	else
	    return retcode;
    }
    if(retcode > 0 && retcode < nSize)
	set_flag(INCOMPLETE_WRITE);
    return retcode;
}

/**
 * \brief ����ָ���ֽ�����ԭʼ����,���Գ�ʱ,������ʱ��ķ���,ֱ��������ϻ��߷���ʧ��
 * \param pBuffer �����͵�ԭʼ����
 * \param nSize �����͵�ԭʼ���ݴ�С
 * \return ���������Ƿ�ɹ�
 */
bool zSocket::sendRawDataIM(const void *pBuffer,const int nSize)
{
    //Zebra::logger->debug("zSocket::sendRawDataIM");
    if (NULL == pBuffer || nSize <= 0)
	return false;
    int offset = 0;
    do
    {
	int retcode = sendRawData(&((char *)pBuffer)[offset], nSize-offset);
	if(-1 == retcode)
	{
	    return false;
	}
	offset += retcode;
    }while(offset < nSize);
    return (offset == nSize);
}

/**
 * \brief ����ָ��
 * \param pstrCmd �����͵�����
 * \param nCmdLen ���������ݵĴ�С
 * \param buffer �Ƿ���Ҫ����
 * \return �����Ƿ�ɹ�
 */
bool zSocket::sendCmd(const void *pstrCmd,const int nCmdLen,const bool buffer)
{
    //Zebra::logger->debug("zSocket::sendCmd buffer:%u nCmdLen:%u",buffer?1:0, nCmdLen);
    if (NULL == pstrCmd || nCmdLen <= 0)
	return false;
    bool retval = true;
    if (buffer)
    {
	t_StackCmdQueue _raw_queue;
	packetAppendNoEnc(pstrCmd,nCmdLen,_raw_queue);
	mutex.lock();
	_snd_queue.put(_raw_queue.rd_buf(),_raw_queue.rd_size());
	_current_cmd = _raw_queue.rd_size();
	mutex.unlock();
    }
    else
    {
	t_StackCmdQueue _raw_queue;
	packetAppend(pstrCmd,nCmdLen,_raw_queue);
	mutex.lock();
	retval = sendRawDataIM(_raw_queue.rd_buf(),_raw_queue.rd_size());
	mutex.unlock();
    }
    return retval;
}

/**
 * \brief ����ԭʼָ������,����������� [ranqd] ����ԭʼ���ݵ�ת��
 * \param pstrCmd �����͵�����
 * \param nCmdLen ���������ݵĴ�С
 * \param buffer �Ƿ���Ҫ����
 * \return �����Ƿ�ɹ�
 */
bool zSocket::sendCmdNoPack(const void *pstrCmd,const int nCmdLen,const bool buffer)
{
    //Zebra::logger->debug("zSocket::sendCmdNoPack");
    if (NULL == pstrCmd || nCmdLen <= 0)
	return false;

    bool retval = true;

    if (buffer)
    {
	mutex.lock();
	_snd_queue.put((unsigned char *)pstrCmd, nCmdLen);
	_current_cmd = nCmdLen;
	mutex.unlock();
    }
    else
    {	
	if ( need_enc() )
	{
	    t_StackCmdQueue _raw_queue;
	    _raw_queue.put((BYTE*)pstrCmd,nCmdLen);

	    packetPackEnc( _raw_queue,_raw_queue.rd_size() );
	    mutex.lock();
	    retval = sendRawDataIM( _raw_queue.rd_buf(),_raw_queue.rd_size() );
	    mutex.unlock();
	}
	else
	{
	    mutex.lock();
	    retval = sendRawDataIM(pstrCmd, nCmdLen);
	    mutex.unlock();
	}
    }
    return retval;
}

bool zSocket::sync()
{
    if(need_enc())
    {
	unsigned int nSize = 0, current_cmd = 0, offset = 0;
	mutex.lock();
	if(_snd_queue.rd_ready())
	{
	    nSize = _snd_queue.rd_size();
	    current_cmd = _current_cmd;
	    offset = _enc_queue.rd_size();

	    debug_offPtr = _enc_queue.offPtr();
	    debug_currPtr = _enc_queue.currPtr();
	    debug_max_size = _enc_queue.maxSize();
	    debug_wait_size = nSize;

	    _enc_queue.put(_snd_queue.rd_buf(), nSize);
	    _snd_queue.rd_flip(nSize);
	    _current_cmd = 0;
	}
	mutex.unlock();
	if(nSize)
	    packetPackEnc(_enc_queue, current_cmd, offset);
	if(_enc_queue.rd_ready())
	{
	    int retcode = sendRawData_NoPoll(_enc_queue.rd_buf(), _enc_queue.rd_size());
	    if(retcode > 0)
	    {
		_enc_queue.rd_flip(retcode);
	    }
	    else if(-1 == retcode)
	    {
		return false;
	    }
	}
    }
    else
    {
	mutex.lock();
	if(_snd_queue.rd_ready())
	{
	    _enc_queue.put(_snd_queue.rd_buf(), _snd_queue.rd_size());
	    _snd_queue.rd_flip(_snd_queue.rd_size());
	}
	mutex.unlock();
	if(_enc_queue.rd_ready())
	{
	    int retcode = sendRawData_NoPoll(_enc_queue.rd_buf(), _enc_queue.rd_size());
	    if(retcode > 0)
	    {
		_enc_queue.rd_flip(retcode);
	    }
	    else if(-1 == retcode)
	    {
		return false;
	    }
	}
    }
    return true;
}

void zSocket::force_sync()
{
    if(need_enc())
    {
	unsigned int nSize = 0, current_cmd = 0, offset = 0;
	mutex.lock();
	if(_snd_queue.rd_ready())
	{
	    nSize = _snd_queue.rd_size();
	    current_cmd = _current_cmd;
	    offset = _enc_queue.rd_size();
	    _enc_queue.put(_snd_queue.rd_buf(), nSize);
	    _snd_queue.rd_flip(nSize);
	    _current_cmd = 0;
	}
	mutex.unlock();
	if(nSize)
	    packetPackEnc(_enc_queue, current_cmd, offset);
	if(_enc_queue.rd_ready())
	{
	    sendRawDataIM(_enc_queue.rd_buf(), _enc_queue.rd_size());
	    _enc_queue.reset();
	}
    }
    else
    {
	mutex.lock();
	if(_snd_queue.rd_ready())
	{
	    _enc_queue.put(_snd_queue.rd_buf(), _snd_queue.rd_size());
	    _snd_queue.rd_flip(_snd_queue.rd_size());
	}
	mutex.unlock();
	if(_enc_queue.rd_ready())
	{
	    sendRawDataIM(_enc_queue.rd_buf(), _enc_queue.rd_size());
	    _enc_queue.reset();
	}
    }
}

/**
 * \brief ����׽ӿ�׼���ö�ȡ����
 * \return �Ƿ�ɹ�
 *       -1,��ʾ����ʧ��
 *       0,��ʾ������ʱ
 *       1,��ʾ�ȴ��ɹ�,�׽ӿ��Ѿ�������׼����ȡ
 */
int zSocket::checkIOForRead()
{
    //Zebra::logger->debug("zSocket::checkIOForRead");
    struct pollfd pfd;

    pfd.fd = sock;
    pfd.events = POLLIN | POLLERR| POLLPRI;
    pfd.revents = 0;

    int retcode = TEMP_FAILURE_RETRY(::poll(&pfd,1,0));
    if (retcode > 0 && 0 == (pfd.revents & POLLIN))
	retcode = -1;
    return retcode;
}

/**
 * \brief �ȴ��׽ӿ�׼����д�����
 * \return �Ƿ�ɹ�
 *       -1,��ʾ����ʧ��
 *       0,��ʾ������ʱ
 *       1,��ʾ�ȴ��ɹ�,�׽ӿ��Ѿ�����д������
 */
int zSocket::checkIOForWrite()
{
    //Zebra::logger->debug("zSocket::checkIOForWrite");

    struct pollfd pfd;

    pfd.fd = sock;
    pfd.events = POLLOUT | POLLERR | POLLPRI;
    pfd.revents = 0;

    int retcode = TEMP_FAILURE_RETRY(::poll(&pfd,1,0));
    if (retcode > 0 && 0 == (pfd.revents & POLLOUT))
	retcode = -1;

    return retcode;
}

/**
 * \brief �����������Ʊ�Ż�ȡָ��������IP��ַ
 * \param ifName ��Ҫȡ��ַ����������
 * \return ��ȡ��ָ��������IP��ַ
 */
const char *zSocket::getIPByIfName(const char *ifName)
{
    int s;
    struct ifreq ifr;
    static char *none_ip = "0.0.0.0";
    //Zebra::logger->info("zSocket::getIPByIfName(%s)",ifName);
    if (NULL == ifName) return none_ip;
    s = ::socket(AF_INET, SOCK_DGRAM, 0);
    if(-1 == s)
	return none_ip;
    bzero(ifr.ifr_name, sizeof(ifr.ifr_name));
    strncpy(ifr.ifr_name, ifName, sizeof(ifr.ifr_name)-1);
    if(-1 == ioctl(s, SIOCGIFADDR, &ifr))
    {
	TEMP_FAILURE_RETRY(::close(s));
	return none_ip;
    }
    TEMP_FAILURE_RETRY(::close(s));
    return inet_ntoa(((struct sockaddr_in *)&ifr.ifr_addr)->sin_addr);
}

#define success_recv_and_dec() \
    do { \
	if ((DWORD)retcode < _rcv_queue.wr_size()) \
	{ \
	    set_flag(INCOMPLETE_READ); \
	} \
	_rcv_queue.wr_flip(retcode); \
	DWORD size = _rcv_queue.rd_size() - _rcv_raw_size - ((_rcv_queue.rd_size() - _rcv_raw_size) % 8); \
	if (size) \
	{ \
	    enc.encdec(&_rcv_queue.rd_buf()[_rcv_raw_size],size,false); \
	    _rcv_raw_size += size; \
	} \
    } while(0)

#define success_recv() \
    do { \
	if ((DWORD)retcode < _rcv_queue.wr_size()) \
	{ \
	    set_flag(INCOMPLETE_READ); \
	} \
	_rcv_queue.wr_flip(retcode); \
	_rcv_raw_size += retcode; \
    } while(0)

/**
 * \brief   �������ݵ�������,��֤�ڵ����������֮ǰ�׽ӿ�׼�����˽���,Ҳ����ʹ��poll��ѯ��
 *       ����Ǽ��ܰ���Ҫ�Ƚ��ܵ����ܻ�����
 * \return   ʵ�ʽ����ֽ���
 *       ����-1,��ʾ���մ���
 *       ����0,��ʾ���ճ�ʱ
 *       ��������,������ʱ��ʾʵ�ʽ��յ��ֽ���,����ʱ���ؽ��ܵ����ֽ���
 */
int zSocket::recvToBuf_NoPoll()
{
    //Zebra::logger->debug("zSocket::recvToBuf_NoPoll");
    int retcode =0; 
    if (need_enc())
    {
	_rcv_queue.wr_reserve(MAX_DATABUFFERSIZE);
	retcode = TEMP_FAILURE_RETRY(::recv(sock, _rcv_queue.wr_buf(),_rcv_queue.wr_size(),MSG_NOSIGNAL));
	if (retcode == -1 && (errno == EAGAIN || errno == EWOULDBLOCK))
	    return 0;//should retry
	if(retcode > 0)
	    success_recv_and_dec();
    }
    else
    {
	_rcv_queue.wr_reserve(MAX_DATABUFFERSIZE);
	retcode = TEMP_FAILURE_RETRY(::recv(sock, _rcv_queue.wr_buf(),_rcv_queue.wr_size(),MSG_NOSIGNAL));
	if (retcode == -1 && (errno == EAGAIN || errno == EWOULDBLOCK))
	    return 0;//should retry

	if (retcode > 0)
	{
	    success_recv();
	}
    }

    if (0 == retcode)
	return -1;//EOF 

    return retcode;
}

/**
 * \brief ����ָ�������,�����׽ӿڽ���ָ��,ֻ�ǰѽ��ջ�������ݽ��
 * \param pstrCmd ָ�����
 * \param nCmdLen ָ������Ĵ�С
 * \return ʵ�ʽ��յ�ָ���С
 *       ����-1,��ʾ���մ���
 *       ����0,��ʾ���ճ�ʱ
 *       ��������,��ʾʵ�ʽ��յ��ֽ���
 */
int zSocket::recvToCmd_NoPoll(void *pstrCmd,const int nCmdLen)
{
    success_unpack();
    return 0;
}

/**
 * \brief ���׽ӿڷ���ԭʼ����,û�д��������,һ�㷢�����ݵ�ʱ����Ҫ�������İ�ͷ,��֤�ڵ����������֮ǰ�׽ӿ�׼�����˽���,Ҳ����ʹ��poll��ѯ��
 * \param pBuffer �����͵�ԭʼ����
 * \param nSize �����͵�ԭʼ���ݴ�С
 * \return ʵ�ʷ��͵��ֽ���
 *       ����-1,��ʾ���ʹ���
 *       ����0,��ʾ���ͳ�ʱ
 *       ��������,��ʾʵ�ʷ��͵��ֽ���
 */
int zSocket::sendRawData_NoPoll(const void *pBuffer,const int nSize)
{

    int retcode = TEMP_FAILURE_RETRY(::send(sock,pBuffer,nSize,MSG_NOSIGNAL));
    if (retcode == -1 && (errno == EAGAIN || errno == EWOULDBLOCK))
	return 0;//should retry

    if (retcode > 0 && retcode < nSize)
	set_flag(INCOMPLETE_WRITE);

    return retcode;
}

/**
 * \brief �����׽ӿ�Ϊ������ģʽ
 * \return �����Ƿ�ɹ�
 */
bool zSocket::setNonblock()
{
    int fd_flags;
    int nodelay = 1;

    if (::setsockopt(sock,IPPROTO_TCP,TCP_NODELAY,(void *)&nodelay,sizeof(nodelay)))
	return false;

    fd_flags = ::fcntl(sock, F_GETFL, 0);

#if defined(O_NONBLOCK)
    fd_flags |= O_NONBLOCK;
#elif defined(O_NDELAY)
    fd_flags |= O_NDELAY;
#elif defined(FNDELAY)
    fd_flags |= O_FNDELAY;
#else
    return false;
#endif

    if(::fcntl(sock, F_SETFL, fd_flags) == -1)
	return false;

    return true;
}

/**
 * \brief �ȴ��׽ӿ�׼���ö�ȡ����
 * \return �Ƿ�ɹ�
 *       -1,��ʾ����ʧ��
 *       0,��ʾ������ʱ
 *       1,��ʾ�ȴ��ɹ�,�׽ӿ��Ѿ�������׼����ȡ
 */
int zSocket::waitForRead()
{
    //Zebra::logger->debug("zSocket::waitForRead");
    struct pollfd pfd;

    pfd.fd = sock;
    pfd.events = POLLIN | POLLERR| POLLPRI;
    pfd.revents = 0;

    int retcode = TEMP_FAILURE_RETRY(::poll(&pfd,1,rd_msec));
    if (retcode > 0 && 0 == (pfd.revents & POLLIN))
	retcode = -1;
    return retcode;
}


/**
 * \brief �ȴ��׽ӿ�׼����д�����
 * \return �Ƿ�ɹ�
 *       -1,��ʾ����ʧ��
 *       0,��ʾ������ʱ
 *       1,��ʾ�ȴ��ɹ�,�׽ӿ��Ѿ�����д������
 */
int zSocket::waitForWrite()
{
    //Zebra::logger->debug("zSocket::waitForWrite");
    struct pollfd pfd;

    pfd.fd = sock;
    pfd.events = POLLOUT | POLLERR |POLLPRI;
    pfd.revents = 0;

    int retcode = TEMP_FAILURE_RETRY(::poll(&pfd,1,wr_msec));
    if (retcode > 0 && 0 == (pfd.revents & POLLOUT))
	retcode = -1;

    return retcode;
}

/**
 * \brief   �������ݵ�������
 *       ����Ǽ��ܰ���Ҫ���ܵ����ܻ�����
 * \return   ʵ�ʽ����ֽ���
 *       ����-1,��ʾ���մ���
 *       ����0,��ʾ���ճ�ʱ
 *       ��������,�����ܰ���ʾʵ�ʽ��յ��ֽ���,���ܰ����ؽ��ܺ�ʵ�ʿ��õ��ֽ���
 */
int zSocket::recvToBuf()
{
    //Zebra::logger->debug("zSocket::recvToBuf");
    int retcode = 0;

    if (need_enc())
    {
	if (isset_flag(INCOMPLETE_READ))
	{
	    clear_flag(INCOMPLETE_READ);
	    goto do_select_enc;
	}

	_rcv_queue.wr_reserve(MAX_DATABUFFERSIZE);
	retcode = TEMP_FAILURE_RETRY(::recv(sock, _rcv_queue.wr_buf(),_rcv_queue.wr_size(),MSG_NOSIGNAL));
	if ( retcode == -1 && (errno == EAGAIN || errno == EWOULDBLOCK))
	{
do_select_enc:
	    retcode = waitForRead();
	    if (1 == retcode)
		retcode = TEMP_FAILURE_RETRY(::recv(sock, _rcv_queue.wr_buf(),_rcv_queue.wr_size(),MSG_NOSIGNAL));
	    else
		return retcode;
	}
	if (retcode > 0) 
	    success_recv_and_dec();
    }
    else
    {
	if (isset_flag(INCOMPLETE_READ))
	{
	    clear_flag(INCOMPLETE_READ);
	    goto do_select;
	}
	_rcv_queue.wr_reserve(MAX_DATABUFFERSIZE);
	retcode = TEMP_FAILURE_RETRY(::recv(sock, _rcv_queue.wr_buf(),_rcv_queue.wr_size(),MSG_NOSIGNAL));
	if ( retcode == -1 && (errno == EAGAIN || errno == EWOULDBLOCK))
	{
do_select:
	    retcode = waitForRead();
	    if (1 == retcode)
		retcode = TEMP_FAILURE_RETRY(::recv(sock, _rcv_queue.wr_buf(),_rcv_queue.wr_size(),MSG_NOSIGNAL));
	    else
		return retcode;
	}

	if (retcode >  0)
	    success_recv();
    }

    if (0 == retcode)
	return -1;//EOF 

    return retcode;
}

/**
 * \brief         ���
 * \param in       ��������
 * \param nPacketLen   ����ʱ���������ݰ�����,����PH_LEN
 * \param out       �������
 * \return         ����Ժ�����ݰ�����
 */
DWORD zSocket::packetUnpack(BYTE *in,const DWORD nPacketLen,BYTE *out)
{
    //Zebra::logger->debug("zSocket::packetUnpack");
    DWORD nRecvCmdLen = nPacketLen - PH_LEN;

    //���ݰ�ѹ����
    if (PACKET_ZIP == ((*(DWORD*)(in)) & PACKET_ZIP))
    {
#ifndef _LZMA
	uLong nUnzipLen = MAX_DATASIZE;
	int retcode = uncompress(out,&nUnzipLen,&(in[PH_LEN]),nRecvCmdLen);
	switch(retcode)
	{
	    case Z_OK:
		break;
	    case Z_MEM_ERROR:
		Zebra::logger->fatal("zSocket::packetUnpack Z_MEM_ERROR.");
		break;
	    case Z_BUF_ERROR:
		Zebra::logger->fatal("zSocket::packetUnpack Z_BUF_ERROR.");
		break;
	    case Z_DATA_ERROR:
		Zebra::logger->fatal("zSocket::packetUnpack Z_DATA_ERROR.");
		break;
	   
	}
	//Zebra::logger->debug("���ݽ�ѹ: ��ѹǰ��С:%u ��ѹ���С:%u", nPacketLen, (DWORD)nUnzipLen);
//#error ������1
#else
	DWORD nUnzipLen = MAX_DATASIZE;
	bool ret = MLzma::LzmaStrUncompress(&(in[PH_LEN]), nRecvCmdLen, out, &nUnzipLen);
	if(!ret)
	{
	    Zebra::logger->fatal("zSocket::packetUnpack MLzma::LzmaStrUncompress");
	}
	//Zebra::logger->debug("���ݽ�ѹ: ��ѹǰ��С:%u ��ѹ���С:%u", nPacketLen, nUnzipLen);
//#error ������2
#endif
	//���صõ����ֽ���
	return nUnzipLen;
    }
    else
    {
	bcopy(&(in[PH_LEN]),out,nRecvCmdLen);
	return nRecvCmdLen;
    }
}



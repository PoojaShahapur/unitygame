/**
 * \brief ʵ�ֹ��������
 *
 * ��һ�����е����з��������й���
 * 
 */
#ifndef _DumpServer_h_
#define _DumpServer_h_

#include <iostream>
#include <string>
#include <ext/numeric>
#include "zService.h"
#include "zThread.h"
#include "zSocket.h"
#include "zTCPServer.h"
#include "zNetService.h"
#include "zDBConnPool.h"
#include "zMisc.h"
#include "zType.h"
#include "zHttpTaskPool.h"


/**
 * \brief �����������
 *
 * �����˻���<code>zNetService</code>
 *
 */
class DumpServer : public zNetService
{

  public:


    /**
     * \brief ��ȡ���Ψһʵ��
     *
     * ʹ����Singleton���ģʽ����֤��һ��������ֻ��һ�����ʵ��
     *
     * \return ���Ψһʵ��
     */
    static DumpServer &getInstance()
    {
      if (NULL == instance)
        instance = new DumpServer();

      return *instance;
    }

    /**
     * \brief �ͷ����Ψһʵ��
     *
     */
    static void delInstance()
    {
      SAFE_DELETE(instance);
    }

    /**
     * \brief ָ�����ݿ����ӳ�ʵ����ָ��
     *
     */
    static zDBConnPool *dbConnPool;

  private:

    /**
     * \brief ���Ψһʵ��ָ��
     *
     */
    static DumpServer *instance;


    /**
     * \brief ���캯��
     *
     */
    DumpServer();

    /**
     * \brief ��������
     *
     * �麯��
     *
     */
    ~DumpServer()
    {
      instance = NULL;

      //�ر��̳߳�
      if (httpTaskPool)
      {
        httpTaskPool->final();
        SAFE_DELETE(httpTaskPool);
      }
    }

    bool init();
    void newTCPTask(const int sock,const struct sockaddr_in *addr);
    void final();

    zHttpTaskPool *httpTaskPool;        /**< ���ӳص�ָ�� */

};

#endif

/**
 * \brief zebra��Ŀ���������������ڴ���������Ͷ�ȡ����
 *
 */
#ifndef _ClientService_h_
#define _ClientService_h_
#include "zService.h"
#include "zType.h"
class LoginClient;

/**
 * \brief ���嵵��������
 *
 * ��Ŀ���������������ڴ���������Ͷ�ȡ����<br>
 * �����ʹ����Singleton���ģʽ����֤��һ��������ֻ��һ�����ʵ��
 *
 */
class ClientService : public zService
{

  public:

    /**
     * \brief ����������
     *
     */
    ~ClientService()
    {
    }

    /**
     * \brief ����Ψһ����ʵ��
     *
     * \return Ψһ����ʵ��
     */
    static ClientService &getInstance()
    {
      if (NULL == instance)
        instance = new ClientService();

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

    bool init();
    bool serviceCallback();
    void final();

    LoginClient *loginClient;
  private:

    /**
     * \brief ���Ψһʵ��ָ��
     *
     */
    static ClientService *instance;

    /**
     * \brief ���캯��
     *
     */
    ClientService() : zService("���Կͻ���")
    {
    }

};

#endif


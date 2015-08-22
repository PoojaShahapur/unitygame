#ifndef _ScenesService_h_
#define _ScenesService_h_

#include "zSubNetService.h"
#include "Zebra.h"
#include "zMisc.h"

/**
 * \brief ���峡��������
 *
 * ��������������Ϸ���󲿷����ݶ��ڱ�ʵ��<br>
 * �����ʹ����Singleton���ģʽ����֤��һ��������ֻ��һ�����ʵ��
 *
 */
class ScenesService : public zSubNetService
{

  public:
    int writeBackTimer;
    int ExpRate;
    int DropRate;
    int DropRateLevel;
    bool msgParse_SuperService(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);
    BYTE countryPower[13];

    /**
     * \brief ����������
     *
     */
    virtual ~ScenesService()
    {
      instance = NULL;

      //�ر��̳߳�
      if (taskPool)
      {
        taskPool->final();
        SAFE_DELETE(taskPool);
      }
    }

    const int getPoolSize() const
    {
      if (taskPool)
      {
        return taskPool->getSize();
      }
      else
      {
        return 0;
      }
    }

    /**
     * \brief ����Ψһ����ʵ��
     *
     * \return Ψһ����ʵ��
     */
    static ScenesService &getInstance()
    {
      if (NULL == instance)
        instance = new ScenesService();

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

    void reloadConfig();
    void checkAndReloadConfig();
    bool isSequeueTerminate() 
    {
      return taskPool == NULL;
    }

    //GM_logger
    static zLogger* gmlogger;
    //��Ʒlog
    static zLogger* objlogger;
    //���_logger
    static zLogger* wglogger;
    
    DWORD verify_version;
    WORD battleZoneID;
    bool battle;
  //  static Cmd::stChannelChatUserCmd * pStampData;
    //static DWORD updateStampData();
  private:

    /**
     * \brief ���Ψһʵ��ָ��
     *
     */
    static ScenesService *instance;
    /**
     * \brief �������¶�ȡ���ñ�־
     *
     */
    static bool reload;

    zTCPTaskPool *taskPool;        /**< TCP���ӳص�ָ�� */

    /**
     * \brief ���캯��
     *
     */
    ScenesService() : zSubNetService("����������",SCENESSERVER)
    {
      writeBackTimer = 0;
      taskPool = NULL;
      battleZoneID = 0;
    }


    bool init();
    void newTCPTask(const int sock,const struct sockaddr_in *addr);
    void final();

	//[Shx Add ��ȡ��װ�����б� SuitInfo.xml] 
	bool LoadSuitInfo();
	//sky ��ȡ��Ʒ��ȴ�����б�
	bool LoadItmeCoolTime();
	//sky ��ȴ���÷������
	bool StlToSendData();
};

#endif

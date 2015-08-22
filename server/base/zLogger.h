#ifndef _ZLOGGER_H_
#define _ZLOGGER_H_
#include <log4cxx/logger.h>
#include <string>
#include "zType.h"
#include "zMutex.h"

#define MSGBUF_MAX 4096

class zServerInfoManager;

/**
* \brief Zebra项目的日志类。
*
* 目前实现了两种写日志方式，即控制台、本地文件。
*
* 默认日志级别是#DEBUG
*
* 此类为线程安全类。
*/
class zLogger
{
public:
	/**
	* \brief zLevel声明了几个日志等级
	*
	* 除了用log4cxx提供的标准日志等级作为日志等级外，还自定义了游戏日志等级.
	*
	* 程序日志等级关系为 #OFF> #FATAL> #ERROR> #WARN> #INFO> #DEBUG> #ALL
	*
	*/

	/**
	* \brief Zebra项目所支持日志等级数字定义
	*/
	class zLevel
	{
	    friend class zLogger;
	    private:
	    enum zLevelInt
	    {
		ALARM_INT   =	log4cxx::Level::ERROR_INT,
		IFFY_INT    =	log4cxx::Level::WARN_INT,
		TRACE_INT    =   log4cxx::Level::INFO_INT,
		GBUG_INT    =   log4cxx::Level::DEBUG_INT
	    };

	    static const log4cxx::LevelPtr LEVELALARM;
	    static const log4cxx::LevelPtr LEVELIFFY;
	    static const log4cxx::LevelPtr LEVELTRACE;
	    static const log4cxx::LevelPtr LEVELGBUG;
	    
	    log4cxx::LevelPtr zlevel;
	    zLevel(log4cxx::LevelPtr level);
	    public:
	    static const zLevel * OFF;
	    static const zLevel * FATAL;
	    static const zLevel * ERROR;
	    static const zLevel * ALARM;
	    static const zLevel * WARN;
	    static const zLevel * IFFY;
	    static const zLevel * INFO;
	    static const zLevel * TRACE;
	    static const zLevel * DEBUG;
	    static const zLevel * GBUG;
	    static const zLevel * ALL;
	};

	zLogger(const std::string &name = "Zebra");
	~zLogger();
	
	std::string getName() const;
	void setName(const std::string &setName);
	static bool addConsoleLog();
	static void removeConsoleLog();
	void addLocalFileLog(const std::string &file);

	void setLevel(const zLevel* level);
	void setInfoM(zServerInfoManager * const info);
	void setLevel(const std::string &level);

	void logtext(const zLevel level,const char * text);
	void log(const zLevel* level,const char * pattern,...)
	    __attribute__((format(printf, 3, 4)));

	void debug(const char * pattern,...)
	    __attribute__((format(printf, 2, 3)));
	void error(const char * pattern,...)
	    __attribute__((format(printf, 2, 3)));
	void info(const char * pattern,...)
	    __attribute__((format(printf, 2, 3)));
	void fatal(const char * pattern,...)
	    __attribute__((format(printf, 2, 3)));
	void warn(const char * pattern,...)
	    __attribute__((format(printf, 2, 3)));
	void alarm(const char * pattern,...)
	    __attribute__((format(printf, 2, 3)));
	void iffy(const char * pattern,...)
	    __attribute__((format(printf, 2, 3)));
	void trace(const char * pattern,...)
	    __attribute__((format(printf, 2, 3)));
	void gbug(const char * pattern,...)
	    __attribute__((format(printf, 2, 3)));//�����������ͻ��ڱ���ʱ�ú�printfһ����check������ȷ�Ͽɱ�����Ƿ���ȷ��
	bool isgood() {	return logger!=NULL?true:false; };
private:
	
	log4cxx::LoggerPtr logger;
	char message[MSGBUF_MAX];

	zServerInfoManager* m_info;

	zMutex msgMut;
	std::string m_sLocalLogFile;
};

#endif


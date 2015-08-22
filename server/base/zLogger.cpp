/**
* \brief Zebra项目日志系统定义文件
*
*/
#include "zLogger.h"
#include "Zebra.h"
#include <stdarg.h>
#include <log4cxx/patternlayout.h>
#include <log4cxx/consoleappender.h>
#include <log4cxx/dailyrollingfileappender.h>
#include <log4cxx/helpers/dateformat.h>
#include <log4cxx/helpers/stringhelper.h>
#include <log4cxx/helpers/loglog.h>
#include <log4cxx/helpers/system.h>
#include <sys/stat.h>
#include <time.h>

const log4cxx::LevelPtr zLogger::zLevel::LEVELALARM(new log4cxx::Level(ALARM_INT, "ALARM", 3));
const log4cxx::LevelPtr zLogger::zLevel::LEVELIFFY(new log4cxx::Level(IFFY_INT, "IFFY", 3));
const log4cxx::LevelPtr zLogger::zLevel::LEVELTRACE(new log4cxx::Level(TRACE_INT, "TRACE", 3));
const log4cxx::LevelPtr zLogger::zLevel::LEVELGBUG(new log4cxx::Level(GBUG_INT, "GBUG", 3));

const zLogger::zLevel * zLogger::zLevel::OFF = new zLevel(log4cxx::Level::getOff());
const zLogger::zLevel * zLogger::zLevel::FATAL = new zLevel(log4cxx::Level::getFatal());
const zLogger::zLevel * zLogger::zLevel::ALARM = new zLevel(LEVELALARM);
const zLogger::zLevel * zLogger::zLevel::ERROR = new zLevel(log4cxx::Level::getError());
const zLogger::zLevel * zLogger::zLevel::IFFY = new zLevel(LEVELIFFY);
const zLogger::zLevel * zLogger::zLevel::WARN = new zLevel(log4cxx::Level::getWarn());
const zLogger::zLevel * zLogger::zLevel::TRACE = new zLevel(LEVELTRACE);
const zLogger::zLevel * zLogger::zLevel::INFO = new zLevel(log4cxx::Level::getInfo());
const zLogger::zLevel * zLogger::zLevel::GBUG = new zLevel(LEVELGBUG);
const zLogger::zLevel * zLogger::zLevel::DEBUG = new zLevel(log4cxx::Level::getDebug());
const zLogger::zLevel * zLogger::zLevel::ALL = new zLevel(log4cxx::Level::getAll());

zLogger::zLevel::zLevel(log4cxx::LevelPtr level):zlevel(level)
{
}

static bool s_bFirst = true;
/**
* \brief 构造一个zLogger 
*
* \param  name zLogger的名字，将会出现在输出的日志中的每一行
*/
zLogger::zLogger(const std::string &name)
{
	bzero(message, sizeof(message));
	m_info = NULL;
	logger = log4cxx::Logger::getLogger(name);
	log4cxx::Logger::getRootLogger()->setLevel(log4cxx::Level::getDebug());
	if(s_bFirst)
	{
		s_bFirst = false;
		addConsoleLog();
	}
}

/**
* \brief 析构函数
*/
zLogger::~zLogger()
{
	
}

std::string zLogger::getName() const
{
	std::string sName;
	logger->getName(sName);
	return sName;
}

void zLogger::setName(const std::string &setName)
{
	logger = log4cxx::Logger::getLogger(setName);
	if(!m_sLocalLogFile.empty())
	{
		addLocalFileLog(m_sLocalLogFile);
	}
}

const char CONSOLE[] = "console";

bool zLogger::addConsoleLog()
{
	log4cxx::LoggerPtr root = log4cxx::Logger::getRootLogger();
	if(root->getAppender(CONSOLE)) return true;
	
	log4cxx::PatternLayoutPtr cpl = new log4cxx::PatternLayout("\%d{\%y\%m\%d-\%H:\%M:\%S} %c %5p: %m%n");
	log4cxx::ConsoleAppenderPtr carp = new log4cxx::ConsoleAppender(cpl);
	carp->setName(CONSOLE);
	root->addAppender(carp);
	return true;
}
/**
* \brief 移除控制台Log输出
*/
void zLogger::removeConsoleLog()
{
	log4cxx::Logger::getRootLogger()->removeAppender(CONSOLE);
}

/**
* \brief 加一个本地文件Log输出
*
* \param file 要输出的文件名，Logger会自动地添加时间后缀 
* \return 无
*/
void zLogger::addLocalFileLog(const std::string &file)
{
	m_sLocalLogFile = file;
	log4cxx::PatternLayoutPtr fpl = new log4cxx::PatternLayout("\%d{\%y\%m\%d-\%H:\%M:\%S} %c %5p: %m%n");
	log4cxx::DailyRollingFileAppender * farp
		= new log4cxx::DailyRollingFileAppender(fpl, file, "'.'yyMMdd-HH");
	farp->setName("localfile:"+file);
	logger->addAppender(farp);
}

#define getMessage(msg, msglen, pat)	\
do	\
{	\
	va_list ap;	\
	bzero(msg, msglen);	\
	va_start(ap, pat);	\
	vsnprintf(msg, msglen-1, pat, ap);	\
	va_end(ap);	\
}while(false)
/**
* \brief 设置写日志等级
* \param  zLevelPtr 日志等级.参见 #zLogger::zLevel
*/
void zLogger::setLevel(const zLevel *zLevelPtr)
{
	log4cxx::Logger::getRootLogger()->setLevel(zLevelPtr->zlevel);
}

/**
* \brief 设置写日志等级
* \param  level 日志等级
*/
void zLogger::setLevel(const std::string &level)
{
	if ("off" == level) setLevel(zLevel::OFF);
	else if ("fatal" == level) setLevel(zLevel::FATAL);
	else if ("error" == level) setLevel(zLevel::ERROR);
	else if ("warn" == level) setLevel(zLevel::WARN);
	else if ("info" == level) setLevel(zLevel::INFO);
	else if ("debug" == level) setLevel(zLevel::DEBUG);
	else if ("all" == level) setLevel(zLevel::ALL);
	else if ("iffy" == level) setLevel(zLevel::IFFY);
	else if ("trace" == level) setLevel(zLevel::TRACE);
	else if ("gbug" == level) setLevel(zLevel::GBUG);
	else if ("alarm" == level) setLevel(zLevel::ALARM);
}



/**
* \brief 写日志
* \param  zLevelPtr 日志等级参见 #zLogger::zLevel
* \param  pattern 输出格式范例，与printf一样
* \return 无
*/
void zLogger::log(const zLevel *level,const char * pattern,...)
{
	msgMut.lock();
	getMessage(message, MSGBUF_MAX, pattern);
	logger->log(level->zlevel,message);
	msgMut.unlock();
}

/**
* \brief 写fatal程序日志
* \param  pattern 输出格式范例，与printf一样
* \return 无
*/
void zLogger::fatal(const char * pattern,...)
{
	msgMut.lock();
	getMessage(message, MSGBUF_MAX, pattern);
	logger->log(zLevel::FATAL->zlevel,message);
	msgMut.unlock();
}

/**
* \brief 写error程序日志
* \param  pattern 输出格式范例，与printf一样
* \return 无
*/
void zLogger::error(const char * pattern,...)
{
	msgMut.lock();
	getMessage(message, MSGBUF_MAX, pattern);
	logger->log(zLevel::ERROR->zlevel,message);
	msgMut.unlock();
}

/**
* \brief 写warn程序日志
* \param  pattern 输出格式范例，与printf一样
* \return 无
*/
void zLogger::warn(const char * pattern,...)
{
	msgMut.lock();
	getMessage(message, MSGBUF_MAX, pattern);
	logger->log(zLevel::WARN->zlevel,message);
	msgMut.unlock();
}

/**
* \brief 写info程序日志
* \param  pattern 输出格式范例，与printf一样
* \return 无
*/
void zLogger::info(const char * pattern,...)
{
	msgMut.lock();
	getMessage(message, MSGBUF_MAX, pattern);
	logger->log(zLevel::INFO->zlevel,message);
	msgMut.unlock();
}

/**
* \brief 写debug程序日志
* \param  pattern 输出格式范例，与printf一样
* \return 无
*/
void zLogger::debug(const char * pattern,...)
{
	msgMut.lock();
	getMessage(message, MSGBUF_MAX, pattern);
	logger->log(zLevel::DEBUG->zlevel,message);
	msgMut.unlock();
	
}

void zLogger::alarm(const char * pattern,...)
{
        msgMut.lock();
	getMessage(message, MSGBUF_MAX, pattern);
	logger->log(zLevel::ALARM->zlevel,message);
	msgMut.unlock();
}

void zLogger::iffy(const char * pattern,...)
{

            msgMut.lock();
	            getMessage(message, MSGBUF_MAX, pattern);
		            logger->log(zLevel::IFFY->zlevel,message);
			            msgMut.unlock();
				        
}

void zLogger::trace(const char * pattern,...)
{

            msgMut.lock();
	            getMessage(message, MSGBUF_MAX, pattern);
		            logger->log(zLevel::TRACE->zlevel,message);
			            msgMut.unlock();
				        
}

void zLogger::gbug(const char * pattern,...)
{

            msgMut.lock();
	            getMessage(message, MSGBUF_MAX, pattern);
		            logger->log(zLevel::GBUG->zlevel,message);
			            msgMut.unlock();
				        
}


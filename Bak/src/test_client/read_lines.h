#ifndef READ_LINES_H
#define READ_LINES_H

#include "singleton.h"  // for Singleton<>

#include <mutex>
#include <queue>

// 线程安全的读取行。用于后台读取lua窗口输入。
class ReadLines : public Singleton<ReadLines>
{
public:
	void Push(const std::string& sLine);
	std::string Pop();
	bool IsEmpty();

private:
	std::mutex m_mtx;
	using LockGuard = std::lock_guard<std::mutex>;

	std::queue<std::string> m_queue;
};  // class ReadLine

#endif  // READ_LINES_H

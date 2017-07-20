#include "read_lines.h"

void ReadLines::Push(const std::string& sLine)
{
	LockGuard lg(m_mtx);
	m_queue.push(sLine);
}

std::string ReadLines::Pop()
{
	LockGuard lg(m_mtx);
	assert(!m_queue.empty());
	std::string s = m_queue.front();
	m_queue.pop();
	return s;
}

bool ReadLines::IsEmpty()
{
	LockGuard lg(m_mtx);
	return m_queue.empty();
}

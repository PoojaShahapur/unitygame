#ifndef _GMIPFITER_H_
#define _GMIPFITER_H_
#include "zType.h"
#include "zSingleton.h"
#include <list>
#include <set>
///////////////////////////////////////////////
//
//code[GmIPFilter.h] defination by codemokey
//
//
///////////////////////////////////////////////

class GmIPFilter : public Singleton<GmIPFilter>
{
    public:
        friend class SingletonFactory<GmIPFilter>;
        GmIPFilter();
        ~GmIPFilter();
	bool init();
	bool check(DWORD id, DWORD ip);
    private:
	std::list<std::pair<DWORD, DWORD> > filterList;
	typedef std::list<std::pair<DWORD, DWORD> >::iterator filter_iter;

	std::set<DWORD> exceptList;
	typedef std::set<DWORD>::iterator except_iter;

};

#endif //_GMIPFITER_H_


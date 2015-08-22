#ifndef _BASE_WORDFILTERMANAGER_H_
#define _BASE_WORDFILTERMANAGER_H_
#include "zType.h"
#include "zSingleton.h"
#include "zRegex.h"
#include <string>
#include <map>
///////////////////////////////////////////////
//
//code[base/WordFilterManager.h] defination by codemokey
//
//
///////////////////////////////////////////////

class WordFilterManager : public Singleton<WordFilterManager>
{
    public:
        friend class SingletonFactory<WordFilterManager>;
        WordFilterManager();
        ~WordFilterManager();

	bool filterWord(const char* src);
	bool loadFile();
	bool checkNewName(char* newName);
	bool doFilter(char* text, unsigned int len);
    private:
	char* escaspeRule(const char* src, char* dest);
	
	zRegex regex;
	
	std::map<std::string, std::string> forbidMaps;


};

#endif //_BASE_WORDFILTERMANAGER_H_


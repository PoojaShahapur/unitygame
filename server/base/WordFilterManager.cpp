#include "WordFilterManager.h"
#include "Zebra.h"
#include "zMisc.h"
#include "Command.h"
#include <fstream>
///////////////////////////////////////////////
//
//code[base/WordFilterManager.cpp] defination by codemokey
//
//
///////////////////////////////////////////////




WordFilterManager::WordFilterManager()
{
    forbidMaps.clear();
}

WordFilterManager::~WordFilterManager()
{
    forbidMaps.clear();
}

bool WordFilterManager::filterWord(const char* src)
{
    return true;
}

bool WordFilterManager::loadFile()
{
    std::string f = Zebra::global["forbidWords"];
    if("" == f)
	f = "Config/forbidWords.txt";
    std::ifstream src(f.c_str());
    if(!src)
    {
	Zebra::logger->error("[forbidWords]打开屏蔽词文件失败");
	return false;
    }
    char buf[256];
    bzero(buf, sizeof(buf));
    std::vector<std::string> vs;
    std::vector<std::string>::iterator word, replacer;
    while(src.getline(buf, sizeof(buf)))
    {
	vs.clear();
	Zebra::stringtok(vs, buf, "\t\r");
	if(vs.size() == 3)
	{
	    replacer = vs.begin();
	    replacer++;
	    word = replacer++;

	    bzero(buf, sizeof(buf));
	    word->copy(buf, word->length(), 0);
	    for(DWORD i=0; i<word->length() && i<(count_of(buf)-1); i++)
	    {
		buf[i] = tolower(buf[i]);
	    }
	    word->assign(buf);

	    forbidMaps[*word] = *replacer;
	}
    }
    Zebra::logger->debug("[forbidWords]加载屏蔽词 %u 个",forbidMaps.size());
    return true;
}

bool WordFilterManager::checkNewName(char* newName)
{
    //首先检查字母数字汉子编码信息

    return true;
}

bool WordFilterManager::doFilter(char* text, unsigned int len)
{
    zRegex regex;
    bool ret = true;

    char copy[MAX_CHATINFO+1];
    bzero(copy, sizeof(copy));
    strncpy(copy, text, MAX_CHATINFO);
    for(DWORD i=0; i<strlen(copy) && i<(count_of(copy)-1); i++)
    {
	copy[i] = tolower(copy[i]);
    }
    std::string content(text);
    std::string content_copy(copy);
    std::string::size_type pos = 0;
    std::map<std::string, std::string>::iterator it=forbidMaps.begin();
    for(; it!= forbidMaps.end(); ++it)
    {
	pos = content_copy.find(it->first.c_str(), 0);
	while(pos != std::string::npos)
	{
	    BYTE val = 0;
	    if(pos)
	    {
		val = content_copy.c_str()[pos-1]; //是否是汉子后面的一半
	    }

	    if(val >= 0x80)
	    {
		pos = content_copy.find(it->first.c_str(), pos+1);
	    }
	    else
	    {
		content.replace(pos, it->first.length(), it->second.c_str());
		content_copy.replace(pos, it->first.length(), it->second.c_str());

		pos = content_copy.find(it->first.c_str(), pos+it->first.length());
		ret = false;
	    }
	}
    }
    strncpy(text, content.c_str(), len);
    return ret;
}



/**
 * \brief ������ʽ�ඨ��
 *
 */
#include "zRegex.h"
#include <iostream>

const int zRegex::REG_UNKNOW(78325);
const int zRegex::REG_FLAGS(78326);
const int zRegex::REG_COMP(78327);
const int zRegex::REG_MATCH(78328);
const int zRegex::REG_MULTILINE(REG_NEWLINE);
const int zRegex::REG_DEFAULT(0);


zRegex::zRegex()
{
    compiled = false;
    matched = false;
    errcode = REG_UNKNOW;
}

zRegex::~zRegex()
{
    if(compiled)
    {
	regfree(&preg);
    }
}

bool zRegex::compile(const char* regex, int flags)
{
    if(compiled)
    {
	regfree(&preg);
	matched = false;
    }
    if(flags == REG_MULTILINE)
    {
	errcode = regcomp(&preg, regex, REG_EXTENDED);
    }
    else if(flags == REG_DEFAULT)
    {
	errcode = regcomp(&preg, regex, REG_EXTENDED | REG_NEWLINE);
    }
    else
	errcode = REG_FLAGS;
    compiled = (errcode == 0);
    return compiled;
}
/**
 * \brief �õ�ƥ������ַ���,�ڴ�֮ǰ�뱣֤�Ѿ���ȷ�ý���ƥ��#match
 *
 * \param s �õ����ַ���������s��
 * \param sub ���ַ�����λ�á�ע��ƥ����ַ���λ��Ϊ0,�������ַ����Դ�����.���ֵΪ31
 * \return ����s 
 */

bool zRegex::match(const char * s)
{
		if(NULL == s)
			return false;
    smatch = s;
    if(compiled)
    {
	errcode = regexec(&preg, s, 32, rgm, 0);
    }
    else
    {
	errcode = REG_COMP;
    }
    matched = (errcode == 0);
    return matched;
}

std::string &zRegex::getSub(std::string &s, int sub)
{
    if(matched)
    {
	if(sub<32 && sub >= 0 && rgm[sub].rm_so!=-1)
	{
	    s=std::string(smatch, rgm[sub].rm_so, rgm[sub].rm_eo-rgm[sub].rm_so);
	}
	else
	    s = "";
    }
    else
	errcode = REG_MATCH;
    return s;
}

/**
 * \brief �õ�������Ϣ 
 * \return ������#compile��#matchʱ����false,�����ô˵õ�������Ϣ
 */
const std::string & zRegex::getError()
{
  if (errcode==REG_UNKNOW)
    errstr="unknow error";
  else if (errcode==REG_FLAGS)
    errstr="flags error";
  else if (errcode==REG_COMP)
    errstr="uncompiled error";
  else if (errcode==REG_MATCH)
    errstr="unmatched error";
  else
  {
    const char* temp = "unknow";// temp;
    //regerror(errcode,&preg,temp,1023);
	errstr=std::string(temp);
  }
  return errstr;
}

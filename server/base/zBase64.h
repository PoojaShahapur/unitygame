/*************************************************************************
 Author: 
 Created Time: 2014年09月26日 星期五 09时53分27秒
 File Name: base/zBase64.h
 Description: 
 ************************************************************************/
#ifndef _zBase64_h_
#define _zBase64_h_

#include <crypt.h>
#include <string>

namespace Zebra
{
    extern void base64_encrypt(const std::string &input,std::string &output);
    extern void base64_decrypt(const std::string &input,std::string &output);
};
#endif


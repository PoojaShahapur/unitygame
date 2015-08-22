/*************************************************************************
 Author: 
 Created Time: 2014年10月08日 星期三 14时11分42秒
 File Name: base/zArg.h
 Description: 
 ************************************************************************/
#ifndef _ZARG_H_
#define _ZARG_H_

#include <argp.h>
#include "zProperties.h"
#include "zNoncopyable.h"

typedef error_t(*argsParser)(int key, char *arg, struct argp_state *state);

class zArg : private zNoncopyable
{
    friend error_t zparse_opt(int, char*, struct argp_state *);
    protected:
    struct argp argp;
    argsParser user_parser;
    zArg();
    ~zArg();
    private:
    static zArg * argInstance;
    void addOptions(const struct argp_option *options);
    struct argp_option *alloptions;
    public:
    static zArg *getArg();
    static void removeArg();
    bool add(const struct argp_option *options=0,
	    argsParser func=0,
	    const char *args_doc=0,
	    const char *doc=0);
    bool parse(int argc, char *argv[]);
};
#endif


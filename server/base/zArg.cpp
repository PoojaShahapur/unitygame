/*************************************************************************
 Author: 
 Created Time: 2014年10月08日 星期三 14时20分29秒
 File Name: base/zArg.cpp
 Description: 
 ************************************************************************/
#include "zArg.h"
#include "Zebra.h"

const char *argp_program_bug_address = "test@163.com";

static const char zebra_args_doc[] = "";

static const char zebra_doc[] = "This is the default doc";

static struct argp_option zebra_options[] =
{
    {0, 0, 0, 0, 0, 0}
};

error_t zparse_opt(int key, char *arg, struct argp_state *state)
{
    switch(key)
    {
	default:
	    {
		if(zArg::getArg()->user_parser != 0)
		    return zArg::getArg()->user_parser(key, arg, state);
		else
		    return ARGP_ERR_UNKNOWN;
	    }
	    break;
    }
    return 0;
}

zArg *zArg::argInstance(0);

zArg::zArg()
{
    user_parser = 0;
    alloptions  = 0;
    argp.children = 0;
    argp.help_filter = 0;
    argp.argp_domain = 0;
    argp.parser = zparse_opt;
    argp.args_doc = zebra_args_doc;
    argp.doc = zebra_doc;
    addOptions(zebra_options);
}

zArg::~zArg()
{
    SAFE_DELETE_VEC(alloptions);
}

zArg *zArg::getArg()
{
    if(argInstance == 0)
	argInstance = new zArg();
    return argInstance;
}

void zArg::removeArg()
{
    SAFE_DELETE(argInstance);
}

void zArg::addOptions(const struct argp_option *options)
{
    if(options == 0) return;
    int ucount = 0;
    while(options[ucount].name != 0)
	ucount++;
    if(alloptions == 0)
    {
	alloptions = new struct argp_option[ucount+1];
	memcpy(alloptions, options, sizeof(argp_option)*(ucount+1));
    }
    else
    {
	int ocount = 0;
	while(alloptions[ocount].name != 0)
	    ocount++;
	struct argp_option *otemp = alloptions;
	alloptions = new argp_option[ucount+ocount+1];
	if(ocount > 0)
	    memcpy(alloptions, otemp, sizeof(argp_option)*ocount);
	memcpy(alloptions+ocount, options, sizeof(argp_option)*(ucount+1));
	SAFE_DELETE(otemp);
    }
    argp.options = alloptions;
}

bool zArg::add(const struct argp_option *options,argsParser func,const char *args_doc,const char *doc)
{
    if(func != 0)
	user_parser = func;
    if(options != 0)
	addOptions(options);
    if(args_doc != 0)
	argp.args_doc = args_doc;
    if(doc != 0)
	argp.doc = doc;
    return true;

}

bool zArg::parse(int argc, char *argv[])
{
    argp_parse(&argp, argc, argv, 0, 0, 0);
    return true;
}

#include <argp.h>
#include <fstream>
#include "Zebra.h"
using namespace std;

const char* argp_program_version = "xmlpg - 1.0";
const char* argp_program_bug_address = "<wangchong-fly@163.com>";
static char doc[] =
"this is a Description for the xmlpg!\n"
"	\n"
"	\n"
"	\n"
"	\n"
"	\n"
"	\n"
"	\n"
"	\n"
"	\n";

static char args_doc[] = "FILE";
static struct argp_option options[] =
{
    {0,		0,	0,	0,	"options:\n"},
    {"output",	'o',	"FILE", 0,	"create the .h file"},
    {"cppoutput",	'c',	"FILE", 0,	"create the .cpp file"},
    {0, 0,	0,	0,	
	"example:\n\n"
	    "	1. tool className\n"
	    "	2. tool className -o cfg.h\n"
	    "	7. tool className -c cfg.cpp\n"
    },

    {0,		0,	0,	0,	0}
};

struct Args
{
    Args()
    {
	xmlfile = NULL;
	outputfile = NULL;
	outputfile2 = NULL;
    }
    char* xmlfile;
    char* outputfile;
    char* outputfile2;
};

static error_t parse_opt(int key, char* arg, struct argp_state* state)
{
    struct Args* args = (struct Args*)state->input;
    switch(key)
    {
	case 'o':
	    args->outputfile = arg;
	    break;
	case 'c':
	    args->outputfile2 = arg;
	    break;
	case ARGP_KEY_ARG:
	    args->xmlfile = arg;
	    break;
	case ARGP_KEY_END:
	    if(state->arg_num < 1)
		argp_usage(state);
	    break;
	default:
	    return 0;

    }
    return 0;
}

static struct argp xmlpg_argp = 
{
    options, parse_opt, args_doc, doc
};

bool run(int argc, char** argv)
{
    Args args;
    argp_parse(&xmlpg_argp, argc, argv, 0, 0, &args);
    if(args.xmlfile)
    {
	if(args.outputfile)
	{
	    std::string tmp = args.outputfile;
	    Zebra::replace_all(tmp, ".","_");
	    Zebra::replace_all(tmp, "/","_");
	    Zebra::replace_all(tmp, "\\","_");
	    Zebra::to_upper(tmp);

	    tmp = "_" + tmp + "_";

	    std::ofstream of(args.outputfile);
	    of<< "#ifndef "<<tmp<<std::endl;
	    of<< "#define "<<tmp<<std::endl;
	    of<< "#include \"zType.h\""<<std::endl;
	    of<< "#include \"zSingleton.h\""<<std::endl;


	    of<< "///////////////////////////////////////////////"<<std::endl;
	    of<< "//"<<std::endl;
	    of<< "//"<<"code["<<args.outputfile<<"] defination by codemokey"<<std::endl;
	    of<< "//"<<std::endl;
	    of<< "//"<<std::endl;
	    of<< "///////////////////////////////////////////////"<<std::endl;
	    of<<std::endl;
	    
	    of<<"class "<<args.xmlfile<<" : public Singleton<"<<args.xmlfile<<">"<<std::endl;
	    of<<"{"<<std::endl;
	    of<<"    public:"<<std::endl;
	    of<<"        friend class SingletonFactory<"<<args.xmlfile<<">;"<<std::endl;
	    of<<"        "<<args.xmlfile<<"();"<<std::endl;
	    of<<"        ~"<<args.xmlfile<<"();"<<std::endl;
	    of<<"};"<<std::endl;
	    of<<std::endl;
	    of<< "#endif //" <<tmp <<"\n"<<std::endl;

	    cout<<std::endl<<"All is done!!!"<<std::endl<<std::endl;
	    cout<<"origin config:"<<args.xmlfile<<std::endl;
	    cout<<"the new code file:"<<args.outputfile<<std::endl;
	}
	else if(args.outputfile2)
	{
	    std::ofstream of(args.outputfile2);
	    of<< "#include \""<< args.xmlfile <<".h\""<<std::endl;
	    of<< "///////////////////////////////////////////////"<<std::endl;
	    of<< "//"<<std::endl;
	    of<< "//"<<"code["<<args.outputfile2<<"] defination by codemokey"<<std::endl;
	    of<< "//"<<std::endl;
	    of<< "//"<<std::endl;
	    of<< "///////////////////////////////////////////////"<<std::endl;
	    of<<std::endl;
	    of<<std::endl;
	    of<<std::endl;
	    of<<std::endl;

	    of<<args.xmlfile<<"::"<<args.xmlfile<<"()"<<std::endl;
	    of<<"{}"<<std::endl;
	    of<<std::endl;
	    of<<args.xmlfile<<"::~"<<args.xmlfile<<"()"<<std::endl;
	    of<<"{}"<<std::endl;

	    cout<<std::endl<<"All is done!!!"<<std::endl<<std::endl;
	    cout<<"origin config:"<<args.xmlfile<<std::endl;
	    cout<<"the new code file:"<<args.outputfile2<<std::endl;

	}
    }
    return true;
}
int main(int argc, char* argv[])
{
	return run(argc, argv);
}

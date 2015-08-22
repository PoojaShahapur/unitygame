#include "../base/xml/xml_parser_generator.h"
#include "../base/zXMLParser.h"
#include <argp.h>
#include <fstream>
using namespace std;

const char* argp_program_version = "xmlpg - 1.0";
const char* argp_program_bug_address = "<test@163.com>";
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
    {"output",	'o',	"FILE", 0,	"create the cpp file"},
    {"memfunc", 'm',	NULL,	0,	"member function "},
    {"indent",	'i',	"N",	0,	"default is 0 "},
    {"parse",	'p',	NULL,	0,	"just parse.for check xml"},
    {"dump",	'd',	NULL,	0,	"parse and dump "},
    {"nodename",'n',	"NODE",	0,	"rootnode name(default is node xmlpg_)  "},
    {"wholefile", 'f',	0,	0,	"xxxxxxxxxxxxxxxxxxxxxxx "},
    {0, 0,	0,	0,	
	"example:\n\n"
	    "	1. tool cfg.xml\n"
	    "	2. tool cfg.xml -o cfg.h\n"
	    "	3. tool cfg.xml -m \n"
	    "	4. tool cfg.xml -d\n"
	    "	5. tool cfg.xml -n\n"
	    "	6. tool cfg.xml -f\n"
    },

    {0,		0,	0,	0,	0}
};

struct Args
{
    Args()
    {
	xmlfile = NULL;
	outputfile = NULL;
	layout = 0;
	indent = 0;
	parse_flag = 0;
	dump_flag = 0;
	generator_root_nodename = NULL;
	wholefile_flag = 0;
    }
    char* xmlfile;
    char* outputfile;
    int layout;
    int indent;
    int parse_flag;
    int dump_flag;
    char* generator_root_nodename;
    int wholefile_flag;
};

static error_t parse_opt(int key, char* arg, struct argp_state* state)
{
    struct Args* args = (struct Args*)state->input;
    switch(key)
    {
	case 'o':
	    args ->outputfile = arg;
	    break;
	case 'm':
	    args->layout = 1;
	    break;
	case 'i':
	    args->indent = atol(arg);
	    break;
	case 'p':
	    args->parse_flag = 1;
	    break;
	case 'd':
	    args->dump_flag = 1;
	    break;
	case 'n':
	    args->generator_root_nodename = arg;
	    break;
	case 'f':
	    args->wholefile_flag = 1;
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
	zXMLParser xml;
	if(!xml.initFile(args.xmlfile))
	{
	    cout<<"file name :"<<args.xmlfile<<" maybe wrong!!"<<endl;
	    return false;
	}
	xmlNodePtr root = xml.getRootNode(NULL);
	if(!root)
	{
	    cout<<"root node error!!!"<<endl;
	    return false;
	}
	xml::xml_node_parser_generator generator;
	xmlNodePtr gnode = root;
	if(args.wholefile_flag)
	{
	    gnode = root;
	}
	else if(args.generator_root_nodename)
	{
	    std::vector<std::string> nodenames;
	    Zebra::stringtok(nodenames, args.generator_root_nodename, ".");
	    for(size_t i = 0; i<nodenames.size(); ++i)
	    {
		gnode = xml.getChildNode(gnode, nodenames[i].c_str());
		if(!gnode)
		{
		    cout<<"there is no node ERROR!!!"<<endl;
		    return false;
		}
	    }
	}
	else
	{
	    gnode = xml.getChildNode(root, "xmlpg_");
	    if(!gnode)
	    {
		cout<<endl<<"It is no \"xmlpg_\" node!!!"<<endl;
		return false;
	    }
	}
	generator.parse(xml, gnode);
	if(args.parse_flag)
	{
	    cout<<endl<<"parse is Ok!!!"<<endl;
	    return true;
	}
	if(args.dump_flag)
	{
	    generator.dump(cout);
	    return true;
	}
	if(args.outputfile)
	{
	    std::string tmp = args.outputfile;
	    Zebra::replace_all(tmp, ".","_");
	    Zebra::replace_all(tmp, "/","_");
	    Zebra::replace_all(tmp, "\\","_");
	    Zebra::to_upper(tmp);

	    tmp = "_XML_" + tmp;

	    std::ofstream of(args.outputfile);
	    of<< "#ifndef "<<tmp<<std::endl;
	    of<< "#define "<<tmp<<std::endl;
	    of<< "#include \"zType.h\""<<std::endl;

	    of<< "///////////////////////////////////////////////"<<std::endl;
	    of<< "//"<<std::endl;
	    of<< "//"<<"config["<<args.xmlfile<<"] defination by codemokey"<<std::endl;
	    of<< "//"<<std::endl;
	    of<< "// notice:this file was created by a tool,please do not modify"<<std::endl;
	    of<< "//"<<std::endl;
	    of<< "///////////////////////////////////////////////"<<std::endl;
	    of<<std::endl;

	    generator.generate(of, args.indent, args.layout);

	    of<<std::endl;
	    of<< "#endif //" <<tmp <<"\n"<<std::endl;

	    cout<<std::endl<<"All is done!!!"<<std::endl<<std::endl;
	    cout<<"origin config:"<<args.xmlfile<<std::endl;
	    cout<<"the new code file:"<<args.outputfile<<std::endl;
	    cout<<"the new class name:"<<generator.structname <<std::endl;
	}
	else
	{
	    generator.generate(cout, args.indent, args.layout);
	}
    }
    return true;
}
int main(int argc, char* argv[])
{
	return run(argc, argv);
}

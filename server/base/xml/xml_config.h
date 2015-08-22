#ifndef _XML_XML_CONFIG_H_
#define _XML_XML_CONFIG_H_

#include "./common.h"
#include "./xml_node.h"
XML_NAMESPACE_BEGIN

struct xml_config_base
{
	virtual ~xml_config_base() {}
	virtual void dynamic_dump(std::ostream& os, int deep = 0) const {}
	virtual bool dynamic_load() {return true;}
	virtual bool dynamic_load(const std::string& filename) {return true;}
	virtual bool dynamic_copy(xml_config_base* config) {return true;}
	virtual xml_config_base* clone() {return NULL;}
	virtual const std::string getFileName() const {return "unkown";}
};

template <typename Config>
class xml_config : public xml_node<Config>, public xml_config_base
{
	public:
		typedef xml_config<Config> this_type;
		xml_config(const std::string& filename = ""):_filename(filename)
		{}
		bool load(const std::string& filename)
		{
			zXMLParser xml;
			if(!xml.initFile(filename))
				return false;
			xmlNodePtr root = xml.getRootNode(NULL);
			if(!root)
				return false;
			this->name = (char*)root->name;
			this->_filename = filename;
			return xml_node<Config>::parse(xml, root, true);
		}
		bool load()
		{
			return load(getFileName());
		}
		bool reload()
		{
			xml_node<Config>::init_value();
			return load();
		}
		bool reload(const std::string& filename)
		{
			xml_node<Config>::init_value();
			return load(filename);
		}
		virtual void dynamic_dump(std::ostream& os, int deep=0) const
		{
			this->dump(os, deep);
		}
		virtual bool dynamic_load()
		{
			return this->reload();
		}
		virtual bool dynamic_load(const std::string& filename)
		{
			return this->reload(filename);
		}
		virtual xml_config_base* clone()
		{
			return new xml_config<Config>(getFileName());
		}
		virtual bool dynamic_copy(xml_config_base* config)
		{
			this_type* tmp = dynamic_cast<this_type*>(config);
			if(!tmp) return false;
			*this = *tmp;
			return true;
		}
		virtual const std::string getFileName() const {return _filename;}
	private:
		std::string _filename;
};

XML_NAMESPACE_END
namespace std
{
	template<typename Config>
	inline std::ostream& operator << (std::ostream& os, const xml::xml_config<Config>& config)
	{
		config.dump(os);
		return os;
	}
}
#endif


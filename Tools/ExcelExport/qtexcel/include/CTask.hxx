#ifndef _CTASK_H
#define _CTASK_H

#include<string>	// include string
#include<vector>	// include vector
#include "tinyxml.h"	// include xml

// import namespace
using namespace std;

/**
 * @brief package a table
 */
class CPackage
{
protected:
	string m_xml;		// xml path + name
	string m_output;	// output path

public:
	CPackage();
	~CPackage();

	string getXml();
	string getOutput();

	void setXml(string xml);
	void setOutput(string output);
	void initByXml(TiXmlElement* elem);
	void destroy();
};


/**
 * @brief one solution
 */
class CSolution
{
protected:
	string m_name;		// solution name
	string m_cmd;		// excute cmd, usually is a bat file
	vector<CPackage*> m_lstPack;	// need pack list

public:
	CSolution();
	~CSolution();

	string getName();
	string getCmd();

	void setName(string name);
	void setCmd(string cmd);
	void initByXml(TiXmlElement* elem);
	vector<CPackage*>& getPackLst();
	void destroy();
};

/**
 * @brief task
 */
class CTask
{
protected:
	vector<CSolution> m_lstSolution;

public:
	CTask();
	~CTask();

	vector<CSolution>& getSolutionLst();
	void readXML();		// read config.xml
	void destroy();
};

#endif
#ifndef _CSortM_h_
#define _CSortM_h_
#include "zType.h"
#include <map>
#include "Session.h"

class CSortM
{
  private:

    CSortM();

    static CSortM *csm;
    //WORD leveltable[MAX_LEVEL+10];

    /**
     * \brief 排序
     */
    struct ltqword
    {
      bool operator()(const QWORD s1,const QWORD s2) const
      {
        return s1>s2;
      }
    };
    std::multimap<QWORD,DWORD,ltqword> _sortKey;
    std::map<DWORD,QWORD> _sortMap;

    typedef std::multimap<QWORD,DWORD,ltqword>::value_type keyValueType;
    typedef std::map<DWORD,QWORD>::value_type mapValueType;

  public:
    ~CSortM();
    static CSortM &getMe();
    bool init();
    static void destroyMe();
    void onlineCount(UserSession *pUser);
    void onlineCount(DWORD dwCharID,WORD wdLevel,QWORD qwExp);
    void offlineCount(UserSession *pUser);
    WORD getLevelDegree(UserSession *pUser);
    void upLevel(UserSession *pUser);
    bool createDBRecord();
    bool clearDBTable();
    void sendSortList(UserSession* lpUser);
};
#endif

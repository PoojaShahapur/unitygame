'''
Created on 2015年2月13日

@author: {Administrator}
'''

from CPP2CSharp.CPPParse import CppItemBase
from CPP2CSharp.Core.Utils import Utils

class CppStructItem(CppItemBase):
    '''
    classdocs
    '''


    def __init__(self):
        '''
        Constructor
        '''
        super(CppStructItem, self).__init__(CppItemBase.CppItemBase.eCppStructItem)
        
        
    def parseCppElem(self, strParam):
        super.parseCppElem(strParam)
        while len(strParam.m_fileStr):
            oneToken = Utils.getToken(strParam)
            if oneToken[0:2] == "*/":
                break
            
            
            
            
            
            
            

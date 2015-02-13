'''
Created on 2015年2月13日

@author: Administrator
'''

class CppItemBase(object):
    '''
    classdocs
    '''
    eCppClassItem = 0,
    eCppNSItem = 1,
    eCppStructItem = 2,
    

    def __init__(self, cppelemtype):
        '''
        Constructor
        '''
        self.m_cppElemType = cppelemtype


    def parseCppElem(self, strParam):
        strParam.lstrip(' \n')          # 删除左边的空格
        pass;
    
    
    
    
    
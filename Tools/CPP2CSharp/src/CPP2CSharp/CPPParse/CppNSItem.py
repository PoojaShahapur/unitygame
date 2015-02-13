'''
Created on 2015年2月13日

@author: Administrator
'''

from CPP2CSharp.CPPParse import CppItemBase

class CppNSItem(CppItemBase):
    '''
    classdocs
    '''


    def __init__(self, params):
        '''
        Constructor
        '''
        super(CppNSItem, self).__init__(CppItemBase.CppItemBase.eCppNSItem)
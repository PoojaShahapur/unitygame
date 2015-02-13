'''
Created on 2015年2月13日

@author: luguhu
'''

class PropertyBase(object):
    '''
    classdocs
    '''
    
    ePropertyString = 0
    ePropertyUInt32 = 1
    ePropertyInt32 = 2
    ePropertyUInt16 = 3
    ePropertyInt16 = 4
    ePropertyUInt8 = 5
    ePropertyInt8 = 6
    

    def __init__(self, cppPropertyType):
        '''
        Constructor
        '''
        self.m_cppPropertyType = cppPropertyType
        
        
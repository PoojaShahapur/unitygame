'''
Created on 2015年2月1日

@author: luguhu
'''

from CPP2CSharp.Core.Logger import Logger

class CodeConv(object):
    '''
    classdocs
    '''
        
    pInstance = None
    
    @staticmethod
    def instance():
        if CodeConv.pInstance is None:
            CodeConv.pInstance = CodeConv()
        return CodeConv.pInstance


    def __init__(self):
        '''
        Constructor
        '''
        pass
    
    
    # 转换 dirName 目录名字中的文件  fileName 
    def convCpp2CSharp(self, filePathName):
        Logger.instance().info(filePathName)
        pass
    
    
    
    
    
    
    
    
    
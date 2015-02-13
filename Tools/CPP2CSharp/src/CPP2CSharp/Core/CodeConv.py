'''
Created on 2015年2月1日

@author: luguhu
'''

from CPP2CSharp.Core.Config import Config
from CPP2CSharp.Core.Logger import Logger
from CPP2CSharp.CPPParse import CppFile

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
    def convCpp2CSharp(self, fineName, filePathName):
        if fineName in Config.pInstance.m_fileNameList:
            Logger.instance().info(filePathName)
            
            cppFile = CppFile.CppFile()
            cppFile.parseCpp(filePathName)
                
                    
                
            
                    
                
            
        
    
    
    
    
    
    
    
    
    
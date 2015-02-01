#-*- encoding=utf-8 -*-
'''
Created on 2015-2-1

@author: Administrator
'''

#import subprocess
import sys
import traceback
import os
import shutil
from CPP2CSharp.Core.Logger import Logger

'''
parameter info
'''
class ParamInfo(object):
    
    pInstance = None
    
    @staticmethod
    def instance():
        if ParamInfo.pInstance is None:
            ParamInfo.pInstance = ParamInfo()
        return ParamInfo.pInstance
    
    def __init__(self):
        self.m_swiftFullXmlFile = ''
        self.m_swiftFullSwcFile = ''
    

class FileOperate(object):
    @staticmethod
    def copyFile(srcfilename, destfilename):
        if os.path.isfile(srcfilename):
            try:
                shutil.copyfile(srcfilename, destfilename)
                Logger.instance().info("copy file success: " + srcfilename)
            except:
                # 错误输出
                Logger.instance().info("copy file error: " + srcfilename)
                typeerr, value, tb = sys.exc_info()
                errstr = traceback.format_exception(typeerr, value, tb)
                Logger.instance().info(errstr)

        else:
            Logger.instance().info("cannot find file: " + srcfilename)



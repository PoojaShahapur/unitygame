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
from CPP2CSharp.Core.CodeConv import CodeConv


class Utils(object):
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

    # 获取字符串的编码
    @staticmethod
    def str2Byte(self):
        #self.subversion.encode("utf-8")
        return bytes(self.subversion, encoding = "utf8")


    # 遍历目录
    @staticmethod
    def traverseDirs(rootDir):
        for root, dirs, files in os.walk(rootDir):
            Logger.instance().info(''.join(dirs))
            Utils.traverseOneDirs(root, files)
    
    
    #一个目录的 md5 码
    @staticmethod
    def traverseOneDirs(directoryName, filesInDirectory):
        Logger.instance().info(directoryName)
        for fname in filesInDirectory:
            fpath = os.path.join(directoryName, fname)
            if not os.path.isdir(fpath):
                CodeConv.pInstance.convCpp2CSharp(fpath)
    
    
    @staticmethod
    def joinPath(path, file):
        return os.path.join(path, file)
    
    @staticmethod
    def makeDir(path):
        # 检查目录
        if not os.path.exists(path):
            os.makedirs(path)




















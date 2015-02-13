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
                Logger.instance().info(''.join(errstr))

        else:
            Logger.instance().info("cannot find file: " + srcfilename)

    # 获取字符串的编码
    @staticmethod
    def str2Byte(self):
        #self.subversion.encode("utf-8")
        return bytes(self.subversion, encoding = "utf8")


    # 遍历目录
    @staticmethod
    def traverseDirs(rootDir, handleDisp):
        for root, dirs, files in os.walk(rootDir):
            Logger.instance().info(''.join(dirs))
            Utils.traverseOneDirs(root, files, handleDisp)
    
    
    #一个目录的 md5 码
    @staticmethod
    def traverseOneDirs(directoryName, filesInDirectory, handleDisp):
        Logger.instance().info(directoryName)
        for fname in filesInDirectory:
            fpath = os.path.join(directoryName, fname)
            if not os.path.isdir(fpath):
                if handleDisp is not None:
                    handleDisp(fname, fpath)
    
    
    @staticmethod
    def joinPath(path, file):
        return os.path.join(path, file)
    
    @staticmethod
    def makeDir(path):
        # 检查目录
        if not os.path.exists(path):
            os.makedirs(path)
            
    
    @staticmethod        
    def logStackInfo():
        typeerr, value, tb = sys.exc_info()
        errstr = traceback.format_exception(typeerr, value, tb)
        Logger.instance().info(''.join(errstr))


    # 从字符串的左边获取一个符号，并且删除这个符号
    @staticmethod
    def getToken(strParam):
        Utils.skipSpaceAndBr(strParam)
        
        idx = 0;
        ret = ''
        while idx < len(strParam.m_fileStr):
            if strParam.m_fileStr[idx] == ' ' or strParam.m_fileStr[idx] == '\n' or strParam.m_fileStr[idx] == '\t':       # 如果遇到空格或者换行符，就算是一个符号
                break 
            ret += strParam.m_fileStr[idx]
            idx += 1
            
        if len(ret):
            strParam.m_fileStr = strParam.m_fileStr[idx:]         # 删除内容
            
        Utils.skipSpaceAndBr(strParam)
        
        return ret
        

    # 跳过当前行
    @staticmethod
    def skipCurLine(strParam):
        idx = 0;
        while idx < len(strParam.m_fileStr):
            if strParam.m_fileStr[idx] == '\n':
                break;
            idx += 1

        strParam.m_fileStr = strParam.m_fileStr[idx:]         # 删除内容
        

    # 跳过空格和换行
    @staticmethod
    def skipSpaceAndBr(strParam):
        idx = 0;
        while idx < len(strParam.m_fileStr):
            if strParam.m_fileStr[idx] != ' ' and strParam.m_fileStr[idx] != '\n' and strParam.m_fileStr[idx] != '\t':       # 如果遇到空格或者换行符，就算是一个符号
                break;
            idx += 1

        strParam.m_fileStr = strParam.m_fileStr[idx:]         # 删除内容


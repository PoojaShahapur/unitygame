#-*- encoding=utf-8 -*-
'''
Created on 2015-2-1

@author: Administrator
'''

#import os
#import datetime
#import time

# base config info
class Config(object):
    
    pInstance = None
    
    @staticmethod
    def instance():
        if Config.pInstance is None:
            Config.pInstance = Config()
        return Config.pInstance
    
    def __init__(self):
        assert(Config.pInstance is None)
        Config.pInstance = self
        
        # 注意全部需要初始化，否则如果配置文件不用，并且没有判断是否有这个属性，就会出问题
        self.m_srcCodePath = ""       # 源代码所在目录
        self.m_fileNameList = []
        self.m_tmpCodePath = ""  # 临时代码目录
        self.m_destCodePath = ""      # dest 代码目录

        #读取初始化数据
    def readInit(self, filename):
        # gbk , gk2312
        with open(filename, 'r', encoding = 'utf8') as fHandle:
            cont = fHandle.read()
            strlist = cont.split('\n')
            idx = 0
            substrList = []
            listlen = len(strlist)
            while idx < listlen:
                substrList = strlist[idx].split('=')
                if len(substrList[0]):
                    if substrList[0] == 'm_fileNameList':
                        if len(substrList[1]) > 0:
                            fileList = substrList[1].split(',')
                            idxList = 0
                            while idxList < len(fileList):
                                self.m_fileNameList.append(fileList[idxList])
                                idxList += 1
                    else:    
                        self.__dict__[substrList[0]] = substrList[1]
                idx += 1


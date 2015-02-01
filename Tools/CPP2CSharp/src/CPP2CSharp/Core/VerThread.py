#-*- encoding=utf-8 -*-
'''
Created on 2015-2-1

@author: Administrator
'''

from threading import Thread
import os

from CPP2CSharp.Core.AppData import AppData

from CPP2CSharp.Core.Config import Config
from CPP2CSharp.Core.Logger import Logger

class VerThread(Thread):
    
    def __init__(self, threadName, func):
        super(VerThread, self).__init__(name = threadName)  # must add
        self.m_runF = func

    def run(self):
        if self.m_runF is not None:
            self.m_runF()

    @staticmethod
    def outVerSwf():
        AppData.instance().m_bOverVer = False
        # 检查目录
        if not os.path.exists(os.path.join(Config.instance().destrootpath,  Config.instance().tmpDir)):
            os.makedirs(os.path.join(Config.instance().destrootpath,  Config.instance().tmpDir))
        
        if not os.path.exists(os.path.join(Config.instance().destrootpath,  Config.instance().outDir)):
            os.makedirs(os.path.join(Config.instance().destrootpath,  Config.instance().outDir))
        
        # 生成 app 文件，这个需要放在生成  versionall.swf 之后，因为需要 versionall.swf 的 md5 ，决定是否重新加载 versionall.swf 
        #AppData.instance().buildAppMd()
        
        # 生成所有的 md5 
        AppData.instance().curmd5FileCount = 0
        AppData.instance().buildAllMd()
        
        # 如果计算文件夹 md5 的时候，才需要计算路径
        if Config.instance().getfoldermd5cmp():
            AppData.instance().buildModuleMd()
            AppData.instance().buildUIMd()
        AppData.instance().closemdfile()

        # 生成版本文件
        AppData.instance().curverFileCount = 0

        
        Logger.instance().info("可以拷贝生成文件到目标文件夹了")
        AppData.instance().m_bOverVer = True

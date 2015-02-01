#-*- encoding=utf-8 -*-
'''
Created on 2015-2-1

@author: Administrator
'''

import os

from CPP2CSharp.Core.ThreadWrap import ThreadWrap
from CPP2CSharp.Core.AppData import AppData

from CPP2CSharp.Core.Config import Config
from CPP2CSharp.Core.Logger import Logger

class ConvThread(ThreadWrap):
    
    def __init__(self, threadName, func):
        super(ConvThread, self).__init__(name = threadName)  # must add
        self.m_runF = func

    def run(self):
        AppData.instance().m_bConvOver = False
        Logger.instance().info("File Conv Start")
        
        # 检查目录
        if not os.path.exists(os.path.join(Config.instance().destrootpath,  Config.instance().tmpDir)):
            os.makedirs(os.path.join(Config.instance().destrootpath,  Config.instance().tmpDir))
        
        if not os.path.exists(os.path.join(Config.instance().destrootpath,  Config.instance().outDir)):
            os.makedirs(os.path.join(Config.instance().destrootpath,  Config.instance().outDir))
        
        Logger.instance().info("File Conv End")
        AppData.instance().m_bConvOver = True



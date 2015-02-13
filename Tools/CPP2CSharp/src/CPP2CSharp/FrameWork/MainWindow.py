# -*- coding: utf-8 -*-
'''
Created on 2015-2-1

@author: Administrator
@brief mainwindow
'''

#from PyQt5 import  QtWidgets, QtCore, QtGui
from PyQt5 import  QtWidgets, QtCore

import CPP2CSharp.UI.ui_mainwindow
from CPP2CSharp.FrameWork import LoggerWin, LeftFnWin
from CPP2CSharp.Core import Config, AppData
from CPP2CSharp.Core.Logger import Logger

from CPP2CSharp.Core.IAppData import IAppData
from CPP2CSharp.Core import CodeConv

class MainWindow(QtWidgets.QMainWindow):
    
    instance = None
    
    def __init__(self, app):
        # make sure multiple instantiation won't happen
        assert(MainWindow.instance is None)
        MainWindow.instance = self

        super(MainWindow, self).__init__()

        self.app = app
        
        self.ui = CPP2CSharp.UI.ui_mainwindow.Ui_MainWindow()
        self.ui.setupUi(self)
        
        # 可停靠的窗口
        self.m_LoggerWin = LoggerWin.LoggerWin()
        self.addDockWidget(QtCore.Qt.BottomDockWidgetArea, self.m_LoggerWin)
        
        # 左边可停靠窗口
        self.m_LeftFnWin = LeftFnWin.LeftFnWin()
        self.addDockWidget(QtCore.Qt.LeftDockWidgetArea, self.m_LeftFnWin)
        
        # 显示初始化单件
        Config.Config.instance()
        Config.Config.instance().readInit('config.txt')

        # 实例化,共享数据
        IAppData.pInstance = AppData.AppData()
        Logger.instance()
        CodeConv.CodeConv.instance()
        
        self.m_qttimer = QtCore.QTimer()
        self.m_qttimer.timeout.connect(self.onTimer)
        self.m_qttimer.start( 1000 )

    def onTimer(self):
        listdata = []
        Logger.instance().getlogger(listdata)
        for dataitem in listdata:
            self.m_LoggerWin.ui.textEdit.appendPlainText(dataitem)



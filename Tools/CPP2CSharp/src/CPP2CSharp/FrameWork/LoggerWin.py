# -*- coding: utf-8 -*-
'''
Created on 2015-2-1

@author: Administrator
'''

from PyQt5 import QtWidgets
import CPP2CSharp.UI.ui_loggerwin

class LoggerWin(QtWidgets.QDockWidget):    
    def __init__(self):
        super(LoggerWin, self).__init__()

        self.ui = CPP2CSharp.UI.ui_loggerwin.Ui_LoggerWin()
        self.ui.setupUi(self)

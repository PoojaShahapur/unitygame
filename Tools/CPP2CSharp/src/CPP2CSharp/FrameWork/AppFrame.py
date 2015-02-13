# -*- coding: utf-8 -*-
'''
Created on 2015-2-1

@author: Administrator
@brief 自己的程序框架
'''

from PyQt5.QtWidgets import QApplication
import sys

class AppFrame(QApplication):
    def __init__(self):
        super(AppFrame, self).__init__(sys.argv)
        
        from CPP2CSharp.FrameWork import MainWindow

        self.mainWindow = MainWindow.MainWindow(self)
        self.mainWindow.show()
        self.mainWindow.raise_()

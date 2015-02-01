# -*- coding: utf-8 -*-

# Form implementation generated from reading ui file 'E:\work\client-05\trunk\tools\CPP2CSharp\CPP2CSharp\src\CPP2CSharp\ui\LoggerWin.ui'
#
# Created: Fri May 24 15:58:47 2013
#      by: PyQt4-uic 0.2.14 running on PyQt4 1.1.2
#
# WARNING! All changes made in this file will be lost!

from PyQt4 import QtCore, QtGui

class Ui_LoggerWin(object):
    def setupUi(self, LoggerWin):
        LoggerWin.setObjectName("LoggerWin")
        LoggerWin.resize(471, 300)
        self.verticalLayoutWidget = QtGui.QWidget()
        self.verticalLayoutWidget.setGeometry(QtCore.QRect(10, 0, 451, 271))
        self.verticalLayoutWidget.setObjectName("verticalLayoutWidget")
        self.verticalLayout = QtGui.QVBoxLayout(self.verticalLayoutWidget)
        self.verticalLayout.setObjectName("verticalLayout")
        self.textEdit = QtGui.QPlainTextEdit(self.verticalLayoutWidget)
        self.textEdit.setObjectName("textEdit")
        self.verticalLayout.addWidget(self.textEdit)
        LoggerWin.setWidget(self.verticalLayoutWidget)

        self.retranslateUi(LoggerWin)
        QtCore.QMetaObject.connectSlotsByName(LoggerWin)

    def retranslateUi(self, LoggerWin):
        LoggerWin.setWindowTitle(QtGui.QApplication.translate("LoggerWin", "logger", None, QtGui.QApplication.UnicodeUTF8))


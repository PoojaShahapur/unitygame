# -*- coding: utf-8 -*-

# Form implementation generated from reading ui file 'D:\file\opensource\unity-game-git\unitygame\unitygame\Tools\CPP2CSharp\src\CPP2CSharp\UI\LoggerWin.ui'
#
# Created: Sun Feb  1 16:46:28 2015
#      by: PyQt4 UI code generator 4.11.3
#
# WARNING! All changes made in this file will be lost!

from PyQt4 import QtCore, QtGui

try:
    _fromUtf8 = QtCore.QString.fromUtf8
except AttributeError:
    def _fromUtf8(s):
        return s

try:
    _encoding = QtGui.QApplication.UnicodeUTF8
    def _translate(context, text, disambig):
        return QtGui.QApplication.translate(context, text, disambig, _encoding)
except AttributeError:
    def _translate(context, text, disambig):
        return QtGui.QApplication.translate(context, text, disambig)

class Ui_LoggerWin(object):
    def setupUi(self, LoggerWin):
        LoggerWin.setObjectName(_fromUtf8("LoggerWin"))
        LoggerWin.resize(471, 300)
        self.verticalLayoutWidget = QtGui.QWidget()
        self.verticalLayoutWidget.setGeometry(QtCore.QRect(10, 0, 451, 271))
        self.verticalLayoutWidget.setObjectName(_fromUtf8("verticalLayoutWidget"))
        self.verticalLayout = QtGui.QVBoxLayout(self.verticalLayoutWidget)
        self.verticalLayout.setObjectName(_fromUtf8("verticalLayout"))
        self.textEdit = QtGui.QPlainTextEdit(self.verticalLayoutWidget)
        self.textEdit.setObjectName(_fromUtf8("textEdit"))
        self.verticalLayout.addWidget(self.textEdit)
        LoggerWin.setWidget(self.verticalLayoutWidget)

        self.retranslateUi(LoggerWin)
        QtCore.QMetaObject.connectSlotsByName(LoggerWin)

    def retranslateUi(self, LoggerWin):
        LoggerWin.setWindowTitle(_translate("LoggerWin", "logger", None))


# -*- coding: utf-8 -*-

# Form implementation generated from reading ui file 'D:\file\opensource\unity-game-git\unitygame\unitygame\Tools\CPP2CSharp\src\CPP2CSharp\UI\LeftFnWin.ui'
#
# Created: Sun Feb  1 16:46:24 2015
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

class Ui_LeftFnWin(object):
    def setupUi(self, LeftFnWin):
        LeftFnWin.setObjectName(_fromUtf8("LeftFnWin"))
        LeftFnWin.resize(235, 300)
        self.verticalLayoutWidget = QtGui.QWidget()
        self.verticalLayoutWidget.setObjectName(_fromUtf8("verticalLayoutWidget"))
        self.verticalLayout = QtGui.QVBoxLayout(self.verticalLayoutWidget)
        self.verticalLayout.setObjectName(_fromUtf8("verticalLayout"))
        self.m_btnCheck = QtGui.QPushButton(self.verticalLayoutWidget)
        self.m_btnCheck.setObjectName(_fromUtf8("m_btnCheck"))
        self.verticalLayout.addWidget(self.m_btnCheck)
        self.m_btnVersion = QtGui.QPushButton(self.verticalLayoutWidget)
        self.m_btnVersion.setObjectName(_fromUtf8("m_btnVersion"))
        self.verticalLayout.addWidget(self.m_btnVersion)
        self.m_btnVerSwf = QtGui.QPushButton(self.verticalLayoutWidget)
        self.m_btnVerSwf.setObjectName(_fromUtf8("m_btnVerSwf"))
        self.verticalLayout.addWidget(self.m_btnVerSwf)
        LeftFnWin.setWidget(self.verticalLayoutWidget)

        self.retranslateUi(LeftFnWin)
        QtCore.QMetaObject.connectSlotsByName(LeftFnWin)

    def retranslateUi(self, LeftFnWin):
        LeftFnWin.setWindowTitle(_translate("LeftFnWin", "function", None))
        self.m_btnCheck.setText(_translate("LeftFnWin", "Test Btn", None))
        self.m_btnVersion.setText(_translate("LeftFnWin", "Copy File", None))
        self.m_btnVerSwf.setText(_translate("LeftFnWin", "Conv File", None))


# -*- coding: utf-8 -*-

# Form implementation generated from reading ui file 'E:\work\client-05\trunk\tools\CPP2CSharp\CPP2CSharp\src\CPP2CSharp\ui\LeftFnWin.ui'
#
# Created: Fri May 24 15:58:47 2013
#      by: PyQt4-uic 0.2.14 running on PyQt4 1.1.2
#
# WARNING! All changes made in this file will be lost!

from PyQt4 import QtCore, QtGui

class Ui_LeftFnWin(object):
    def setupUi(self, LeftFnWin):
        LeftFnWin.setObjectName("LeftFnWin")
        LeftFnWin.resize(235, 300)
        self.verticalLayoutWidget = QtGui.QWidget()
        self.verticalLayoutWidget.setObjectName("verticalLayoutWidget")
        self.verticalLayout = QtGui.QVBoxLayout(self.verticalLayoutWidget)
        self.verticalLayout.setObjectName("verticalLayout")
        self.m_btnCheck = QtGui.QPushButton(self.verticalLayoutWidget)
        self.m_btnCheck.setObjectName("m_btnCheck")
        self.verticalLayout.addWidget(self.m_btnCheck)
        self.m_btnVersion = QtGui.QPushButton(self.verticalLayoutWidget)
        self.m_btnVersion.setObjectName("m_btnVersion")
        self.verticalLayout.addWidget(self.m_btnVersion)
        self.m_btnVerSwf = QtGui.QPushButton(self.verticalLayoutWidget)
        self.m_btnVerSwf.setObjectName("m_btnVerSwf")
        self.verticalLayout.addWidget(self.m_btnVerSwf)
        LeftFnWin.setWidget(self.verticalLayoutWidget)

        self.retranslateUi(LeftFnWin)
        QtCore.QMetaObject.connectSlotsByName(LeftFnWin)

    def retranslateUi(self, LeftFnWin):
        LeftFnWin.setWindowTitle(QtGui.QApplication.translate("LeftFnWin", "function", None, QtGui.QApplication.UnicodeUTF8))
        self.m_btnCheck.setText(QtGui.QApplication.translate("LeftFnWin", "test btn", None, QtGui.QApplication.UnicodeUTF8))
        self.m_btnVersion.setText(QtGui.QApplication.translate("LeftFnWin", "copy file", None, QtGui.QApplication.UnicodeUTF8))
        self.m_btnVerSwf.setText(QtGui.QApplication.translate("LeftFnWin", "build version swf", None, QtGui.QApplication.UnicodeUTF8))


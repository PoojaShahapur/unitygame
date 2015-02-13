# -*- coding: utf-8 -*-

# Form implementation generated from reading ui file 'E:\Self\Self\unity\unitygame\Tools\CPP2CSharp\src\CPP2CSharp\UI\LeftFnWin.ui'
#
# Created: Fri Feb 13 14:29:27 2015
#      by: PyQt5 UI code generator 5.4
#
# WARNING! All changes made in this file will be lost!

from PyQt5 import QtCore, QtGui, QtWidgets

class Ui_LeftFnWin(object):
    def setupUi(self, LeftFnWin):
        LeftFnWin.setObjectName("LeftFnWin")
        LeftFnWin.resize(235, 300)
        self.verticalLayoutWidget = QtWidgets.QWidget()
        self.verticalLayoutWidget.setObjectName("verticalLayoutWidget")
        self.verticalLayout = QtWidgets.QVBoxLayout(self.verticalLayoutWidget)
        self.verticalLayout.setObjectName("verticalLayout")
        self.m_btnCheck = QtWidgets.QPushButton(self.verticalLayoutWidget)
        self.m_btnCheck.setObjectName("m_btnCheck")
        self.verticalLayout.addWidget(self.m_btnCheck)
        self.m_btnVersion = QtWidgets.QPushButton(self.verticalLayoutWidget)
        self.m_btnVersion.setObjectName("m_btnVersion")
        self.verticalLayout.addWidget(self.m_btnVersion)
        self.m_btnVerSwf = QtWidgets.QPushButton(self.verticalLayoutWidget)
        self.m_btnVerSwf.setObjectName("m_btnVerSwf")
        self.verticalLayout.addWidget(self.m_btnVerSwf)
        LeftFnWin.setWidget(self.verticalLayoutWidget)

        self.retranslateUi(LeftFnWin)
        QtCore.QMetaObject.connectSlotsByName(LeftFnWin)

    def retranslateUi(self, LeftFnWin):
        _translate = QtCore.QCoreApplication.translate
        LeftFnWin.setWindowTitle(_translate("LeftFnWin", "function"))
        self.m_btnCheck.setText(_translate("LeftFnWin", "Test Btn"))
        self.m_btnVersion.setText(_translate("LeftFnWin", "Copy File"))
        self.m_btnVerSwf.setText(_translate("LeftFnWin", "Conv File"))


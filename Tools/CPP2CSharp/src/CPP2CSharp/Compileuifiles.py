# -*- coding: utf-8 -*-
'''
Created on 2015-2-1

@author: Administrator 
'''

import os
from PyQt4.uic import compileUi

def compileUIFiles(uiDir):
    for name in os.listdir(uiDir):
        uiFilePath = os.path.join(uiDir, name)

        if os.path.isfile(uiFilePath):
            if name.endswith(".ui"):
                uiResultPath = ('ui_' + name[:-3] + ".py").lower()

                with open(os.path.join(uiDir, uiResultPath), "w") as f:
                    compileUi(uiFilePath, f)

def main():
    from CPP2CSharp import Paths

    compileUIFiles(Paths.UI_DIR)

if __name__ == "__main__":
    main()

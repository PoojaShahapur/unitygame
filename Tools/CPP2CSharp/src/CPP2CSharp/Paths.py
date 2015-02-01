# -*- coding: utf-8 -*-
'''
Created on 2015-2-1

@author: Administrator
'''

import os

FROZEN = False

from CPP2CSharp import Fake

PACKAGE_DIR = os.path.dirname(os.path.abspath(Fake.__file__))

if PACKAGE_DIR.endswith(os.path.join("library.zip", "CPP2CSharp")):
    FROZEN = True
    PACKAGE_DIR = os.path.dirname(PACKAGE_DIR)

UI_DIR = os.path.join(PACKAGE_DIR, "ui")

if not FROZEN:
    assert(os.path.exists(PACKAGE_DIR))
    assert(os.path.exists(UI_DIR))
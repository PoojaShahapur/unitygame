#-*- encoding=utf-8 -*-
'''
Created on 2015-2-1

@author: Administrator
'''

from threading import Thread

class ThreadWrap(Thread):
    
    def __init__(self, threadName):
        super(ThreadWrap, self).__init__(name = threadName)  # must add

    def run(self):
        pass;

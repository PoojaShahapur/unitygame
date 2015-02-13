'''
Created on 2015年2月13日

@author: {Administrator}
'''

from CPP2CSharp.Core import Utils
from CPP2CSharp.DataStruct import MStack
from CPP2CSharp.CPPParse import CppItemBase
from CPP2CSharp.CPPParse import CppMultiComment

class CppFile(CppItemBase):
    '''
    classdocs
    '''


    def __init__(self, params):
        '''
        Constructor
        '''
        self.m_pStack = MStack.MStack()
        self.m_curCppElem = None
    
    
    #for line in fHandle:
    #line.lstrip()          # 删除左边的空格
    #lineList = line.split()            # 空格分割
    #if len(lineList):           # 如果还有内容
    
    def parseCpp(self, filePathName):
        with open(filePathName, 'r', encoding = 'utf8') as fHandle:
            allLine = fHandle.read();       # 读取所有的内容
            allLine.lstrip(' \n')          # 删除左边的空格
            while len(allLine):
                oneToken = Utils.getToken(allLine)
                if oneToken == "#ifndef":
                    Utils.skipCurLine(allLine)
                elif oneToken == "#define":
                    Utils.skipCurLine(allLine)
                elif oneToken == "#include":
                    Utils.skipCurLine(allLine)
                elif oneToken[0:3] == "/**":
                    self.m_curCppElem = CppMultiComment.CppMultiComment()
                    self.m_pStack.push(self.m_curCppElem)
                    self.m_curCppElem.
    
                
                allLine.lstrip(' \n')          # 删除左边的空格
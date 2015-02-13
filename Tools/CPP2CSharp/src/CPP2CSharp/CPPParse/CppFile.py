'''
Created on 2015年2月13日

@author: {Administrator}
'''

from CPP2CSharp.Core.Utils import Utils
from CPP2CSharp.DataStruct import MStack
from CPP2CSharp.CPPParse import CppItemBase
from CPP2CSharp.CPPParse import CppMultiComment
from CPP2CSharp.CPPParse import TokenParseData
from CPP2CSharp.CPPParse import CppStructItem
from CPP2CSharp.Core.Logger import Logger


class CppFile(CppItemBase.CppItemBase):
    '''
    classdocs
    '''


    def __init__(self):
        '''
        Constructor
        '''
        self.m_pStack = MStack.MStack(10)
        self.m_curCppElem = None
    
    
    #for line in fHandle:
    #line.lstrip()          # 删除左边的空格
    #lineList = line.split()            # 空格分割
    #if len(lineList):           # 如果还有内容
    
    def parseCpp(self, filePathName):
        with open(filePathName, 'r', encoding = 'utf8') as fHandle:
            tokenParseData = TokenParseData.TokenParseData('')
            try:
                tokenParseData.m_fileStr = fHandle.read();       # 读取所有的内容
            except Exception as e:
                Utils.logStackInfo()
                Logger.instance().info("{0} 文件解析错误".format(filePathName))
                return
            
                
            Utils.skipSpaceAndBr(tokenParseData)          # 删除左边的空格
            while len(tokenParseData.m_fileStr):
                oneToken = Utils.getToken(tokenParseData)
                if oneToken == "#ifndef":
                    Utils.skipCurLine(tokenParseData)
                elif oneToken == "#define":
                    Utils.skipCurLine(tokenParseData)
                elif oneToken == "#include":
                    Utils.skipCurLine(tokenParseData)
                elif oneToken[0:3] == "/**":
                    self.m_curCppElem = CppMultiComment.CppMultiComment()
                    self.m_pStack.push(self.m_curCppElem)
                    self.m_curCppElem.parseCppElem(tokenParseData)
                elif oneToken[0:2] == "//":
                    Utils.skipCurLine(tokenParseData)
                elif oneToken == "struct":
                    self.m_curCppElem = CppStructItem.CppStructItem()
                    self.m_curCppElem.parseCppElem(tokenParseData)
    
                
                Utils.skipSpaceAndBr(tokenParseData)          # 删除左边的空格
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
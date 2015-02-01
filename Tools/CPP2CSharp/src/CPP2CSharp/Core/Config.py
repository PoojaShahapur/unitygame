#-*- encoding=utf-8 -*-
'''
Created on 2015-2-1

@author: Administrator
'''

import os
import datetime
#import time

# base config info
class Config(object):
    
    pInstance = None
    
    @staticmethod
    def instance():
        if Config.pInstance is None:
            Config.pInstance = Config()
        return Config.pInstance
    
    def __init__(self):
        assert(Config.pInstance is None)
        Config.pInstance = self
        
        # 注意全部需要初始化，否则如果配置文件不用，并且没有判断是否有这个属性，就会出问题
        self.srcrootpath = ""       # 资源的根目录,这个是原始资源的 目录
        self.srcrootassetpath = ""  # 源代码 asset 目录名字
        self.destrootpath = ""      # 存放最终文件的目录,这个就是临时目录
        self.preversion = ""        # 之前版本，倒数第二次保存的版本信息
        self.curversion = ""        # 当前版本，最后一次保存的版本信息
        self.preckdirname = "md5dir"      # 目录的 md5 文件前缀
        self.preckfilename = "md5file"       # 校验和文件名字前缀
        self.preckappname = "md5app"        # app md5原始文件
        self.preverfilename = "version"       # 版本文件名字前缀
        
        self.preckallverfilename = "versionall"   # all 模块的前缀
        self.preckappverfilename = "versionapp"   # app 模块的前缀
        
        self.allverclass = 'art.ver.all'        # all 版本类名字
        self.appverclass = 'art.ver.app'        # app 版本类名字
        self.appmodulerelpath = 'asset\module\ModuleApp.swf'      # ModuleApp.swf 相对目录
        
        self.m_appmodulenameandext = 'ModuleApp.swf'                # ModuleApp.swf 模块的名字和扩展名字，美誉路径
        
        self.startpicpath = 'asset\\uiimage\\loadinglogo\\uiloadinglogo.swf'   # 启动的时候就显示的图片，主要是启动的时候可能加载到游戏的模块比较慢，一直黑屏，因此启动模块直接加载这个模块，系那是一个图片
        
        # 工具类
        self.z7z= 'D:\\ProgramFiles\\7-Zip\\7z.exe'
        self.swiftjar = 'E:\\work\\client-05\\trunk\\tools\\swift-tool\\Swift.jar'
        
        self.tmpDir = 'tmp'         # 临时文件夹目录
        self.outDir = 'output'      # 最终输出文件目录
        # 这两个字段不再使用，但是为了代码仍然保留
        self.htmlname = 'Start.html'
        self.htmltemplate = "E:\\e\\work\\client-new-1224\\client\\module\\modulestart\\html-template\\index.template.html"      # html 模板文件
        
        self.startswfname = 'Start.swf'
        self.srccoderoot = ''       # 源代码目录,用目录 md5 的时候使用
        self.foldermd5cmp = False   # 是否开启比较文件夹 md5 ，主要是之前老的 asc1.0 编译的时候重新编译一个模块，模块的 md5 就会改变，而新的 asc 2.0 就不会，因此 asc2.0 还是不用比较文件夹目录
        
        self.subversion = '0';        # 子版本号，主要是升级所有的文件的版本，经常有些文件传到服务器上，的那是文件有问题，需要主动升级版本号,'0' 是没有版本号
        
        #读取初始化数据
    def readInit(self, filename):
        fHandle = open(filename, 'r', encoding = 'utf8')
        cont = fHandle.read()
        strlist = cont.split('\n')
        idx = 0
        substrList = []
        listlen = len(strlist)
        while(idx < listlen):
            substrList = strlist[idx].split('=')
            if len(substrList[0]):
                if substrList[0] == 'foldermd5cmp':
                    if int(substrList[1]):
                        self.__dict__[substrList[0]] = True
                    else:
                        self.__dict__[substrList[0]] = False
                else:    
                    self.__dict__[substrList[0]] = substrList[1]
            idx += 1
        
        # 修正资源目录
        self.srcrootassetpath = self.srcrootpath + '/' + 'asset'
        # 计算临时数据
        self.postInit()

    # 当前 md 校验和文件路径加名字
    def curCKFilePath(self):
        curfilename = ('%s_%s%s' % (self.preckfilename, self.curversion, '.txt'))
        return os.path.join(self.destrootpath, curfilename)

    # 之前 md 校验和文件路径加名字
    def preCKFilePath(self):
        curfilename = ('%s_%s%s' % (self.preckfilename, self.preversion, '.txt'))
        return os.path.join(self.destrootpath, curfilename)

    # 当前版本文件路径加名字
    def curVerFilePath(self):
        curfilename = ('%s_%s%s' % (self.preverfilename, self.curversion, '.txt'))
        return os.path.join(self.destrootpath, curfilename)        

    # 当前版本文件路径加名字
    def preVerFilePath(self):
        curfilename = ('%s_%s%s' % (self.preverfilename, self.preversion, '.txt'))
        return os.path.join(self.destrootpath, curfilename)
    
    # 之前的目录 md 文件
    def preVerDirPath(self):
        curfilename = ('%s_%s%s' % (self.preckdirname, self.preversion, '.txt'))
        return os.path.join(self.destrootpath, curfilename)
    
        # 当前的目录 md 文件
    def curVerDirPath(self):
        curfilename = ('%s_%s%s' % (self.preckdirname, self.curversion, '.txt'))
        return os.path.join(self.destrootpath, curfilename)

    # 当前的目录 md 文件
    def uiSrcRootPath(self):
        return os.path.join(self.srccoderoot, "ui")
    
    # core code 当前的目录 md 文件
    def coreSrcRootPath(self):
        return os.path.join(self.srccoderoot, "core")
    
    # module code 当前的目录 md 文件
    def moduleSrcRootPath(self):
        return os.path.join(self.srccoderoot, "module")
    
        # module code 当前的目录 md 文件
    def externSrcRootPath(self):
        return os.path.join(self.srccoderoot, "extern")

    # 返回 app 文件全
    def appCKFilePath(self):
        curfilename = ('%s_%s%s' % (self.preckappname, self.curversion, '.txt'))
        return os.path.join(self.destrootpath, curfilename)
    
    # ModuleApp 模块的目录
    def appAppSwfPath(self):
        return os.path.join(self.srcrootpath, self.appmodulerelpath)
    
    def postInit(self):
        idx = self.appmodulerelpath.rfind('\\')
        if idx != -1:
            self.m_appmodulenameandext = self.appmodulerelpath[idx + 1:]
        else:
            idx = self.appmodulerelpath.rfind('/')
            if idx != -1:
                self.m_appmodulenameandext = self.appmodulerelpath[idx + 1:]
            else:
                self.m_appmodulenameandext = self.appmodulerelpath

    
    # ModuleApp 模块的文件名字和扩展名字，没有路径
    def appAppSwfNameAndExt(self): 
        return self.m_appmodulenameandext
    
    # startpicpath 模块的目录
    def startPicPath(self):
        return os.path.join(self.srcrootpath, self.startpicpath)
    
    # 获取最终生成的 versionall.swf 的 md5 
    def allverFilePath(self):
        return os.path.join(self.destrootpath, self.outDir, self.preckallverfilename + '.swf')    

    def appStartSwfPath(self):
        return os.path.join(self.srcrootpath, self.startswfname)
    
    def htmlPath(self):
        return os.path.join(self.destrootpath, self.htmlname)
    
    def uiPath(self):
        return os.path.join(self.srcrootpath, 'asset\\ui')
    
    def modulePath(self):
        return os.path.join(self.srcrootpath, 'asset\\module')
    
    def isExistPreV(self):
        return self.preversion != '0'
    
    def isExistCurV(self):
        return self.curversion != '0'
    
    # 返回是否比较文件夹目录 md5 ，asc1.0 版本的时候，每一次重新编译，生成的  swf 的 md5 都不一样， asc2.0 重新编译生成的swf 的 md5 一样
    def getfoldermd5cmp(self):
        return self.foldermd5cmp
    
    def swiftVersion(self):
        self.preversion = self.curversion
        #self.curversion = str(datetime.date.year) + str(datetime.date.month) + str(datetime.date.day) + str(datetime.time.hour) + str(datetime.time.minute) + str(datetime.time.second)
        #nowdata = datetime.date()
        #nowtime = datetime.time()
        self.curversion = str(datetime.datetime.now())
        dotidx = self.curversion.find('.')
        self.curversion = self.curversion[0:dotidx]
        self.curversion = self.curversion.replace(' ', '-')
        self.curversion = self.curversion.replace(':', '-')

    
    def saveCFG(self):
        filename = "config.txt"
        fHandle = open(filename, 'w', encoding = 'utf8')
        fHandle.write('preversion=' + self.preversion + '\n')
        fHandle.write('curversion=' + self.curversion + '\n')
        fHandle.write('preckdirname=' + self.preckdirname + '\n')
        fHandle.write('preckfilename=' + self.preckfilename + '\n')
        fHandle.write('preckappname=' + self.preckappname + '\n')
        fHandle.write('preverfilename=' + self.preverfilename + '\n')
        fHandle.write('preckallverfilename=' + self.preckallverfilename + '\n')
        fHandle.write('preckappverfilename=' + self.preckappverfilename + '\n')
        fHandle.write('allverclass=' + self.allverclass + '\n')
        
        fHandle.write('appverclass=' + self.appverclass + '\n')
        #fHandle.write('startswfname=' + self.startswfname + '\n')
        fHandle.write('appmodulerelpath=' + self.appmodulerelpath + '\n')
        fHandle.write('startpicpath=' + self.startpicpath + '\n')
        #fHandle.write('htmlname=' + self.htmlname + '\n')
        #fHandle.write('htmltemplate=' + self.htmltemplate + '\n')
        fHandle.write('srcrootpath=' + self.srcrootpath + '\n')
        fHandle.write('destrootpath=' + self.destrootpath + '\n')
        fHandle.write('z7z=' + self.z7z + '\n')
        fHandle.write('swiftjar=' + self.swiftjar + '\n')
        fHandle.write('srccoderoot=' + self.srccoderoot + '\n')
        if self.foldermd5cmp:
            fHandle.write('foldermd5cmp=' + str(1) + '\n')
        else: 
            fHandle.write('foldermd5cmp=' + str(0) + '\n')
            
        fHandle.write('subversion=' + self.subversion)
        
        fHandle.close()
        
    def hasSubVersion(self):
        return self.subversion != '0'       # '0' 是没有子版本号

    # 获取版本字符串的编码
    def subVersionByte(self):
        #self.subversion.encode("utf-8")
        return bytes(self.subversion, encoding = "utf8")


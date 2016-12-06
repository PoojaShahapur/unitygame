#coding : utf-8
#!/usr/bin/python

import os
import shutil

import re
import time

spaceholder = "--[[替换占位符(勿删)--]]"
UIFormIDStr =     """this.eUIDefaultForm = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    --[[替换占位符(勿删)--]]"""
UIAttrSystemStr = """M[GlobalNS.UIFormID.eUIDefaultForm] = {
            m_widgetPath = "UI/UIDefaultForm/UIDefaultForm.prefab",
            m_luaScriptPath = "MyLua.UI.UIDefaultForm.UIDefaultForm",
			m_luaScriptTableName = "GlobalNS.UILua",
			m_canvasId = GlobalNS.UICanvasID.eUIFirstCanvas,
			m_layerId = GlobalNS.UILayerID.eUISecondLayer,
			m_preFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			m_preFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	--[[替换占位符(勿删)--]]"""

def listFiles(dirPath):

    fileList=[]

    for root,dirs,files in os.walk(dirPath):
        for fileObj in files:

            fileList.append(os.path.join(root,fileObj))
    return fileList

def replacefilecontent(fileName, str, replace_name):
	global spaceholder
	global UIFormID
	global UIAttrSystemStr
	
	f = open(fileName,'r+')
	
	with open('temp', 'a') as new_f:
		all_the_lines=f.readlines()
		for eachline in all_the_lines:
			new_f.write(eachline.replace(spaceholder,str).replace('DefaultForm', replace_name))
	f.close()
	shutil.copyfile('temp', fileName)
	os.remove('temp')
	
def main():
	fileDir = "./DefaultForm"
	fileList = listFiles(fileDir)
	replace_name = 'RelivePanel'  #RelivePanel 新的界面名称
	os.mkdir('UI' + replace_name)
	
	#替换DefaultForm文件夹
	for fileObj in fileList:
		if not fileObj.endswith('.lua'):
			continue
		f = open(fileObj,'r+')
		new_filename = fileObj.replace('./DefaultForm','./UI'+replace_name+'/').replace('DefaultForm', replace_name)
		with open(new_filename, 'a') as new_f:
			all_the_lines=f.readlines()
			for eachline in all_the_lines:
				new_f.write(eachline.replace('DefaultForm',replace_name).replace('BtnDefault','BtnRelive').replace('mDefaultBtn','mReliveBtn').replace('Default_BtnTouch','Relive_BtnTouch'))  #BtnRelive  mReliveBtn Default_BtnTouch示例按钮可根据需要增减

    #替换UIFormID.lua文件
	fileName = "./../../Libs/UI/UICore/UIFormID.lua"
	replacefilecontent(fileName, UIFormIDStr, replace_name)
	
	#替换UIAttrSystem.lua文件
	fileName = "./../../Libs/UI/UICore/UIAttrSystem.lua"
	replacefilecontent(fileName, UIAttrSystemStr, replace_name)
	
if __name__=='__main__':	
	main() 
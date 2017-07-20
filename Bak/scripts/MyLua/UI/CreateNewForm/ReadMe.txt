

创建一个新的lua界面框架

1. 先在unity中做好自己的ui预制体，具体问他人

2. 将UI预制体命名（如：UIRelivePanel，名称中不要包含Default关键字，以防替换出错）

3. 修改CreateNewForm.py中的参数，见脚本注释。另：需安装python坏境

4. 双击CreateNewForm.py脚本，此时目录下生成新的界面目录，将其剪切后放置在上级UI目录下

5. 测试自己的窗口是否成功。 修改LoginModule.cs文件中形如Ctx.mInstance.mLuaSystem.openForm(10002)代码，将10002替换为自己的界面id  
   ★★★另：界面id从10000开始，在UIFormId.lua文件中查看自己的id

6. ★★★注意：如果重复执行该脚本会在UIFormId.lua和UIAttrSystem.lua文件中多次添加数据，请手动删除★★★

行为树编辑器

生成 *.qm 文件步骤
(1)
执行 gen-qt-projectfile.bat ，会在 QtBTEditor\languages 目录下生成 translate.pro 文件
(2)
执行 QtBTEditor\languages\compile.bat 文件，会生成 QtBTEditor\languages\qt_zh.qm 文件
(3)
拷贝文件 QtBTEditor\languages\qt_zh.qm 到目录 RunPath\languages 下面
#########################################################################
# Author:
#!/bin/bash
#find /home/wang/Work -name *.[cpp,h] | wc -l

printf "Total file num is:"
ls -lR | grep "^-"| wc -l
printf "Total code count is:"
find /home/wang/Work -type f | xargs cat | wc -l
#统计/home/wang/Mobile目录下的所有代码文件行数,过滤掉空行
find /home/wang/Mobile -name "*.[cpp,h,c]" |xargs cat|grep -v ^$|wc -l
#统计当前目录及其子目录的所有文件行数
find  . *.[cpp,c,h] | xargs wc -l

#########################################################################
# Author:
#!/bin/bash
#find /home/wang/Work -name *.[cpp,h] | wc -l

printf "Total file num is:"
ls -lR | grep "^-"| wc -l
printf "Total code count is:"
find /home/wang/Work -type f | xargs cat | wc -l
#ͳ��/home/wang/MobileĿ¼�µ����д����ļ�����,���˵�����
find /home/wang/Mobile -name "*.[cpp,h,c]" |xargs cat|grep -v ^$|wc -l
#ͳ�Ƶ�ǰĿ¼������Ŀ¼�������ļ�����
find  . *.[cpp,c,h] | xargs wc -l

#!/bin/sh
corefile_num=`ls /home/wang/WuLin/core.* 2>/dev/null| wc -l`
if [ $corefile_num -eq 0 ]
then
    exit 0
else
    exit 1
fi

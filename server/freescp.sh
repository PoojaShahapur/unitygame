#!/bin/bash
cd /home/wang/WuLin
ver=$(svn info | grep ����޸ĵ��޶��� | awk -F ��  {'print $2'})
dir=`date +%Y%m%d`_$ver"_TEST"

ssh wang@192.168.125.79 "mkdir ~/$dir"
scp /home/wang/WuLin/autobuild.sh wang@192.168.125.79:~/$dir

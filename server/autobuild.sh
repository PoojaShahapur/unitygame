#!/bin/bash

svn up ~/WuLin >> /home/wang/buildlog/game_`date +%Y%m%d`.log

sleep 1

cd ~/WuLin >> /home/wang/buildlog/game_`date +%Y%m%d`.log

pwd >> /home/wang/buildlog/game_`date +%Y%m%d`.log

~/WuLin/makeMobile.sh >> /home/wang/buildlog/game_`date +%Y%m%d`.log

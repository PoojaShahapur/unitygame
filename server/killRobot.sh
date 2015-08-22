#!/bin/bash
#
# restart --- start or restart the servers
#

tmp=$IFS
IFS='
'
for var in $(ps -u $(basename $HOME) | grep Robot)
do
    pid=$(echo $var | cut -c1-5)
    pname=$(echo $var | cut -c25-)

    if kill -2 $pid
    then
	echo "$pname stoped"
    else
	echo "$pname can't be stoped"
    fi
done

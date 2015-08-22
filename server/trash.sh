#!/bin/sh
#
# FILE: trash.sh
# DESC: 移动垃圾到回收站
#
# ORIG: Renzo.Liu
# DATE: 2011/11/27
#

TRASHDIR="${HOME}/.trash/$(date +%Y%m%d)"
TRASHLOG="${TRASHDIR}/trash.log"
 
if [ $# -eq 0 ]
then
        exit
fi
 
[ -d ${TRASHDIR} ] || mkdir -p ${TRASHDIR}
[ -s ${TRASHLOG} ] && echo >> ${TRASHLOG}
       
echo "===================================TRASHLOG ===================================" >>${TRASHLOG}
echo -e " $(date "+DATE:%D TIME:%T") USER:$(whoami) IP:$(who -m|awk '{print $5'}|tr -d '(|)')" >> ${TRASHLOG} 
echo "--------------------------------------------------------------------------------" >> ${TRASHLOG}
      
 
for SRCFILE in $*
do
        if [ ! -e "${SRCFILE}" ]
        then
                echo "\"${SRCFILE}\" doesn't exist!"
                continue
        fi
 
        # Move trashery to recycle bin
        TRASHERY=${TRASHDIR}/$(basename ${SRCFILE})_$$
        mv "${SRCFILE}" "${TRASHERY}"
 
        # Write trashery log
        echo "\"${SRCFILE}\" has been moved to recycle bin!"
        echo "[$(pwd)/${SRCFILE}] => [${TRASHERY}]" >>${TRASHLOG}
done
 
# End of trash.sh

#!/bin/bash
function GetSysCPU() 
{ 
    CpuIdle=`vmstat 1 5 |sed -n '3,$p' |awk '{x = x + $15} END {print x/5}' |awk -F. '{print $1}'`
    CpuNum=`echo "100-$CpuIdle" | bc` 
    echo $CpuNum 
}

cpu=`GetSysCPU` 

echo "The system CPU is $cpu"

if [ $cpu -gt 90 ] 
then 
    { 
	echo "The usage of system cpu is larger than 90%"
    } 
else 
    { 
	echo "The usage of system cpu is normal"
    } 
fi


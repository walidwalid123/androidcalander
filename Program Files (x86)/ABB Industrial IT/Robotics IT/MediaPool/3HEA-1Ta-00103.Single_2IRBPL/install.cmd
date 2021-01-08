#
# Property of ABB
#
# Install.cmd script for 5.0
#
# BUILD REV  = 1442
# BUILD DATE = 2008/04/07 11:55:58
#
#
echo -text "AW configuration installation start ..."
echo -text " Robot: 00-00000"
echo -text " Disk : 3HEA-000-00000.01"
#
# create environment variable
setenv -Name "AWEXTOPTDIR" -value $BOOTPATH
#
# copy disk info to HOME
copy -from $BOOTPATH/info.txt -to $HOME/info.txt
#
# Make required directories
mkdir -path $HOME/ApplData
mkdir -path $HOME/ApplConf
mkdir -path $HOME/ApplSys
#
mkdir -path $HOME/ApplData/ROB1
mkdir -path $HOME/ApplConf/ROB1
mkdir -path $HOME/ApplSys/ROB1
register -type option -description fpdefault -path $BOOTPATH/FP/default
#
setvar -var 10 -value 0
getkey -id "GapMedia" -var 10 -strvar $ANSWER -errlabel NO_OPTGAP
#
config -filename $BOOTPATH/EIO/IoGap.cfg -domain EIO
#
config -filename $BOOTPATH/PROC/pGapTskMapR1.cfg -domain PROC -replace
config -filename $BOOTPATH/PROC/pApiStateR1.cfg -domain PROC
config -filename $BOOTPATH/PROC/pApiCmdR12.cfg -domain PROC
#NO_OPTGAP
setvar -var 10 -value 0
getkey -id "GapMedia" -var 10 -strvar $ANSWER -errlabel NO_OPTGAP
config -filename $BOOTPATH/EIO/IoGapCr10.cfg -domain EIO
#NO_OPTGAP
#
#
config -filename $BOOTPATH/EIO/Drveio_1.cfg -domain EIO
config -filename $BOOTPATH/EIO/Sim_irbp.cfg -domain EIO
config -filename $BOOTPATH/EIO/IoLT_1_Drveio1.cfg -domain EIO
#
config -filename $BOOTPATH/MOC/EXT_FYV01.cfg -domain MOC
config -filename $BOOTPATH/MOC/SEC_FYV01.cfg.enc -domain MOC -internal
config -filename $BOOTPATH/MOC/SEC_FYM01.cfg.enc -domain MOC -internal
#
echo -text "Installing config file for drive unit C, M9DM1"
config -filename $RELEASE/robots/drv1_lib/drvcfg/EXT_DRIVE_9C_DM1.cfg -domain MOC -replace
config -filename $BOOTPATH/MOC/EXT_FYI01.cfg -domain MOC
config -filename $BOOTPATH/MOC/SEC_FYI01.cfg.enc -domain MOC -internal
#
config -filename $BOOTPATH/EIO/IoLT_2_Drveio1.cfg -domain EIO
#
config -filename $BOOTPATH/MOC/EXT_FYI02.cfg -domain MOC
config -filename $BOOTPATH/MOC/SEC_FYI02.cfg.enc -domain MOC -internal
#
# Installing OP
config -filename $BOOTPATH/EIO/IO712_type.cfg -domain EIO -replace
config -filename $BOOTPATH/EIO/OP1_eio.cfg -domain EIO
config -filename $BOOTPATH/EIO/OP2_eio.cfg -domain EIO
config -filename $BOOTPATH/EIO/Sim_op.cfg -domain EIO
setvar -var 10 -value 0
getkey -id "GapMedia" -var 10 -strvar $ANSWER -errlabel NO_OPTGAP
config -filename $BOOTPATH/EIO/IoGapOpCrR1.cfg -domain EIO
#NO_OPTGAP
copy -from $BOOTPATH/Code/GpOpQueue.sys -to $HOME/ApplSys/GpOpQueue.sys
copy -from $BOOTPATH/Code/GpOpDrv.sys -to $HOME/ApplSys/GpOpDrv.sys
config -filename $BOOTPATH/MMC/opmmc.cfg -domain MMC
setvar -var 10 -value 0
getkey -id "GapMedia" -var 10 -strvar $ANSWER -errlabel NO_OPTGAP
copy -from $BOOTPATH/Code/GoOpCell.sys -to $HOME/ApplSys/GoOpCell.sys
config -filename $BOOTPATH/MMC/opmmcc.cfg -domain MMC
#NO_OPTGAP
copy -from $BOOTPATH/Code/GpOp2.sys -to $HOME/ApplSys/GpOp.sys
setvar -var 10 -value 0
getkey -id "GapMedia" -var 10 -strvar $ANSWER -errlabel NO_OPTGAP
copy -from $BOOTPATH/Code/GpOpEEvR12.sys -to $HOME/ApplSys/GpOpEEvR1.sys
#NO_OPTGAP
config -filename $BOOTPATH/EIO/IoSOp.cfg -domain EIO
config -filename $BOOTPATH/EIO/Sim_op_2p.cfg -domain EIO
config -filename $BOOTPATH/EIO/Sim_opCr1.cfg -domain EIO
config -filename $BOOTPATH/EIO/Sim_opCr2.cfg -domain EIO
config -filename $BOOTPATH/EIO/Io1Op1.cfg -domain EIO
config -filename $BOOTPATH/EIO/Io2Op1.cfg -domain EIO
config -filename $BOOTPATH/SYS/sOpR1.cfg -domain SYS
config -filename $BOOTPATH/SYS/sOpDrvR1.cfg -domain SYS
setvar -var 10 -value 0
getkey -id "GapMedia" -var 10 -strvar $ANSWER -errlabel NO_OPTGAP
config -filename $BOOTPATH/SYS/sOpDrvR1c.cfg -domain SYS
config -filename $BOOTPATH/SYS/sOpDrvR1g.cfg -domain SYS
#NO_OPTGAP
#
# text resource routines
setvar -var 10 -value 0
getkey -id "GapMedia" -var 10 -strvar $ANSWER -errlabel NO_OPTGAP
#
copy -from $BOOTPATH/Code/TXTRES.sys -to $HOME/ApplSys/TXTRES.sys
config -filename $BOOTPATH/SYS/sTxtRes.cfg -domain SYS
#
#
copy -from $BOOTPATH/Code/GoSafeSH.sys -to $HOME/ApplSys/GoSafeSH.sys
copy -from $BOOTPATH/Code/GoSafeBI.sys -to $HOME/ApplSys/GoSafeBI.sys
copy -from $BOOTPATH/Code/GoSafeUsr.sys -to $HOME/ApplSys/GoSafeUsr.sys
copy -from $BOOTPATH/Code/GoSafeEEv.sys -to $HOME/ApplSys/GoSafeEEv.sys
config -filename $BOOTPATH/EIO/hjeio.cfg -domain EIO
config -filename $BOOTPATH/MMC/hjmmc.cfg -domain MMC
config -filename $BOOTPATH/MMC/hjmmcusr.cfg -domain MMC
#
config -filename $BOOTPATH/SYS/shjR1.cfg -domain SYS
config -filename $BOOTPATH/EIO/hjeioPR1.cfg -domain EIO
config -filename $BOOTPATH/EIO/hjeioR12.cfg -domain EIO
config -filename $BOOTPATH/MMC/hjmnummc2.cfg -domain MMC
copy -from $BOOTPATH/Code/GoSafeDataR12.sys -to $HOME/ApplSys/ROB1/GoSafeData.sys
#
#
copy -from $BOOTPATH/Code/StnInd.sys -to $HOME/ApplSys/StnInd.sys
copy -from $BOOTPATH/Code/StnIndCmp.sys -to $HOME/ApplSys/StnIndCmp.sys
config -filename $BOOTPATH/MMC/stnindmmc.cfg -domain MMC
config -filename $BOOTPATH/SYS/stnindR1.cfg -domain SYS
copy -from $BOOTPATH/Code/StnIndCfg11.sys -to $HOME/ApplSys/StnIndCfg11.sys
config -filename $BOOTPATH/SYS/stnindR111.cfg -domain SYS
copy -from $BOOTPATH/Code/StnIndCfg21.sys -to $HOME/ApplSys/StnIndCfg21.sys
config -filename $BOOTPATH/SYS/stnindR121.cfg -domain SYS
#
copy -from $BOOTPATH/Code/gapMain.mod -to $HOME/ApplSys/gapMain.mod
config -filename $BOOTPATH/SYS/sGapMainR1.cfg -domain SYS
#
copy -from $BOOTPATH/Code/GAP_PARTADV.sys -to $HOME/ApplSys/GAP_PARTADV.sys
copy -from $BOOTPATH/Code/GAP_PARTADV_SHRD.sys -to $HOME/ApplSys/GAP_PARTADV_SHRD.sys
config -filename $BOOTPATH/SYS/sPartAdvShrd.cfg -domain SYS
config -filename $BOOTPATH/MMC/mPartAdv.cfg -domain MMC
config -filename $BOOTPATH/SYS/sPartAdvR1.cfg -domain SYS
#
copy -from $BOOTPATH/Code/IrbpPrc.sys -to $HOME/ApplSys/IrbpPrc.sys
copy -from $BOOTPATH/Code/IrbpUtil.sys -to $HOME/ApplSys/IrbpUtil.sys
config -filename $BOOTPATH/SYS/sIrbpR1.cfg -domain SYS
copy -from $BOOTPATH/Code/IrbpL1Data.sys -to $HOME/ApplSys/Irbp1Data.sys
copy -from $BOOTPATH/Code/IrbpL1Prc.sys -to $HOME/ApplSys/Irbp1Prc.sys
copy -from $BOOTPATH/Code/IrbpL1Mnu.sys -to $HOME/ApplSys/Irbp1Mnu.sys
copy -from $BOOTPATH/Code/IrbpL1EEv.sys -to $HOME/ApplSys/Irbp1EEv.sys
config -filename $BOOTPATH/SYS/sIrbp1Rg.cfg -domain SYS
copy -from $BOOTPATH/Code/IrbpL2Data.sys -to $HOME/ApplSys/Irbp2Data.sys
copy -from $BOOTPATH/Code/IrbpL2Prc.sys -to $HOME/ApplSys/Irbp2Prc.sys
copy -from $BOOTPATH/Code/IrbpL2Mnu.sys -to $HOME/ApplSys/Irbp2Mnu.sys
copy -from $BOOTPATH/Code/IrbpL2EEv.sys -to $HOME/ApplSys/Irbp2EEv.sys
config -filename $BOOTPATH/SYS/sIrbp2Rg.cfg -domain SYS
#NO_OPTGAP
#
mkdir -path $HOME/DynPart
setvar -var 10 -value 0
getkey -id "GapMedia" -var 10 -strvar $ANSWER -errlabel NO_OPTGAP
copy -from $BOOTPATH/Code/PartR1S1.mod -to $HOME/ApplSys/PartR1S1.mod
copy -from $BOOTPATH/Code/PartR1S2.mod -to $HOME/ApplSys/PartR1S2.mod
config -filename $BOOTPATH/SYS/sModRob1S1m.cfg -domain SYS
config -filename $BOOTPATH/SYS/sModRob1S2m.cfg -domain SYS
copy -from $BOOTPATH/Code/DynPartR1S1.mod -to $HOME/ApplSys/DynPartR1S1.mod
copy -from $BOOTPATH/Code/DynPartPrcR1S1.mod -to $HOME/DynPart/DynPartPrcR1S1.mod
copy -from $BOOTPATH/Code/DynPartR1S2.mod -to $HOME/ApplSys/DynPartR1S2.mod
copy -from $BOOTPATH/Code/DynPartPrcR1S2.mod -to $HOME/DynPart/DynPartPrcR1S2.mod
config -filename $BOOTPATH/SYS/sDynModRob1S1m.cfg -domain SYS
config -filename $BOOTPATH/SYS/sDynModRob1S2m.cfg -domain SYS
#NO_OPTGAP
#END_SCRIPT

# Install language resource $BOOTPATH/TXT/Language
setstr -strvar $TMP_BDM -value $BOOTPATH
setstr -strvar $BOOTPATH -value "$BOOTPATH/TXT"
include -path "$RELEASE/system/instlang.cmd"
setstr -strvar $BOOTPATH -value "$TMP_BDM"

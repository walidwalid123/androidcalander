#
echo -text "AW configuration installation start ..."
echo -text " Robot: 00-00000"
#
# Make required directories
mkdir -path $HOME/ApplData
mkdir -path $HOME/ApplConf
mkdir -path $HOME/ApplSys
#
mkdir -path $HOME/ApplData/ROB1
mkdir -path $HOME/ApplConf/ROB1
mkdir -path $HOME/ApplSys/ROB1
mkdir -path $HOME/ApplData/ROB2
mkdir -path $HOME/ApplConf/ROB2
mkdir -path $HOME/ApplSys/ROB2
#
#
config -filename $BOOTPATH/SYS/sPos1Tsk.cfg -domain SYS
# IRBP TASK 1 USING MOTION PLANNER 3
config -filename $BOOTPATH/SYS/sMug1SImp3.cfg -domain SYS
#
config -filename $BOOTPATH/EIO/IO712_type.cfg -domain EIO
config -filename $BOOTPATH/EIO/BPOS_Io_21.cfg -domain EIO
config -filename $BOOTPATH/EIO/Sim_irbp.cfg -domain EIO
config -filename $BOOTPATH/EIO/IoKR.cfg -domain EIO
#
config -filename $BOOTPATH/MOC/EXT_FYV01.cfg -domain MOC
config -filename $BOOTPATH/MOC/SEC_FYV01.cfg.enc -domain MOC -internal
config -filename $BOOTPATH/MOC/SEC_FYM01.cfg.enc -domain MOC -internal
echo -text "Installing config file for drive unit T, M7DM1"
config -filename $RELEASE/robots/drv1_lib/drvcfg/EXT_DRIVE_7T_DM1.cfg -domain MOC -replace
echo -text "Installing config file for drive unit C, M8DM1"
config -filename $RELEASE/robots/drv1_lib/drvcfg/EXT_DRIVE_8C_DM1.cfg -domain MOC -replace
echo -text "Installing config file for drive unit C, M9DM1"
config -filename $RELEASE/robots/drv1_lib/drvcfg/EXT_DRIVE_9C_DM1.cfg -domain MOC -replace
config -filename $BOOTPATH/MOC/EXT_FYH01.cfg -domain MOC
config -filename $BOOTPATH/MOC/SEC_FYH01.cfg.enc -domain MOC -internal
# IRBP Calib. package
# 1.0.5
#
echo -text "Installing IRBP Calib..."
echo -text " ver 1.0.5"
copy -from $BOOTPATH/Code/PoscalibCon.sys -to $HOME/ApplSys/PoscalibCon.sys
copy -from $BOOTPATH/Code/Poscalib.sys -to $HOME/ApplSys/Poscalib.sys
config -filename $BOOTPATH/MMC/mPoscalib.cfg -domain MMC
text -filename  $BOOTPATH/TXT/PoscalibTxt.$LANG -package 0
config -filename $BOOTPATH/SYS/sPoscalibP1.cfg -domain SYS
#
#
copy -from $BOOTPATH/Code/Tool_data.sys -to $HOME/ApplSys/Tool_data.sys
config -filename $BOOTPATH/SYS/sToolDataR1.cfg -domain SYS
config -filename $BOOTPATH/SYS/sToolDataR2.cfg -domain SYS
#
#
#END_SCRIPT

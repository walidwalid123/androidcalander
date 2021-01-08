%%%
  VERSION: 1
  LANGUAGE: ENGLISH
%%%

MODULE DynPartPrcR2S1
  !**********************************************************
  !
  ! (c) ABB
  !
  !**********************************************************
  !
  ! Module:       DynPartPrcR2S1
  !
  ! Written by:   SÖ
  !
  ! Description:  Module that holds the robot 2 dynamic program for station 1.
  !
  ! Procedures:   DynProgStn1
  !
  ! Functions:    None
  !
  ! Created:      2006-05-11
  !
  ! Version:      1.0
  !
  ! History:      1.0
  !                 Created
  !
  !************************************************************
  !
  PROC DynProgStn1()
    !
    ! This is the work program for station 1
    ! Put your process instructions here or
    ! put a procedure call for your program
    !
    ! Remove below stop instruction before
    ! production.
    !
    !**************************************
    ! For error handling Use:
    !  RecoveryPosSet and RecoveryPosReset
    !
    !  There is a limitations to the use of RecoveryPosSet. The Pathrecorder can not be turned on
    !  with RecoveryPosSet before a WaitSyncTask instruction, i.e. the robot can never escape past
    !  a WaitSyncTask instruction. Therefore, make sure that RecoveryPosSet is always used after
    !  the WaitSyncTask instruction in the RAPID program.
    !
    Stop;
  ENDPROC
ENDMODULE

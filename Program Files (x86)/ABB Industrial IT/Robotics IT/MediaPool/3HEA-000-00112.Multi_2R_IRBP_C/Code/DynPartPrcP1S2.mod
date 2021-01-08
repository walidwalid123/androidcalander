%%%
  VERSION: 1
  LANGUAGE: ENGLISH
%%%

MODULE DynPartPrcP1S2
  !**********************************************************
  !
  ! (c) ABB
  !
  !**********************************************************
  !
  ! Module:       DynPartPrcP1S2
  !
  ! Written by:   SÖ
  !
  ! Description:  Module that holds the IRBP 1 program for station 2.
  !
  ! Procedures:   DynProgStn2
  !
  ! Functions:    None
  !
  ! Created:      2003-12-02
  !
  ! Version:      1.0
  !
  ! History:      1.0
  !                 Created
  !
  !************************************************************
  !
  PROC DynProgStn2()
    !
    ! This is the work program for station 2
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

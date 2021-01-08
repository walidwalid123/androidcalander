%%%
  VERSION:1
  LANGUAGE:ENGLISH
%%%

MODULE PartR2S2
  !**********************************************************
  !
  ! (c) ABB
  !
  !**********************************************************
  !
  ! Module:       PartR2S2
  !
  ! Written by:   SÖ
  !
  ! Description:  Module that holds the robot 2 program for station 2.
  !
  ! Procedures:   ProgStn2
  !
  ! Functions:    None
  !
  ! Created:      2003-12-02
  !
  ! Version:      1.0
  !
  ! History:      1.0
  !                 Created
  !               2005-09-16 SÖ
  !                 Changed deklaration of partdata to TASK PERS due to
  !                 problems in production manager when editing part.
  !               2005-11-04 SÖ
  !                 Changed def for partadv data to match changes to the datatype, service angle added
  !
  !************************************************************
  !
  TASK PERS partdata pdProgStn2:=["ProgStn2","Program station 2","T_ROB1:T_ROB2:T_POS1",2,0,"GapEmptyPart200.gif","pdvProgStn2"];
  PERS partadv pdvProgStn2:=[[0,0,0,0,0,0],[0,0,0,0,0,0],[0,0,0,0,0,0],[0,[0,0,0],[0,0,0,0],0,0,0]];
  !
  !---------------------------------------------------------------------------
  ! Procedure ProgStn2
  !---------------------------------------------------------------------------
  ! Desc: Program for station 2
  !
  ! Parameters: None
  !
  !---------------------------------------------------------------------------
  ! History:
  ! - <DATE> ! Created ! <AUTHOR>
  PROC ProgStn2()
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
    !
  ENDPROC
ENDMODULE

---------------------------------------
Version Information:
---------------------------------------

RtsUvc_NoLogo
Driver = 10.0.177


=================================================================================

<<< For Win7/Win8/Win8.1/Win10 x86/x64 Driver >>>

---------------------------------------
Setup Driver at first time:
---------------------------------------

Step 1. Run the setup.exe program to start the installation.

Step 2. At the end of the procedure, select "Yes,I want to restart my computer now.",
        press [Finish] to complete the installation and restart.


---------------------------------------
Update Driver:
---------------------------------------

Step 1. Run setup.exe, it will remove the original driver in your system and then install new driver.

Step 2. At the end of the procedure, select "Yes,I want to restart my computer now.",
        press [Finish] to complete the installation and restart.


---------------------------------------
Remove Driver:
---------------------------------------

Step 1. Go to Control Panel.

Step 2. Click 'Uninstall a program'.

Step 3. Select Camera Device and press [Uninstall] button.

Step 4. Click on [OK] to confirm the uninstallation.

Step 5. At the end of the procedure, press [Finish] to complete the uninstallation.


---------------------------------------
Silent Installation:
---------------------------------------
Run  setup.exe /s /f2"path\LogFile"
i.e. setup.exe /s /f2"c:\mylog.log"


---------------------------------------
Silent Uninstallation:
---------------------------------------
Run  setup.exe /removeonly /s /f1"path\usetup.iss" /f2"path\LogFile"
i.e. setup.exe /removeonly /s /f1".\usetup.iss" /f2"c:\mylog.log"


---------------------------------------
Return Codes for Silent Installation/Uninstallation:
---------------------------------------
0    Success.
-1   General error.
-2   Invalid mode.
-3   Required data not found in the Setup.iss file.
-4   Not enough memory available.
-5   File does not exist.
-6   Cannot write to the response file.
-7   Unable to write to the log file.
-8   Invalid path to the InstallShield Silent response (.iss) file.
-9   Not a valid list type (string or number).
-10  Data type is invalid.
-11  Unknown error during setup.
-12  Dialog boxes are out of order.
-51  Cannot create the specified folder.
-52  Cannot access the specified file or folder.
-53  Invalid option selected.


-------------------------------------------------------------
Install Package UI Support Languages (Others show English):
-------------------------------------------------------------
Basque, Bulgarian, Catalan,
Chinese(Simplified), Chinese(Traditional), Croatian, Czech,
Danish, Dutch, English, Finnish, French(Canada), French(France),
German, Greek, Hungarian, Indonesian, Italian, Japanese,
Korean, Norwegian, Polish, Portuguese(Brazil), Portuguese(Portugal),
Romanian, Russian, Serbian(Cyrillic), Slovak, Slovenian,
Spanish, Swedish, Thai, Turkish

Release Version:  Production Version

Package: 542778

Intel(R) Graphics Driver: 25.20.100.6847
 

Intel(R) Display Audio Driver:  11.1.0.6 


Operating System(s):   

		Microsoft Windows* 10-64 (19H1)

Platforms:

	Ice Lake
	
Release Date: June 21, 2019


CONTENTS OF THIS DOCUMENT

I.	Installation Information
II.	System Requirements
III.	Localized Language Abbreviations
IV.	Installing the Software
V.	Verifying Installation of the Software
VI.	Identifying the Software Version Number
VII.	Installation Switches 
VIII.	Uninstalling the Software
IX.	Intel Software License Agreement
X.	Disclaimer


I.  INSTALLATION INFORMATION

Supports Intel(R) UHD Graphics, Intel(R) HD Graphics and Intel(R) Iris(R) Plus Graphics on:

 	Ice Lake

	  
DISCLAIMER: Intel is making no claims of usability, efficacy or warranty.  The INTEL SOFTWARE LICENSE AGREEMENT contained herein completely defines the license and use of this software.
   
This document contains information on products in the design phase of development. The information here is subject to change without notice. Do not finalize a  design with this information.


II.  SYSTEM REQUIREMENTS

1.  The system must contain one of the following Intel chipsets/processors:
		
	Ice Lake	
	        	
2.  The software should be installed on systems with at
    least 1 GB of system memory.

3.  There should be sufficient hard disk space in the <TEMP>
    directory on the system in order to install this
    software.

    The drivers included with this distribution package are
    designed to function with all released versions of
    Microsoft  Windows* 10 OS(s) available at the time of release of this package.

Please check with your system provider to determine the
operating system and Intel chipset used in your system.


III.  LOCALIZED LANGUAGE ABBREVIATIONS

The following list contains the hexadecimal key of all
languages into which the driver has been localized. You may
have to refer to this section while using this document.

ara – Arabic (Saudi Arabia)
cht – Chinese (Simplified)
cht – Chinese (Traditional)
hrv - Croatian 
cys - Czech
dan - Danish
nld - Dutch
enu - English (US)
fin - Finnish
fra-  French 
deu - German
ell - Greek
heb - Hebrew
hun - Hungarian
ita - Italian
jpn - Japanese
kor - Korean
nor – Norwegian (Bokmal)
plk - Polish
ptg - Portuguese (Brazilian)
ptb - Portuguese 
rom - Romanian
rus-  Russian
SKY - Slovak
SLV - Slovenian
Esp - Spanish
Sve - Swedish  
Tha - Thai
Trk - Turkish


IV.  INSTALLING THE SOFTWARE

General Installation Notes:

1.  The operating system must be installed prior to the installation of the 
    driver.

2.  This installation procedure is specific only to the version of driver 
    and installation file included in this release.

3.  This procedure assumes that all of the software associated with this 
    release is located in the same directory.


INSTALLATION INSTRUCTIONS 
------------------------------------------------------------------

To install from a Web download, you will download either a .zip file or an 
.exe file.

a. If it is a .zip file, double-click on the file you downloaded and choose 
   "Extract all files". Browse to a destination folder and choose "Extract".

b. If it is an .exe file, double-click on the file you downloaded and specify a 
   location to extract the  driver files. Click "Unzip" and the files will 
   extract. Click "OK" on the next window, then click "Close". 


------------------------------------------------------------------
  Microsoft Windows* "igxpin.exe" Installation
------------------------------------------------------------------

1. Locate the hard drive directory where the driver files are stored 
   using the browser or the Explore feature of Windows*.

2. From this directory, double-click the "igxpin.exe" file.

3. The first dialog of the installation user interface will appear. 
 
4. Click "Next" to continue.

5. Read the License Agreement and, if you agree with the terms, 
   click "Yes" to proceed.

6. Review the Readme File information and click "Next" to proceed.

7. When the "Setup Progress" is complete, click "Next" to proceed.

8. When the "Setup is Complete" screen appears, click "Finish" to
   complete the installation.
 
  
------------------------------------------------------------------
   Microsoft Windows* "Have Disk" Installation 
------------------------------------------------------------------

1.  Click "Start", right-click "Computer", and click "Properties". 

2.  Open "Device Manager" and select "Display Adapters".

3.  Right click on the device listed under "Display Adapters" and select ‘Update Driver’.

4.  Select "Browse my computer for driver software".

5.  Select "Let me pick from a list of device drivers on my computer".

6.  Click on "Have Disk".

7.  Click on “browse" and go to DriverFolder\Graphics

8.  Select any *.INF file from the Graphics folder; select "Open" and then "Ok". 
  
9.  Click the "Next" button and wait until the driver gets installed to get a message
   "Windows has successfully updated your driver software’.

Note:  No Changes in Graphics Driver Uninstallation
------------------------------------------------------------------
	Microsoft Windows* Manual Installation Instructions
------------------------------------------------------------------

1.  Click "Start", right-click "Computer", and click "Properties".

2.  Click "Device Manager" on the left.

3.  In the "User Account Control" window, click "Yes".

    IF UPDATING DRIVER GO TO STEP 5

4.  Double-click "Video Controller (VGA Compatible)" if present under 
    "Other Devices". (Go to step 6)

5.  Expand "Display adapters" and double-click the graphics controller.  

6.  In the "Driver" tab, click "Update Driver".

7.  Click "Browse my computer for driver software".

8.  Click directly "Browse".

9.  Browse to the directory where you unzipped the file you downloaded
    and click the "Graphics" folder.

10. Click "OK" and click "Next". The operating system will install the 
    driver if it considers this an upgrade.
    
11. Click "Close" and click "Yes" to reboot. The driver should now 
    be loaded.

------------------------------------------------------------------
	For Intel(R) Display Audio Driver:
------------------------------------------------------------------

1.  Click "Start", right-click "Computer", and click "Properties".

2.  Click "Device Manager" on the left. 

3.  In the "User Account Control" window, click "Yes".

4.  Double-click "Sound, video and game controllers".

5.  If installing from scratch, right-click the "High Definition Audio" 
    controller. If updating the driver, right-click the "Intel(R) 
    Display Audio" controller. Click "Update Driver Software...".

6.  Click "Browse my computer for driver software". 

7.  Click "Let me pick from a list of device drivers on my computer".
  
8.  Click "Have Disk..." and click "Browse".

9.  Browse to the directory where you unzipped the file you 
    downloaded, click the "DisplayAudio" folder, and select the
    "IntcDAud.inf" file. Click "Open" and click "OK".

10. Select "Intel(R) Display Audio" and click "Next". 

11. The operating system will install the driver. Click "Finish" to
    complete the installation. 

12. Click "Yes" to reboot. The driver should now be loaded. 

To determine if the driver has been loaded correctly, refer to the 
Verifying Installation of the Software section below.


V.  VERIFYING INSTALLATION OF THE SOFTWARE

1.  Click "Start", right-click "Computer", and click "Properties".

2.  Click "Device Manager" on the left. 

3.  In the "User Account Control" window, click "Yes".

4.  Expand "Display adapters". The Intel(R) Graphics Driver should 
    be listed. If not, the driver is not installed correctly. 

------------------------------------------------------------------
	For Intel(R) Display Audio Driver:
------------------------------------------------------------------

1.  Click "Start", right-click "Computer", and click "Properties".

2.  Click "Device Manager" on the left.

3.  In the "User Account Control" window, click "Yes".

4.  Expand "Sound, video and game controllers". The "Intel(R) Display 
    Audio" driver should be listed and should not show a yellow bang. 
    If not, the driver is not installed correctly. 

To check the version of the driver, refer to the section below.


VI.  IDENTIFYING THE SOFTWARE VERSION NUMBER

1.  Click "Start", right-click "Computer", and click "Properties".

2.  Click "Device Manager" on the left.

3.  In the "User Account Control" window, click "Yes".

4.  Expand "Display adapters" and double-click the graphics controller.  

5.  In the "Driver" tab, note the Driver Version.

------------------------------------------------------------------
	For Intel(R) Display Audio Driver:
------------------------------------------------------------------

1.  Click "Start", right-click "Computer", and click "Properties".

2.  Click "Device Manager" on the left.

3.  In the "User Account Control" window, click "Yes". 

4.  Expand "Sound, video and game controllers" and double-click 
    "Intel(R) Display Audio".

5.  In the "Driver" tab, click "Driver Details". The function driver 
    (IntcDAud.sys) version should be listed on this screen.


VII.   INSTALLATION SWITCHES

The switches in the igxpin.exe file will have the following syntax. 
Switches are not case-sensitive and may be specified in any order 
(except for the -s switch). Switches must be separated by spaces.
SETUP [-b] [-overwrite] [-nowinsat] [-l<LCID>] [-s] [-report <path>] 

GFX-INSTALLATION CUSTOM SWITCHES
-b Forces a system reboot after the installation completes.
In non-silent mode, the absence of this switch will prompt
the user to reboot. In silent mode, the absence of this
switch forces the igxpin.exe to complete without rebooting
(the user must manually reboot to conclude the installation
process).

-overwrite Installs the Intel(R) Graphics Driver regardless of 
the version of previously installed driver. In non-silent mode,
the absence of this switch will prompt the user to confirm
overwrite of a newer Intel(R) Graphics Driver. In silent mode, 
the absence of this switch means that the installation will 
abort any attempts to regress the revision of the Intel(R) Graphics 
driver.

-l<LCID> Specifies the language used in the installation user 
interface. Without this switch, the installation user interface 
will be shown in the Display language of the OS by default. 
Hexadecimal values for the supported languages can be found in 
the localized language abbreviations section of this readme.

-s Runs in silent mode. The absence of this switch causes
the installation to be performed in verbose mode.

-report <path> Specifies an alternate location for the log file 
created by a silent installation. By default, the log file is 
created and stored during a silent installation under <root 
directory>\Intel\Logs directory as IntelGFX.log
(<WINDIR>\Temp\IntelGFX.log).

-nowinsat Turns off the automatic support for updating / 
obtaining the WinSAT* DWM score on Windows* during installation. 
In non-silent mode, the absence of this switch will enable 
automatic support for updating the WinSAT* score, but allows 
the option to disable this support with a user accessible 
checkbox shown within the installation user interface. 
In silent mode, the absence of this switch forces an automatic 
run of WinSAT and will enable the Windows* Aero* desktop theme 
(if supported).


VIII.  UNINSTALLING THE SOFTWARE

NOTE: This procedure assumes the above installation process
was successful. This uninstallation procedure is specific
only to the version of the driver and installation files
included in this package.


	FOR INTEL(R) GRAPHICS DRIVER:
------------------------------------------------------------------
1. Click "Start", click "Control Panel" icon, and double-click 
   "Programs and Features".

2. Right-click "Intel(R) Graphics Driver" and select "Uninstall".

3. Click "Next" and "Next" to uninstall the driver.

4. Click "Finish" to restart the computer.


	For INTEL(R) DISPLAY AUDIO DRIVER:
------------------------------------------------------------------

1.  Click "Start", right-click "Computer", and click "Properties".

2.  Click "Device Manager" on the left.

3.  In the "User Account Control" window, click "Yes". 

4.  Expand "Sound, video and game controllers", right-click "Intel(R) 
    Display Audio", and select "Uninstall".

5.  In the "Confirm Device Uninstall" window, click "OK" and the Intel(R)
    Display Audio driver will be uninstalled.

------------------------------------------------------------------
IX.	INTEL SOFTWARE LICENSE AGREEMENT
	(OEM / IHV / ISV Distribution & Single User)
------------------------------------------------------------------

 
IMPORTANT - READ BEFORE COPYING, INSTALLING OR USING. 
Do not use or load software (including drivers) from this site or any associated materials (collectively, the "Software") until you have carefully read the following terms and conditions. By loading or using the Software, you agree to the terms of this Agreement, which Intel may modify from time to time following reasonable notice to You. If you do not wish to so agree, do not install or use the Software. 
Please Also Note: 
. If you are an Original Equipment Manufacturer (OEM), Independent Hardware Vendor (IHV) or Independent Software Vendor (ISV), this complete LICENSE AGREEMENT applies; 

. If you are an End-User, then only Exhibit A, the INTEL SOFTWARE LICENSE AGREEMENT, applies. 


For OEMs, IHVs and ISVs: 
LICENSE. Subject to the terms of this Agreement, Intel grants to You a nonexclusive, nontransferable, worldwide, fully paid-up license under Intel's copyrights to: 
. Use, modify and copy the Software internally for Your own development and maintenance purposes; and 

. Modify, copy and distribute (subject to any restrictions imposed by Intel) the Software, including derivative works of the Software, to Your end-users, but only under a license agreement with terms at least as restrictive as those contained in Intel's Final, Single User License Agreement, attached as Exhibit A; and 

. Modify, copy and distribute the end-user documentation which may accompany the Software, but only in association with the Software. 


Intel reserves the right to further restrict your distribution of the Software to specific Intel-approved platforms, operating systems, segments, and/or devices in its sole and absolute discretion upon reasonable notice to You. 
If You are not the final manufacturer or vendor of a computer system or software program incorporating the Software, then You may transfer a copy of the Software, including derivative works of the Software (and related end-user documentation) to Your recipient for use in accordance with the terms of this Agreement, provided such recipient agrees to be fully bound by the terms hereof. You will not otherwise assign, sublicense, lease, or in any other way transfer or disclose Software to any third party. You will not reverse- compile, disassemble or otherwise reverse-engineer the Software. 
You may not subject the Software, in whole or in part, to any license obligations of Open Source Software including without limitation combining or distributing the Software with Open Source Software in a manner that subjects the Software or any portion of the Software provided by Intel hereunder to any license obligations of such Open Source Software. "Open Source Software" means any software that requires as a condition of use, modification and/or distribution of such software that such software or other software incorporated into, derived from or distributed with such software (a) be disclosed or distributed in source code form; or (b) be licensed by the user to third parties for the purpose of making and/or distributing derivative works; or (c) be redistributable at no charge. Open Source Software includes, without limitation, software licensed or distributed under any of the following licenses or distribution models, or licenses or distribution models substantially similar to any of the following: (a) GNU’s General Public License (GPL) or Lesser/Library GPL (LGPL), (b) the Artistic License (e.g., PERL), (c) the Mozilla Public License, (d) the Netscape Public License, (e) the Sun Community Source License (SCSL), (f) the Sun Industry Source License (SISL), (g) the Apache Software license and (h) the Common Public License (CPL).  
NO OTHER RIGHTS. The Software is protected by the intellectual property laws of the United States and other countries, and international treaty provisions. Except as otherwise expressly above, Intel grants no express or implied rights under Intel patents, copyrights, trademarks, or other intellectual property rights. Except as expressly stated in this Agreement, no license or right is granted to You directly or by implication, inducement, estoppel or otherwise. Intel will have the right to inspect or have an independent auditor inspect Your relevant records to verify Your compliance with the terms and conditions of this Agreement. 
CONFIDENTIALITY. If You wish to have a third party consultant or subcontractor ("Contractor") perform work on Your behalf which involves access to or use of Software, You will obtain a written confidentiality agreement from the Contractor which contains terms and obligations with respect to access to or use of Software no less restrictive than those set forth in this Agreement and excluding any distribution rights, and use for any other purpose. Otherwise, You will not disclose the terms or existence of this Agreement or use Intel's Name in any publications, advertisements, or other announcements without Intel's prior written consent. You do not have any rights to use any Intel trademarks or logos. 
OWNERSHIP OF SOFTWARE AND COPYRIGHTS. Title to all copies of the Software remains with Intel or its suppliers. The Software is copyrighted and protected by the laws of the United States and other countries, and international treaty provisions. You may not remove any copyright notices from the Software. Intel may make changes to the Software, or to items referenced therein, at any time without notice, but is not obligated to support or update the Software. Except as otherwise expressly provided, Intel grants no express or implied right under Intel patents, copyrights, trademarks, or other intellectual property rights. You may transfer the Software only if the recipient agrees to be fully bound by these terms and if you retain no copies of the Software. 
SUPPORT. Intel may make changes to the Software, or to items referenced therein, at any time without notice, but is not obligated to support, update or provide training for the Software. Intel may in its sole discretion offer such services under separate terms at Intel’s then-current rates. You may request additional information on Intel’s service offerings from an Intel sales 
representative. You agree to be solely responsible to Your End Users for any update or support obligation or other liability which may arise from the distribution of the Software. 
EXCLUSION OF OTHER WARRANTIES. THE SOFTWARE IS PROVIDED "AS IS" WITHOUT ANY EXPRESS OR IMPLIED WARRANTY OF ANY KIND INCLUDING WARRANTIES OF MERCHANTABILITY, NONINFRINGEMENT, OR FITNESS FOR A PARTICULAR PURPOSE. Intel does not warrant or assume responsibility for the accuracy or completeness of any information, text, graphics, links or other items contained within the Software. 
LIMITATION OF LIABILITY. IN NO EVENT WILL INTEL OR ITS SUPPLIERS BE LIABLE FOR ANY DAMAGES WHATSOEVER (INCLUDING, WITHOUT LIMITATION, LOST PROFITS, BUSINESS INTERRUPTION, OR LOST INFORMATION) ARISING OUT OF THE USE OF OR INABILITY TO USE THE SOFTWARE, EVEN IF INTEL HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGES. SOME JURISDICTIONS PROHIBIT EXCLUSION OR LIMITATION OF LIABILITY FOR IMPLIED WARRANTIES OR CONSEQUENTIAL OR INCIDENTAL DAMAGES, SO THE ABOVE LIMITATION MAY NOT APPLY TO YOU. YOU MAY ALSO HAVE OTHER LEGAL RIGHTS THAT VARY FROM JURISDICTION TO JURISDICTION. THE SOFTWARE LICENSED HEREUNDER IS NOT DESIGNED OR INTENDED FOR USE IN ANY MEDICAL, LIFE SAVING OR LIFE SUSTAINING SYSTEMS, TRANSPORTATION SYSTEMS, NUCLEAR SYSTEMS, OR FOR ANY OTHER MISSION CRITICAL APPLICATION IN WHICH THE FAILURE OF THE SOFTWARE COULD LEAD TO PERSONAL INJURY OR DEATH. YOU WILL INDEMNIFY AND HOLD INTEL AND THE INTEL PARTIES HARMLESS AGAINST ALL CLAIMS, COSTS, DAMAGES, AND EXPENSES, AND REASONABLE ATTORNEY FEES ARISING OUT OF, DIRECTLY OR INDIRECTLY, THE DISTRIBUTION OF THE SOFTWARE AND ANY CLAIM OF PRODUCT LIABILITY, PERSONAL INJURY OR DEATH ASSOCIATED WITH ANY UNINTENDED USE, EVEN IF SUCH CLAIM ALLEGES THAT AN INTEL PARTY WAS NEGLIGENT REGARDING THE DESIGN OR MANUFACTURE OF THE SOFTWARE. THE LIMITED REMEDIES, WARRANTY DISCLAIMER AND LIMITED LIABILITY ARE FUNDAMENTAL ELEMENTS OF THE BASIS OF THE BARGAIN BETWEEN INTEL AND YOU. INTEL WOULD NOT BE ABLE TO PROVIDE THE SOFTWARE WITHOUT SUCH LIMITATIONS.  
TERMINATION OF THIS AGREEMENT. Intel may terminate this Agreement immediately, upon notice from Intel, if You violate its terms. Upon termination, You will immediately destroy the Software (including providing certification of such destruction back to Intel) or return all copies of the Software to Intel. In the event of termination of this Agreement, all licenses granted to You hereunder will immediately terminate, except for licenses that you have previously distributed to Your end-users pursuant to the license grant above. 
APPLICABLE LAWS. Any claims arising under or relating to this Agreement will be governed by the internal substantive laws of the State of Delaware or federal courts located in Delaware, without regard to principles of conflict of laws. Each Party hereby agrees to jurisdiction and venue in the courts of the State of Delaware for all disputes and litigation arising under or relating to this Agreement. The Parties agree that the United Nations Convention on Contracts for the International Sale of Goods is specifically excluded from application to this Agreement. The Parties consent to the personal jurisdiction of the above courts.  
Export Regulations / Export Control. You will not export, either directly or indirectly, any product, service or technical data or system incorporating such items without first obtaining any required license or other approval from the U. S. Department of Commerce or any other agency or department of the United States Government. In the event any product is exported from the United States or re-exported from a foreign destination by You, You will ensure that the distribution and export/re-export or import of the product is in compliance with all laws, regulations, orders, or other restrictions of the U.S. Export Administration Regulations and the appropriate foreign government. You agree that neither you nor any of your subsidiaries will export/re-export any technical data, process, product, or service, directly or indirectly, to any country for which the United States government or any agency thereof or the foreign government from where it is shipping requires an export license, or other governmental approval, without first obtaining such license or approval.  
GOVERNMENT RESTRICTED RIGHTS. The Software is a "commercial item" as that term is defined in 48 C.F.R. 2.101, consisting of "commercial computer software" and "commercial computer software documentation" as such terms are used in 48 C.F.R. 12.212. Consistent with 48 C.F.R. 12.212 and 48 C.F.R 227.7202-1 through 227.7202-4, You will provide the Software to the U.S. Government as an End User only pursuant to the terms and conditions therein. Contractor or Manufacturer is Intel Corporation, 2200 Mission College Blvd., Santa Clara, CA 95052.  
Assignment. You may not delegate, assign or transfer this Agreement, the license(s) granted or any of Your rights or duties hereunder, expressly, by implication, by operation of law, by way of merger (regardless of whether You are the surviving entity) or acquisition, or otherwise and any attempt to do so, without Intel’s express prior written consent, will be null and void. Intel may assign this Agreement, and its rights and obligations hereunder, in its sole discretion.  
Entire Agreement. The terms and conditions of this Agreement constitutes the entire agreement between the parties with respect to the subject matter hereof, and merges and supersedes all prior, contemporaneous agreements, understandings, negotiations and discussions. Neither of the parties hereto will be bound by any conditions, definitions, warranties, understandings or representations with respect to the subject matter hereof other than as expressly provided for herein. Intel is not obligated under any other agreements unless they are in writing and signed by an authorized representative of Intel. Without limiting the foregoing, terms and conditions on any purchase orders or similar materials submitted by You to Intel, and any terms contained in Intel’s standard acknowledgment form that are in conflict with these terms, will be of no force or effect.  
Attorneys’ Fees. In the event any proceeding or lawsuit is brought by Intel or You in connection with this Agreement, the prevailing party in such proceeding will be entitled to receive its costs, expert witness fees and reasonable attorneys’ fees, including costs and fees on appeal.  
No Agency. Nothing contained herein will be construed as creating any agency, employment relationship, partnership, principal-agent or other form of joint enterprise between the parties.  
Severability. In the event that any provision of this Agreement will be unenforceable or invalid under any applicable law or be so held by applicable court decision, such unenforceability or invalidity will not render this Agreement unenforceable or invalid as a whole, and, in such event, such provision will be changed and interpreted so as to best accomplish the objectives of such 
unenforceable or invalid provision within the limits of applicable law or applicable court decisions.  
Waiver. The failure of either party to require performance by the other party of any provision hereof will not affect the full right to require such performance at any time thereafter; nor will the waiver by either party of a breach of any provision hereof be taken or held to be a waiver of the provision itself.  
Language. This Agreement is in the English language only, which language will be controlling in all respects, and all versions of this Agreement in any other language will be for accommodation only and will not be binding on you or Intel. All communications and notices made or given pursuant to this Agreement, and all documentation and support to be provided, unless otherwise noted, will be in the English language. 
 
SLAOEMISV1/RBK/11-02-17 
EXHIBIT “A” 
INTEL SOFTWARE LICENSE AGREEMENT  
(Final, Single User) 
 
IMPORTANT - READ BEFORE COPYING, INSTALLING OR USING. 
 
Do not use or load software from this site or any associated materials (collectively, the "Software") until you have carefully read the following terms and conditions. By loading or using the Software, you agree to the terms of this Agreement, which Intel may modify from time to time. If you do not wish to so agree, do not install or use the Software. 

LICENSE. You may copy the Software onto a single computer for your personal, or internal business purpose use, and you may make one back-up copy of the Software, subject to these conditions: 

You may not copy, modify, rent, sell, distribute or transfer any part of the Software except as provided in this Agreement, and you agree to prevent unauthorized copying of the Software.  

You may not reverse engineer, decompile, or disassemble the Software.  

You may not sublicense or permit simultaneous use of the Software by more than one user.  

The Software may contain the software or other property of third party suppliers, some of which may be identified in, and licensed in accordance with, any enclosed “license.txt” file or other text or file.  


OWNERSHIP OF SOFTWARE AND COPYRIGHTS. Title to all copies of the Software remains with Intel or its suppliers. The Software is copyrighted and protected by the laws of the United States and other countries, and international treaty provisions. You may not remove any copyright notices from the Software. Intel may make changes to the Software, or to items referenced therein, at any time without notice, but is not obligated to support or update the Software. Except as otherwise expressly provided, Intel grants no express or implied right under Intel patents, copyrights, trademarks, or other intellectual property rights. You may transfer the Software only if the recipient agrees to be fully bound by these terms and if you retain no copies of the Software. 

EXCLUSION OF OTHER WARRANTIES. THE SOFTWARE IS PROVIDED "AS IS" WITHOUT ANY EXPRESS OR IMPLIED WARRANTY OF ANY KIND INCLUDING WARRANTIES OF MERCHANTABILITY, NONINFRINGEMENT, OR FITNESS FOR A PARTICULAR PURPOSE. Intel does not warrant or assume responsibility for the accuracy or completeness of any information, text, graphics, links or other items contained within the Software. 

LIMITATION OF LIABILITY. IN NO EVENT WILL INTEL OR ITS SUPPLIERS BE LIABLE FOR ANY DAMAGES WHATSOEVER (INCLUDING, WITHOUT LIMITATION, LOST PROFITS, BUSINESS INTERRUPTION, OR LOST INFORMATION) ARISING OUT OF THE USE OF OR INABILITY TO USE THE SOFTWARE, EVEN IF INTEL HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGES. SOME JURISDICTIONS PROHIBIT EXCLUSION OR LIMITATION OF LIABILITY FOR IMPLIED WARRANTIES OR CONSEQUENTIAL OR 
INCIDENTAL DAMAGES, SO THE ABOVE LIMITATION MAY NOT APPLY TO YOU. YOU MAY ALSO HAVE OTHER LEGAL RIGHTS THAT VARY FROM JURISDICTION TO JURISDICTION. 

TERMINATION OF THIS AGREEMENT. Intel may terminate this Agreement at any time if you violate its terms. Upon termination, you will immediately destroy the Software or return all copies of the Software to Intel. 

APPLICABLE LAWS. Claims arising under this Agreement will be governed by the laws of Delaware, excluding its principles of conflict of laws and the United Nations Convention on Contracts for the Sale of Goods. You may not export the Software in violation of applicable export laws and regulations. Intel is not obligated under any other agreements unless they are in writing and signed by an authorized representative of Intel. 

GOVERNMENT RESTRICTED RIGHTS. The Software is provided with "RESTRICTED RIGHTS." Use, duplication, or disclosure by the Government is subject to restrictions as set forth in FAR52.227-14 and DFAR252.227-7013 et seq. or its successor. Use of the Software by the Government constitutes acknowledgment of Intel's proprietary rights therein. Contractor or Manufacturer is Intel Corporation, 2200 Mission College Blvd., Santa Clara, CA 95052.  

PUSH COLLATERAL. The Software may include features that push, edit, save, or delete content (e.g., promotional materials or other collateral) installed via internet connection to the computer on which the Software is used. Depending on your setting, this content may be displayed in certain windows, utilities, or applications available from the Software. By using this Software, you consent to the use of the internet connection to the installed computer and to the display, editing, storage, and deletion of this content as controlled by the Software. Settings are provided in the Software allowing you to disable the download and presentation of this content. 

* Other names and brands may be claimed as the property of 
others.

Copyright (C) 2017 Intel Corporation.  All rights reserved.
------------------------------------------------------------------

Copyright (C) 2016  3Dlabs Inc. Ltd.
All rights reserved

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

Neither the name of 3Dlabs Inc. Ltd. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS"AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDERS OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

---------------------------------------------------------------

(c) Copyright 1994-9, Silicon Graphics, Inc.
 ALL RIGHTS RESERVED 
Permission to use, copy, modify, and distribute this software for any purpose and without fee is hereby granted,  provided that the above copyright notice appear in all copies and that both the copyright notice and this permission notice appear in supporting documentation, and that the name of Silicon Graphics, Inc. not be used in advertising or publicity pertaining to distribution of the software without specific, written prior permission. 

THE MATERIAL EMBODIED ON THIS SOFTWARE IS PROVIDED TO YOU "AS-IS" AND WITHOUT WARRANTY OF ANY KIND, EXPRESS, IMPLIED OR OTHERWISE, INCLUDING WITHOUT LIMITATION, ANY WARRANTY OF MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE. IN NO EVENT SHALL SILICON GRAPHICS, INC. BE LIABLE TO YOU OR ANYONE ELSE FOR ANY DIRECT, SPECIAL, INCIDENTAL, INDIRECT OR CONSEQUENTIAL DAMAGES OF ANY KIND, OR ANY DAMAGES WHATSOEVER, INCLUDING WITHOUT LIMITATION, LOSS OF PROFIT, LOSS OF USE, SAVINGS OR REVENUE, OR THE CLAIMS OF THIRD PARTIES, WHETHER OR NOT SILICON GRAPHICS, INC. HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH LOSS, HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, ARISING OUT OF OR IN CONNECTION WITH THE POSSESSION, USE OR PERFORMANCE OF THIS SOFTWARE.
 
----------------------------------------------------------------

Copyright (c) 2010, Google, Inc.

 All rights reserved. 
Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met: 

•Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 

•Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution. 

•Neither the name of Google nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. 

Subject to the terms and conditions of the above License, Google hereby grants to You a perpetual, worldwide, non-exclusive, no-charge, royalty-free, irrevocable (except as stated in this section) patent license to make, have made, use, offer to sell, sell, import, and otherwise transfer this implementation of VP8, where such license applies only to those patent claims, both currently owned by Google and acquired in the future, licensable by Google that are necessarily infringed by this implementation of VP8. If You or your agent or exclusive licensee institute or order or agree to the institution of patent litigation against any entity (including a cross-claim or counterclaim in a lawsuit) alleging that this implementation of VP8 or any code incorporated within this implementation of VP8 constitutes direct or contributory patent infringement, or inducement of patent infringement, then any rights granted to You under this License for this implementation of VP8 shall terminate as of the date such litigation is filed. 

-----------------------------------------------------------------

University of Illinois/NCSA Open Source License
Copyright (c) 2000-2004 University of Illinois Board of Trustees Copyright (c) 2000-2004 Mark D. Roth All rights reserved. 
Developed by: Campus Information Technologies and Educational Services, University of Illinois at Urbana-Champaign 

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the ``Software''), to deal with the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: * 

Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimers. * 

Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimers in the documentation and/or other materials provided with the distribution. * 

Neither the names of Campus Information Technologies and Educational Services, University of Illinois at Urbana-Champaign, nor the names of its contributors may be used to endorse or promote products derived from this Software without specific prior written permission. 

THE SOFTWARE IS PROVIDED ``AS IS'', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE CONTRIBUTORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS WITH THE SOFTWARE.

-----------------------------------------------------------------

Copyright (c) 2011 Google, Inc. 
 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: 
 
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software. 
 
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 

CityHash, by Geoff Pike and Jyrki Alakuijala 
---------------------------------------------------------------------
Microsoft Visual Studio 2017 Enterprise, Professional, Test Professional and Trial Edition
---------------------------------------------------------------------

Copyright (c) 2008-2010 The Khronos Group Inc.  

Permission is hereby granted, free of charge, to any person obtaining a  copy of this software and/or associated documentation files (the  "Materials"), to deal in the Materials without restriction, including  without limitation the rights to use, copy, modify, merge, publish,  distribute, sublicense, and/or sell copies of the Materials, and to  permit persons to whom the Materials are furnished to do so, subject to  the following conditions:   The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Materials.   

THE MATERIALS ARE PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,  EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF  MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY  CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,  TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE  MATERIALS OR THE USE OR OTHER DEALINGS IN THE MATERIALS.
---------------------------------------------------------------------------

LLVM Release License

University of Illinois/NCSA
Open Source License

Copyright (c) 2003-2013 University of Illinois at Urbana-Champaign.
All rights reserved.

Developed by:

    LLVM Team

    University of Illinois at Urbana-Champaign

    http://llvm.org

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal with
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
of the Software, and to permit persons to whom the Software is furnished to do
so, subject to the following conditions:

    * Redistributions of source code must retain the above copyright notice,
      this list of conditions and the following disclaimers.

    * Redistributions in binary form must reproduce the above copyright notice,
      this list of conditions and the following disclaimers in the
      documentation and/or other materials provided with the distribution.

    * Neither the names of the LLVM Team, University of Illinois at
      Urbana-Champaign, nor the names of its contributors may be used to
      endorse or promote products derived from this Software without specific
      prior written permission.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL THE
CONTRIBUTORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS WITH THE
SOFTWARE.

==============================================================================
Copyrights and Licenses for Third Party Software Distributed with LLVM:
==============================================================================
The LLVM software contains code written by third parties.  Such software will
have its own individual LICENSE.TXT file in the directory in which it appears.
This file will describe the copyrights, license, and restrictions which apply
to that code.

The disclaimer of warranty in the University of Illinois Open Source License
applies to all code in the LLVM Distribution, and nothing in any of the
other licenses gives permission to use the names of the LLVM Team or the
University of Illinois to endorse or promote products derived from this
Software.

The following pieces of software have additional or alternate copyrights,
licenses, and/or restrictions:

Program             Directory
-------             ---------
Autoconf            llvm/autoconf
                    llvm/projects/ModuleMaker/autoconf
                    llvm/projects/sample/autoconf
Google Test         llvm/utils/unittest/googletest
OpenBSD regex       llvm/lib/Support/{reg*, COPYRIGHT.regex}
pyyaml tests        llvm/test/YAMLParser/{*.data, LICENSE.TXT}
ARM contributions   llvm/lib/Target/ARM/LICENSE.TXT
md5 contributions   llvm/lib/Support/MD5.cpp llvm/include/llvm/Support/MD5.h
 
-----------------------------------------------------------------------
Microsoft Public License (Ms-PL)
This license governs use of the accompanying software. If you use the software, you accept this license. If you do not accept the license, do not use the software.
1.	Definitions
The terms "reproduce," "reproduction," "derivative works," and "distribution" have the same meaning here as under U.S. copyright law.
A "contribution" is the original software, or any additions or changes to the software.
A "contributor" is any person that distributes its contribution under this license.
"Licensed patents" are a contributor's patent claims that read directly on its contribution.
2.	Grant of Rights

A.	Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
B.	Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.
3.	Conditions and Limitations

A.	No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
B.	If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.
C.	If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.
D.	If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.
E.	The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement.

-------------------------------------------------------------------------------
MICROSOFT SOFTWARE LICENSE TERMS
MICROSOFT SENSOR DEVELOPMENT RESOURCES:
SENSOR DEVELOPMENT KIT, SENSOR AND LOCATION .NET INTEROPERABILITY SAMPLE LIBRARY, SENSOR DIAGNOSTIC TOOL, ENHANCED DEFAULT LOCATION APPLICATION
These license terms are an agreement between Microsoft Corporation (or based on where you live, one of its affiliates) and you.  Please read them.  They apply to the software named above, which includes the media on which you received it, if any.  The terms also apply to any Microsoft
•         updates,
•         supplements,
•         Internet-based services, and
•         support services
for this software, unless other terms accompany those items.  If so, those terms apply.
By using the software, you accept these terms.  If you do not accept them, do not use the software.

PLEASE SEE COMPLETE MICROSOFT SOFTWARE LICENSE TERMS AT: https://cdrdv2.intel.com/v1/dl/getContent/598826

----------------------------------------------------------------------------------- 
MICROSOFT WINDOWS SERVER 2003 DRIVER DEVELOPMENT KIT SERVICE PACK 1
IMPORTANT-READ CAREFULLY:  This End-User License Agreement ("EULA") is a legal agreement between you (either an individual or a single entity) and Microsoft Corporation ("Microsoft") for the Microsoft software that accompanies this EULA, which includes computer software and may include associated media, printed materials, "online" or electronic documentation, and Internet-based services ("Software"). An amendment or addendum to this EULA may accompany the Software. YOU AGREE TO BE BOUND BY THE TERMS OF THIS EULA BY INSTALLING, COPYING, OR OTHERWISE USING THE SOFTWARE. IF YOU DO NOT AGREE, DO NOT INSTALL, COPY, OR USE THE SOFTWARE; YOU MAY RETURN IT TO YOUR PLACE OF PURCHASE (IF APPLICABLE) FOR A FULL REFUND.
SEE COMPLETE END-USER LICENSE AT: https://cdrdv2.intel.com/v1/dl/getContent/598824

-------------------------------------------------------------------------------------
The MIT License
Copyright (c) <2018> 

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

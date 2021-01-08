fileexist -path $BOOTPATH/Language/$LANG/Poscalib.xml -label INSTALL_POSCALIB
goto -label END_INSTALL_POSCALIB
#INSTALL_POSCALIB
echo -text "Installing Poscalib.xml"
text -filename $BOOTPATH/Language/$LANG/Poscalib.xml -package $LANG
#END_INSTALL_POSCALIB

fileexist -path $BOOTPATH/Language/$LANG/CellText.xml -label INSTALL_CELLTEXT
goto -label END_INSTALL_CELLTEXT
#INSTALL_CELLTEXT
echo -text "Installing CellText.xml"
text -filename $BOOTPATH/Language/$LANG/CellText.xml -package $LANG
#END_INSTALL_CELLTEXT


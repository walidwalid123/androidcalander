fileexist -path $BOOTPATH/Language/$LANG/CellText.xml -label INSTALL_CELLTEXT
goto -label END_INSTALL_CELLTEXT
#INSTALL_CELLTEXT
echo -text "Installing CellText.xml"
text -filename $BOOTPATH/Language/$LANG/CellText.xml -package $LANG
#END_INSTALL_CELLTEXT


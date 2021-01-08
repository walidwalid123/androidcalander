@echo off
setlocal

set TRACE_DIR=%1
set SCRIPTS_PATH=%cd%

if not exist %TRACE_DIR% goto end

pushd %TRACE_DIR%
mkdir TMP_FILEVER_DIR
popd

pushd %SystemRoot%\System32\Drivers
for %%f in (Tbt*.sys) do (
	copy %%f %TRACE_DIR%\TMP_FILEVER_DIR
)
popd

pushd %SystemRoot%\System32
for %%f in (Tbt*.exe) do (
	copy %%f %TRACE_DIR%\TMP_FILEVER_DIR
)

for %%f in (Thunderbolt*.exe) do (
	copy %%f %TRACE_DIR%\TMP_FILEVER_DIR
)
popd

pushd %TRACE_DIR%
pushd TMP_FILEVER_DIR
for %%f in (*.*) do (
call %SCRIPTS_PATH%\filever.exe -v %%f >> ..\%%f.info.txt
)
popd
rmdir /Q /S TMP_FILEVER_DIR
popd


:end

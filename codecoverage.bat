@echo off
 
SET dotnet="C:\Program Files\dotnet\dotnet.exe" 
SET opencover="OpenCover.Console"
SET reportgenerator="ReportGenerator"
SET reportconverter = "OpenCoverToCoberturaConverter"
 
SET targetargs="test -l trx;logfilename=TestResults.trx" 
SET filter="+[*]*" 
SET coveragefile=Coverage.xml  
SET coveragedir=Coverage
 
REM cd "tests"
for /d %%i in (tests\*) do (
cd "%%i"
REM Run code coverage analysis  
%opencover% -oldStyle -register:user -target:%dotnet% -output:$scriptPath\%coveragefile% -targetargs:%targetargs% -filter:%filter% -skipautoprops -hideskipped:All -mergeoutput
)
REM cd ".."

start %reportconverter% "-input:$scriptPath\%coveragefile% -output:$scriptPath\%coveragedir%\Cobertura.xml"

REM Generate the report  
%reportgenerator% -targetdir:%coveragedir% -reporttypes:Latex;HtmlSummary -reports:%coveragefile% -verbosity:Error
 
REM Open the report  
REM start "report" "%coveragedir%\index.htm"
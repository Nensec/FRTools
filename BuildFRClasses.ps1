#the path to VSWhere.exe is always in programfiles(x86)

$progFilesx86Path = [System.Environment]::ExpandEnvironmentVariables("%programfiles(x86)%")
$vsWherePath = Join-Path $progFilesx86Path "\Microsoft Visual Studio\Installer\vswhere.exe"

# this tells vswhere to use paths of the latest version of visual studio installed 
# to locate this exe anywhere in those paths, and return a single textual 
# value (not a json object or xml payload)

$ttExe = & $vsWherePath -latest -find **\TextTransform.exe -format value
if (-Not(Test-Path $ttExe)){
    throw "Could not locate TextTransform.exe"
}

#then to invoke a transformation
& "$ttExe"  ".\FRTools.Core.Data\GeneratedFlightRisingClasses.tt"
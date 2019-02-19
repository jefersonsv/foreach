# Foreach
Turn easy the execution of loops, for and batch command line programs

[![License](http://img.shields.io/:license-mit-blue.svg)](http://gep13.mit-license.org) 
[![NuGet version (FFmpeg-Fluent)](https://img.shields.io/nuget/v/foreach.svg?style=flat-square)](https://www.nuget.org/packages/foreach/) 
[![Build status](https://ci.appveyor.com/api/projects/status/u3k9evib8q8i71tl?svg=true)](https://ci.appveyor.com/project/jefersonsv/foreach)

> **Foreach is looking for maintainers**

# Usage

```
Usage:
    foreach.exe files <file-pattern> [--input=<inputvariable>] [--bat=<batfilename>] [--fname=<fnamevariable>] [-full=<fullvariable>] [-index=<indexvariable>] [--ext=<extvariable>]
    foreach.exe filesrecursive <file-pattern> [--input=<input>] [--bat=<bat>] [--fname=<fname>] [-full=<full>] [-index=<index>] [--ext=<ext>]
    foreach.exe text <source-text-file> [--lambda=<eachline>] [--index=<indexvariable>] [--bat=<batfilename>]
    foreach.exe --version

Commands:
    files               Foreach in files founded. You can use a file pattern to search.
    filesrecursive      Foreach in files in all folders that are founded. You can use a file pattern to search.
    text                Foreach in all not blank lines inside a file.

Options:
    -p inputvariable, --input=<inputvariable>   Variable to be replaced with the input file.
    -n fnamevariable, --fname=<fnamevariable>   Variable to be replaced with the input file without extension.
    -u fullvariable, --full=<fullvariable>      Variable to be replaced with the full path and filename of input file.
    -i indexvariable, --index=<indexvariable>   Variable to be replaced with the index of loop each.
    -e extvariable, --ext=<extvariable>         Variable to be replaced with the extension of input file.
    -b batfilename, --bat=<batfilename>         Don't execute the command instead of create a batch file with all commands.
    -l eachline, --lambda=<eachline>            Variable to be replaced with each line of source text file.
    -h, --help                                  Show this screen
    --version                                   Show version
```
## Install

```
dotnet tool install -g foreach
```

## Uninstall

```
dotnet tool uninstall -g foreach
```

# Examples
* The command to execute its after * symbol

## Download a list of youtube videos

* Create of youtube urls in a file: list.txt
* The lambda variable "$1" will be replaced to each not blank line of list.txt

```bash
foreach.exe text list.txt --lambda $1 * youtube-dl $1
```

## Copy files in all folders to destination path

* Generate a command of copy all files with mask "log*.txt" to destionation folder "c:\logs"
* Save the commands to batch file "copy-logs.cmd"
* The lambda variable "__u1" will be replaced to full path and file name of each file found
* The lambda variable "__p1" will be replaced to only file name of each file found

```bash
foreach.exe filesrecursive "log*.txt" --bat copy-logs.cmd -p __p1 -u __u1 * copy "__u1" "c:\logs\__p1"
```

## Extract audio track from videos files

* It will create an batch file: runner.cmd with each command line to execute
* The file variable "$1" will be replaced to filename of each file founded
* The fname variable "$2" will be replaced to filename without extension of each file founded
* In your command line (after the*) you must escape the character " put a slash before then \"

```bash
foreach.exe files *.avi --bat runner-audio.cmd --file $1 --fname $2 * ffmpeg -i \"$1\"  \"$2.mp3\" 
```

**Note: You must have the ffmpeg to execute it**

# Thanks

- [Colorful.Console](https://github.com/tomakita/Colorful.Console) C# library that wraps around the System.Console class, exposing enhanced styling functionality
- [CliWrap](https://github.com/Tyrrrz/CliWrap) Wrapper for command line interface executables
- [docopt](https://github.com/docopt/docopt.net) Port of docopt to .net 
- [Microsoft.Extensions.FileSystemGlobbing](https://www.nuget.org/packages/Microsoft.Extensions.FileSystemGlobbing/) File system globbing to find files matching a specified pattern
- [Contessa](http://www.textfiles.com/art/contessa.flf) Font by Christopher Joseph Pirillo
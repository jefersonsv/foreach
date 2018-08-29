# Foreach
Turn easy the execution of loops, for and batch command line programs

[![License](http://img.shields.io/:license-mit-blue.svg)](http://gep13.mit-license.org) [![NuGet version (FFmpeg-Fluent)](https://img.shields.io/nuget/v/ffmpeg-fluent.svg?style=flat-square)](https://www.nuget.org/packages/foreach/)

> **Foreach is looking for maintainers**

# Usage

```
Usage:
    foreach.exe files <file-pattern> [--input=<inputvariable>] [--bat=<batfilename>] [--fname=<fnamevariable>] [-full=<fullvariable>] [-index=<indexvariable>] [--ext=<extvariable>]
    foreach.exe text <source-text-file> [--lambda=<eachline>] [--index=<indexvariable>] [--bat=<batfilename>]
    foreach.exe --version

Commands:
    files   Foreach in files founded. You can use a file pattern to search.
    text    Foreach in all not blank lines inside a file.

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

## Download a list of youtube videos

* The command to execute its after * symbol
* Create of youtube urls in a file: list.txt
* The lambda variable "$1" will be replaced to each not blank line of list.txt

```bash
foreach.exe text list.txt --lambda $1 * youtube-dl $1
```

## Extract audio track from videos files

* The command to execute its after * symbol
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
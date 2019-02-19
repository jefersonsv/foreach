using System;
using System.Collections.Generic;
using System.Text;

namespace Foreach
{
    public class Usage
    {
        /*
         * 
        --relpath RELPATHVARIABLE     Variable of relative path of the file
        */
        /// <summary>
        /// Try online
        /// <see cref="http://try.docopt.org"/>
        /// <see cref="https://stackoverflow.com/questions/16863371/why-doesnt-my-docopt-option-have-its-default-value"/>
        /// <see cref="https://dmerej.info/blog/post/docopt-v-argparse/"/>
        /// </summary>
        public const string SHORT_HELP = @"Foreach

Usage:
    foreach.exe files <file-pattern> [--input=<input>] [--bat=<bat>] [--fname=<fname>] [-full=<full>] [-index=<index>] [--ext=<ext>]
    foreach.exe filesrecursive <file-pattern> [--input=<input>] [--bat=<bat>] [--fname=<fname>] [-full=<full>] [-index=<index>] [--ext=<ext>]
    foreach.exe text <source-text-file> [--lambda=<lambda>] [--index=<index>] [--bat=<bat>]
    foreach.exe --version

Commands:
    files               Foreach in files founded. You can use a file pattern to search.
    filesrecursive      Foreach in files in all folders that are founded. You can use a file pattern to search.
    text                Foreach in all not blank lines inside a file.

Options:
    -p input, --input=<input>       Variable to be replaced with the input file.
    -n fname, --fname=<fname>       Variable to be replaced with the input file without extension.
    -u full, --full=<full>          Variable to be replaced with the full path and filename of input file.
    -i index, --index=<index>       Variable to be replaced with the index of loop each.
    -e ext, --ext=<ext>             Variable to be replaced with the extension of input file.
    -b bat, --bat=<bat>             Don't execute the command instead of create a batch file with all commands.
    -l lambda, --lambda=<lambda>    Variable to be replaced with each line of source text file.
    -h, --help                      Show this screen
    --version                       Show version
";

    }
}
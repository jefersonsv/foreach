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
    foreach.exe files <file-pattern> [-f=<filevariable>] [-b=<batfilename>] [-n=<fnamevariable>] [-u=<fullvariable>] [-i=<indexvariable>] [-e=<extvariable>]
    foreach.exe text <source-text-file> [--lambda=<eachline>] [--index=<indexvariable>] [--bat=<batfilename>]
    foreach.exe --version

Commands:
    files   Foreach in files founded. You can use a file pattern to search.
    text    Foreach in all not blank lines inside a file.

Options:
    -f filevariable, --file=<filevariable>      Variable to be replaced with the input file.
    -n fnamevariable, --fname=<fnamevariable>   Variable to be replaced with the input file without extension.
    -u fullvariable, --full=<fullvariable>      Variable to be replaced with the full path and filename of input file.
    -i indexvariable, --index=<indexvariable>   Variable to be replaced with the index of loop each.
    -e extvariable, --ext=<extvariable>         Variable to be replaced with the extension of input file.
    -b batfilename, --bat=<batfilename>         Don't execute the command instead of create a batch file with all commands.
    -l eachline, --lambda=<eachline>            Variable to be replaced with each line of source text file.
    -h, --help                                  Show this screen
    --version                                   Show version
";

        // files C:\Data\Temp\Videos\*.mp4 --bat teste.cmd --fname __FNAME__ --full __FULL__ * ffmpeg -n -i \"__FULL__\"  \"C:\Data\Temp\Videos\__FNAME__.mp3\" 
    }
}
//--bat youtube.cmd 
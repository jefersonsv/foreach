using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Services;
using Colorful;
using DocoptNet;
using Microsoft.Extensions.FileSystemGlobbing;
using Console = Colorful.Console;

namespace Foreach
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(string.Empty);
            var desc = "    Foreach";

            var fontArr = System.Text.Encoding.Default.GetBytes(ContessaFont.CONTESSA);
            FigletFont font = FigletFont.Load(fontArr);
            Figlet figlet = new Figlet(font);
            Console.WriteLine(figlet.ToAscii(desc), Color.Blue);

            Console.WriteFormatted("Turn easy the execution of loops, for and batch command line programs using statments like ", Color.White);
            Console.WriteLineFormatted("foreach ", Color.Green);

            try
            {
                var multiplierIndex = args.ToList().IndexOf("*");
                if (multiplierIndex == -1)
                    throw new DocoptNet.DocoptInputErrorException("You must to specify * multiplier character");

                var beforeArgs = SubArray<string>(args, 0, multiplierIndex);
                var afterArgs = SubArray<string>(args, multiplierIndex + 1, args.Length - multiplierIndex - 1);

                var arguments = new Docopt().Apply(Usage.SHORT_HELP, beforeArgs, version: "Foreach", exit: false);

                try
                {
                    Run(afterArgs, arguments).Wait();
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Environment.Exit(2);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(Usage.SHORT_HELP);
                Console.WriteLine(string.Empty);

                //Console.WriteLine($@"        > To convert all wav files in currenty folder (and sub-directories recursivelly) and convert to mp3 format");
                //Console.WriteLineFormatted($@"        ffmpeg-batch -s /**/*.wav -o mp3", Color.Green);
                //Console.WriteLine(string.Empty);
                //Console.WriteLine($@"        > To convert all wma files in c:\music and convert to mp3 format");
                //Console.WriteLineFormatted($@"        ffmpeg-batch -s c:\music\*.wma -o mp3", Color.Green);

                Console.WriteLine("Install/Uninstall tool:");
                Console.WriteLine($@"        > To install tool from system");
                Console.WriteLineFormatted($@"        dotnet tool install -g foreach", Color.Green);
                Console.WriteLine(string.Empty);
                Console.WriteLine($@"        > To uninstall tool from system");
                Console.WriteLineFormatted($@"        dotnet tool uninstall -g foreach", Color.Green);

                Environment.Exit(1);
            }
        }

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static (string, string) SplitPathFromPattern(string pathFullyQualified)
        {
            if (Path.IsPathFullyQualified(pathFullyQualified))
            {
                var lastSegment = Path.GetFileName(Path.GetFileName(pathFullyQualified));
                if (Path.GetInvalidFileNameChars().Any(s => lastSegment.Contains(s)))
                {
                    return (lastSegment, pathFullyQualified.Substring(0, pathFullyQualified.Length - lastSegment.Length));
                }
                else
                {
                    return (null, pathFullyQualified);
                }
            }
            else
            {
                return (pathFullyQualified, null);
            }
        }

        public static async Task Run(string[] afterArgs, IDictionary<string, ValueObject> param)
        {
            StringBuilder batString = new StringBuilder();
            batString.AppendLine("REM " + string.Join(' ', afterArgs));

            if (param["files"].IsTrue)
            {
                var splited = SplitPathFromPattern(param["<file-pattern>"].Value.ToString());
                var pattern = splited.Item1 ?? "*.*";
                var path = splited.Item2 ?? Environment.CurrentDirectory;

                var files = System.IO.Directory.GetFiles(path, pattern);

                for (int i = 0; i < files.Count(); i++)
                {
                    string command = string.Join(' ', afterArgs);
                    Dictionary<string, string> tokens = new Dictionary<string, string>();
                    PrintVariables();

                    command = Variables.IndexVariable(param, i, command, tokens);
                    command = Variables.InputVariable(param, files, i, command, tokens);
                    command = Variables.FullVariable(param, files, i, command, tokens);
                    command = Variables.FnameVariable(param, files, i, command, tokens);

                    ExecuteCommand(param, batString, command, tokens);
                }
            }
            else if (param["text"].IsTrue)
            {
                var sourceTextFile = param["<source-text-file>"].Value.ToString();
                var lines = System.IO.File.ReadAllLines(sourceTextFile);

                for (int i = 0; i < lines.Count(); i++)
                {
                    if (!string.IsNullOrEmpty(lines[i]))
                    {
                        string command = string.Join(' ', afterArgs);
                        Dictionary<string, string> tokens = new Dictionary<string, string>();
                        PrintVariables();

                        command = Variables.IndexVariable(param, i, command, tokens);
                        command = Variables.LambaVariable(param, lines[i], command, tokens);

                        ExecuteCommand(param, batString, command, tokens);
                    }
                }
            }

            if (param["--bat"] != null)
            {
                var batFile = param["--bat"].Value.ToString();
                File.WriteAllText(batFile, batString.ToString());
            }
        }

        private static void PrintVariables()
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLineFormatted($@"Variables: ", Color.CadetBlue);
        }

        private static void ExecuteCommand(IDictionary<string, ValueObject> param, StringBuilder batString, string command, Dictionary<string, string> tokens)
        {
            foreach (var token in tokens)
            {
                command = command.Replace(token.Key, token.Value);
            }


            if (param["--bat"] != null)
            {
                batString.AppendLine(command);
            }
            else
            {
                // Execute each batch
                
                using (var cli = new Cli("cmd"))
                {
                    var handler = new BufferHandler(
                            stdOutLine => Console.WriteLine(stdOutLine),
                            stdErrLine => Console.WriteLine(stdErrLine));

                    Console.WriteLine(string.Empty);

                    Console.WriteLineFormatted($@"Executing: ", Color.CadetBlue);
                    Console.WriteLineFormatted("    " + command, Color.White);
                    Console.WriteLine(string.Empty);

                    cli.Execute($@"/c ""{command}""", bufferHandler: handler);
                }
                
            }
        }
    }
}

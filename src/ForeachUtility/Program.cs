using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Services;
using Colorful;
using CommandLineParser.Arguments;
using CommandLineParser.Exceptions;
using DocoptNet;
using Microsoft.Extensions.FileSystemGlobbing;
using Console = Colorful.Console;

namespace ForeachUtility
{
    static class Program
    {
        /*
         * 
      --pat PATHVARIABLE            Variable of full path of the file
      --relpath RELPATHVARIABLE     Variable of relative path of the file

        */

        // foreach.exe files <file-pattern> [--file FILEVARIABLE] [--fname FNAMEVARIABLE] [--ext EXTVARIABLE] [--pat PATHVARIABLE] [--relpath RELPATHVARIABLE] [--index INDEXVARIABLE] *
        private const string usage = @"Foreach

    Usage:
        
      foreach.exe files <file-pattern> [--file=<filevariable>] [--fname=<fnamevariable>] [--full=<fullvariable>] [--index=<indexvariable>] [--ext=<extvariable>] [--sync=<kn>] *
      foreach.exe --version

      --file=<filevariable>  Speed in knots.
      --fname=<fnamevariable>  Speed in knots.
      --full=<fullvariable>  Speed in knots.
      --index=<indexvariable>  Speed in knots.
      --ext=<extvariable>  Speed in knots.

    Options:
      --sync=<kn>  Speed in knots [default: True].
      -h --help     Show this screen
      --version     Show version
    ";
        //[-e=[file-extension]] [-p=[path]] [-l=[relative-path]] [-i=index]
        //--file FILEVARIABLE           Variable of the name of the file
        //
        //--index INDEXVARIABLE         Variable of index of each file
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
        

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

            // -p *.pdb -l variavel2 -i  __idx__ * dir \"variavel2\" /s
            try
            {
                var multiplierIndex = args.ToList().IndexOf("*");
                if (multiplierIndex == -1)
                    throw new CommandLineException("You must to specify * multiplier character");

                var beforeArgs = SubArray<string>(args, 0, multiplierIndex + 1);
                var afterArgs = SubArray<string>(args, multiplierIndex + 1, args.Length - multiplierIndex - 1);

                var arguments = new Docopt().Apply(usage, beforeArgs, version: "Foreach", exit: false);

                Run(afterArgs, arguments).Wait();
                Environment.Exit(0);
            }

            catch (CommandLineException)
            {
#if (DEBUGE)
                

#endif

                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("Usage:");
                Console.WriteLine("        * ... Multiplier symbol. After it put the command to be executed");
                Console.WriteLine("        -l, --lambda[optional]... Specify lambda variable");
                Console.WriteLine("        -i, --index[optional]... Specify index variable");
                Console.WriteLine("        -p, --pattern[optional]... Specify file pattern to find");
                Console.WriteLine("        -w, --synchronous[optional]... Execute and wait command line");
                Console.WriteLine(string.Empty);
                
                Console.WriteLineFormatted($@"        dotnet tool install -g foreach", Color.Green);
                Console.WriteLine(string.Empty);
                Console.WriteLine($@"        > To uninstall tool from system");
                Console.WriteLineFormatted($@"        dotnet tool uninstall -g foreach", Color.Green);

                Console.WriteLine(string.Empty);
                Console.WriteLine("Examples:");
                //Console.WriteLine($@"        > To convert all wav files in currenty folder (and sub-directories recursivelly) and convert to mp3 format");
                //Console.WriteLineFormatted($@"        ffmpeg-batch -s /**/*.wav -o mp3", Color.Green);
                //Console.WriteLine(string.Empty);
                //Console.WriteLine($@"        > To convert all wma files in c:\music and convert to mp3 format");
                //Console.WriteLineFormatted($@"        ffmpeg-batch -s c:\music\*.wma -o mp3", Color.Green);

                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("Install/Uninstall tool:");
                Console.WriteLine($@"        > To install tool from system");
                Console.WriteLineFormatted($@"        dotnet tool install -g foreach", Color.Green);
                Console.WriteLine(string.Empty);
                Console.WriteLine($@"        > To uninstall tool from system");
                Console.WriteLineFormatted($@"        dotnet tool uninstall -g foreach", Color.Green);

                Environment.Exit(1);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error executing utility");
                Console.WriteLine(e.ToString());
                Environment.Exit(2);
            }
        }

        public static async Task Run(
                string[] afterArgs,
                IDictionary<string, ValueObject> param)
        {
            if (param["<file-pattern>"] != null)
            {
                var path = string.Empty;
                var pattern = string.Empty;
                if (Path.IsPathFullyQualified(param["<file-pattern>"].Value.ToString()))
                {
                    var lastSegment = Path.GetFileName(Path.GetFileName(param["<file-pattern>"].Value.ToString()));
                    if (Path.GetInvalidFileNameChars().Any(s => lastSegment.Contains(s)))
                    {
                        pattern = lastSegment;
                        path = param["<file-pattern>"].Value.ToString().Substring(0, param["<file-pattern>"].Value.ToString().Length - pattern.Length);
                    }
                    else
                    {
                        pattern = param["<file-pattern>"].Value.ToString();
                        path = Environment.CurrentDirectory;
                    }
                }
                else
                {
                    pattern = param["<file-pattern>"].Value.ToString();
                    path = Environment.CurrentDirectory;
                }

                var files = System.IO.Directory.GetFiles(path, pattern);

                for (int i = 0; i < files.Count(); i++)
                {
                    string command = string.Join(' ', afterArgs);
                    Dictionary<string, string> tokens = new Dictionary<string, string>();

                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLineFormatted($@"Variables: ", Color.CadetBlue);

                    // Update index
                    if (param["--index"] != null)
                    {
                        var guid = $@"{{{Guid.NewGuid().ToString().ToUpper()}}}";
                        if (command.IndexOf(param["--index"].Value.ToString()) >= 0)
                        {
                            var value = (i + 1).ToString();
                            tokens.Add(guid, value);
                            command = command.Replace(param["--index"].Value.ToString(), guid);

                            string msg = "  {0} = {1}";
                            Formatter[] fmts = new Formatter[]
                            {
                                new Formatter(param["--index"].Value.ToString(), Color.LightGoldenrodYellow),
                                new Formatter(value, Color.Pink)
                            };

                            Console.WriteLineFormatted(msg, Color.White, fmts);
                        }
                    }

                    // Update file variable
                    if (param["--file"] != null)
                    {
                        var guid = $@"{{{Guid.NewGuid().ToString().ToUpper()}}}";
                        if (command.IndexOf(param["--file"].Value.ToString()) >= 0)
                        {
                            var value = Path.GetFileName(files[i]);
                            tokens.Add(guid, value);
                            command = command.Replace(param["--file"].Value.ToString(), guid);

                            string msg = "  {0} = {1}";
                            Formatter[] fmts = new Formatter[]
                            {
                                new Formatter(param["--file"].Value.ToString(), Color.LightGoldenrodYellow),
                                new Formatter(value, Color.Pink)
                            };

                            Console.WriteLineFormatted(msg, Color.White, fmts);
                        }
                    }

                    // Update full variable
                    if (param["--full"] != null)
                    {
                        var guid = $@"{{{Guid.NewGuid().ToString().ToUpper()}}}";
                        if (command.IndexOf(param["--full"].Value.ToString()) >= 0)
                        {
                            var value = Path.GetFullPath(files[i]);
                            tokens.Add(guid, value);
                            command = command.Replace(param["--full"].Value.ToString(), guid);

                            string msg = "  {0} = {1}";
                            Formatter[] fmts = new Formatter[]
                            {
                                new Formatter(param["--full"].Value.ToString(), Color.LightGoldenrodYellow),
                                new Formatter(value, Color.Pink)
                            };

                            Console.WriteLineFormatted(msg, Color.White, fmts);
                        }
                    }

                    // Update fname variable
                    if (param["--fname"] != null)
                    {
                        var guid = $@"{{{Guid.NewGuid().ToString().ToUpper()}}}";
                        if (command.IndexOf(param["--fname"].Value.ToString()) >= 0)
                        {
                            var value = Path.GetFileNameWithoutExtension(files[i]);
                            tokens.Add(guid, value);
                            command = command.Replace(param["--fname"].Value.ToString(), guid);

                            string msg = "  {0} = {1}";
                            Formatter[] fmts = new Formatter[]
                            {
                                new Formatter(param["--fname"].Value.ToString(), Color.LightGoldenrodYellow),
                                new Formatter(value, Color.Pink)
                            };

                            Console.WriteLineFormatted(msg, Color.White, fmts);
                        }
                    }


                    foreach (var token in tokens)
                    {
                        command = command.Replace(token.Key, token.Value);
                    }

                    // Execute each batch
                    if (param["--sync"] != null && param["--sync"].Value.ToString() == true.ToString() )
                    {
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
    }
}

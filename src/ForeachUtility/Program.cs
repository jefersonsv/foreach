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
using Microsoft.Extensions.FileSystemGlobbing;
using Console = Colorful.Console;

namespace ForeachUtility
{
    static class Program
    {
        private const string usage = @"Naval Fate.

    Usage:
        
      foreach.exe files <find-pattern> <var> l
      foreach.exe --version

    Options:
      -h --help     Show this screen.
      --version     Show version.
      --speed=<kn>  Speed in knots [default: 10].
      --moored      Moored (anchored) mine.
      --drifting    Drifting mine.

    ";

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
        

        static void Main(string[] args)
        {
            var desc = "    Foreach";

            var fontArr = System.Text.Encoding.Default.GetBytes(ContessaFont.CONTESSA);
            FigletFont font = FigletFont.Load(fontArr);
            Figlet figlet = new Figlet(font);
            Console.WriteLine(figlet.ToAscii(desc), Color.Blue);

            Console.WriteFormatted("Turn easy the execution of loops, for and batch command line programs using statments like ", Color.White);
            Console.WriteLineFormatted("foreach ", Color.Green);

            // Arguments
            ValueArgument<string> lambda = new ValueArgument<string>(
                'l', "lambda", "Specify lambda variable");

            ValueArgument<string> index = new ValueArgument<string>(
                'i', "index", "Specify index variable");

            ValueArgument<string> pattern = new ValueArgument<string>(
                'p', "pattern", "Specify file pattern to find");

            SwitchArgument synchronous = new SwitchArgument (
                'w', "synchronous", "Execute and wait command line", true);

            var parser = new CommandLineParser.CommandLineParser();
            parser.Arguments.Add(lambda);
            parser.Arguments.Add(index);
            parser.Arguments.Add(pattern);
            parser.Arguments.Add(synchronous);

            var multiplierIndex = args.ToList().IndexOf("*");
            var beforeArgs = SubArray<string>(args, 0, multiplierIndex);
            var afterArgs = SubArray<string>(args, multiplierIndex + 1, args.Length - multiplierIndex - 1);
            
            try
            {
                parser.ParseCommandLine(beforeArgs);
                Run(afterArgs, pattern, index, lambda, synchronous).Wait();
                Environment.Exit(0);
            }
            catch (CommandLineException e)
            {

            }
        }

        public static async Task Run(
                string[] afterArgs,
                ValueArgument<string> pattern,
                ValueArgument<string> index,
                ValueArgument<string> lambda,
                SwitchArgument synchronous)
        {
            string command = string.Join(' ', afterArgs);
            Dictionary<string, string> tokens = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(pattern.Value))
            {
                var files = System.IO.Directory.GetFiles(Environment.CurrentDirectory, pattern.Value);

                for (int i = 0; i < files.Count(); i++)
                {
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLineFormatted($@"Variables: ", Color.CadetBlue);

                    // Update index
                    if (!string.IsNullOrEmpty(index.Value))
                    {
                        var guid = $@"{{{Guid.NewGuid().ToString().ToUpper()}}}";
                        if (command.IndexOf(index.Value) >= 0)
                        {
                            var value = (i + 1).ToString();
                            tokens.Add(guid, value);
                            command = command.Replace(index.Value, guid);

                            string msg = "  {0} = {1}";
                            Formatter[] fmts = new Formatter[]
                            {
                                new Formatter(index.Value, Color.LightGoldenrodYellow),
                                new Formatter(value, Color.Pink)
                            };

                            Console.WriteLineFormatted(msg, Color.White, fmts);
                        }
                    }

                    // Update lambda
                    if (!string.IsNullOrEmpty(lambda.Value))
                    {
                        var guid = $@"{{{Guid.NewGuid().ToString().ToUpper()}}}";
                        if (command.IndexOf(lambda.Value) >= 0)
                        {
                            var value = files[i];
                            tokens.Add(guid, files[i]);
                            command = command.Replace(lambda.Value, guid);

                            string msg = "  {0} = {1}";
                            Formatter[] fmts = new Formatter[]
                            {
                                new Formatter(index.Value, Color.LightGoldenrodYellow),
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
                    if (synchronous.Value)
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

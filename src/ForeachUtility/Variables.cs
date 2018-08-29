using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Colorful;
using DocoptNet;
using Console = Colorful.Console;

namespace ForeachUtility
{
    public static class Variables
    {
        public static string FnameVariable(IDictionary<string, ValueObject> param, string[] files, int i, string command, Dictionary<string, string> tokens)
        {
            // Update fname variable
            if (param["--fname"] != null)
            {
                var realValue = Path.GetFileNameWithoutExtension(files[i]);
                var paramValue = param["--fname"].Value.ToString();
                command = ReplaceCommandIfNeeded(command, tokens, realValue, paramValue);
            }

            return command;
        }

        public static string FullVariable(IDictionary<string, ValueObject> param, string[] files, int i, string command, Dictionary<string, string> tokens)
        {
            // Update full variable
            if (param["--full"] != null)
            {
                var realValue = Path.GetFullPath(files[i]);
                var paramValue = param["--full"].Value.ToString();
                command = ReplaceCommandIfNeeded(command, tokens, realValue, paramValue);
            }

            return command;
        }

        public static string FileVariable(IDictionary<string, ValueObject> param, string[] files, int i, string command, Dictionary<string, string> tokens)
        {
            // Update file variable
            if (param["--file"] != null)
            {
                var realValue = Path.GetFileName(files[i]);
                var paramValue = param["--file"].Value.ToString();
                command = ReplaceCommandIfNeeded(command, tokens, realValue, paramValue);
            }

            return command;
        }

        public static string IndexVariable(IDictionary<string, ValueObject> param, int i, string command, Dictionary<string, string> tokens)
        {
            // Update index
            if (param["--index"] != null)
            {
                var realValue = (i + 1).ToString();
                var paramValue = param["--index"].Value.ToString();
                command = ReplaceCommandIfNeeded(command, tokens, realValue, paramValue);
            }

            return command;
        }

        public static string LambaVariable(IDictionary<string, ValueObject> param, string realValue, string command, Dictionary<string, string> tokens)
        {
            // Update index
            if (param["--lambda"] != null)
            {
                var paramValue = param["--lambda"].Value.ToString();
                command = ReplaceCommandIfNeeded(command, tokens, realValue, paramValue);
            }

            return command;
        }

        private static string ReplaceCommandIfNeeded(string command, Dictionary<string, string> tokens, string realValue, string paramValue)
        {
            if (command.IndexOf(paramValue) >= 0)
            {
                command = AddToken(command, tokens, paramValue, realValue);
            }

            return command;
        }

        private static string AddToken(string command, Dictionary<string, string> tokens, string paramValue, string value)
        {
            var guid = $@"{{{Guid.NewGuid().ToString().ToUpper()}}}";
            tokens.Add(guid, value);
            command = command.Replace(paramValue, guid);
            DebugParameter(paramValue, value);
            return command;
        }

        private static void DebugParameter(string paramName, string value)
        {
            string msg = "  {0} = {1}";
            Formatter[] fmts = new Formatter[]
            {
                new Formatter(paramName, Color.LightGoldenrodYellow),
                new Formatter(value, Color.Pink)
            };

            Console.WriteLineFormatted(msg, Color.White, fmts);
        }
    }
}

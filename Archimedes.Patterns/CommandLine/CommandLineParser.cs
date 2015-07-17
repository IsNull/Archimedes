using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Archimedes.Patterns.CommandLine
{
    /// <summary>
    /// Utility to parse command-line arguments into a CommandLineOptions object
    /// </summary>
    public static class CommandLineParser
    {

        /// <summary>
        /// Parses commandline arguments into a generic CommandLineOptions object.
        /// </summary>
        /// <param name="argString"></param>
        /// <returns></returns>
        public static CommandLineOptions ParseCommandLineArgs(string argString)
        {
            // Tokenize the argument string, which is actually splitting it by spaces.
            // However, there are escape sequences to support spaces as part of the argument

            string[] tokens = TokenizeCmdArgs(argString).ToArray();
            return ParseCommandLineArgs(tokens);
        }


        /// <summary>
        /// Parses commandline arguments into a generic CommandLineOptions object.
        /// 
        /// Supported Format
        /// 
        /// [parameter1 parameter2 parameter3] /flag /flagWithValue value
        /// [parameter1 parameter2 parameter3] -flag -flagWithValue value
        /// 
        /// 
        /// Specifing a flag without a value is equally as setting it to true
        /// Example
        /// '-quiet' has the same effect '-quiet true'
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static CommandLineOptions ParseCommandLineArgs(string[] args)
        {
            var commandLineOptions = new CommandLineOptions();
            var flagParser = new Regex("[/|-](.*)$");

            string currentFlag = null;

            for (int i = 0; i != args.Length; ++i)
            {
                string current = args[i];

                if (flagParser.IsMatch(current))
                {
                    if (currentFlag != null)
                    {
                        commandLineOptions.SetParameterEnabled(currentFlag);
                    }
                    currentFlag = flagParser.Match(current).Groups[1].Value;
                }
                else
                {
                    // Argument
                    if (currentFlag == null)
                    {
                        commandLineOptions.AddArgument(current);
                    }
                    else
                    {
                        commandLineOptions.SetParameterValue(currentFlag, current);
                        currentFlag = null;
                    }
                }
            }

            return commandLineOptions;
        }



        /// <summary>
        /// Tokenizes a cmd argument string the same way the internal c# args are prepared.
        /// This will generally split tokens on spaces, but supports also escape sequences with quote.
        /// </summary>
        /// <param name="argString"></param>
        /// <returns></returns>
        public static IEnumerable<string> TokenizeCmdArgs(string argString)
        {
            const char delemiter = ' ';
            const char escapeToggle = '"';

            bool inEscapeSequence = false;
            int start = 0;

            for (int i = 0; i < argString.Length; i++)
            {
                if (!inEscapeSequence && argString[i] == delemiter)
                {
                    yield return argString.Substring(start, i - start).Trim(escapeToggle);
                    start = i+1;
                }
                else if (argString[i] == escapeToggle)
                {
                    inEscapeSequence = !inEscapeSequence;
                }
            }

            if (inEscapeSequence) throw new FormatException("The given argument string misses a closing escape (" + escapeToggle + ") switch!");

            yield return argString.Substring(start, argString.Length - start).Trim(escapeToggle);
        }
    }
}

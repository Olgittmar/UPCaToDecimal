using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;

namespace UPCaToDecimalApp
{
    class Program
    {
        public class CommandLineOptions
        {
            [Value(
                index: 0,
                MetaName = "InputFile",
                Required = true,
                HelpText = "Input file containing one UPC-A per row. [UTF-8 no BOM, CRLF]")]
            public string InputFile { get; set; }
            
            [Option(
                shortName: 'o',
                longName: "OutputFile",
                Default = "",
                HelpText = "Name of file to where results will be printed. If left out results will be printed to console instead.")]
            public string OutputFile { get; set; }

            [Option(
                shortName: 'c',
                Default = false,
                HelpText = "If true force results to be printed to console.")]
            public bool OutputToConsole { get; set; }
        }
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(RunWithOptions)
                .WithNotParsed(HandleParseError);
        }

        static void RunWithOptions(CommandLineOptions opts)
        {
            if (opts.InputFile.Length == 0) {
                return;
            }
            if (!File.Exists(opts.InputFile)) {
                return;
            }
            string results = "";
            using (StreamReader sr = File.OpenText(opts.InputFile))
            {
                string line;
                while((line = sr.ReadLine()) != null)
                {
                    // append results of each line read to 'results'
                    if (results.Length != 0) {
                        results += "\r\n";
                    }
                    results += HelperProgram.Convert_UPC_A_To_Decimal_String( line );
                }
            }
            if(opts.OutputFile.Length != 0)
            {
                using (StreamWriter sw = File.CreateText(opts.OutputFile))
                {
                    sw.Write(results);
                }
            }
            if (opts.OutputToConsole || opts.OutputFile.Length == 0)
            {
                foreach (string line in results.Split("\r\n"))
                {
                    Console.WriteLine(line);
                }
            }
        }

        static void HandleParseError(IEnumerable<Error> errs)
        {
            foreach( Error err in errs )
            {
                // not sure what errors could occur yet, so not sure how to handle them...
                Console.WriteLine(err.ToString());
            }
        }
    }
}

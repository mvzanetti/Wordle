using System;

using System.Collections.Generic; // For arguments parsing

namespace Game
{

    public class CommandLineArguments 
    {
        private Dictionary<string, string> argumentMap;

        public CommandLineArguments(string[] args) 
        {
            argumentMap = new Dictionary<string, string>();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("--"))
                {
                    if (i + 1 < args.Length)
                    {
                        argumentMap[args[i].Substring(2)] = args[i + 1];
                        i++;
                    }
                }
                else if (args[i].StartsWith("-") && args[i].Length == 2) 
                {
                    if (i + 1 < args.Length)
                    {
                        argumentMap[args[i].Substring(1)] = args[i + 1];
                        i++;
                    }
                }
                else
                {
                    // Handle unknown cases
                }
            }
        }

        public string GetValue(string key) 
        {
            if (argumentMap.TryGetValue(key, out string value))
            {
                return value;
            }
            return null;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {

            // Initial check to see arguments input

            var commandLineArgs = new CommandLineArguments(args);

            if (args == null || args.Length == 0)
            {
                Console.WriteLine("No arguments input. At least input the game mode with -m:\n\n-m play -> Play the game");
            }
            else
            {
                // arguments
                //foreach (s)

                Console.WriteLine("arg 0 -> {0}", commandLineArgs.GetValue("m"));
                Console.WriteLine("arg 1 -> {0}", commandLineArgs.GetValue("help"));

            }
        }
    }
}

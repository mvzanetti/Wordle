using System;

using System.Collections.Generic; // For arguments parsing

using System.IO; // For reading files
using System.Linq.Expressions;
using System.Net;

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
                    //argumentMap["a"] = "a";
                    //var mytriple = new List<Tuple<string, int, bool>>();
                    //var mydic = new Dictionary<string,Tuple<int,int>>();
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

    public class WordleGame
    {
        private bool _isRandomGame;
        private int _n_try;
        private string _guess;
        private string _word;
        private string _path;

        public WordleGame(bool isRandomGame, int n_try, string guess, string word, string path) 
        {
            _isRandomGame = isRandomGame;
            _n_try = n_try;
            _guess = guess;
            _word = word;
            _path = path;
        }

        public string play_game() 
        {
            if (!this._isRandomGame)
            {
                return null;
            }
            else
            {
                var random = new Random();
                var list = new List<string>();
                list = readfile(this._path);
                string new_game_word = list[random.Next(0,list.Count)];

                Console.WriteLine("your guess:{0}, the word:{1}, the score:{2}", this._guess, new_game_word, getGameResult(this._guess, new_game_word));

                return getGameResult(this._guess, new_game_word);
            }
        }

        private string getGameResult(string guess, string word) 
        {

            string result = "";

            bool aux = false;

            // First loop to run on each letter separately

            // Second loop checks the match on every letter of the correct word

            // ** NEED TO IMPLEMENT CHECK ON DOUBLE LETTERS APPEARANCES

            for (int i = 0; i < guess.Length; i++) 
            {

                for (int j = 0; j < guess.Length; j++) 
                {
                    aux = false;
                    if (String.Equals(guess[i], word[j]))
                    {
                        if (i == j)
                        {
                            result += "2";
                            break;
                        }
                        else
                        {
                            result += "1";
                            break;
                        }
                    }
                    else 
                    {
                        
                    }
                    aux = true;
                }
                if (aux) 
                {
                    result += "0";
                }

            }

            return result;

        }

        //private int

        // make method to return chars locations, make method to check single instance of game, make method to play full game

        private static List<string> readfile(string path)
        {
            List<string> wordList = new List<string>();

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        wordList.Add(line);
                    }
                }
                return wordList;
            }
            catch (FileNotFoundException)
            {

                Console.WriteLine("File not found.");
                return null;
            }
            catch (Exception ex)
            {

                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }

        }

    }


    class Program
    {
        static int Main(string[] args)
        {

            // Initial check to see arguments input

            var commandLineArgs = new CommandLineArguments(args);

            if (args == null || args.Length == 0)
            {
                Console.WriteLine("No arguments input. At least input the game mode with -m:\n\n-m play -> Play the game");

                return -1;
            }
            else
            {
                // arguments
                //foreach (s)

                Console.WriteLine("arg 0 -> {0}", commandLineArgs.GetValue("m"));
                Console.WriteLine("arg 1 -> {0}", commandLineArgs.GetValue("help"));
                Console.WriteLine("hhelkp");

                Console.WriteLine(args.Length.ToString());

                /*foreach (var item in readfile("Lists/official_wordle_common.txt"))
                {
                    Console.WriteLine(item);
                }*/

                var game = new WordleGame(true, 1, "crane","", "Lists/official_wordle_common.txt");

                game.play_game();



                return 0;
            }
        }

    }
}

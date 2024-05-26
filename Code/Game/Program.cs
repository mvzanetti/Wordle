using System;

using System.Collections.Generic; // For arguments parsing

using System.IO; // For reading files
using System.Linq.Expressions;
using System.Net;
using System.Security.Cryptography;

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
                     //if (i + 1 < args.Length)
                    //{
                        argumentMap[args[i].Substring(2)] = args[i].Substring(2);
                        i++;
                    //}
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
        public bool isRandomGame;
        public int n_try;
        public string guess;
        public string word;
        public string path;

        private bool _isRandomGame;
        private int _n_try;
        private string _guess;
        private string _word;
        private string _path;

        private bool flag = false;

        public WordleGame(string guess, string word)
        {
            this.flag = true; // Maybe we won't need this approach to differentiate the xtors different calls
        }

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
                Console.WriteLine("your guess:{0}, the word:{1}, the score:{2}", this._guess, this._word, getGameResult(this._guess, this._word));

                // getGameResult called twice for the same effort here, fix...            
                return getGameResult(this._guess, this._word);
            }
            else
            {
                var random = new Random();
                var list = new List<string>();
                list = readfile(this._path);
                string new_game_word = list[random.Next(0, list.Count)];

                Console.WriteLine("your guess:{0}, the word:{1}, the score:{2}", this._guess, new_game_word, getGameResult(this._guess, new_game_word));

                return getGameResult(this._guess, new_game_word);
            }
        }


        // Returns game result as a string of 5 numbers. First input is the guess, and the second one is the word to be tested against. Like the Wordle game, 0 means an error (grey), 1 means a correct letter but not on the spot (yellow), and 2 means a match on that spot (green).
        public string getGameResult(string guess, string word)
        {

            string result = "";

            // This function still fails with -> g= bwosh, w = swosh. Need to fix. *** FIXED, MF.

            // The below array represents the Word, not the Guess

            bool[] guesses_result = { false, false, false, false, false };

            // Aux is a variable to check if a match has not been made inside the inner loop. if not, we need to put a 0.
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

                        if (guesses_result[j] == false)
                        {

                            // Commented the line below to insert it manually on pinpointed locations
                            //guesses_result[j] = true;
                            if (i == j)
                            {
                                result += "2";
                                guesses_result[j] = true; // This line is necessary because this is standard in this loop
                                break;
                            }
                            else
                            {
                                // This if handles the special case where one letter comes right but comes as a yellow, and there's still a correct letter on the same spot (green). Ex: -> g= bwosh, w = swosh 
                                if (String.Equals(guess[i], word[i]))
                                {
                                    result += "2"; // This line lacks the "guesses_results[j] = true"  because it's a odd appearance
                                    break;
                                }
                                // This if handles the same but inverted. If not this this part, an input of g = aabaa and w = ababa would result in -> 20102, should've been 21102
                                else if (String.Equals(guess[j], word[j]))
                                {
                                    result += "0"; // This line lacks the "guesses_results[j] = true"  because it's a odd appearance
                                    break;

                                }

                                result += "1";
                                guesses_result[j] = true; // This line is necessary because this is standard in this loop
                                break;
                            }
                        }
                    }
                    // remove unnecessary else
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

        public List<string> readfile(string path)
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

        public void printColoredResults(string result)
        {
            for (int i = 0; i < result.Length; i++)
            {

                Console.ResetColor();

                if (String.Equals(result[i], '1'))
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                if (String.Equals(result[i], '2'))
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                }

                Console.Write(guess[i]);

            }
            Console.ResetColor();
        }

    }

    public class Information_Tools 
    {

        void get_info(string word, string path) // choose better func name 
        {
            WordleGame game = new WordleGame(false, 1, "", "", "Lists/official_wordle_common.txt");

            //List<string> list = game.readfile(path);

            //Dictionary<string, string[]> keyValuePairs = new Dictionary<string, string[]>(); // change this name also

            List<string> list = game.readfile(path);

            Dictionary<string, List<string>> keyValuePairs = new Dictionary<string, List<string>>(); // change this name also

            for (int i = 0; i < list.Count; i++) 
            {
                //comment only to make compiler run, fix below
               // keyValuePairs.Add(list[i], List<string>.Add(game.getGameResult(list[i],word))); // check if is needed the string[], I think its a pre step towards the goal
            }



            return;
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

                //Console.WriteLine("arg 0 -> {0}", commandLineArgs.GetValue("m"));
                //Console.WriteLine("arg 1 -> {0}", commandLineArgs.GetValue("help"));

                //Console.WriteLine(args.Length.ToString());

                /*foreach (var item in readfile("Lists/official_wordle_common.txt"))
                {
                    Console.WriteLine(item);
                }*/

                string guess = commandLineArgs.GetValue("g");

                string word = commandLineArgs.GetValue("w");

                string random_string = commandLineArgs.GetValue("random");

                Console.WriteLine(random_string);

                bool random = false;

                if (random_string != null) 
                {
                    random = true;
                }

                var game = new WordleGame(random, 1, guess, word, "Lists/official_wordle_common.txt");

                string result = game.play_game();

                //Console.WriteLine("your guess:{0}, the word:{1}, the score:{2}", "ambar", "riser", result);

                Console.WriteLine();

                game.printColoredResults(result);


                // To future batch programming ->

                // Fix main match function

                // Word recognizing is OK, colors OK. Implement 5 tries game as standard and change limit per argument before runnning the words entropy

                // Make better checks for arguments, make ifs to check mass unecessary input

                // Current way to call exe -> Game.exe -g crane -w amber --random

                //Console.BackgroundColor = ConsoleColor.Blue;
                //Console.ForegroundColor = ConsoleColor.White;
                //Console.WriteLine("White on blue.");
                //Console.ResetColor();


                // For entropy calculations:

                // Do a program that computes word by word on the list, the expected information to be given by adding all other calculations from other words, except the one we're using

                // make a function that work with a string as input (later will be word_Array[i] as input). The function will loop the entire word list, and store in an array the results
                // Later a collector inside or not of the function will sort and bucket the words into the patterns (can be a array)

                return 0;
            }
        }

    }
}

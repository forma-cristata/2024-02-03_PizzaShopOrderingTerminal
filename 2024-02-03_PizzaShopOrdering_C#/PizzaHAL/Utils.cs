using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace PizzaHAL
{
    public static class Utils
    {
        private const ConsoleColor FLICKER_COLOR = ConsoleColor.DarkGray;
        private const ConsoleColor PROMPT_COLOR = ConsoleColor.Red;
        private const ConsoleColor INPUT_COLOR = ConsoleColor.DarkMagenta;
        private const String INTRO_SENTENCE = "To The Devilish Deli";
        private const int MAX_SHOWN_REVIEWS = 3;
        public static void Flicker()
        {//Presents a screen-short mockup in between console.Clears in different sections of the program
            Console.Clear();
            Console.BackgroundColor = FLICKER_COLOR;
            for (int i = 0; i < Console.LargestWindowHeight; i++)
            {
                Console.WriteLine(new String(' ', Console.LargestWindowWidth));
            }
            Console.ResetColor();
            Console.Clear();
            Console.ForegroundColor = FLICKER_COLOR;
            for (int i = 0; i < Console.LargestWindowHeight; i++)
            {
                Console.WriteLine(new String('x', Console.LargestWindowWidth));
            }
            Console.ResetColor();
            Console.Clear();
        }
        public static void BeginningPrint()
        {
            int welcomingWidth = "'--'   '--'   `------'`------'   `-----'      `-----' `--'   `--'  `------' ".Length;//This is a line of the welcome message
            Console.ForegroundColor = Program.TABLE_COLOR;
            Console.WriteLine($"{Environment.NewLine}" +
                              $"{Environment.NewLine}  (`\\ .-') /`   ('-.                                  _   .-')       ('-.   " +
                              $"{Environment.NewLine}   `.( OO ),' _(  OO)                                ( '.( OO )_   _(  OO)  " +
                              $"{Environment.NewLine},--./  .--.  (,------.,--.       .-----.  .-'),-----. ,--.   ,--.)(,------. " +
                              $"{Environment.NewLine}|      |  |   |  .---'|  |.-')  '  .--./ ( OO'  .-.  '|   `.'   |  |  .---' " +
                              $"{Environment.NewLine}|  |   |  |,  |  |    |  | OO ) |  |('-. /   |  | |  ||         |  |  |     " +
                              $"{Environment.NewLine}|  |.'.|  |_)(|  '--. |  |`-' |/_) |OO  )\\_) |  |\\|  ||  |'.'|  | (|  '--.  " +
                              $"{Environment.NewLine}|         |   |  .--'(|  '---.'||  |`-'|   \\ |  | |  ||  |   |  |  |  .--'  " +
                              $"{Environment.NewLine}|   ,'.   |   |  `---.|      |(_'  '--'\\    `'  '-'  '|  |   |  |  |  `---. " +
                              $"{Environment.NewLine}'--'   '--'   `------'`------'   `-----'      `-----' `--'   `--'  `------' " +
                              $"{Environment.NewLine}{Environment.NewLine}");
            Thread.Sleep(1500);
            Console.WriteLine(new String(' ', (welcomingWidth - INTRO_SENTENCE.Length) / 2) + INTRO_SENTENCE);
            List<String> sampledReviews = RandomLine();
            if (sampledReviews != null)//If there were reviews in the file
            {
                foreach (String review in sampledReviews)
                {
                    Thread.Sleep(1000);
                    String print;
                    if (review.Length > welcomingWidth)
                    {//If the review is too long to be centered, then we trim it and add ... to the end
                        print = $"{review}"[..(welcomingWidth - "...".Length)] + "...";
                    }
                    else { print = review; }
                    Console.Write(Environment.NewLine +
                                  Environment.NewLine +
                                  new String(' ', (welcomingWidth - print.Length) / 2) + //Print them centered.
                                  $"\"{print}\"" + Environment.NewLine);
                }
            }
            Console.ReadKey();
            Flicker(); Flicker();
            Console.Clear();
        }
        private static List<String> RandomLine()
        {//returns up to 3 random reviews from previous uses of the program upon the welcoming print
            Random rand = new();
            List<String> reviewsPrinted = [];
            List<String> reviewsSample = [];
            try { reviewsSample = [.. File.ReadAllLines(Program.REVIEWS)]; }
            catch { reviewsSample = []; }
            if (reviewsSample.Count > 0)
            {
                for (int i = 0; i < reviewsSample.Count; i++)//Can only iterate if there are reviews to be seen (1 review or 2 reviews only)
                {
                    if (i == MAX_SHOWN_REVIEWS) { break; }//Can only return 3 reviews max
                    int index = rand.Next(reviewsSample.Count);//Picks a random review from the array of lines from the file
                    reviewsPrinted.Add(reviewsSample[index]);
                    reviewsSample.RemoveAt(index);
                }
                return reviewsPrinted;
            }
            else { return null; }//Returns nothing if there are no reviews
        }
        public static void PrintPrompt(String prompt)//Changes the color of the prompt message.
        {
            Console.ForegroundColor = PROMPT_COLOR;
            Console.Write(prompt);
        }
        public static String ChangeConsoleColorAndTrimAndLowerInput()
        {
            Console.ForegroundColor = INPUT_COLOR;
            String input = Console.ReadLine().Trim().ToLower();
            return input;
        }//Trims, Lowers, and Recolors all input.
        public static String InputValidation(String[] validInputs, String input)//Portable input validation method.
        {
           
            while (String.IsNullOrWhiteSpace(input) || !validInputs.Contains(input, StringComparer.OrdinalIgnoreCase))
            {//Checks if input is empty and if it is a valid answer.
                Console.ForegroundColor = PROMPT_COLOR;
                Console.Write($"Your response, \"{input}\", is gibberish... Try again{Environment.NewLine}{Environment.NewLine}>>");
                input = Utils.ChangeConsoleColorAndTrimAndLowerInput();//Reprompts the user
            }
            return input;
        }
        public static String InputValidationNullsOkay(String[] validInputs, String input)//Portable input validation method for categories, where null indicates a choice as well
        {
            
            while (!String.IsNullOrWhiteSpace(input) && !validInputs.Contains(input, StringComparer.OrdinalIgnoreCase))
            {//Checks if input is empty and if it is a valid answer.
                Console.ForegroundColor = PROMPT_COLOR;
                Console.Write($"Your response, \"{input}\", is gibberish... Try again{Environment.NewLine}{Environment.NewLine}>>");
                input = Utils.ChangeConsoleColorAndTrimAndLowerInput();//Reprompts the user
            }
            return input;
        }
        public static void FinishingPrint()
        {
            String[] finishingBye = [".-. .-')                 ('-.   ", 
                                     "\\  ( OO )              _(  OO)  ", 
                                     " ;-----.\\  ,--.   ,--.(,------. ", 
                                     " | .-.  |   \\  `.'  /  |  .---' ", 
                                     " | '-' /_).-')     /   |  |     ", 
                                     " | .-. `.(OO  \\   /   (|  '--.  ", 
                                     " | |  \\  ||   /  /\\_   |  .--'  ", 
                                     " | '--'  /`-./  /.__)  |  `---. ", 
                                     " `------'   `--'       `------' "];
            String finishingSentence = "Thanks for visiting" + INTRO_SENTENCE[2..];
            Console.Clear();
            int maxLength = finishingBye[^2].Length * 2 + "    ".Length;
            PrintPrompt(new string(' ', (maxLength - finishingSentence.Length) / 2) + finishingSentence + Environment.NewLine + Environment.NewLine);//gets rid of "to" in the intro sentence to reuse it
            Thread.Sleep(1000);
            int howManyByes = 2;
            Console.ForegroundColor = Program.TABLE_COLOR;
            foreach (String line in finishingBye)
            {
                Console.WriteLine(string.Concat(System.Linq.Enumerable.Repeat(line + "    ", howManyByes)));
                Thread.Sleep(100);
            }
            Thread.Sleep(400);
        }

        }
    }
    


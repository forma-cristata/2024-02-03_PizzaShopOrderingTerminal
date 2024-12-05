using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables;

namespace PizzaHAL
{
    public class Soup : Item
    {
        private const float PRICE = 4.49f;
        private const float S_O_D_PRICE = 3.49f;

        private static readonly List<String> AllSoups = ["Blood Basil", "Trees and Cheese", "Chicken Entrails", "Italian Funeral", "Damned Chowder"];
        public Soup(String name, float price) : base(name, price) { }
        public static List<String> GetSoups()//Only called once, at the beginning of the program
        {//Returns the three soups prepped that day, ensuring no duplicates
            Random rand = new Random();
            List<String> soupsPossible = AllSoups;
            List<String> soupsOfTheDay = new();
            for (int i = 0; i < 3; i++)
            {
                int filler = rand.Next(0, soupsPossible.Count);
                soupsOfTheDay.Add(soupsPossible[filler]);
                soupsPossible.RemoveAt(filler);//This is how I ensure no duplicates
            }
            return soupsOfTheDay;
        }
        public static Item OrderASoup(String[] soups)
        {
            (String soup, float price) = PrintSoups(soups);
            Item item = new(soup, price);
            return item;
        }
        private static (String, float) PrintSoups(String[] soups)
        {
            Console.ForegroundColor = Program.TABLE_COLOR;
            (String soup, float price)[] soupsPricing = [(soups[0], PRICE), (soups[1], PRICE), (soups[2], S_O_D_PRICE)];//Soup of the day is a dollar less than the others

            ConsoleTable table = new("  ", soupsPricing[0].soup, $"${soupsPricing[0].price}");
            table.AddRow("  ", soupsPricing[1].soup, $"${soupsPricing[1].price:#.##}");
            table.AddRow("Soup of The Day", soupsPricing[2].soup, $"${soupsPricing[2].price:#.##}");//Letting user know the soup of the day
            table.Options.EnableCount = false;
            table.Write();
            Utils.PrintPrompt($"The wetter the better. What will it be?{Environment.NewLine}{Environment.NewLine}>>");//Gross
            String input = Utils.ChangeConsoleColorAndTrimAndLowerInput();
            input = Utils.InputValidation(soups, input);
            return soupsPricing[Array.FindIndex(soups, x => x.Equals(input, StringComparison.OrdinalIgnoreCase))];
        }
    }
}

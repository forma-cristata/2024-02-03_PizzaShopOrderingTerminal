using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables;

namespace PizzaHAL
{
    public class Drink : Item
    {
        private string name { get; init; }
        private string size { get; init; }
        private string ice { get; init; }
        private float price { get; init; }
        
        private record DrinkPricing(string Size, float Price);
        private static readonly DrinkPricing[] DrinkSize = [new("Miniscule", 1.19f), new("Average", 1.59f), new("Looming", 1.99f)];
        private static readonly string[] DrinkNames = ["Dr. Death", "Dr. Thunder", "Pucker Up", "Mountain Lightning", "Mr. Fibb", "7 Dead"];
        private static readonly string[] IceChoices = ["Lukewarm", "Chilly", "Freezing"];
        private Drink(string name, string size,
                 string ice, float price) : base("Drink", price)
        {
            this.name = name;
            this.size = size;
            this.ice = ice;
            this.price = price;
        }
        public static Item OrderADrink()
        {
            (string size, float price) = PrintDrinkSizes();
            Utils.Flicker();
            string name = PrintDrinks();
            Utils.Flicker();
            string ice = PrintIce();
            Drink drink = new(name, size, ice, price);
            Item item = new(drink.ToString(), price);
            return item;
        }
        private static DrinkPricing PrintDrinkSizes()
        {
            Console.ForegroundColor = Program.TABLE_COLOR;
            ConsoleTable table = new(DrinkSize[0].Size, $"${DrinkSize[0].Price}");
            for (int i = 1; i < DrinkSize.Length; i++) { table.AddRow(DrinkSize[i].Size, $"${DrinkSize[i].Price}"); }
            table.Options.EnableCount = false;
            table.Write();//All above is printing the table
            Utils.PrintPrompt($"What size?{Environment.NewLine}{Environment.NewLine}>>");
            string input = Utils.ChangeConsoleColorAndTrimAndLowerInput();
            string[] drinkSizes = DrinkSize.Select(x => x.Size).ToArray();
            input = Utils.InputValidation(drinkSizes, input);//User chooses their size
            return DrinkSize[Array.FindIndex(drinkSizes, x => x.Equals(input, StringComparison.OrdinalIgnoreCase))];//Returns their valid drink size and price to the user
        }
        private static string PrintDrinks()//Gets the user's drink of choice
        {
            Console.ForegroundColor = Program.TABLE_COLOR;
            ConsoleTable table = new(DrinkNames[0], DrinkNames[1], DrinkNames[2]);
            table.AddRow(DrinkNames[3], DrinkNames[4], DrinkNames[5]);
            table.Options.EnableCount = false;
            table.Write();//All above is printing the table
            Utils.PrintPrompt($"Pick your poison.{Environment.NewLine}{Environment.NewLine}>>");
            string input = Utils.ChangeConsoleColorAndTrimAndLowerInput();
            input = Utils.InputValidation(DrinkNames, input);
            return DrinkNames[Array.FindIndex(DrinkNames, x => x.Equals(input, StringComparison.OrdinalIgnoreCase))];//Just for formatting, gets the drink in the correct capitalization
        }
        private static string PrintIce()//Gets the user's ice choicde
        {
            Console.ForegroundColor = Program.TABLE_COLOR;
            ConsoleTable table = new(IceChoices[0]);
            for (int i = 1; i < IceChoices.Length; i++) { table.AddRow(IceChoices[i]); }
            table.Options.EnableCount = false;
            table.Write();
            Utils.PrintPrompt($"How cold would like your revenge to be served?{Environment.NewLine}{Environment.NewLine}>>");
            string input = Utils.ChangeConsoleColorAndTrimAndLowerInput();
            input = Utils.InputValidation(IceChoices, input);
            return IceChoices[Array.FindIndex(IceChoices, x => x.Equals(input, StringComparison.OrdinalIgnoreCase))];
        }
        public override string ToString()
        {
            return $"{size}, {ice} {name}";
        }
    }
}

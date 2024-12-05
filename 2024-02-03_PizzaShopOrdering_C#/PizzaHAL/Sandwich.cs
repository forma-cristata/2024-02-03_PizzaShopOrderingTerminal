using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables;

namespace PizzaHAL
{
    public class Sandwich : Item
    {
        public const float PRICE = 5.49f;//all sandwiches are the same price
        private static readonly String[] Sandwiches = ["Buffalo Chicken", "Mediterranean Veggie", "Killy Cheese Steak", "Chicken Harm"];
        
        private Sandwich(String name, float price) : base(name, price) { }
        public static Item OrderASandwich()
        {
            return new Sandwich(PrintSandwiches(), PRICE);
        }
        private static String PrintSandwiches()//returns the sandwiche's name
        {
            Console.ForegroundColor = Program.TABLE_COLOR;
            ConsoleTable table = new(Sandwiches[0], $"${PRICE}");
            for (int i = 1; i < Sandwiches.Length; i++) { table.AddRow(Sandwiches[i], $"${PRICE}"); }
            table.Options.EnableCount = false;
            table.Write();//All above is just printing the table
            Utils.PrintPrompt($"Pick your poison.{Environment.NewLine}{Environment.NewLine}>>");
            String input = Utils.ChangeConsoleColorAndTrimAndLowerInput();
            input = Utils.InputValidation(Sandwiches, input);
            return Sandwiches[Array.FindIndex(Sandwiches, x => x.Equals(input, StringComparison.OrdinalIgnoreCase))];
        }
    }
}

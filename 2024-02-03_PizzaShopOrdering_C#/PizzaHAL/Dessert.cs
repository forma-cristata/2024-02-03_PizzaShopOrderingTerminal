using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables;

namespace PizzaHAL
{
    public class Dessert : Item
    {//This whole class is almost the exact same as sandwich except the prices change per dessert.
        private record DessertPricing(String Dessert, float Price);
        private static readonly DessertPricing[] Desserts = [new("Cookie Brownie", 6.49f), new("Chocolate Chip Cookie", 2.49f), new("Tiramisu", 4.99f)];
        public Dessert(String name, float price) : base(name, price) { }
        public static Item OrderADessert()
        {
            (String dessertName, float dessertPrice) = PrintDesserts();
            Item item = new(dessertName, dessertPrice);
            return item;
        }
        private static DessertPricing PrintDesserts()
        {
            Console.ForegroundColor = Program.TABLE_COLOR;
            ConsoleTable table = new(Desserts[0].Dessert, $"${Desserts[0].Price}");
            for (int i = 1; i < Desserts.Length; i++) { table.AddRow(Desserts[i].Dessert, $"${Desserts[i].Price}"); }
            table.Options.EnableCount = false;
            table.Write();
            Utils.PrintPrompt($"Pick this evening's indulgence.{Environment.NewLine}{Environment.NewLine}>>");
            String input = Utils.ChangeConsoleColorAndTrimAndLowerInput();
            String[] dessertNames = Desserts.Select(x => x.Dessert).ToArray();
            input = Utils.InputValidation(dessertNames, input);
            return Desserts[Array.FindIndex(dessertNames, x => x.Equals(input, StringComparison.OrdinalIgnoreCase))];

        }
    }
}

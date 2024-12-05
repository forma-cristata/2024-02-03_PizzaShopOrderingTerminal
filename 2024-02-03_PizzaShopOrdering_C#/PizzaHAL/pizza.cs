using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using ConsoleTables;

namespace PizzaHAL
{
    public class Pizza : Item
    {
        private string size { get; init; }
        private string type { get; init; }
        public List<string> toppings { get; private set; }
        public float price { get; init; }
        private record PizzaPricing(String Size, float Price, float ToppingsPrice);
        private static readonly PizzaPricing[] PizzaSize =
              [new("Small", 5.99f, 0.5f),
               new("Medium", 7.99f, 0.7f),
               new("Large", 9.99f, 0.9f),
               new("X-Large", 11.99f, 1.00f)];
        private record PizzaVeggieDesignation(string topping, bool isVegan, bool choseThisTopping);//This program does not ask the user if they want veggie or regular pizza
        private static readonly PizzaVeggieDesignation[] PizzaToppings = [new("Bacon", false, false),//This designation is automatic, determined by their topping choice
                                                                          new("Mushrooms", true, false),//If they choose a meat, they no longer have a veggie pizza
                                                                          new("Pepperoni", false, false),
                                                                          new("Ghost-Peppers", true, false),
                                                                          new("Pineapple", true, false),
                                                                          new("Sausage", false, false)];
        private Pizza(string size,//Constructor
                 string type,
                 List<string> toppings,
                 float price) : base("Pizza", price)
        { 
            this.size = size;
            this.type = type;
            this.toppings = toppings;
            this.price = price;
        }
        public static Item OrderAPizza()
        {
            string size = PrintPizzaSize();//returns the pizza size from user - validated
            Utils.Flicker();
            (List<string> toppings, string type) = PrintPizzaToppings();//Type is dependent on toppings according to PizzaVeggieDesignation record
            float price = SetTotalPrice(size, toppings);

            Pizza pizza = new(size, type, toppings, price);//Unfortunately need both of these lines for the override to string to function properly.
            Item item = new(pizza.ToString(), price);
            return item;
        }
        private static string PrintPizzaSize()
        {
            Console.ForegroundColor = Program.TABLE_COLOR;
            ConsoleTable table = new(PizzaSize[0].Size, $"${PizzaSize[0].Price}", $"${PizzaSize[0].ToppingsPrice} per Topping");
            for (int i = 1; i < PizzaSize.Length; i++)
            {
                table.AddRow(PizzaSize[i].Size, $"${PizzaSize[i].Price}", $"${PizzaSize[i].ToppingsPrice} per Topping");
            }
            table.Options.EnableCount = false;
            table.Write();//All above is printing options to the user
            Utils.PrintPrompt($"What size pizza would you like?{Environment.NewLine}{Environment.NewLine}>>");
            String input = Utils.ChangeConsoleColorAndTrimAndLowerInput();
            String[] validPizzaSizes = Pizza.GetValidSizes();
            input = Utils.InputValidation(validPizzaSizes, input);
            return validPizzaSizes[Array.FindIndex(validPizzaSizes, x => x.Equals(input, StringComparison.OrdinalIgnoreCase))];
        }
        private static String[] GetValidSizes()//Protects the record array and returns only the size part of the tuple
        {
            String[] validSizes = new String[PizzaSize.Length];
            for(int i = 0; i < validSizes.Length; i++)
            {
                validSizes[i] = PizzaSize[i].Size;
            }
            return validSizes;
            
        }
        private static (List<string> tops, string type) PrintPizzaToppings()
        {
            Console.ForegroundColor = Program.TABLE_COLOR;
            ConsoleTable table = new("FLESH:", "FLESH FREE:");
            table.AddRow(PizzaToppings[0].topping, PizzaToppings[1].topping);
            table.AddRow(PizzaToppings[2].topping, PizzaToppings[3].topping);
            table.AddRow(PizzaToppings[5].topping, PizzaToppings[4].topping);
            table.Options.EnableCount = false;
            table.Write();//All above is printing the table of options to the user
            Utils.PrintPrompt($"What morsels shall I lay to rest atop your pizza?" +
                              $"{Environment.NewLine}Separate them by spaces if you desire multiple." +
                              $"{Environment.NewLine}Enter nothing for boring pizza." +
                              $"{Environment.NewLine}{Environment.NewLine}>>");
            String input = Utils.ChangeConsoleColorAndTrimAndLowerInput();
            bool isveg = true;//Pizza always begins as veggie until meat toppings are added
            List<String> toppers = [];//The List of toppings they have chosen
            if (!String.IsNullOrWhiteSpace(input))//If they did not input nothing, then they chose toppings
            {
                toppers = [.. input.Split(' ')];
                String[] validToppings = PizzaToppings.Select(x => x.topping).ToArray();
                for (int i = 0; i < toppers.Count; i++)//This type of input required its own input validation.  Only used once, so I did not modularize it.
                {//For each topping the user put in...
                    if (!validToppings.Contains(toppers[i], StringComparer.OrdinalIgnoreCase) && toppers.Count != 0)
                    {//If it is not a valid topping and if they put any toppings...
                        Utils.PrintPrompt($"Your response, \"{String.Join(' ', toppers)}\", is gibberish... Try again{Environment.NewLine}{Environment.NewLine}>>");
                        toppers.Clear();//We get rid of their prior response that was invalid
                        input = Utils.ChangeConsoleColorAndTrimAndLowerInput();//We get their chosen toppings again
                        if (String.IsNullOrWhiteSpace(input))
                        {
                            toppers = null;
                            goto NeedAGoTo;//I could write a bunch of ifs or I could just skip a few lines of code....
                        }
                        toppers = [..input.Split(' ')];//As a list
                        i = -1;//And we restart the forloop to check every topping they may have put into their list
                    }
                }
                
                foreach (String topper in toppers)
                {
                    if (!PizzaToppings[Array.FindIndex(validToppings, x => x.Equals(topper, StringComparison.OrdinalIgnoreCase))].isVegan)//If any of the toppings is NOT vegan
                    {
                        isveg = false;//The pizza type is no longer 'veggie'
                        break;
                    }
                }
            }
            else
            {
                toppers = null;
            }
            NeedAGoTo:

            string type = isveg ? "Veggie" : "Regular";
            return (toppers, type);//Return their chosen toppings and subsequent pizza type
        }
        
        private static float SetTotalPrice(string size, List<string> toppings)
        {
            float finalPrice = 0;
            String[] sizes = PizzaSize.Select(x => x.Size).ToArray();//Get string array of sizes from tuple arrray
            if (toppings == null)
            {
                finalPrice = PizzaSize[Array.FindIndex(sizes, t => t.Equals(size, StringComparison.OrdinalIgnoreCase))].Price;
            }
            else {
                finalPrice = PizzaSize[Array.FindIndex(sizes, t => t.Equals(size, StringComparison.OrdinalIgnoreCase))].Price //Price calculations
                                   + (PizzaSize[Array.FindIndex(sizes, t => t.Equals(size, StringComparison.OrdinalIgnoreCase))].ToppingsPrice * toppings.Count); }
            return finalPrice;
        }
        public override string ToString()
        {//Automatically changes the characteristics of a pizza to be compatable with an item (name, price)
            String newName;
            if(toppings != null)
            {
                newName = $"{size.ToLower()}, {type.ToLower()} pizza with ";
                foreach (String topping in toppings)
                {
                    if (topping == toppings[^1])
                    {
                        newName += topping;
                    }
                    else if (topping == toppings[^2])
                    {
                        newName += $"{topping} and ";
                    }
                    else
                    {
                        newName += $"{topping}, ";
                    }
                }
            }
            else
            {
                newName = $"{size[..1].ToUpper()}{size[1..]}, {type.ToLower()} pizza";
            }
            
            return newName ;
        }
    }

    
}

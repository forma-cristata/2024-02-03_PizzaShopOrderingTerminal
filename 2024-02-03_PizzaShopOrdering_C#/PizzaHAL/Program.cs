/*
 * Name: Kaci Craycraft
 * South Hills Username: kcraycraft45
 */

using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using ConsoleTables;

namespace PizzaHAL
{
    public class Program
    {
        public const ConsoleColor TABLE_COLOR = ConsoleColor.DarkRed;
        private const ConsoleColor DONT_SHOW_COLOR = ConsoleColor.Black;
        private const float DELIVERY_PRICE = 8f;
        private const float TAX_RATE = 0.06f;
        public const String REVIEWS = "Reviews.txt";
        private static List<Item> Order = [];
        public static void Main()
        {
            Console.Title = "Devilish Deli";
            String[] soups = [.. Soup.GetSoups()];//Only get the 3 soups of the day once: when the program begins
            for (int i = 0; i < int.MaxValue; i++)//User can add as many items to their cart as they could possibly want.
            {
                Utils.Flicker();//Just design for spookies
                if (i == 0)
                {//Only print the title and introduction on the first iteration
                    Utils.BeginningPrint();
                }
                Menu.PrintCategories();

                Utils.PrintPrompt($"Which delight would you prefer to consume?{Environment.NewLine}" +
                                  $"Enter nothing if satisfied." +
                                  $"{Environment.NewLine}{Environment.NewLine}>>");

                String input = Utils.ChangeConsoleColorAndTrimAndLowerInput();//Takes all inputs in a specific color, and stores them as ToLower()Trim()
                if (String.IsNullOrWhiteSpace(input))//If they input nothing, then they did not want to add anything to their order
                {
                    Utils.Flicker();
                    break;
                }
                else//They wanted to add something to their order
                {
                    String[] validInputs = Menu.GetCategoryArray();
                    input = Utils.InputValidationNullsOkay(validInputs, input);
                    if (String.IsNullOrWhiteSpace(input))//If they input nothing, then they did not want to add anything to their order
                    {
                        Utils.Flicker();
                        break;
                    }
                    Utils.Flicker();
                    switch (input)
                    {
                        case "pizza":
                            Item pizza = Pizza.OrderAPizza();
                            Order.Add(pizza);
                            break;

                        case "sandwich": 
                            Item sandwich = Sandwich.OrderASandwich();
                            Order.Add(sandwich);
                            break;

                        case "soup":
                            Item soup = Soup.OrderASoup(soups);
                            Order.Add(soup);
                            break;

                        case "dessert":
                            Item dessert = Dessert.OrderADessert();
                            Order.Add(dessert);
                            break;

                        case "drink":
                            Item drink = Drink.OrderADrink();
                            Order.Add(drink);
                            break;
                    }
                    Utils.Flicker();
                    String theItem = AorAn(Order[i].Name) + Order[i].Name;//Formatting grammar
                    Utils.PrintPrompt($"You ordered {theItem} for ${Order[i].Price:#.##}.{Environment.NewLine}" +
                                      $"Press any key to continue.");//Tells the user the item they ordered every time
                    Console.ReadKey();
                    Utils.Flicker();
                }
            }
            if (Order.Count == 0)//If they did not order anything
            {
                String[] threat = ["Order something next time.", "Or else..."];
                foreach (String line in threat)
                {
                    Utils.PrintPrompt(new string(' ', (Console.WindowWidth - line.Length) / 2) + line);
                    Console.Write(Environment.NewLine);
                    Thread.Sleep(1000);
                }
                Thread.Sleep(1000);
            }
            else//If they did order things
            {
                Utils.Flicker();
                FinishingTouch();
            }


            Utils.FinishingPrint();
            Thread.Sleep(1000);
            Utils.Flicker();




            Console.ForegroundColor = DONT_SHOW_COLOR;
        }
        private static String AorAn(String itemName)
        {
            char[] vowels = ['a', 'e', 'i', 'o', 'u', 'x'];
            if (vowels.Contains(itemName.ToLower()[0]))
            {
                return "an ";
            }
            else { return "a "; }
        }//Ensures the grammar for telling the user what they ordered.
        private static void FinishingTouch()
        {
            Console.ForegroundColor = TABLE_COLOR;
            ConsoleTable receipt = new("Item", "Price");//Print out their receipt
            float subtotal = 0f;
            foreach (Item item in Order)
            {
                receipt.AddRow(item.Name, $"${item.Price:#.##}");
                subtotal += item.Price;
            }
            receipt.AddRow("TOTAL", $"${subtotal:#.##}");
            receipt.Options.EnableCount = false;
            receipt.Write();
            Utils.PrintPrompt($"Must we deliver your spoils? (yes/no){Environment.NewLine}{Environment.NewLine}>>");
            String input = Utils.ChangeConsoleColorAndTrimAndLowerInput();
            bool delivery = (Utils.InputValidation(["yes", "no"], input) == "yes");//must answer yes or no
            Utils.Flicker();
            Console.ForegroundColor = TABLE_COLOR;
            ConsoleTable table = new("Subtotal", $"${subtotal:#.##}");//Print out their total w/ or w/o delivery price
            table.AddRow("Tax", $"${(subtotal * TAX_RATE):#.##}");
            if (delivery)
            {
                table.AddRow("Delivery Fee", $"${DELIVERY_PRICE}");
                table.AddRow("TOTAL", $"${(DELIVERY_PRICE + subtotal + subtotal * TAX_RATE):#.##}");
            }
            else
            {
                table.AddRow("TOTAL", $"${(subtotal + subtotal * TAX_RATE):#.##}");
            }
            table.Options.EnableCount = false;
            table.Write();
            ReviewSystem();
            
        }
        private static void ReviewSystem()//Asks the user if they want to leave a review and saves it to a file
        {//When the program begins, it prints out several of the reviews below the welcoming message (3 max)
            if (!File.Exists(REVIEWS))
            {
                File.Create(REVIEWS).Close();
            }
            Utils.PrintPrompt($"Leave us a review?{Environment.NewLine}>>");
            String input = Utils.ChangeConsoleColorAndTrimAndLowerInput();
            input = Utils.InputValidation(["yes", "no"], input);
            if (input == "yes")
            {
                Console.Clear();
                Utils.PrintPrompt($"How did we do?{Environment.NewLine}>>");
                input = Utils.ChangeConsoleColorAndTrimAndLowerInput();
                File.AppendAllText(REVIEWS, $"{input}{Environment.NewLine}");
            }
        }
        
        






    }
}

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables;

namespace PizzaHAL
{
    public class Menu
    {
        private record CategoriesAndDescriptions(String category, String description);
        private readonly static ImmutableArray<CategoriesAndDescriptions> Categories = [new("Pizza", "The sauce is definitely marinara..."),
                                                                               new("Sandwich", "The crunch will tear apart your insides."),
                                                                               new("Soup", "Warm up your cold, dead heart."),
                                                                               new("Dessert", "Indulge in the sweetness of the unknown."),
                                                                               new("Drink", "Don't choke.")];
        public static void PrintCategories()
        {
            Console.ForegroundColor = Program.TABLE_COLOR;
            ConsoleTable table = new(Categories[0].category, Categories[0].description);
            for (int i = 1; i < Categories.Count(); i++)
            {
                table.AddRow(Categories[i].category, Categories[i].description);
            }
            table.Options.EnableCount = false;
            table.Write();//Prints the categories in a table to the user
        }
        public static String[] GetCategoryArray()//Gets the category arry from the tuple array to use while protecting original values
        {
            String[] cats = new String[Categories.Count()];
            for(int i = 0; i < Categories.Count(); i++)
            {
                cats[i] = Categories[i].category;
            }
            return cats;
        }
        
        
        
        
        
    }
}

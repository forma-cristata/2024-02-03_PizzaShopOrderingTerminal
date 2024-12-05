using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaHAL
{
    public class Item
    {
      
        public String Name { get; protected set; }
        public float Price { get; protected set; }

        public Item(String name, float price)
        {
            this.Name = name;
            this.Price = price;
        }

    }
}

using horse_racing.userControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace horse_racing.cs
{
    public class Item
    {
        public string? BuffType { get; set; }
        public int Position { get; set; }

        List<string> buffName = new()
        {
            "sprint",
        };
        List<string> DebuffName = new()
        {
            "reverse", 
            //"sturn", 
            "teleport"
        };

        public Item Itemm()
        {
            Random random = new Random();
            int randomType = random.Next(0, 5);

            if(randomType == 0)
            {
                BuffType = buffName[0];
            }else
            {
                BuffType = DebuffName[random.Next(0, 2)];
            }

            Item item = new()
            {
                BuffType = BuffType,
            };

            return item;
        }
    }
}

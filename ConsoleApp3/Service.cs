using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    internal class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public TimeSpan Duration { get; set; }

        public override string ToString() => $"{Name} - {Price:C} ({Duration.TotalMinutes} мин)";

        public string ToFileString()
        {
            return $"{Id}|{Name}|{Price}|{Duration}";
        }

        public static Service FromFileString(string line)
        {
            var parts = line.Split('|');
            return new Service
            {
                Id = int.Parse(parts[0]),
                Name = parts[1],
                Price = decimal.Parse(parts[2]),
                Duration = TimeSpan.Parse(parts[3])
            };
        }
    }
}

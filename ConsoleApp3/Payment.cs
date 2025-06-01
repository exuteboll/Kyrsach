using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    internal class Payment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public override string ToString() => $"{Date:dd.MM.yyyy} - {Amount:C} (Пациент ID:{PatientId})";

        public string ToFileString()
        {
            return $"{Id}|{PatientId}|{Amount}|{Date:yyyy-MM-dd}";
        }

        public static Payment FromFileString(string line)
        {
            var parts = line.Split('|');
            return new Payment
            {
                Id = int.Parse(parts[0]),
                PatientId = int.Parse(parts[1]),
                Amount = decimal.Parse(parts[2]),
                Date = DateTime.Parse(parts[3])
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    internal class Patient
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string ContactInfo { get; set; }
        public List<int> VisitHistoryIds { get; set; } = new List<int>();
        public List<int> PaymentIds { get; set; } = new List<int>();

        public override string ToString() => $"{FullName} (р. {BirthDate:dd.MM.yyyy})";

        public string ToFileString()
        {
            var visits = string.Join(",", VisitHistoryIds);
            var payments = string.Join(",", PaymentIds);
            return $"{Id}|{FullName}|{BirthDate:yyyy-MM-dd}|{ContactInfo}|{visits}|{payments}";
        }

        public static Patient FromFileString(string line)
        {
            var parts = line.Split('|');
            var patient = new Patient
            {
                Id = int.Parse(parts[0]),
                FullName = parts[1],
                BirthDate = DateTime.Parse(parts[2]),
                ContactInfo = parts[3]
            };

            if (parts.Length > 4 && !string.IsNullOrEmpty(parts[4]))
                patient.VisitHistoryIds = parts[4].Split(',').Select(int.Parse).ToList();

            if (parts.Length > 5 && !string.IsNullOrEmpty(parts[5]))
                patient.PaymentIds = parts[5].Split(',').Select(int.Parse).ToList();

            return patient;
        }
    }
}

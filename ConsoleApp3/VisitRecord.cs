using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    internal class VisitRecord
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int ServiceId { get; set; }
        public bool IsCompleted { get; set; }

        public override string ToString() =>
            $"{Date:dd.MM.yyyy} {Time:hh\\:mm} - Пациент ID:{PatientId} к врачу ID:{DoctorId} (Услуга ID:{ServiceId})";

        public string ToFileString()
        {
            return $"{Id}|{PatientId}|{DoctorId}|{Date:yyyy-MM-dd}|{Time}|{ServiceId}|{IsCompleted}";
        }

        public static VisitRecord FromFileString(string line)
        {
            var parts = line.Split('|');
            return new VisitRecord
            {
                Id = int.Parse(parts[0]),
                PatientId = int.Parse(parts[1]),
                DoctorId = int.Parse(parts[2]),
                Date = DateTime.Parse(parts[3]),
                Time = TimeSpan.Parse(parts[4]),
                ServiceId = int.Parse(parts[5]),
                IsCompleted = bool.Parse(parts[6])
            };
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    internal class Doctor
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Specialization { get; set; }
        public Dictionary<DayOfWeek, (TimeSpan Start, TimeSpan End)> WorkSchedule { get; set; }

        public Doctor()
        {
            WorkSchedule = new Dictionary<DayOfWeek, (TimeSpan Start, TimeSpan End)>();
        }

        public override string ToString() => $"{FullName} ({Specialization})";

        public string ToFileString()
        {
            var schedule = string.Join(";", WorkSchedule.Select(kv =>
                $"{kv.Key}:{kv.Value.Start}:{kv.Value.End}"));
            return $"{Id}|{FullName}|{Specialization}|{schedule}";
        }

        public static Doctor FromFileString(string line)
        {
            var parts = line.Split('|');
            var doctor = new Doctor
            {
                Id = int.Parse(parts[0]),
                FullName = parts[1],
                Specialization = parts[2]
            };

            if (parts.Length > 3)
            {
                var scheduleParts = parts[3].Split(';');
                foreach (var item in scheduleParts)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        var dayParts = item.Split(':');
                        var day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayParts[0]);
                        var start = TimeSpan.Parse(dayParts[1]);
                        var end = TimeSpan.Parse(dayParts[2]);
                        doctor.WorkSchedule[day] = (start, end);
                    }
                }
            }

            return doctor;
        }
    }
}

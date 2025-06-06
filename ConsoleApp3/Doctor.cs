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
        public Dictionary<DayOfWeek, (TimeSpan Start, TimeSpan End)> WorkSchedule { get; set; } = new Dictionary<DayOfWeek, (TimeSpan Start, TimeSpan End)>();

        public string ToFileString()
        {
            var schedule = string.Join(";", WorkSchedule.Select(kv =>
                $"{kv.Key}:{kv.Value.Start.TotalHours}:{kv.Value.End.TotalHours}"));
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

            if (parts.Length > 3 && !string.IsNullOrEmpty(parts[3]))
            {
                foreach (var item in parts[3].Split(';'))
                {
                    if (string.IsNullOrEmpty(item)) continue;

                    var dayParts = item.Split(':');
                    var day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayParts[0]);
                    var startHours = double.Parse(dayParts[1]);
                    var endHours = double.Parse(dayParts[2]);

                    doctor.WorkSchedule[day] = (
                        TimeSpan.FromHours(startHours),
                        TimeSpan.FromHours(endHours)
                    );
                }
            }

            return doctor;
        }
    }
}

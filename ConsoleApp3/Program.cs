using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    internal class Program
    {
        static DentalClinic clinic = new DentalClinic();
        private static readonly Dictionary<DayOfWeek, string> RussianDayNames = new Dictionary<DayOfWeek, string>
         {
         { DayOfWeek.Monday, "Понедельник" },
         { DayOfWeek.Tuesday, "Вторник" },
         { DayOfWeek.Wednesday, "Среда" },
         { DayOfWeek.Thursday, "Четверг" },
         { DayOfWeek.Friday, "Пятница" },
         { DayOfWeek.Saturday, "Суббота" },
         { DayOfWeek.Sunday, "Воскресенье" }
          };
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Стоматологическая клиника - система управления ===");
                Console.WriteLine("1. Управление врачами");
                Console.WriteLine("2. Управление пациентами");
                Console.WriteLine("3. Управление услугами");
                Console.WriteLine("4. Управление записями на прием");
                Console.WriteLine("5. Управление платежами");
                Console.WriteLine("6. Отчеты и аналитика");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите пункт меню: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ManageDoctors();
                        break;
                    case "2":
                        ManagePatients();
                        break;
                    case "3":
                        ManageServices();
                        break;
                    case "4":
                        ManageVisits();
                        break;
                    case "5":
                        ManagePayments();
                        break;
                    case "6":
                        ShowReports();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void ManageDoctors()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Управление врачами ===");
                Console.WriteLine("1. Добавить врача");
                Console.WriteLine("2. Удалить врача");
                Console.WriteLine("3. Просмотреть всех врачей");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите пункт меню: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddDoctor();
                        break;
                    case "2":
                        RemoveDoctor();
                        break;
                    case "3":
                        ViewAllDoctors();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddDoctor()
        {
            Console.Clear();
            Console.WriteLine("=== Добавление нового врача ===");

            var doctor = new Doctor();

            Console.Write("ФИО врача: ");
            doctor.FullName = Console.ReadLine();

            Console.Write("Специализация: ");
            doctor.Specialization = Console.ReadLine();

            Console.WriteLine("График работы (введите дни недели и время через пробел, например: Понедельник 9:00 17:00)");
            Console.WriteLine("Для завершения введите 'продолжить'");

            // Словарь для преобразования русских названий в DayOfWeek
            var dayOfWeekMap = new Dictionary<string, DayOfWeek>
            {
                ["понедельник"] = DayOfWeek.Monday,
                ["вторник"] = DayOfWeek.Tuesday,
                ["среда"] = DayOfWeek.Wednesday,
                ["четверг"] = DayOfWeek.Thursday,
                ["пятница"] = DayOfWeek.Friday,
                ["суббота"] = DayOfWeek.Saturday,
                ["воскресенье"] = DayOfWeek.Sunday
            };

            while (true)
            {
                Console.Write("День и время (или 'продолжить'): ");
                var input = Console.ReadLine();
                if (input.ToLower() == "продолжить") break;

                try
                {
                    var parts = input.Split(' ');
                    if (parts.Length < 3)
                    {
                        Console.WriteLine("Неверный формат. Используйте: День/Начало/Конец");
                        continue;
                    }

                    // Объединяем все части кроме последних двух (на случай если день состоит из нескольких слов)
                    var dayPart = string.Join(" ", parts.Take(parts.Length - 2)).ToLower();
                    var startPart = parts[parts.Length - 2];
                    var endPart = parts[parts.Length - 1];

                    if (!dayOfWeekMap.TryGetValue(dayPart, out DayOfWeek day))
                    {
                        Console.WriteLine("Неверное название дня недели. Попробуйте снова.");
                        continue;
                    }

                    var start = TimeSpan.Parse(startPart);
                    var end = TimeSpan.Parse(endPart);
                    doctor.WorkSchedule[day] = (start, end);
                }
                catch
                {
                    Console.WriteLine("Ошибка ввода. Попробуйте снова.");
                }
            }

            clinic.AddDoctor(doctor);
            Console.WriteLine("Врач успешно добавлен. Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        
        }

        static void RemoveDoctor()
        {
            Console.Clear();
            Console.WriteLine("=== Удаление врача ===");
            ViewAllDoctors();

            Console.Write("Введите ID врача для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                if (clinic.RemoveDoctor(id))
                {
                    Console.WriteLine("Врач успешно удален.");
                }
                else
                {
                    Console.WriteLine("Не удалось удалить врача (возможно, ID неверный или есть связанные записи).");
                }
            }
            else
            {
                Console.WriteLine("Неверный ID.");
            }

            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void ViewAllDoctors()
        {
            Console.Clear();
            Console.WriteLine("=== Список всех врачей ===");
            foreach (var doctor in clinic.Doctors)
            {
                Console.WriteLine($"ID: {doctor.Id}, {doctor.FullName}, {doctor.Specialization}");
                Console.WriteLine("График работы:");
                foreach (var schedule in doctor.WorkSchedule)
                {
                    // Используем словарь для перевода на русский
                    string russianDay = RussianDayNames[schedule.Key];
                    Console.WriteLine($"  {russianDay}: {schedule.Value.Start.ToString("hh\\:mm")} - {schedule.Value.End.ToString("hh\\:mm")}");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void ManagePatients()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Управление пациентами ===");
                Console.WriteLine("1. Добавить пациента");
                Console.WriteLine("2. Просмотреть всех пациентов");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите пункт меню: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddPatient();
                        break;
                    case "2":
                        ViewAllPatients();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddPatient()
        {
            Console.Clear();
            Console.WriteLine("=== Добавление нового пациента ===");

            var patient = new Patient();

            Console.Write("ФИО пациента: ");
            patient.FullName = Console.ReadLine();

            Console.Write("Дата рождения (гггг-мм-дд): ");
            if (DateTime.TryParse(Console.ReadLine(), out var birthDate))
            {
                patient.BirthDate = birthDate;
            }
            else
            {
                Console.WriteLine("Неверный формат даты. Установлена текущая дата.");
                patient.BirthDate = DateTime.Today;
            }

            Console.Write("Контактные данные: ");
            patient.ContactInfo = Console.ReadLine();

            clinic.AddPatient(patient);
            Console.WriteLine("Пациент успешно добавлен. Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void ViewAllPatients()
        {
            Console.Clear();
            Console.WriteLine("=== Список всех пациентов ===");
            foreach (var patient in clinic.Patients)
            {
                Console.WriteLine($"ID: {patient.Id}, {patient.FullName}, {patient.BirthDate:dd.MM.yyyy}, {patient.ContactInfo}");
                Console.WriteLine();
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void ManageServices()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Управление услугами ===");
                Console.WriteLine("1. Добавить услугу");
                Console.WriteLine("2. Просмотреть все услуги");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите пункт меню: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddService();
                        break;
                    case "2":
                        ViewAllServices();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddService()
        {
            Console.Clear();
            Console.WriteLine("=== Добавление новой услуги ===");

            var service = new Service();

            Console.Write("Название услуги: ");
            service.Name = Console.ReadLine();

            Console.Write("Цена: ");
            if (decimal.TryParse(Console.ReadLine(), out var price))
            {
                service.Price = price;
            }
            else
            {
                Console.WriteLine("Неверный формат цены. Установлено 0.");
                service.Price = 0;
            }

            Console.Write("Продолжительность (чч:мм): ");
            if (TimeSpan.TryParse(Console.ReadLine(), out var duration))
            {
                service.Duration = duration;
            }
            else
            {
                Console.WriteLine("Неверный формат времени. Установлено 30 минут.");
                service.Duration = TimeSpan.FromMinutes(30);
            }

            clinic.AddService(service);
            Console.WriteLine("Услуга успешно добавлена. Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void ViewAllServices()
        {
            Console.Clear();
            Console.WriteLine("=== Список всех услуг ===");
            foreach (var service in clinic.Services)
            {
                Console.WriteLine($"ID: {service.Id}, {service.Name}, {service.Price:C}, {service.Duration.TotalMinutes} мин");
                Console.WriteLine();
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void ManageVisits()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Управление записями на прием ===");
                Console.WriteLine("1. Добавить запись");
                Console.WriteLine("2. Просмотреть все записи");
                Console.WriteLine("3. Просмотреть записи на сегодня");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите пункт меню: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddVisit();
                        break;
                    case "2":
                        ViewAllVisits();
                        break;
                    case "3":
                        ViewTodaysVisits();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddVisit()
        {
            Console.Clear();
            Console.WriteLine("=== Добавление новой записи на прием ===");

            var visit = new VisitRecord();

            ViewAllPatients();
            Console.Write("ID пациента: ");
            if (int.TryParse(Console.ReadLine(), out int patientId))
            {
                visit.PatientId = patientId;
            }
            else
            {
                Console.WriteLine("Неверный ID пациента. Запись не добавлена.");
                Console.ReadKey();
                return;
            }

            ViewAllDoctors();
            Console.Write("ID врача: ");
            if (int.TryParse(Console.ReadLine(), out int doctorId))
            {
                visit.DoctorId = doctorId;
            }
            else
            {
                Console.WriteLine("Неверный ID врача. Запись не добавлена.");
                Console.ReadKey();
                return;
            }

            Console.Write("Дата приема (гггг-мм-дд): ");
            if (DateTime.TryParse(Console.ReadLine(), out var date))
            {
                visit.Date = date;
            }
            else
            {
                Console.WriteLine("Неверный формат даты. Установлена текущая дата.");
                visit.Date = DateTime.Today;
            }

            Console.Write("Время приема (чч:мм): ");
            if (TimeSpan.TryParse(Console.ReadLine(), out var time))
            {
                visit.Time = time;
            }
            else
            {
                Console.WriteLine("Неверный формат времени. Установлено 10:00.");
                visit.Time = new TimeSpan(10, 0, 0);
            }

            ViewAllServices();
            Console.Write("ID услуги: ");
            if (int.TryParse(Console.ReadLine(), out int serviceId))
            {
                visit.ServiceId = serviceId;
            }
            else
            {
                Console.WriteLine("Неверный ID услуги. Запись не добавлена.");
                Console.ReadKey();
                return;
            }

            Console.Write("Завершен ли прием (y/n): ");
            visit.IsCompleted = Console.ReadLine().ToLower() == "y";

            clinic.AddVisitRecord(visit);
            Console.WriteLine("Запись успешно добавлена. Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void ViewAllVisits()
        {
            Console.Clear();
            Console.WriteLine("=== Список всех записей ===");
            foreach (var visit in clinic.VisitRecords)
            {
                var patient = clinic.Patients.FirstOrDefault(p => p.Id == visit.PatientId)?.FullName ?? "Неизвестный";
                var doctor = clinic.Doctors.FirstOrDefault(d => d.Id == visit.DoctorId)?.FullName ?? "Неизвестный";
                var service = clinic.Services.FirstOrDefault(s => s.Id == visit.ServiceId)?.Name ?? "Неизвестная";

                Console.WriteLine($"ID: {visit.Id}, {visit.Date:dd.MM.yyyy} {visit.Time:hh\\:mm}");
                Console.WriteLine($"Пациент: {patient}, Врач: {doctor}");
                Console.WriteLine($"Услуга: {service}, Статус: {(visit.IsCompleted ? "Завершен" : "Запланирован")}");
                Console.WriteLine();
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void ViewTodaysVisits()
        {
            Console.Clear();
            Console.WriteLine("=== Записи на сегодня ===");
            foreach (var visit in clinic.GetTodaysAppointments())
            {
                var patient = clinic.Patients.FirstOrDefault(p => p.Id == visit.PatientId)?.FullName ?? "Неизвестный";
                var doctor = clinic.Doctors.FirstOrDefault(d => d.Id == visit.DoctorId)?.FullName ?? "Неизвестный";
                var service = clinic.Services.FirstOrDefault(s => s.Id == visit.ServiceId)?.Name ?? "Неизвестная";

                Console.WriteLine($"{visit.Time:hh\\:mm} - Пациент: {patient}, Врач: {doctor}");
                Console.WriteLine($"Услуга: {service}, Статус: {(visit.IsCompleted ? "Завершен" : "Запланирован")}");
                Console.WriteLine();
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void ManagePayments()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Управление платежами ===");
                Console.WriteLine("1. Добавить платеж");
                Console.WriteLine("2. Просмотреть все платежи");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите пункт меню: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddPayment();
                        break;
                    case "2":
                        ViewAllPayments();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddPayment()
        {
            Console.Clear();
            Console.WriteLine("=== Добавление нового платежа ===");

            var payment = new Payment();

            ViewAllPatients();
            Console.Write("ID пациента: ");
            if (int.TryParse(Console.ReadLine(), out int patientId))
            {
                payment.PatientId = patientId;
            }
            else
            {
                Console.WriteLine("Неверный ID пациента. Платеж не добавлен.");
                Console.ReadKey();
                return;
            }

            Console.Write("Сумма платежа: ");
            if (decimal.TryParse(Console.ReadLine(), out var amount))
            {
                payment.Amount = amount;
            }
            else
            {
                Console.WriteLine("Неверный формат суммы. Установлено 0.");
                payment.Amount = 0;
            }

            Console.Write("Дата платежа (гггг-мм-дд): ");
            if (DateTime.TryParse(Console.ReadLine(), out var date))
            {
                payment.Date = date;
            }
            else
            {
                Console.WriteLine("Неверный формат даты. Установлена текущая дата.");
                payment.Date = DateTime.Today;
            }

            clinic.AddPayment(payment);
            Console.WriteLine("Платеж успешно добавлен. Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void ViewAllPayments()
        {
            Console.Clear();
            Console.WriteLine("=== Список всех платежей ===");
            foreach (var payment in clinic.Payments)
            {
                var patient = clinic.Patients.FirstOrDefault(p => p.Id == payment.PatientId)?.FullName ?? "Неизвестный";
                Console.WriteLine($"ID: {payment.Id}, {payment.Date:dd.MM.yyyy} - {payment.Amount:C}");
                Console.WriteLine($"Пациент: {patient}");
                Console.WriteLine();
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void ShowReports()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Отчеты и аналитика ===");
                Console.WriteLine("1. Врачи по дням недели");
                Console.WriteLine("2. Самые востребованные услуги");
                Console.WriteLine("3. Пациенты с задолженностью");
                Console.WriteLine("4. Доход за месяц");
                Console.WriteLine("5. Средний чек");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите пункт меню: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ShowDoctorsByDay();
                        break;
                    case "2":
                        ShowPopularServices();
                        break;
                    case "3":
                        ShowPatientsWithDebt();
                        break;
                    case "4":
                        ShowMonthlyIncome();
                        break;
                    case "5":
                        ShowAveragePayment();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void ShowDoctorsByDay()
        {
            Console.Clear();
            Console.WriteLine("=== Врачи по дням недели ===");

            // Словарь для преобразования русских названий в DayOfWeek
            var russianDayMap = new Dictionary<string, DayOfWeek>
            {
                ["понедельник"] = DayOfWeek.Monday,
                ["вторник"] = DayOfWeek.Tuesday,
                ["среда"] = DayOfWeek.Wednesday,
                ["четверг"] = DayOfWeek.Thursday,
                ["пятница"] = DayOfWeek.Friday,
                ["суббота"] = DayOfWeek.Saturday,
                ["воскресенье"] = DayOfWeek.Sunday
            };

            Console.Write("Введите день недели (например, Понедельник или Monday): ");
            var input = Console.ReadLine().ToLower();

            // Пробуем сначала русские названия
            if (russianDayMap.TryGetValue(input, out DayOfWeek day))
            {
                Console.WriteLine($"\nВрачи, работающие в {RussianDayNames[day]}:");
                foreach (var doctor in clinic.GetDoctorsWorkingOnDay(day))
                {
                    var schedule = doctor.WorkSchedule[day];
                    Console.WriteLine($"{doctor.FullName} ({doctor.Specialization}): {schedule.Start:hh\\:mm} - {schedule.End:hh\\:mm}");
                }
            }
            // Пробуем английские названия
            else if (Enum.TryParse<DayOfWeek>(input, true, out day))
            {
                Console.WriteLine($"\nВрачи, работающие в {RussianDayNames[day]}:");
                foreach (var doctor in clinic.GetDoctorsWorkingOnDay(day))
                {
                    var schedule = doctor.WorkSchedule[day];
                    Console.WriteLine($"{doctor.FullName} ({doctor.Specialization}): {schedule.Start:hh\\:mm} - {schedule.End:hh\\:mm}");
                }
            }
            else
            {
                Console.WriteLine("Неверное название дня недели. Используйте русские или английские названия.");
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void ShowPopularServices()
        {
            Console.Clear();
            Console.WriteLine("=== Самые востребованные услуги ===");
            var popularServices = clinic.GetMostPopularServices();

            Console.WriteLine("Топ-5 самых востребованных услуг:");
            foreach (var (service, count) in popularServices)
            {
                Console.WriteLine($"{service.Name}: {count} записей");
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void ShowPatientsWithDebt()
        {
            Console.Clear();
            Console.WriteLine("=== Пациенты с задолженностью ===");
            var patientsWithDebt = clinic.GetPatientsWithDebt();

            if (patientsWithDebt.Any())
            {
                foreach (var (patient, debt) in patientsWithDebt)
                {
                    Console.WriteLine($"{patient.FullName}: задолженность {debt:C}");
                }
            }
            else
            {
                Console.WriteLine("Нет пациентов с задолженностью.");
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void ShowMonthlyIncome()
        {
            Console.Clear();
            Console.WriteLine("=== Доход за месяц ===");
            Console.Write("Введите год: ");
            if (int.TryParse(Console.ReadLine(), out int year))
            {
                Console.Write("Введите месяц (1-12): ");
                if (int.TryParse(Console.ReadLine(), out int month) && month >= 1 && month <= 12)
                {
                    var income = clinic.GetMonthlyIncome(year, month);
                    Console.WriteLine($"\nДоход за {month}/{year}: {income:C}");
                }
                else
                {
                    Console.WriteLine("Неверный месяц.");
                }
            }
            else
            {
                Console.WriteLine("Неверный год.");
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void ShowAveragePayment()
        {
            Console.Clear();
            Console.WriteLine("=== Средний чек пациента ===");
            var average = clinic.GetAveragePayment();
            Console.WriteLine($"Средний чек: {average:C}");

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}
    


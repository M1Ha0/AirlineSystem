using System.Security.Cryptography;
using System.Text;

namespace AirlineApi.Models
{
    public static class SeedData
    {
        public static void SeedDatabase(AirlineContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { Email = "admin@avia.ru", Password = AuthOptions.GetHash("1234"), FullName = "Администратор Системы", Role = "Администратор" },
                    new User { Email = "dispatcher@avia.ru", Password = AuthOptions.GetHash("1234"), FullName = "Сменный Диспетчер", Role = "Диспетчер" }
                );
                context.SaveChanges();
            }

            if (!context.CrewMembers.Any())
            {
                context.CrewMembers.AddRange(
                    new CrewMember { FullName = "Иванов Иван Иванович", Role = "1-й пилот", EmployeeNumber = "1001", MedicalClearance = true },
                    new CrewMember { FullName = "Соколов Дмитрий Андреевич", Role = "1-й пилот", EmployeeNumber = "1002", MedicalClearance = true },
                    new CrewMember { FullName = "Петров Петр Сергеевич", Role = "2-й пилот", EmployeeNumber = "2001", MedicalClearance = true },
                    new CrewMember { FullName = "Морозов Артем Олегович", Role = "2-й пилот", EmployeeNumber = "2002", MedicalClearance = true },
                    new CrewMember { FullName = "Сидоров Алексей Михайлович", Role = "Штурман", EmployeeNumber = "3001", MedicalClearance = true },
                    new CrewMember { FullName = "Волков Николай Игоревич", Role = "Штурман", EmployeeNumber = "3002", MedicalClearance = false },
                    new CrewMember { FullName = "Кузнецов Виктор Павлович", Role = "Радист", EmployeeNumber = "4001", MedicalClearance = true },
                    new CrewMember { FullName = "Смирнова Анна Дмитриевна", Role = "Стюардесса", EmployeeNumber = "5001", MedicalClearance = true },
                    new CrewMember { FullName = "Васильева Елена Игоревна", Role = "Стюардесса", EmployeeNumber = "5002", MedicalClearance = true },
                    new CrewMember { FullName = "Новикова Ольга Сергеевна", Role = "Стюардесса", EmployeeNumber = "5003", MedicalClearance = true }
                );
                context.SaveChanges();
            }

            if (!context.Flights.Any())
            {
                var f1 = new Flight
                {
                    FlightNumber = "SU-1002",
                    Aircraft = "Airbus A320",
                    Departure = "Калининград (KGD)",
                    Destination = "Москва (SVO)",
                    DepartureDate = DateTime.Today.AddDays(1).ToString("dd.MM.yyyy"),
                    DepartureTime = "14:30",
                    Status = "Готов к вылету"
                };
                var f2 = new Flight
                {
                    FlightNumber = "AFL-104",
                    Aircraft = "Boeing 737-800",
                    Departure = "Москва (SVO)",
                    Destination = "Санкт-Петербург (LED)",
                    DepartureDate = DateTime.Today.AddDays(2).ToString("dd.MM.yyyy"),
                    DepartureTime = "09:15",
                    Status = "Формируется"
                };
                var f3 = new Flight
                {
                    FlightNumber = "S7-2020",
                    Aircraft = "Embraer E170",
                    Departure = "Калининград (KGD)",
                    Destination = "Новосибирск (OVB)",
                    DepartureDate = DateTime.Today.AddDays(3).ToString("dd.MM.yyyy"),
                    DepartureTime = "18:45",
                    Status = "Формируется"
                };

                context.Flights.AddRange(f1, f2, f3);
                context.SaveChanges();

                // Assign full crew to SU-1002
                var staff = context.CrewMembers.ToList();
                context.FlightCrewAssignments.AddRange(
                    new FlightCrewAssignment { FlightId = f1.Id, CrewMemberId = staff[0].Id, AssignedRole = "1-й пилот" },
                    new FlightCrewAssignment { FlightId = f1.Id, CrewMemberId = staff[2].Id, AssignedRole = "2-й пилот" },
                    new FlightCrewAssignment { FlightId = f1.Id, CrewMemberId = staff[4].Id, AssignedRole = "Штурман" },
                    new FlightCrewAssignment { FlightId = f1.Id, CrewMemberId = staff[6].Id, AssignedRole = "Радист" },
                    new FlightCrewAssignment { FlightId = f1.Id, CrewMemberId = staff[7].Id, AssignedRole = "Стюардесса" },
                    new FlightCrewAssignment { FlightId = f1.Id, CrewMemberId = staff[8].Id, AssignedRole = "Стюардесса" }
                );
                context.SaveChanges();
            }
        }
    }
}

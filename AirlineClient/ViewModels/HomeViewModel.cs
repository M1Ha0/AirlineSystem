using AirlineClient.Models;
using AirlineClient.Services;
using AirlineClient.Utils;

namespace AirlineClient.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private int _totalFlights;
        public int TotalFlights { get => _totalFlights; set { _totalFlights = value; OnPropertyChanged(); } }

        private int _readyFlights;
        public int ReadyFlights { get => _readyFlights; set { _readyFlights = value; OnPropertyChanged(); } }

        private int _totalCrew;
        public int TotalCrew { get => _totalCrew; set { _totalCrew = value; OnPropertyChanged(); } }

        public string WelcomeTitle => $"Добро пожаловать, {SessionState.UserFullName}!";
        public string RoleInfo => $"Ваша роль в системе: {SessionState.UserRole}";

        public HomeViewModel()
        {
            _ = LoadStats();
        }

        private async Task LoadStats()
        {
            try
            {
                var flightSvc = new FlightService();
                var crewSvc = new CrewService();
                var flights = await flightSvc.GetAll();
                var crew = await crewSvc.GetAll();

                TotalFlights = flights.Count;
                ReadyFlights = flights.Count(f => f.Status == "Готов к вылету");
                TotalCrew = crew.Count;
            }
            catch { }
        }
    }
}

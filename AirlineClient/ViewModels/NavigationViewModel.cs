using AirlineClient.Models;
using AirlineClient.Utils;
using System.Windows.Input;

namespace AirlineClient.ViewModels
{
    public class NavigationViewModel : ViewModelBase
    {
        private object? _currentView;
        public object? CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public string UserDisplay => $"{SessionState.UserFullName} ({SessionState.UserRole})";

        public ICommand HomeCommand { get; }
        public ICommand FlightCommand { get; }
        public ICommand CrewCommand { get; }

        public NavigationViewModel()
        {
            HomeCommand = new RelayCommand(_ => CurrentView = new HomeViewModel());
            FlightCommand = new RelayCommand(_ => CurrentView = new FlightViewModel());
            CrewCommand = new RelayCommand(_ => CurrentView = new CrewViewModel());
            CurrentView = new HomeViewModel();
        }
    }
}

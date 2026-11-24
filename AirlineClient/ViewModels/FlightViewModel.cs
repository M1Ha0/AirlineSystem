using AirlineClient.Models;
using AirlineClient.Services;
using AirlineClient.Utils;
using AirlineClient.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace AirlineClient.ViewModels
{
    public class FlightViewModel : ViewModelBase
    {
        private readonly FlightService _flightService = new FlightService();

        public ObservableCollection<Flight> FlightList { get; } = new();

        private Flight? _selected;
        public Flight? Selected
        {
            get => _selected;
            set { _selected = value; OnPropertyChanged(); }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand FormCrewCommand { get; }
        public ICommand ManifestCommand { get; }
        public ICommand RefreshCommand { get; }

        public FlightViewModel()
        {
            AddCommand = new RelayCommand(async _ => await AddFlight());
            EditCommand = new RelayCommand(async obj => await EditFlight(obj as Flight));
            DeleteCommand = new RelayCommand(async obj => await DeleteFlight(obj as Flight));
            FormCrewCommand = new RelayCommand(async obj => await OpenCrewFormation(obj as Flight));
            ManifestCommand = new RelayCommand(obj => OpenManifest(obj as Flight));
            RefreshCommand = new RelayCommand(async _ => await LoadData());

            _ = LoadData();
        }

        public async Task LoadData()
        {
            try
            {
                var list = await _flightService.GetAll();
                FlightList.Clear();
                foreach (var f in list) FlightList.Add(f);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки рейсов: " + ex.Message);
            }
        }

        private async Task AddFlight()
        {
            var win = new AddEditFlightWindow(null);
            if (win.ShowDialog() == true && win.ResultFlight != null)
            {
                await _flightService.Add(win.ResultFlight);
                await LoadData();
            }
        }

        private async Task EditFlight(Flight? flight)
        {
            if (flight == null) { MessageBox.Show("Ошибка: сначала выберите рейс в таблице!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            var win = new AddEditFlightWindow(flight);
            if (win.ShowDialog() == true && win.ResultFlight != null)
            {
                await _flightService.Update(win.ResultFlight);
                await LoadData();
            }
        }

        private async Task DeleteFlight(Flight? flight)
        {
            if (flight == null) { MessageBox.Show("Ошибка: сначала выберите рейс для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            if (MessageBox.Show($"Удалить рейс {flight.FlightNumber}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                await _flightService.Delete(flight);
                await LoadData();
            }
        }

        private async Task OpenCrewFormation(Flight? flight)
        {
            if (flight == null) { MessageBox.Show("Ошибка: выберите рейс в таблице для формирования экипажа!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            var win = new FlightCrewWindow(flight);
            win.ShowDialog();
            await LoadData();
        }

        private void OpenManifest(Flight? flight)
        {
            if (flight == null) { MessageBox.Show("Ошибка: выберите рейс в таблице для просмотра ведомости!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            var win = new ManifestWindow(flight);
            win.ShowDialog();
        }
    }
}

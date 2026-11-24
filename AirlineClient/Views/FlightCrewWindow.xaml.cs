using AirlineClient.Models;
using AirlineClient.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AirlineClient.Views
{
    public partial class FlightCrewWindow : Window
    {
        private readonly FlightService _flightService = new FlightService();
        private readonly CrewService _crewService = new CrewService();
        private Flight _flight;
        private List<CrewMember> _allStaff = new();

        public FlightCrewWindow(Flight flight)
        {
            InitializeComponent();
            _flight = flight;

            CmbRole.ItemsSource = new[] { "1-й пилот", "2-й пилот", "Штурман", "Радист", "Стюардесса" };
            CmbRole.SelectedIndex = 0;

            _ = LoadInitialData();
        }

        private async Task LoadInitialData()
        {
            _allStaff = await _crewService.GetAll();
            var fresh = (await _flightService.GetAll()).FirstOrDefault(f => f.Id == _flight.Id);
            if (fresh != null) _flight = fresh;

            FilterStaffByRole();
            RefreshUi();
        }

        private void CmbRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterStaffByRole();
        }

        private void FilterStaffByRole()
        {
            if (CmbRole.SelectedItem == null) return;
            string role = CmbRole.SelectedItem.ToString()!;
            var filtered = _allStaff.Where(s => s.Role == role).ToList();
            CmbStaff.ItemsSource = filtered.Count > 0 ? filtered : _allStaff;
            if (CmbStaff.Items.Count > 0) CmbStaff.SelectedIndex = 0;
        }

        private void RefreshUi()
        {
            TxtHeader.Text = $"Экипаж рейса {_flight.FlightNumber}";
            TxtSubHeader.Text = $"{_flight.Departure} → {_flight.Destination} | {_flight.DepartureDate} {_flight.DepartureTime} | ВС: {_flight.Aircraft}";

            GridAssigned.ItemsSource = null;
            GridAssigned.ItemsSource = _flight.CrewAssignments;

            if (_flight.Status == "Готов к вылету")
            {
                TxtReadiness.Text = "✓ Экипаж полностью укомплектован и допущен. Статус рейса: «Готов к вылету».";
                TxtReadiness.Foreground = new SolidColorBrush(Color.FromRgb(5, 150, 105));
            }
            else
            {
                TxtReadiness.Text = $"Статус рейса: «{_flight.Status}». Требуются: 1-й пилот, 2-й пилот, штурман, радист и минимум 2 стюардессы с допуском ВЛЭК.";
                TxtReadiness.Foreground = new SolidColorBrush(Color.FromRgb(180, 83, 9));
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }

        private async void Assign_Click(object sender, RoutedEventArgs e)
        {
            if (CmbStaff.SelectedItem is not CrewMember selectedMember || CmbRole.SelectedItem == null)
            {
                MessageBox.Show("Ошибка: выберите должность и сотрудника из списка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_flight.Status == "Отменен")
            {
                MessageBox.Show("Ошибка: нельзя назначать экипаж на отмененный рейс!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!selectedMember.MedicalClearance)
            {
                MessageBox.Show($"Ошибка допуска: у сотрудника {selectedMember.FullName} отсутствует действующий медицинский допуск (ВЛЭК)! Назначение на рейс запрещено.", "Медицинский контроль", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string role = CmbRole.SelectedItem.ToString()!;
            if (role == "Стюардесса" && _flight.CrewAssignments.Any(a => a.CrewMemberId == selectedMember.Id))
            {
                MessageBox.Show($"Ошибка: сотрудник {selectedMember.FullName} уже назначен на этот рейс!", "Дублирование", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var updated = await _flightService.AssignCrewMember(_flight.Id, selectedMember.Id, role);
            if (updated != null)
            {
                _flight = updated;
                RefreshUi();
            }
            else
            {
                MessageBox.Show("Не удалось сохранить назначение на сервере.", "Ошибка API", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (GridAssigned.SelectedItem is not FlightCrewAssignment selected)
            {
                MessageBox.Show("Ошибка: выберите сотрудника в таблице экипажа для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var updated = await _flightService.RemoveAssignment(_flight.Id, selected.Id);
            if (updated != null)
            {
                _flight = updated;
                RefreshUi();
            }
        }

        private void Manifest_Click(object sender, RoutedEventArgs e)
        {
            var win = new ManifestWindow(_flight);
            win.ShowDialog();
        }

        private void Close_Click(object sender, RoutedEventArgs e) => Close();
    }
}

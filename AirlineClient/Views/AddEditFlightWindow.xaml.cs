using AirlineClient.Models;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AirlineClient.Views
{
    public partial class AddEditFlightWindow : Window
    {
        private readonly Flight? _original;
        public Flight? ResultFlight { get; private set; }

        public AddEditFlightWindow(Flight? flight)
        {
            InitializeComponent();
            _original = flight;

            CmbStatus.ItemsSource = new[] { "Формируется", "Готов к вылету", "Отменен" };

            if (flight != null)
            {
                TitleText.Text = $"Редактирование рейса {flight.FlightNumber}";
                TxtNumber.Text = flight.FlightNumber;
                TxtAircraft.Text = flight.Aircraft;
                TxtDeparture.Text = flight.Departure;
                TxtDestination.Text = flight.Destination;
                TxtTime.Text = flight.DepartureTime;
                CmbStatus.SelectedItem = flight.Status;

                if (DateTime.TryParseExact(flight.DepartureDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                    DpDate.SelectedDate = dt;
                else
                    DpDate.SelectedDate = DateTime.Today;
            }
            else
            {
                DpDate.SelectedDate = DateTime.Today.AddDays(1);
                CmbStatus.SelectedIndex = 0;
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }

        private void ShowError(string message)
        {
            if (TxtError == null) return;
            TxtError.Text = message;
            TxtError.Visibility = Visibility.Visible;
        }

        private void ClearError()
        {
            if (TxtError == null) return;
            TxtError.Text = string.Empty;
            TxtError.Visibility = Visibility.Collapsed;
        }

        private void Input_TextChanged(object sender, TextChangedEventArgs e) => ClearError();

        private void TxtTime_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, @"^[0-9:]+$"))
            {
                e.Handled = true;
                ShowError("Ошибка ввода: во времени разрешены только цифры и двоеточие (ЧЧ:ММ)!");
            }
            else
            {
                ClearError();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNumber.Text))
            {
                ShowError("Ошибка: введите номер рейса!");
                TxtNumber.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtAircraft.Text))
            {
                ShowError("Ошибка: укажите тип воздушного судна!");
                TxtAircraft.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtDeparture.Text) || string.IsNullOrWhiteSpace(TxtDestination.Text))
            {
                ShowError("Ошибка: укажите пункты отправления и назначения!");
                return;
            }

            if (string.Equals(TxtDeparture.Text.Trim(), TxtDestination.Text.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                ShowError("Ошибка: пункт отправления и пункт назначения не могут совпадать!");
                return;
            }

            if (DpDate.SelectedDate == null)
            {
                ShowError("Ошибка: выберите дату вылета в календаре!");
                return;
            }

            if (!Regex.IsMatch(TxtTime.Text.Trim(), @"^([01]?[0-9]|2[0-3]):[0-5][0-9]$"))
            {
                ShowError("Ошибка: введите корректное время в формате ЧЧ:ММ (например, 14:30)!");
                TxtTime.Focus();
                return;
            }

            ResultFlight = new Flight
            {
                Id = _original?.Id ?? 0,
                FlightNumber = TxtNumber.Text.Trim(),
                Aircraft = TxtAircraft.Text.Trim(),
                Departure = TxtDeparture.Text.Trim(),
                Destination = TxtDestination.Text.Trim(),
                DepartureDate = DpDate.SelectedDate.Value.ToString("dd.MM.yyyy"),
                DepartureTime = TxtTime.Text.Trim(),
                Status = CmbStatus.SelectedItem?.ToString() ?? "Формируется"
            };

            DialogResult = true;
            Close();
        }
    }
}

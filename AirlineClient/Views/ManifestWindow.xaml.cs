using AirlineClient.Models;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace AirlineClient.Views
{
    public partial class ManifestWindow : Window
    {
        public ManifestWindow(Flight flight)
        {
            InitializeComponent();

            var sb = new StringBuilder();
            sb.AppendLine("==========================================================");
            sb.AppendLine("               ПОЛЕТНАЯ ВЕДОМОСТЬ АВИАРЕЙСА               ");
            sb.AppendLine("==========================================================");
            sb.AppendLine($"РЕЙС:        {flight.FlightNumber}");
            sb.AppendLine($"МАРШРУТ:     {flight.Departure} -> {flight.Destination}");
            sb.AppendLine($"ДАТА/ВРЕМЯ:  {flight.DepartureDate} {flight.DepartureTime}");
            sb.AppendLine($"ВОЗД. СУДНО: {flight.Aircraft}");
            sb.AppendLine($"СТАТУС:      {flight.Status}");
            sb.AppendLine("----------------------------------------------------------");
            sb.AppendLine("СОСТАВ ЛЕТНОЙ БРИГАДЫ:");

            if (flight.CrewAssignments.Count == 0)
            {
                sb.AppendLine("  (Экипаж не назначен)");
            }
            else
            {
                int i = 1;
                foreach (var a in flight.CrewAssignments)
                {
                    string name = a.CrewMember?.FullName ?? "Неизвестно";
                    string num = a.CrewMember?.EmployeeNumber ?? "-";
                    string med = (a.CrewMember?.MedicalClearance == true) ? "Допущен" : "НЕТ ДОПУСКА";
                    sb.AppendLine($"  {i++}. {a.AssignedRole,-12} | {name,-26} | №{num} | {med}");
                }
            }

            sb.AppendLine("----------------------------------------------------------");
            sb.AppendLine($"Сформировал: {SessionState.UserFullName} ({SessionState.UserRole})");
            sb.AppendLine($"Дата печати: {DateTime.Now:dd.MM.yyyy HH:mm}");

            TxtManifest.Text = sb.ToString();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e) => Close();
    }
}

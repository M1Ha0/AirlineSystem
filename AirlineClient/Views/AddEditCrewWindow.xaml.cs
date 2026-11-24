using AirlineClient.Models;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AirlineClient.Views
{
    public partial class AddEditCrewWindow : Window
    {
        private readonly CrewMember? _original;
        public CrewMember? ResultMember { get; private set; }

        public AddEditCrewWindow(CrewMember? member)
        {
            InitializeComponent();
            _original = member;

            CmbRole.ItemsSource = new[] { "1-й пилот", "2-й пилот", "Штурман", "Радист", "Стюардесса" };

            if (member != null)
            {
                TitleText.Text = "Редактирование сотрудника";
                TxtFullName.Text = member.FullName;
                CmbRole.SelectedItem = member.Role;
                TxtEmpNumber.Text = member.EmployeeNumber;
                ChkMedical.IsChecked = member.MedicalClearance;
            }
            else
            {
                CmbRole.SelectedIndex = 0;
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }

        private void ShowError(string message)
        {
            TxtError.Text = message;
            TxtError.Visibility = Visibility.Visible;
        }

        private void ClearError()
        {
            TxtError.Text = string.Empty;
            TxtError.Visibility = Visibility.Collapsed;
        }

        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            ClearError();
        }

        private void TxtEmpNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, @"^[0-9]+$"))
            {
                e.Handled = true;
                ShowError("Ошибка ввода: в табельный номер разрешено вводить только цифры!");
            }
            else
            {
                ClearError();
            }
        }

        private void TxtEmpNumber_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
                ShowError("Ошибка ввода: пробел в табельном номере запрещен!");
            }
        }

        private void TxtEmpNumber_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                string text = (string)e.DataObject.GetData(DataFormats.Text);
                if (!Regex.IsMatch(text, @"^[0-9]+$"))
                {
                    e.CancelCommand();
                    ShowError("Ошибка вставки: табельный номер должен состоять только из цифр!");
                }
            }
            else
            {
                e.CancelCommand();
                ShowError("Ошибка вставки: недопустимый формат данных!");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtFullName.Text))
            {
                ShowError("Ошибка: введите ФИО сотрудника!");
                TxtFullName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtEmpNumber.Text) || !Regex.IsMatch(TxtEmpNumber.Text.Trim(), @"^[0-9]+$"))
            {
                ShowError("Ошибка: введите корректный числовой табельный номер!");
                TxtEmpNumber.Focus();
                return;
            }

            ResultMember = new CrewMember
            {
                Id = _original?.Id ?? 0,
                FullName = TxtFullName.Text.Trim(),
                Role = CmbRole.SelectedItem?.ToString() ?? "1-й пилот",
                EmployeeNumber = TxtEmpNumber.Text.Trim(),
                MedicalClearance = ChkMedical.IsChecked == true
            };

            DialogResult = true;
            Close();
        }
    }
}

using AirlineClient.Models;
using AirlineClient.Services;
using AirlineClient.Utils;
using AirlineClient.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace AirlineClient.ViewModels
{
    public class CrewViewModel : ViewModelBase
    {
        private readonly CrewService _crewService = new CrewService();

        public ObservableCollection<CrewMember> CrewList { get; } = new();

        private CrewMember? _selected;
        public CrewMember? Selected
        {
            get => _selected;
            set { _selected = value; OnPropertyChanged(); }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand RefreshCommand { get; }

        public CrewViewModel()
        {
            AddCommand = new RelayCommand(async _ => await AddCrew());
            EditCommand = new RelayCommand(async obj => await EditCrew(obj as CrewMember));
            DeleteCommand = new RelayCommand(async obj => await DeleteCrew(obj as CrewMember));
            RefreshCommand = new RelayCommand(async _ => await LoadData());

            _ = LoadData();
        }

        public async Task LoadData()
        {
            try
            {
                var list = await _crewService.GetAll();
                CrewList.Clear();
                foreach (var c in list) CrewList.Add(c);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки персонала: " + ex.Message);
            }
        }

        private async Task AddCrew()
        {
            var win = new AddEditCrewWindow(null);
            if (win.ShowDialog() == true && win.ResultMember != null)
            {
                await _crewService.Add(win.ResultMember);
                await LoadData();
            }
        }

        private async Task EditCrew(CrewMember? member)
        {
            if (member == null) { MessageBox.Show("Ошибка: выберите сотрудника в таблице для редактирования!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            var win = new AddEditCrewWindow(member);
            if (win.ShowDialog() == true && win.ResultMember != null)
            {
                await _crewService.Update(win.ResultMember);
                await LoadData();
            }
        }

        private async Task DeleteCrew(CrewMember? member)
        {
            if (member == null) { MessageBox.Show("Ошибка: выберите сотрудника в таблице для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            if (MessageBox.Show($"Удалить сотрудника {member.FullName}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                await _crewService.Delete(member);
                await LoadData();
            }
        }
    }
}

using AirlineClient.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace AirlineClient.Views
{
    public partial class CrewView : UserControl
    {
        public CrewView() => InitializeComponent();

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is CrewViewModel vm && vm.Selected != null)
            {
                vm.EditCommand.Execute(vm.Selected);
            }
        }
    }
}

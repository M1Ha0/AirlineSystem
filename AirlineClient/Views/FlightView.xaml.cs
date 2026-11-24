using AirlineClient.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace AirlineClient.Views
{
    public partial class FlightView : UserControl
    {
        public FlightView() => InitializeComponent();

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is FlightViewModel vm && vm.Selected != null)
            {
                vm.EditCommand.Execute(vm.Selected);
            }
        }
    }
}

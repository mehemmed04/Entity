using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Entity
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<City> Cities{ get; set; }
        public ObservableCollection<Schedule> Schedules{ get; set; }
        public ObservableCollection<Airplane> Airplanes{ get; set; }
        public MainWindow()
        {
            InitializeComponent();

            using(var context = new AirTransportDBEntities1())
            {
                var cities = context.Cities;
                Cities = new ObservableCollection<City>(cities);
                CitiesCmb.ItemsSource = Cities;
            }


        }

        private void BuyBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CitiesCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = CitiesCmb.SelectedItem as City;
            using(var context = new AirTransportDBEntities1())
            {
                var schedules = context.Schedules
                                .Where(s => s.Id == item.Id);
                Schedules = new ObservableCollection<Schedule>(schedules);
            }
            SchedulesCmb.ItemsSource = Schedules;
        }

        private void SchedulesCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AirplanesCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

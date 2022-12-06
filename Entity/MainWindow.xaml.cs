using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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
        public ObservableCollection<City> Cities { get; set; }
        public ObservableCollection<Schedule> Schedules { get; set; }
        public ObservableCollection<Airplane> Airplanes { get; set; }
        public Pilot Pilot { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            using (var context = new AirTransportDBEntities2())
            {
                var cities = context.Cities;
                Cities = new ObservableCollection<City>(cities);
                CitiesCmb.ItemsSource = Cities;
            }
        }

        private void BuyBtn_Click(object sender, RoutedEventArgs e)
        {
            var city = CitiesCmb.SelectedItem as City;
            var schedule = SchedulesCmb.SelectedItem as Schedule;
            var airplane = AirplanesCmb.SelectedItem as Airplane;

            int typeId;
            if ((bool)EconomRdBtn.IsChecked)
            {
                typeId = 1;
            }
            else if ((bool)BusinessRdBtn.IsChecked)
            {
                typeId = 2;
            }
            else if ((bool)PremiumRdBtn.IsChecked)
            {
                typeId = 3;
            }
            else
            {
                System.Windows.MessageBox.Show("Business Type not selected");
                return;
            }
            var ticket = new Ticket
            {
                CityId = city.Id,
                ScheduleId = schedule.Id,
                FlightTypeId = typeId
            };
            dynamic flightType;
            using (var context = new AirTransportDBEntities2())
            {
                flightType = context.FlightTypes
                                .FirstOrDefault(s => s.Id == typeId);

                context.Entry(ticket).State = EntityState.Added;
            }
            string t = $@"~~~~~~~Ticket~~~~~~~
Ticket Id : {ticket.Id}
City : {city.Name}
Schedule : {schedule.StartDateTime}
FlightType : {flightType.Type}

Thanks For Buying";
            System.Windows.MessageBox.Show(t);
            this.Close();
        }

        private void CitiesCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = CitiesCmb.SelectedItem as City;
            using (var context = new AirTransportDBEntities2())
            {
                var schedules = context.Schedules
                                .Where(s => s.Id == item.Id).ToList();
                Schedules = new ObservableCollection<Schedule>(schedules);
            }
            SchedulesCmb.ItemsSource = Schedules;
        }

        private void SchedulesCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = SchedulesCmb.SelectedItem as Schedule;
            using (var context = new AirTransportDBEntities2())
            {
                var airplanes = context.Airplanes.Where(a => a.Id == item.Id);

                Airplanes = new ObservableCollection<Airplane>(airplanes);

            }
            AirplanesCmb.ItemsSource = Airplanes;

        }

        private void AirplanesCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = AirplanesCmb.SelectedItem as Airplane;

            using (var context = new AirTransportDBEntities2())
            {
                var pilot = context.Pilots.FirstOrDefault(p => p.Id == item.Id);
                Pilot = pilot;
            }
            if (Pilot != null)
            {
                PilotNameTxb.Text = Pilot.Name;
                PilotSurnameTxb.Text = Pilot.Surname;
            }
            else
            {
                System.Windows.MessageBox.Show("There is not such flight");
            }
        }
    }
}

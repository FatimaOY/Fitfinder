using System;
using System.Collections.Generic;
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

namespace Fitfinder
{
    /// <summary>
    /// Interaction logic for TrainerMainPage.xaml
    /// </summary>
    public partial class TrainerMainPage : Page
    {
        public TrainerMainPage()
        {
            InitializeComponent();
        }

        private void TrainerProfile_click(object sender, RoutedEventArgs e)
        {
            TrainerProfile trainerProfile = new TrainerProfile();
            this.NavigationService.Navigate(trainerProfile);
        }

        private void TrainerCalander_Button(object sender, RoutedEventArgs e)
        {
            CalendarTrainer calendarTrainer = new CalendarTrainer();
            this.NavigationService.Navigate(calendarTrainer);
        }
    }
}

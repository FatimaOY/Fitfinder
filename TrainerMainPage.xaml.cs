﻿using System;
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
        private int userId;
        private int trainerId;
        public TrainerMainPage(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            Data data = new Data();
            int trainerId = data.GetTrainerID(userId);
        }

        private void TrainerProfile_click(object sender, RoutedEventArgs e)
        {
            TrainerProfile trainerProfile = new TrainerProfile();
            this.NavigationService.Navigate(trainerProfile);
        }

        private void TrainerCalander_Button(object sender, RoutedEventArgs e)
        {
            // Show the week selection window
            WeekSelectionWindow weekSelection = new WeekSelectionWindow();
            if (weekSelection.ShowDialog() == true)
            {
                // Get the selected week number from the WeekSelection instance
                int selectedWeek = weekSelection.SelectedWeek;

                // Pass the selected week number to the CalendarTrainer constructor
                CalendarTrainer calendarTrainer = new CalendarTrainer(selectedWeek);
                this.NavigationService.Navigate(calendarTrainer);
            }
        }
        private void AppointmentRequests_Button(object sender, RoutedEventArgs e)
        {
            Request request = new Request(trainerId);
            this.NavigationService.Navigate(request);
        }

        private void YourWorkouts_button(object sender, RoutedEventArgs e)
        {
            YourWorkoutsTrainer yourworkoutsTrainer = new YourWorkoutsTrainer();
            this.NavigationService.Navigate(yourworkoutsTrainer);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using static Fitfinder.BrowseTrainers;

namespace Fitfinder
{
    public partial class TrainerDetails : Page
    {
        public TrainerDetails(TrainerBrowse trainer)
        {
            InitializeComponent();
            DisplayTrainerDetails(trainer);
        }

        private void DisplayTrainerDetails(TrainerBrowse trainer)
        {
            NameText.Text = $"Name: {trainer.Name}";
            SurnameText.Text = $"Surname: {trainer.Surname}";
            EmailText.Text = $"Email: {trainer.Email}";
            LocationText.Text = $"Location: {trainer.Location}";
            DescriptionText.Text = $"Description: {trainer.Description}";
            PriceText.Text = $"Price: {trainer.Price:C}";
            GenderText.Text = $"Gender: {trainer.GenderName}";

            WorkoutTypesList.Items.Clear();
            foreach (var workoutType in trainer.WorkoutTypes)
            {
                WorkoutTypesList.Items.Add(new TextBlock { Text = workoutType });
            }

            // Load and display the profile picture if available
            // Example: ProfilePicture.Source = new BitmapImage(new Uri("path/to/profile/picture.jpg"));
        }

        private void Back_button1(object sender, System.Windows.RoutedEventArgs e)
        {
            BrowseTrainers browseTrainers = new BrowseTrainers();
            this.NavigationService.Navigate(browseTrainers);
        }
        private void ScheduleWorkout_button(object sender, RoutedEventArgs e)
        {
            // Handle the Schedule a Workout button click event
        }

        private void Message_button(object sender, RoutedEventArgs e)
        {
            // Handle the Message button click event
        }

    }
}

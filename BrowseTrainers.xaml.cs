﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace Fitfinder
{
    public partial class BrowseTrainers : Page
    {
        private TrainerProfile _trainerProfile;
        private List<WorkoutType> workoutTypes;

        public BrowseTrainers()
        {
            InitializeComponent();
            _trainerProfile = new TrainerProfile();
            genderComboBox.ItemsSource = new List<string> { "Male", "Female", "Other" };
            workoutTypes = new List<WorkoutType>
            {
                new WorkoutType(1, "Weightlifting"),
                new WorkoutType(2, "Cardio"),
                new WorkoutType(3, "Stretching"),
                new WorkoutType(4, "Yoga"),
                new WorkoutType(5, "Pilates"),
                new WorkoutType(6, "CrossFit"),
                new WorkoutType(7, "Calisthenics"),
                new WorkoutType(8, "Swimming")
            };

            workoutTypeComboBox.ItemsSource = workoutTypes;
            workoutTypeComboBox.DisplayMemberPath = "Name";
            List<TrainerBrowse> trainers = GetTrainersFromDatabase();
            TrainersListBox.ItemsSource = trainers;
        }

        private void TrainersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TrainersListBox.SelectedItem is TrainerBrowse selectedTrainer)
            {
                TrainerDetails trainerDetailsPage = new TrainerDetails(selectedTrainer);
                if (this.NavigationService != null)
                {
                    this.NavigationService.Navigate(trainerDetailsPage);
                }
                else
                {
                    MessageBox.Show("Navigation service is not available.");
                }
            }
        }

        private void GenderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedGender = genderComboBox.SelectedItem as string;
            if (selectedGender != null)
            {
                MessageBox.Show($"Selected gender: {selectedGender}");
            }
        }

        private void WorkoutTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WorkoutType selectedWorkoutType = workoutTypeComboBox.SelectedItem as WorkoutType;
            if (selectedWorkoutType != null)
            {
                MessageBox.Show($"Selected workout type: {selectedWorkoutType.Name}");
            }
        }

        public class TrainerBrowse
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Email { get; set; }
            public string Location { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public List<string> WorkoutTypes { get; set; }
        }

        public List<TrainerBrowse> GetTrainersFromDatabase()
        {
            Data data = new Data();
            string connectionString = "datasource=127.0.0.1;" +
                                      "port=3306;" +
                                      "username=root;" +
                                      "password=;" +
                                      "database=fitfinder4";

            List<TrainerBrowse> trainers = new List<TrainerBrowse>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sqlQuery = "SELECT TrainerId, PersonId, Location, Description, Price FROM Trainer";

                using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int userId = Convert.ToInt32(reader["PersonId"]);
                        int trainerId = Convert.ToInt32(reader["TrainerId"]);

                        List<string> stylestList = _trainerProfile.UpdateTrainerStyles(trainerId);
                        var userInfo = data.GetUserInformationById(userId);
                        TrainerBrowse trainerBrowse = new TrainerBrowse
                        {
                            Name = userInfo.Name,
                            Surname = userInfo.Surname,
                            Email = userInfo.Email,
                            Location = reader["Location"].ToString(),
                            Description = reader["Description"].ToString(),
                            Price = Convert.ToDecimal(reader["Price"]),
                            WorkoutTypes = stylestList
                        };

                        trainers.Add(trainerBrowse);
                    }

                    reader.Close();
                }
            }

            return trainers;
        }

        private void Profile_button(object sender, RoutedEventArgs e)
        {
            YourProfil yourProfil = new YourProfil();
            this.NavigationService.Navigate(yourProfil);
        }

        private void BrowseTrainer_button(object sender, RoutedEventArgs e)
        {
            BrowseTrainers browseTrainers = new BrowseTrainers();
            this.NavigationService.Navigate(browseTrainers);
        }

        private void YourWorkouts_button(object sender, RoutedEventArgs e)
        {
            YourWorkouts yourWorkouts = new YourWorkouts();
            this.NavigationService.Navigate(yourWorkouts);
        }

        private void Messages_button(object sender, RoutedEventArgs e)
        {
            // Handle messages button click
        }
    }
}

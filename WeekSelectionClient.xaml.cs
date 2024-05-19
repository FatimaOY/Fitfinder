using System;
using System.Windows;
using System.Windows.Controls;

namespace Fitfinder
{
    public partial class WeekSelectionClient : Window
    {
        public int SelectedWeek { get; private set; }

        public WeekSelectionClient()
        {
            InitializeComponent();

            // Populate the ComboBox with week numbers
            for (int i = 1; i <= 52; i++)
            {
                weekComboBox.Items.Add(i);
            }
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected week number
            if (weekComboBox.SelectedItem != null && int.TryParse(weekComboBox.SelectedItem.ToString(), out int selectedWeek))
            {
                SelectedWeek = selectedWeek;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please select a week number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

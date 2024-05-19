using System.Windows;

namespace Fitfinder
{
    public partial class WeekSelectionWindow : Window
    {
        public int SelectedWeek { get; private set; }

        public WeekSelectionWindow()
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
    }
}
